using ParkingApp.Common.Models.Authentication;
using ParkingApp.Common.Models.User;
using System.Net.Http.Headers;

namespace ParkingApp.Gui.Services;

public class DataService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<DataService> _logger;
    private readonly AuthService _authService;

    public DataService(HttpClient httpClient, ILogger<DataService> logger, AuthService authService)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri("http://localhost:5000/");
        _logger = logger;
        _authService = authService;
    }

    public async Task<UserDataModel?> GetUserDataAsync()
    {
        var userId = await GetUserIdAsync();
        if (userId == null) return null;

        UserDataModel? userData = null;

        bool success = await HandleHttpRequestAsync(
            async () =>
            {
                var response = await _httpClient.GetAsync($"api/auth/user/{userId}");
                if (response.IsSuccessStatusCode)
                {
                    userData = await response.Content.ReadFromJsonAsync<UserDataModel>();
                }
                return response;
            },
            "User data retrieved successfully.",
            "Failed to retrieve user data."
        );

        return success ? userData : null;
    }

    public async Task<bool> UpdateUserEmailAsync(string id, string email)
    {
        return await HandleHttpRequestAsync(
           () => _httpClient.PutAsJsonAsync($"api/auth/user/{id}/email", email),
           "User email updated successfully.",
           "Failed to update user email."
        );
    }

    public async Task<bool> UpdateUserPassword(string id, UpdatePasswordModel passwordModel)
    {
        return await HandleHttpRequestAsync(
            () => _httpClient.PutAsJsonAsync($"api/auth/user/{id}/password", passwordModel),
            "User password updated successfully.",
            "Failed to update user password."
        );
    }

    public async Task<bool> UpdateUserAddressAsync(string id, Address newAddress)
    {
        return await HandleHttpRequestAsync(
            () => _httpClient.PutAsJsonAsync($"api/auth/user/{id}/address", newAddress),
            "User address updated successfully.",
            "Failed to update user address."
        );
    }

    public async Task<bool> UpdateContactAsync(string id, ContactBase updatedContact)
    {
        return await HandleHttpRequestAsync(
            () => _httpClient.PutAsJsonAsync($"api/contact/{id}", updatedContact),
            "Contact updated successfully.",
            "Failed to update contact."
        );
    }

    public async Task<bool> UpdateVehicleAsync(string id, VehicleBase updatedVehicle)
    {
        return await HandleHttpRequestAsync(
            () => _httpClient.PutAsJsonAsync($"api/vehicle/{id}", updatedVehicle),
            "Vehicle updated successfully.",
            "Failed to update vehicle."
        );
    }

    public async Task<bool> AddVehicleAsync(VehicleBase newVehicle)
    {
        var userId = await GetUserIdAsync();
        if (userId == null) return false;

        return await HandleHttpRequestAsync(
            () => _httpClient.PostAsJsonAsync($"api/vehicle/{userId}", newVehicle),
            "Vehicle added successfully.",
            "Failed to add vehicle."
        );
    }

    public async Task<bool> AddContactAsync(ContactBase newContact)
    {
        var userId = await GetUserIdAsync();
        if (userId == null) return false;

        return await HandleHttpRequestAsync(
            () => _httpClient.PostAsJsonAsync($"api/contact/{userId}", newContact),
            "Contact added successfully.",
            "Failed to add contact."
        );
    }

    public async Task<bool> DeleteUserAccount(string userId)
    {
        return await HandleHttpRequestAsync(
            () => _httpClient.DeleteAsync($"api/auth/user/{userId}"),
            "Account deleted successfully.",
            "Failed to delete account."
        );
    }

    public async Task<bool> DeleteVehicleAsync(string vehicleId)
    {
        return await HandleHttpRequestAsync(
            () => _httpClient.DeleteAsync($"api/vehicle/{vehicleId}"),
            "Vehicle deleted successfully.",
            "Failed to delete vehicle."
        );
    }

    public async Task<bool> DeleteContactAsync(string contactId)
    {
        return await HandleHttpRequestAsync(
            () => _httpClient.DeleteAsync($"api/contact/{contactId}"),
            "Contact deleted successfully.",
            "Failed to delete contact."
        );
    }

    public async Task SetToken()
    {
        var token = await _authService.GetTokenAsync();
        if (!string.IsNullOrEmpty(token))
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
    }
    private async Task<string?> GetUserIdAsync()
    {
        var userId = await _authService.GetUserId();

        if (string.IsNullOrEmpty(userId))
        {
            _logger.LogWarning("User ID is null or empty.");
            return null;
        }

        return userId;
    }

    private async Task<bool> HandleHttpRequestAsync(Func<Task<HttpResponseMessage>> httpRequestFunc, string successMessage, string failureMessage)
    {
        try
        {
            await SetToken();
            var response = await httpRequestFunc();

            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation(successMessage);
                return true;
            }
            else
            {
                _logger.LogWarning($"{failureMessage} StatusCode: {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, failureMessage);
        }

        return false;
    }
}