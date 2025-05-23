using Microsoft.Data.Sqlite;
using POSLib.Models;

namespace POSLib.Repositories
{
    public class UserRepository(string connStr) : IUserRepository
    {
        private readonly string _connStr = connStr;

        /// <summary>
        /// Get the permissions for a user based on their role
        /// </summary>
        /// <param name="role">The role of the user</param>
        /// <returns>The permissions for the user</returns>
        internal static List<Permission> GetPermissions(Role role)
        {
            return role switch
            {
                Role.Manager =>
                [
                    Permission.ViewProducts,
                    Permission.AddProducts,
                    Permission.EditProducts,
                    Permission.DeleteProducts,
                    Permission.EditCategories,
                    Permission.ViewCategories,
                    Permission.AddCategories,
                    Permission.DeleteCategories,
                    Permission.ViewHelp,
                ],
                Role.Cashier =>
                [
                    Permission.ViewProducts,
                    Permission.ViewCategories,
                    Permission.ViewHelp,
                ],
                _ => [],
            };
        }

        /// <summary>
        /// Get a user by username and password
        /// </summary>
        /// <param name="username">The username of the user</param>
        /// <param name="password">The password of the user</param>
        /// <returns>The user if found, otherwise null</returns>
        public User? Login(string username, string password)
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
                Role role = (Role)
                    Enum.Parse(typeof(Role), reader.GetString(reader.GetOrdinal("role")));

                return new User
                {
                    Id = reader.GetInt32(reader.GetOrdinal("id")),
                    Username = reader.GetString(reader.GetOrdinal("username")),
                    Password = reader.GetString(reader.GetOrdinal("password")),
                    Role = role,
                    Permissions = GetPermissions(role),
                };
            }

            return null;
        }
    }
}
