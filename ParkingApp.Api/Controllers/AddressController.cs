using Microsoft.AspNetCore.Mvc;

using ParkingApp.Api.Services;
using ParkingApp.Common.Services;
using ParkingApp.Common.Models.User;

using Keys =  ParkingApp.Common.Constants.Keys.Address;

namespace ParkingApp.Api.Controllers;

[ApiController]
[Route("api/address")]
public class AddressController : ControllerBase
{
    private readonly SQLService _dbService;
    private readonly ILocalizationService<AddressController> _localization;
    private readonly ILogger _logger;

    public AddressController(
        SQLService dbService,
        ILocalizationService<AddressController> localization,
        ILogger logger)
    {
        _dbService = dbService;
        _localization = localization;
        _logger = logger;
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Address>> GetAddressById(string id)
    {
        string message;
        var address = await _dbService.GetAddressByIdAsync(id);

        if (address == null)
        {
            message = _localization.GetLocalizedString(Keys.NotFound);
            _logger.LogError(message);
            return NotFound();
        }

        message = _localization.GetLocalizedString(Keys.Found, id);
        _logger.LogInformation(message);
        return address;
    }

    [HttpGet("{userId}")]
    public async Task<ActionResult<Address>> GetAddressByUserId(string userId)
    {
        string message;
        var address = await _dbService.GetAddressByUserIdAsync(userId);

        if (address == null)
        {
            message = _localization.GetLocalizedString(Keys.NotFound);
            _logger.LogError(message);
            return NotFound();
        }

        message = _localization.GetLocalizedString(Keys.Found, userId);
        _logger.LogInformation(message);
        return address;
    }

    [HttpPut("address")]
    public async Task<IActionResult> UpdateAddress([FromBody] Address updatedAddress)
    {
        string message;
        var result = await _dbService.UpdateAddressAsync(updatedAddress);

        if (!result)
        {
            message = _localization.GetLocalizedString(Keys.NotFound);
            _logger.LogError(message);
            return NotFound();
        }

        message = _localization.GetLocalizedString(Keys.Updated, updatedAddress.UserId);
        _logger.LogInformation(message);
        return NoContent();
    }
}
