using Microsoft.AspNetCore.Mvc;

using ParkingApp.Api.Services;
using ParkingApp.Common.Models.User;

namespace ParkingApp.Api.Controllers;

[ApiController]
[Route("api/address")]
public class AddressController : ControllerBase
{
    private readonly SQLService _dbService;

    public AddressController(SQLService dbService)
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
