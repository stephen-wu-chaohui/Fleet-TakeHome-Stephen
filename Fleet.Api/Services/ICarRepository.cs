using Fleet.Api.Models;

namespace Fleet.Api.Services;

public interface ICarRepository {
    IEnumerable<Car> GetAll(string? make = null);

    /// <summary>
    /// Tracks the timestamp of the last registration-expiry scan.
    /// 
    /// RegistrationCheckService uses this value to compute a delta window
    /// (LastCheckPoint → now) and only emit events for vehicles whose
    /// expiry timestamp falls inside that window.
    ///
    /// This enables efficient incremental processing and avoids re-checking
    /// the entire fleet every cycle, which is important when scaling
    /// beyond a few thousand vehicles.
    /// </summary>
    DateTime LastCheckPoint { get; set; }
}
