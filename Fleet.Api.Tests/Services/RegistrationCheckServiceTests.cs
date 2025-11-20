using System;
using System.Threading.Tasks;
using Xunit;
using Fleet.Api.Hubs;
using Fleet.Api.Services;
using Fleet.Api.Tests.Mocks;
using Microsoft.Extensions.Logging.Abstractions;

namespace Fleet.Api.Tests.Services
{
    public class RegistrationCheckServiceTests
    {
        [Fact]
        public async Task Broadcasts_ExpiredCars()
        {
            var repo = new MockCarRepository {
                LastCheckPoint = DateTime.UtcNow.AddMinutes(-10)
            };
            repo.Cars[1].RegistrationExpiry = DateTime.UtcNow.AddSeconds(-1);

            var hub = new FakeHubContext<RegistrationHub>();
            var logger = NullLogger<RegistrationCheckService>.Instance;

            var svc = new RegistrationCheckService(logger, repo, hub);

            await svc.UpdateRegistrationStatus(CancellationToken.None);

            var msg = ((FakeHubContext<RegistrationHub>.FakeClientProxy)hub.Clients.All).LastMessage;

            Assert.NotNull(msg);
        }

        [Fact]
        public async Task Updates_LastCheckPoint()
        {
            var repo = new MockCarRepository();
            var hub = new FakeHubContext<RegistrationHub>();
            var logger = NullLogger<RegistrationCheckService>.Instance;

            var svc = new RegistrationCheckService(logger, repo, hub);

            var before = repo.LastCheckPoint;

            await Task.Delay(20);
            await svc.UpdateRegistrationStatus(CancellationToken.None);

            Assert.True(repo.LastCheckPoint > before);
        }
    }
}
