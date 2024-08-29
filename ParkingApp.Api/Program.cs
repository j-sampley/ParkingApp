using ParkingApp.Api.Configuration;
using ParkingApp.Common.Configuration;
using static ParkingApp.Common.Constants.AppConstants;

ApiBanner();

var builder = WebApplication.CreateBuilder(args);

LogConfiguration.Set(builder.Host);
CultureConfiguration.Set(builder.Configuration);
LocalizationConfiguration.Set(builder.Services);
AuthConfiguration.Set(builder);
ApiConfiguration.Set(builder);

var app = builder.Build();
var urls = builder.Configuration["Urls"];

LogConfiguration.LogListening(urls);
AppInitializer.CreateDatabase(app);
AppInitializer.ConfigureApp(app);

app.Run();