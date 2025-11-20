using Fleet.Api.Models;
using Fleet.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Fleet.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CarsController(ICarRepository repository) : ControllerBase {

    // GET /api/cars?make=Toyota
    [HttpGet]
    public ActionResult<IEnumerable<Car>> GetCars([FromQuery] string? make) {
        var cars = repository.GetAll(make);
        return Ok(cars);
    }
}
