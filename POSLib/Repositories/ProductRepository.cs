using Microsoft.Data.Sqlite;
using POSLib.Exceptions;
using POSLib.Models;

namespace POSLib.Repositories
{

    public class ProductRepository : IProductRepository
    {
        private readonly string _connStr;
        public ProductRepository(string connStr) => _connStr = connStr;

        /// <summary>
        /// Product-iig SqliteDataReader-saar maplah.
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private Product MapProduct(SqliteDataReader reader)
        {
            return new Product
            {
                Id = reader.GetInt32(reader.GetOrdinal("id")),
                Name = reader.GetString(reader.GetOrdinal("name")),
                Price = reader.GetDouble(reader.GetOrdinal("price")),
                Barcode = reader.GetString(reader.GetOrdinal("barcode")),
                Category = new ProductCategory
                {
                    Id = reader.GetInt32(reader.GetOrdinal("categoryId")),
                    Name = reader.GetString(reader.GetOrdinal("categoryName"))
                }
            };
        }

        /// <summary>
        /// Buh product-iig avna.
        /// </summary>
        /// <returns></returns>
        public List<Product> GetAll()
        {
            var products = new List<Product>();
            using var connection = new SqliteConnection(_connStr);
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = @"
                SELECT p.id, p.name, p.price, p.barcode, c.id as categoryId, c.name as categoryName
                FROM Products p
                JOIN ProductCategories c ON p.categoryId = c.id
            ";

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                products.Add(MapProduct(reader));
            }
            return products;
        }

        /// <summary>
        /// Category-t baigaa product-uudiig avna.
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public List<Product> GetAllByCategory(int categoryId)
        {
            var products = new List<Product>();
            using var connection = new SqliteConnection(_connStr);
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = @"
                SELECT p.id, p.name, p.price, p.barcode, c.id as categoryId, c.name as categoryName
                FROM Products p
                JOIN ProductCategories c ON p.categoryId = c.id
                WHERE c.id = $categoryId
            ";
            command.Parameters.AddWithValue("$categoryId", categoryId);

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                products.Add(MapProduct(reader));
            }
            return products;
        }

        /// <summary>
        /// Barcode-oor baraa haij avchirah. Oldoogui bol null utga butsaana.
        /// </summary>
        /// <param name="barcode"></param>
        /// <returns></returns>
        public Product? GetByBarcode(string barcode)
        {
            using var connection = new SqliteConnection(_connStr);
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = @"
                SELECT p.id, p.name, p.price, p.barcode, c.id as categoryId, c.name as categoryName
                FROM Products p
                JOIN ProductCategories c ON p.categoryId = c.id
                WHERE p.barcode = $barcode
            ";
            command.Parameters.AddWithValue("$barcode", barcode);

            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return MapProduct(reader);
            }
            return null;
        }

        public void Add(Product product)
        {
            using var connection = new SqliteConnection(_connStr);
            connection.Open();

            using var command = connection.CreateCommand();

            command.CommandText = "INSERT INTO Products (name, price, barcode, categoryId) VALUES ($name, $price, $barcode, $categoryId)";
            command.Parameters.AddWithValue("$name", product.Name);
            command.Parameters.AddWithValue("$price", product.Price);
            command.Parameters.AddWithValue("$barcode", product.Barcode);
            command.Parameters.AddWithValue("$categoryId", product.Category.Id);

            try
            {
                command.ExecuteNonQuery();
            }
            catch (SqliteException ex) when (ex.SqliteErrorCode == 19) // SQLITE_CONSTRAINT
            {
                if (ex.Message.Contains("UNIQUE constraint failed: Products.barcode"))
                {
                    throw new BarcodeAlreadyExistsException($"A product with the barcode '{product.Barcode}' already exists.", ex);
                }
                throw;
            }
        }

        public void Update(Product product)
        {
            using var connection = new SqliteConnection(_connStr);
            connection.Open();

            using var command = connection.CreateCommand();

            command.CommandText = "UPDATE Products SET name = $name, price = $price, barcode = $barcode WHERE id = $id";
            command.Parameters.AddWithValue("$name", product.Name);
            command.Parameters.AddWithValue("$price", product.Price);
            command.Parameters.AddWithValue("$barcode", product.Barcode);
            command.Parameters.AddWithValue("$id", product.Id);

            command.ExecuteNonQuery();
        }

        public void Delete(int id)
        {
            using var connection = new SqliteConnection(_connStr);
            connection.Open();

            using var command = connection.CreateCommand();

            command.CommandText = "DELETE FROM Products WHERE id = $id";
            command.Parameters.AddWithValue("$id", id);

            command.ExecuteNonQuery();
        }

        public void AddCategory(ProductCategory category)
        {
            using var connection = new SqliteConnection(_connStr);
            connection.Open();

            using var command = connection.CreateCommand();

            command.CommandText = "INSERT INTO ProductCategories (name) VALUES ($name)";
            command.Parameters.AddWithValue("$name", category.Name);

            command.ExecuteNonQuery();
        }

        public void UpdateCategory(ProductCategory category)
        {
            using var connection = new SqliteConnection(_connStr);
            connection.Open();

            using var command = connection.CreateCommand();

            command.CommandText = "UPDATE ProductCategories SET name = $name WHERE id = $id";
            command.Parameters.AddWithValue("$name", category.Name);
            command.Parameters.AddWithValue("$id", category.Id);

            command.ExecuteNonQuery();
        }

        public void DeleteCategory(int id)
        {
            using var connection = new SqliteConnection(_connStr);
            connection.Open();

            using var command = connection.CreateCommand();

            command.CommandText = "DELETE FROM ProductCategories WHERE id = $id";
            command.Parameters.AddWithValue("$id", id);

            command.ExecuteNonQuery();
        }

        public List<ProductCategory> GetAllCategories()
        {
            var categories = new List<ProductCategory>();
            using var connection = new SqliteConnection(_connStr);
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = "SELECT id, name FROM ProductCategories";

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                categories.Add(new ProductCategory
                {
                    Id = reader.GetInt32(reader.GetOrdinal("id")),
                    Name = reader.GetString(reader.GetOrdinal("name"))
                });
            }
            return categories;
        }
    }
}