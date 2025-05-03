import React, { useEffect, useState } from 'react';
import { Table, Tag, Button, Spin, Alert } from 'antd';
import { useNavigate } from 'react-router-dom';
import axiosInstance from '../../../utils/axiosConfig';
import { useAuth } from '../../../context/AuthContext';
import PaginationControls from '../../../components/pagination';

export default function MyBorrowing() {
  const [requests, setRequests] = useState([]);
  const [loading, setLoading] = useState(false);
  const [pagination, setPagination] = useState({
    current: 1,
    pageSize: 5,
    total: 0,
    totalPages: 0,
    hasNext: false,
    hasPrev: false
  });
  
  const navigate = useNavigate();
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
        console.log('Requests data:', res.data);
        
        // Check if response is paginated
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

  const columns = [
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

        return <Tag color={color}>{status.toUpperCase()}</Tag>;
      }
    },
    { title: 'Approver', dataIndex: 'approver', key: 'approver', render: text => text || 'N/A' },
    {
      title: 'Action',
      key: 'action',
      render: (_, record) => (
        <Button
          type="primary"
          onClick={() => {
            navigate(`/requests/${record.id}`);
          }}
        >
          View Details
        </Button>
      ),
    }
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
    </div>
  );
}