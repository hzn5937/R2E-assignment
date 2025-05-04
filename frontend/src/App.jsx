import React from 'react';
import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom';
import { AuthProvider } from './context/AuthContext';
import ProtectedRoute from './components/ProtectedRoute';

// Single shared layout
import AppLayout from './layout/app_layout';

// Public Pages
import LoginPage from './pages/LoginPage';

// User Pages
import UserHome from './pages/user/UserHome';
import MyBorrowing from './pages/user/Request/MyBorrowing';
import RequestDetailPage from './pages/user/Request/RequestDetailPage';

// Admin Pages
import AdminHome from './pages/admin/AdminHome';
import AdminBooks from './pages/admin/AdminBooks'; // Import AdminBooks component
import AdminCategories from './pages/admin/AdminCategories'; // Import AdminCategories component
import AdminRequests from './pages/admin/AdminRequests'; // Import AdminRequests component
import AdminUsers from './pages/admin/AdminUsers'; // Import AdminUsers component

function App() {
  return (
    <Router>
      <AuthProvider>
        <Routes>
          {/* Public Routes */}
          <Route path="/login" element={<LoginPage />} />
          
          {/* Admin Routes - using shared layout */}
          <Route element={<ProtectedRoute allowedRoles={['admin']} />}>
            <Route element={<AppLayout />}>
              <Route path="/admin" element={<AdminHome />} />
              <Route path="/admin/books" element={<AdminBooks />} /> {/* Add AdminBooks route */}
              <Route path="/admin/categories" element={<AdminCategories />} /> {/* Add AdminCategories route */}
              <Route path="/admin/requests" element={<AdminRequests />} /> {/* Add AdminRequests route */}
              <Route path="/admin/users" element={<AdminUsers />} /> {/* Add AdminUsers route */}
              {/* Add more admin routes as they become available */}
            </Route>
          </Route>
          
          {/* User Routes - using same shared layout */}
          <Route element={<ProtectedRoute allowedRoles={['user']} />}>
            <Route element={<AppLayout />}>
              <Route path="/" element={<UserHome />} />
              <Route path="/requests" element={<MyBorrowing />} />
              <Route path="/requests/:id" element={<RequestDetailPage />} />
              <Route path="/request/:id" element={<RequestDetailPage />} /> {/* Added this as a fallback for existing links */}
              {/* Add more user routes as they become available */}
            </Route>
          </Route>
          
          {/* Default redirect */}
          <Route path="*" element={<Navigate to="/" replace />} />
        </Routes>
      </AuthProvider>
    </Router>
  );
}

export default App;
