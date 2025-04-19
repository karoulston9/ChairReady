import React, { useEffect, useState } from 'react';

const BACKEND_URL = process.env.REACT_APP_BACKEND_URL || 'http://localhost:5279';

const Home: React.FC = () => {
  const [about, setAbout] = useState<string>('');
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    fetch(`${BACKEND_URL}/api/aboutus`)
      .then(res => {
        if (!res.ok) throw new Error('Failed to fetch About Us');
        return res.json();
      })
      .then(data => {
        setAbout(data?.content || '');
        setLoading(false);
      })
      .catch(err => {
        setError('Could not load About Us content.');
        setLoading(false);
      });
  }, []);

  return (
    <div className="page home-page">
      <h1>About Us</h1>
      {loading ? (
        <p>Loading...</p>
      ) : error ? (
        <p style={{ color: 'red' }}>{error}</p>
      ) : (
        <p>{about}</p>
      )}
    </div>
  );
};

export default Home;
