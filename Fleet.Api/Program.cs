using Fleet.Api.Hubs;
using Fleet.Api.Repositories;
using Fleet.Api.Services;
using Fleet.Api.Services.Startup;
using Microsoft.Azure.SignalR;

var builder = WebApplication.CreateBuilder(args);


// ----------------------------------------------------------------------
// Data seeding
// ----------------------------------------------------------------------
// Generate fresh demo data only outside Test environment
if (!builder.Environment.IsEnvironment("Test")) {
    JsonDataSeeder.CreateCarDataFile(builder.Environment);
}

// ----------------------------------------------------------------------
// CORS (configured via appsettings.*.json)
// ----------------------------------------------------------------------
var allowedOrigins =
    builder.Configuration
           .GetSection("Cors:AllowedOrigins")
           .Get<string[]>() ?? Array.Empty<string>();

builder.Services.AddCors(options => {
    options.AddDefaultPolicy(policy => {
        policy
            .WithOrigins(allowedOrigins)
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

// ----------------------------------------------------------------------
// SignalR
// ----------------------------------------------------------------------
var azureConnectionString = builder.Configuration["Azure:SignalR:ConnectionString"];
if (!string.IsNullOrEmpty(azureConnectionString)) {
    builder.Services.AddSignalR().AddAzureSignalR(azureConnectionString);
} else {
    builder.Services.AddSignalR();
}

// ----------------------------------------------------------------------
// Application services
// ----------------------------------------------------------------------
builder.Services.AddSingleton<ICarRepository, CarRepository>();
builder.Services.AddHostedService<RegistrationCheckService>();

var app = builder.Build();

// ----------------------------------------------------------------------
// Middleware
// ----------------------------------------------------------------------
app.UseCors();

// ----------------------------------------------------------------------
// API endpoints
// ----------------------------------------------------------------------
app.MapGet("/api/cars", async (ICarRepository repo, string? make) => {
    var cars = await repo.GetAllAsync(make);
    return Results.Ok(cars);
});

app.MapGet("/api/registration", async (ICarRepository repo) => {
    var cars = await repo.GetAllWithExpiryAsync();
    return Results.Ok(cars);
});

// ----------------------------------------------------------------------
// SignalR hubs
// ----------------------------------------------------------------------
app.MapHub<RegistrationHub>("/hubs/registration");

app.Run();
