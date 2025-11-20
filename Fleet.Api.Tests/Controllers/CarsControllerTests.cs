using Xunit;
using Microsoft.AspNetCore.Mvc;
using Fleet.Api.Controllers;
using Fleet.Api.Tests.Mocks;

namespace Fleet.Api.Tests.Controllers
{
    public class CarsControllerTests
    {
        [Fact]
        public void GetCars_ReturnsAll()
        {
            var repo = new MockCarRepository();
            var controller = new CarsController(repo);

            var result = controller.GetCars(null);

            Assert.NotNull(result.Result);
            Assert.NotNull(result.Result);
        }

        [Fact]
        public void GetCars_FilterByMake()
        {
            var repo = new MockCarRepository();
            var controller = new CarsController(repo);

            var result = controller.GetCars("Honda");

            Assert.NotNull(result);
            Assert.NotNull(result.Result);
        }
    }
}
