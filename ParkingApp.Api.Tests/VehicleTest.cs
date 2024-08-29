using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using ParkingApp.Api.Controllers;
using ParkingApp.Api.Services;
using ParkingApp.Common.Models.User;
using ParkingApp.Common.Services;


namespace ParkingApp.Api.Tests
{
    public class VehicleControllerTests
    {
        private readonly Mock<ISQLService> _dbServiceMock;
        private readonly Mock<ILocalizationService<VehicleController>> _localizationMock;
        private readonly Mock<ILogger<Vehicle>> _loggerMock;
        private readonly VehicleController _controller;

        public VehicleControllerTests()
        {
            _dbServiceMock = new Mock<ISQLService>();
            _localizationMock = new Mock<ILocalizationService<VehicleController>>();
            _loggerMock = new Mock<ILogger<Vehicle>>();
            _controller = new VehicleController(_dbServiceMock.Object, _localizationMock.Object, _loggerMock.Object);
        }


        [Fact]
        public async Task GetVehicleById_ReturnsOk_WhenVehicleExists()
        {
            // Arrange
            var vehicleId = "123";
            var vehicle = new Vehicle { Id = vehicleId, Make = "Toyota", Model = "Camry", Year = 2020 };
            _dbServiceMock.Setup(service => service.GetVehicleByIdAsync(vehicleId))
                          .ReturnsAsync(vehicle);

            // Act
            var result = await _controller.GetVehicleById(vehicleId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedVehicle = Assert.IsType<Vehicle>(okResult.Value);
            Assert.Equal(vehicleId, returnedVehicle.Id);
        }

        [Fact]
        public async Task GetVehicleById_ReturnsNotFound_WhenVehicleDoesNotExist()
        {
            // Arrange
            var vehicleId = "123";
            _dbServiceMock.Setup(service => service.GetVehicleByIdAsync(vehicleId))
                          .ReturnsAsync((Vehicle)null);

            // Act
            var result = await _controller.GetVehicleById(vehicleId);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task GetVehiclesByUserKey_ReturnsOk_WithVehicles()
        {
            // Arrange
            var userKey = "user123";
            var vehicles = new List<Vehicle>
            {
                new() { Id = "1", Make = "Toyota", Model = "Camry", Year = 2020 },
                new() { Id = "2", Make = "Ford", Model = "Mustang", Year = 2019 }
            };
            _dbServiceMock.Setup(service => service.GetVehiclesByUserIdAsync(userKey))
                          .ReturnsAsync(vehicles);

            // Act
            var result = await _controller.GetVehiclesByUserKey(userKey);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedVehicles = Assert.IsType<List<Vehicle>>(okResult.Value);
            Assert.Equal(2, returnedVehicles.Count);
        }

        [Fact]
        public async Task CreateVehicle_ReturnsOk_WhenVehicleIsCreated()
        {
            // Arrange
            var vehicleId = "123";
            var newVehicle = new VehicleBase { Make = "Toyota", Model = "Camry", Year = 2020 };
            var createdVehicle = new Vehicle { Id = vehicleId, Make = "Toyota", Model = "Camry", Year = 2020 };

            _dbServiceMock.Setup(service => service.CreateVehicleAsync(It.IsAny<Vehicle>()))
                          .ReturnsAsync(createdVehicle);

            // Act
            var result = await _controller.CreateVehicle(vehicleId, newVehicle);

            // Assert
            Assert.IsType<OkResult>(result);
        }


        [Fact]
        public async Task UpdateVehicle_ReturnsNoContent_WhenVehicleIsUpdated()
        {
            // Arrange
            var vehicleId = "123";
            var updatedVehicle = new VehicleBase { Make = "Toyota", Model = "Camry", Year = 2020 };
            _dbServiceMock.Setup(service => service.UpdateVehicleAsync(vehicleId, updatedVehicle))
                          .ReturnsAsync(true);

            // Act
            var result = await _controller.UpdateVehicle(vehicleId, updatedVehicle);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteVehicle_ReturnsNoContent_WhenVehicleIsDeleted()
        {
            // Arrange
            var vehicleId = "123";
            _dbServiceMock.Setup(service => service.DeleteVehicleAsync(vehicleId))
                          .ReturnsAsync(true);

            // Act
            var result = await _controller.DeleteVehicle(vehicleId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteVehicle_ReturnsNotFound_WhenVehicleDoesNotExist()
        {
            // Arrange
            var vehicleId = "123";
            _dbServiceMock.Setup(service => service.DeleteVehicleAsync(vehicleId))
                          .ReturnsAsync(false);

            // Act
            var result = await _controller.DeleteVehicle(vehicleId);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }
    }
}
