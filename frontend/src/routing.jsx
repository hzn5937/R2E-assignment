import React from 'react';
import { Routes, Route } from 'react-router-dom';
import Home from './pages/user/Home';
import MyBorrowing from './pages/user/MyBorrowing';
import LoginPage from './pages/user/LoginPage'; // Import Login Page
import ProtectedRoute from './components/ProtectedRoute'; // Import ProtectedRoute
import AppLayout from './layout/app_layout'; // Assuming AppLayout contains Navbar/Header

const Routing = () => (
  <Routes>
    <Route path="/login" element={<LoginPage />} />

    {/* Wrap protected routes within AppLayout */}
    <Route element={<AppLayout />}>
        <Route
            path="/"
            element={
                <ProtectedRoute>
                    <Home />
                </ProtectedRoute>
            }
        />
        <Route
            path="/borrowing"
            element={
                <ProtectedRoute>
                    <MyBorrowing />
                </ProtectedRoute>
            }
        />
         {/* Add other protected routes here */}
    </Route>

    {/* You might have other public routes outside AppLayout if needed */}
  </Routes>
);