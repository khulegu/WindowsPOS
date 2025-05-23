using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Microsoft.Data.Sqlite;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using POSLib.Models;
using POSLib.Repositories;

namespace POSLibTest
{
    [TestClass]
    public class ProductRepositoryTest
    {
        private readonly string _testDbPath = Path.GetTempFileName();
        public required ProductRepository _repository;
        public required SqliteConnection _connection;

        [TestInitialize]
        public void Setup()
        {
            if (File.Exists(_testDbPath))
            {
                File.Delete(_testDbPath);
            }

            var testConnectionString = $"Data Source={_testDbPath}";
            // Create and keep the connection open for in-memory database
            _connection = new SqliteConnection(testConnectionString);
            _connection.Open();

            // Initialize repository with the connection string
            _repository = new ProductRepository(testConnectionString);

            try
            {
                // Create tables
                var createCategoryTable = _connection.CreateCommand();
                createCategoryTable.CommandText =
                    "CREATE TABLE ProductCategories (Id INTEGER PRIMARY KEY AUTOINCREMENT, Name TEXT)";
                createCategoryTable.ExecuteNonQuery();

                var createProductTable = _connection.CreateCommand();
                createProductTable.CommandText =
                    @"
                    CREATE TABLE Products (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Name TEXT,
                        Price REAL,
                        Barcode TEXT UNIQUE,
                        CategoryId INTEGER,
                        ImageUrl TEXT,
                        FOREIGN KEY(CategoryId) REFERENCES ProductCategories(Id)
                    )";
                createProductTable.ExecuteNonQuery();

                // Insert test category
                var insertCategory = _connection.CreateCommand();
                insertCategory.CommandText =
                    "INSERT INTO ProductCategories (Name) VALUES ('Beverages')";
                insertCategory.ExecuteNonQuery();

                // Get inserted category id
                var getCategoryId = _connection.CreateCommand();
                getCategoryId.CommandText =
                    "SELECT Id FROM ProductCategories WHERE Name = 'Beverages'";
                var result = getCategoryId.ExecuteScalar();

                if (result == null)
                    throw new InvalidOperationException("Failed to insert test category");

                int categoryId = Convert.ToInt32(result);

                // Insert test product
                var insertProduct = _connection.CreateCommand();
                insertProduct.CommandText =
                    "INSERT INTO Products (Name, Price, Barcode, CategoryId, ImageUrl) VALUES ('Coke', 1.5, '123456', @CategoryId, 'coke.png')";
                insertProduct.Parameters.AddWithValue("@CategoryId", categoryId);
                insertProduct.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to setup test database", ex);
            }
        }

        [TestCleanup]
        public void Cleanup()
        {
            // Dispose repository if it implements IDisposable
            // _repository?.Dispose();

            // Close and dispose the connection
            _connection?.Close();
            _connection?.Dispose();
        }

        [TestMethod]
        public void GetAll_ReturnsProducts()
        {
            var products = _repository.GetAll();
            Assert.IsTrue(products.Count > 0, "Should return at least one product");
            Assert.AreEqual("Coke", products[0].Name);
        }

        [TestMethod]
        public void GetByBarcode_ReturnsCorrectProduct()
        {
            var product = _repository.GetByBarcode("123456");
            Assert.IsNotNull(product, "Product should be found by barcode");
            Assert.AreEqual("Coke", product.Name);
        }

        [TestMethod]
        public void GetByBarcode_ReturnsNullForNonExistentBarcode()
        {
            var product = _repository.GetByBarcode("999999");
            Assert.IsNull(product, "Should return null for non-existent barcode");
        }

        [TestMethod]
        public void Add_AddsProduct()
        {
            // Arrange
            var category = _repository.GetAllCategories()[0];
            var newProduct = new Product
            {
                Name = "Pepsi",
                Price = 1.4,
                Barcode = "654321",
                Category = category,
                ImageUrl = "pepsi.png",
            };

            // Act
            _repository.Add(newProduct);

            // Assert
            var product = _repository.GetByBarcode("654321");
            Assert.IsNotNull(product, "Product should be added and found");
            Assert.AreEqual("Pepsi", product.Name);
            Assert.AreEqual(1.4, product.Price);
        }

        [TestMethod]
        public void Add_AddsProductWithNullImageUrl()
        {
            var category = _repository.GetAllCategories()[0];
            var newProduct = new Product
            {
                Name = "Pepsi",
                Price = 1.4,
                Barcode = "654322",
                Category = category,
                ImageUrl = null,
            };

            // Act
            _repository.Add(newProduct);

            // Assert
            var product = _repository.GetByBarcode("654322");
            Assert.IsNotNull(product, "Product should be added and found");
            Assert.AreEqual("Pepsi", product.Name);
            Assert.AreEqual(1.4, product.Price);
            Assert.IsNull(product.ImageUrl, "ImageUrl should be null");
        }

        [TestMethod]
        public void Add_ThrowsExceptionIfBarcodeAlreadyExists()
        {
            var category = _repository.GetAllCategories()[0];
            var existingProduct = _repository.GetAll()[0];
            Debug.WriteLine(existingProduct.Barcode);
            var newProduct = new Product
            {
                Name = "Pepsi",
                Price = 1.4,
                Barcode = existingProduct.Barcode, // Same barcode as existing product
                Category = category,
                ImageUrl = "pepsi.png",
            };

            // Act & Assert
            Assert.ThrowsException<DuplicateNameException>(() => _repository.Add(newProduct));
        }

        [TestMethod]
        public void GetAllCategories_ReturnsCategories()
        {
            var categories = _repository.GetAllCategories();
            Assert.IsTrue(categories.Count > 0, "Should return at least one category");
            Assert.AreEqual("Beverages", categories[0].Name);
        }

        [TestMethod]
        public void GetAllByCategory_ReturnsProductsByCategory()
        {
            var categories = _repository.GetAllCategories();
            var beverageCategory = categories.Find(c => c.Name == "Beverages");
            Assert.IsNotNull(beverageCategory, "Beverages category should exist");

            var products = _repository.GetAllByCategory(beverageCategory.Id);
            Assert.IsTrue(products.Count > 0, "Should return at least one product");
            Assert.AreEqual("Coke", products[0].Name);
        }

        [TestMethod]
        public void GetAllByCategory_ReturnsEmptyListForNonExistentCategory()
        {
            var products = _repository.GetAllByCategory(999);
            Assert.AreEqual(
                0,
                products.Count,
                "Should return empty list for non-existent category"
            );
        }

        [TestMethod]
        public void AddCategory_AddsCategory()
        {
            var category = new ProductCategory { Name = "Snacks" };

            _repository.AddCategory(category);

            var categories = _repository.GetAllCategories();
            var addedCategory = categories.Find(c => c.Name == "Snacks");
            Assert.IsNotNull(addedCategory, "Category should be added and found");
            Assert.AreEqual("Snacks", addedCategory.Name);
        }

        [TestMethod]
        public void AddCategory_AssignsId()
        {
            var category = new ProductCategory { Name = "Electronics" };

            _repository.AddCategory(category);

            var categories = _repository.GetAllCategories();
            var addedCategory = categories.Find(c => c.Name == "Electronics");
            Assert.IsNotNull(addedCategory, "Category should be added");
            Assert.IsTrue(addedCategory.Id > 0, "Category should have an assigned ID");
        }
    }
}
