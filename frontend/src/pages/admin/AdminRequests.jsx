import React, { useState, useEffect } from 'react';
import { Tabs, Table, Tag, Button, Spin, Alert, Space, Modal } from 'antd';
import { CheckCircleOutlined, CloseCircleOutlined } from '@ant-design/icons';
import axiosInstance from '../../utils/axiosConfig';
import PaginationControls from '../../components/pagination';
import { useAuth } from '../../context/AuthContext';

const { TabPane } = Tabs;

const AdminRequests = () => {
  const [activeTab, setActiveTab] = useState('all');
  const [requests, setRequests] = useState([]);
  const [loading, setLoading] = useState(false);
  const [processingRequestId, setProcessingRequestId] = useState(null);
  const [isApproveModalVisible, setIsApproveModalVisible] = useState(false);
  const [isRejectModalVisible, setIsRejectModalVisible] = useState(false);
  const [requestToAction, setRequestToAction] = useState(null);
  const [pagination, setPagination] = useState({
    current: 1,
    pageSize: 5,
    total: 0,
    totalPages: 0,
    hasNext: false,
    hasPrev: false
  });
  const [apiError, setApiError] = useState(null);
  const [apiSuccess, setApiSuccess] = useState(null); 
  const { user } = useAuth();

  const fetchRequests = (status = '', pageNum = 1, pageSize = 5) => {
    setLoading(true);
    setApiError(null);
    
    let url = `/api/requests/all?pageNum=${pageNum}&pageSize=${pageSize}`;
    if (status && status !== 'all') {
      url += `&status=${status}`;
    }
    
    axiosInstance.get(url)
      .then(res => {
        if (res.data.items) {
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
          setRequests([]); 
        }
      })
      .catch(err => {
        console.error('Failed to fetch requests:', err);
        setApiError({
          type: 'error',
          message: 'Failed to load requests',
          description: err.response?.data?.message || 'There was an error loading the requests. Please try again.'
        });
        setRequests([]);
      })
      .finally(() => {
        setLoading(false);
      });
  };

  useEffect(() => {
    // Initial load - fetch all requests
    fetchRequests(activeTab);
  }, []);

  const handleTabChange = (key) => {
    setActiveTab(key);
    fetchRequests(key);
  };

  const handlePageChange = (page, pageSize) => {
    fetchRequests(activeTab, page, pageSize);
  };
  
  const handlePageSizeChange = (newPageSize) => {
    fetchRequests(activeTab, 1, newPageSize);
  };

  const getStatusTag = (status) => {
    let color = 'default';
    if (status === 'Approved') color = 'green';
    else if (status === 'Rejected') color = 'red';
    else if (status === 'Waiting' || status === 'Pending') color = 'gold';
    else if (status === 'Returned') color = 'blue';

    return <Tag color={color}>{status.toUpperCase()}</Tag>;
  };

  const handleApproveRequest = (requestId) => {
    setRequestToAction(requestId);
    setIsApproveModalVisible(true);
  };

  const handleRejectRequest = (requestId) => {
    setRequestToAction(requestId);
    setIsRejectModalVisible(true);
  };

  const confirmApprove = async () => {
    if (!requestToAction) return;
    
    setProcessingRequestId(requestToAction);
    try {
      await axiosInstance.put(`/api/requests/${requestToAction}`, {
        adminId: user.id,
        status: "Approved"
      });
      
      // Get the request details for success message
      const approvedRequest = requests.find(req => req.id === requestToAction);
      setApiSuccess({
        message: 'Request Approved Successfully',
        description: `Request #${requestToAction} from ${approvedRequest?.requestor || 'user'} has been approved.`
      });
      fetchRequests(activeTab, pagination.current, pagination.pageSize);
      setIsApproveModalVisible(false);
    } catch (err) {
      console.error('Failed to approve request:', err);
      setApiError({
        type: 'error',
        message: 'Failed to Approve Request',
        description: err.response?.data?.message || 'An error occurred while approving the request. Please try again.'
      });
    } finally {
      setProcessingRequestId(null);
      setRequestToAction(null);
    }
  };

  const confirmReject = async () => {
    if (!requestToAction) return;
    
    setProcessingRequestId(requestToAction);
    try {
      await axiosInstance.put(`/api/requests/${requestToAction}`, {
        adminId: user.id,
        status: "Rejected"
      });
      
      // Get the request details for success message
      const rejectedRequest = requests.find(req => req.id === requestToAction);
      setApiSuccess({
        message: 'Request Rejected Successfully',
        description: `Request #${requestToAction} from ${rejectedRequest?.requestor || 'user'} has been rejected.`
      });
      fetchRequests(activeTab, pagination.current, pagination.pageSize);
      setIsRejectModalVisible(false);
    } catch (err) {
      console.error('Failed to reject request:', err);
      setApiError({
        type: 'error',
        message: 'Failed to Reject Request',
        description: err.response?.data?.message || 'An error occurred while rejecting the request. Please try again.'
      });
    } finally {
      setProcessingRequestId(null);
      setRequestToAction(null);
    }
  };

  // Expandable row to show book details
  const expandedRowRender = (record) => {
    const columns = [
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
        <h4 className="font-semibold mb-2">Books in this request:</h4>
        {!record.books?.length ? (
          <Alert 
            message="No books in this request."
            type="info"
            showIcon
          />
        ) : (
          <Table 
            columns={columns} 
            dataSource={record.books} 
            pagination={false} 
            rowKey={record => `${record.title}-${record.author}`} // Create unique key from title and author
          />
        )}
      </div>
    );
  };

  // Get columns based on current tab
  const getColumns = () => {
    // Base columns for all tabs
    const baseColumns = [
      { title: 'ID', dataIndex: 'id', key: 'id' },
      { title: 'Requestor', dataIndex: 'requestor', key: 'requestor' },
      { title: 'Approver', dataIndex: 'approver', key: 'approver', render: text => text || 'N/A' },
      { 
        title: 'Status',
        dataIndex: 'status',
        key: 'status',
        render: status => getStatusTag(status)
      },
      { 
        title: 'Requested Date', 
        dataIndex: 'dateRequested', 
        key: 'dateRequested',
        render: date => date ? new Date(date).toLocaleString() : 'N/A'
      }
    ];

    // Add date returned for returned books
    if (activeTab === 'all' || activeTab === 'Returned') {
      baseColumns.push({ 
        title: 'Date Returned', 
        dataIndex: 'dateReturned', 
        key: 'dateReturned',
        render: date => date ? new Date(date).toLocaleString() : 'N/A'
      });
    }

    // Add actions column only for "all" and "Waiting" tabs (excluding Approved, Rejected, and Returned)
    if (activeTab === 'all' || activeTab === 'Waiting' || activeTab === 'Pending') {
      baseColumns.push({
        title: 'Actions',
        key: 'actions',
        render: (_, record) => {
          if (record.status === 'Waiting' || record.status === 'Pending') {
            return (
              <Space size="small">
                <Button
                  type="primary"
                  icon={<CheckCircleOutlined />}
                  onClick={() => handleApproveRequest(record.id)}
                  loading={processingRequestId === record.id}
                >
                  Approve
                </Button>
                <Button
                  danger
                  icon={<CloseCircleOutlined />}
                  onClick={() => handleRejectRequest(record.id)}
                  loading={processingRequestId === record.id}
                >
                  Reject
                </Button>
              </Space>
            );
          }
          return null;
        }
      });
    }

    return baseColumns;
  };

  return (
    <div className="p-6">
      <h1 className="text-2xl font-bold mb-4">📋 Request Management</h1>
      
      {apiError && (
        <Alert
          message={apiError.message}
          description={apiError.description}
          type={apiError.type}
          showIcon
          closable
          onClose={() => setApiError(null)}
          className="mb-4"
        />
      )}

      {apiSuccess && (
        <Alert
          message={apiSuccess.message}
          description={apiSuccess.description}
          type="success"
          showIcon
          closable
          onClose={() => setApiSuccess(null)}
          className="mb-4"
        />
      )}
      
      <Tabs defaultActiveKey="all" onChange={handleTabChange}>
        <TabPane tab="All Requests" key="all">
          {renderRequestsTable()}
        </TabPane>
        <TabPane tab="Waiting" key="Waiting">
          {renderRequestsTable()}
        </TabPane>
        <TabPane tab="Approved" key="Approved">
          {renderRequestsTable()}
        </TabPane>
        <TabPane tab="Rejected" key="Rejected">
          {renderRequestsTable()}
        </TabPane>
        <TabPane tab="Returned" key="Returned">
          {renderRequestsTable()}
        </TabPane>
      </Tabs>

      {/* Approve confirmation modal */}
      <Modal
        title="Approve Request"
        open={isApproveModalVisible}
        onOk={confirmApprove}
        onCancel={() => setIsApproveModalVisible(false)}
        okText="Yes, Approve"
        cancelText="Cancel"
        okButtonProps={{ loading: processingRequestId === requestToAction }}
      >
        <p>Are you sure you want to approve this request?</p>
        <p>This will allow the user to borrow the requested books.</p>
      </Modal>

      {/* Reject confirmation modal */}
      <Modal
        title="Reject Request"
        open={isRejectModalVisible}
        onOk={confirmReject}
        onCancel={() => setIsRejectModalVisible(false)}
        okText="Yes, Reject"
        cancelText="Cancel"
        okButtonProps={{ danger: true, loading: processingRequestId === requestToAction }}
      >
        <p>Are you sure you want to reject this request?</p>
        <p>The user will not be able to borrow these books and will need to submit a new request.</p>
      </Modal>
    </div>
  );

  function renderRequestsTable() {
    if (loading && !requests.length) {
      return (
        <div className="flex justify-center items-center h-64">
          <Spin size="large" />
        </div>
      );
    }
    
    if (!requests.length > 0) {
      return (
        <Alert 
          message="No requests found."
          type="info"
          showIcon
          className="mb-4"
        />
      );
    }
    
    return (
      <>
        <Table
          dataSource={requests}
          columns={getColumns()} 
          rowKey="id"
          pagination={false}
          expandable={{
            expandedRowRender,
            expandRowByClick: true
          }}
          bordered
          loading={loading}
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
    );
  }
};

export default AdminRequests;
