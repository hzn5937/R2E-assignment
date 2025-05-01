import React, { useEffect, useState } from 'react';
import axiosInstance from '../../utils/axiosConfig';
import { Table, Alert, Checkbox, Button, Tag, Pagination, Select } from 'antd';
import { useAuth } from '../../context/AuthContext';
import PaginationControls from '../../components/pagination';

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
  const { user } = useAuth();

  const fetchBooks = (pageNum = 1, pageSize = 5) => {
    setLoading(true);
    axiosInstance.get(`/api/books?pageNum=${pageNum}&pageSize=${pageSize}`)
      .then(res => {
        console.log(res.data);
        setBooks(res.data.items);
        setPagination({
          current: res.data.pageNum,
          pageSize: res.data.pageSize,
          total: res.data.totalCount,
          totalPages: res.data.totalPage,
          hasNext: res.data.hasNext,
          hasPrev: res.data.hasPrev,
        });
        setLoading(false);
      })
      .catch(err => {
        console.error('Failed to fetch books:', err);
        setLoading(false);
      });
  };

  useEffect(() => {
    fetchBooks();
  }, []);

  const handlePageChange = (page, pageSize) => {
    fetchBooks(page, pageSize);
  };

  const handlePageSizeChange = (newPageSize) => {
    // Reset to first page when changing page size
    fetchBooks(1, newPageSize);
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

      {!books.length > 0 && !loading && (
        <Alert 
          message="No books available."
          type="info"
          showIcon
          className="mb-4"
        />
      )}

      {showLimitWarning && (
        <Alert
          message="You can only select up to 5 books."
          type="warning"
          showIcon
          className="mb-4"
        />
      )}

      {books.length > 0 && (
        <>
          <Table
            className="mb-4"
            dataSource={books}
            columns={columns}
            rowKey="id"
            pagination={false}
            bordered
            loading={loading}
          />
          
          {/* Separated borrow button row */}
          <div className="mt-4 mb-4 flex justify-start items-center">
            <Button
              type="primary"
              disabled={selectedRowKeys.length === 0}
              onClick={() => console.log('Borrowing:', selectedRowKeys)}
            >
              Borrow selected books ({selectedRowKeys.length} / 5)
            </Button>
          </div>

          {/* Row 2: Book count display and page size selection */}
          <PaginationControls 
            pagination={pagination}
            onPageChange={handlePageChange}
            onPageSizeChange={handlePageSizeChange}
            itemName="books"
          />
        </>
      )}
    </div>
  );
};

export default Home;