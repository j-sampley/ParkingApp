using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

using ParkingApp.Api.Utils;
using ParkingApp.Api.Services;
using ParkingApp.Common.Models.User;
using ParkingApp.Common.Models.Authentication;

namespace ParkingApp.Api.Controllers;

[ApiController]
[AllowAnonymous]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly AccountService _accountService;
    private readonly SignInManager<UserDataModel> _signInManager;
    private readonly IConfiguration _configuration;
    private readonly ILocalizationService<AuthController> _localization;

    public AuthController(
        AccountService accountService,
        SignInManager<UserDataModel> signInManager,
        IConfiguration configuration,
        ILocalizationService<AuthController> localization
        )
    {
        _accountService = accountService;
        _signInManager = signInManager;
        _configuration = configuration;
        _localization = localization;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _accountService.CreateUserAsync(model);

        if (result.Succeeded)
        {
            return Ok();
        }
        return BadRequest(result.Errors);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, isPersistent: true, lockoutOnFailure: true);

        if (result.Succeeded)
        {
            var user = await _accountService.LookupUserAsync(model.Email);

            if (user == null)
            {
                return Unauthorized(_localization.GetLocalizedString("UserNotFound"));
            }

            try
            {
                var token = JwtHelper.GenerateToken(user, _configuration);
                LoginResponse response = new(token, user);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, _localization.GetLocalizedString("TokenGenerationError"));
            }
        }
        else if (result.IsLockedOut)
        {
            return StatusCode(StatusCodes.Status403Forbidden, _localization.GetLocalizedString("UserAccountLocked"));
        }

        return Unauthorized(_localization.GetLocalizedString("InvalidLogin"));
    }
}