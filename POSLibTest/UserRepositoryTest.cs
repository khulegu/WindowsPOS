using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using POSLib.Models;
using POSLib.Repositories;

namespace POSLibTest
{
    [TestClass]
    public class UserRepositoryTest
    {
        private readonly string _testConnectionString = "Data Source=pos-test.db";
        public required UserRepository _repository;

        [TestInitialize]
        public void Setup()
        {
            _repository = new UserRepository(_testConnectionString);
            var connection = new SqliteConnection(_testConnectionString);
            connection.Open();

            // Create table
            var createTableCommand = connection.CreateCommand();
            createTableCommand.CommandText =
                "CREATE TABLE IF NOT EXISTS Users (Id INTEGER PRIMARY KEY AUTOINCREMENT, Username TEXT, Password TEXT, Role TEXT)";
            createTableCommand.ExecuteNonQuery();

            // Clear existing data
            var clearCommand = connection.CreateCommand();
            clearCommand.CommandText = "DELETE FROM Users";
            clearCommand.ExecuteNonQuery();

            // Insert test data
            var command = connection.CreateCommand();
            command.CommandText =
                "INSERT INTO Users (Username, Password, Role) VALUES ('testuser', 'testpass', 'Manager')";
            command.ExecuteNonQuery();
            command.CommandText =
                "INSERT INTO Users (Username, Password, Role) VALUES ('cashier', '1234', 'Cashier')";
            command.ExecuteNonQuery();

            connection.Close();
        }

        [TestMethod]
        public void GetPermissions_WithUnknownRole_ReturnsEmptyList()
        {
            var result = UserRepository.GetPermissions((Role)999);
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void Login_ValidCredentials_ReturnsUser()
        {
            // Arrange
            string username = "testuser";
            string password = "testpass";

            // Act
            var result = _repository.Login(username, password);

            // Assert
            Assert.IsNotNull(result, "Expected user to be returned but got null");
            Assert.AreEqual(username, result.Username, "Username mismatch");
            Assert.AreEqual(password, result.Password, "Password mismatch");
            Assert.AreEqual(Role.Manager, result.Role, "Role mismatch");

            // Check permissions
            var expectedPermissions = new List<Permission>
            {
                Permission.ViewProducts,
                Permission.AddProducts,
                Permission.EditProducts,
                Permission.DeleteProducts,
                Permission.EditCategories,
                Permission.ViewCategories,
                Permission.AddCategories,
                Permission.DeleteCategories,
                Permission.ViewHelp,
            };

            foreach (var permission in expectedPermissions)
            {
                Assert.IsTrue(
                    result.Permissions.Contains(permission),
                    $"Expected permission {permission} to be present but it was not"
                );
            }
        }

        [TestMethod]
        public void Login_InvalidCredentials_ReturnsNull()
        {
            // Arrange
            string username = "testuser";
            string wrongPassword = "wrongpass";

            // Act
            var result = _repository.Login(username, wrongPassword);

            // Assert
            Assert.IsNull(
                result,
                "Expected null to be returned for invalid credentials but got a user"
            );
        }

        [TestMethod]
        public void Login_CashierRole_ReturnsCorrectPermissions()
        {
            // Arrange
            string username = "cashier";
            string password = "1234";

            // Act
            var result = _repository.Login(username, password);

            // Assert
            Assert.IsNotNull(result, "Expected user to be returned but got null");
            Assert.AreEqual(Role.Cashier, result.Role, "Role mismatch");

            // Check required permissions are present
            var requiredPermissions = new List<Permission>
            {
                Permission.ViewProducts,
                Permission.ViewCategories,
                Permission.ViewHelp,
            };

            foreach (var permission in requiredPermissions)
            {
                Assert.IsTrue(
                    result.Permissions.Contains(permission),
                    $"Expected permission {permission} to be present but it was not"
                );
            }

            // Check restricted permissions are not present
            var restrictedPermissions = new List<Permission>
            {
                Permission.AddProducts,
                Permission.EditProducts,
                Permission.DeleteProducts,
            };

            foreach (var permission in restrictedPermissions)
            {
                Assert.IsFalse(
                    result.Permissions.Contains(permission),
                    $"Expected permission {permission} to not be present but it was"
                );
            }
        }

        [TestMethod]
        public void Login_ManagerRole_ReturnsCorrectPermissions()
        {
            // Arrange
            string username = "testuser";
            string password = "testpass";

            // Act
            var result = _repository.Login(username, password);

            // Assert
            Assert.IsNotNull(result, "Expected user to be returned but got null");
            Assert.AreEqual(Role.Manager, result.Role, "Role mismatch");

            // Check all manager permissions are present
            var requiredPermissions = new List<Permission>
            {
                Permission.ViewProducts,
                Permission.AddProducts,
                Permission.EditProducts,
                Permission.DeleteProducts,
                Permission.EditCategories,
                Permission.ViewCategories,
                Permission.AddCategories,
                Permission.DeleteCategories,
                Permission.ViewHelp,
            };

            foreach (var permission in requiredPermissions)
            {
                Assert.IsTrue(
                    result.Permissions.Contains(permission),
                    $"Expected permission {permission} to be present but it was not"
                );
            }
        }
    }
}
