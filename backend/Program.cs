using backend;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ChairReadyContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add JWT authentication
var jwtKey = "SuperSecretKey12345"; // Use a secure key in production
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(jwtKey))
    };
})
.AddGoogle("Google", options =>
{
    options.ClientId = builder.Configuration["Authentication:Google:ClientId"] ?? "";
    options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"] ?? "";
})
.AddMicrosoftAccount("Microsoft", options =>
{
    options.ClientId = builder.Configuration["Authentication:Microsoft:ClientId"] ?? "";
    options.ClientSecret = builder.Configuration["Authentication:Microsoft:ClientSecret"] ?? "";
});

builder.Services.AddAuthorization();

// Add CORS services
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy => policy.WithOrigins("http://localhost:3000")
                        .AllowAnyHeader()
                        .AllowAnyMethod());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Use CORS before authentication/authorization
app.UseCors("AllowFrontend");

app.UseAuthentication();
app.UseAuthorization();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();

// About Us endpoints
app.MapGet("/api/aboutus", async (ChairReadyContext db) =>
    await db.AboutUs.FirstOrDefaultAsync());

app.MapPut("/api/aboutus", [Authorize] async (ChairReadyContext db, AboutUs aboutUs) =>
{
    var entity = await db.AboutUs.FirstOrDefaultAsync();
    if (entity == null)
    {
        db.AboutUs.Add(aboutUs);
    }
    else
    {
        entity.Content = aboutUs.Content;
        db.AboutUs.Update(entity);
    }
    await db.SaveChangesAsync();
    return Results.Ok();
});

// Locations endpoints
app.MapGet("/api/locations", async (ChairReadyContext db) =>
    await db.Locations.ToListAsync());

app.MapGet("/api/locations/{id}", async (ChairReadyContext db, int id) =>
    await db.Locations.FindAsync(id) is Location loc ? Results.Ok(loc) : Results.NotFound());

app.MapPost("/api/locations", [Authorize] async (ChairReadyContext db, Location location) =>
{
    db.Locations.Add(location);
    await db.SaveChangesAsync();
    return Results.Created($"/api/locations/{location.Id}", location);
});

app.MapPut("/api/locations/{id}", [Authorize] async (ChairReadyContext db, int id, Location updated) =>
{
    var loc = await db.Locations.FindAsync(id);
    if (loc == null) return Results.NotFound();
    loc.Name = updated.Name;
    loc.Address = updated.Address;
    loc.Phone = updated.Phone;
    await db.SaveChangesAsync();
    return Results.Ok(loc);
});

app.MapDelete("/api/locations/{id}", [Authorize] async (ChairReadyContext db, int id) =>
{
    var loc = await db.Locations.FindAsync(id);
    if (loc == null) return Results.NotFound();
    db.Locations.Remove(loc);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

// Bookings endpoints
app.MapGet("/api/bookings", async (ChairReadyContext db) =>
    await db.Bookings.Include(b => b.Location).ToListAsync());

app.MapGet("/api/bookings/{id}", async (ChairReadyContext db, int id) =>
    await db.Bookings.Include(b => b.Location).FirstOrDefaultAsync(b => b.Id == id) is Booking b ? Results.Ok(b) : Results.NotFound());

app.MapPost("/api/bookings", async (ChairReadyContext db, Booking booking) =>
{
    db.Bookings.Add(booking);
    await db.SaveChangesAsync();
    return Results.Created($"/api/bookings/{booking.Id}", booking);
});

// Logo endpoints
app.MapGet("/api/logo", async (ChairReadyContext db) =>
    await db.Logos.FirstOrDefaultAsync());

app.MapPut("/api/logo", [Authorize] async (ChairReadyContext db, Logo logo) =>
{
    var entity = await db.Logos.FirstOrDefaultAsync();
    if (entity == null)
    {
        db.Logos.Add(logo);
    }
    else
    {
        entity.FileName = logo.FileName;
        entity.Data = logo.Data;
        db.Logos.Update(entity);
    }
    await db.SaveChangesAsync();
    return Results.Ok();
});

// Availability endpoints
app.MapGet("/api/availability", async (ChairReadyContext db) =>
    await db.Availabilities.Include(a => a.Location).ToListAsync());

app.MapGet("/api/availability/{id}", async (ChairReadyContext db, int id) =>
    await db.Availabilities.Include(a => a.Location).FirstOrDefaultAsync(a => a.Id == id) is Availability a ? Results.Ok(a) : Results.NotFound());

app.MapPost("/api/availability", [Authorize] async (ChairReadyContext db, Availability availability) =>
{
    db.Availabilities.Add(availability);
    await db.SaveChangesAsync();
    return Results.Created($"/api/availability/{availability.Id}", availability);
});

app.MapPut("/api/availability/{id}", [Authorize] async (ChairReadyContext db, int id, Availability updated) =>
{
    var avail = await db.Availabilities.FindAsync(id);
    if (avail == null) return Results.NotFound();
    avail.LocationId = updated.LocationId;
    avail.Date = updated.Date;
    avail.StartTime = updated.StartTime;
    avail.EndTime = updated.EndTime;
    await db.SaveChangesAsync();
    return Results.Ok(avail);
});

app.MapDelete("/api/availability/{id}", [Authorize] async (ChairReadyContext db, int id) =>
{
    var avail = await db.Availabilities.FindAsync(id);
    if (avail == null) return Results.NotFound();
    db.Availabilities.Remove(avail);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

// External login endpoints
app.MapGet("/api/auth/google/login", (HttpContext http) =>
{
    var redirectUrl = "/api/auth/external/callback";
    var properties = new Microsoft.AspNetCore.Authentication.AuthenticationProperties { RedirectUri = redirectUrl };
    return Results.Challenge(properties, new[] { "Google" });
});

app.MapGet("/api/auth/microsoft/login", (HttpContext http) =>
{
    var redirectUrl = "/api/auth/external/callback";
    var properties = new Microsoft.AspNetCore.Authentication.AuthenticationProperties { RedirectUri = redirectUrl };
    return Results.Challenge(properties, new[] { "Microsoft" });
});

app.MapGet("/api/auth/external/callback", async (HttpContext http) =>
{
    var result = await http.AuthenticateAsync();
    if (!result.Succeeded || result.Principal == null)
        return Results.Unauthorized();
    var claims = result.Principal.Identities.First().Claims.Select(c => new { c.Type, c.Value });
    // Issue a JWT for the authenticated user
    var jwt = GenerateJwtToken(result.Principal);
    return Results.Ok(new { Message = "External login successful", Token = jwt, Claims = claims });
});

string GenerateJwtToken(System.Security.Claims.ClaimsPrincipal principal)
{
    var handler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
    var key = System.Text.Encoding.UTF8.GetBytes("SuperSecretKey12345"); // Use a secure key in production
    var descriptor = new Microsoft.IdentityModel.Tokens.SecurityTokenDescriptor
    {
        Subject = new System.Security.Claims.ClaimsIdentity(principal.Claims),
        Expires = DateTime.UtcNow.AddHours(8),
        SigningCredentials = new Microsoft.IdentityModel.Tokens.SigningCredentials(
            new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(key),
            Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256Signature)
    };
    var token = handler.CreateToken(descriptor);
    return handler.WriteToken(token);
}

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
