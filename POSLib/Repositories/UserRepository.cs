using Microsoft.Data.Sqlite;
using POSLib.Models;

namespace POSLib.Repositories
{
    public class UserRepository(string connStr) : IUserRepository
    {
        private readonly string _connStr = connStr;

        private static List<Permission> GetPermissions(Role role)
        {
            if (role == Role.Manager)
            {
                return [
                    Permission.ViewProducts,
                    Permission.AddProducts,
                    Permission.EditProducts,
                    Permission.DeleteProducts,
                    Permission.EditCategories,
                    Permission.ViewCategories,
                    Permission.AddCategories,
                    Permission.DeleteCategories,
                    Permission.ViewHelp,
                ];
            }
            else if (role == Role.Cashier)
            {
                return [
                    Permission.ViewProducts,
                    Permission.ViewHelp,
                ];
            }
            return [];
        }

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
                Role role = (Role)Enum.Parse(typeof(Role), reader.GetString(reader.GetOrdinal("role")));

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
