using Fleet.Api.Hubs;
using Fleet.Api.Repositories;
using Fleet.Api.Services;
using Fleet.Api.Services.Startup;

var builder = WebApplication.CreateBuilder(args);

// Prepare fresh JSON every time API starts
JsonDataSeeder.CreateCarDataFile(builder.Environment);

// Add services
builder.Services.AddSignalR();
builder.Services.AddSingleton<ICarRepository, CarRepository>();
builder.Services.AddHostedService<RegistrationCheckService>();

var app = builder.Build();

// Minimal API endpoints
app.MapGet("/api/cars", async (ICarRepository repo, string? make) => {
    var cars = await repo.GetAllAsync(make);
    return Results.Ok(cars);
});

app.MapGet("/api/registration", async (ICarRepository repo) => {
    var cars = await repo.GetAllWithExpiryAsync();
    return Results.Ok(cars);
});

app.MapHub<RegistrationHub>("/hubs/registration");

app.Run();
