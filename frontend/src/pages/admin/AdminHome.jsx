import React, { useState, useEffect } from 'react';
import { Row, Col, Card, Statistic } from 'antd';
import { BookOutlined, UserOutlined, SwapOutlined, ClockCircleOutlined } from '@ant-design/icons';
import axiosInstance from '../../utils/axiosConfig';

const AdminHome = () => {
  const [stats, setStats] = useState({
    totalBooks: 0,
    totalUsers: 0,
    totalAdmin: 0,
    totalRequestCount: 0,
    approvedRequestCount: 0,
    pendingRequestCount: 0,
    rejectedRequestCount: 0,
    books: []
  });

  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchStats = async () => {
      try {
        // Fetch data from all three endpoints in parallel
        const [booksResponse, usersResponse, requestsResponse] = await Promise.all([
          axiosInstance.get('/api/books/overview'),
          axiosInstance.get('/api/users/count'),
          axiosInstance.get('/api/requests/overview')
        ]);
        
        setStats({
          totalBooks: booksResponse.data.totalBooks,
          books: booksResponse.data,
          totalUsers: usersResponse.data.totalUsers,
          totalAdmin: usersResponse.data.totalAdmin,
          totalRequestCount: requestsResponse.data.totalRequestCount,
          approvedRequestCount: requestsResponse.data.approvedRequestCount,
          pendingRequestCount: requestsResponse.data.pendingRequestCount,
          rejectedRequestCount: requestsResponse.data.rejectedRequestCount
        });
        setLoading(false);
      } catch (error) {
        console.error('Failed to fetch dashboard stats:', error);
        setLoading(false);
      }
    };

    fetchStats();
  }, []);

  return (
    <div className="admin-dashboard">
      <h1 className="text-2xl font-bold mb-6">Admin Dashboard</h1>
      
      <Row gutter={16}>
        <Col span={6}>
          <Card loading={loading}>
            <Statistic
              title="Total Books"
              value={stats.totalBooks}
              prefix={<BookOutlined />}
            />
          </Card>
        </Col>
        <Col span={6}>
          <Card loading={loading}>
            <Statistic
              title="Registered Users"
              value={stats.totalUsers}
              prefix={<UserOutlined />}
            />
          </Card>
        </Col>
        <Col span={6}>
          <Card loading={loading}>
            <Statistic
              title="Total Requests"
              value={stats.totalRequestCount}
              prefix={<SwapOutlined />}
            />
          </Card>
        </Col>
        <Col span={6}>
          <Card loading={loading}>
            <Statistic
              title="Pending Requests"
              value={stats.pendingRequestCount}
              prefix={<ClockCircleOutlined />}
            />
          </Card>
        </Col>
      </Row>

      <div className="mt-8">
        <h2 className="text-xl font-bold mb-4">Recent Activities</h2>
        {/* Recent activities component would go here */}
      </div>
    </div>
  );
};

export default AdminHome;
