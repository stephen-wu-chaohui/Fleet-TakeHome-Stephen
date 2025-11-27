// Models/Car.cs
namespace Fleet.Api.Models;

public sealed class Car {
    public int Id { get; set; }
    public string Make { get; set; } = "";
    public string Model { get; set; } = "";
    public string Plate { get; set; } = "";

    public DateTime RegistrationExpiry { get; set; }
}
