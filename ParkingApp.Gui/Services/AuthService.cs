using ParkingApp.Common.Models.Authentication;

namespace ParkingApp.Gui.Services;

using Blazored.LocalStorage;

public class AuthService
{
    private readonly HttpClient _httpClient;
    private readonly ILocalStorageService _localStorage;
    private readonly ILogger<AuthService> _logger;

    public AuthService(HttpClient httpClient, ILogger<AuthService> logger, ILocalStorageService localStorage)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri("http://localhost:5000/");
        _logger = logger;
        _localStorage = localStorage;
    }

    public async Task<bool> LoginAsync(LoginModel loginModel)
    {
        var response = await _httpClient.PostAsJsonAsync("api/auth/login", loginModel);
        if (response.IsSuccessStatusCode)
        {
            var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponse>();
            if (loginResponse != null)
            {
                await SaveAuthToken(loginResponse.Token);
                await SaveUserId(loginResponse.Id);

                return true;
            }
        }
        return false;
    }

    public async Task<bool> RegisterAsync(RegisterModel registerModel)
    {
        var response = await _httpClient.PostAsJsonAsync("api/auth/register", registerModel);
        return response.IsSuccessStatusCode;
    }

    public async Task<string?> GetTokenAsync()
    {
        try
        {
            var token = await _localStorage.GetItemAsync<string>("authToken");

            if (string.IsNullOrEmpty(token))
            {
                _logger.LogWarning("JWT token retrieval failed: token is null or empty.");
                return null;
            }

            return token;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving JWT token from local storage.");
            return null;
        }
    }

    public async Task<string?> GetUserId()
    {
        try
        {
            var token = await _localStorage.GetItemAsync<string>("userId");

            if (string.IsNullOrEmpty(token))
            {
                _logger.LogWarning("User id retrieval failed: user id is null or empty.");
                return null;
            }

            return token;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving user id from local storage.");
            return null;
        }
    }

    public async ValueTask SaveAuthToken(string token)
    {
        await _localStorage.SetItemAsync("authToken", token);
    }

    public async ValueTask SaveUserId(string token)
    {
        await _localStorage.SetItemAsync("userId", token);
    }

    public async Task LogoutAsync()
    {
        try
        {
            await _localStorage.RemoveItemAsync("authToken");
            await _localStorage.RemoveItemAsync("userId");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during logout.");
        }
    }
}