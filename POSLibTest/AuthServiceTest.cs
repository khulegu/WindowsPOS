using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq; // Using Moq for mocking the repository
using POSLib.Models;
using POSLib.Repositories;
using POSLib.Services;
using System;
using System.Collections.Generic;

namespace POSLibTest
{
    [TestClass]
    public class AuthServiceTest
    {
        private Mock<IUserRepository> _mockUserRepo = null!;
        private AuthService _authService = null!;

        [TestInitialize]
        public void TestInitialize()
        {
            // Setup mock before each test
            _mockUserRepo = new Mock<IUserRepository>();
            _authService = new AuthService(_mockUserRepo.Object);
        }

        [TestMethod]
        public void Login_ValidCredentials_ReturnsUser()
        {
            // Arrange
            string username = "testuser";
            string password = "password123";
            var expectedUser = new User
            {
                Id = 1,
                Username = username,
                Password = password, // In real scenarios, password shouldn't be stored plain text
                Role = Role.Cashier,
                Permissions = new List<Permission> { Permission.ViewProducts, Permission.ViewHelp }
            };

            // Configure the mock repository to return the expected user
            // when GetUser is called with the specific username and password
            _mockUserRepo.Setup(repo => repo.GetUser(username, password))
                         .Returns(expectedUser);

            // Act
            User? actualUser = _authService.Login(username, password);

            // Assert
            Assert.IsNotNull(actualUser);
            Assert.AreEqual(expectedUser.Id, actualUser.Id);
            Assert.AreEqual(expectedUser.Username, actualUser.Username);
            Assert.AreEqual(expectedUser.Role, actualUser.Role);
            CollectionAssert.AreEqual(expectedUser.Permissions, actualUser.Permissions);

            // Verify that GetUser was called exactly once with the correct parameters
            _mockUserRepo.Verify(repo => repo.GetUser(username, password), Times.Once);
        }

        [TestMethod]
        public void Login_InvalidCredentials_ThrowsUnauthorizedAccessException()
        {
            // Arrange
            string username = "wronguser";
            string password = "wrongpassword";

            // Configure the mock repository to return null for these credentials
            _mockUserRepo.Setup(repo => repo.GetUser(username, password))
                         .Returns((User?)null); // Explicitly return null User

            // Act & Assert
            var ex = Assert.ThrowsException<UnauthorizedAccessException>(() =>
            {
                _authService.Login(username, password);
            });

            // Optional: Assert on the exception message
            Assert.AreEqual("Invalid username or password.", ex.Message);

            // Verify that GetUser was called exactly once
            _mockUserRepo.Verify(repo => repo.GetUser(username, password), Times.Once);
        }

        [TestMethod]
        public void Login_ManagerCredentials_ReturnsManagerUserWithPermissions()
        {
            // Arrange
            string username = "manager";
            string password = "securepassword";
            var expectedManagerUser = new User
            {
                Id = 2,
                Username = username,
                Password = password,
                Role = Role.Manager,
                Permissions = new List<Permission> { // Example manager permissions
                    Permission.ViewProducts, Permission.AddProducts, Permission.EditProducts, Permission.DeleteProducts,
                    Permission.ViewCategories, Permission.AddCategories, Permission.EditCategories, Permission.DeleteCategories,
                    Permission.ViewHelp
                }
            };

            _mockUserRepo.Setup(repo => repo.GetUser(username, password))
                         .Returns(expectedManagerUser);

            // Act
            User? actualUser = _authService.Login(username, password);

            // Assert
            Assert.IsNotNull(actualUser);
            Assert.AreEqual(expectedManagerUser.Id, actualUser.Id);
            Assert.AreEqual(expectedManagerUser.Username, actualUser.Username);
            Assert.AreEqual(Role.Manager, actualUser.Role);
            Assert.IsTrue(actualUser.Permissions.Contains(Permission.DeleteProducts)); // Check a manager-specific permission
            Assert.IsTrue(actualUser.Permissions.Contains(Permission.AddCategories));
            CollectionAssert.AreEqual(expectedManagerUser.Permissions, actualUser.Permissions);

            _mockUserRepo.Verify(repo => repo.GetUser(username, password), Times.Once);
        }
    }
}