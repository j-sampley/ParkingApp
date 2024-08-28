using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

using ParkingApp.Api.Utils;
using ParkingApp.Api.Services;
using ParkingApp.Common.Models.User;
using ParkingApp.Common.Models.Authentication;

using Keys = ParkingApp.Common.Constants.Keys.Account;

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
        if (!ModelState.IsValid)
        {
            message = _localization.GetLocalizedString(Keys.InvalidRegistration);
            _logger.LogError(message);
            return BadRequest(ModelState);
        }

        var result = await _accountService.CreateUserAsync(model);

        if (result.Succeeded)
        {
            message = _localization.GetLocalizedString(Keys.Created, model.Email);
            _logger.LogInformation(message);
            return Ok();
        }
        _logger.LogError(Keys.BadRegistration);
        return BadRequest(result.Errors);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        string message;
        var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, isPersistent: true, lockoutOnFailure: true);

        if (result.Succeeded)
        {
            var user = await _accountService.LookupUserAsync(model.Email);

            if (user == null)
            {
                message = _localization.GetLocalizedString(Keys.NotFound);
                _logger.LogError(message);
                return Unauthorized(message);
            }

            try
            {
                var token = JwtHelper.GenerateToken(user, _configuration);
                LoginResponse response = new(token, user);
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

    [HttpPut("update-email/{email}")]
    public async Task<IActionResult> UpdateEmail(string email, [FromBody] UpdateEmailModel model)
    {
        string message;

        var user = await _accountService.LookupUserAsync(email);
        if (user == null)
        {
            message = _localization.GetLocalizedString(Keys.NotFound);
            _logger.LogError(message);
            return NotFound(message);
        }

        var result = await _accountService.UpdateUserEmailAsync(user, model.NewEmail);
        if (result.Succeeded)
        {
            message = _localization.GetLocalizedString(Keys.EmailUpdated, user.Email!, model.NewEmail);
            _logger.LogInformation(message);
            return NoContent();
        }

        message = _localization.GetLocalizedString(Keys.EmailUpdateFailed);
        _logger.LogError(message);
        return BadRequest(result.Errors);
    }

    [HttpPut("update-password/{email}")]
    public async Task<IActionResult> UpdatePassword(string email, [FromBody] UpdatePasswordModel model)
    {
        string message;

        var user = await _accountService.LookupUserAsync(email);
        if (user == null)
        {
            message = _localization.GetLocalizedString(Keys.NotFound);
            _logger.LogError(message);
            return NotFound(message);
        }

        var result = await _accountService.UpdateUserPasswordAsync(user, model.CurrentPassword, model.NewPassword);
        if (result.Succeeded)
        {
            message = _localization.GetLocalizedString(Keys.PasswordUpdated);
            _logger.LogInformation(message);
            return NoContent();
        }

        message = _localization.GetLocalizedString(Keys.PasswordUpdateFailed);
        _logger.LogError(message);
        return BadRequest(result.Errors);
    }

    [HttpDelete("delete/{email}")]
    public async Task<IActionResult> DeleteAccount(string email)
    {
        string message;

        var user = await _accountService.LookupUserAsync(email);
        if (user == null)
        {
            message = _localization.GetLocalizedString(Keys.NotFound);
            _logger.LogError(message);
            return NotFound(message);
        }

        var result = await _accountService.DeleteUserAsync(user);
        if (result.Succeeded)
        {
            message = _localization.GetLocalizedString(Keys.Deleted, email);
            _logger.LogInformation(message);
            return NoContent();
        }

        message = _localization.GetLocalizedString(Keys.DeletionFailed);
        _logger.LogError(message);
        return BadRequest(result.Errors);
    }
}