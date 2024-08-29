using ParkingApp.Common.Models.Authentication;
using System.ComponentModel.DataAnnotations;

namespace ParkingApp.Common.Models.User;

public record Vehicle : VehicleBase
{
    [Key]
    public string Id { get; set; }
    public string UserKey { get; set; } // Foreign key
}

public record VehicleBase 
{
    public string Make { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public int Year { get; set; }
    public string Color { get; set; } = string.Empty;
    public string LicensePlate { get; set; } = string.Empty;
}

public class VehicleFactory
{
    public static Vehicle BuildVehicle(VehicleBase model, string userId)
    {
        return new Vehicle
        {
            Id = Guid.NewGuid().ToString(),
            Make = model.Make,
            Model = model.Model,
            Year = model.Year,
            Color = model.Color,
            LicensePlate = model.LicensePlate,
            UserKey = userId
        };
    }
}