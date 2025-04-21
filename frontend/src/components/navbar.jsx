import React from 'react';
import { Menu } from 'antd';
import { useNavigate, useLocation } from 'react-router-dom';

const NavBar = () => {
  const navigate = useNavigate();
  const location = useLocation();

    const menuItems = [
        {
            label: 'Home',
            key: '/',
        },
        {
            label: 'Post',
            key: '/post',
        },
    ];

  return (
        <div className='flex items-center px-4 py-2 shadow-md bg-white'>
            <div className="text-xl font-bold mr-8 text-blue-600"> My App</div>

            <Menu
                mode="horizontal"
                selectedKeys={[location.pathname]}
                onClick={({ key }) => navigate(key)}
                items={menuItems}
            />
        </div>
    );
};

export default NavBar;
