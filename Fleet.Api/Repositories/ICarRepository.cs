// Repositories/ICarRepository.cs
using Fleet.Api.DTOs;
using Fleet.Api.Models;

namespace Fleet.Api.Repositories;

public interface ICarRepository {
    Task<IEnumerable<CarDto>> GetAllAsync(string? make);
    Task<IEnumerable<CarExpiryDto>> GetAllWithExpiryAsync();
    Task<IEnumerable<CarExpiryDto>> GetExpiryUpdateAsync();
}
