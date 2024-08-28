using Microsoft.Extensions.Localization;

using ParkingApp.Api.Configuration;

namespace ParkingApp.Api.Services;

public interface ILocalizationService<T>
{
    string GetLocalizedString(string key);
    string GetLocalizedString(string key, params object[] arguments);
}

public class LocalizationService<T> : ILocalizationService<T>
{
    private readonly IConfiguration _configuration;
    private readonly IStringLocalizer<T> _localizer;

    public LocalizationService(IConfiguration configuration, IStringLocalizer<T> localizer)
    {
        _configuration = configuration;
        _localizer = localizer;
        // ideally the RequestLocalizationMiddleware would set the culture but this is my workaround for time
        CultureConfiguration.Set(_configuration); 
    }

    public string GetLocalizedString(string key)
    {
        return _localizer[key];
    }

    public string GetLocalizedString(string key, params object[] arguments)
    {
        return _localizer[key, arguments];
    }
}