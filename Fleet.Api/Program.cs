using Fleet.Api.Hubs;
using Fleet.Api.Services;
using Fleet.Api.Services.Startup;

var builder = WebApplication.CreateBuilder(args);

// Prepare fresh JSON every time API starts
JsonDataSeeder.CreateCarDataFile(builder.Environment);

// Register services AFTER JSON file is created
builder.Services.AddSingleton<ICarRepository, CarRepository>();

// Add services
builder.Services.AddControllers();
builder.Services.AddSignalR();

builder.Services.AddHostedService<RegistrationCheckService>();

// CORS for React dev server
var corsPolicyName = "AllowReact";
var clientUrls = builder.Configuration.GetSection("Client:Urls").Get<string[]>()
    ?? [ "http://localhost:3000" ];

builder.Services.AddCors(options => {
    options.AddPolicy(corsPolicyName, policy => {
        policy
            .WithOrigins(clientUrls)
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

var app = builder.Build();

app.UseHttpsRedirection();

app.UseRouting();
app.UseCors(corsPolicyName);

app.UseAuthorization();

app.MapControllers();
app.MapHub<RegistrationHub>("/hubs/registration");

app.Run();
