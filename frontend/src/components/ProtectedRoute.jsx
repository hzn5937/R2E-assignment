// src/components/ProtectedRoute.jsx
import React from 'react';
import { Navigate, Outlet } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';

export const ProtectedRoute = ({ allowedRoles }) => {
  const { user } = useAuth();
  
  // Check if user exists instead of using isAuthenticated
  if (!user) {
    return <Navigate to="/login" replace />;
  }
  
  if (allowedRoles && !allowedRoles.includes(user.role.toLowerCase())) {
    // Redirect admin to admin dashboard, users to user dashboard
    return <Navigate to={user.role.toLowerCase() === 'admin' ? '/admin' : '/'} replace />;
  }
  
  return <Outlet />;
};

export default ProtectedRoute;