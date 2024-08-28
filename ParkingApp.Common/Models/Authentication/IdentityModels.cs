using System.ComponentModel.DataAnnotations;

using ParkingApp.Common.Models.User;

namespace ParkingApp.Common.Models.Authentication;

public record LoginModel(string Email, string Password);

public record RegisterModel()
{
    public required string Email { get; set; }
    public required string Password { get; set; }
    public required Address Address { get; set; }
    [MinLength(1, ErrorMessage = "At least one contact is required.")]
    public required List<Contact> Contacts { get; set; }
    [MinLength(1, ErrorMessage = "At least one vehicle is required.")]
    public required List<Vehicle> Vehicles { get; set; }
}

public record UpdateEmailModel
{
    public required string NewEmail { get; set; }
}

public record UpdatePasswordModel
{
    public required string CurrentPassword { get; set; }
    public required string NewPassword { get; set; }
}
