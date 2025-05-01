// Example in src/components/header.jsx
import React from 'react';
import { MenuUnfoldOutlined, MenuFoldOutlined, LogoutOutlined } from '@ant-design/icons';
import { Button } from 'antd';
import {useAuth} from '../context/AuthContext';
import { useNavigate } from 'react-router-dom';

const Header = ({ collapsed, toggle }) => {
  const { logout } = useAuth();
  const navigate = useNavigate();

  const handleLogout = () => {
    logout();
    navigate('/login'); // Redirect to login after logout
  };

  return (
    <header className="flex items-center justify-between bg-blue-500 text-white shadow px-4 h-16">
      <div className="flex items-center">
        {React.createElement(collapsed ? MenuUnfoldOutlined : MenuFoldOutlined, {
          className: 'text-xl cursor-pointer',
          onClick: toggle,
        })}
        <h2 className="ml-3 pt-1 text-lg font-semibold">Library Management System</h2>
      </div>
      <Button
          type="primary"
          danger
          icon={<LogoutOutlined />}
          onClick={handleLogout}
      >
          Logout
      </Button>
    </header>
  );
};

export default Header;