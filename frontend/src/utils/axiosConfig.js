// src/utils/axiosConfig.js
import axios from 'axios';

const axiosInstance = axios.create({
  baseURL: 'https://localhost:7246', // Your API base URL
  headers: {
    'Content-Type': 'application/json',
    'Accept': 'application/json',
  },
});

// Request interceptor - adds auth token to requests
axiosInstance.interceptors.request.use(
  config => {
    const storedTokens = localStorage.getItem('authTokens');
    if (storedTokens) {
      const tokens = JSON.parse(storedTokens);
      if (tokens?.accessToken) {
        config.headers['Authorization'] = `Bearer ${tokens.accessToken}`;
      }
    }
    return config;
  },
  error => {
    return Promise.reject(error);
  }
);

// Response interceptor - handles token refresh on 401 errors
axiosInstance.interceptors.response.use(
  response => {
    return response; // Pass through successful responses
  },
  async error => {
    const originalRequest = error.config;
    const storedTokens = localStorage.getItem('authTokens');
    const tokens = storedTokens ? JSON.parse(storedTokens) : null;

    // Check if it's a 401 error, not for the refresh endpoint itself,
    // and that we haven't already retried
    if (
      error.response?.status === 401 && 
      originalRequest.url !== '/api/authentication/refresh' && 
      !originalRequest._retry
    ) {
      originalRequest._retry = true; // Mark that we've retried

      if (tokens?.refreshToken) {
        try {
          console.log("Attempting to refresh token...");
          
          // Use correct payload structure for refresh
          const refreshResponse = await axios.post(
            '/api/authentication/refresh', 
            {
              accessToken: tokens.accessToken,
              refreshToken: tokens.refreshToken,
            }, 
            { 
              baseURL: axiosInstance.defaults.baseURL 
            }
          );

          // Extract token data using correct property names
          const {
            accessToken,
            accessTokenExpires,
            refreshToken,
            refreshTokenExpires
          } = refreshResponse.data;

          // Store new tokens
          const newTokens = {
            accessToken,
            accessTokenExpires,
            refreshToken,
            refreshTokenExpires
          };

          localStorage.setItem('authTokens', JSON.stringify(newTokens));

          // Update headers
          axiosInstance.defaults.headers['Authorization'] = `Bearer ${accessToken}`;
          originalRequest.headers['Authorization'] = `Bearer ${accessToken}`;

          console.log("Token refreshed successfully, retrying original request.");
          return axiosInstance(originalRequest); // Retry original request with new token
        } catch (refreshError) {
          console.error("Token refresh failed:", refreshError);
          // Logout the user if refresh fails
          localStorage.removeItem('authTokens');
          // Redirect to login page
          window.location.href = '/login';
          return Promise.reject(refreshError);
        }
      } else {
        console.error("No refresh token available.");
        // Logout the user if no refresh token
        localStorage.removeItem('authTokens');
        window.location.href = '/login';
      }
    }

    // For other errors or failed refresh, reject the promise
    return Promise.reject(error);
  }
);

export default axiosInstance;