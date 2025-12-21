using Fleet.Api.Hubs;
using Fleet.Api.Repositories;
using Fleet.Api.Services;
using Fleet.Api.Services.Startup;
using Microsoft.Azure.SignalR;

var builder = WebApplication.CreateBuilder(args);

// Prepare fresh JSON every time API starts
// Generate fresh demo data only outside Test environment
if (!builder.Environment.IsEnvironment("Test")) {
    JsonDataSeeder.CreateCarDataFile(builder.Environment);
}


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

// Static frontend
app.UseDefaultFiles();
app.UseStaticFiles();

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

// SPA fallback
app.MapFallbackToFile("index.html");

app.Run();
