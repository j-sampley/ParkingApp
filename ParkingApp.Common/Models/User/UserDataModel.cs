using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ParkingApp.Common.Models.User;

public class UserDataModel : IdentityUser
{
    [Key]
    public int Key { get; set; }
    public required Address Address { get; set; }
    public required List<Contact> Contacts { get; set; }
    public required List<Vehicle> Vehicles { get; set; }
}