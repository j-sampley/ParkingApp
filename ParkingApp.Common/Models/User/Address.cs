namespace ParkingApp.Common.Models.User;

public record Address()
{
    public int Id { get; set; }
    public string UserId { get; set; } // Foreign key
    public string Street { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string PostalCode { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
}