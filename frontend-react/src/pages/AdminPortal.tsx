import React from 'react';

const AdminPortal: React.FC = () => (
  <div className="page admin-portal-page">
    <h1>Admin Portal</h1>
    <p>Manage bookings, locations, and user accounts. This portal is for authorized staff only.</p>
    {/* Placeholder for admin controls */}
    <div className="admin-controls">
      <button>View Bookings</button>
      <button>Manage Locations</button>
      <button>Manage Users</button>
    </div>
  </div>
);

export default AdminPortal;
