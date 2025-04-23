using POSLib.Models;
using POSLib.Repositories;
using POSLib.Services;

namespace POSForm
{
    public partial class MainForm : Form
    {

        private static readonly string connStr = "Data Source=pos.db";
        private static readonly UserRepository userRepo = new(connStr);
        private static readonly AuthService authService = new(userRepo);
        private static readonly ProductRepository productRepo = new(connStr);

        private readonly User? _user = null;
        private readonly ProductService _productService = null!;
        private readonly Cart cart = new();

        public MainForm()
        {
            DatabaseInitializer.InitializeDatabase(connStr);

            _user = authService.Login("manager", "1234");

            if (_user == null)
            {
                MessageBox.Show("Login failed");
                return;
            }

            _productService = new ProductService(productRepo, _user);

            InitializeComponent();

            foreach (Product product in _productService.GetAllProducts())
            {
                ProductControl productPanel = new();
                productPanel.InitializeProduct(product);
                productPanel.ProductClicked += (sender, p) =>
                {
                    cart.Add(product);
                };
                productsLayout.Controls.Add(productPanel);
            }

            cartDataGrid.AutoGenerateColumns = false;
            cartDataGrid.DataSource = new BindingSource(cart.CartItems, null);

            Binding grandTotalBinding = new("Text", cart, "Total");

            grandTotalBinding.Format += (s, e) =>
            {
                if (e.Value is double total)
                {
                    e.Value = $"Total: {total:C}";
                }
            };

            label1.DataBindings.Add(grandTotalBinding);
        }
        private void PayButton_Click(object sender, EventArgs e)
        {
            // TODO: Implement payment logic
        }

        private void BarcodeTextBox_KeyPress(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string barcode = textBoxBarcode.Text;
                Product product = _productService.GetProductByBarcode(barcode);
                if (product != null)
                {
                    cart.Add(product);
                    textBoxBarcode.Clear();
                }
                else
                {
                    //MessageBox.Show("Product not found");
                }
            }
        }


        private void CartGridCell_DoubleClickOrClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0)
            {
                return;
            }
            string columnName = cartDataGrid.Columns[e.ColumnIndex].Name;
            CartItem cartItem = cart.CartItems.ElementAt(e.RowIndex);

            if (columnName == "Decrement")
            {
                cartItem.Decrement();
            }
            else if (columnName == "Increment")
            {
                cartItem.Increment();
            }
        }
    }
}
