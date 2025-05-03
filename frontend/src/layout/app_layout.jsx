import React, { useState } from 'react';
import { Layout } from 'antd';
import { Outlet } from 'react-router-dom'; // Import Outlet
import UserMenu from '../components/UserNavbar';
import AdminMenu from '../components/AdminNavbar'; 
import Header from '../components/header';
import { useAuth } from '../context/AuthContext'; // Import useAuth hook

const { Sider, Content } = Layout;

const AppLayout = () => {
  const [collapsed, setCollapsed] = useState(false);
  const toggle = () => setCollapsed(!collapsed);
  const { user } = useAuth(); // Get user from auth context

  // Determine which menu to show based on user role
  const renderMenu = () => {
    if (user && user.role === "Admin") {
      return <AdminMenu />;
    } else {
      return <UserMenu />;
    }
  };

  return (
    <Layout className="h-screen">
      <Sider trigger={null} collapsible collapsed={collapsed}>
        {renderMenu()}
      </Sider>
      <Layout>
        <Header collapsed={collapsed} toggle={toggle} />
        <Content className="m-6 p-6 bg-white rounded shadow h-full overflow-auto">
            <Outlet /> {/* Render nested routes here */}
        </Content>
      </Layout>
    </Layout>
  );
};

export default AppLayout;