import React from 'react';
import { Routes, Route } from 'react-router-dom';
import Home from './pages/user/Home';
import MyBorrowing from './pages/user/MyBorrowing';
import LoginPage from './pages/user/LoginPage'; // Import Login Page
import ProtectedRoute from './components/ProtectedRoute'; // Import ProtectedRoute
import AppLayout from './layout/app_layout'; // Assuming AppLayout contains Navbar/Header
import AdminHome from './pages/admin/AdminHome';
import AdminBooks from './pages/admin/AdminBooks'; // Import the new AdminBooks component
import AdminCategories from './pages/admin/AdminCategories'; // Import the new AdminCategories component
import AdminRequests from './pages/admin/AdminRequests'; // Import the new AdminRequests component
import AdminUsers from './pages/admin/AdminUsers'; // Import the new AdminUsers component
import AdminReports from './pages/admin/AdminReports'; // Import the new AdminReports component

const Routing = () => (
  <Routes>
    <Route path="/login" element={<LoginPage />} />

    {/* Wrap protected routes within AppLayout */}
    <Route element={<AppLayout />}>
        {/* User Routes */}
        <Route
            path="/"
            element={
                <ProtectedRoute allowedRoles={['user']}>
                    <Home />
                </ProtectedRoute>
            }
        />
        <Route
            path="/borrowing"
            element={
                <ProtectedRoute allowedRoles={['user']}>
                    <MyBorrowing />
                </ProtectedRoute>
            }
        />    {/* You might have other public routes outside AppLayout if needed */}
        
        {/* Admin Routes */}        
        <Route
            path="/admin"
            element={
                <ProtectedRoute allowedRoles={['admin']}>
                    <AdminHome />
                </ProtectedRoute>
            }
        />
        <Route
            path="/admin/books"
            element={
                <ProtectedRoute allowedRoles={['admin']}>
                    <AdminBooks />
                </ProtectedRoute>
            }
        />
        <Route
            path="/admin/categories"
            element={
                <ProtectedRoute allowedRoles={['admin']}>
                    <AdminCategories />
                </ProtectedRoute>
            }
        />
        <Route
            path="/admin/requests"
            element={
                <ProtectedRoute allowedRoles={['admin']}>
                    <AdminRequests />
                </ProtectedRoute>
            }
        />
        <Route
            path="/admin/users"
            element={
                <ProtectedRoute allowedRoles={['admin']}>
                    <AdminUsers />
                </ProtectedRoute>
            }
        />
        <Route
            path="/admin/reports"
            element={
                <ProtectedRoute allowedRoles={['admin']}>
                    <AdminReports />
                </ProtectedRoute>
            }
        />
    </Route>

    {/* You might have other public routes outside AppLayout if needed */}
  </Routes>
);

export default Routing;