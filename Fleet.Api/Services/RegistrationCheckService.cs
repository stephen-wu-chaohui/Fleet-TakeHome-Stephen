using Fleet.Api.Hubs;
using Fleet.Api.Models;
using Microsoft.AspNetCore.SignalR;

namespace Fleet.Api.Services;

/// <summary>
/// Background service that periodically checks for vehicles whose
/// registration has just expired, and emits real-time updates over SignalR.
///
/// Instead of re-checking the entire fleet every cycle (which does not
/// scale for large deployments), the service uses a "delta window"
/// approach:
/// 
///   LastCheckPoint ----- now
///
/// Only vehicles whose RegistrationExpiry timestamps fall inside this
/// window are considered "newly expired".
///
/// This is the same pattern used in production telematics systems where
/// expiry/compliance events occur infrequently relative to fleet size.
/// </summary>
public class RegistrationCheckService(
    ILogger<RegistrationCheckService> logger,
    ICarRepository carRepository,
    IHubContext<RegistrationHub> hubContext) : BackgroundService {

    public async Task UpdateRegistrationStatus(CancellationToken stoppingToken) {
        // This method can be used for testing purposes to trigger
        // a single execution of the registration status check.
        var lastCheckpoint = carRepository.LastCheckPoint;
        var now = DateTime.UtcNow;

        // Compute the delta: vehicles whose expiry timestamp
        // transitioned into the window [from → to)
        var statuses = carRepository.GetAll()
            .Where(c => c.RegistrationExpiry > lastCheckpoint)
            .Select(c => new CarRegistrationStatus {
                CarId = c.Id,
                RegistrationNumber = c.RegistrationNumber,
                RegistrationExpiry = c.RegistrationExpiry,
                IsExpired = c.RegistrationExpiry <= now
            })
            .Where(s => s.IsExpired); // Only send expired registrations

        // Push to all connected SignalR clients if any expired registrations exist
        if (statuses.Any()) {
            await hubContext.Clients.All
            .SendAsync("RegistrationStatusUpdated", statuses, stoppingToken);
        }
        // advance checkpoint
        carRepository.LastCheckPoint = now;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
        logger.LogInformation("RegistrationStatusBackgroundService started.");

        while (!stoppingToken.IsCancellationRequested) {
            try {
                await UpdateRegistrationStatus(stoppingToken);
                // Check every 10 seconds (adjust as needed)
                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
            } catch (TaskCanceledException) {
                // Normal during shutdown
                break;
            } catch (Exception ex) {
                logger.LogError(ex, "Error while broadcasting registration status.");
            }

        }

        logger.LogInformation("RegistrationStatusBackgroundService stopping.");
    }
}
