using System.ComponentModel.DataAnnotations;

namespace ParkingApp.Common.Models.User;

public class Contact: ContactBase
{
    [Key]
    public string Id { get; set; }
    public string UserKey { get; set; } // Foreign key
}

public class ContactBase 
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Relationship { get; set; } = string.Empty;
}

public class ContactFactory
{
    public static Contact BuildContact(ContactBase model, string userId)
    {
        return new Contact
        {
            Id = Guid.NewGuid().ToString(),
            FirstName = model.FirstName,
            LastName = model.LastName,
            PhoneNumber = model.PhoneNumber,
            Email = model.Email,
            Relationship = model.Relationship,
            UserKey = userId
        };
    }
}