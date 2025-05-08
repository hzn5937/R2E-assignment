import React, { useEffect, useState } from 'react';
import { Table, Tag, Button, Spin, Alert, Space, Modal, message, Card, Tooltip } from 'antd';
import { RollbackOutlined, DownOutlined } from '@ant-design/icons';
import axiosInstance from '../../../utils/axiosConfig';
import { useAuth } from '../../../context/AuthContext';
import PaginationControls from '../../../components/pagination';

export default function MyBorrowing() {
  const [requests, setRequests] = useState([]);
  const [loading, setLoading] = useState(false);
  const [returningRequestId, setReturningRequestId] = useState(null);
  const [isReturnModalVisible, setIsReturnModalVisible] = useState(false);
  const [returning, setReturning] = useState(false);
  const [expandedRequestDetails, setExpandedRequestDetails] = useState({});
  const [loadingDetails, setLoadingDetails] = useState({});
  const [pagination, setPagination] = useState({
    current: 1,
    pageSize: 5,
    total: 0,
    totalPages: 0,
    hasNext: false,
    hasPrev: false
  });
  
  const { user } = useAuth();
  
  const fetchRequests = (pageNum = 1, pageSize = 5) => {
    setLoading(true);
    
    // Make sure user exists and has an id
    if (!user || !user.id) {
      console.error('User ID not available');
      setLoading(false);
      return;
    }
    
    axiosInstance.get(`/api/requests?userId=${user.id}&pageNum=${pageNum}&pageSize=${pageSize}`)
      .then(res => {
        if (res.data.items) {
          // Paginated response
          setRequests(res.data.items);
          setPagination({
            current: res.data.pageNum,
            pageSize: res.data.pageSize,
            total: res.data.totalCount,
            totalPages: res.data.totalPage,
            hasNext: res.data.hasNext,
            hasPrev: res.data.hasPrev
          });
        } else {
          // Non-paginated response
          setRequests(res.data);
        }
        setLoading(false);
      })
      .catch(err => {
        console.error('Failed to fetch requests:', err);
        setLoading(false);
      });
  };

  useEffect(() => {
    fetchRequests();
  }, []);

  const handlePageChange = (page, pageSize) => {
    fetchRequests(page, pageSize);
  };
  
  const handlePageSizeChange = (newPageSize) => {
    fetchRequests(1, newPageSize);
  };

  const handleReturn = (requestId) => {
    setReturningRequestId(requestId);
    setIsReturnModalVisible(true);
  };

  const confirmReturn = async () => {
    setReturning(true);
    try {
      const payload = {
        requestId: returningRequestId,
        processedById: user.id // Include the user's ID so the backend can verify they're authorized
      };

      await axiosInstance.post(`/api/requests/${returningRequestId}/return`, payload);
      message.success('Books returned successfully');
      
      // Update the local state to reflect the status change
      setExpandedRequestDetails(prev => {
        const updatedDetails = { ...prev };
        if (updatedDetails[returningRequestId]) {
          updatedDetails[returningRequestId] = {
            ...updatedDetails[returningRequestId],
            status: 'Returned'
          };
        }
        return updatedDetails;
      });
      
      // Update the main requests list to reflect the status change
      setRequests(prev => 
        prev.map(request => 
          request.id === returningRequestId 
            ? { ...request, status: 'Returned' } 
            : request
        )
      );
      
      setIsReturnModalVisible(false);
      // We still fetch from the server to ensure everything is in sync
      fetchRequests(pagination.current, pagination.pageSize);
    } catch (err) {
      console.error('Failed to return books:', err);
      // Extract error message from API response if available
      const errorMessage = err.response?.data?.message || 'Failed to return books';
      message.error(errorMessage);
    } finally {
      setReturning(false);
    }
  };

  // Function to fetch request details when a row is expanded
  const fetchRequestDetails = async (requestId) => {
    setLoadingDetails(prev => ({ ...prev, [requestId]: true }));
    
    try {
      const response = await axiosInstance.get(`/api/requests/${requestId}/details`);
      // Store the details in state
      setExpandedRequestDetails(prev => ({
        ...prev,
        [requestId]: response.data
      }));
    } catch (err) {
      console.error(`Failed to fetch details for request ${requestId}:`, err);
      message.error('Failed to load request details');
    } finally {
      setLoadingDetails(prev => ({ ...prev, [requestId]: false }));
    }
  };

  // Expandable row render function
  const expandedRowRender = (record) => {
    const requestId = record.id;
    const details = expandedRequestDetails[requestId];
    
    // If we don't have details yet, fetch them
    if (!details && !loadingDetails[requestId]) {
      fetchRequestDetails(requestId);
    }
    
    // Columns for the books table in expanded row
    const bookColumns = [
      { title: 'Title', dataIndex: 'title', key: 'title' },
      { title: 'Author', dataIndex: 'author', key: 'author' },
      { 
        title: 'Category', 
        dataIndex: 'categoryName',
        key: 'categoryName',
        render: category => <Tag color="blue">{category}</Tag>
      }
    ];
    
    return (
      <div className="px-4 py-2">
        {loadingDetails[requestId] ? (
          <div className="flex justify-center items-center py-4">
            <Spin size="default" />
          </div>
        ) : details ? (
          <Card title="Request Details" bordered={false}>
            <div className="mb-4">
              <h4 className="font-semibold mb-2">Books in this request:</h4>
              {!details.books?.length ? (
                <Alert 
                  message="No books in this request."
                  type="info"
                  showIcon
                />
              ) : (
                <Table 
                  columns={bookColumns} 
                  dataSource={details.books} 
                  pagination={false} 
                  rowKey={record => `${record.title}-${record.author}`}
                  size="small"
                />
              )}
            </div>
            {details.status === 'Approved' && (
              <div className="flex justify-end">
                <Button
                  type="primary"
                  ghost
                  icon={<RollbackOutlined />}
                  onClick={() => handleReturn(requestId)}
                >
                  Return All
                </Button>
              </div>
            )}
          </Card>
        ) : (
          <Alert 
            message="Failed to load request details."
            type="error"
            showIcon
          />
        )}
      </div>
    );
  };

  const columns = [
    // Add a new column with an expand indicator
    { 
      title: '', 
      key: 'expand',
      width: 40,
      render: () => (
        <Tooltip title="Click row to view details">
          <DownOutlined style={{ color: '#1890ff' }} />
        </Tooltip>
      ) 
    },
    { title: 'ID', dataIndex: 'id', key: 'id' },
    { 
      title: 'Date Requested', 
      dataIndex: 'requestedDate', 
      key: 'requestedDate',
      render: text => new Date(text).toLocaleString()
    },
    {
      title: 'Status',
      dataIndex: 'status',
      key: 'status',
      render: status => {
        let color = 'default';
        if (status === 'Approved') color = 'green';
        else if (status === 'Rejected') color = 'red';
        else if (status === 'Pending') color = 'gold';
        else if (status === 'Returned') color = 'blue';

        return <Tag color={color}>{status.toUpperCase()}</Tag>;
      }
    },
    { title: 'Approver', dataIndex: 'approver', key: 'approver', render: text => text || 'N/A' }
  ];

  return (
    <div className="p-6">
      <h1 className="text-2xl font-bold mb-4">📚 My Borrowing</h1>
      
      {loading && !requests.length ? (
        <div className="flex justify-center items-center h-64">
          <Spin size="large" />
        </div>
      ) : (
        <>
          {!requests.length > 0 ? (
            <Alert 
              message="No borrowing requests found."
              type="info"
              showIcon
              className="mb-4"
            />
          ) : (
            <>
              <Table
                dataSource={requests}
                columns={columns}
                rowKey="id"
                pagination={false}
                bordered
                loading={loading}
                expandable={{
                  expandedRowRender,
                  expandRowByClick: true,
                  expandIconColumnIndex: -1, // Hide the expand icon column
                }}
              />
              
              <div className="mt-4">
                <PaginationControls 
                  pagination={pagination}
                  onPageChange={handlePageChange}
                  onPageSizeChange={handlePageSizeChange}
                  itemName="requests"
                />
              </div>
            </>
          )}
        </>
      )}

      <Modal
        title="Return Books"
        open={isReturnModalVisible}
        onOk={confirmReturn}
        onCancel={() => setIsReturnModalVisible(false)}
        confirmLoading={returning}
      >
        <p>Are you sure you want to return the books?</p>
      </Modal>
    </div>
  );
}