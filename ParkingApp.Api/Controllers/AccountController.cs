using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;

using ParkingApp.Common.Models.User;
using ParkingApp.Common.Models.Authentication;


namespace ParkingApp.Api.Controllers;

[ApiController]
[AllowAnonymous]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly UserManager<UserDataModel> _userManager;
    private readonly SignInManager<UserDataModel> _signInManager;
    private readonly IConfiguration _configuration;

    public AuthController(UserManager<UserDataModel> userManager, SignInManager<UserDataModel> signInManager, IConfiguration configuration)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        UserDataModel newUser = new()
        {
            UserName = model.Email,
            Email = model.Email,
            Address = model.Address,
            Contacts = model.Contacts,
            Vehicles = model.Vehicles
        };

        var result = await _userManager.CreateAsync(newUser, model.Password);

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
            // Grab user data for UI
            var user = await _userManager.Users
                .Include(u => u.Address)
                .Include(u => u.Contacts)
                .Include(u => u.Vehicles)
                .FirstOrDefaultAsync(u => u.Email == model.Email);

            if (user == null)
            {
                return Unauthorized("User not found.");
            }

            try
            {
                var token = GenerateJwtToken(user);
                LoginResponse response = new(token, user);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while generating the token.");
            }
        }
        else if (result.IsLockedOut)
        {
            return Forbid("User account locked out due to too many failed login attempts.");
        }

        return Unauthorized("Invalid login attempt.");
    }

    private string GenerateJwtToken(IdentityUser user)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.UserName ?? throw new InvalidOperationException("UserName is not set.")),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.Id ?? throw new InvalidOperationException("User ID is not set.")),
            new Claim(ClaimTypes.Email, user.Email ?? throw new InvalidOperationException("Email is not set."))
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key is not configured.")));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}