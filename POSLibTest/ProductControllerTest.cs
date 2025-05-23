using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using POSLib.Controllers;
using POSLib.Models;
using POSLib.Repositories;

namespace POSLibTest
{
    [TestClass]
    public class ProductControllerTest
    {
        public required TestProductRepository _testProductRepo;
        public required User _testUser;
        public required ProductController _controller;

        [TestInitialize]
        public void Setup()
        {
            _testProductRepo = new TestProductRepository();
            _testUser = new User
            {
                Username = "testuser",
                Password = "testpass",
                Role = Role.Manager,
                Permissions = new List<Permission>
                {
                    Permission.AddProducts,
                    Permission.DeleteCategories,
                    Permission.DeleteProducts,
                    Permission.EditProducts,
                },
            };
            _controller = new ProductController(_testProductRepo, _testUser);
        }

        [TestMethod]
        public void InitializeProducts_ShouldLoadCategoriesAndProducts()
        {
            // Arrange
            var category1 = new ProductCategory { Id = 1, Name = "Category 1" };
            var category2 = new ProductCategory { Id = 2, Name = "Category 2" };
            _testProductRepo.AddCategory(category1);
            _testProductRepo.AddCategory(category2);

            var product1 = new Product
            {
                Id = 1,
                Name = "Product 1",
                Barcode = "123",
                Category = category1,
            };
            var product2 = new Product
            {
                Id = 2,
                Name = "Product 2",
                Barcode = "456",
                Category = category1,
            };
            _testProductRepo.AddProduct(product1);
            _testProductRepo.AddProduct(product2);

            // Act
            _controller.InitializeProducts();

            // Assert
            Assert.AreEqual(2, _controller.Categories.Count);
            Assert.AreEqual(2, _controller.ProductsFiltered.Count);
        }

        [TestMethod]
        public void SelectedCategory_WhenChanged_ShouldUpdateProductsFiltered()
        {
            // Arrange
            var category = new ProductCategory { Id = 1, Name = "Category 1" };
            _testProductRepo.AddCategory(category);

            var product1 = new Product
            {
                Id = 1,
                Name = "Product 1",
                Barcode = "123",
                Category = category,
            };
            var product2 = new Product
            {
                Id = 2,
                Name = "Product 2",
                Barcode = "456",
                Category = category,
            };
            _testProductRepo.AddProduct(product1);
            _testProductRepo.AddProduct(product2);

            // Act
            _controller.SelectedCategory = category;

            // Assert
            Assert.AreEqual(category, _controller.SelectedCategory);
            Assert.AreEqual(2, _controller.ProductsFiltered.Count);
        }

        [TestMethod]
        public void GetProductByBarcode_ShouldReturnCorrectProduct()
        {
            // Arrange
            var barcode = "123456789";
            var expectedProduct = new Product
            {
                Id = 1,
                Barcode = barcode,
                Name = "Test Product",
                Category = new ProductCategory { Id = 1, Name = "Test Category" },
            };
            _testProductRepo.AddProduct(expectedProduct);

            // Act
            var result = _controller.GetProductByBarcode(barcode);

            // Assert
            Assert.AreEqual(expectedProduct, result);
        }

        [TestMethod]
        public void AddProduct_WithValidPermission_ShouldAddProduct()
        {
            // Arrange
            var product = new Product
            {
                Id = 1,
                Name = "New Product",
                Barcode = "123",
                Category = new ProductCategory { Id = 1, Name = "Test Category" },
            };

            // Act
            _controller.AddProduct(product);

            // Assert
            var addedProduct = _testProductRepo.GetByBarcode("123");
            Assert.IsNotNull(addedProduct);
            Assert.AreEqual(product.Name, addedProduct.Name);
        }

        [TestMethod]
        public void DeleteCategory_WithValidPermission_ShouldDeleteCategory()
        {
            // Arrange
            var category = new ProductCategory { Id = 1, Name = "Test Category" };
            _testProductRepo.AddCategory(category);

            // Act
            _controller.DeleteCategory(category.Id);

            // Assert
            var deletedCategory = _testProductRepo
                .GetAllCategories()
                .FirstOrDefault(c => c.Id == category.Id);
            Assert.IsNull(deletedCategory);
        }

        [TestMethod]
        [ExpectedException(typeof(UnauthorizedAccessException))]
        public void AddProduct_WithoutPermission_ShouldThrowException()
        {
            // Arrange
            var unauthorizedUser = new User
            {
                Username = "unauthorized",
                Password = "testpass",
                Role = Role.Cashier,
                Permissions = new List<Permission>(),
            };
            var controller = new ProductController(_testProductRepo, unauthorizedUser);
            var product = new Product
            {
                Id = 1,
                Name = "New Product",
                Barcode = "123",
                Category = new ProductCategory { Id = 1, Name = "Test Category" },
            };

            // Act
            controller.AddProduct(product);

            // Assert is handled by ExpectedException attribute
        }

