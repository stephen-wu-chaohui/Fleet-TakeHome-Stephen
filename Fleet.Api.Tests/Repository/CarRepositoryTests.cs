using Xunit;
using Fleet.Api.Tests.Mocks;
using Fleet.Api.Services;
using System.IO;

namespace Fleet.Api.Tests.Repository
{
    public class CarRepositoryTests
    {
        [Fact]
        public void LoadsCars()
        {
            var baseDir = AppContext.BaseDirectory;
            var contentRootPath = Path.GetFullPath(Path.Combine(baseDir, "..", "..", ".."));

            var env = new FakeHostEnvironment {
                ContentRootPath = contentRootPath
            };

            var repo = new CarRepository(env);
            var cars = repo.GetAll();

            Assert.NotNull(cars);
            Assert.NotEmpty(cars);
        }
    }
}
