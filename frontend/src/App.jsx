import React from 'react';
import NavBar from './components/navbar';
import Routing from './routing';
import { Routes, Route, Link } from 'react-router-dom';
import { Button } from 'antd';

const Home = () => <h2>Home Page</h2>;
const About = () => <h2>About Page</h2>;

function App() {
  return (
    <div style={{ padding: '2rem' }}>
      <NavBar />
      <Routing/>
    </div>
  );
}

export default App;
