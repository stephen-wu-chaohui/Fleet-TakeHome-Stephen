using Fleet.Api.DTOs;
using Fleet.Api.Repositories;
using Fleet.Api.Tests.Fakes;
using Fleet.Api.Models;
using FluentAssertions;
using System.Text.Json;

namespace Fleet.Api.Tests;

public class CarRepositoryTests {
    // Helper to create a JSON file
    private static void WriteJson(string path, object data) {
        Directory.CreateDirectory(Path.GetDirectoryName(path)!);
        var json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(path, json);
    }

    private static string PreparePath() {
        var temp = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(temp);
        return temp;
    }

    [Fact]
    public void Constructor_Throws_WhenFileMissing() {
        // Arrange
        var root = PreparePath();
        var env = new FakeHostEnvironment(root);

        // Act
        Action act = () => new CarRepository(env);

        // Assert
        act.Should().Throw<FileNotFoundException>();
    }

    [Fact]
    public void Constructor_Throws_WhenJsonCorrupt() {
        // Arrange
        var root = PreparePath();
        var env = new FakeHostEnvironment(root);

        var file = Path.Combine(root, "wwwroot", "data", "cars.json");
        Directory.CreateDirectory(Path.GetDirectoryName(file)!);
        File.WriteAllText(file, "{ this is not valid json");

        // Act
        Action act = () => new CarRepository(env);

        // Assert
        act.Should().Throw<Exception>();
    }

    [Fact]
    public void Constructor_LoadsCarsSuccessfully() {
        // Arrange
        var root = PreparePath();
        var env = new FakeHostEnvironment(root);

        var file = Path.Combine(root, "wwwroot", "data", "cars.json");
        WriteJson(file, new[]
        {
            new { Id = 12, Make = "Toyota", Plate = "ABC123", RegistrationExpiry = DateTime.UtcNow }
        });

        // Act
        var repo = new CarRepository(env);

        // Assert
        var cars = repo.GetAllAsync(null).Result;
        cars.Should().HaveCount(1);
        cars.First().Make.Should().Be("Toyota");
    }


    // -----------------------
    // Filtering tests
    // -----------------------

    [Fact]
    public void GetAllAsync_FiltersByMake_CaseInsensitive() {
        // Arrange
        var root = PreparePath();
        var env = new FakeHostEnvironment(root);
        var file = Path.Combine(root, "wwwroot", "data", "cars.json");

        WriteJson(file, new[]
        {
            new { Id = 11, Make = "Toyota", Plate = "AAA111", RegistrationExpiry = DateTime.UtcNow },
            new { Id = 12, Make = "Honda", Plate = "BBB222", RegistrationExpiry = DateTime.UtcNow }
        });

        var repo = new CarRepository(env);

        // Act
        var result = repo.GetAllAsync("toyota").Result;

        // Assert
        result.Should().HaveCount(1);
        result.First().Make.Should().Be("Toyota");
    }

    [Fact]
    public void GetAllAsync_NullMakeInSource_IsSafe() {
        // Arrange
        var root = PreparePath();
        var env = new FakeHostEnvironment(root);
        var file = Path.Combine(root, "wwwroot", "data", "cars.json");

        WriteJson(file, new[]
        {
            new { Id = 11, Make = (string?)null, Plate = "AAA111", RegistrationExpiry = DateTime.UtcNow },
            new { Id = 12, Make = "Toyota", Plate = "BBB222", RegistrationExpiry = DateTime.UtcNow }
        });

        var repo = new CarRepository(env);

        // Act
        var result = repo.GetAllAsync("Toyota").Result;

        // Assert
        result.Should().ContainSingle(x => x.Make == "Toyota");
    }


    // -----------------------
    // Expiry logic tests
    // -----------------------

    [Fact]
    public void GetAllWithExpiryAsync_CalculatesIsExpiredFlag() {
        // Arrange
        var root = PreparePath();
        var env = new FakeHostEnvironment(root);
        var file = Path.Combine(root, "wwwroot", "data", "cars.json");

        WriteJson(file, new[]
        {
            new { Id = 11, Make = "Toyota", Plate = "AAA111", RegistrationExpiry = DateTime.UtcNow.AddDays(-1) },
            new { Id = 12, Make = "Honda", Plate = "BBB222", RegistrationExpiry = DateTime.UtcNow.AddDays(5) }
        });

        var repo = new CarRepository(env);

        // Act
        var expiry = repo.GetAllWithExpiryAsync().Result;

        // Assert
        expiry.Should().HaveCount(2);
        expiry.First(x => x.Plate == "AAA111").IsExpired.Should().BeTrue();
        expiry.First(x => x.Plate == "BBB222").IsExpired.Should().BeFalse();
    }

    // -----------------------
    // Delta window tests
    // -----------------------

    [Fact]
    public void GetExpiryUpdateAsync_ReturnsNewlyExpiredOnly() {
        // Arrange
        var root = PreparePath();
        var env = new FakeHostEnvironment(root);
        var file = Path.Combine(root, "wwwroot", "data", "cars.json");

        var now = DateTime.UtcNow;

        WriteJson(file, new[]
        {
            new { Id = 11, Make = "Toyota", Plate="AAA111", RegistrationExpiry = now.AddSeconds(-1) },
            new { Id = 12, Make = "Honda",  Plate="BBB222", RegistrationExpiry = now.AddSeconds(10) }
        });

        var repo = new CarRepository(env);

        // First call sets the window
        var initialRun = repo.GetExpiryUpdateAsync().Result;

        // Nothing should be emitted now — no new events
        var secondRun = repo.GetExpiryUpdateAsync().Result;

        // Assert
        initialRun.Should().NotBeEmpty();   // initial delta from DateTime.MinValue
        secondRun.Should().BeEmpty();       // no new expiries detected
    }
}
