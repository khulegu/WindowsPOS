using System.Diagnostics;
using Microsoft.Data.Sqlite;
using POSLib.Models;

namespace POSLib.Repositories
{
    public static class DatabaseInitializer
    {
        private static readonly string[] TableNames = { "Users", "Products", "ProductCategories" };

        private static readonly string CreateUserTableSql =
            @"
            CREATE TABLE IF NOT EXISTS Users (
                id       INTEGER PRIMARY KEY AUTOINCREMENT,
                username TEXT NOT NULL UNIQUE,
                password TEXT NOT NULL,
                role     TEXT NOT NULL
            );";

        private static readonly string CreateProductCategoryTableSql =
            @"
            CREATE TABLE IF NOT EXISTS ProductCategories (
                id      INTEGER PRIMARY KEY AUTOINCREMENT,
                name    TEXT NOT NULL UNIQUE
            );";

        private static readonly string CreateProductTableSql =
            @"
            CREATE TABLE IF NOT EXISTS Products (
                id      INTEGER PRIMARY KEY AUTOINCREMENT,
                name    TEXT NOT NULL,
                price   REAL NOT NULL,
                barcode TEXT NOT NULL UNIQUE,
                categoryId INTEGER NOT NULL,
                imageUrl TEXT,
                FOREIGN KEY (categoryId) REFERENCES ProductCategories (id)
            );";

        private static readonly (string Username, string Password, string Role)[] DefaultUsers =
        {
            ("manager", "1234", "Manager"),
            ("cashier1", "1234", "Cashier"),
            ("cashier2", "1234", "Cashier"),
        };

        private static readonly (
            string Name,
            double Price,
            string Barcode,
            string Category,
            string ImageUrl
        )[] DefaultProducts =
        {
            (
                "Apples (Gala)",
                2.50,
                "100001",
                "Produce",
                "https://media.istockphoto.com/id/184276818/photo/red-apple.jpg?s=612x612&w=0&k=20&c=NvO-bLsG0DJ_7Ii8SSVoKLurzjmV0Qi4eGfn6nW3l5w="
            ),
            (
                "Milk (Whole)",
                4.00,
                "100003",
                "Dairy & Cheese",
                "https://atlas-content-cdn.pixelsquid.com/stock-images/milk-glass-Ka6B5N0-600.jpg"
            ),
            ("Bagels (Plain)", 5.50, "100004", "Bakery", "https://loremflickr.com/320/320/bagels"),
            (
                "Orange Juice",
                3.75,
                "100005",
                "Beverages",
                "https://loremflickr.com/320/320/orange-juice"
            ),
            (
                "Peanut Butter",
                6.00,
                "100006",
                "Pantry",
                "https://loremflickr.com/320/320/peanut-butter"
            ),
            (
                "Chicken Breast (Pack)",
                12.99,
                "100007",
                "Meat & Seafood",
                "https://loremflickr.com/320/320/chicken"
            ),
            (
                "Pasta (Spaghetti)",
                2.25,
                "100008",
                "Pantry",
                "https://loremflickr.com/320/320/pasta"
            ),
            (
                "Yogurt (Strawberry)",
                1.25,
                "100009",
                "Dairy & Cheese",
                "https://loremflickr.com/320/320/yogurt"
            ),
            (
                "Coffee Beans (Arabica)",
                15.00,
                "100010",
                "Beverages",
                "https://loremflickr.com/320/320/coffee"
            ),
            (
                "Crackers (Saltine)",
                3.00,
                "100011",
                "Snacks",
                "https://loremflickr.com/320/320/crackers"
            ),
            ("Bananas", 1.99, "100012", "Produce", "https://loremflickr.com/320/320/bananas"),
            ("Eggs (Dozen)", 5.25, "100013", "Dairy & Cheese", "https://loremflickr.com/320/240"),
            ("Croissants", 4.75, "100014", "Bakery", "https://loremflickr.com/320/240"),
            ("Iced Tea (Peach)", 2.00, "100015", "Beverages", "https://loremflickr.com/320/240"),
            ("Almonds (Bag)", 7.50, "100016", "Snacks", "https://loremflickr.com/320/240"),
            ("Ground Beef", 9.50, "100017", "Meat & Seafood", "https://loremflickr.com/320/240"),
            ("Rice (Basmati)", 4.25, "100018", "Pantry", "https://loremflickr.com/320/240"),
            ("Butter (Stick)", 2.75, "100019", "Dairy & Cheese", "https://loremflickr.com/320/240"),
            ("Green Tea Bags", 6.75, "100020", "Beverages", "https://loremflickr.com/320/240"),
            ("Pretzels", 3.50, "100021", "Snacks", "https://loremflickr.com/320/240"),
        };

