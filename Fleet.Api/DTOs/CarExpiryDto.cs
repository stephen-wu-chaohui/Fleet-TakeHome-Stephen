// DTOs/ExpiryUpdateDto.cs
namespace Fleet.Api.DTOs;

public sealed record CarExpiryDto(
    int CarId,
    string Plate,
    DateTime RegistrationExpiry,
    bool IsExpired,
    DateTime LastCheckPoint
);
