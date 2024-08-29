using Microsoft.AspNetCore.Mvc;

using ParkingApp.Api.Services;
using ParkingApp.Common.Services;
using ParkingApp.Common.Models.User;

using Keys = ParkingApp.Common.Constants.Keys.Vehicle;
using Microsoft.AspNetCore.Authorization;

namespace ParkingApp.Api.Controllers;

[AllowAnonymous]
[ApiController]
[Route("api/vehicle")]
public class VehicleController : ControllerBase
{
    private readonly ISQLService _dbService;
    private readonly ILocalizationService<VehicleController> _localization;
    private readonly ILogger<Vehicle> _logger;

    public VehicleController(
        ISQLService dbService,
        ILocalizationService<VehicleController> localization,
        ILogger<Vehicle> logger)
    {
        _dbService = dbService;
        _localization = localization;
        _logger = logger;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Vehicle>> GetVehicleById(string id)
    {
        string message;
        var Vehicle = await _dbService.GetVehicleByIdAsync(id);

        if (Vehicle == null)
        {
            message = _localization.GetLocalizedString(Keys.NotFound, id);
            _logger.LogError(message);
            return NotFound();
        }

        message = _localization.GetLocalizedString(Keys.Found, id);
        _logger.LogInformation(message);
        return Ok(Vehicle);
    }

    [HttpGet("{userKey}")]
    public async Task<ActionResult<List<Vehicle>>> GetVehiclesByUserKey(string userKey)
    {
        string message;
        var Vehicles = await _dbService.GetVehiclesByUserIdAsync(userKey);

        if (Vehicles == null || !Vehicles.Any())
        {
            message = _localization.GetLocalizedString(Keys.NotFoundPlural, userKey);
            _logger.LogError(message);
            return NotFound();
        }

        message = _localization.GetLocalizedString(Keys.FoundPlural, userKey);
        _logger.LogInformation(message, userKey);
        return Ok(Vehicles);
    }

    [HttpPost("{id}")]
    public async Task<ActionResult> CreateVehicle(string id, [FromBody] VehicleBase newVehicle)
    {
        string message;
        if (!ModelState.IsValid)
        {
            message = _localization.GetLocalizedString(Keys.InvalidVehicle);
            _logger.LogError(message);
            return BadRequest(ModelState);
        }

        var builtVehicle = VehicleFactory.BuildVehicle(newVehicle, id);

        await _dbService.CreateVehicleAsync(builtVehicle);

        message = _localization.GetLocalizedString(Keys.Created, newVehicle.Make, newVehicle.Model, newVehicle.Year);
        _logger.LogInformation(message);
        return Ok();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateVehicle(string id, [FromBody] VehicleBase updatedVehicle)
    {
        string message;

        if (!ModelState.IsValid)
        {
            message = _localization.GetLocalizedString(Keys.InvalidVehicle);
            _logger.LogError(message);
            return BadRequest(ModelState);
        }

        var result = await _dbService.UpdateVehicleAsync(id, updatedVehicle);

        if (!result)
        {
            message = _localization.GetLocalizedString(Keys.NotFound, id);
            _logger.LogError(message);
            return NotFound(message);
        }

        message = _localization.GetLocalizedString(Keys.Updated, id);
        _logger.LogInformation(message);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteVehicle(string id)
    {
        string message;
        var result = await _dbService.DeleteVehicleAsync(id);

        if (!result)
        {
            message = _localization.GetLocalizedString(Keys.NotFound, id);
            _logger.LogError(message);
            return NotFound(message);
        }

        message = _localization.GetLocalizedString(Keys.Deleted, id);
        _logger.LogInformation(message);
        return NoContent();
    }
}