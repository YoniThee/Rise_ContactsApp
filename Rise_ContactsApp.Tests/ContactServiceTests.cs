using Microsoft.EntityFrameworkCore;
using Moq;
using Rise_ContactsApp.DBAccess;
using Rise_ContactsApp.Models;
using Rise_ContactsApp.Services;
using Xunit;
using Assert = Xunit.Assert;

namespace Rise_ContactsApp.Tests
{
    [TestClass]
    public class ContactServiceTests
    {

        private readonly ContactService _service;
        private readonly ContactsDbContext _context;

        public ContactServiceTests()
        {
            var options = new DbContextOptionsBuilder<ContactsDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new ContactsDbContext(options);

            _context.Contacts.AddRange(new List<Contact>
        {
            new Contact { ContactId = 1, FirstName = "Yoni", LastName = "Levi", PhoneNumber = "1234567890", Address = "Tel Aviv" },
            new Contact { ContactId = 2, FirstName = "Ori", LastName = "Cohen", PhoneNumber = "0987654321", Address = "Lod" }
        });

            _context.SaveChanges();

            _service = new ContactService(_context);
        }

        [TestMethod]
        public async Task GetContactsAsync_ShouldReturnContacts()
        {
            // Act
            var result = await _service.GetContactsAsync(page: 1, pageSize: 2);

            // Assert
            Assert.NotEmpty(result);
            Assert.Equal(2, result.Count);
        }
        [TestMethod]
        public async Task GetContactByIdAsync_ShouldReturnContact_WhenContactExists()
        {
            // Act
            var result = await _service.GetContactByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.ContactId);
        }

        [TestMethod]
        public async Task GetContactByIdAsync_ShouldReturnNull_WhenContactDoesNotExist()
        {
            // Act
            var result = await _service.GetContactByIdAsync(99);

            // Assert
            Assert.Null(result);
        }
        [TestMethod]
        public async Task AddContactAsync_ShouldAddContact_WhenValidInput()
        {
            // Arrange
            var newContact = new Contact { FirstName = "Test", LastName = "User", PhoneNumber = "1231231234", Address = "Jerusalem" };

            // Act
            var result = await _service.AddContactAsync(newContact);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Test", result.FirstName);
        }

        [TestMethod]
        public async Task AddContactAsync_ShouldThrowArgumentException_WhenPhoneNumberIsInvalid()
        {
            // Arrange
            var newContact = new Contact { FirstName = "Test", LastName = "User", PhoneNumber = "123ABC1234", Address = "789 Test St" };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _service.AddContactAsync(newContact));
        }

        [TestMethod]
        public async Task UpdateContactAsync_ShouldUpdateContact_WhenContactExists()
        {
            // Arrange
            var updatedContact = new Contact { FirstName = "Updated", LastName = "User", PhoneNumber = "1231231234", Address = "Updated Address" };

            // Act
            var result = await _service.UpdateContactAsync(1, updatedContact);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Updated", result.FirstName);
            Assert.Equal("Updated Address", result.Address);
        }
    }
}
