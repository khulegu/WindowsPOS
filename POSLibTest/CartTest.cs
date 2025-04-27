using System.ComponentModel;
using POSLib.Models;

namespace POSLibTest
{
    // Simple mock implementation of ICartItem for testing purposes
    public class MockCartItem : ICartItem
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public string Barcode { get; set; } = string.Empty;
        public double Price { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Discount { get; set; } = string.Empty; // Assuming simple string for test

        private int _quantity;
        public int Quantity
        {
            get => _quantity;
            set
            {
                if (_quantity != value)
                {
                    _quantity = value;
                    OnPropertyChanged(nameof(Quantity));
                    OnPropertyChanged(nameof(Total));
                }
            }
        }

        public double Total => Price * Quantity;

        public void Increment() => Quantity++;
        public void Decrement() => Quantity--;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }


    [TestClass]
    public sealed class CartTest
    {
        private MockCartItem _item1 = new MockCartItem { Barcode = "111", Name = "Item 1", Price = 10.0, Quantity = 1 };
        private MockCartItem _item2 = new MockCartItem { Barcode = "222", Name = "Item 2", Price = 25.5, Quantity = 1 };
        private MockCartItem _item1_duplicate = new MockCartItem { Barcode = "111", Name = "Item 1 Dup", Price = 10.0, Quantity = 2 }; // Same barcode, different quantity

        [TestMethod]
        public void AddItem_NewItem_AddsToCart()
        {
            // Arrange
            var cart = new Cart();

            // Act
            cart.AddItem(_item1);

            // Assert
            Assert.AreEqual(1, cart.CartItems.Count);
            Assert.AreSame(_item1, cart.CartItems[0]); // Should add the exact instance
            Assert.AreEqual(1, cart.CartItems[0].Quantity);
            Assert.AreEqual(10.0, cart.Total);
        }

        [TestMethod]
        public void AddItem_ExistingItem_UpdatesQuantity()
        {
            // Arrange
            var cart = new Cart();
            cart.AddItem(_item1); // Initial quantity 1

            // Act
            cart.AddItem(_item1_duplicate); // Add item with same barcode, quantity 2

            // Assert
            Assert.AreEqual(1, cart.CartItems.Count); // Count should remain 1
            Assert.AreSame(_item1, cart.CartItems[0]); // Should be the original instance
            Assert.AreEqual(1 + 2, cart.CartItems[0].Quantity); // Original quantity + new item's quantity
            Assert.AreEqual(10.0 * 3, cart.Total);
        }

        [TestMethod]
        public void UpdateItemQuantity_Increase_UpdatesQuantityAndTotal()
        {
            // Arrange
            var cart = new Cart();
            cart.AddItem(_item1); // Quantity 1
            var itemInCart = cart.CartItems[0];

            // Act
            cart.UpdateItemQuantity(itemInCart, 2); // Increase by 2

            // Assert
            Assert.AreEqual(3, itemInCart.Quantity);
            Assert.AreEqual(1, cart.CartItems.Count);
            Assert.AreEqual(10.0 * 3, cart.Total);
        }

        [TestMethod]
        public void UpdateItemQuantity_Decrease_UpdatesQuantityAndTotal()
        {
            // Arrange
            var cart = new Cart();
            cart.AddItem(_item1); // Quantity 1
            cart.UpdateItemQuantity(cart.CartItems[0], 2); // Quantity becomes 3
            var itemInCart = cart.CartItems[0];

            // Act
            cart.UpdateItemQuantity(itemInCart, -1); // Decrease by 1

            // Assert
            Assert.AreEqual(2, itemInCart.Quantity);
            Assert.AreEqual(1, cart.CartItems.Count);
            Assert.AreEqual(10.0 * 2, cart.Total);
        }

        [TestMethod]
        public void UpdateItemQuantity_DecreaseToZero_RemovesItem()
        {
            // Arrange
            var cart = new Cart();
            cart.AddItem(_item1); // Quantity 1
            cart.AddItem(_item2); // Quantity 1
            var itemToRemove = cart.CartItems.First(i => i.Barcode == _item1.Barcode);

            // Act
            cart.UpdateItemQuantity(itemToRemove, -1); // Decrease by 1

            // Assert
            Assert.AreEqual(1, cart.CartItems.Count); // Item1 should be removed
            Assert.IsFalse(cart.CartItems.Any(i => i.Barcode == _item1.Barcode));
            Assert.IsTrue(cart.CartItems.Any(i => i.Barcode == _item2.Barcode));
            Assert.AreEqual(_item2.Price * _item2.Quantity, cart.Total); // Only item2 left
        }

        [TestMethod]
        public void UpdateItemQuantity_DecreaseBelowZero_RemovesItem()
        {
            // Arrange
            var cart = new Cart();
            cart.AddItem(_item1); // Quantity 1
            var itemToRemove = cart.CartItems[0];

            // Act
            cart.UpdateItemQuantity(itemToRemove, -5); // Decrease by 5

            // Assert
            Assert.AreEqual(0, cart.CartItems.Count); // Item should be removed
            Assert.AreEqual(0, cart.Total);
        }

        [TestMethod]
        public void Total_CalculatesCorrectly_MultipleItems()
        {
            // Arrange
            var cart = new Cart();

            // Act
            cart.AddItem(_item1); // 1 * 10.0 = 10.0
            cart.AddItem(_item2); // 1 * 25.5 = 25.5
            cart.AddItem(_item1_duplicate); // Adds 2 to item1's quantity (now 3 * 10.0 = 30.0)

            // Assert
            Assert.AreEqual(30.0 + 25.5, cart.Total); // 55.5
        }

        [TestMethod]
        public void Total_IsZero_ForEmptyCart()
        {
            // Arrange
            var cart = new Cart();

            // Assert
            Assert.AreEqual(0, cart.Total);
        }


        [TestMethod]
        public void PropertyChanged_FiresForTotal_OnAddItem()
        {
            // Arrange
            var cart = new Cart();
            bool eventFired = false;
            string? changedProperty = null;
            cart.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(Cart.Total))
                {
                    eventFired = true;
                    changedProperty = args.PropertyName;
                }
            };

            // Act
            cart.AddItem(_item1);

            // Assert
            Assert.IsTrue(eventFired);
            Assert.AreEqual(nameof(Cart.Total), changedProperty);
        }

        [TestMethod]
        public void PropertyChanged_FiresForTotal_OnUpdateItemQuantity()
        {
            // Arrange
            var cart = new Cart();
            cart.AddItem(_item1);
            var itemInCart = cart.CartItems[0];
            bool eventFired = false;
            string? changedProperty = null;
            cart.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(Cart.Total))
                {
                    eventFired = true;
                    changedProperty = args.PropertyName;
                }
            };

            // Act
            cart.UpdateItemQuantity(itemInCart, 1); // Change quantity

            // Assert
            Assert.IsTrue(eventFired);
            Assert.AreEqual(nameof(Cart.Total), changedProperty);
        }

        [TestMethod]
        public void PropertyChanged_FiresForTotal_OnRemoveItemViaUpdate()
        {
            // Arrange
            var cart = new Cart();
            cart.AddItem(_item1);
            var itemInCart = cart.CartItems[0];
            bool eventFired = false;
            string? changedProperty = null;
            cart.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(Cart.Total))
                {
                    eventFired = true;
                    changedProperty = args.PropertyName;
                }
            };

            // Act
            cart.UpdateItemQuantity(itemInCart, -1); // Remove item

            // Assert
            Assert.IsTrue(eventFired);
            Assert.AreEqual(nameof(Cart.Total), changedProperty);
        }
    }
}