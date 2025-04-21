using System.Windows.Forms;
using POSLib.Exceptions;
using POSLib.Models;
using POSLib.Repositories;
using POSLib.Services;

namespace POSForm
{
    public partial class MainForm : Form
    {

        private static string connStr = "Data Source=pos.db";
        private static UserRepository userRepo = new(connStr);
        private static AuthService authService = new(userRepo);
        private static ProductRepository productRepo = new(connStr);

        private User _user;
        private ProductService _productService;
        private CartService _cartService = new();

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
            CalculateGrandTotal();

            foreach (Product product in _productService.GetAllProducts())
            {
                AddProductToCategory(product);
            }

            cartDataGrid.AutoGenerateColumns = false;
            cartDataGrid.DataSource = new BindingSource(_cartService.CartItems, null);
        }

        private void AddProductToCategory(Product product)
        {
            FlowLayoutPanel productsPanel = productsLayout;

            // Create product panel
            Panel productPanel = new Panel
            {
                Size = new Size(100, 120),
                BorderStyle = BorderStyle.FixedSingle,
                Margin = new Padding(5)
            };

            // Product image
            PictureBox image = new PictureBox
            {
                Size = new Size(90, 70),
                Location = new Point(5, 5),
                BackColor = Color.LightGray,
                SizeMode = PictureBoxSizeMode.Zoom
            };
            productPanel.Controls.Add(image);

            // Product name
            Label nameLabel = new Label
            {
                Text = product.Name,
                Location = new Point(5, 80),
                Size = new Size(90, 20),
                TextAlign = ContentAlignment.MiddleCenter
            };

            productPanel.Controls.Add(nameLabel);

            // Product price
            Label priceLabel = new Label
            {
                Text = $"${product.Price:F2}",
                Location = new Point(5, 100),
                Size = new Size(90, 20),
                TextAlign = ContentAlignment.MiddleCenter
            };
            productPanel.Controls.Add(priceLabel);

            // Add click event to add product to cart
            productPanel.Click += (sender, e) => AddProductToCart(product);
            image.Click += (sender, e) => AddProductToCart(product);
            nameLabel.Click += (sender, e) => AddProductToCart(product);
            priceLabel.Click += (sender, e) => AddProductToCart(product);

            // Add to the flow panel
            productsPanel.Controls.Add(productPanel);
        }

        private void buttonPay_Click(object sender, EventArgs e)
        {
            // TODO: Implement payment logic
        }

        private void textBoxBarcode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string barcode = textBoxBarcode.Text;
                Product product = _productService.GetProductByBarcode(barcode);
                if (product != null)
                {
                    AddProductToCart(product);
                    textBoxBarcode.Clear();
                }
                else
                {
                    //MessageBox.Show("Product not found");
                }
            }
        }

        private void AddProductToCart(Product product)
        {
            _cartService.AddToCart(product);
        }


        /// <summary>
        /// Calculates the total amount in the cart and updates the label.
        /// </summary>
        private void CalculateGrandTotal()
        {
            label1.Text = $"Total: {_cartService.GetTotal():C}";
        }

        private void cartDataGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            
            if (cartDataGrid.Columns[e.ColumnIndex].Name == "Decrement" && e.RowIndex >= 0)
            {
                _cartService.CartItems.ElementAt(e.RowIndex).Decrement();
            }
            else if (cartDataGrid.Columns[e.ColumnIndex].Name == "Increment" && e.RowIndex >= 0)
            {
                _cartService.CartItems.ElementAt(e.RowIndex).Increment();
            }
        }
    }
}
