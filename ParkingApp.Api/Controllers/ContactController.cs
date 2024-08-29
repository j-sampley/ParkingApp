using Microsoft.AspNetCore.Mvc;

using ParkingApp.Api.Services;
using ParkingApp.Common.Services;
using ParkingApp.Common.Models.User;

using Keys = ParkingApp.Common.Constants.Keys.Contact;
using Microsoft.AspNetCore.Authorization;

namespace ParkingApp.Api.Controllers;

[AllowAnonymous]
[ApiController]
[Route("api/contact")]
public class ContactController : ControllerBase
{
    private readonly ISQLService _dbService;
    private readonly ILocalizationService<ContactController> _localization;
    private readonly ILogger<ContactController> _logger;

    public ContactController(
        ISQLService dbService,
        ILocalizationService<ContactController> localization,
        ILogger<ContactController> logger)
    {
        _dbService = dbService;
        _logger = logger;
        _localization = localization;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Contact>> GetContactById(string id)
    {
        string message;
        var contact = await _dbService.GetContactByIdAsync(id);

        if (contact == null)
        {
            message = _localization.GetLocalizedString(Keys.NotFound, id);
            _logger.LogError(message);
            return NotFound();
        }

        message = _localization.GetLocalizedString(Keys.Found, id);
        _logger.LogInformation(message);
        return Ok(contact);
    }

    [HttpGet("{userKey}")]
    public async Task<ActionResult<List<Contact>>> GetContactsByUserId(string userId)
    {
        string message;
        var contacts = await _dbService.GetContactsByUserIdAsync(userId);

        if (contacts == null || !contacts.Any())
        {
            message = _localization.GetLocalizedString(Keys.NotFoundPlural, userId);
            _logger.LogError(message);
            return NotFound();
        }

        message = _localization.GetLocalizedString(Keys.FoundPlural, userId);
        _logger.LogInformation(message);
        return Ok(contacts);
    }

    [HttpPost("{id}")]
    public async Task<ActionResult> CreateContact(string id, [FromBody] ContactBase newContact)
    {
        string message;
        if (!ModelState.IsValid)
        {
            message = _localization.GetLocalizedString(Keys.InvalidContact);
            _logger.LogError(message);
            return BadRequest(ModelState);
        }

        var builtContact = ContactFactory.BuildContact(newContact, id);

        await _dbService.CreateContactAsync(builtContact);

        message = _localization.GetLocalizedString(Keys.Created, newContact.Email);
        _logger.LogInformation(message);
        return Ok();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateContact(string id, [FromBody] ContactBase updatedContact)
    {
        string message;
        if (!ModelState.IsValid)
        {
            message = _localization.GetLocalizedString(Keys.InvalidContact);
            _logger.LogError(message);
            return BadRequest(ModelState);
        }

        var result = await _dbService.UpdateContactAsync(id, updatedContact);

        if (!result)
        {
            message = _localization.GetLocalizedString(Keys.NotFound, id);
            _logger.LogError(message);
            return NotFound(message);
        }

        message = _localization.GetLocalizedString(Keys.Updated, id);
        _logger.LogInformation(message);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteContact(string id)
    {
        string message;
        var result = await _dbService.DeleteContactAsync(id);

        if (!result)
        {
            message = _localization.GetLocalizedString(Keys.NotFound, id);
            _logger.LogError(message);
            return NotFound(message);
        }

        message = _localization.GetLocalizedString(Keys.Deleted, id);
        _logger.LogInformation(message);
        return NoContent();
    }
}