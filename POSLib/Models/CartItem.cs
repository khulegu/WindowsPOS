using System.ComponentModel;

namespace POSLib.Models
{
    public class CartItem : INotifyPropertyChanged
    {
        public required Product Product { get; set; }

        private int _quantity;
        public int Quantity {
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

        public void Increment()
        {
            Quantity++;
        }

        public void Decrement()
        {
            if (Quantity > 0)
            {
                Quantity--;
            }
        }

        public string Name => Product.Name;
        public double Price => Product.Price;
        public double Total => Product.Price * Quantity;
        public string Discount => "0%";

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
