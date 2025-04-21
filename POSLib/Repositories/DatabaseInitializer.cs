using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Data.Sqlite;
using System;


namespace POSLib.Repositories
{
    public static class DatabaseInitializer
    {

        private static void CreateUserIfDoesntExist(SqliteConnection connection, string username, string password, string role)
        {
            string checkUserSql = "SELECT COUNT(*) FROM Users WHERE username = @username";
            using (var command = connection.CreateCommand())
            {
                command.CommandText = checkUserSql;
                command.Parameters.AddWithValue("@username", username);
                long userExists = (long?)command.ExecuteScalar() ?? 0;

                if (userExists == 0)
                {
                    string insertUserSql = "INSERT INTO Users (username, password, role) VALUES (@username, @password, @role)";
                    command.CommandText = insertUserSql;
                    command.Parameters.AddWithValue("@password", password); // Hash the password in a real application
                    command.Parameters.AddWithValue("@role", role);
                    command.ExecuteNonQuery();
                }
            }
        }

        public static void InitializeDatabase(string connectionString)
        {
            using var connection = new SqliteConnection(connectionString);
            try
            {
                connection.Open();
                Console.WriteLine($"Database file checked/created at: {connection.DataSource}");

                string createUserTableSql = @"
            CREATE TABLE IF NOT EXISTS Users (
                id       INTEGER PRIMARY KEY AUTOINCREMENT, -- Standard auto-incrementing ID
                username TEXT NOT NULL UNIQUE,             -- Usernames must exist and be unique
                password TEXT NOT NULL,                    -- Passwords must exist (HASH THEM!)
                role     TEXT NOT NULL                     -- Roles must exist
            );";

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = createUserTableSql;
                    command.ExecuteNonQuery(); // Executes commands that don't return data (like CREATE, INSERT, UPDATE, DELETE)
                    Console.WriteLine("Users table checked/created.");
                }

                string createProductTableSql = @"
            CREATE TABLE IF NOT EXISTS Products (
                id      INTEGER PRIMARY KEY AUTOINCREMENT,
                name    TEXT NOT NULL,
                price   REAL NOT NULL, -- REAL is suitable for C# double. Use NUMERIC for C# decimal.
                barcode TEXT NOT NULL UNIQUE -- Barcodes must exist and be unique
            );";

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = createProductTableSql;
                    command.ExecuteNonQuery();
                    Console.WriteLine("Products table checked/created.");
                }

                // --- Seeding Initial Data ---
                CreateUserIfDoesntExist(connection, "manager", "1234", "Manager");
                CreateUserIfDoesntExist(connection, "cashier1", "1234", "Cashier");
                CreateUserIfDoesntExist(connection, "cashier2", "1234", "Cashier");

                Console.WriteLine("Database initialization complete.");

            }
            catch (SqliteException ex)
            {
                // Log the error or handle it appropriately
                Console.WriteLine($"Database initialization error: {ex.Message}");
                // Consider re-throwing or handling critical failures
                throw;
            }
            // Connection is automatically closed here by the 'using' statement
        }
    }
}
