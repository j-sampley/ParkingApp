using ParkingApp.Api.Configuration;
using static ParkingApp.Common.Constants.AppConstants;

Banner();

var builder = WebApplication.CreateBuilder(args);

CultureConfiguration.Set(builder.Configuration);
LocalizationConfiguration.Set(builder);
AuthConfiguration.Set(builder);
ApiConfiguration.Set(builder);

var app = builder.Build();

AppInitializer.CreateDatabase(app);
AppInitializer.ConfigureApp(app);

app.Run();