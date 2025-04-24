import React from 'react';
import NavBar from './components/navbar';
import Routing from './routing';
import { Routes, Route, Link, BrowserRouter } from 'react-router-dom';
import { Button } from 'antd';
import AppLayout from './layout/app_layout';

const Home = () => <h2>Home Page</h2>;
const About = () => <h2>About Page</h2>;

function App() {
  return (
    <BrowserRouter>
      <AppLayout/>
    </BrowserRouter>
  );
}

export default App;
