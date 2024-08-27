using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace ParkingApp.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class SecureController : ControllerBase
{
    [HttpGet("data")]
    public IActionResult GetSecureData()
    {
        return Ok("Testing!");
    }
}