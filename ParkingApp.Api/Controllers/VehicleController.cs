using Microsoft.AspNetCore.Mvc;

using ParkingApp.Api.Services;
using ParkingApp.Common.Models.User;

using Keys = ParkingApp.Common.Constants.Keys.Vehicle;

namespace ParkingApp.Api.Controllers;

[ApiController]
[Route("api/vehicle")]
public class VehicleController : ControllerBase
{
    private readonly SQLService _dbService;
    private readonly ILocalizationService<VehicleController> _localization;
    private readonly ILogger _logger;

    public VehicleController(
        SQLService dbService,
        ILocalizationService<VehicleController> localization,
        ILogger logger)
    {
        _dbService = dbService;
        _localization = localization;
        _logger = logger;
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Vehicle>> GetVehicleById(int id)
    {
        string message;
        var Vehicle = await _dbService.GetVehicleByIdAsync(id);

        if (Vehicle == null)
        {
            message = _localization.GetLocalizedString(Keys.NotFound, id);
            _logger.LogError(message);
            return NotFound();
        }

        message = _localization.GetLocalizedString(Keys.NotFound, id);
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

    [HttpPost]
    public async Task<ActionResult<Vehicle>> CreateVehicle([FromBody] Vehicle newVehicle)
    {
        string message;
        if (!ModelState.IsValid)
        {
            message = _localization.GetLocalizedString(Keys.InvalidVehicle);
            _logger.LogError(message);
            return BadRequest(ModelState);
        }

        var createdVehicle = await _dbService.CreateVehicleAsync(newVehicle);

        message = _localization.GetLocalizedString(Keys.Created, newVehicle.Make, newVehicle.Model, newVehicle.Year);
        _logger.LogInformation(message);
        return CreatedAtAction(nameof(GetVehicleById), new { id = createdVehicle.Id }, createdVehicle);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateVehicle(int id, [FromBody] Vehicle updatedVehicle)
    {
        string message;
        if (id != updatedVehicle.Id)
        {
            message = _localization.GetLocalizedString(Keys.IDMismatch);
            _logger.LogError(message);
            return BadRequest(message);
        }

        if (!ModelState.IsValid)
        {
            message = _localization.GetLocalizedString(Keys.InvalidVehicle);
            _logger.LogError(message);
            return BadRequest(ModelState);
        }

        var result = await _dbService.UpdateVehicleAsync(updatedVehicle);

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
    public async Task<IActionResult> DeleteVehicle(int id)
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