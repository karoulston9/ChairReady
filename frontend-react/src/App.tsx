import React from 'react';
import { BrowserRouter as Router, Routes, Route, Link } from 'react-router-dom';
import Home from './pages/Home';
import Locations from './pages/Locations';
import Booking from './pages/Booking';
import AdminPortal from './pages/AdminPortal';
import Login from './pages/Login';
import PrivateRoute from './PrivateRoute';
import './App.css';

function App() {
  return (
    <Router>
      <nav className="navbar">
        <div className="navbar-logo">ChairReady</div>
        <ul className="navbar-links">
          <li><Link to="/">About Us</Link></li>
          <li><Link to="/locations">Locations</Link></li>
          <li><Link to="/booking">Booking</Link></li>
          <li><Link to="/admin">Admin Portal</Link></li>
        </ul>
      </nav>
      <div className="main-content">
        <Routes>
          <Route path="/" element={<Home />} />
          <Route path="/locations" element={<Locations />} />
          <Route path="/booking" element={<Booking />} />
          <Route path="/admin" element={
            <PrivateRoute>
              <AdminPortal />
            </PrivateRoute>
          } />
          <Route path="/login" element={<Login />} />
        </Routes>
      </div>
    </Router>
  );
}

export default App;
