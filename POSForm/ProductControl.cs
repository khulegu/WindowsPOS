using System.Xml;
using POSLib.Models;

namespace POSForm
{
    public partial class ProductControl : UserControl
    {
        public ProductControl()
        {
            InitializeComponent();
        }

        public event EventHandler<Product>? ProductClicked;
        private Product? _product;

        public void InitializeProduct(Product product)
        {
            labelName.Text = product.Name;
            labelPrice.Text = $"${product.Price:F2}";
            labelBarcode.Text = product.Barcode;
            this.Click += ProductPanel_Click;
            pictureBox.Click += ProductPanel_Click;
            labelName.Click += ProductPanel_Click;
            labelPrice.Click += ProductPanel_Click;

            this.DoubleClick += ProductPanel_Click;
            pictureBox.DoubleClick += ProductPanel_Click;
            labelName.DoubleClick += ProductPanel_Click;
            labelPrice.DoubleClick += ProductPanel_Click;

            _product = product;
        }

        private void ProductPanel_Click(object? sender, EventArgs e)
        {
            ProductClicked?.Invoke(this, _product!);
        }
    }
}
