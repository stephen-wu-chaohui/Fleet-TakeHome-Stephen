using Fleet.Api.Controllers;
using Fleet.Api.Models;
using Fleet.Api.Tests.Mocks;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Fleet.Api.Tests.Controllers
{
    public class RegistrationControllerTests
    {
        [Fact]
        public void GetStatuses_ReturnsList()
        {
            var repo = new MockCarRepository();
            var controller = new RegistrationController(repo);

            var result = controller.GetRegistrationStatus();

            Assert.NotNull(result.Result);
        }

        [Fact]
        public async Task GetStatuses_UpdateLastCheckPoint() {
            var repo = new MockCarRepository();
            var controller = new RegistrationController(repo);

            var before = repo.LastCheckPoint;
            await Task.Delay(20);
            var result = controller.GetRegistrationStatus();
            var after = repo.LastCheckPoint;
            Assert.True(after > before);
        }
    }
}
