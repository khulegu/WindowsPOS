using POSLib.Models;

namespace POSLib.Services
{
    public class CartService
    {
        private readonly Dictionary<string, CartItem> _cart = new();

        public void AddToCart(Product product)
        {
            if (_cart.ContainsKey(product.Barcode))
                _cart[product.Barcode].Quantity++;
            else
                _cart[product.Barcode] = new CartItem { Product = product, Quantity = 1 };
        }

        public void RemoveFromCart(string barcode)
        {
            if (_cart.ContainsKey(barcode))
            {
                _cart[barcode].Quantity--;
                if (_cart[barcode].Quantity <= 0)
                    _cart.Remove(barcode);
            }
        }

        public List<CartItem> GetCartItems() => _cart.Values.ToList();

        public double GetTotal() => _cart.Values.Sum(x => x.Product.Price * x.Quantity);

        public void ClearCart() => _cart.Clear();
    }

}
