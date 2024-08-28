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
}