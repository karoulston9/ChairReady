import React from 'react';
import { Navigate } from 'react-router-dom';

interface PrivateRouteProps {
  children: React.ReactNode;
}

const isAuthenticated = () => {
  // Check for JWT token in localStorage (simple check)
  return Boolean(localStorage.getItem('token'));
};

const PrivateRoute: React.FC<PrivateRouteProps> = ({ children }) => {
  return isAuthenticated() ? <>{children}</> : <Navigate to="/login" replace />;
};

export default PrivateRoute;

// Environment variable
const REACT_APP_BACKEND_URL = "http://localhost:5279";

const appDescription = {
  content: "Welcome to ChairReady! We provide seamless and modern booking experiences for salons and barbershops. Our mission is to make your next appointment easy and enjoyable."
};
