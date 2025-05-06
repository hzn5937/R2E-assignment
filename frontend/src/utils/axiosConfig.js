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

export default axiosInstance;