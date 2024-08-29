using MudBlazor;

namespace ParkingApp.Gui.Styling;

public class AppTheme
{
    public static MudTheme GetTheme()
    {
        return new MudTheme
        {
            PaletteLight = new PaletteLight
            {
                Primary = "#F2EFE9",
                Secondary = "#904E55",
                Background = "#252627",
                Surface = "#585A5B",
                AppbarBackground = "#904E55",
                TextPrimary = "#F2EFE9"
            },
            Typography = new Typography
            {
                Default = new Default
                {
                    FontFamily = new[] { "Roboto", "Helvetica", "Arial", "sans-serif" },
                    FontSize = "1rem",
                },
                H1 = new H1
                {
                    FontSize = "2.5rem",
                    FontWeight = 500,
                },
                Button = new Button
                {
                    FontSize = "0.875rem",
                    FontWeight = 700,
                }
            }
        };
    }
}
