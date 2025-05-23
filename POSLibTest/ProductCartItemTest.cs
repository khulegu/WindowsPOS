using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using POSLib.Models;

namespace POSLibTest
{
    [TestClass]
    public class ProductCartItemTests
    {
        public required Product _testProduct;

        [TestInitialize]
        public void Setup()
        {
            _testProduct = new Product
            {
                Barcode = "123456",
                Name = "Test Product",
                Price = 10.0,
                Category = new ProductCategory { Name = "Test Category" },
            };
        }

        [TestMethod]
        public void Quantity_SetValue_RaisesPropertyChangedAndUpdatesTotal()
        {
            var cartItem = new ProductCartItem { Product = _testProduct };
            var changes = new List<string>();

            cartItem.PropertyChanged += (s, e) => changes.Add(e.PropertyName!);

            cartItem.Quantity = 3;

            Assert.AreEqual(3, cartItem.Quantity);
            Assert.AreEqual(30.0, cartItem.Total);
            CollectionAssert.AreEqual(new[] { "Quantity", "Total" }, changes);
        }

        [TestMethod]
        public void Increment_IncreasesQuantity()
        {
            var cartItem = new ProductCartItem { Product = _testProduct };
            cartItem.Quantity = 1;
            cartItem.Increment();
            Assert.AreEqual(2, cartItem.Quantity);
        }

        [TestMethod]
        public void Decrement_DecreasesQuantity_WhenAboveZero()
        {
            var cartItem = new ProductCartItem { Product = _testProduct };
            cartItem.Quantity = 2;
            cartItem.Decrement();
            Assert.AreEqual(1, cartItem.Quantity);
        }

        [TestMethod]
        public void Decrement_DoesNotGoBelowZero()
        {
            var cartItem = new ProductCartItem { Product = _testProduct };
            cartItem.Quantity = 0;
            cartItem.Decrement();
            Assert.AreEqual(0, cartItem.Quantity);
        }

        [TestMethod]
        public void Properties_ReflectProductValues()
        {
            var cartItem = new ProductCartItem { Product = _testProduct };
            Assert.AreEqual("Test Product", cartItem.Name);
            Assert.AreEqual("123456", cartItem.Barcode);
            Assert.AreEqual(10.0, cartItem.Price);
            Assert.AreEqual("0%", cartItem.Discount);
        }
    }
}
