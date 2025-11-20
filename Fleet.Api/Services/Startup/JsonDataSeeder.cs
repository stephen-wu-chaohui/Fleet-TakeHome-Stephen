using System.Text.Json;
using Fleet.Api.Models;

namespace Fleet.Api.Services.Startup;

public static class JsonDataSeeder {
    private static readonly string[] Makes =
    {
        "Toyota", "Ford", "Mazda", "Honda", "Nissan", "Hyundai", "Kia",
        "Mitsubishi", "Volkswagen", "Subaru"
    };

    private static readonly Dictionary<string, string[]> Models =
        new() {
            ["Toyota"] = ["Corolla", "Hilux", "RAV4", "Camry"],
            ["Ford"] = ["Ranger", "Focus", "Everest"],
            ["Mazda"] = ["CX-5", "Mazda3", "BT-50"],
            ["Honda"] = ["Civic", "HR-V", "CR-V"],
            ["Nissan"] = ["Navara", "X-Trail", "Qashqai"],
            ["Hyundai"] = ["i30", "Tucson", "Santa Fe"],
            ["Kia"] = ["Sportage", "Sorento", "Cerato"],
            ["Mitsubishi"] = ["Triton", "Outlander", "ASX"],
            ["Volkswagen"] = ["Golf", "Tiguan", "Amarok"],
            ["Subaru"] = ["Forester", "Outback", "XV"],
        };

    private static readonly Random Rng = new();

    public static void CreateCarDataFile(IWebHostEnvironment env) {
        var dataFolder = Path.Combine(env.ContentRootPath, "wwwroot", "data");
        Directory.CreateDirectory(dataFolder);

        var filePath = Path.Combine(dataFolder, "cars.json");

        int count = Rng.Next(8, 20); // generate between 8–20 cars

        var cars = new List<Car>();

        for (int i = 1; i <= count; i++) {
            string make = Makes[Rng.Next(Makes.Length)];
            string model = Models[make][Rng.Next(Models[make].Length)];

            var car = new Car {
                Id = i,
                Make = make,
                Model = model,
                RegistrationNumber = GeneratePlate(),
                RegistrationExpiry = GenerateExpiry()
            };

            cars.Add(car);
        }

        var json = JsonSerializer.Serialize(
            cars,
            new JsonSerializerOptions { WriteIndented = true }
        );

        File.WriteAllText(filePath, json);
    }

    private static string GeneratePlate() {
        // Example: ABC123 or ZQX918
        char L1 = (char)('A' + Rng.Next(26));
        char L2 = (char)('A' + Rng.Next(26));
        char L3 = (char)('A' + Rng.Next(26));
        int numbers = Rng.Next(100, 999);

        return $"{L1}{L2}{L3}{numbers}";
    }

    private static DateTime GenerateExpiry() {
        // 20% chance expired
        bool expired = Rng.NextDouble() < 0.20;

        if (expired) {
            // Expired 1–120 days ago
            return DateTime.UtcNow.AddDays(-Rng.Next(1, 121));
        } else {
            // Valid but expiring 1–10 minutes from now
            return DateTime.UtcNow.AddMinutes(Rng.Next(1, 11));
        }
    }
}
