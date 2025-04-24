import React, { useState } from 'react';
import { Layout } from 'antd';
import SiderMenu from '../components/navbar';
import Header from '../components/header';
import Routing from '../routing';

const { Sider, Content } = Layout;

const AppLayout = () => {
  const [collapsed, setCollapsed] = useState(false);
  const toggle = () => setCollapsed(!collapsed);

  return (
    <Layout className="h-screen">
      <Sider trigger={null} collapsible collapsed={collapsed}>
        <SiderMenu />
      </Sider>
      <Layout>
        <Header collapsed={collapsed} toggle={toggle} />
        <Content className="m-6 p-6 bg-white rounded shadow h-full overflow-auto">
            <Routing />
        </Content>
      </Layout>
    </Layout>
  );
};

export default AppLayout;
