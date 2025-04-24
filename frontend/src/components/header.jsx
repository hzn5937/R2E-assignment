import React from 'react';
import { MenuUnfoldOutlined, MenuFoldOutlined } from '@ant-design/icons';

const Header = ({ collapsed, toggle }) => {
  return (
    <header className="flex items-center bg-blue-500 text-white shadow px-4 h-16">
      {React.createElement(collapsed ? MenuUnfoldOutlined : MenuFoldOutlined, {
        className: 'text-xl cursor-pointer',
        onClick: toggle,
      })}
      <h2 className="ml-3 pt-2 text-lg font-semibold">Library Management System</h2>
    </header>
  );
};

export default Header;
