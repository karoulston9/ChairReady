using Microsoft.EntityFrameworkCore;

namespace backend;

public class ChairReadyContext : DbContext
{
    public ChairReadyContext(DbContextOptions<ChairReadyContext> options) : base(options) { }

    public DbSet<AboutUs> AboutUs { get; set; }
    public DbSet<Location> Locations { get; set; }
    public DbSet<Booking> Bookings { get; set; }
    public DbSet<Logo> Logos { get; set; }
    public DbSet<Availability> Availabilities { get; set; }
    public DbSet<AdminUser> AdminUsers { get; set; }
}

public class AboutUs
{
    public int Id { get; set; }
    public string Content { get; set; } = string.Empty;
}

public class Location
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
}

public class Booking
{
    public int Id { get; set; }
    public int LocationId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string CustomerEmail { get; set; } = string.Empty;
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public Location? Location { get; set; }
}

public class Logo
{
    public int Id { get; set; }
    public string FileName { get; set; } = string.Empty;
    public byte[] Data { get; set; } = Array.Empty<byte>();
}

public class Availability
{
    public int Id { get; set; }
    public int LocationId { get; set; }
    public DateTime Date { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public Location? Location { get; set; }
}

public class AdminUser
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
}
