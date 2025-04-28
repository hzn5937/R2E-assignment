import React, { useEffect, useState } from 'react';
import axios from 'axios';
import { Table, Tag, Button } from 'antd';
import { useNavigate } from 'react-router-dom';

export default function Post() {
  const [requests, setRequests] = useState([]);
  const navigate = useNavigate();

  useEffect(() => {
    axios.get('http://localhost:3000/requests')
      .then(res => setRequests(res.data))
      .catch(err => console.error('Failed to fetch requests:', err));
  }, []);

  const columns = [
    { title: 'ID', dataIndex: 'id', key: 'id' },
    { title: 'Date Requested', dataIndex: 'date_requested', key: 'date_requested' },
    {
      title: 'Status',
      dataIndex: 'status',
      key: 'status',
      render: status => {
        let color = 'default';
        if (status === 'approved') color = 'green';
        else if (status === 'rejected') color = 'red';
        else if (status === 'pending') color = 'gold';

        return <Tag color={color}>{status.toUpperCase()}</Tag>;
      }
    },
    { title: 'Approver', dataIndex: 'approver', key: 'approver', render: text => text || '—' },
    {
      title: 'Action',
      key: 'action',
      render: (_, record) => (
        <Button
          type="primary"
          onClick={() => navigate(`/request/${record.id}`)}
        >
          View Details
        </Button>
      ),
    }
  ];

  return (
    <div className="p-6">
      <h1 className="text-2xl font-bold mb-4">📚 My Borrowing</h1>
      <Table
        dataSource={requests}
        columns={columns}
        rowKey="id"
        pagination={false}
        bordered
      />
    </div>
  );
}
