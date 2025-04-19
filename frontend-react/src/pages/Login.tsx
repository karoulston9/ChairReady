import React from 'react';

const BACKEND_URL = process.env.REACT_APP_BACKEND_URL || 'http://localhost:5279';

const Login: React.FC = () => {
  const handleLogin = (provider: 'google' | 'microsoft') => {
    window.location.href = `${BACKEND_URL}/api/auth/${provider}/login`;
  };

  return (
    <div className="login-container">
      <h2>Sign in to ChairReady</h2>
      <button className="login-btn google" onClick={() => handleLogin('google')}>
        Sign in with Google
      </button>
      <button className="login-btn microsoft" onClick={() => handleLogin('microsoft')}>
        Sign in with Microsoft
      </button>
    </div>
  );
};

export default Login;
