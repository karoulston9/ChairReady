import React from 'react';

const Booking: React.FC = () => (
  <div className="page booking-page">
    <h1>Book an Appointment</h1>
    <p>Choose your preferred location, service, and time. Our simple booking process gets you ready in minutes!</p>
    {/* Placeholder for booking form */}
    <form className="booking-form">
      <input type="text" placeholder="Your Name" required />
      <input type="text" placeholder="Preferred Location" required />
      <input type="datetime-local" required />
      <button type="submit">Book Now</button>
    </form>
  </div>
);

export default Booking;
