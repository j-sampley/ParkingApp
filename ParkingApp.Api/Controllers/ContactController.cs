using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ParkingApp.Api.Services;
using ParkingApp.Common.Models.User;


namespace ParkingApp.Api.Controllers;

[AllowAnonymous]
[ApiController]
[Route("api/contact")]
public class ContactController : ControllerBase
{
    private readonly DbService _dbService;

    public ContactController(DbService dbService)
    {
        _dbService = dbService;
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Contact>> GetContactById(int id)
    {
        var contact = await _dbService.GetContactByIdAsync(id);

        if (contact == null)
        {
            return NotFound();
        }

        return Ok(contact);
    }

    [HttpGet("{userKey}")]
    public async Task<ActionResult<List<Contact>>> GetContactsByUserKey(string userKey)
    {
        var contacts = await _dbService.GetContactsByUserIdAsync(userKey);

        if (contacts == null || !contacts.Any())
        {
            return NotFound();
        }

        return Ok(contacts);
    }

    [HttpPost]
    public async Task<ActionResult<Contact>> CreateContact([FromBody] Contact newContact)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var createdContact = await _dbService.CreateContactAsync(newContact);
        return CreatedAtAction(nameof(GetContactById), new { id = createdContact.Id }, createdContact);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateContact(int id, [FromBody] Contact updatedContact)
    {
        if (id != updatedContact.Id)
        {
            return BadRequest("Contact ID mismatch");
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _dbService.UpdateContactAsync(updatedContact);

        if (!result)
        {
            return NotFound("Contact not found.");
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteContact(int id)
    {
        var result = await _dbService.DeleteContactAsync(id);

        if (!result)
        {
            return NotFound("Contact not found.");
        }

        return NoContent();
    }
}