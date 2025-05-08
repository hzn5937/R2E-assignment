import React, { useEffect, useState } from 'react';
import { Table, Button, Space, Tag, Modal, Form, Input, Select, InputNumber, message, Spin, Alert, Divider } from 'antd';
import { PlusOutlined, EditOutlined, DeleteOutlined } from '@ant-design/icons';
import axiosInstance from '../../utils/axiosConfig';
import PaginationControls from '../../components/pagination';
import SearchBar from '../../components/SearchBar';
import FilterBar from '../../components/FilterBar';

const AdminBooks = () => {
  const [books, setBooks] = useState([]);
  const [loading, setLoading] = useState(false);
  const [pagination, setPagination] = useState({
    current: 1,
    pageSize: 10,
    total: 0,
    totalPages: 0,
    hasNext: false,
    hasPrev: false,
  });
  const [categories, setCategories] = useState([]);
  const [isModalVisible, setIsModalVisible] = useState(false);
  const [isDeleteModalVisible, setIsDeleteModalVisible] = useState(false); // New state for delete modal (completely different from edit/add modal)
  const [bookToDelete, setBookToDelete] = useState(null); 
  const [modalType, setModalType] = useState('add');
  const [currentBook, setCurrentBook] = useState(null);
  const [apiError, setApiError] = useState(null);
  const [apiSuccess, setApiSuccess] = useState(null); 
  const [form] = Form.useForm();
  const [searchTerm, setSearchTerm] = useState('');
  const [searchLoading, setSearchLoading] = useState(false);
  
  // Filter-related states
  const [selectedCategory, setSelectedCategory] = useState(null);
  const [availabilityFilter, setAvailabilityFilter] = useState(null);
  const [isFilterApplied, setIsFilterApplied] = useState(false);
  const [filterLoading, setFilterLoading] = useState(false);

  // Fetch books with pagination
  const fetchBooks = (pageNum = 1, pageSize = 5) => {
    setLoading(true);
    axiosInstance.get(`/api/books?pageNum=${pageNum}&pageSize=${pageSize}`)
      .then(res => {
        setBooks(res.data.items);
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
        console.error('Failed to fetch books:', err);
        message.error('Could not load books. Please try again.');
      })
      .finally(() => {
        setLoading(false);
      });
  };

  // Handle search functionality
  const searchBooks = (pageNum = 1, pageSize = 5) => {
    if (!searchTerm.trim()) {
      fetchBooks(pageNum, pageSize);
      return;
    }
    
    setSearchLoading(true);
    setLoading(true);
    axiosInstance.get(`/api/books/search?searchTerm=${encodeURIComponent(searchTerm)}&pageNum=${pageNum}&pageSize=${pageSize}`)
      .then(res => {
        setBooks(res.data.items);
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
        console.error('Failed to search books:', err);
        message.error('Search failed. Please try again.');
      })
      .finally(() => {
        setLoading(false);
        setSearchLoading(false);
      });
  };
  
  // Handle filtering books by category and availability
  const filterBooks = (pageNum = 1, pageSize = 5) => {
    setFilterLoading(true);
    setLoading(true);
    
    let url = `/api/books/filter?pageNum=${pageNum}&pageSize=${pageSize}`;
    
    if (selectedCategory !== null) {
      url += `&categoryId=${selectedCategory}`;
    }
    
    if (availabilityFilter !== null) {
      url += `&isAvailable=${availabilityFilter}`;
    }
    
    axiosInstance.get(url)
      .then(res => {
        setBooks(res.data.items);
        setPagination({
          current: res.data.pageNum,
          pageSize: res.data.pageSize,
          total: res.data.totalCount,
          totalPages: res.data.totalPage,
          hasNext: res.data.hasNext,
          hasPrev: res.data.hasPrev,
        });
        setIsFilterApplied(true);
      })
      .catch(err => {
        console.error('Failed to filter books:', err);
        message.error('Filter failed. Please try again.');
      })
      .finally(() => {
        setLoading(false);
        setFilterLoading(false);
      });
  };

  // Handle search submission
  const handleSearch = () => {
    // Clear any filters when performing a search
    setSelectedCategory(null);
    setAvailabilityFilter(null);
    setIsFilterApplied(false);
    searchBooks(1, pagination.pageSize);
  };

  // Clear search and reset to all books
  const handleClearSearch = () => {
    setSearchTerm('');
    fetchBooks(1, pagination.pageSize);
  };
  
  // Handle applying filters
  const handleApplyFilters = () => {
    // Clear search term when filtering
    setSearchTerm('');
    filterBooks(1, pagination.pageSize);
  };

  // Handle clearing filters
  const handleClearFilters = () => {
    setSelectedCategory(null);
    setAvailabilityFilter(null);
    setIsFilterApplied(false);
    fetchBooks(1, pagination.pageSize);
  };

  // Fetch categories for the form dropdown
  const fetchCategories = () => {
    axiosInstance.get('/api/categories?pageSize=1000')
      .then(res => {
        const categoriesData = Array.isArray(res.data) ? res.data : 
                              res.data.items ? res.data.items : [];
        setCategories(categoriesData);
      })
      .catch(err => {
        console.error('Failed to fetch categories:', err);
        message.error('Could not load categories.');
        // Ensure categories is reset to an empty array on error
        setCategories([]);
      });
  };

  useEffect(() => {
    fetchBooks();
    fetchCategories();
  }, []);

  const handlePageChange = (page, pageSize) => {
    if (searchTerm.trim()) {
      searchBooks(page, pageSize);
    } else if (isFilterApplied) {
      filterBooks(page, pageSize);
    } else {
      fetchBooks(page, pageSize);
    }
  };

  const handlePageSizeChange = (newPageSize) => {
    if (searchTerm.trim()) {
      searchBooks(1, newPageSize);
    } else if (isFilterApplied) {
      filterBooks(1, newPageSize);
    } else {
      fetchBooks(1, newPageSize);
    }
  };

  // Show modal for adding a new book
  const showAddModal = () => {
    setModalType('add');
    setCurrentBook(null);
    form.resetFields();
    setIsModalVisible(true);
  };

  // Show modal for editing an existing book
  const showEditModal = (book) => {
    setModalType('edit');
    setCurrentBook(book);
    form.setFieldsValue({
      title: book.title,
      author: book.author,
      categoryId: book.categoryId,
      totalQuantity: book.totalQuantity,
    });
    setIsModalVisible(true);
  };

  // Handle form submission
  const handleFormSubmit = () => {
    form.validateFields().then(values => {
      if (modalType === 'add') {
        // Add new book
        axiosInstance.post('/api/books', values)
          .then(() => {
            setApiSuccess({
              message: 'Book Added Successfully',
              description: `Book "${values.title}" has been added to the database.`
            });
            setIsModalVisible(false);
            fetchBooks(pagination.current, pagination.pageSize);
          })
          .catch(err => {
            console.error('Failed to add book:', err);
            // Get error message directly from API response
            const errorMsg = err.response?.data?.error || 'Failed to add book. Please try again.';
            
            setApiError({
              type: 'error',
              message: errorMsg, 
              description: 'Please check your input and try again.'
            });
            
            // Close modal to show the error message
            setIsModalVisible(false);
          });
      } else {
        axiosInstance.put(`/api/books/${currentBook.id}`, values)
          .then(() => {
            setApiSuccess({
              message: 'Book Updated Successfully',
              description: `Book "${values.title}" has been updated.`
            });
            setIsModalVisible(false);
            fetchBooks(pagination.current, pagination.pageSize);
          })
          .catch(err => {
            console.error('Failed to update book:', err);
            // Extract and display server error message if available
            const errorMsg = err.response?.data?.error || 'Failed to update book. Please try again.';
            
            setApiError({
              type: 'error',
              message: errorMsg, 
              description: 'Please check your input and try again.'
            });
            
            // Close modal to show the error message
            setIsModalVisible(false);
          });
      }
    });
  };

  // Handle book deletion - Modified to use custom modal
  const handleDelete = (bookId) => {
    setBookToDelete(bookId);
    setIsDeleteModalVisible(true);
  };

  // Function to execute deletion
  const confirmDelete = () => {
    if (!bookToDelete) return;
    
    const isLastItemOnPage = books.length === 1;
    const isLastPage = pagination.current === pagination.totalPages;
    // Determine which page to go to after deletion (last item on last page)
    const targetPage = (isLastItemOnPage && isLastPage && pagination.current > 1) 
      ? pagination.current - 1  // Go to previous page if deleting last item on last page
      : pagination.current;     // Otherwise stay on current page
    
    axiosInstance.delete(`/api/books/${bookToDelete}`)
      .then(() => {
        // Find the deleted book to include its title in the success message
        const deletedBook = books.find(book => book.id === bookToDelete);
        setApiSuccess({
          message: 'Book Deleted Successfully',
          description: `Book "${deletedBook?.title || 'Selected book'}" has been removed from the database.`
        });
        
        if (searchTerm.trim()) {
          searchBooks(targetPage, pagination.pageSize);
        } else if (isFilterApplied) {
          filterBooks(targetPage, pagination.pageSize);
        } else {
          fetchBooks(targetPage, pagination.pageSize);
        }
        
        setIsDeleteModalVisible(false);
      })
      .catch(err => {
        console.error('Failed to delete book:', err);
        // Extract and display server error message if available
        const errorMsg = err.response?.data?.message || 'Failed to delete book. Please try again.';
        
        setApiError({
          type: 'error',
          message: 'Failed to Delete Book',
          description: errorMsg
        });
        
        // Close modal to show the error message
        setIsDeleteModalVisible(false);
      });
  };

  const columns = [
    { title: 'ID', dataIndex: 'id', key: 'id' },
    { title: 'Title', dataIndex: 'title', key: 'title' },
    { title: 'Author', dataIndex: 'author', key: 'author' },
    { 
      title: 'Category', 
      dataIndex: 'categoryName', 
      key: 'category',
      render: category => <Tag color="blue">{category}</Tag>, 
    },
    {
      title: 'Total',
      dataIndex: 'totalQuantity',
      key: 'totalQuantity',
      render: total => <Tag color="purple">{total} copies</Tag>,
    },
    {
      title: 'Available',
      dataIndex: 'availableQuantity',
      key: 'available',
      render: available => available > 0 ? <Tag color='green'>{available} remaining</Tag> : <Tag color='red'>Not Available</Tag>,
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
        <h1 className="text-2xl font-bold">📚 Book Management</h1>
        <Button 
          type="primary" 
          icon={<PlusOutlined />} 
          onClick={showAddModal}
        >
          Add New Book
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

      {/* Search bar component */}
      <SearchBar 
        searchTerm={searchTerm}
        setSearchTerm={setSearchTerm}
        handleSearch={handleSearch}
        handleClearSearch={handleClearSearch}
        searchLoading={searchLoading}
      />

      <Divider />

      {/* Filter bar component */}
      <FilterBar
        categories={categories}
        selectedCategory={selectedCategory}
        availabilityFilter={availabilityFilter}
        onCategoryChange={setSelectedCategory}
        onAvailabilityChange={setAvailabilityFilter}
        onApplyFilters={handleApplyFilters}
        onClearFilters={handleClearFilters}
        isFilterApplied={isFilterApplied}
        filterLoading={filterLoading}
      />

      {loading && !books.length ? (
        <div className="flex justify-center items-center h-64">
          <Spin size="large" />
        </div>
      ) : (
        <>
          {!books.length > 0 ? (
            <Alert 
              message="No books available."
              type="info"
              showIcon
              className="mb-4"
            />
          ) : (
            <>
              <Table
                dataSource={books}
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
                  itemName="books"
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
        <p>Are you sure you want to delete this book?</p>
        <p>This action cannot be undone.</p>
      </Modal>

      {/* Modal for adding/editing books */}
      <Modal
        title={modalType === 'add' ? 'Add New Book' : 'Edit Book'}
        open={isModalVisible}
        onOk={handleFormSubmit}
        onCancel={() => setIsModalVisible(false)}
        destroyOnClose={true}
      >
        <Form form={form} layout="vertical">
          <Form.Item
            name="title"
            label="Title"
            rules={[{ required: true, message: 'Please enter the book title' }]}
          >
            <Input placeholder="Enter book title" />
          </Form.Item>
          
          <Form.Item
            name="author"
            label="Author"
            rules={[{ required: true, message: 'Please enter the author name' }]}
          >
            <Input placeholder="Enter author name" />
          </Form.Item>
          
          <Form.Item
            name="categoryId"
            label="Category"
            rules={[{ required: true, message: 'Please select a category' }]}
          >
            <Select placeholder="Select a category">
              {Array.isArray(categories) && categories.map(category => (
                <Select.Option key={category.id} value={category.id}>
                  {category.name}
                </Select.Option>
              ))}
            </Select>
          </Form.Item>
          
          {/* Only show totalQuantity field when editing */}
          {modalType === 'edit' && currentBook && (
            <Form.Item
              name="totalQuantity"
              label={
                <span>
                  Total Quantity 
                  <span className="text-gray-500 ml-2">
                    (minimum: {currentBook.totalQuantity - currentBook.availableQuantity})
                  </span>
                </span>
              }
              rules={[
                { required: true, message: 'Please enter the total quantity' },
                {
                  validator: (_, value) => {
                    const minAllowed = currentBook.totalQuantity - currentBook.availableQuantity;
                    if (value < minAllowed) {
                      return Promise.reject(`Minimum allowed quantity is ${minAllowed}`);
                    }
                    return Promise.resolve();
                  }
                }
              ]}
            >
              <InputNumber 
                min={currentBook.totalQuantity - currentBook.availableQuantity} 
                placeholder="Enter total quantity" 
                style={{ width: '100%' }} 
              />
            </Form.Item>
          )}
        </Form>
      </Modal>
    </div>
  );
};

export default AdminBooks;
