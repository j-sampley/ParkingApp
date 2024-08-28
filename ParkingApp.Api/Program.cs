using ParkingApp.Api.Configuration;
using static ParkingApp.Common.Constants.AppConstants;

Banner();

var builder = WebApplication.CreateBuilder(args);

LogConfiguration.Set(builder.Host);
CultureConfiguration.Set(builder.Configuration);
LocalizationConfiguration.Set(builder);
AuthConfiguration.Set(builder);
ApiConfiguration.Set(builder);

var app = builder.Build();
var urls = builder.Configuration["Urls"];

AppInitializer.LogListening(urls);
AppInitializer.CreateDatabase(app);
AppInitializer.ConfigureApp(app);

app.Run();