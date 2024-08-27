using System.ComponentModel.DataAnnotations;

namespace ParkingApp.Common.Models.User;

public class Contact
{
    public int Id { get; set; }
    public string UserKey { get; set; } // Foreign key
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Relationship { get; set; } = string.Empty;
}