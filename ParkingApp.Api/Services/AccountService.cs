using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using ParkingApp.Common.Models.User;
using ParkingApp.Common.Models.Authentication;

namespace ParkingApp.Api.Services;

public class AccountService
{
    private readonly UserManager<UserDataModel> _userManager;

    public AccountService(UserManager<UserDataModel> userManager)
    {
        _userManager = userManager;
    }

    public async Task<IdentityResult> CreateUserAsync(RegisterModel model)
    {
        UserDataModel user = new()
        {
            UserName = model.Email,
            Email = model.Email,
            Address = model.Address,
            Contacts = model.Contacts,
            Vehicles = model.Vehicles
        };

        return await _userManager.CreateAsync(user, model.Password);
    }

    public async Task<UserDataModel?> LookupUserAsync(string email)
    {
        return await _userManager.Users
            .Include(u => u.Address)
            .Include(u => u.Contacts)
            .Include(u => u.Vehicles)
            .AsSplitQuery()
            .FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<IdentityResult> UpdateUserEmailAsync(UserDataModel user, string newEmail)
    {
        var token = await _userManager.GenerateChangeEmailTokenAsync(user, newEmail);
        return await _userManager.ChangeEmailAsync(user, newEmail, token);
    }

    public async Task<IdentityResult> UpdateUserPasswordAsync(UserDataModel user, string currentPassword, string newPassword)
    {
        return await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
    }

    public async Task<IdentityResult> DeleteUserAsync(UserDataModel user)
    {
        return await _userManager.DeleteAsync(user);
    }
}
