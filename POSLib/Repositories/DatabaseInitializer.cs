using Microsoft.Data.Sqlite;

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
                    command.Parameters.AddWithValue("@password", password);
                    command.Parameters.AddWithValue("@role", role);
                    command.ExecuteNonQuery();
                }
            }
        }

        private static void CreateProductIfDoesntExist(SqliteConnection connection, string name, double price, string barcode, string category)
        {
            string checkProductSql = "SELECT COUNT(*) FROM Products WHERE barcode = @barcode";
            using (var command = connection.CreateCommand())
            {
                command.CommandText = checkProductSql;
                command.Parameters.AddWithValue("@barcode", barcode);
                long productExists = (long?)command.ExecuteScalar() ?? 0;

                if (productExists == 0)
                {
                    string insertProductSql = "INSERT INTO Products (name, price, barcode, category) VALUES (@name, @price, @barcode, @category)";
                    command.CommandText = insertProductSql;
                    command.Parameters.AddWithValue("@name", name);
                    command.Parameters.AddWithValue("@price", price);
                    command.Parameters.AddWithValue("@category", category);
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
                id       INTEGER PRIMARY KEY AUTOINCREMENT,
                username TEXT NOT NULL UNIQUE,
                password TEXT NOT NULL,
                role     TEXT NOT NULL
            );";

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = createUserTableSql;
                    command.ExecuteNonQuery();
                    Console.WriteLine("Users table checked/created.");
                }

                string createProductTableSql = @"
            CREATE TABLE IF NOT EXISTS Products (
                id      INTEGER PRIMARY KEY AUTOINCREMENT,
                name    TEXT NOT NULL,
                price   REAL NOT NULL,
                barcode TEXT NOT NULL UNIQUE,
                category TEXT NOT NULL
            );";

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = createProductTableSql;
                    command.ExecuteNonQuery();
                    Console.WriteLine("Products table checked/created.");
                }

                CreateUserIfDoesntExist(connection, "manager", "1234", "Manager");
                CreateUserIfDoesntExist(connection, "cashier1", "1234", "Cashier");
                CreateUserIfDoesntExist(connection, "cashier2", "1234", "Cashier");

                CreateProductIfDoesntExist(connection, "Apples (Gala)", 2.50, "001122334455", "Produce");
                CreateProductIfDoesntExist(connection, "Milk (Whole)", 4.00, "667788990011", "Dairy & Cheese");
                CreateProductIfDoesntExist(connection, "Bagels (Plain)", 5.50, "111222333444", "Bakery");
                CreateProductIfDoesntExist(connection, "Orange Juice", 3.75, "555666777888", "Beverages");
                CreateProductIfDoesntExist(connection, "Peanut Butter", 6.00, "999000111222", "Pantry");
                CreateProductIfDoesntExist(connection, "Chicken Breast (Pack)", 12.99, "222333444555", "Meat & Seafood");
                CreateProductIfDoesntExist(connection, "Pasta (Spaghetti)", 2.25, "777888999000", "Pantry");
                CreateProductIfDoesntExist(connection, "Yogurt (Strawberry)", 1.25, "333444555666", "Dairy & Cheese");
                CreateProductIfDoesntExist(connection, "Coffee Beans (Arabica)", 15.00, "888999000111", "Beverages");
                CreateProductIfDoesntExist(connection, "Crackers (Saltine)", 3.00, "444555666777", "Snacks");
                CreateProductIfDoesntExist(connection, "Bananas", 1.99, "009988776655", "Produce");
                CreateProductIfDoesntExist(connection, "Eggs (Dozen)", 5.25, "665544332211", "Dairy & Cheese");
                CreateProductIfDoesntExist(connection, "Croissants", 4.75, "119988776655", "Bakery");
                CreateProductIfDoesntExist(connection, "Iced Tea (Peach)", 2.00, "559988776655", "Beverages");
                CreateProductIfDoesntExist(connection, "Almonds (Bag)", 7.50, "991122334455", "Snacks");
                CreateProductIfDoesntExist(connection, "Ground Beef", 9.50, "229988776655", "Meat & Seafood");
                CreateProductIfDoesntExist(connection, "Rice (Basmati)", 4.25, "771122334455", "Pantry");
                CreateProductIfDoesntExist(connection, "Butter (Stick)", 2.75, "339988776655", "Dairy & Cheese");
                CreateProductIfDoesntExist(connection, "Green Tea Bags", 6.75, "881122334455", "Beverages");
                CreateProductIfDoesntExist(connection, "Pretzels", 3.50, "449988776655", "Snacks");

                Console.WriteLine("Database initialization complete.");

            }
            catch (SqliteException ex)
            {
                Console.WriteLine($"Database initialization error: {ex.Message}");
                throw;
            }
        }
    }
}
