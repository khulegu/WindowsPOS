using System.ComponentModel;

namespace POSLib.Models
{
    public class Cart : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public BindingList<ICartItem> CartItems { get; } = [];

        /// <summary>
        /// Бараа сагсанд нэмэх. Хэрвээ бараа сагсанд байгаа бол тоог нэмнэ.
        /// </summary>
        /// <param name="cartItem">Барааны обьект</param>
        public void AddItem(ICartItem cartItem)
        {
            ICartItem? matchingCartItem = CartItems.FirstOrDefault(x =>
                x.Barcode == cartItem.Barcode
            );

            if (matchingCartItem != null)
            {
                UpdateItemQuantity(matchingCartItem, cartItem.Quantity);
            }
            else
            {
                CartItems.Add(cartItem);
            }
            OnPropertyChanged(nameof(Total));
        }

        /// <summary>
        /// Updates the quantity of a product in the cart. If the quantity is less than or equal to 0, it removes the product from the cart.
        /// </summary>
        /// <param name="cartItem">Cart Item to update</param>
        /// <param name="by">How much to update quantity</param>
        public void UpdateItemQuantity(ICartItem cartItem, int by)
        {
            cartItem.Quantity += by;
            if (cartItem.Quantity <= 0)
            {
                CartItems.Remove(cartItem);
            }
            OnPropertyChanged(nameof(Total));
        }

        public double Total
        {
            get => CartItems.Sum(x => x.Total);
        }

        protected virtual void OnPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
