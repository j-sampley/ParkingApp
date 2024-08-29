using Serilog;
using Serilog.Events;

namespace ParkingApp.Api.Configuration;

public static class LogConfiguration
{
    public static void Set(ConfigureHostBuilder host)
    {
        Log.Logger = new LoggerConfiguration()
         .MinimumLevel.Debug()
         .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
         .Enrich.FromLogContext()
         .WriteTo.Console()
         .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
         .CreateLogger();
        host.UseSerilog();
    }

    public static void LogListening(string? urls)
    {
        if (!string.IsNullOrEmpty(urls))
        {
            var urlArray = urls.Split(';');
            string? httpAddress = null;
            string? httpsAddress = null;

            foreach (var url in urlArray)
            {
                if (url.StartsWith("http://"))
                {
                    httpAddress = url;
                }
                else if (url.StartsWith("https://"))
                {
                    httpsAddress = url;
                }
            }

            Log.Information("Listening on: {HTTP}, {HTTPS}", httpAddress, httpsAddress);
        }
    }
}