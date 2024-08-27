using ParkingApp.Common.Models.User;

namespace ParkingApp.Common.Models.Authentication;

public record LoginResponse(string Token, UserDataModel User);