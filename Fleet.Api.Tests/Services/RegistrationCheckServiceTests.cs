using Fleet.Api.Hubs;
using Fleet.Api.Services;
using Fleet.Api.Tests.Fakes;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace Fleet.Api.Tests;

public class RegistrationCheckServiceTests {
    private class ServiceTestContext {
        public Mock<IHubClients> HubClients { get; }
        public Mock<IClientProxy> ClientProxy { get; }
        public Mock<IHubContext<RegistrationHub>> HubContext { get; }

        public ServiceTestContext() {
            HubClients = new Mock<IHubClients>();
            ClientProxy = new Mock<IClientProxy>();
            HubContext = new Mock<IHubContext<RegistrationHub>>();

            HubClients.Setup(c => c.All).Returns(ClientProxy.Object);
            HubContext.Setup(c => c.Clients).Returns(HubClients.Object);
        }
    }

    [Fact]
    public async Task CheckRegistrationsOnce_BroadcastsExpiryUpdates() {
        // Arrange
        var logger = NullLogger<RegistrationCheckService>.Instance;
        var repo = new FakeCarRepositoryExpiry();
        var ctx = new ServiceTestContext();
        var stoppingToken = CancellationToken.None;

        var service = new RegistrationCheckService(logger, repo, ctx.HubContext.Object);

        // Act — first run sends an update (one expired)
        await service.CheckRegistrationsOnce(stoppingToken);

        // Assert — Verify SignalR broadcast
        ctx.ClientProxy.Verify(
            x => x.SendCoreAsync(
                "RegistrationStatusUpdated",
                It.IsAny<object[]>(),
                default),
            Times.Once);
    }

    [Fact]
    public async Task CheckRegistrationsOnce_DoesNotBroadcast_WhenNoNewExpiry() {
        // Arrange
        var logger = NullLogger<RegistrationCheckService>.Instance;
        var repo = new FakeCarRepositoryExpiry();
        var ctx = new ServiceTestContext();
        var service = new RegistrationCheckService(logger, repo, ctx.HubContext.Object);
        var stoppingToken = CancellationToken.None;

        // First run (consumes first batch)
        await service.CheckRegistrationsOnce(stoppingToken);

        // Reset calls
        ctx.ClientProxy.Invocations.Clear();

        // Act — second run (no updates)
        await service.CheckRegistrationsOnce(stoppingToken);

        // Assert
        ctx.ClientProxy.Verify(
            x => x.SendCoreAsync(
                "RegistrationStatusUpdated",
                It.IsAny<object[]>(),
                default),
            Times.Never);
    }
}
