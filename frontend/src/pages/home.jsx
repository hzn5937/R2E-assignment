import React, { useEffect, useState } from 'react';
import axios from 'axios';
import { Table, Alert, Checkbox, Button, Tag } from 'antd';

const Home = () => {
  const [books, setBooks] = useState([]);
  const [selectedRowKeys, setSelectedRowKeys] = useState([]);
  const [showLimitWarning, setShowLimitWarning] = useState(false);

  useEffect(() => {
    axios.get('http://localhost:3000/books')
      .then(res => {console.log(res.data); setBooks(res.data)})
      .catch(err => console.error('Failed to fetch books:', err));
  }, []);

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
          disabled={record.available === 0}
        />
      ),
    },
    { title: 'Title', dataIndex: 'title', key: 'title' },
    { title: 'Author', dataIndex: 'author', key: 'author' },
    { 
      title: 'Category', 
      dataIndex: 'category', 
      key: 'category',
      render: category => <Tag color="blue">{category}</Tag>, 
    },
    {
      title: 'Available',
      dataIndex: 'available',
      key: 'available',
      render: available => available > 0 ? <Tag color='green'>{available} remaining</Tag> : <Tag color='red'>Not Available</Tag>,
    },
  ];

  return (
    <div className="p-6">
        <h1 className="text-2xl font-bold mb-4">📚 Book Inventory</h1>

        
        {!books.length > 0 && (
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

        {books.length > 0 && (<Table
            className="mb-4"
            dataSource={books}
            columns={columns}
            rowKey="id"
            pagination={false}
            bordered
        />)}

        {books.length > 0 && (<Button
            type="primary"
            disabled={selectedRowKeys.length === 0}
            onClick={() => console.log('Borrowing:', selectedRowKeys)}
        >
            Borrow selected books ({selectedRowKeys.length} / 5)
        </Button>)}
    </div>
  );
};

export default Home;
