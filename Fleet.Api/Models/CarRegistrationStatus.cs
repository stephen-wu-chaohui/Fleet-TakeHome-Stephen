namespace Fleet.Api.Models;

public class CarRegistrationStatus {
    public int CarId { get; set; }
    public string RegistrationNumber { get; set; } = "";
    public DateTime RegistrationExpiry { get; set; }
    public bool IsExpired { get; set; }
}
