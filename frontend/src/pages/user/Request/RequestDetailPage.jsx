import React, { useEffect, useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { Card, Descriptions, Table, Tag, Button, Spin, Alert, Modal, message } from 'antd';
import { ArrowLeftOutlined, RollbackOutlined } from '@ant-design/icons';
import axiosInstance from '../../../utils/axiosConfig';
import { useAuth } from '../../../context/AuthContext';

const RequestDetailPage = () => {
  const { id, requestId } = useParams(); // Check for both possible param names
  const actualId = id || requestId; // Use whichever is available
  const navigate = useNavigate();
  const { user } = useAuth();
  const [requestDetails, setRequestDetails] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [returning, setReturning] = useState(false);
  const [isReturnModalVisible, setIsReturnModalVisible] = useState(false);

  useEffect(() => {
    const fetchRequestDetails = async () => {
      // Log the ID for debugging
      
      try {
        setLoading(true);
        
        // Log the API URL for debugging
        const apiUrl = `/api/requests/${actualId}/details`;
        
        const response = await axiosInstance.get(apiUrl);
        
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
    else if (status === 'Waiting') color = 'gold';
    else if (status === 'Returned') color = 'blue';

    return <Tag color={color}>{status.toUpperCase()}</Tag>;
  };

  const handleBackClick = () => {
    navigate('/requests');
  };

  const showReturnModal = () => {
    setIsReturnModalVisible(true);
  };

  const handleReturnBooks = async () => {
    setReturning(true);
    try {
      const payload = {
        requestId: parseInt(actualId),
        processedById: user.id // Include the user's ID so the backend can verify they're authorized
      };

      const response = await axiosInstance.post(
        `/api/requests/${actualId}/return`, 
        payload
      );

      message.success('Books returned successfully!');
      
      // Update the request details with the new status and return date
      setRequestDetails(response.data);
      setIsReturnModalVisible(false);
    } catch (err) {
      console.error('Failed to return books:', err);
      
      // Handle error
      const errorMessage = err.response?.data?.message || 'An error occurred while returning books';
      message.error(errorMessage);
    } finally {
      setReturning(false);
    }
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

  // Determine if the request is eligible for return
  const canReturn = requestDetails?.status === "Approved";

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
              {requestDetails.dateReturned && (
                <Descriptions.Item label="Date Returned">
                  {new Date(requestDetails.dateReturned).toLocaleString()}
                </Descriptions.Item>
              )}
            </Descriptions>
          </Card>

          <Card 
            title="Books in this Request"
            extra={
              canReturn && (
                <Button
                  type="primary"
                  icon={<RollbackOutlined />}
                  onClick={showReturnModal}
                  disabled={!canReturn}
                >
                  Return Books
                </Button>
              )
            }
          >
            {!requestDetails?.books?.length ? (
              <Alert 
                message="No books in this request."
                type="info"
                showIcon
              />
            ) : (
              <Table 
                dataSource={requestDetails.books} 
                columns={columns} 
                rowKey="title"
                pagination={false}
                bordered
              />
            )}
          </Card>

          {/* Return confirmation modal */}
          <Modal
            title="Return Books"
            open={isReturnModalVisible}
            onOk={handleReturnBooks}
            onCancel={() => setIsReturnModalVisible(false)}
            okText="Yes, Return Books"
            cancelText="Cancel"
            okButtonProps={{ loading: returning }}
          >
            <p>Are you sure you want to return these books?</p>
            <p>This action will mark the books as returned and make them available for others to borrow.</p>
          </Modal>
        </>
      )}
    </div>
  );
};

export default RequestDetailPage;
