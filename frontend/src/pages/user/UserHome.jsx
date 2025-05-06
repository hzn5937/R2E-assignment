import React, { useEffect, useState } from 'react';
import axiosInstance from '../../utils/axiosConfig';
import { Table, Alert, Checkbox, Button, Tag, message, Spin, Divider } from 'antd';
import { useAuth } from '../../context/AuthContext';
import PaginationControls from '../../components/pagination';
import SearchBar from '../../components/SearchBar';
import FilterBar from '../../components/FilterBar';

const Home = () => {
  const [books, setBooks] = useState([]);
  const [selectedRowKeys, setSelectedRowKeys] = useState([]);
  const [showLimitWarning, setShowLimitWarning] = useState(false);
  const [pagination, setPagination] = useState({
    current: 1,
    pageSize: 5,
    total: 0,
    totalPages: 0,
    hasNext: false,
    hasPrev: false,
  });
  const [loading, setLoading] = useState(false);
  const [borrowing, setBorrowing] = useState(false);
  const [availableRequests, setAvailableRequests] = useState(null);
  const [requestsLoading, setRequestsLoading] = useState(false);
  const [apiError, setApiError] = useState(null);
  const { user } = useAuth();
  const [searchTerm, setSearchTerm] = useState('');
  const [searchLoading, setSearchLoading] = useState(false);
  
  // Filter-related states
  const [categories, setCategories] = useState([]);
  const [selectedCategory, setSelectedCategory] = useState(null);
  const [availabilityFilter, setAvailabilityFilter] = useState(null);
  const [isFilterApplied, setIsFilterApplied] = useState(false);
  const [filterLoading, setFilterLoading] = useState(false);

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

  // New function to handle book search
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

  // New function to handle filtering books by category and availability
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

  // Fetch categories for filters
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

  const fetchAvailableRequests = () => {
    if (!user || !user.id) return;
    
    setRequestsLoading(true);
    axiosInstance.get(`/api/requests/available/${user.id}`)
      .then(res => {
        setAvailableRequests(res.data.availableRequests);
      })
      .catch(err => {
        console.error('Failed to fetch available requests:', err);
        message.error('Could not load available requests.');
      })
      .finally(() => {
        setRequestsLoading(false);
      });
  };

  useEffect(() => {
    if (user?.id) {
      fetchBooks();
      fetchAvailableRequests();
      fetchCategories(); // Fetch categories when component mounts
    }
  }, [user?.id]); // Re-fetch when user ID changes

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

  const handleCheckboxChange = (record, checked) => {
    if (checked && selectedRowKeys.length >= 5) {
      setShowLimitWarning(true);
      return;
    }

    setShowLimitWarning(false);

    const newKeys = checked
      ? [...selectedRowKeys, record.id]
      : selectedRowKeys.filter(key => key !== record.id);

    setSelectedRowKeys(newKeys);
  };

  const handleBorrow = async () => {
    if (selectedRowKeys.length === 0) return;
    
    setBorrowing(true);
    setApiError(null); // Clear previous errors
    
    try {
      // Create payload with user ID and selected book IDs
      const payload = {
        userId: user.id,
        bookIds: selectedRowKeys
      };
      
      // Send POST request to create a borrow request
      const response = await axiosInstance.post('/api/requests/create', payload);
      
      message.success(`Successfully requested ${selectedRowKeys.length} book(s)!`);
      
      // Clear selections after successful request
      setSelectedRowKeys([]);
      
      // Refresh the book list to update availability
      fetchBooks(pagination.current, pagination.pageSize);
      
      // Option 2: Call API again to get the updated available requests
      fetchAvailableRequests();
    } catch (err) {
      console.error('Failed to borrow books:', err);
      
      // Extract error details from response
      const statusCode = err.response?.status;
      const errorMessage = err.response?.data?.message || 'An unknown error occurred';
      
      // Handle specific error cases
      if (statusCode === 400) {
        if (errorMessage.includes('quota') || errorMessage.includes('limit')) {
          setApiError({
            type: 'error',
            message: 'Monthly Quota Exceeded',
            description: 'You have used all your available monthly borrowing requests.'
          });
        } else if (errorMessage.includes('stock') || errorMessage.includes('available')) {
          setApiError({
            type: 'warning',
            message: 'Books No Longer Available',
            description: 'Some selected books are no longer in stock. Please refresh and try again.'
          });
        } else {
          setApiError({
            type: 'error',
            message: 'Request Failed',
            description: errorMessage
          });
        }
      } else if (statusCode === 401 || statusCode === 403) {
        setApiError({
          type: 'error',
          message: 'Authentication Error',
          description: 'Your session has expired. Please login again.'
        });
      } else {
        setApiError({
          type: 'error',
          message: 'Request Failed',
          description: 'There was a problem processing your request. Please try again later.'
        });
      }
      
      // Always refresh available requests to ensure accuracy
      fetchAvailableRequests();
    } finally {
      setBorrowing(false);
    }
  };

  const columns = [
    {
      title: '',
      dataIndex: 'checkbox',
      key: 'checkbox',
      render: (_, record) => (
        <Checkbox
          checked={selectedRowKeys.includes(record.id)}
          onChange={(e) => handleCheckboxChange(record, e.target.checked)}
          disabled={record.availableQuantity === 0}
        />
      ),
    },
    { title: 'Title', dataIndex: 'title', key: 'title' },
    { title: 'Author', dataIndex: 'author', key: 'author' },
    { 
      title: 'Category', 
      dataIndex: 'categoryName', 
      key: 'category',
      render: category => <Tag color="blue">{category}</Tag>, 
    },
    {
      title: 'Available',
      dataIndex: 'availableQuantity',
      key: 'available',
      render: available => available > 0 ? <Tag color='green'>{available} remaining</Tag> : <Tag color='red'>Not Available</Tag>,
    },
  ];

  return (
    <div className="p-6">
      <h1 className="text-2xl font-bold mb-4">📚 Book Inventory</h1>

      {/* Search bar component */}
      <SearchBar 
        searchTerm={searchTerm}
        setSearchTerm={setSearchTerm}
        handleSearch={handleSearch}
        handleClearSearch={handleClearSearch}
        searchLoading={searchLoading}
      />

      {/* Divider between search and filter bars */}
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
              {showLimitWarning && (
                <Alert
                  message="You can only select up to 5 books."
                  type="warning"
                  showIcon
                  className="mb-4"
                />
              )}

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

              <Table
                className="mb-4"
                dataSource={books}
                columns={columns}
                rowKey="id"
                pagination={false}
                bordered
                loading={loading}
              />
              
              {/* Borrow button row with available requests tag */}
              <div className="mt-4 mb-4 flex justify-between items-center">
                <div className="flex items-center">
                  <Button
                    type="primary"
                    disabled={selectedRowKeys.length === 0 || availableRequests === 0}
                    onClick={handleBorrow}
                    loading={borrowing}
                  >
                    Borrow selected books ({selectedRowKeys.length} / 5)
                  </Button>
                </div>
                <div className="flex items-center">
                  {requestsLoading ? (
                    <Spin size="small" className="mr-2" />
                  ) : (
                    availableRequests !== null && (
                      <Tag 
                        color={availableRequests > 2 ? "green" : availableRequests > 0 ? "gold" : "red"} 
                        className="text-base"
                      >
                        {availableRequests} monthly requests available
                      </Tag>
                    )
                  )}
                </div>
              </div>

              <PaginationControls 
                pagination={pagination}
                onPageChange={handlePageChange}
                onPageSizeChange={handlePageSizeChange}
                itemName="books"
              />
            </>
          )}
        </>
      )}
    </div>
  );
};

export default Home;