import React, { useEffect, useState } from 'react';
import { Table, Button, Space, Modal, Form, Input, message, Spin, Alert } from 'antd';
import { PlusOutlined, EditOutlined, DeleteOutlined } from '@ant-design/icons';
import axiosInstance from '../../utils/axiosConfig';
import PaginationControls from '../../components/pagination';

const AdminCategories = () => {
  const [categories, setCategories] = useState([]);
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
  const [categoryToDelete, setCategoryToDelete] = useState(null);
  const [modalType, setModalType] = useState('add');
  const [currentCategory, setCurrentCategory] = useState(null);
  const [apiError, setApiError] = useState(null);
  const [apiSuccess, setApiSuccess] = useState(null); // Added success state
  const [form] = Form.useForm();

  // Fetch categories with pagination
  const fetchCategories = (pageNum = 1, pageSize = 5) => {
    setLoading(true);
    axiosInstance.get(`/api/categories?pageNum=${pageNum}&pageSize=${pageSize}`)
      .then(res => {
        console.log('Categories data:', res.data);
        setCategories(res.data.items);
        setPagination({
          current: res.data.pageNum,
          pageSize: res.data.pageSize,
          total: res.data.totalCount,
          totalPages: res.data.totalPage,
          hasNext: res.data.hasNext,
          hasPrev: res.data.hasPrev,
        });
      })
      .catch(err => {
        console.error('Failed to fetch categories:', err);
        message.error('Could not load categories. Please try again.');
      })
      .finally(() => {
        setLoading(false);
      });
  };

  useEffect(() => {
    fetchCategories();
  }, []);

  const handlePageChange = (page, pageSize) => {
    fetchCategories(page, pageSize);
  };

  const handlePageSizeChange = (newPageSize) => {
    fetchCategories(1, newPageSize);
  };

  // Show modal for adding a new category
  const showAddModal = () => {
    setModalType('add');
    setCurrentCategory(null);
    form.resetFields();
    setIsModalVisible(true);
  };

  // Show modal for editing an existing category
  const showEditModal = (category) => {
    setModalType('edit');
    setCurrentCategory(category);
    form.setFieldsValue({
      name: category.name,
    });
    setIsModalVisible(true);
  };

  // Handle form submission
  const handleFormSubmit = () => {
    form.validateFields().then(values => {
      if (modalType === 'add') {
        // Add new category
        axiosInstance.post('/api/categories', values)
          .then(() => {
            setApiSuccess({
              message: 'Category Added Successfully',
              description: `Category "${values.name}" has been added to the database.`
            });
            setIsModalVisible(false);
            fetchCategories(pagination.current, pagination.pageSize);
          })
          .catch(err => {
            console.error('Failed to add category:', err);
            const errorMsg = err.response?.data?.error || 'Failed to add category. Please try again.';
            
            setApiError({
              type: 'error',
              message: errorMsg,
              description: 'Please check your input and try again.'
            });
            
            setIsModalVisible(false);
          });
      } else {
        // Edit existing category
        axiosInstance.put(`/api/categories/${currentCategory.id}`, values)
          .then(() => {
            setApiSuccess({
              message: 'Category Updated Successfully',
              description: `Category "${values.name}" has been updated.`
            });
            setIsModalVisible(false);
            fetchCategories(pagination.current, pagination.pageSize);
          })
          .catch(err => {
            console.error('Failed to update category:', err);
            const errorMsg = err.response?.data?.error || 'Failed to update category. Please try again.';
            
            setApiError({
              type: 'error',
              message: errorMsg,
              description: 'Please check your input and try again.'
            });
            
            setIsModalVisible(false);
          });
      }
    });
  };

  // Handle category deletion
  const handleDelete = (categoryId) => {
    console.log('Delete button clicked for category ID:', categoryId);
    setCategoryToDelete(categoryId);
    setIsDeleteModalVisible(true);
  };

  // Function to execute deletion
  const confirmDelete = () => {
    if (!categoryToDelete) return;
    
    console.log('OK button clicked, would delete category ID:', categoryToDelete);
    
    // Check if we're about to delete the last item on the page
    const isLastItemOnPage = categories.length === 1;
    // Check if we're on the last page
    const isLastPage = pagination.current === pagination.totalPages;
    // Determine which page to go to after deletion
    const targetPage = (isLastItemOnPage && isLastPage && pagination.current > 1) 
      ? pagination.current - 1  // Go to previous page if deleting last item on last page
      : pagination.current;     // Otherwise stay on current page
    
    axiosInstance.delete(`/api/categories/${categoryToDelete}`)
      .then(() => {
        // Find the deleted category to include its name in the success message
        const deletedCategory = categories.find(category => category.id === categoryToDelete);
        setApiSuccess({
          message: 'Category Deleted Successfully',
          description: `Category "${deletedCategory?.name || 'Selected category'}" has been removed from the database.`
        });
        fetchCategories(targetPage, pagination.pageSize);
        setIsDeleteModalVisible(false);
      })
      .catch(err => {
        console.error('Failed to delete category:', err);
        const errorMsg = err.response?.data?.message || 'Failed to delete category. Please try again.';
        
        setApiError({
          type: 'error',
          message: 'Failed to Delete Category',
          description: errorMsg
        });
        
        setIsDeleteModalVisible(false);
      });
  };

  const columns = [
    { title: 'ID', dataIndex: 'id', key: 'id' },
    { title: 'Name', dataIndex: 'name', key: 'name' },
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
        <h1 className="text-2xl font-bold">📚 Category Management</h1>
        <Button 
          type="primary" 
          icon={<PlusOutlined />} 
          onClick={showAddModal}
        >
          Add New Category
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

      {loading && !categories.length ? (
        <div className="flex justify-center items-center h-64">
          <Spin size="large" />
        </div>
      ) : (
        <>
          {!categories.length > 0 ? (
            <Alert 
              message="No categories available."
              type="info"
              showIcon
              className="mb-4"
            />
          ) : (
            <>
              <Table
                dataSource={categories}
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
                  itemName="categories"
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
        <p>Are you sure you want to delete this category?</p>
        <p>This action cannot be undone and may affect books assigned to this category.</p>
      </Modal>

      {/* Modal for adding/editing categories */}
      <Modal
        title={modalType === 'add' ? 'Add New Category' : 'Edit Category'}
        open={isModalVisible}
        onOk={handleFormSubmit}
        onCancel={() => setIsModalVisible(false)}
        destroyOnClose={true}
      >
        <Form form={form} layout="vertical">
          <Form.Item
            name="name"
            label="Category Name"
            rules={[{ required: true, message: 'Please enter the category name' }]}
          >
            <Input placeholder="Enter category name" />
          </Form.Item>
        </Form>
      </Modal>
    </div>
  );
};

export default AdminCategories;
