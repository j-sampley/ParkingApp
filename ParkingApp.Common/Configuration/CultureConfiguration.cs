using System.Globalization;
using Microsoft.Extensions.Configuration;

namespace ParkingApp.Common.Configuration;

public static class CultureConfiguration
{
    public static void Set(IConfiguration configuration)
    {
        var cultureName = configuration["Culture"];
        var cultureInfo = CultureInfo.GetCultureInfo(cultureName);

        CultureInfo.CurrentCulture = cultureInfo;
        CultureInfo.CurrentUICulture = cultureInfo;
    }
}