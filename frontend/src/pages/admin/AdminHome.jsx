import React, { useState, useEffect } from 'react';
import { Row, Col, Card, Statistic } from 'antd';
import { BookOutlined, UserOutlined, SwapOutlined, ClockCircleOutlined } from '@ant-design/icons';
import axiosInstance from '../../utils/axiosConfig';
import ChartComponent from '../../components/ChartComponent';

const AdminHome = () => {
  const [stats, setStats] = useState({
    totalBooks: 0,
    totalUsers: 0,
    totalAdmin: 0,
    totalRequestCount: 0,
    approvedRequestCount: 0,
    pendingRequestCount: 0,
    rejectedRequestCount: 0,
    returnedRequestCount: 0,
    books: []
  });

  const [loading, setLoading] = useState(true);
  const [bookQuantities, setBookQuantities] = useState(null);
  const [requestStatus, setRequestStatus] = useState(null);
  const [booksPerCategory, setBooksPerCategory] = useState(null);
  const [mostPopular, setMostPopular] = useState(null);

  useEffect(() => {
    const fetchStats = async () => {
      try {
        // Fetch data from all three endpoints in parallel
        const [booksResponse, usersResponse, requestsResponse] = await Promise.all([
          axiosInstance.get('/api/statistics/book-overview'),
          axiosInstance.get('/api/statistics/user-count'),
          axiosInstance.get('/api/statistics/request-overview')
        ]);
        
        setStats({
          totalBooks: booksResponse.data.totalBooks,
          books: booksResponse.data,
          totalUsers: usersResponse.data.totalUsers,
          totalAdmin: usersResponse.data.totalAdmin,
          totalRequestCount: requestsResponse.data.totalRequestCount,
          approvedRequestCount: requestsResponse.data.approvedRequestCount,
          pendingRequestCount: requestsResponse.data.pendingRequestCount,
          rejectedRequestCount: requestsResponse.data.rejectedRequestCount,
          returnedRequestCount: requestsResponse.data.returnedRequestCount
        });
        setLoading(false);
      } catch (error) {
        console.error('Failed to fetch dashboard stats:', error);
        setLoading(false);
      }
    };

    const fetchData = async () => {
      try {
        const [bookQuantitiesRes, requestStatusRes, booksPerCategoryRes, mostPopularRes] = await Promise.all([
          axiosInstance.get('/api/statistics/book-quantities'),
          axiosInstance.get('/api/statistics/request-overview'),
          axiosInstance.get('/api/statistics/books-per-category'),
          axiosInstance.get('/api/statistics/most-popular'),
        ]);

        setBookQuantities(bookQuantitiesRes.data);
        setRequestStatus(requestStatusRes.data);
        setBooksPerCategory(booksPerCategoryRes.data.booksPerCategory);
        setMostPopular(mostPopularRes.data);
      } catch (error) {
        console.error('Error fetching data:', error);
      }
    };

    fetchStats();
    fetchData();
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
        <h2 className="text-xl font-bold mb-4">Library Analytics</h2>
        
        <Row gutter={[16, 16]}>
          {/* Book Availability Overview */}
          <Col xs={24} lg={12}>
            {bookQuantities && (
              <Card title="Book Availability Overview">
                <ChartComponent
                  type="bar"
                  data={{
                    labels: ['Total Books', 'Available Books', 'Borrowed Books'],
                    datasets: [
                      {
                        label: 'Books',
                        data: [
                          bookQuantities.totalBooks,
                          bookQuantities.availableBooks,
                          bookQuantities.borrowedBooks,
                        ],
                        backgroundColor: ['#36A2EB', '#4BC0C0', '#FF6384'],
                      },
                    ],
                  }}
                  options={{
                    responsive: true,
                    maintainAspectRatio: true,
                    plugins: {
                      legend: { position: 'top' },
                    },
                  }}
                />
              </Card>
            )}
          </Col>

          {/* Number of Books per Category - Now in second position */}
          <Col xs={24} lg={12}>
            {booksPerCategory && (
              <Card title="Number of Books per Category">
                <ChartComponent
                  type="bar"
                  data={{
                    labels: booksPerCategory.map((category) => category.categoryName),
                    datasets: [
                      {
                        label: 'Books',
                        data: booksPerCategory.map((category) => category.bookCount),
                        backgroundColor: '#36A2EB',
                      },
                    ],
                  }}
                  options={{
                    responsive: true,
                    maintainAspectRatio: true,
                    plugins: {
                      legend: { position: 'top' },
                    },
                    indexAxis: 'y',
                  }}
                />
              </Card>
            )}
          </Col>

          {/* Borrowing Request Status - Now in third position */}
          <Col xs={24} lg={12}>
            {requestStatus && (
              <Card title="Borrowing Request Status">
                <ChartComponent
                  type="pie"
                  data={{
                    labels: ['Approved', 'Pending', 'Rejected', 'Returned'],
                    datasets: [
                      {
                        label: 'Requests',
                        data: [
                          requestStatus.approvedRequestCount,
                          requestStatus.pendingRequestCount,
                          requestStatus.rejectedRequestCount,
                          requestStatus.returnedRequestCount,
                        ],
                        backgroundColor: ['#4BC0C0', '#FFCE56', '#FF6384', '#9966FF'],
                      },
                    ],
                  }}
                  options={{
                    responsive: true,
                    maintainAspectRatio: true,
                    plugins: {
                      legend: { position: 'top' },
                    },
                  }}
                />
              </Card>
            )}
          </Col>

          {/* Most Popular Book and Category */}
          <Col xs={24} lg={12}>
            {mostPopular && (
              <Card title="Most Popular">
                <div style={{ height: '100%', display: 'flex', flexDirection: 'column', justifyContent: 'center' }}>
                  <div className="p-4 text-center">
                    <h3 className="text-lg font-semibold mb-2">Most Popular Book</h3>
                    <p className="text-xl mb-4">{mostPopular.titleAuthor}</p>
                    <h3 className="text-lg font-semibold mb-2">Most Popular Category</h3>
                    <p className="text-xl">{mostPopular.categoryName}</p>
                  </div>
                </div>
              </Card>
            )}
          </Col>
        </Row>
      </div>
    </div>
  );
};

export default AdminHome;
