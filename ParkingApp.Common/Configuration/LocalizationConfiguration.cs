using System.Globalization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;

using ParkingApp.Common.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ParkingApp.Common.Configuration;

public static class LocalizationConfiguration
{
    public static void Set(IServiceCollection services)
    {
        services.AddLocalization(options => options.ResourcesPath = "Resources");
        services.AddScoped(typeof(ILocalizationService<>), typeof(LocalizationService<>));
    }

    public static void SetSupportedCultures(IApplicationBuilder app)
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