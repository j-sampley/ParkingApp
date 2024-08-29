using System.ComponentModel.DataAnnotations;

using ParkingApp.Common.Models.User;

namespace ParkingApp.Common.Models.Authentication;

public class LoginModel
{
    public LoginModel()
    {
    }

    public LoginModel(string email, string password)
    {
        Email = email;
        Password = password;
    }

    [Required]
    [EmailAddress]
    public required string Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public required string Password { get; set; }
}

public record LoginResponse(string Token, string Id);

public record RegisterModel()
{
    public required string Email { get; set; }
    public required string Password { get; set; }
    public required AddressBase Address { get; set; }
    [MinLength(1, ErrorMessage = "At least one contact is required.")]
    public required List<ContactBase> Contacts { get; set; }
    [MinLength(1, ErrorMessage = "At least one vehicle is required.")]
    public required List<VehicleBase> Vehicles { get; set; }
}

public class UpdatePasswordModel
{
    public string CurrentPassword { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
}