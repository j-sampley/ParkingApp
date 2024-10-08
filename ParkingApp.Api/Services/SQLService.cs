﻿using Microsoft.EntityFrameworkCore;
using ParkingApp.Api.Database;
using ParkingApp.Common.Models.User;

namespace ParkingApp.Api.Services;

public interface ISQLService
{
    // Vehicle-related methods
    Task<Vehicle?> GetVehicleByIdAsync(string id);
    Task<List<Vehicle>> GetVehiclesByUserIdAsync(string userKey);
    Task<Vehicle> CreateVehicleAsync(Vehicle vehicle);
    Task<bool> UpdateVehicleAsync(string id, VehicleBase vehicle);
    Task<bool> DeleteVehicleAsync(string id);

    // Contact-related methods
    Task<Contact?> GetContactByIdAsync(string id);
    Task<List<Contact>> GetContactsByUserIdAsync(string userKey);
    Task<Contact> CreateContactAsync(Contact newContact);
    Task<bool> UpdateContactAsync(string id, ContactBase updatedContact);
    Task<bool> DeleteContactAsync(string id);
}
public class SQLService : ISQLService
{
    private readonly ParkingDbContext _context;

    public SQLService(ParkingDbContext context)
    {
        _context = context;
    }

    // Addresses begin
    public async Task<Address?> GetAddressByUserIdAsync(string userId)
    {
        return await _context.Addresses.SingleOrDefaultAsync(a => a.UserId == userId);
    }

    public async Task<Address?> GetAddressByIdAsync(string id)
    {
        return await _context.Addresses.SingleOrDefaultAsync(a => a.Id == id);
    }

    public async Task<bool> UpdateAddressAsync(Address updatedAddress)
    {
        var existingAddress = await _context.Addresses
            .FirstOrDefaultAsync(a => a.Id == updatedAddress.Id);

        if (existingAddress == null)
        {
            return false;
        }

        existingAddress.Address1 = updatedAddress.Address1;
        existingAddress.Address2 = updatedAddress.Address2;
        existingAddress.City = updatedAddress.City;
        existingAddress.State = updatedAddress.State;
        existingAddress.PostalCode = updatedAddress.PostalCode;
        existingAddress.Country = updatedAddress.Country;

        await _context.SaveChangesAsync();
        return true;
    }
    // Addresses end

    // Contacts begin
    public async Task<Contact?> GetContactByIdAsync(string id)
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

    public async Task<bool> UpdateContactAsync(string id, ContactBase updatedContact)
    {
        var existingContact = await _context.Contacts.FirstOrDefaultAsync(c => c.Id == id);

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

    public async Task<bool> DeleteContactAsync(string id)
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
    public async Task<Vehicle?> GetVehicleByIdAsync(string id)
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

    public async Task<bool> UpdateVehicleAsync(string id, VehicleBase updatedVehicle)
    {
        var existingVehicle = await _context.Vehicles.FirstOrDefaultAsync(c => c.Id == id);

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

    public async Task<bool> DeleteVehicleAsync(string id)
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
