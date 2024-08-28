using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ParkingApp.Api.Services;
using ParkingApp.Common.Models.User;

namespace ParkingApp.Api.Controllers;

[AllowAnonymous]
[ApiController]
[Route("api/vehicle")]
public class VehicleController : ControllerBase
{
    private readonly DbService _dbService;

    public VehicleController(DbService dbService)
    {
        _dbService = dbService;
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Vehicle>> GetVehicleById(int id)
    {
        var Vehicle = await _dbService.GetVehicleByIdAsync(id);

        if (Vehicle == null)
        {
            return NotFound();
        }

        return Ok(Vehicle);
    }

    [HttpGet("{userKey}")]
    public async Task<ActionResult<List<Vehicle>>> GetVehiclesByUserKey(string userKey)
    {
        var Vehicles = await _dbService.GetVehiclesByUserIdAsync(userKey);

        if (Vehicles == null || !Vehicles.Any())
        {
            return NotFound();
        }

        return Ok(Vehicles);
    }

    [HttpPost]
    public async Task<ActionResult<Vehicle>> CreateVehicle([FromBody] Vehicle newVehicle)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var createdVehicle = await _dbService.CreateVehicleAsync(newVehicle);
        return CreatedAtAction(nameof(GetVehicleById), new { id = createdVehicle.Id }, createdVehicle);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateVehicle(int id, [FromBody] Vehicle updatedVehicle)
    {
        if (id != updatedVehicle.Id)
        {
            return BadRequest("Vehicle ID mismatch");
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _dbService.UpdateVehicleAsync(updatedVehicle);

        if (!result)
        {
            return NotFound("Vehicle not found.");
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteVehicle(int id)
    {
        var result = await _dbService.DeleteVehicleAsync(id);

        if (!result)
        {
            return NotFound("Vehicle not found.");
        }

        return NoContent();
    }
}