import React from 'react';
import { Routes, Route } from 'react-router-dom';
import Home from './pages/home';
import Post from './pages/post';

const Routing = () => (
  <Routes>
    <Route path="/" element={<Home />} />
    <Route path="/borrowing" element={<Post />} />
  </Routes>
);

export default Routing;
