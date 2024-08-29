using System.ComponentModel.DataAnnotations;

namespace ParkingApp.Common.Models.User;

public record Address : AddressBase
{
    [Key]
    public string Id { get; set; }
    public string UserId { get; set; } // Foreign key
}

public record AddressBase()
{
	public string Address1 { get; set; } = string.Empty;
	public string Address2 { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string PostalCode { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
}