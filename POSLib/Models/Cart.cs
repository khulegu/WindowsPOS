using System.ComponentModel;

namespace POSLib.Models
{
    public class Cart: INotifyPropertyChanged
    {
        private readonly Dictionary<string, CartItem> _cart = [];

        public event PropertyChangedEventHandler? PropertyChanged;

        public BindingList<CartItem> CartItems { get; } = [];

        public void Add(Product product)
        {
            if (_cart.TryGetValue(product.Barcode, out CartItem? value))
            {
                value.Quantity++;
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

            OnPropertyChanged(nameof(Total));
        }

        public void RemoveFromCart(string barcode)
        {
            if (_cart.TryGetValue(barcode, out CartItem? value))
            {
                value.Quantity--;
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

            OnPropertyChanged(nameof(Total));
        }

        public double Total
        {
            get => CartItems.Sum(x => x.Product.Price * x.Quantity);
        }

        public void ClearCart()
        {
            _cart.Clear();
            CartItems.Clear();
        }

        protected virtual void OnPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
