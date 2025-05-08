// src/pages/LoginPage.jsx
import React, { useState, useEffect } from 'react';
import { Form, Input, Button, Card, Alert } from 'antd';
import { UserOutlined, LockOutlined } from '@ant-design/icons';
import { useAuth } from '../context/AuthContext';
import { useNavigate, useLocation } from 'react-router-dom';

const LoginPage = () => {
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');
  const { login, user } = useAuth();
  const navigate = useNavigate();
  const location = useLocation();

  // Get the path the user was trying to access
  const from = location.state?.from?.pathname || '/';

  // If already logged in, redirect to appropriate dashboard
  useEffect(() => {
    if (user) {
      const redirectPath = user.role.toLowerCase() === 'admin' ? '/admin' : '/';
      navigate(redirectPath, { replace: true });
    }
  }, [user, navigate]);

  const onFinish = async (values) => {
    setLoading(true);
    setError('');
    try {
      const result = await login(values.username, values.password);
      
      if (result.success) {
        // Redirect user to the page they were trying to access or their dashboard
        const redirectPath = result.user?.role?.toLowerCase() === 'admin' ? '/admin' : from;
        navigate(redirectPath, { replace: true });
      } else {
        setError(result.error || 'Login failed. Please check your credentials.');
      }
    } catch (err) {
      console.error('Login error:', err);
      setError('An error occurred during login. Please try again.');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="flex justify-center items-center bg-gray-100 h-screen pb-32">
      <Card title="Sign In" className="w-full max-w-sm shadow-lg">
        {error && (
          <Alert 
            message={error} 
            type="error" 
            showIcon 
            className="mb-4"
          />
        )}
        <Form
          name="login_form"
          initialValues={{ remember: true }}
          onFinish={onFinish}
          layout="vertical"
        >
          <Form.Item
            name="username"
            rules={[{ required: true, message: 'Please input your Username!' }]}
          >
            <Input 
              prefix={<UserOutlined />} 
              placeholder="Username" 
              size="large"
            />
          </Form.Item>
          <Form.Item
            name="password"
            rules={[{ required: true, message: 'Please input your Password!' }]}
          >
            <Input.Password 
              prefix={<LockOutlined />} 
              placeholder="Password" 
              size="large"
            />
          </Form.Item>
          <Form.Item>
            <Button 
              type="primary" 
              htmlType="submit" 
              loading={loading} 
              block
              size="large"
            >
              Log in
            </Button>
          </Form.Item>
        </Form>
      </Card>
    </div>
  );
};

export default LoginPage;