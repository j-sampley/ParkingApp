using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

using ParkingApp.Api.Utils;
using ParkingApp.Api.Services;
using ParkingApp.Common.Services;
using ParkingApp.Common.Models.User;
using ParkingApp.Common.Models.Authentication;

using Keys = ParkingApp.Common.Constants.Keys.Account;
using System.Text.Json;

namespace ParkingApp.Api.Controllers;

[ApiController]
[AllowAnonymous]
[Route("api/auth")]
public class AccountController : ControllerBase
{
    private readonly AccountService _accountService;
    private readonly SignInManager<UserDataModel> _signInManager;
    private readonly IConfiguration _configuration;
    private readonly ILogger _logger;
    private readonly ILocalizationService<AccountController> _localization;

    public AccountController(
        AccountService accountService,
        SignInManager<UserDataModel> signInManager,
        IConfiguration configuration,
        ILocalizationService<AccountController> localization,
        ILogger<AccountController> logger
        )
    {
        _accountService = accountService;
        _signInManager = signInManager;
        _configuration = configuration;
        _localization = localization;
        _logger = logger;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterModel model)
    {
        string message;
        var result = await _accountService.CreateUserAsync(model);

        if (result.Succeeded)
        {
            message = _localization.GetLocalizedString(Keys.Created, model.Email);
            _logger.LogInformation(message);
            return Ok();
        }
        message = _localization.GetLocalizedString(Keys.BadRegistration);
        var errors = JsonSerializer.Serialize(result.Errors);
        return BadRequest(result.Errors);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        string message;
        var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, isPersistent: true, lockoutOnFailure: true);

        if (result.Succeeded)
        {
            var user = await _accountService.LookupUserByEmailAsync(model.Email);

            if (user == null)
            {
                message = _localization.GetLocalizedString(Keys.NotFound);
                _logger.LogError(message);
                return Unauthorized(message);
            }

            try
            {
                var token = JwtHelper.GenerateToken(user, _configuration);

                LoginResponse response = new(token, user.Id);
                message = _localization.GetLocalizedString(Keys.LoggedIn, model.Email);
                _logger.LogInformation(message);
                return Ok(response);
            }
            catch (Exception ex)
            {
                message = _localization.GetLocalizedString(Keys.TokenGenerationError);
                _logger.LogError(ex, message);
                return StatusCode(StatusCodes.Status500InternalServerError, message);
            }
        }
        else if (result.IsLockedOut)
        {
            message = _localization.GetLocalizedString(Keys.Locked);
            _logger.LogError(message);
            return StatusCode(StatusCodes.Status403Forbidden, message);
        }

        message = _localization.GetLocalizedString(Keys.InvalidLogin);
        _logger.LogError(message);
        return Unauthorized(message);
    }

    [Authorize]
    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetUserById(string userId)
    {
        var user = await _accountService.LookupUserByIdAsync(userId);

        if (user == null)
        {
            var message = _localization.GetLocalizedString(Keys.NotFound);
            _logger.LogError(message);
            return NotFound(message);
        }

        return Ok(user);
    }

    [Authorize]
    [HttpPut("user/{userId}/email")]
    public async Task<IActionResult> UpdateUserEmail(string userId, [FromBody] string email)
    {
        string message;

        var user = await _accountService.LookupUserByIdAsync(userId);
        if (user == null)
        {
            message = _localization.GetLocalizedString(Keys.NotFound);
            _logger.LogError(message);
            return NotFound(message);
        }

        var result = await _accountService.UpdateUserEmailAsync(user, email);
        if (result.Succeeded)
        {
            message = _localization.GetLocalizedString(Keys.Updated, email);
            _logger.LogInformation(message);
            return Ok(user); 
        }

        message = _localization.GetLocalizedString(Keys.UpdateFailed);
        _logger.LogError(message);
        return BadRequest(result.Errors);
    }

    [Authorize]
    [HttpPut("user/{userId}/password")]
    public async Task<IActionResult> UpdateUserPassword(string userId, [FromBody] UpdatePasswordModel passwordModel)
    {
        string message;

        var user = await _accountService.LookupUserByIdAsync(userId);
        if (user == null)
        {
            message = _localization.GetLocalizedString(Keys.NotFound);
            _logger.LogError(message);
            return NotFound(message);
        }

        var result = await _accountService.UpdateUserPasswordAsync(user, passwordModel.CurrentPassword, passwordModel.NewPassword);
        if (result.Succeeded)
        {
            message = _localization.GetLocalizedString(Keys.Updated, user.Email);
            _logger.LogInformation(message);
            return Ok(user);
        }

        message = _localization.GetLocalizedString(Keys.UpdateFailed);
        _logger.LogError(message);
        return BadRequest(result.Errors);
    }

    [Authorize]
    [HttpPut("user/{userId}/address")]
    public async Task<IActionResult> UpdateUserAddress(string userId, [FromBody] Address newAddress)
    {
        string message;

        var user = await _accountService.LookupUserByIdAsync(userId);
        if (user == null)
        {
            message = _localization.GetLocalizedString(Keys.NotFound);
            _logger.LogError(message);
            return NotFound(message);
        }

        var result = await _accountService.UpdateUserAddressAsync(user, newAddress);
        if (result.Succeeded)
        {
            message = _localization.GetLocalizedString(Keys.Updated, user.Email);
            _logger.LogInformation(message);
            return Ok(user);
        }

        message = _localization.GetLocalizedString(Keys.UpdateFailed);
        _logger.LogError(message);
        return BadRequest(result.Errors);
    }

    [Authorize]
    [HttpDelete("user/{id}")]
    public async Task<IActionResult> DeleteAccount(string id)
    {
        string message;

        var user = await _accountService.LookupUserByIdAsync(id);
        if (user == null)
        {
            message = _localization.GetLocalizedString(Keys.NotFound);
            _logger.LogError(message);
            return NotFound(message);
        }

        var result = await _accountService.DeleteUserAsync(user);
        if (result.Succeeded)
        {
            message = _localization.GetLocalizedString(Keys.Deleted, user.Id);
            _logger.LogInformation(message);
            return NoContent();
        }

        message = _localization.GetLocalizedString(Keys.DeletionFailed);
        _logger.LogError(message);
        return BadRequest(result.Errors);
    }
}