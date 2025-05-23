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

        [TestInitialize]
        public void Setup()
        {
            if (File.Exists(_testDbPath))
            {
                File.Delete(_testDbPath);
            }

            var testConnectionString = $"Data Source={_testDbPath}";
            DatabaseInitializer.InitializeDatabase(testConnectionString);
            _repository = new ProductRepository(testConnectionString);
        }

        [TestMethod]
        public void GetAll_ReturnsProducts()
        {
            var products = _repository.GetAll();
            Assert.IsTrue(products.Count > 0, "Should return at least one product");
            Assert.AreEqual("Apples (Gala)", products[0].Name);
        }

        [TestMethod]
        public void GetByBarcode_ReturnsCorrectProduct()
        {
            var product = _repository.GetByBarcode("100001");
            Assert.IsNotNull(product, "Product should be found by barcode");
            Assert.AreEqual("Apples (Gala)", product.Name);
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
            Assert.AreEqual("Produce", categories[0].Name);
        }

        [TestMethod]
        public void GetAllByCategory_ReturnsProductsByCategory()
        {
            var categories = _repository.GetAllCategories();
            var beverageCategory = categories.Find(c => c.Name == "Produce");
            Assert.IsNotNull(beverageCategory, "Produce category should exist");

            var products = _repository.GetAllByCategory(beverageCategory.Id);
            Assert.IsTrue(products.Count > 0, "Should return at least one product");
            Assert.AreEqual("Apples (Gala)", products[0].Name);
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
            var category = new ProductCategory { Name = "Test" };

            _repository.AddCategory(category);

            var categories = _repository.GetAllCategories();
            var addedCategory = categories.Find(c => c.Name == "Test");
            Assert.IsNotNull(addedCategory, "Category should be added and found");
            Assert.AreEqual("Test", addedCategory.Name);
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

        [TestMethod]
        public void DeleteCategory_DeletesCategoryAndProducts()
        {
            var category = _repository.GetAllCategories()[0];
            _repository.DeleteCategory(category.Id);

            var products = _repository.GetAllByCategory(category.Id);
            Assert.AreEqual(0, products.Count, "Should return empty list for deleted category");

            var categories = _repository.GetAllCategories();
            var deletedCategory = categories.Find(c => c.Id == category.Id);
            Assert.IsNull(deletedCategory, "Category should be deleted");
        }

        [TestMethod]
        public void DeleteProduct_DeletesProduct()
        {
            var product = _repository.GetAll()[0];
            _repository.DeleteProduct(product.Id);

            Assert.IsNull(_repository.GetByBarcode(product.Barcode));
        }
    }
}
