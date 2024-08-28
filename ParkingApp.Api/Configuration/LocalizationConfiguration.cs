using System.Globalization;
using Microsoft.AspNetCore.Localization;

using ParkingApp.Api.Services;

namespace ParkingApp.Api.Configuration;

public static class LocalizationConfiguration
{
    public static void Set(WebApplicationBuilder builder)
    {
        builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
        builder.Services.AddScoped(typeof(ILocalizationService<>), typeof(LocalizationService<>));
    }

    public static void SetSupportedCultures(WebApplication app)
    {
        var supportedCultures = new[] { "en-US", "fr-FR" };

        app.UseRequestLocalization(new RequestLocalizationOptions
        {
            DefaultRequestCulture = new RequestCulture("fr-FR"),
            SupportedCultures = supportedCultures.Select(c => new CultureInfo(c)).ToList(),
            SupportedUICultures = supportedCultures.Select(c => new CultureInfo(c)).ToList(),
            RequestCultureProviders = new List<IRequestCultureProvider>
            {
                new QueryStringRequestCultureProvider(),
                new CookieRequestCultureProvider(),
                new AcceptLanguageHeaderRequestCultureProvider()
            }
        });
    }
}