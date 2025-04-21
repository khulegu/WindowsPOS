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
        public static void InitializeDatabase(string connectionString)
        {
            // The 'using' statement ensures the connection is closed and disposed of,
            // even if errors occur.
            using var connection = new SqliteConnection(connectionString);
            try
            {
                // Opening the connection creates the database file if it doesn't exist.
                connection.Open();
                Console.WriteLine($"Database file checked/created at: {connection.DataSource}");

                // --- Create Users Table ---
                // Use "CREATE TABLE IF NOT EXISTS" to avoid errors if the table already exists.
                // This makes the initialization safe to run multiple times.
                string createUserTableSql = @"
            CREATE TABLE IF NOT EXISTS Users (
                id       INTEGER PRIMARY KEY AUTOINCREMENT, -- Standard auto-incrementing ID
                username TEXT NOT NULL UNIQUE,             -- Usernames must exist and be unique
                password TEXT NOT NULL,                    -- Passwords must exist (HASH THEM!)
                role     TEXT NOT NULL                     -- Roles must exist
            );";

                // Use 'using' for the command as well.
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = createUserTableSql;
                    command.ExecuteNonQuery(); // Executes commands that don't return data (like CREATE, INSERT, UPDATE, DELETE)
                    Console.WriteLine("Users table checked/created.");
                }

                // --- Create Products Table ---
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

                // --- (Optional) Add Indexes ---
                // Indexes speed up lookups on specific columns (like barcode).
                // UNIQUE constraints often create an index automatically, but you can add others.
                /*
                string createBarcodeIndexSql = @"
                CREATE INDEX IF NOT EXISTS idx_product_barcode ON Products (barcode);";
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = createBarcodeIndexSql;
                    command.ExecuteNonQuery();
                    Console.WriteLine("Product barcode index checked/created.");
                }
                */

                // --- (Optional) Seed Initial Data ---
                // You might want to add default users or products here if needed,
                // checking if they exist first. Example (Admin User):
                /*
                string checkAdminSql = "SELECT COUNT(*) FROM Users WHERE username = 'admin'";
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = checkAdminSql;
                    // Ensure ExecuteScalar returns a non-null value before converting
                    long adminExists = (long?)command.ExecuteScalar() ?? 0;

                    if (adminExists == 0)
                    {
                       string insertAdminSql = @"
                       INSERT INTO Users (username, password, role)
                       VALUES ('admin', 'hashed_admin_password', 'Administrator');"; // ALWAYS HASH PASSWORDS!
                       command.CommandText = insertAdminSql;
                       command.ExecuteNonQuery();
                       Console.WriteLine("Default admin user created.");
                    }
                }
                */

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
