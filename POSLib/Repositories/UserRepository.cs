using System;
using Microsoft.Data.Sqlite;
using POSLib.Models;

namespace POSLib.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly string _connStr;
        public UserRepository(string connStr) => _connStr = connStr;

        public User? GetUser(string username, string password)
        {
            using var connection = new SqliteConnection(_connStr);
            connection.Open();

            using var command = connection.CreateCommand();

            command.CommandText =
                @"SELECT id, username, password, role
                  FROM Users
                  WHERE username = $username AND password = $password";

            command.Parameters.AddWithValue("$username", username);
            command.Parameters.AddWithValue("$password", password);

            using var reader = command.ExecuteReader();

            if (reader.Read())
            {
                return new User
                {
                    Id = reader.GetInt32(reader.GetOrdinal("id")),
                    Username = reader.GetString(reader.GetOrdinal("username")),
                    Password = reader.GetString(reader.GetOrdinal("password")),
                    Role = (Role) Enum.Parse(typeof(Role), reader.GetString(reader.GetOrdinal("role")))
                };
            }

            return null;
        }
    }
}
