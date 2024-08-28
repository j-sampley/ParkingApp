using Microsoft.AspNetCore.Mvc;

using ParkingApp.Api.Services;
using ParkingApp.Common.Models.User;

using Keys = ParkingApp.Common.Constants.Keys.Contact;

namespace ParkingApp.Api.Controllers;

[ApiController]
[Route("api/contact")]
public class ContactController : ControllerBase
{
    private readonly SQLService _dbService;
    private readonly ILocalizationService<ContactController> _localization;
    private readonly ILogger _logger;

    public ContactController(
        SQLService dbService,
        ILocalizationService<ContactController> localization,
        ILogger logger)
    {
        _dbService = dbService;
        _logger = logger;
        _localization = localization;
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Contact>> GetContactById(int id)
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

    [HttpPost]
    public async Task<ActionResult<Contact>> CreateContact([FromBody] Contact newContact)
    {
        string message;
        if (!ModelState.IsValid)
        {
            message = _localization.GetLocalizedString(Keys.InvalidContact);
            _logger.LogError(message);
            return BadRequest(ModelState);
        }

        var createdContact = await _dbService.CreateContactAsync(newContact);

        message = _localization.GetLocalizedString(Keys.Created, newContact.Email);
        _logger.LogInformation(message);

        return CreatedAtAction(nameof(GetContactById), new { id = createdContact.Id }, createdContact);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateContact(int id, [FromBody] Contact updatedContact)
    {
        string message;
        if (id != updatedContact.Id)
        {
            message = _localization.GetLocalizedString(Keys.IDMismatch);
            _logger.LogError(message);
            return BadRequest(message);
        }

        if (!ModelState.IsValid)
        {
            message = _localization.GetLocalizedString(Keys.InvalidContact);
            _logger.LogError(message);
            return BadRequest(ModelState);
        }

        var result = await _dbService.UpdateContactAsync(updatedContact);

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
    public async Task<IActionResult> DeleteContact(int id)
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