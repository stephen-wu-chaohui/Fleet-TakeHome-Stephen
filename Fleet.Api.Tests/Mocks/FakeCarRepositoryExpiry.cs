using Fleet.Api.DTOs;
using Fleet.Api.Repositories;

namespace Fleet.Api.Tests.Fakes;

public class FakeCarRepositoryExpiry : ICarRepository {
    private readonly Queue<IEnumerable<CarExpiryDto>> _batches = new();

    public FakeCarRepositoryExpiry() {
        // First run: one expired car
        _batches.Enqueue(
        [
            new CarExpiryDto(
                CarId: 12,
                Plate: "ABC123",
                RegistrationExpiry: DateTime.UtcNow.AddDays(-1),
                IsExpired: true,
                LastCheckPoint: DateTime.UtcNow
            )
        ]);

        // Second run: no changes
        _batches.Enqueue(Array.Empty<CarExpiryDto>());
    }

    public Task<IEnumerable<CarExpiryDto>> GetAllWithExpiryAsync()
        => Task.FromResult(_batches.Dequeue());

    public Task<IEnumerable<CarExpiryDto>> GetExpiryUpdateAsync()
        => Task.FromResult(_batches.Dequeue());

    public Task<IEnumerable<CarDto>> GetAllAsync(string? make)
        => Task.FromResult(Enumerable.Empty<CarDto>());
}
