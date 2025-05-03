// src/pages/LoginPage.jsx
import React, { useState } from 'react';
import { Form, Input, Button, Card, Alert } from 'antd';
import { UserOutlined, LockOutlined } from '@ant-design/icons';
import { useAuth } from '../context/AuthContext';
import { useNavigate, useLocation } from 'react-router-dom';

const LoginPage = () => {
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');
  const { login } = useAuth();
  const navigate = useNavigate();
  const location = useLocation();

  const from = location.state?.from?.pathname || "/"; // Redirect back after login

  const onFinish = async (values) => {
    setLoading(true);
    setError('');
    try {
      const result = await login(values.username, values.password);
      
      if (result.success) {
        // Use the user data returned directly from the login function
        const role = result.user?.role || '';
        const redirectPath = role.toLowerCase() === 'admin' ? '/admin' : '/';
        
        console.log('Login successful, redirecting to:', redirectPath);
        navigate(redirectPath, { replace: true });
      } else {
        console.log('Login failed:', result.error);
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
      <Card title="Login" className="w-full max-w-sm">
        {error && (
          <Alert 
            message={error} 
            type="error" 
            showIcon 
            className="mb-4" 
            style={{ marginBottom: '16px' }}
          />
        )}
        <Form
          name="login_form"
          initialValues={{ remember: true }}
          onFinish={onFinish}
        >
          {/* No error display here - moved outside form */}
          <Form.Item
            name="username"
            rules={[{ required: true, message: 'Please input your Username!' }]}
          >
            <Input prefix={<UserOutlined />} placeholder="Username" />
          </Form.Item>
          <Form.Item
            name="password"
            rules={[{ required: true, message: 'Please input your Password!' }]}
          >
            <Input.Password prefix={<LockOutlined />} placeholder="Password" />
          </Form.Item>
          <Form.Item>
            <Button type="primary" htmlType="submit" loading={loading} block>
              Log in
            </Button>
          </Form.Item>
        </Form>
      </Card>
    </div>
  );
};

export default LoginPage;