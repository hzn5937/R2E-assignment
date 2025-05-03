import React from 'react';
import { Menu } from 'antd';
import { Link, useLocation } from 'react-router-dom';
import {
  UserOutlined,
  DatabaseTwoTone,
} from '@ant-design/icons';

const SiderMenu = () => {
  const location = useLocation();

  return (
    <>
      <div className="h-8 m-4 bg-white/30 rounded" />
      <Menu theme="dark" mode="inline" selectedKeys={[location.pathname]}>
        <Menu.Item key="/" icon={<UserOutlined />}>
          <Link to="/">Home</Link>
        </Menu.Item>
        <Menu.Item key="/requests" icon={<DatabaseTwoTone />}>
          <Link to="/requests">My Borrowing</Link>
        </Menu.Item>
      </Menu>
    </>
  );
};

export default SiderMenu;
