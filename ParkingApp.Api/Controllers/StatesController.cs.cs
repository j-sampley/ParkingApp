using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using ParkingApp.Common.Models;
using System.Text.Json;

namespace ParkingApp.Api.Controllers;

[AllowAnonymous]
[ApiController]
[Route("api/states")]
public class StatesController : ControllerBase
{
    private readonly IWebHostEnvironment _env;

    public StatesController(IWebHostEnvironment env)
    {
        _env = env;
    }

    [HttpGet]
    public async Task<ActionResult<List<string>>> GetStates()
    {
        var filePath = Path.Combine(_env.ContentRootPath, "Data", "states.json");

        if (!System.IO.File.Exists(filePath))
        {
            return NotFound("States file not found.");
        }

        try
        {
            var json = await System.IO.File.ReadAllTextAsync(filePath);
            var states = JsonSerializer.Deserialize<List<string>>(json);

            if (states == null || states.Count == 0)
            {
                return BadRequest("Failed to deserialize states or the list is empty.");
            }

            return Ok(states);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}