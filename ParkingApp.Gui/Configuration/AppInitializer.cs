using ParkingApp.Common.Configuration;

namespace ParkingApp.Gui.Configuration;

public static class AppInitializer
{
    public static void ConfigureApp(WebApplication app)
    {
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }

        LocalizationConfiguration.SetSupportedCultures(app);

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();
        app.MapControllers();
        app.MapBlazorHub();
        app.MapFallbackToPage("/_Host");
    }
}