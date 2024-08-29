using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using ParkingApp.Common.Models.User;
using ParkingApp.Common.Models.Authentication;

namespace ParkingApp.Api.Services;

public class AccountService
{
    private readonly UserManager<UserDataModel> _userManager;
    private readonly SQLService _sqlService;
    private readonly ILogger<AccountService> _logger;

    public AccountService(UserManager<UserDataModel> userManager, SQLService sqlService, ILogger<AccountService> logger)
    {
        _userManager = userManager;
        _sqlService = sqlService;
        _logger = logger;
    }

    public async Task<IdentityResult> CreateUserAsync(RegisterModel model)
    {
        try
        {
            var userId = Guid.NewGuid().ToString();
            var user = UserDataModelFactory.CreateUser(model);

            return await _userManager.CreateAsync(user, model.Password);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An error occurred while creating the user: {ex.Message}");

            var error = new IdentityError
            {
                Code = "UserCreationFailed",
                Description = "An error occurred while creating the user: " + ex.Message
            };
            return IdentityResult.Failed(error);
        }
    }

    public async Task<UserDataModel?> LookupUserByIdAsync(string userId)
    {
        return await _userManager.Users
            .Include(u => u.Address)
            .Include(u => u.Contacts)
            .Include(u => u.Vehicles)
            .AsSplitQuery()
            .FirstOrDefaultAsync(u => u.Id == userId);
    }

    public async Task<UserDataModel?> LookupUserByEmailAsync(string email)
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

    public async Task<IdentityResult> UpdateUserAddressAsync(UserDataModel user, Address newAddress)
    {
        try
        {
            var updated = await _sqlService.UpdateAddressAsync(newAddress);
            if (!updated)
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Code = "AddressUpdateFailed",
                    Description = "Failed to update the address."
                });
            }

            return IdentityResult.Success;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while updating the address.");
            return IdentityResult.Failed(new IdentityError
            {
                Code = "AddressUpdateException",
                Description = ex.Message
            });
        }
    }

    public async Task<IdentityResult> DeleteUserAsync(UserDataModel user)
    {
        return await _userManager.DeleteAsync(user);
    }
}
