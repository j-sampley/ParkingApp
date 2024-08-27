using System.ComponentModel.DataAnnotations;

namespace ParkingApp.Common.Models.User;

public record Vehicle
{
    public int Id { get; set; }
    public string UserKey { get; set; } // Foreign key
    public string Make { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public int Year { get; set; }
    public string Color { get; set; } = string.Empty;
    public string LicensePlate { get; set; } = string.Empty;
}