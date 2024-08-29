using Microsoft.AspNetCore.Identity;
using ParkingApp.Common.Models.Authentication;

namespace ParkingApp.Common.Models.User;

public class UserDataModel : IdentityUser
{
    public required Address Address { get; set; }
    public required List<Contact> Contacts { get; set; }
    public required List<Vehicle> Vehicles { get; set; }
}

public class UserDataModelFactory
{
    public static UserDataModel CreateUser(RegisterModel model)
    {
        var userId = Guid.NewGuid().ToString();

        return new UserDataModel
        {
            Id = userId,
            UserName = model.Email,
            Email = model.Email,
            Address = new Address
            {
                Id = Guid.NewGuid().ToString(),
                Address1 = model.Address.Address1,
                Address2 = model.Address.Address2,
                City = model.Address.City,
                State = model.Address.State,
                PostalCode = model.Address.PostalCode,
                Country = model.Address.Country,
                UserId = userId
            },
            Contacts = model.Contacts.Select(c => new Contact
            {
                Id = Guid.NewGuid().ToString(),
                FirstName = c.FirstName,
                LastName = c.LastName,
                PhoneNumber = c.PhoneNumber,
                Email = c.Email,
                Relationship = c.Relationship,
                UserKey = userId
            }).ToList(),
            Vehicles = model.Vehicles.Select(v => new Vehicle
            {
                Id = Guid.NewGuid().ToString(),
                Make = v.Make,
                Model = v.Model,
                Year = v.Year,
                Color = v.Color,
                LicensePlate = v.LicensePlate,
                UserKey = userId
            }).ToList()
        };
    }
}
