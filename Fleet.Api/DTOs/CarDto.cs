// DTOs/CarDto.cs
using Fleet.Api.Models;

namespace Fleet.Api.DTOs;

public sealed record CarDto(
    int Id,
    string Make,
    string Model,
    string Plate,
    DateTime RegistrationExpiry
) {
    public CarDto(Car c): this(
        c.Id,
        c.Make,
        c.Model,
        c.Plate,
        c.RegistrationExpiry
    ) { }
}
