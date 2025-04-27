using System.ComponentModel;

namespace POSLib.Models
{
    public interface ICartItem : INotifyPropertyChanged
    {
        public string Barcode { get; }
        public double Price { get; }
        public double Total { get; }
        public string Name { get; }
        public string Discount { get; }

        public int Quantity
        {
            get; set;
        }
        public void Increment();
        public void Decrement();
    }
}
