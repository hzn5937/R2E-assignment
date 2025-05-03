import React, { useEffect, useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { Card, Descriptions, Table, Tag, Button, Spin, Alert } from 'antd';
import { ArrowLeftOutlined } from '@ant-design/icons';
import axiosInstance from '../../../utils/axiosConfig';

const RequestDetailPage = () => {
  const { id, requestId } = useParams(); // Check for both possible param names
  const actualId = id || requestId; // Use whichever is available
  const navigate = useNavigate();
  const [requestDetails, setRequestDetails] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    const fetchRequestDetails = async () => {
      // Log the ID for debugging
      console.log('Fetching request details for ID:', actualId);
      
      try {
        setLoading(true);
        
        // Log the API URL for debugging
        const apiUrl = `/api/requests/${actualId}/details`;
        console.log('API URL:', apiUrl);
        
        const response = await axiosInstance.get(apiUrl);
        console.log('API Response:', response.data);
        
        setRequestDetails(response.data);
        setError(null);
      } catch (err) {
        console.error('Failed to fetch request details:', err);
        
        // More detailed error information
        if (err.response) {
          console.error('Response status:', err.response.status);
          console.error('Response data:', err.response.data);
          setError(`Error ${err.response.status}: ${err.response.data.message || 'Failed to load details'}`);
        } else if (err.request) {
          console.error('No response received');
          setError('Server did not respond. Please check your connection.');
        } else {
          setError('Failed to load request details. Please try again.');
        }
      } finally {
        setLoading(false);
      }
    };

    if (actualId) {
      fetchRequestDetails();
    } else {
      console.error('No request ID found in URL parameters');
      setError('No request ID provided');
      setLoading(false);
    }
  }, [actualId]);

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

  const getStatusTag = (status) => {
    let color = 'default';
    if (status === 'Approved') color = 'green';
    else if (status === 'Rejected') color = 'red';
    else if (status === 'Pending') color = 'gold';

    return <Tag color={color}>{status.toUpperCase()}</Tag>;
  };

  const handleBackClick = () => {
    navigate('/requests');
  };

  if (loading) {
    return (
      <div className="flex justify-center items-center h-64">
        <Spin size="large" />
      </div>
    );
  }

  if (error) {
    return (
      <div className="p-6">
        <Alert
          message="Error"
          description={error}
          type="error"
          showIcon
        />
        <Button 
          type="primary" 
          icon={<ArrowLeftOutlined />} 
          onClick={handleBackClick}
          className="mt-4"
        >
          Back to Borrowing
        </Button>
      </div>
    );
  }

  return (
    <div className="p-6">
      <div className='flex justify-between items-center mb-4'>
        <h1 className="text-2xl font-bold mb-4">📋 Request Details</h1>


        <Button 
            type="primary" 
            icon={<ArrowLeftOutlined />} 
            onClick={handleBackClick}
            className="mb-4"
        >
            Back to Borrowing
        </Button>
      </div>
      
      {requestDetails && (
        <>
          <Card className="mb-6">
            <Descriptions 
              title="Request Information" 
              bordered
              column={{ xxl: 4, xl: 3, lg: 3, md: 2, sm: 1, xs: 1 }}
            >
              <Descriptions.Item label="Request ID">{requestDetails.id}</Descriptions.Item>
              <Descriptions.Item label="Requestor">{requestDetails.requestor}</Descriptions.Item>
              <Descriptions.Item label="Status">{getStatusTag(requestDetails.status)}</Descriptions.Item>
              <Descriptions.Item label="Date Requested">{new Date(requestDetails.requestedDate).toLocaleString()}</Descriptions.Item>
              <Descriptions.Item label="Approver">{requestDetails.approver || 'N/A'}</Descriptions.Item>
            </Descriptions>
          </Card>

          <Card title="Books in this Request">
            <Table 
              dataSource={requestDetails.books} 
              columns={columns} 
              rowKey="title"
              pagination={false}
              bordered
            />
          </Card>
        </>
      )}
    </div>
  );
};

export default RequestDetailPage;
