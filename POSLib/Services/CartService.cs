using System.ComponentModel;
using POSLib.Models;

namespace POSLib.Services
{
    public class CartService
    {
        private readonly Dictionary<string, CartItem> _cart = new();
        public BindingList<CartItem> CartItems { get; } = new();

        public void AddToCart(Product product)
        {
            if (_cart.ContainsKey(product.Barcode))
            {
                _cart[product.Barcode].Quantity++;
                // Notify UI about the change
                var item = CartItems.FirstOrDefault(x => x.Product.Barcode == product.Barcode);
                if (item != null)
                {
                    var idx = CartItems.IndexOf(item);
                    CartItems.ResetItem(idx);
                }
            }
            else
            {
                var cartItem = new CartItem { Product = product, Quantity = 1 };
                _cart[product.Barcode] = cartItem;
                CartItems.Add(cartItem);
            }
        }

        public void RemoveFromCart(string barcode)
        {
            if (_cart.ContainsKey(barcode))
            {
                _cart[barcode].Quantity--;
                var item = CartItems.FirstOrDefault(x => x.Product.Barcode == barcode);
                if (_cart[barcode].Quantity <= 0)
                {
                    _cart.Remove(barcode);
                    if (item != null)
                        CartItems.Remove(item);
                }
                else if (item != null)
                {
                    var idx = CartItems.IndexOf(item);
                    CartItems.ResetItem(idx);
                }
            }
        }

        public double GetTotal() => CartItems.Sum(x => x.Product.Price * x.Quantity);

        public void ClearCart()
        {
            _cart.Clear();
            CartItems.Clear();
        }
    }
}
