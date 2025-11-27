using Fleet.Api.DTOs;
using Fleet.Api.Models;
using System.Text.Json;

namespace Fleet.Api.Repositories;

public class CarRepository(IWebHostEnvironment env) : ICarRepository {
    private readonly List<Car> _cars = LoadCars(env);

    private DateTime LastCheckPoint { get; set; } = DateTime.MinValue;

    private static List<Car> LoadCars(IWebHostEnvironment env) {
        var filePath = Path.Combine(env.ContentRootPath, "wwwroot", "data", "cars.json");

        try {
            if (!File.Exists(filePath))
                throw new FileNotFoundException($"Car data file not found: {filePath}");

            var json = File.ReadAllText(filePath);

            var options = new JsonSerializerOptions {
                PropertyNameCaseInsensitive = true
            };

            var cars = JsonSerializer.Deserialize<List<Car>>(json, options);

            if (cars == null)
                throw new JsonException("Car JSON file is empty or invalid.");

            return cars;
        } catch (Exception ex) {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Fleet.Api failed to load car data:");
            Console.ResetColor();

            Console.WriteLine(ex.Message);
            Console.WriteLine(ex.StackTrace);

            // Fail fast — API should NOT start with invalid data
            throw;
        }
    }

    public Task<IEnumerable<CarDto>> GetAllAsync(string? make) {
        IEnumerable<Car> result = _cars;

        if (!string.IsNullOrWhiteSpace(make)) {
            // null-safe filtering
            result = result.Where(c =>
                string.Equals(c.Make, make, StringComparison.OrdinalIgnoreCase));
        }

        var dtos = result.Select(c => new CarDto(c));
        return Task.FromResult(dtos);
    }

    public Task<IEnumerable<CarExpiryDto>> GetAllWithExpiryAsync() {
        LastCheckPoint = DateTime.UtcNow;

        var result = _cars.Select(c =>
            new CarExpiryDto(
                c.Id,
                c.Plate,
                c.RegistrationExpiry,
                IsExpired: c.RegistrationExpiry < LastCheckPoint,
                LastCheckPoint: LastCheckPoint));

        return Task.FromResult(result);
    }

    public Task<IEnumerable<CarExpiryDto>> GetExpiryUpdateAsync() {
        var fromCheckPoint = LastCheckPoint;
        var toCheckPoint = DateTime.UtcNow;

        var result = _cars
            .Where(c => c.RegistrationExpiry >= fromCheckPoint &&
                        c.RegistrationExpiry < toCheckPoint)
            .Select(c =>
                new CarExpiryDto(
                    c.Id,
                    c.Plate,
                    c.RegistrationExpiry,
                    IsExpired: true,
                    LastCheckPoint: toCheckPoint));

        LastCheckPoint = toCheckPoint;
        return Task.FromResult(result);
    }
}
