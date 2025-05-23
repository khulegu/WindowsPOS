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
        private TableLayoutPanel _mainTable;

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
            SuspendLayout();
            //
            // EditProductForm
            //
            ClientSize = new Size(700, 800);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "EditProductForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Edit Product";
            ResumeLayout(false);
        }

        private void InitializeControls()
        {
            _mainTable = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 6,
                Padding = new Padding(10),
                CellBorderStyle = TableLayoutPanelCellBorderStyle.Single,
            };

            // Configure columns
            _mainTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
            _mainTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70F));

            // Configure rows
            for (int i = 0; i < 6; i++)
            {
                _mainTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            }

            // Name
            var nameLabel = new Label
            {
                Text = "Name:",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleRight,
                Padding = new Padding(0, 0, 10, 0),
            };
            _nameTextBox = new TextBox { Dock = DockStyle.Fill };

            // Price
            var priceLabel = new Label
            {
                Text = "Price:",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleRight,
                Padding = new Padding(0, 0, 10, 0),
            };
            _priceTextBox = new TextBox { Dock = DockStyle.Fill };

            // Barcode
            var barcodeLabel = new Label
            {
                Text = "Barcode:",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleRight,
                Padding = new Padding(0, 0, 10, 0),
            };
            _barcodeTextBox = new TextBox { Dock = DockStyle.Fill };

            // Category
            var categoryLabel = new Label
            {
                Text = "Category:",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleRight,
                Padding = new Padding(0, 0, 10, 0),
            };
            _categoryComboBox = new ComboBox
            {
                Dock = DockStyle.Fill,
                DropDownStyle = ComboBoxStyle.DropDownList,
            };
            _categoryComboBox.DisplayMember = "Name";
            _categoryComboBox.ValueMember = "Id";
            _categoryComboBox.DataSource = _categories;

            // Image URL
            var imageUrlLabel = new Label
            {
                Text = "Image URL:",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleRight,
                Padding = new Padding(0, 0, 10, 0),
            };
            _imageUrlTextBox = new TextBox { Dock = DockStyle.Fill };

            // Buttons panel
            var buttonPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.RightToLeft,
                AutoSize = true,
            };

            _saveButton = new Button
            {
                Text = "Save",
                DialogResult = DialogResult.OK,
                Width = 140,
                Height = 56,
            };
            _saveButton.Click += SaveButton_Click;

            _cancelButton = new Button
            {
                Text = "Cancel",
                DialogResult = DialogResult.Cancel,
                Width = 140,
                Height = 56,
            };

            buttonPanel.Controls.Add(_cancelButton);
            buttonPanel.Controls.Add(_saveButton);

            // Add controls to table
            _mainTable.Controls.Add(nameLabel, 0, 0);
            _mainTable.Controls.Add(_nameTextBox, 1, 0);
            _mainTable.Controls.Add(priceLabel, 0, 1);
            _mainTable.Controls.Add(_priceTextBox, 1, 1);
            _mainTable.Controls.Add(barcodeLabel, 0, 2);
            _mainTable.Controls.Add(_barcodeTextBox, 1, 2);
            _mainTable.Controls.Add(categoryLabel, 0, 3);
            _mainTable.Controls.Add(_categoryComboBox, 1, 3);
            _mainTable.Controls.Add(imageUrlLabel, 0, 4);
            _mainTable.Controls.Add(_imageUrlTextBox, 1, 4);
            _mainTable.Controls.Add(buttonPanel, 1, 5);

            // Add table to form
            this.Controls.Add(_mainTable);
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
