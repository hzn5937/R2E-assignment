import React, { useEffect, useState } from 'react';
import { Table, Button, Space, Modal, Form, Input, Select, Tag, message, Spin, Alert } from 'antd';
import { PlusOutlined, EditOutlined, DeleteOutlined } from '@ant-design/icons';
import axiosInstance from '../../utils/axiosConfig';
import PaginationControls from '../../components/pagination';
import { useAuth } from '../../context/AuthContext';

const AdminUsers = () => {
  const [users, setUsers] = useState([]);
  const [loading, setLoading] = useState(false);
  const [pagination, setPagination] = useState({
    current: 1,
    pageSize: 10,
    total: 0,
    totalPages: 0,
    hasNext: false,
    hasPrev: false,
  });
  const [isModalVisible, setIsModalVisible] = useState(false);
  const [isDeleteModalVisible, setIsDeleteModalVisible] = useState(false);
  const [userToDelete, setUserToDelete] = useState(null);
  const [modalType, setModalType] = useState('add');
  const [currentUser, setCurrentUser] = useState(null);
  const [apiError, setApiError] = useState(null);
  const [apiSuccess, setApiSuccess] = useState(null); // Added success state
  const [form] = Form.useForm();
  const { user: currentAuthUser } = useAuth();

  // Fetch users with pagination
  const fetchUsers = (pageNum = 1, pageSize = 10) => {
    setLoading(true);
    axiosInstance.get(`/api/admin/users?pageNum=${pageNum}&pageSize=${pageSize}`)
      .then(res => {
        if (res.data.items) {
          setUsers(res.data.items);
          setPagination({
            current: res.data.pageNum,
            pageSize: res.data.pageSize,
            total: res.data.totalCount,
            totalPages: res.data.totalPage,
            hasNext: res.data.hasNext,
            hasPrev: res.data.hasPrev,
          });
        } else {
          setUsers(res.data);
        }
      })
      .catch(err => {
        console.error('Failed to fetch users:', err);
        message.error('Could not load users. Please try again.');
        setApiError({
          type: 'error',
          message: 'Failed to load users',
          description: err.response?.data?.message || 'There was an error loading the users. Please try again.'
        });
      })
      .finally(() => {
        setLoading(false);
      });
  };

  useEffect(() => {
    fetchUsers();
  }, []);

  const handlePageChange = (page, pageSize) => {
    fetchUsers(page, pageSize);
  };
  
  const handlePageSizeChange = (newPageSize) => {
    fetchUsers(1, newPageSize);
  };

  // Show modal for adding a new user
  const showAddModal = () => {
    setModalType('add');
    setCurrentUser(null);
    form.resetFields();
    form.setFieldsValue({
      role: 'User'
    });
    setIsModalVisible(true);
  };

  // Show modal for editing an existing user
  const showEditModal = (user) => {
    setModalType('edit');
    setCurrentUser(user);
    form.setFieldsValue({
      username: user.username,
      email: user.email,
      role: user.role,
      password: '' // Empty password field for editing
    });
    setIsModalVisible(true);
  };

  // Handle form submission
  const handleFormSubmit = () => {
    form.validateFields().then(values => {
      // Remove empty password field for updates if not provi ded
      if (modalType === 'edit' && !values.password) {
        delete values.password;
      }

      if (modalType === 'add') {
        // Add new user
        axiosInstance.post('/api/admin/users', values)
          .then(() => {
            setApiSuccess({
              message: 'User Added Successfully',
              description: `User "${values.username}" has been added to the system.`
            });
            setIsModalVisible(false);
            fetchUsers(pagination.current, pagination.pageSize);
          })
          .catch(err => {
            console.error('Failed to add user:', err);
            let errorMsg = 'Failed to add user. Please try again.';
            
            // Check for specific error messages
            if (err.response?.status === 409) {
              if (err.response.data.includes('Username')) {
                errorMsg = 'Username is already taken';
              } else if (err.response.data.includes('Email')) {
                errorMsg = 'Email is already registered';
              }
            } else if (err.response?.data) {
              errorMsg = err.response.data;
            }
            
            setApiError({
              type: 'error',
              message: 'User Creation Failed',
              description: errorMsg
            });
            
            setIsModalVisible(false);
          });
      } else {
        // Edit existing user
        axiosInstance.put(`/api/admin/users/${currentUser.id}`, values)
          .then(() => {
            setApiSuccess({
              message: 'User Updated Successfully',
              description: `User "${values.username}" has been updated.`
            });
            setIsModalVisible(false);
            fetchUsers(pagination.current, pagination.pageSize);
          })
          .catch(err => {
            console.error('Failed to update user:', err);
            let errorMsg = 'Failed to update user. Please try again.';
            
            // Check for specific error messages
            if (err.response?.status === 409) {
              if (err.response.data.includes('Username')) {
                errorMsg = 'Username is already taken';
              } else if (err.response.data.includes('Email')) {
                errorMsg = 'Email is already registered';
              }
            } else if (err.response?.data) {
              errorMsg = err.response.data;
            }
            
            setApiError({
              type: 'error',
              message: 'User Update Failed',
              description: errorMsg
            });
            
            setIsModalVisible(false);
          });
      }
    });
  };

  // Handle user deletion
  const handleDelete = (userId) => {
    // Prevent self-deletion
    if (userId === currentAuthUser.id) {
      message.error("You cannot delete your own account!");
      return;
    }
    
    setUserToDelete(userId);
    setIsDeleteModalVisible(true);
  };

  // Function to execute deletion
  const confirmDelete = () => {
    if (!userToDelete) return;
    
    // Check if we're about to delete the last item on the page
    const isLastItemOnPage = users.length === 1;
    // Check if we're on the last page
    const isLastPage = pagination.current === pagination.totalPages;
    // Determine which page to go to after deletion
    const targetPage = (isLastItemOnPage && isLastPage && pagination.current > 1) 
      ? pagination.current - 1  // Go to previous page if deleting last item on last page
      : pagination.current;     // Otherwise stay on current page
    
    axiosInstance.delete(`/api/admin/users/${userToDelete}`)
      .then(() => {
        // Find the deleted user to include the username in success message
        const deletedUser = users.find(user => user.id === userToDelete);
        setApiSuccess({
          message: 'User Deleted Successfully',
          description: `User "${deletedUser?.username || 'Selected user'}" has been removed from the system.`
        });
        fetchUsers(targetPage, pagination.pageSize);
        setIsDeleteModalVisible(false);
      })
      .catch(err => {
        console.error('Failed to delete user:', err);
        const errorMsg = err.response?.data || 'Failed to delete user. Please try again.';
        
        setApiError({
          type: 'error',
          message: 'Failed to Delete User',
          description: errorMsg
        });
        
        setIsDeleteModalVisible(false);
      });
  };

  // Function to render role tag with appropriate color
  const getRoleTag = (role) => {
    let color = 'blue';
    if (role === 'Admin') color = 'red';
    return <Tag color={color}>{role}</Tag>;
  };

  const columns = [
    { title: 'ID', dataIndex: 'id', key: 'id' },
    { title: 'Username', dataIndex: 'username', key: 'username' },
    { title: 'Email', dataIndex: 'email', key: 'email' },
    { 
      title: 'Role', 
      dataIndex: 'role', 
      key: 'role',
      render: role => getRoleTag(role)
    },
    {
      title: 'Actions',
      key: 'actions',
      render: (_, record) => (
        <Space size="middle">
          <Button 
            icon={<EditOutlined />} 
            onClick={() => showEditModal(record)} 
            type="primary"
          >
            Edit
          </Button>
          <Button 
            icon={<DeleteOutlined />} 
            onClick={() => handleDelete(record.id)} 
            danger
            disabled={record.id === currentAuthUser.id} // Disable delete button for current user
          >
            Delete
          </Button>
        </Space>
      ),
    },
  ];

  return (
    <div className="p-6">
      <div className="flex justify-between items-center mb-4">
        <h1 className="text-2xl font-bold">👥 User Management</h1>
        <Button 
          type="primary" 
          icon={<PlusOutlined />} 
          onClick={showAddModal}
        >
          Add New User
        </Button>
      </div>

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

      {loading && !users.length ? (
        <div className="flex justify-center items-center h-64">
          <Spin size="large" />
        </div>
      ) : (
        <>
          {!users.length > 0 ? (
            <Alert 
              message="No users available."
              type="info"
              showIcon
              className="mb-4"
            />
          ) : (
            <>
              <Table
                dataSource={users}
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
                  itemName="users"
                />
              </div>
            </>
          )}
        </>
      )}

      {/* Delete confirmation modal */}
      <Modal
        title="Confirm Deletion"
        open={isDeleteModalVisible}
        onOk={confirmDelete}
        onCancel={() => setIsDeleteModalVisible(false)}
        okText="Yes, Delete"
        cancelText="Cancel"
        okButtonProps={{ danger: true }}
      >
        <p>Are you sure you want to delete this user?</p>
        <p>This action cannot be undone.</p>
      </Modal>

      {/* Modal for adding/editing users */}
      <Modal
        title={modalType === 'add' ? 'Add New User' : 'Edit User'}
        open={isModalVisible}
        onOk={handleFormSubmit}
        onCancel={() => setIsModalVisible(false)}
        destroyOnClose={true}
      >
        <Form form={form} layout="vertical">
          <Form.Item
            name="username"
            label="Username"
            rules={[{ required: true, message: 'Please enter the username' }]}
          >
            <Input placeholder="Enter username" />
          </Form.Item>
          
          <Form.Item
            name="email"
            label="Email"
            rules={[
              { required: true, message: 'Please enter email' },
              { type: 'email', message: 'Please enter a valid email' }
            ]}
          >
            <Input placeholder="Enter email" />
          </Form.Item>
          
          <Form.Item
            name="role"
            label="Role"
            rules={[{ required: true, message: 'Please select a role' }]}
          >
            <Select placeholder="Select role">
              <Select.Option value="User">User</Select.Option>
              <Select.Option value="Admin">Admin</Select.Option>
            </Select>
          </Form.Item>
          
          <Form.Item
            name="password"
            label={modalType === 'add' ? "Password" : "Password (Leave blank to keep current)"}
            rules={[
              { required: modalType === 'add', message: 'Please enter password' },
              { min: 6, message: 'Password must be at least 6 characters' }
            ]}
          >
            <Input.Password placeholder={modalType === 'add' ? "Enter password" : "Enter new password (optional)"} />
          </Form.Item>
        </Form>
      </Modal>
    </div>
  );
};

export default AdminUsers;