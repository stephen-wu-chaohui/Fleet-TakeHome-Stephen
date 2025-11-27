using Fleet.Api.Hubs;
using Fleet.Api.Repositories;
using Microsoft.AspNetCore.SignalR;

namespace Fleet.Api.Services;

public class RegistrationCheckService(
    ILogger<RegistrationCheckService> logger,
    ICarRepository carRepository,
    IHubContext<RegistrationHub> hubContext) : BackgroundService {
    public Task CheckRegistrationsOnce(CancellationToken stoppingToken) => UpdateRegistrationStatus(stoppingToken);

    protected async Task UpdateRegistrationStatus(CancellationToken stoppingToken) {
        if (stoppingToken.IsCancellationRequested)
            return;

        var statuses = await carRepository.GetExpiryUpdateAsync();
        if (statuses.Any()) {
            await hubContext.Clients.All
            .SendAsync("RegistrationStatusUpdated", statuses, stoppingToken);
        }
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
