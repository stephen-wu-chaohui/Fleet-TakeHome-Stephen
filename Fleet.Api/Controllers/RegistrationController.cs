using Fleet.Api.Models;
using Fleet.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Fleet.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RegistrationController(ICarRepository repository) : ControllerBase {

    // GET /api/registration
    [HttpGet]
    public ActionResult<IEnumerable<CarRegistrationStatus>> GetRegistrationStatus() {
        var now = DateTime.UtcNow;

        var statuses = repository.GetAll()
            .Select(c => new CarRegistrationStatus {
                CarId = c.Id,
                RegistrationNumber = c.RegistrationNumber,
                RegistrationExpiry = c.RegistrationExpiry,
                IsExpired = c.RegistrationExpiry <= now
            });

        repository.LastCheckPoint = now;

        return Ok(statuses);
    }
}