        public static void InitializeDatabase(string connectionString)
        {
            using var connection = new SqliteConnection(connectionString);
            try
            {
                connection.Open();
                DropExistingTables(connection);
                CreateTables(connection);
                InitializeDefaultData(connection);
            }
            catch (SqliteException ex)
            {
                Console.WriteLine($"Database initialization error: {ex.Message}");
                throw;
            }
        }

        private static void DropExistingTables(SqliteConnection connection)
        {
            foreach (var tableName in TableNames)
            {
                using var command = connection.CreateCommand();
                command.CommandText = $"DROP TABLE IF EXISTS {tableName};";
                command.ExecuteNonQuery();
            }
        }

        private static void CreateTables(SqliteConnection connection)
        {
            var createTableCommands = new[]
            {
                CreateUserTableSql,
                CreateProductCategoryTableSql,
                CreateProductTableSql,
            };

            foreach (var sql in createTableCommands)
            {
                using var command = connection.CreateCommand();
                command.CommandText = sql;
                command.ExecuteNonQuery();
            }
        }

        private static void InitializeDefaultData(SqliteConnection connection)
        {
            foreach (var user in DefaultUsers)
            {
                CreateUserIfDoesntExist(connection, user.Username, user.Password, user.Role);
            }

            foreach (var product in DefaultProducts)
            {
                CreateProductIfDoesntExist(
                    connection,
                    product.Name,
                    product.Price,
                    product.Barcode,
                    product.Category,
                    product.ImageUrl
                );
            }
        }

        private static void CreateUserIfDoesntExist(
            SqliteConnection connection,
            string username,
            string password,
            string role
        )
        {
            using var command = connection.CreateCommand();
            command.CommandText =
                @"
                INSERT OR IGNORE INTO Users (username, password, role)
                VALUES (@username, @password, @role);";

            command.Parameters.AddWithValue("@username", username);
            command.Parameters.AddWithValue("@password", password);
            command.Parameters.AddWithValue("@role", role);

            command.ExecuteNonQuery();
        }

        private static void CreateProductIfDoesntExist(
            SqliteConnection connection,
            string name,
            double price,
            string barcode,
            string categoryName,
            string imageUrl
        )
        {
            // First ensure category exists
            using (var command = connection.CreateCommand())
            {
                command.CommandText =
                    @"
                    INSERT OR IGNORE INTO ProductCategories (name)
                    VALUES (@categoryName);
                    SELECT last_insert_rowid();";

                command.Parameters.AddWithValue("@categoryName", categoryName);
                command.ExecuteNonQuery();
            }

            // Then insert product
            using (var command = connection.CreateCommand())
            {
                command.CommandText =
                    @"
                    INSERT OR IGNORE INTO Products (name, price, barcode, categoryId, imageUrl)
                    SELECT @name, @price, @barcode, id, @imageUrl
                    FROM ProductCategories
                    WHERE name = @categoryName;";

                command.Parameters.AddWithValue("@name", name);
                command.Parameters.AddWithValue("@price", price);
                command.Parameters.AddWithValue("@barcode", barcode);
                command.Parameters.AddWithValue("@categoryName", categoryName);
                command.Parameters.AddWithValue("@imageUrl", imageUrl);

                command.ExecuteNonQuery();
            }
        }
    }
}
