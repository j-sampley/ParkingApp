using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ParkingApp.Api.Database;
using ParkingApp.Api.Services;
using ParkingApp.Common.Models.User;

namespace ParkingApp.Api.Controllers;

[AllowAnonymous]
[ApiController]
[Route("api/address")]
public class AddressController : ControllerBase
{
    private readonly DbService _dbService;

    public AddressController(DbService dbService)
    {
        _dbService = dbService;
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Address>> GetAddress(int id)
    {
        var address = await _dbService.GetAddressByIdAsync(id);

        if (address == null)
        {
            return NotFound();
        }

        return address;
    }

    [HttpGet("{userId}")]
    public async Task<ActionResult<Address>> GetAddress(string userId)
    {
        var address = await _dbService.GetAddressByUserIdAsync(userId);

        if (address == null)
        {
            return NotFound();
        }

        return address;
    }

    [HttpPut("address")]
    public async Task<IActionResult> UpdateAddress([FromBody] Address updatedAddress)
    {
        var result = await _dbService.UpdateAddressAsync(updatedAddress);

        if (!result)
        {
            return NotFound("Address not found.");
        }

        return NoContent();
    }
}
