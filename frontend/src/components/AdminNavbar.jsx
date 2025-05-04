import React from 'react';
import { Menu } from 'antd';
import { Link, useLocation } from 'react-router-dom';
import {
  UserOutlined,
  BookTwoTone,
  BarsOutlined,
  DatabaseTwoTone,
  HomeOutlined,
} from '@ant-design/icons';

const SiderMenu = () => {
  const location = useLocation();

  return (
    <>
      <div className="h-8 m-4 bg-white/30 rounded" />
      <Menu theme="dark" mode="inline" selectedKeys={[location.pathname]}>
        <Menu.Item key="/" icon={<HomeOutlined />}>
          <Link to="/admin">Home</Link>
        </Menu.Item>
        <Menu.Item key="/admin/books" icon={<BookTwoTone />}>
          <Link to="/admin/books">Books</Link>
        </Menu.Item>
        <Menu.Item key="/admin/categories" icon={<BarsOutlined />}>
          <Link to="/admin/categories">Categories</Link>
        </Menu.Item>
        <Menu.Item key="/admin/requests" icon={<DatabaseTwoTone />}>
          <Link to="/admin/requests">Requests</Link>
        </Menu.Item>
        <Menu.Item key="/admin/users" icon={<UserOutlined />}>
          <Link to="/admin/users">User Management</Link>
        </Menu.Item>
      </Menu>
    </>
  );
};

export default SiderMenu;