        [TestMethod]
        public void PropertyChanged_ShouldBeRaisedWhenSelectedCategoryChanges()
        {
            // Arrange
            var category = new ProductCategory { Id = 1, Name = "Category 1" };
            var propertyChangedRaised = false;
            _controller.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(ProductController.SelectedCategory))
                {
                    propertyChangedRaised = true;
                }
            };

            // Act
            _controller.SelectedCategory = category;

            // Assert
            Assert.IsTrue(propertyChangedRaised);
        }

        [TestMethod]
        public void DeleteProduct_WithValidPermission_ShouldDeleteProduct()
        {
            // Arrange
            var product = new Product
            {
                Id = 1,
                Name = "Test Product",
                Barcode = "123",
                Category = new ProductCategory { Id = 1, Name = "Test Category" },
            };
            _testProductRepo.AddProduct(product);

            // Act
            _controller.DeleteProduct(product.Id);

            // Assert
            var deletedProduct = _testProductRepo.GetByBarcode("123");
            Assert.IsNull(deletedProduct);
        }

        [TestMethod]
        [ExpectedException(typeof(UnauthorizedAccessException))]
        public void DeleteProduct_WithoutPermission_ShouldThrowException()
        {
            // Arrange
            var unauthorizedUser = new User
            {
                Username = "unauthorized",
                Password = "testpass",
                Role = Role.Cashier,
                Permissions = new List<Permission>(),
            };

            var controller = new ProductController(_testProductRepo, unauthorizedUser);
            var product = new Product
            {
                Id = 1,
                Name = "Test Product",
                Barcode = "123",
                Category = new ProductCategory { Id = 1, Name = "Test Category" },
            };

            // Act
            controller.DeleteProduct(product.Id);

            // Assert is handled by ExpectedException attribute
        }

        [TestMethod]
        [ExpectedException(typeof(UnauthorizedAccessException))]
        public void DeleteCategory_WithoutPermission_ShouldThrowException()
        {
            // Arrange
            var unauthorizedUser = new User
            {
                Username = "unauthorized",
                Password = "testpass",
                Role = Role.Cashier,
                Permissions = new List<Permission>(),
            };

            var controller = new ProductController(_testProductRepo, unauthorizedUser);
            var category = new ProductCategory { Id = 1, Name = "Test Category" };
            _testProductRepo.AddCategory(category);

            // Act
            controller.DeleteCategory(category.Id);

            // Assert is handled by ExpectedException attribute
        }

        [TestMethod]
        public void EditProduct_WithValidPermission_ShouldEditProduct()
        {
            // Arrange
            var product = new Product
            {
                Id = 1,
                Name = "Test Product",
                Barcode = "123",
                Category = new ProductCategory { Id = 1, Name = "Test Category" },
            };
            _testProductRepo.AddProduct(product);

            var updatedProduct = new Product
            {
                Id = 1,
                Name = "Updated Product",
                Barcode = "123",
                Category = new ProductCategory { Id = 1, Name = "Test Category" },
            };

            // Act
            _controller.EditProduct(updatedProduct);

            // Assert
            var updatedProductGot = _testProductRepo.GetByBarcode("123");
            Assert.IsNotNull(updatedProductGot);
            Assert.AreEqual("Updated Product", updatedProductGot.Name);
        }

        [TestMethod]
        [ExpectedException(typeof(UnauthorizedAccessException))]
        public void EditProduct_WithoutPermission_ShouldThrowException()
        {
            // Arrange
            var unauthorizedUser = new User
            {
                Username = "unauthorized",
                Password = "testpass",
                Role = Role.Cashier,
                Permissions = new List<Permission>(),
            };
            var controller = new ProductController(_testProductRepo, unauthorizedUser);
            var product = new Product
            {
                Id = 1,
                Name = "Test Product",
                Barcode = "123",
                Category = new ProductCategory { Id = 1, Name = "Test Category" },
            };

            // Act
            controller.EditProduct(product);

            // Assert is handled by ExpectedException attribute
        }
    }

    // Test implementation of IProductRepository
    public class TestProductRepository : IProductRepository
    {
        private readonly List<Product> _products = new();
        private readonly List<ProductCategory> _categories = new();

        public List<Product> GetAll()
        {
            return _products;
        }

        public void AddCategory(ProductCategory category)
        {
            _categories.Add(category);
        }

        public void AddProduct(Product product)
        {
            _products.Add(product);
        }

        public void Add(Product product)
        {
            _products.Add(product);
        }

        public List<ProductCategory> GetAllCategories()
        {
            return _categories;
        }

        public List<Product> GetAllByCategory(int categoryId)
        {
            return _products.Where(p => p.Category.Id == categoryId).ToList();
        }

        public Product? GetByBarcode(string barcode)
        {
            return _products.FirstOrDefault(p => p.Barcode == barcode);
        }

        public void DeleteCategory(int categoryId)
        {
            _categories.RemoveAll(c => c.Id == categoryId);
        }

        public void DeleteProduct(int productId)
        {
            _products.RemoveAll(p => p.Id == productId);
        }

        public void UpdateProduct(Product product)
        {
            var index = _products.FindIndex(p => p.Id == product.Id);
            if (index != -1)
            {
                _products[index] = product;
            }
        }
    }
}
