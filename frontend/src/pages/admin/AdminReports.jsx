import React, { useState, useEffect } from 'react';
import { Card, DatePicker, Button, Table, Alert, Space, Typography, Spin, Tabs, message } from 'antd';
import { DownloadOutlined } from '@ant-design/icons';
import axiosInstance from '../../utils/axiosConfig';
import moment from 'moment';

const { Title, Text } = Typography;
const { RangePicker } = DatePicker;

const AdminReports = () => {
  const [singleMonth, setSingleMonth] = useState(null);
  const [dateRange, setDateRange] = useState(null);
  const [singleMonthReport, setSingleMonthReport] = useState(null);
  const [rangeReports, setRangeReports] = useState([]);
  const [activeTab, setActiveTab] = useState('single');
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);
  const [apiSuccess, setApiSuccess] = useState(null); // Added success state

  // Add debugging log for component state
  useEffect(() => {
    console.log("AdminReports state:", {
      singleMonth,
      dateRange,
      singleMonthReport,
      rangeReports,
      activeTab,
      loading,
      error
    });
  }, [singleMonth, dateRange, singleMonthReport, rangeReports, activeTab, loading, error]);

  const fetchSingleMonthReport = async () => {
    if (!singleMonth) {
      setError('Please select a month');
      return;
    }

    setLoading(true);
    setError(null);

    try {
      const year = singleMonth.year();
      const month = singleMonth.month() + 1; // Moment months are 0-indexed

      console.log(`Fetching report for ${year}-${month}`);
      const response = await axiosInstance.get(
        `/api/statistics/monthly-report?year=${year}&month=${month}`
      );
      console.log('API response:', response.data);
      setSingleMonthReport(response.data);
    } catch (err) {
      console.error('Error fetching report:', err);
      setError('Failed to fetch report: ' + (err.response?.data || err.message));
    } finally {
      setLoading(false);
    }
  };

  const fetchRangeReports = async () => {
    if (!dateRange || !dateRange[0] || !dateRange[1]) {
      setError('Please select a date range');
      return;
    }

    setLoading(true);
    setError(null);

    try {
      const startYear = dateRange[0].year();
      const startMonth = dateRange[0].month() + 1;
      const endYear = dateRange[1].year();
      const endMonth = dateRange[1].month() + 1;

      console.log(`Fetching reports from ${startYear}-${startMonth} to ${endYear}-${endMonth}`);
      const response = await axiosInstance.get(
        `/api/statistics/monthly-reports-range?startYear=${startYear}&startMonth=${startMonth}&endYear=${endYear}&endMonth=${endMonth}`
      );
      console.log('API response (range):', response.data);
      setRangeReports(response.data);
    } catch (err) {
      console.error('Error fetching reports:', err);
      setError('Failed to fetch reports: ' + (err.response?.data || err.message));
    } finally {
      setLoading(false);
    }
  };

  const downloadSingleMonthExcel = async () => {
    if (!singleMonth) {
      setError('Please select a month');
      return;
    }

    setLoading(true);
    try {
      const year = singleMonth.year();
      const month = singleMonth.month() + 1;
      
      console.log(`Downloading report for ${year}-${month}`);
      
      // Use axios to handle the download with proper authentication
      const response = await axiosInstance.get(
        `/api/statistics/export/monthly-report?year=${year}&month=${month}`,
        { 
          responseType: 'blob', // Important for file downloads
        }
      );
      
      console.log('Excel download response:', response);
      
      // Create a download link and trigger it
      const url = window.URL.createObjectURL(new Blob([response.data]));
      const link = document.createElement('a');
      link.href = url;
      link.setAttribute('download', `Monthly_Report_${year}_${month}.xlsx`);
      document.body.appendChild(link);
      link.click();
      document.body.removeChild(link);
      window.URL.revokeObjectURL(url);
      
      // Set success alert instead of message
      setApiSuccess({
        message: 'Report Downloaded Successfully',
        description: `Monthly report for ${moment(singleMonth).format('MMMM YYYY')} has been downloaded.`
      });
    } catch (err) {
      console.error('Error downloading report:', err);
      setError('Failed to download report: ' + (err.response?.data || err.message));
    } finally {
      setLoading(false);
    }
  };

  const downloadRangeExcel = async () => {
    if (!dateRange || !dateRange[0] || !dateRange[1]) {
      setError('Please select a date range');
      return;
    }

    setLoading(true);
    try {
      const startYear = dateRange[0].year();
      const startMonth = dateRange[0].month() + 1;
      const endYear = dateRange[1].year();
      const endMonth = dateRange[1].month() + 1;
      
      console.log(`Downloading reports from ${startYear}-${startMonth} to ${endYear}-${endMonth}`);
      
      // Use axios to handle the download with proper authentication
      const response = await axiosInstance.get(
        `/api/statistics/export/monthly-reports-range?startYear=${startYear}&startMonth=${startMonth}&endYear=${endYear}&endMonth=${endMonth}`,
        { 
          responseType: 'blob', // Important for file downloads
        }
      );
      
      console.log('Excel download response:', response);
      
      // Create a download link and trigger it
      const url = window.URL.createObjectURL(new Blob([response.data]));
      const link = document.createElement('a');
      link.href = url;
      link.setAttribute('download', `Monthly_Reports_${startYear}_${startMonth}_to_${endYear}_${endMonth}.xlsx`);
      document.body.appendChild(link);
      link.click();
      document.body.removeChild(link);
      window.URL.revokeObjectURL(url);
      
      // Set success alert instead of message
      setApiSuccess({
        message: 'Reports Downloaded Successfully',
        description: `Monthly reports from ${moment(dateRange[0]).format('MMMM YYYY')} to ${moment(dateRange[1]).format('MMMM YYYY')} have been downloaded.`
      });
    } catch (err) {
      console.error('Error downloading reports:', err);
      setError('Failed to download reports: ' + (err.response?.data || err.message));
    } finally {
      setLoading(false);
    }
  };

  const popularBooksColumns = [
    {
      title: 'Title',
      dataIndex: 'title',
      key: 'title',
    },
    {
      title: 'Author',
      dataIndex: 'author',
      key: 'author',
    },
    {
      title: 'Borrow Count',
      dataIndex: 'borrowCount',
      key: 'borrowCount',
    },
  ];

  const popularCategoriesColumns = [
    {
      title: 'Category',
      dataIndex: 'categoryName',
      key: 'categoryName',
    },
    {
      title: 'Borrow Count',
      dataIndex: 'borrowCount',
      key: 'borrowCount',
    },
  ];

  const summaryColumns = [
    {
      title: 'Month',
      dataIndex: 'month',
      key: 'month',
      render: (text) => moment(text).format('MMMM YYYY'),
    },
    {
      title: 'Total Requests',
      dataIndex: 'totalRequests',
      key: 'totalRequests',
    },
    {
      title: 'Approved',
      dataIndex: 'approvedRequests',
      key: 'approvedRequests',
    },
    {
      title: 'Pending',
      dataIndex: 'pendingRequests',
      key: 'pendingRequests',
    },
    {
      title: 'Rejected',
      dataIndex: 'rejectedRequests',
      key: 'rejectedRequests',
    },
    {
      title: 'Active Users',
      dataIndex: 'totalActiveUsers',
      key: 'totalActiveUsers',
    },
  ];

  const renderSingleMonthReport = () => {
    if (!singleMonthReport) {
      console.log("No single month report to render");
      return null;
    }

    console.log("Rendering single month report:", singleMonthReport);

    try {
      // Check if month field exists and is valid
      if (!singleMonthReport.month) {
        console.error("Missing or invalid 'month' field in report data");
        return (
          <Alert
            message="Invalid Report Data"
            description="The report data format is invalid. Missing month field."
            type="error"
            showIcon
          />
        );
      }

      return (
        <div className="space-y-6">
          <Title level={4}>
            Monthly Report: {moment(singleMonthReport.month).format('MMMM YYYY')}
          </Title>

          <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-4">
            <Card>
              <div className="text-center">
                <div className="text-lg font-bold">Total Requests</div>
                <div className="text-2xl">{singleMonthReport.totalRequests}</div>
              </div>
            </Card>
            <Card>
              <div className="text-center">
                <div className="text-lg font-bold">Approved</div>
                <div className="text-2xl">{singleMonthReport.approvedRequests}</div>
              </div>
            </Card>
            <Card>
              <div className="text-center">
                <div className="text-lg font-bold">Pending</div>
                <div className="text-2xl">{singleMonthReport.pendingRequests}</div>
              </div>
            </Card>
            <Card>
              <div className="text-center">
                <div className="text-lg font-bold">Rejected</div>
                <div className="text-2xl">{singleMonthReport.rejectedRequests}</div>
              </div>
            </Card>
          </div>

          <Card title="Active Users">
            <div className="text-center">
              <div className="text-3xl">{singleMonthReport.totalActiveUsers}</div>
              <div className="text-gray-500">Users who made requests this month</div>
            </div>
          </Card>

          <Card title="Most Popular Books">
            {!singleMonthReport.popularBooks || singleMonthReport.popularBooks.length === 0 ? (
              <div className="text-center text-gray-500">No data available</div>
            ) : (
              <Table
                dataSource={singleMonthReport.popularBooks}
                columns={popularBooksColumns}
                rowKey="bookId"
                pagination={false}
              />
            )}
          </Card>

          <Card title="Most Popular Categories">
            {!singleMonthReport.popularCategories || singleMonthReport.popularCategories.length === 0 ? (
              <div className="text-center text-gray-500">No data available</div>
            ) : (
              <Table
                dataSource={singleMonthReport.popularCategories}
                columns={popularCategoriesColumns}
                rowKey="categoryId"
                pagination={false}
              />
            )}
          </Card>

          <div className="flex justify-end">
            <Button
              type="primary"
              icon={<DownloadOutlined />}
              onClick={downloadSingleMonthExcel}
            >
              Download Excel Report
            </Button>
          </div>
        </div>
      );
    } catch (renderError) {
      console.error("Error rendering monthly report:", renderError);
      return (
        <Alert
          message="Error Rendering Report"
          description={`An error occurred while rendering the report: ${renderError.message}`}
          type="error"
          showIcon
        />
      );
    }
  };

  const renderRangeReports = () => {
    if (!rangeReports || rangeReports.length === 0) {
      console.log("No range reports to render");
      return null;
    }

    console.log("Rendering range reports:", rangeReports);

    try {
      // Check if first report has month field
      if (!rangeReports[0].month) {
        console.error("Missing or invalid 'month' field in report data");
        return (
          <Alert
            message="Invalid Report Data"
            description="The report data format is invalid. Missing month field."
            type="error"
            showIcon
          />
        );
      }

      return (
        <div className="space-y-6">
          <Title level={4}>
            Monthly Reports: {moment(rangeReports[0].month).format('MMM YYYY')} -{' '}
            {moment(rangeReports[rangeReports.length - 1].month).format('MMM YYYY')}
          </Title>

          <Card title="Summary">
            <Table
              dataSource={rangeReports}
              columns={summaryColumns}
              rowKey={(record) => record.month}
              pagination={false}
            />
          </Card>

          <div className="flex justify-end">
            <Button
              type="primary"
              icon={<DownloadOutlined />}
              onClick={downloadRangeExcel}
            >
              Download Excel Report
            </Button>
          </div>
        </div>
      );
    } catch (renderError) {
      console.error("Error rendering range reports:", renderError);
      return (
        <Alert
          message="Error Rendering Reports"
          description={`An error occurred while rendering the reports: ${renderError.message}`}
          type="error"
          showIcon
        />
      );
    }
  };

  const tabItems = [
    {
      key: 'single',
      label: 'Single Month Report',
      children: (
        <div className="space-y-6">
          <Card title="Select Month">
            <Space direction="vertical" size="middle" className="w-full">
              <DatePicker
                picker="month"
                value={singleMonth}
                onChange={setSingleMonth}
                style={{ width: '100%' }}
              />
              <Button type="primary" onClick={fetchSingleMonthReport} loading={loading}>
                Generate Report
              </Button>
            </Space>
          </Card>
          {error && <Alert type="error" message={error} />}
          {loading ? <div className="text-center py-8"><Spin size="large" /></div> : renderSingleMonthReport()}
        </div>
      ),
    },
    {
      key: 'range',
      label: 'Date Range Reports',
      children: (
        <div className="space-y-6">
          <Card title="Select Date Range">
            <Space direction="vertical" size="middle" className="w-full">
              <RangePicker
                picker="month"
                value={dateRange}
                onChange={setDateRange}
                style={{ width: '100%' }}
              />
              <Button type="primary" onClick={fetchRangeReports} loading={loading}>
                Generate Reports
              </Button>
            </Space>
          </Card>
          {error && <Alert type="error" message={error} />}
          {loading ? <div className="text-center py-8"><Spin size="large" /></div> : renderRangeReports()}
        </div>
      ),
    },
  ];

  return (
    <div className="admin-reports-page">
      <div className="flex justify-between items-center mb-6">
        <Title level={2}>Reports</Title>
      </div>

      {error && <Alert type="error" message={error} className="mb-4" />}

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

      <Tabs
        activeKey={activeTab}
        onChange={setActiveTab}
        items={tabItems}
      />
    </div>
  );
};

export default AdminReports;