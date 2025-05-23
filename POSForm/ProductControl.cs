using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using POSLib.Models;

namespace POSForm
{
    public partial class ProductControl : UserControl
    {
        private bool _isSelected = false;
        public event EventHandler<Product>? ProductClicked;
        private Product? _product;

        public Product? Product => _product;
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                if (_isSelected)
                {
                    this.BackColor = Color.LightBlue;
                }
                else
                {
                    this.BackColor = SystemColors.Control;
                }
            }
        }

        public ProductControl()
        {
            InitializeComponent();
        }

        public void InitializeProduct(Product product)
        {
            labelName.Text = product.Name;
            labelPrice.Text = $"${product.Price:F2}";
            labelBarcode.Text = product.Barcode;
            if (product.ImageUrl != null)
            {
                Debug.WriteLine($"Loading image from {product.ImageUrl}");
                pictureBox.LoadAsync(product.ImageUrl);
                pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            }
            this.Click += ProductPanel_Click;
            pictureBox.Click += ProductPanel_Click;
            labelName.Click += ProductPanel_Click;
            labelPrice.Click += ProductPanel_Click;
            labelBarcode.Click += ProductPanel_Click;

            this.DoubleClick += ProductPanel_Click;
            pictureBox.DoubleClick += ProductPanel_Click;
            labelName.DoubleClick += ProductPanel_Click;
            labelPrice.DoubleClick += ProductPanel_Click;
            labelBarcode.DoubleClick += ProductPanel_Click;

            _product = product;
        }

        private void ProductPanel_Click(object? sender, EventArgs e)
        {
            ProductClicked?.Invoke(this, _product!);
        }
    }
}
