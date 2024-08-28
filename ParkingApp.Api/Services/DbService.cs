using Microsoft.EntityFrameworkCore;
using ParkingApp.Api.Database;
using ParkingApp.Common.Models.User;

namespace ParkingApp.Api.Services;

public class DbService
{
    private readonly ParkingDbContext _context;

    public DbService(ParkingDbContext context)
    {
        _context = context;
    }

    // Addresses begin
    public async Task<Address?> GetAddressByUserIdAsync(string userId)
    {
        return await _context.Addresses.SingleOrDefaultAsync(a => a.UserId == userId);
    }

    public async Task<Address?> GetAddressByIdAsync(int id)
    {
        return await _context.Addresses.FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<bool> UpdateAddressAsync(Address updatedAddress)
    {
        var existingAddress = await _context.Addresses
            .FirstOrDefaultAsync(a => a.Id == updatedAddress.Id);

        if (existingAddress == null)
        {
            return false;
        }

        existingAddress.Street = updatedAddress.Street;
        existingAddress.City = updatedAddress.City;
        existingAddress.State = updatedAddress.State;
        existingAddress.PostalCode = updatedAddress.PostalCode;
        existingAddress.Country = updatedAddress.Country;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAddressByIdAsync(int id)
    {
        var address = await _context.Addresses.FirstOrDefaultAsync(a => a.Id == id);

        if (address == null)
        {
            return false;
        }

        _context.Addresses.Remove(address);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAddressByUserIdAsync(string userId)
    {
        var address = await _context.Addresses.FirstOrDefaultAsync(a => a.UserId == userId);

        if (address == null)
        {
            return false; 
        }

        _context.Addresses.Remove(address);
        await _context.SaveChangesAsync();
        return true;
    }
    // Addresses end

    // Contacts begin
    public async Task<Contact?> GetContactByIdAsync(int id)
    {
        return await _context.Contacts
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<List<Contact>> GetContactsByUserIdAsync(string userKey)
    {
        return await _context.Contacts
            .Where(c => c.UserKey == userKey)
            .ToListAsync();
    }

    public async Task<Contact> CreateContactAsync(Contact newContact)
    {
        _context.Contacts.Add(newContact);
        await _context.SaveChangesAsync();
        return newContact;
    }

    public async Task<bool> UpdateContactAsync(Contact updatedContact)
    {
        var existingContact = await _context.Contacts.FirstOrDefaultAsync(c => c.Id == updatedContact.Id);

        if (existingContact == null)
        {
            return false;
        }

        existingContact.FirstName = updatedContact.FirstName;
        existingContact.LastName = updatedContact.LastName;
        existingContact.PhoneNumber = updatedContact.PhoneNumber;
        existingContact.Email = updatedContact.Email;
        existingContact.Relationship = updatedContact.Relationship;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteContactAsync(int id)
    {
        var contact = await _context.Contacts.FirstOrDefaultAsync(c => c.Id == id);

        if (contact == null)
        {
            return false;
        }

        _context.Contacts.Remove(contact);
        await _context.SaveChangesAsync();
        return true;
    }
    // Contacts end

    // Vehicles begin
    public async Task<Vehicle?> GetVehicleByIdAsync(int id)
    {
        return await _context.Vehicles
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<List<Vehicle>> GetVehiclesByUserIdAsync(string userKey)
    {
        return await _context.Vehicles
            .Where(c => c.UserKey == userKey)
            .ToListAsync();
    }

    public async Task<Vehicle> CreateVehicleAsync(Vehicle newVehicle)
    {
        _context.Vehicles.Add(newVehicle);
        await _context.SaveChangesAsync();
        return newVehicle;
    }

    public async Task<bool> UpdateVehicleAsync(Vehicle updatedVehicle)
    {
        var existingVehicle = await _context.Vehicles.FirstOrDefaultAsync(c => c.Id == updatedVehicle.Id);

        if (existingVehicle == null)
        {
            return false;
        }

        existingVehicle.Make = updatedVehicle.Make;
        existingVehicle.Model = updatedVehicle.Model;
        existingVehicle.Year = updatedVehicle.Year;
        existingVehicle.Color = updatedVehicle.Color;
        existingVehicle.LicensePlate = updatedVehicle.LicensePlate;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteVehicleAsync(int id)
    {
        var vehicle = await _context.Vehicles.FirstOrDefaultAsync(c => c.Id == id);

        if (vehicle == null)
        {
            return false;
        }

        _context.Vehicles.Remove(vehicle);
        await _context.SaveChangesAsync();
        return true;
    }
    // Vehicles end
}
