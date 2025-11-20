# 🚚 Fleet API  
*A .NET 10 backend with SignalR, background services, and real-time registration expiry monitoring.*

## 📌 Overview

Fleet API is the backend service for the **Fleet** demo project.  
It showcases:

- Modern ASP.NET Core (.NET 10-ready structure)
- Real-time messaging with **SignalR**
- Background processing via `BackgroundService`
- Efficient delta-based registration expiry checks
- A repository abstraction for vehicle data
- Configuration-driven CORS and client origins

The design aims to model how a real fleet management or telematics backend might handle registration compliance events at scale.

---

## ✨ Features

### 🚗 Cars API

**Endpoint:**

```http
GET /api/cars
GET /api/cars?make={make}
```

- Returns a list of cars, optionally filtered by `make`
- Data is sourced from an `ICarRepository` implementation (`CarRepository` or JSON-based)
- Intended to be consumed by the frontend Cars page (`/`)

### 🕒 Registration Status API

**Endpoint:**

```http
GET /api/registration
```

- Returns a snapshot of current registration expiry status for all vehicles
- Primarily used by the frontend Registration page for initial state

### 📡 SignalR Hub (`RegistrationHub`)

**Endpoint:**

```http
/hubs/registration
```

- Used to push **real-time registration expiry updates** to connected clients
- The hub itself is a thin channel (`RegistrationHub : Hub`) with no RPC-style methods
- All updates are produced by the background service `RegistrationCheckService`

### 🔁 Background Service (`RegistrationCheckService`)

`RegistrationCheckService` is a hosted service derived from `BackgroundService`.  
It periodically examines the repository of cars and emits **delta updates** when vehicles transition into an expired state.

Key responsibilities:

- Maintain a sliding window using `ICarRepository.LastCheckPoint`
- On each iteration:
  - Compute the time window `[LastCheckPoint → now)`
  - Determine which cars have `RegistrationExpiry` in that window
  - Broadcast only those vehicles as **newly expired**
- Advance `LastCheckPoint` to the current time
- Sleep for a configured interval (e.g. 10 seconds) and repeat

This approach is efficient enough to scale to fleets of thousands of vehicles without broadcasting redundant data.

---

## 🗂 Folder Structure

```text
Fleet.Api/
 ├── Controllers/
 │    ├── CarsController.cs              # HTTP API for listing cars
 │    └── RegistrationController.cs      # HTTP API for registration snapshot
 ├── Hubs/
 │    └── RegistrationHub.cs             # SignalR hub for registration updates
 ├── Services/
 │    └── RegistrationCheckService.cs    # Background service emitting delta updates
 ├── Repository/
 │    ├── ICarRepository.cs              # Vehicle repository abstraction
 │    └── CarRepository.cs               # Demo in-memory implementation
 ├── Models/
 │    └── Car.cs                         # Car domain model
 ├── wwwroot/
 │    └── data/
 │         └── cars.json                 # Seeded car data (if using JSON-based seeding)
 ├── appsettings.json                    # Base configuration
 ├── appsettings.Development.json        # Dev overrides
 ├── Program.cs                          # Application bootstrap (DI, middleware, endpoints)
 └── Fleet.Api.csproj
```

You can plug in different repository implementations (e.g., EF Core, Dapper, external APIs) behind `ICarRepository` without changing controllers or services.

---

## ⚙️ Configuration

### CORS – Allowed Client Origins

The API reads allowed client origins from configuration under `Client:Urls`.

**appsettings.json** (production-style example):

```json
{
  "Client": {
    "Urls": [
      "https://your-frontend-domain.com"
    ]
  }
}
```

**appsettings.Development.json** (local dev):

```json
{
  "Client": {
    "Urls": [
      "http://localhost:3000"
    ]
  }
}
```

**Program.cs**:

```csharp
var corsPolicyName = "AllowClient";

var clientUrls = builder.Configuration
    .GetSection("Client:Urls")
    .Get<string[]>()
    ?? new[] { "http://localhost:3000" };

builder.Services.AddCors(options =>
{
    options.AddPolicy(corsPolicyName, policy =>
    {
        policy
            .WithOrigins(clientUrls)
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

// later...
app.UseCors(corsPolicyName);
```

This allows you to change environments (dev/staging/prod) by changing configuration, not code.

### Repository Seeding & LastCheckPoint

The repository may:

- Seed car data on startup (e.g., random car set, JSON file generation)
- Expose a `LastCheckPoint` property used by `RegistrationCheckService` to compute deltas

```csharp
public interface ICarRepository
{
    IReadOnlyList<Car> GetAll();

    /// <summary>
    /// Tracks the timestamp of the last registration-expiry scan.
    /// RegistrationCheckService uses this to compute a delta window
    /// (LastCheckPoint → now) for newly expired vehicles.
    /// </summary>
    DateTime LastCheckPoint { get; set; }
}
```

---

## ▶️ Running Locally

### 1. Restore dependencies

```bash
dotnet restore
```

### 2. Run the API

```bash
dotnet run
```

By default, it will listen on a URL like:

```text
https://localhost:5001
```

Make sure the frontend is configured to call the same origin (via `config.js` or `REACT_APP_API_URL`).

---

## 🧪 Testing

If you created a test project such as `Fleet.Api.Tests`:

```bash
dotnet test
```

Typical tests include:

- Repository initialization from seeded data
- JSON seeding (if present)
- `RegistrationCheckService` behaviour (e.g., delta logic across checkpoints)
- Controller responses for `/api/cars` and `/api/registration`

---

## 🚀 Deployment

You can deploy Fleet.Api to:

- **Azure App Service**
- **Azure Container Apps**
- **AWS ECS / Fargate**
- **Docker/Kubernetes**
- Any environment that supports .NET 8/10

Key steps for deployment:

1. Set `ASPNETCORE_ENVIRONMENT` appropriately (`Production`, `Staging`, etc.)
2. Configure `Client:Urls` in environment-specific configuration or platform settings
3. Ensure HTTPS is configured correctly
4. Optionally enable logging and Application Insights / OpenTelemetry

---

## 🧩 Design Notes

- **Delta-based processing:** Instead of re-sending all vehicles each cycle, the backend emits only incremental changes based on `LastCheckPoint`. This models realistic behaviour for large-scale fleets where registration expiry events are rare relative to fleet size.
- **SignalR as a push channel:** Clients do not call hub methods directly; all real-time events originate from the background service. This separation keeps the hub thin and focused on broadcasting.
- **Repository abstraction:** Allows swapping in a database-backed implementation without impacting services/controllers.

---

## 📄 License

This backend is provided under the MIT License.  
You are free to reuse, extend, or adapt it for your own learning and projects.
