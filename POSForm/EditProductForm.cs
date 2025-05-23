using System.Drawing;
using System.Windows.Forms;
using POSLib.Models;

namespace POSForm
{
    public partial class EditProductForm : Form
    {
        private readonly Product _product;
        private List<ProductCategory> _categories;
        private TextBox _nameTextBox;
        private TextBox _priceTextBox;
        private TextBox _barcodeTextBox;
        private TextBox _imageUrlTextBox;
        private ComboBox _categoryComboBox;
        private Button _saveButton;
        private Button _cancelButton;

        public Product? EditedProduct { get; private set; }

        public EditProductForm(Product product, List<ProductCategory> categories)
        {
            _product = product;
            _categories = categories;

            InitializeComponent();
            InitializeControls();
            LoadProductData();
        }

        private void InitializeComponent()
        {
            this.Text = "Edit Product";
            this.Size = new Size(400, 300);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;
        }

        private void InitializeControls()
        {
            // Name
            var nameLabel = new Label
            {
                Text = "Name:",
                Location = new Point(20, 20),
                AutoSize = true,
            };
            _nameTextBox = new TextBox { Location = new Point(120, 20), Width = 250 };

            // Price
            var priceLabel = new Label
            {
                Text = "Price:",
                Location = new Point(20, 50),
                AutoSize = true,
            };
            _priceTextBox = new TextBox { Location = new Point(120, 50), Width = 250 };

            // Barcode
            var barcodeLabel = new Label
            {
                Text = "Barcode:",
                Location = new Point(20, 80),
                AutoSize = true,
            };
            _barcodeTextBox = new TextBox { Location = new Point(120, 80), Width = 250 };

            // Category
            var categoryLabel = new Label
            {
                Text = "Category:",
                Location = new Point(20, 110),
                AutoSize = true,
            };
            _categoryComboBox = new ComboBox
            {
                Location = new Point(120, 110),
                Width = 250,
                DropDownStyle = ComboBoxStyle.DropDownList,
            };
            _categoryComboBox.DisplayMember = "Name";
            _categoryComboBox.ValueMember = "Id";
            _categoryComboBox.DataSource = _categories;

            // Image URL
            var imageUrlLabel = new Label
            {
                Text = "Image URL:",
                Location = new Point(20, 140),
                AutoSize = true,
            };
            _imageUrlTextBox = new TextBox { Location = new Point(120, 140), Width = 250 };

            // Buttons
            _saveButton = new Button
            {
                Text = "Save",
                DialogResult = DialogResult.OK,
                Location = new Point(120, 200),
            };
            _saveButton.Click += SaveButton_Click;

            _cancelButton = new Button
            {
                Text = "Cancel",
                DialogResult = DialogResult.Cancel,
                Location = new Point(220, 200),
            };

            // Add controls to form
            this.Controls.AddRange(
                new Control[]
                {
                    nameLabel,
                    _nameTextBox,
                    priceLabel,
                    _priceTextBox,
                    barcodeLabel,
                    _barcodeTextBox,
                    categoryLabel,
                    _categoryComboBox,
                    imageUrlLabel,
                    _imageUrlTextBox,
                    _saveButton,
                    _cancelButton,
                }
            );
        }

        private void LoadProductData()
        {
            _nameTextBox.Text = _product.Name;
            _priceTextBox.Text = _product.Price.ToString("F2");
            _barcodeTextBox.Text = _product.Barcode;
            _categoryComboBox.SelectedValue = _product.Category.Id;
            _imageUrlTextBox.Text = _product.ImageUrl ?? string.Empty;
        }

        private void SaveButton_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_nameTextBox.Text))
            {
                MessageBox.Show(
                    "Please enter a product name.",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return;
            }

            if (!double.TryParse(_priceTextBox.Text, out double price) || price < 0)
            {
                MessageBox.Show(
                    "Please enter a valid price.",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return;
            }

            if (string.IsNullOrWhiteSpace(_barcodeTextBox.Text))
            {
                MessageBox.Show(
                    "Please enter a barcode.",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return;
            }

            if (_categoryComboBox.SelectedItem == null)
            {
                MessageBox.Show(
                    "Please select a category.",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return;
            }

            EditedProduct = new Product
            {
                Id = _product.Id,
                Name = _nameTextBox.Text,
                Price = price,
                Barcode = _barcodeTextBox.Text,
                Category = (ProductCategory)_categoryComboBox.SelectedItem,
                ImageUrl = string.IsNullOrWhiteSpace(_imageUrlTextBox.Text)
                    ? null
                    : _imageUrlTextBox.Text,
            };

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
