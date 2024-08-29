using ParkingApp.Gui.Configuration;
using ParkingApp.Common.Configuration;
using static ParkingApp.Common.Constants.AppConstants;


GuiBanner();

var builder = WebApplication.CreateBuilder(args);

LogConfiguration.Set(builder.Host);
CultureConfiguration.Set(builder.Configuration);
LocalizationConfiguration.Set(builder.Services);
GuiConfiguration.Set(builder.Services);

var app = builder.Build();
var urls = builder.Configuration["Urls"];

LogConfiguration.LogListening(urls);
AppInitializer.ConfigureApp(app);

app.Run();