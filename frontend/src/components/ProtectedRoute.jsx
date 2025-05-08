// src/components/ProtectedRoute.jsx
import React from 'react';
import { Navigate, Outlet, useLocation } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';

const ProtectedRoute = ({ allowedRoles }) => {
  const { user, loading } = useAuth();
  const location = useLocation();

  // Show loading state while authentication is being initialized
  if (loading) {
    return <div className="flex justify-center items-center h-screen">Loading authentication...</div>;
  }

  // If not logged in, redirect to login with return URL
  if (!user) {
    return <Navigate to="/login" state={{ from: location }} replace />;
  }
  
  // If user doesn't have required role, redirect to appropriate dashboard
  if (allowedRoles && !allowedRoles.includes(user.role.toLowerCase())) {
    // Redirect admin to admin dashboard, users to user dashboard
    return <Navigate to={user.role.toLowerCase() === 'admin' ? '/admin' : '/'} replace />;
  }
  
  // User is authenticated and authorized
  return <Outlet />;
};

export default ProtectedRoute;