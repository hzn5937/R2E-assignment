// src/context/AuthContext.jsx
import React, {
  createContext,
  useContext,
  useState,
  useEffect,
  useCallback,
  useRef,
} from 'react';
import axiosInstance from '../utils/axiosConfig';

const AuthContext = createContext(null);

export const AuthProvider = ({ children }) => {
  /* ----------  State ---------- */
  const [user, setUser] = useState(() => {
    const stored = localStorage.getItem('user');
    return stored ? JSON.parse(stored) : null;
  });

  const [tokens, _setTokens] = useState(() => {
    const stored = localStorage.getItem('authTokens');
    return stored ? JSON.parse(stored) : null;
  });

  // keep a ref to avoid re-adding the interceptor on every token change
  const tokensRef = useRef(tokens);
  const setTokens = (t) => {
    tokensRef.current = typeof t === 'function' ? t(tokensRef.current) : t;
    _setTokens(tokensRef.current);
  };

  const [loading, setLoading] = useState(true);

  /* ----------  Persist tokens & header ---------- */
  useEffect(() => {
    if (tokensRef.current) {
      localStorage.setItem('authTokens', JSON.stringify(tokensRef.current));
      axiosInstance.defaults.headers.common.Authorization =
        `Bearer ${tokensRef.current.accessToken}`;
    } else {
      localStorage.removeItem('authTokens');
      delete axiosInstance.defaults.headers.common.Authorization;
    }
  }, [tokens]); // still re-runs when the state object changes

  /* ----------  Persist user ---------- */
  useEffect(() => {
    if (user) {
      localStorage.setItem('user', JSON.stringify(user));
    } else {
      localStorage.removeItem('user');
    }
  }, [user]);

  /* ----------  Helper: refresh access token ---------- */
  const refreshAccessToken = useCallback(async () => {
    const curTokens = tokensRef.current;
    if (!curTokens) return null;

    try {
      const res = await axiosInstance.post('/api/authentication/refresh', {
        accessToken: curTokens.accessToken,
        refreshToken: curTokens.refreshToken,
      });

      const {
        accessToken,
        accessTokenExpires,
        refreshToken,
        refreshTokenExpires,
      } = res.data;

      setTokens({
        accessToken,
        accessTokenExpires,
        refreshToken,
        refreshTokenExpires,
      });

      // patch defaults immediately to close the race window
      axiosInstance.defaults.headers.common.Authorization = `Bearer ${accessToken}`;
      return accessToken;
    } catch (err) {
      logout(); // force logout if refresh fails
      return null;
    }
  }, []); // no deps – reads latest tokens from the ref

  /* ----------  Axios interceptor: auto-refresh expired tokens ---------- */
  useEffect(() => {
    const id = axiosInstance.interceptors.request.use(async (config) => {
      const curTokens = tokensRef.current;
      if (!curTokens) return config;

      const expiry =
        Number(curTokens.accessTokenExpires) > 1e12
          ? Number(curTokens.accessTokenExpires)       // already ms
          : Number(curTokens.accessTokenExpires) * 1000; // seconds → ms

      if (Date.now() >= expiry) {
        const newAccess = await refreshAccessToken();
        if (newAccess) {
          axiosInstance.defaults.headers.common.Authorization = `Bearer ${newAccess}`;
          config.headers.Authorization = `Bearer ${newAccess}`;
        }
      }
      return config;
    });

    return () => axiosInstance.interceptors.request.eject(id);
  }, [refreshAccessToken]); // stable because refreshAccessToken has empty deps

  /* ----------  Initial bootstrap ---------- */
  useEffect(() => {
    const init = async () => {
      const curTokens = tokensRef.current;
      if (curTokens) {
        const expiry =
          Number(curTokens.accessTokenExpires) > 1e12
            ? Number(curTokens.accessTokenExpires)
            : Number(curTokens.accessTokenExpires) * 1000;

        if (Date.now() >= expiry) {
          await refreshAccessToken();
        } else {
          axiosInstance.defaults.headers.common.Authorization =
            `Bearer ${curTokens.accessToken}`;
        }
      }
      setLoading(false);
    };
    init();
  }, [refreshAccessToken]);

  /* ----------  Auth actions ---------- */
  const login = async (username, password) => {
    try {
      const res = await axiosInstance.post('/api/authentication/login', {
        username,
        password,
      });

      const {
        id,
        username: serverUsername,
        role,
        accessToken,
        accessTokenExpires,
        refreshToken,
        refreshTokenExpires,
      } = res.data;

      setUser({ id, username: serverUsername, role });
      setTokens({
        accessToken,
        accessTokenExpires,
        refreshToken,
        refreshTokenExpires,
      });

      return true;
    } catch (err) {
      console.error('Login failed:', err);
      logout();
      return false;
    }
  };

  const logout = () => {
    setUser(null);
    setTokens(null);
  };

  /* ----------  Context value ---------- */
  const value = {
    user,
    tokens,
    login,
    logout,
    loading,
  };

  return (
    <AuthContext.Provider value={value}>
      {loading ? <div>Loading...</div> : children}
    </AuthContext.Provider>
  );
};

/* Convenience hook */
export const useAuth = () => useContext(AuthContext);

export default AuthContext;
