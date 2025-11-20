using System.Text.Json;
using Fleet.Api.Models;

namespace Fleet.Api.Services;

public class CarRepository(IWebHostEnvironment env) : ICarRepository {
    private readonly List<Car> _cars = LoadCars(env);

    /// <summary>
    /// The last time the registration expiry delta calculation was executed.
    /// Initialized at service startup so that no historical vehicles
    /// are accidentally marked as newly expired on first run.
    /// </summary>
    DateTime ICarRepository.LastCheckPoint { get; set; } = DateTime.UtcNow;

    private static List<Car> LoadCars(IWebHostEnvironment env) {
        var filePath = Path.Combine(env.ContentRootPath, "wwwroot", "data", "cars.json");

        if (!File.Exists(filePath))
            throw new FileNotFoundException($"Car data file not found: {filePath}");

        var json = File.ReadAllText(filePath);

        var options = new JsonSerializerOptions {
            PropertyNameCaseInsensitive = true
        };

        var cars = JsonSerializer.Deserialize<List<Car>>(json, options) ?? [];

        return cars;
    }

    public IEnumerable<Car> GetAll(string? make = null)
        => string.IsNullOrWhiteSpace(make)
            ? _cars
            : _cars.Where(c =>
                string.Equals(c.Make, make, StringComparison.OrdinalIgnoreCase));
}
