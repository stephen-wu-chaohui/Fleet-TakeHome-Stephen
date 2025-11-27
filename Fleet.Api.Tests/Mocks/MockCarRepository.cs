using Fleet.Api.DTOs;
using Fleet.Api.Models;
using Fleet.Api.Repositories;

namespace Fleet.Api.Tests.Mocks
{
    public class MockCarRepository : ICarRepository
    {
        public List<Car> Cars { get; } =
        [
            new Car { Id = 1, Make = "Toyota", Model = "Corolla", Plate = "A1", RegistrationExpiry = DateTime.UtcNow.AddDays(3) },
            new Car { Id = 2, Make = "Honda", Model = "Civic", Plate = "B2", RegistrationExpiry = DateTime.UtcNow.AddDays(-1) }
        ];

        private DateTime LastCheckPoint { get; set; } = DateTime.MinValue;

        public Task<IEnumerable<CarExpiryDto>> GetAllWithExpiryAsync() {
            LastCheckPoint = DateTime.MinValue;
            return GetExpiryUpdateAsync();
        }

        public Task<IEnumerable<CarExpiryDto>> GetExpiryUpdateAsync() {
            var fromCheckPoint = LastCheckPoint;
            LastCheckPoint = DateTime.UtcNow;
            var result = Cars.Where(c => c.RegistrationExpiry < DateTime.UtcNow)
                .Select(c => new CarExpiryDto(
                    c.Id,
                    c.Plate,
                    c.RegistrationExpiry,
                    IsExpired: c.RegistrationExpiry < LastCheckPoint,
                    LastCheckPoint: LastCheckPoint
                ));
            return Task.FromResult(result);
        }

        Task<IEnumerable<CarDto>> ICarRepository.GetAllAsync(string? make) {
            var result = Cars.Where(c => string.IsNullOrEmpty(make) || string.Compare(c.Make, make, StringComparison.OrdinalIgnoreCase) == 0)
                .Select(c => new CarDto(c));
            return Task.FromResult(result);
        }
    }
}
