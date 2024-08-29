## ParkingApp

**ParkingApp** is a web application developed in .NET 7.0, designed to manage parking accounts and related data, including user information, vehicles, and contacts. The application consists of a RESTful API for backend management and a responsive GUI built with MudBlazor. Both the API and the GUI enable full CRUD operations, allowing users to manage their data.

## Table of Contents

- [Features](#features)
- [Technologies](#technologies)
- [Installation](#installation)
- [Usage](#usage)
- [API Endpoints](#api-endpoints)
- [Testing](#testing)

## Features

- **User Management**: Register, update, and delete users.
- **Address Management**: Update and maintain user addresses.
- **Vehicle Management**: Add, edit, and delete vehicles associated with users.
- **Contact Management**: Manage contacts associated with users.

## Technologies

- **Backend**: ASP.NET Core 7.0, Entity Framework Core
- **Frontend**: Blazor with MudBlazor components
- **Database**: SQLite
- **Testing**: xUnit, Moq

## Installation

### Prerequisites

- .NET 7.0 SDK
- An IDE such as Visual Studio or Visual Studio Code
- SQLite (included with the project)

### Steps

1. **Clone the repository:**
  
  ```bash
  git clone https://github.com/j-sampley/ParkingApp.git
  cd ParkingApp
  ```
  
2. **Build the solution:**
  
  ```bash
  dotnet build
  ```
  
3. **Run the API and GUI**
  
  ```bash
  dotnet run --project ParkingApp.Api
  dotnet run --project ParkingApp.Gui
  ```

### API Endpoints

#### **User Management**
- **POST /api/auth/register**: Register a new user.
- **POST /api/auth/login**: Authenticate a user and return a JWT token.
- **GET /api/auth/user/{userId}**: Retrieve details of a specific user by their ID.
- **PUT /api/auth/user/{userId}/email**: Update a user's email address.
- **PUT /api/auth/user/{userId}/password**: Update a user's password.
- **PUT /api/auth/user/{userId}/address**: Update a user's address.
- **DELETE /api/auth/user/{userId}**: Delete a user's account.

#### **Vehicle Management**
- **POST /api/vehicle/{id}**: Add a new vehicle for a user.
- **GET /api/vehicle/{id}**: Retrieve details of a specific vehicle by its ID.
- **GET /api/vehicle/{userKey}**: Retrieve a list of vehicles associated with a specific user key.
- **PUT /api/vehicle/{id}**: Update details of a specific vehicle.
- **DELETE /api/vehicle/{id}**: Delete a specific vehicle.

#### **Contact Management**
- **POST /api/contact/{id}**: Add a new contact for a user.
- **GET /api/contact/{id}**: Retrieve details of a specific contact by its ID.
- **GET /api/contact/{userKey}**: Retrieve a list of contacts associated with a specific user key.
- **PUT /api/contact/{id}**: Update details of a specific contact.
- **DELETE /api/contact/{id}**: Delete a specific contact.

#### **Address Management**
- **GET /api/address/{id}**: Retrieve details of a specific address by its ID.
- **GET /api/address/{userId}**: Retrieve an address associated with a specific user ID.
- **PUT /api/address/address**: Update the details of an address.

#### **U.S. State Retrieval**
- **GET /api/states**: Retrieve a list of U.S. states from the `states.json` file.

### Notes
- **Authentication**: Most user-related operations require the user to be authenticated, which is managed via JSON Web Tokens.
- **Localization**: Basic implementations of localization as proof-of-concept.

## Testing

### Running Unit Tests

The project uses xUnit for unit testing. To run the tests, from the project root execute:

```bash
dotnet test
```

This will run all tests and provide feedback on the console.

## Closing Remarks

Thank you for taking the time to review my submission. I highly appreciate the opportunity to demonstrate my abilities & approach to software dev. I welcome any and all feedback or questions you may have!

P.S., localization is incomplete, as translations take time, but I hope there is enough of it (particularly in ParkingApp.Api) to get the idea across. 
