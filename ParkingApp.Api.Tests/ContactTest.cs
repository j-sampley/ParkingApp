using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using ParkingApp.Api.Controllers;
using ParkingApp.Api.Services;
using ParkingApp.Common.Models.User;
using ParkingApp.Common.Services;

namespace ParkingApp.Api.Tests
{
    public class ContactControllerTests
    {
        private readonly Mock<ISQLService> _dbServiceMock;
        private readonly Mock<ILocalizationService<ContactController>> _localizationMock;
        private readonly Mock<ILogger<ContactController>> _loggerMock;
        private readonly ContactController _controller;

        public ContactControllerTests()
        {
            _dbServiceMock = new Mock<ISQLService>();
            _localizationMock = new Mock<ILocalizationService<ContactController>>();
            _loggerMock = new Mock<ILogger<ContactController>>();
            _controller = new ContactController(_dbServiceMock.Object, _localizationMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task GetContactById_ReturnsOk_WhenContactExists()
        {
            // Arrange
            var contactId = "123";
            var contact = new Contact { Id = contactId, FirstName = "John", LastName = "Doe", Email = "john.doe@example.com" };
            _dbServiceMock.Setup(service => service.GetContactByIdAsync(contactId))
                          .ReturnsAsync(contact);

            // Act
            var result = await _controller.GetContactById(contactId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedContact = Assert.IsType<Contact>(okResult.Value);
            Assert.Equal(contactId, returnedContact.Id);
        }

        [Fact]
        public async Task GetContactById_ReturnsNotFound_WhenContactDoesNotExist()
        {
            // Arrange
            var contactId = "123";
            _dbServiceMock.Setup(service => service.GetContactByIdAsync(contactId))
                          .ReturnsAsync((Contact)null);

            // Act
            var result = await _controller.GetContactById(contactId);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task GetContactsByUserId_ReturnsOk_WithContacts()
        {
            // Arrange
            var userId = "user123";
            var contacts = new List<Contact>
            {
                new Contact { Id = "1", FirstName = "John", LastName = "Doe", Email = "john.doe@example.com" },
                new Contact { Id = "2", FirstName = "Jane", LastName = "Doe", Email = "jane.doe@example.com" }
            };
            _dbServiceMock.Setup(service => service.GetContactsByUserIdAsync(userId))
                          .ReturnsAsync(contacts);

            // Act
            var result = await _controller.GetContactsByUserId(userId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedContacts = Assert.IsType<List<Contact>>(okResult.Value);
            Assert.Equal(2, returnedContacts.Count);
        }

        [Fact]
        public async Task CreateContact_ReturnsOk_WhenContactIsCreated()
        {
            // Arrange
            var contactId = "123";
            var newContact = new ContactBase { FirstName = "John", LastName = "Doe", Email = "john.doe@example.com" };
            var createdContact = new Contact { Id = contactId, FirstName = "John", LastName = "Doe", Email = "john.doe@example.com" };

            _dbServiceMock.Setup(service => service.CreateContactAsync(It.IsAny<Contact>()))
                          .ReturnsAsync(createdContact);

            // Act
            var result = await _controller.CreateContact(contactId, newContact);

            // Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task UpdateContact_ReturnsNoContent_WhenContactIsUpdated()
        {
            // Arrange
            var contactId = "123";
            var updatedContact = new ContactBase { FirstName = "John", LastName = "Doe", Email = "john.doe@example.com" };
            _dbServiceMock.Setup(service => service.UpdateContactAsync(contactId, updatedContact))
                          .ReturnsAsync(true);

            // Act
            var result = await _controller.UpdateContact(contactId, updatedContact);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteContact_ReturnsNoContent_WhenContactIsDeleted()
        {
            // Arrange
            var contactId = "123";
            _dbServiceMock.Setup(service => service.DeleteContactAsync(contactId))
                          .ReturnsAsync(true);

            // Act
            var result = await _controller.DeleteContact(contactId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteContact_ReturnsNotFound_WhenContactDoesNotExist()
        {
            // Arrange
            var contactId = "123";
            _dbServiceMock.Setup(service => service.DeleteContactAsync(contactId))
                          .ReturnsAsync(false);

            // Act
            var result = await _controller.DeleteContact(contactId);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }
    }
}