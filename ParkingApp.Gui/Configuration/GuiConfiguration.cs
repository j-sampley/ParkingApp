using Blazored.LocalStorage;
using MudBlazor;
using MudBlazor.Services;
using ParkingApp.Gui.Services;

namespace ParkingApp.Gui.Configuration;

public static class GuiConfiguration
{
    public static void Set(IServiceCollection services)
    {
        services.AddMudServices( config =>
        { 
            config.SnackbarConfiguration.PreventDuplicates = false;
            config.SnackbarConfiguration.NewestOnTop = false;
            config.SnackbarConfiguration.ShowCloseIcon = true;
            config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomRight;
            config.SnackbarConfiguration.VisibleStateDuration = 5000;
            config.SnackbarConfiguration.HideTransitionDuration = 1000;
            config.SnackbarConfiguration.ShowTransitionDuration = 500;
            config.SnackbarConfiguration.SnackbarVariant = Variant.Filled;
        }
        );
        services.AddRazorPages();
        services.AddServerSideBlazor();
        services.AddHttpClient();
        services.AddScoped<AuthService>();
        services.AddScoped<DataService>();
        services.AddBlazoredLocalStorage();
    }
}