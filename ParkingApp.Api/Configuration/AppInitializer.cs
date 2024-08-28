using Serilog;

using ParkingApp.Api.Database;

namespace ParkingApp.Api.Configuration;

public static class AppInitializer
{
    public static void CreateDatabase(WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;
        var context = services.GetRequiredService<ParkingDbContext>();
        context.Database.EnsureCreated();
    }

    public static void ConfigureApp(WebApplication app)
    {
        // Add Swagger
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        LocalizationConfiguration.SetSupportedCultures(app);
        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseSerilogRequestLogging();

        app.MapControllers();
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