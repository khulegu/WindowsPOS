using System.ComponentModel;

namespace POSLib.Models
{
    /// <summary>
    /// A product cart item
    /// </summary>
    public class ProductCartItem : ICartItem
    {
        /// <summary>
        /// The product of the cart item
        /// </summary>
        public required Product Product { get; set; }

        private int _quantity;

        /// <summary>
        /// The quantity of the cart item
        /// </summary>
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

        /// <summary>
        /// The barcode of the product
        /// </summary>
        public string Barcode => Product.Barcode;

        /// <summary>
        /// Increment the quantity of the cart item
        /// </summary>
        public void Increment()
        {
            Quantity++;
        }

        /// <summary>
        /// Decrement the quantity of the cart item
        /// </summary>
        public void Decrement()
        {
            if (Quantity > 0)
            {
                Quantity--;
            }
        }

        /// <summary>
        /// The name of the product
        /// </summary>
        public string Name => Product.Name;

        /// <summary>
        /// The price of the product
        /// </summary>
        public double Price => Product.Price;

        /// <summary>
        /// The total price of the cart item
        /// </summary>
        public double Total => Product.Price * Quantity;

        /// <summary>
        /// The discount of the cart item
        /// </summary>
        public string Discount => "0%";

        /// <summary>
        /// The event handler for property changes
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// On property changed
        /// </summary>
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
