using System.Diagnostics;
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
        /// Map a SqliteDataReader to a Product object
        /// </summary>
        /// <param name="reader">The SqliteDataReader to map</param>
        /// <returns>The mapped Product object</returns>
        private Product MapProduct(SqliteDataReader reader)
        {
            Debug.WriteLine(reader.IsDBNull(reader.GetOrdinal("imageUrl")) ? "NULL" : reader.GetString(reader.GetOrdinal("imageUrl")));
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
                },
                ImageUrl = reader.IsDBNull(reader.GetOrdinal("imageUrl")) ? null : reader.GetString(reader.GetOrdinal("imageUrl"))
            };
        }

        /// <summary>
        /// Get all products
        /// </summary>
        /// <returns>A list of all products</returns>
        public List<Product> GetAll()
        {
            var products = new List<Product>();
            using var connection = new SqliteConnection(_connStr);
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = @"
                SELECT p.id, p.name, p.price, p.barcode, c.id as categoryId, c.name as categoryName, p.imageUrl
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
        /// Get all products by category
        /// </summary>
        /// <param name="categoryId">The id of the category</param>
        /// <returns>A list of all products in the category</returns>
        public List<Product> GetAllByCategory(int categoryId)
        {
            var products = new List<Product>();
            using var connection = new SqliteConnection(_connStr);
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = @"
                SELECT p.id, p.name, p.price, p.barcode, c.id as categoryId, c.name as categoryName, p.imageUrl
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
        /// Get a product by barcode
        /// </summary>
        /// <param name="barcode">The barcode of the product</param>
        /// <returns>The product if found, otherwise null</returns>
        public Product? GetByBarcode(string barcode)
        {
            using var connection = new SqliteConnection(_connStr);
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = @"
                SELECT p.id, p.name, p.price, p.barcode, c.id as categoryId, c.name as categoryName, p.imageUrl
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

        /// <summary>
        /// Add a product
        /// </summary>
        /// <param name="product">The product to add</param>
        public void Add(Product product)
        {
            using var connection = new SqliteConnection(_connStr);
            connection.Open();

            using var command = connection.CreateCommand();

            command.CommandText = "INSERT INTO Products (name, price, barcode, categoryId, imageUrl) VALUES ($name, $price, $barcode, $categoryId, $imageUrl)";
            command.Parameters.AddWithValue("$name", product.Name);
            command.Parameters.AddWithValue("$price", product.Price);
            command.Parameters.AddWithValue("$barcode", product.Barcode);
            command.Parameters.AddWithValue("$categoryId", product.Category.Id);
            command.Parameters.AddWithValue("$imageUrl", product.ImageUrl ?? (object)DBNull.Value);
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

        /// <summary>
        /// Update a product
        /// </summary>
        /// <param name="product">The product to update</param>
        public void Update(Product product)
        {
            using var connection = new SqliteConnection(_connStr);
            connection.Open();

            using var command = connection.CreateCommand();

            command.CommandText = "UPDATE Products SET name = $name, price = $price, barcode = $barcode WHERE id = $id, categoryId = $categoryId, imageUrl = $imageUrl";
            command.Parameters.AddWithValue("$name", product.Name);
            command.Parameters.AddWithValue("$price", product.Price);
            command.Parameters.AddWithValue("$barcode", product.Barcode);
            command.Parameters.AddWithValue("$id", product.Id);
            command.Parameters.AddWithValue("$categoryId", product.Category.Id);
            command.Parameters.AddWithValue("$imageUrl", product.ImageUrl ?? (object)DBNull.Value);

            command.ExecuteNonQuery();
        }

        /// <summary>
        /// Delete a product
        /// </summary>
        /// <param name="id">The id of the product to delete</param>
        public void Delete(int id)
        {
            using var connection = new SqliteConnection(_connStr);
            connection.Open();

            using var command = connection.CreateCommand();

            command.CommandText = "DELETE FROM Products WHERE id = $id";
            command.Parameters.AddWithValue("$id", id);

            command.ExecuteNonQuery();
        }

        /// <summary>
        /// Add a category
        /// </summary>
        /// <param name="category">The category to add</param>
        public void AddCategory(ProductCategory category)
        {
            using var connection = new SqliteConnection(_connStr);
            connection.Open();

            using var command = connection.CreateCommand();

            command.CommandText = "INSERT INTO ProductCategories (name) VALUES ($name)";
            command.Parameters.AddWithValue("$name", category.Name);

            command.ExecuteNonQuery();
        }

        /// <summary>
        /// Update a category
        /// </summary>
        /// <param name="category">The category to update</param>
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

        /// <summary>
        /// Delete a category
        /// </summary>
        /// <param name="id">The id of the category to delete</param>
        public void DeleteCategory(int id)
        {
            using var connection = new SqliteConnection(_connStr);
            connection.Open();

            using var command = connection.CreateCommand();

            command.CommandText = "DELETE FROM ProductCategories WHERE id = $id";
            command.Parameters.AddWithValue("$id", id);

            command.ExecuteNonQuery();
        }

        /// <summary>
        /// Get all categories
        /// </summary>
        /// <returns>A list of all categories</returns>
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