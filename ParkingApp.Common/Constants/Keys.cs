namespace ParkingApp.Common.Constants;

public static class Keys
{
    public static class Account 
    {
        public const string BadRegistration = "BadRegistration";
        public const string InvalidLogin = "InvalidLogin";
        public const string LoginError = "LoginError";
        public const string InvalidRegistration = "InvalidRegistration";
        public const string RegistrationError = "RegistrationError";
        public const string TokenGenerationError = "TokenGenerationError";
        public const string NotFound = "UserNotFound";
        public const string LoggedIn = "UserLoggedIn";
        public const string Created = "UserAccountCreated";
        public const string EmailUpdated = "EmailUpdated";
        public const string EmailUpdateFailed = "EmailUpdateFailed";
        public const string PasswordUpdated = "PasswordUpdated";
        public const string PasswordUpdateFailed = "PasswordUpdateFailed";
        public const string Deleted = "UserAccountDeleted";
        public const string DeletionFailed = "UserAccountDeletionFailed";
        public const string Locked = "UserAccountLocked";
        public const string Updated = "AccountUpdated";
        public const string UpdateFailed = "AccountUpdateFailed";

    }
    public static class Address
    {
        public const string Updated = "AddressUpdated";
        public const string Found = "AddressFound";
        public const string NotFound = "AddressNotFound";
    }
    public static class Contact
    {
        public const string Created = "ContactCreated";
        public const string Updated = "ContactUpdated";
        public const string Deleted = "ContactDeleted";
        public const string Found = "ContactFound";
        public const string NotFound = "ContactNotFound";
        public const string FoundPlural = "ContactsFound";
        public const string NotFoundPlural = "ContactsNotFound";
        public const string IDMismatch = "IDMismatch";
        public const string InvalidContact = "InvalidContact";
    }
    public static class Vehicle
    {
        public const string Created = "VehicleCreated";
        public const string Updated = "VehicleUpdated";
        public const string Deleted = "VehicleDeleted";
        public const string Found = "VehicleFound";
        public const string NotFound = "VehicleNotFound";
        public const string FoundPlural = "VehiclesFound";
        public const string NotFoundPlural = "VehiclesNotFound";
        public const string IDMismatch = "IDMismatch";
        public const string InvalidVehicle = "InvalidVehicle";
    }
}
