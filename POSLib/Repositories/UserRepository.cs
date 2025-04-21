using System;
using Microsoft.Data.Sqlite; // Use the Microsoft.Data.Sqlite namespace
using POSLib.Models;         // Assuming User model is here

namespace POSLib.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly string _connStr;
        public UserRepository(string connStr) => _connStr = connStr;

        public User GetUser(string username, string password)
        {
            // Use SqliteConnection from Microsoft.Data.Sqlite
            using var connection = new SqliteConnection(_connStr);
            connection.Open();

            // Create and dispose the command properly
            using var command = connection.CreateCommand();
            // Use $ prefix for parameters (common practice with Microsoft.Data.Sqlite)
            // Explicitly list columns instead of SELECT * for clarity and robustness
            command.CommandText =
                @"SELECT id, username, password, role
                  FROM Users
                  WHERE username = $username AND password = $password";

            // Add parameters using AddWithValue (ensure parameter names match the SQL)
            command.Parameters.AddWithValue("$username", username);
            command.Parameters.AddWithValue("$password", password); // Note: Storing plain text passwords is insecure! Consider hashing.

            // ExecuteReader returns a SqliteDataReader
            using var reader = command.ExecuteReader();

            if (reader.Read()) // Check if a user was found
            {
                return new User
                {
                    // Use GetOrdinal and typed getters for robustness and performance
                    Id = reader.GetInt32(reader.GetOrdinal("id")),
                    Username = reader.GetString(reader.GetOrdinal("username")),
                    Password = reader.GetString(reader.GetOrdinal("password")), // Still retrieving the password - consider if needed client-side
                    Role = reader.GetString(reader.GetOrdinal("role"))
                };
            }

            // No user found with the given credentials
            return null;
        }
    }
}
