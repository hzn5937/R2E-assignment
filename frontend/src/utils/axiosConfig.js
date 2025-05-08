import axios from 'axios';

// Create main axios instance
const axiosInstance = axios.create({
  baseURL: 'https://localhost:7246',
  headers: {
    'Content-Type': 'application/json',
    'Accept': 'application/json',
  },
});

// Check if we have tokens in localStorage and set auth header accordingly
const authTokens = localStorage.getItem('authTokens');
if (authTokens) {
  const tokens = JSON.parse(authTokens);
  axiosInstance.defaults.headers.common['Authorization'] = `Bearer ${tokens.accessToken}`;
}

// Add a refreshing flag and promise to track and coordinate refresh operations
let isRefreshing = false;
let refreshPromise = null;
let failedQueue = [];

// Process the queue of failed requests
const processQueue = (error, token = null) => {
  failedQueue.forEach(prom => {
    if (error) {
      prom.reject(error);
    } else {
      prom.resolve(token);
    }
  });
  
  failedQueue = [];
};

// Add a response interceptor to handle token refresh
axiosInstance.interceptors.response.use(
  response => response,
  async error => {
    // Skip handling if no response or we're in the auth process
    if (!error.response || 
        error.config.url.includes('/api/authentication/login') ||
        error.config.url.includes('/api/authentication/refresh') ||
        error.config.url.includes('/api/authentication/logout')) {
      return Promise.reject(error);
    }
    
    // Handle 401 Unauthorized errors
    if (error.response.status === 401 && !error.config._retry) {
      error.config._retry = true;
      
      // If we're already refreshing, add this request to the queue
      if (isRefreshing) {
        return new Promise((resolve, reject) => {
          failedQueue.push({ resolve, reject });
        })
          .then(token => {
            error.config.headers['Authorization'] = `Bearer ${token}`;
            return axiosInstance(error.config);
          })
          .catch(err => {
            return Promise.reject(err);
          });
      }
      
      // Set refreshing flag
      isRefreshing = true;
      
      try {
        // Create the refresh token promise if it doesn't exist
        if (!refreshPromise) {
          refreshPromise = (async () => {
            try {
              // Get stored auth tokens
              const storedTokens = localStorage.getItem('authTokens');
              if (!storedTokens) {
                throw new Error('No tokens available');
              }
              
              const tokens = JSON.parse(storedTokens);
              
              // Call refresh token endpoint
              const response = await axios.post('https://localhost:7246/api/authentication/refresh', {
                accessToken: tokens.accessToken,
                refreshToken: tokens.refreshToken
              });
              
              const data = response.data;
              
              if (data && data.accessToken) {
                // Update tokens in localStorage
                const newTokens = {
                  accessToken: data.accessToken,
                  accessTokenExpires: data.accessTokenExpires,
                  refreshToken: data.refreshToken,
                  refreshTokenExpires: data.refreshTokenExpires
                };
                
                localStorage.setItem('authTokens', JSON.stringify(newTokens));
                
                // Update auth header for future requests
                axiosInstance.defaults.headers.common['Authorization'] = `Bearer ${data.accessToken}`;
                
                // Process all queued requests with the new token
                processQueue(null, data.accessToken);
                
                return data.accessToken;
              } else {
                throw new Error('Invalid refresh response');
              }
            } catch (err) {
              processQueue(err, null);
              throw err;
            } finally {
              isRefreshing = false;
              refreshPromise = null;
            }
          })();
        }
        
        // Wait for the refresh token operation to complete
        const newToken = await refreshPromise;
        
        // Update the failed request with new token and retry
        error.config.headers['Authorization'] = `Bearer ${newToken}`;
        return axiosInstance(error.config);
        
      } catch (refreshError) {
        console.error("Token refresh error:", refreshError);
        
        // Clear auth data on refresh failure
        localStorage.removeItem('authTokens');
        localStorage.removeItem('user');
        
        // Remove auth header
        delete axiosInstance.defaults.headers.common['Authorization'];
        
        // Redirect to login page if needed
        if (window.location.pathname !== '/login') {
          window.location.href = '/login';
        }
        
        return Promise.reject(error);
      }
    }
    
    return Promise.reject(error);
  }
);

export default axiosInstance;