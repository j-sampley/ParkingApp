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

        app.MapControllers();
    }
}