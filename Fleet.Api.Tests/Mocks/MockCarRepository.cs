using Fleet.Api.Models;
using Fleet.Api.Services;

namespace Fleet.Api.Tests.Mocks
{
    public class MockCarRepository : ICarRepository
    {
        public List<Car> Cars { get; } =
        [
            new Car { Id = 1, Make = "Toyota", Model = "Corolla", RegistrationNumber = "A1", RegistrationExpiry = DateTime.UtcNow.AddDays(3) },
            new Car { Id = 2, Make = "Honda", Model = "Civic", RegistrationNumber = "B2", RegistrationExpiry = DateTime.UtcNow.AddDays(-1) }
        ];

        public DateTime LastCheckPoint { get; set; } = DateTime.UtcNow;

        public IEnumerable<Car> GetAll(string? make) => Cars.Where(c => string.IsNullOrEmpty(make) || string.Compare(c.Make, make, StringComparison.OrdinalIgnoreCase) == 0);
    }
}
