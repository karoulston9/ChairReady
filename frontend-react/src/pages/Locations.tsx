import React, { useEffect, useState } from 'react';

const BACKEND_URL = process.env.REACT_APP_BACKEND_URL || 'http://localhost:5279';

interface Location {
  id: number;
  name: string;
  address: string;
  phone: string;
}

const Locations: React.FC = () => {
  const [locations, setLocations] = useState<Location[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    fetch(`${BACKEND_URL}/api/locations`)
      .then(res => {
        if (!res.ok) throw new Error('Failed to fetch locations');
        return res.json();
      })
      .then(data => {
        setLocations(data);
        setLoading(false);
      })
      .catch(() => {
        setError('Could not load locations.');
        setLoading(false);
      });
  }, []);

  return (
    <div className="page locations-page">
      <h1>Our Locations</h1>
      <p>Find a ChairReady partner salon or barbershop near you. We are expanding to new locations regularly!</p>
      {loading ? (
        <p>Loading...</p>
      ) : error ? (
        <p style={{ color: 'red' }}>{error}</p>
      ) : (
        <ul className="locations-list">
          {locations.map(loc => (
            <li key={loc.id}>
              <strong>{loc.name}</strong> - {loc.address} <br />
              <span>{loc.phone}</span>
            </li>
          ))}
        </ul>
      )}
    </div>
  );
};

export default Locations;
