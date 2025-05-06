import React, { createContext, useContext, useState, useEffect, useCallback, useRef } from 'react';
import axios from 'axios';

// Base API URL
const API_URL = 'https://localhost:7246';

// Create a standalone axios instance for auth operations
const authAxios = axios.create({
  baseURL: API_URL,
  headers: {
    'Content-Type': 'application/json',
    'Accept': 'application/json',
  },
});

const AuthContext = createContext(null);

export const AuthProvider = ({ children }) => {
  // User state - contains user info (id, username, role)
  const [user, setUser] = useState(() => {
    const storedUser = localStorage.getItem('user');
    return storedUser ? JSON.parse(storedUser) : null;
  });
  
  // Auth tokens state - contains accessToken, refreshToken and expiry times
  const [authTokens, setAuthTokens] = useState(() => {
    const storedTokens = localStorage.getItem('authTokens');
    return storedTokens ? JSON.parse(storedTokens) : null;
  });
  
  // Loading state to track auth initialization
  const [loading, setLoading] = useState(true);
  
  // Add a refreshing flag reference to prevent multiple simultaneous refresh calls
  const isRefreshing = useRef(false);
  
  // Authentication initialization effect
  useEffect(() => {
    const initAuth = async () => {
      // If we have tokens, verify they're valid or refresh them
      if (authTokens) {
        try {
          // Check if token is expired or will expire in the next 30 seconds
          const expiryDate = new Date(authTokens.accessTokenExpires);
          const currentTime = Date.now();
          const bufferTime = 30 * 1000; // 30 seconds buffer
          
          if (currentTime >= expiryDate.getTime() - bufferTime) {
            console.log(`${currentTime} Token expired or expiring soon, attempting to refresh`);
            await refreshToken();
          } else {
            console.log("Token still valid until", expiryDate.toLocaleString());
            // Set default auth header for all future requests
            axios.defaults.headers.common['Authorization'] = `Bearer ${authTokens.accessToken}`;
          }
        } catch (error) {
          console.error("Auth initialization error:", error);
          // Clear invalid auth state
          setUser(null);
          setAuthTokens(null);
          localStorage.removeItem('user');
          localStorage.removeItem('authTokens');
        }
      }
      
      // Complete initialization
      setLoading(false);
    };
    
    initAuth();
  }, []);
  
  // Persist user to localStorage when it changes
  useEffect(() => {
    if (user) {
      localStorage.setItem('user', JSON.stringify(user));
    } else {
      localStorage.removeItem('user');
    }
  }, [user]);
  
  // Persist tokens to localStorage and set auth header when they change
  useEffect(() => {
    if (authTokens) {
      localStorage.setItem('authTokens', JSON.stringify(authTokens));
      axios.defaults.headers.common['Authorization'] = `Bearer ${authTokens.accessToken}`;
    } else {
      localStorage.removeItem('authTokens');
      delete axios.defaults.headers.common['Authorization'];
    }
  }, [authTokens]);
  
  // Login function
  const login = async (username, password) => {
    try {
      const response = await authAxios.post('/api/authentication/login', {
        username,
        password
      });
      
      const data = response.data;
      
      if (!data || !data.accessToken) {
        throw new Error('Invalid response format');
      }
      
      // Extract user data
      const userData = {
        id: data.id,
        username: data.username,
        role: data.role
      };
      
      // Extract token data
      const tokenData = {
        accessToken: data.accessToken,
        accessTokenExpires: data.accessTokenExpires,
        refreshToken: data.refreshToken,
        refreshTokenExpires: data.refreshTokenExpires
      };
      
      // Update state
      setUser(userData);
      setAuthTokens(tokenData);
      
      // Set auth header
      axios.defaults.headers.common['Authorization'] = `Bearer ${data.accessToken}`;
      
      return { success: true, user: userData };
    } catch (error) {
      console.error("Login error:", error);
      return {
        success: false,
        error: error.response?.data?.message || 'Authentication failed'
      };
    }
  };
  
  // Logout function - no longer depends on useNavigate
  const logout = useCallback(async () => {
    if (authTokens) {
      try {
        // Best effort to notify backend about logout
        await authAxios.post('/api/authentication/logout', {}, {
          headers: {
            'Authorization': `Bearer ${authTokens.accessToken}`
          }
        });
      } catch (error) {
        console.error("Logout API error:", error);
        // Continue with client-side logout even if API call fails
      }
    }
    
    // Clear auth state
    setUser(null);
    setAuthTokens(null);
    
    // Remove from localStorage
    localStorage.removeItem('user');
    localStorage.removeItem('authTokens');
    
    // Clear auth header
    delete axios.defaults.headers.common['Authorization'];
    
    // We no longer navigate here - this will be handled in components
  }, [authTokens]);
  
  // Refresh token function with locking mechanism to prevent multiple calls
  const refreshToken = useCallback(async () => {
    // If already refreshing or no tokens available, exit early
    if (isRefreshing.current || !authTokens) {
      console.log(isRefreshing.current ? "Token refresh already in progress" : "No refresh token available");
      if (!authTokens) {
        logout();
        return false;
      }
      // Return current refresh status to avoid redundant refreshes
      return !isRefreshing.current;
    }
    
    // Set refreshing flag to true
    isRefreshing.current = true;
    
    try {
      console.log("Making refresh token API call");
      const response = await authAxios.post('/api/authentication/refresh', {
        accessToken: authTokens.accessToken,
        refreshToken: authTokens.refreshToken
      });
      
      const data = response.data;
      
      if (!data || !data.accessToken) {
        throw new Error('Invalid refresh response format');
      }
      
      // Extract token data
      const tokenData = {
        accessToken: data.accessToken,
        accessTokenExpires: data.accessTokenExpires,
        refreshToken: data.refreshToken,
        refreshTokenExpires: data.refreshTokenExpires
      };
      
      // Update user info if provided in the response
      if (data.id && data.username && data.role) {
        setUser({
          id: data.id,
          username: data.username,
          role: data.role
        });
      }
      
      // Update tokens
      setAuthTokens(tokenData);
      
      // Update auth header
      axios.defaults.headers.common['Authorization'] = `Bearer ${data.accessToken}`;
      
      console.log("Token refresh successful");
      isRefreshing.current = false;
      return true;
    } catch (error) {
      console.error("Token refresh error:", error);
      logout();
      isRefreshing.current = false;
      return false;
    }
  }, [authTokens, logout]);
  
  // Set up axios interceptor to handle 401 errors
  useEffect(() => {
    const responseInterceptor = axios.interceptors.response.use(
      response => response,
      async error => {
        // Skip handling if no response or we're in the login/refresh process
        if (!error.response || 
            error.config.url.includes('/api/authentication/login') ||
            error.config.url.includes('/api/authentication/refresh') ||
            error.config.url.includes('/api/authentication/logout')) {
          return Promise.reject(error);
        }
        
        // Handle 401 Unauthorized errors
        if (error.response.status === 401 && !error.config._retry) {
          error.config._retry = true;
          
          try {
            // Wait for any ongoing refresh to complete or initiate a new one
            const success = await refreshToken();
            
            if (success) {
              // Update the authorization header with the new token
              error.config.headers['Authorization'] = `Bearer ${authTokens.accessToken}`;
              // Retry the original request
              return axios(error.config);
            }
          } catch (refreshError) {
            console.error("Error in refresh process:", refreshError);
            // Logout if refresh fails
            logout();
            return Promise.reject(refreshError);
          }
        }
        
        return Promise.reject(error);
      }
    );
    
    // Clean up interceptor on unmount
    return () => {
      axios.interceptors.response.eject(responseInterceptor);
    };
  }, [authTokens, refreshToken, logout]);
  
  // Authentication context value
  const contextValue = {
    user,
    authTokens,
    login,
    logout,
    loading,
    refreshToken
  };
  
  return (
    <AuthContext.Provider value={contextValue}>
      {children}
    </AuthContext.Provider>
  );
};

// Custom hook to use auth context
export const useAuth = () => {
  const context = useContext(AuthContext);
  if (!context) {
    throw new Error('useAuth must be used within an AuthProvider');
  }
  return context;
};

export default AuthContext;
