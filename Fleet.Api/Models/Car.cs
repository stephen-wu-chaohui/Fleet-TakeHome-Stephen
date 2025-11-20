namespace Fleet.Api.Models;

public class Car {
    public int Id { get; set; }
    public string Make { get; set; } = "";
    public string Model { get; set; } = "";
    public string RegistrationNumber { get; set; } = "";
    public DateTime RegistrationExpiry { get; set; }
}
