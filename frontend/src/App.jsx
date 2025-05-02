// src/App.jsx
import React from 'react';
import { BrowserRouter, Routes, Route } from 'react-router-dom';
import AppLayout from './layout/app_layout';
import Home from './pages/user/Home';
import MyBorrowing from './pages/user/MyBorrowing';
import LoginPage from './pages/LoginPage';
import ProtectedRoute from './components/ProtectedRoute';

export default function App() {
  return (
    <BrowserRouter>
      <Routes>
        {/* 1️⃣  Public */}
        <Route path="/login" element={<LoginPage />} />

        {/* 2️⃣  Protected – everything here renders inside AppLayout */}
        <Route
          element={
            <ProtectedRoute>
              <AppLayout />
            </ProtectedRoute>
          }
        >
          <Route index element={<Home />} />           {/* “/”          */}
          <Route path="requests" element={<MyBorrowing />} />{/* “/requests” */}
          {/* add more private pages here */}
        </Route>
      </Routes>
    </BrowserRouter>
  );
}
