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
        private static UserRepository userRepo = new UserRepository(connStr);
        private static AuthService authService = new AuthService(userRepo);
        private static ProductRepository productRepo = new ProductRepository(connStr);

        private User _user;
        private ProductService _productService;

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
                    MessageBox.Show("Product not found");
                }
            }
        }

        private void AddProductToCart(Product product)
        {
            // Ehleed baraa sagsand baigaa esehiig shalgana.
            foreach (DataGridViewRow row in cartDataGrid.Rows)
            {
                if (row.Cells["ItemName"].Value.ToString() == product.Name)
                {
                    int quantity = Convert.ToInt32(row.Cells["Quantity"].Value);
                    quantity++;
                    row.Cells["Quantity"].Value = quantity;
                    row.Cells["Total"].Value = quantity * product.Price;
                    CalculateGrandTotal();
                    return;
                }
            }

            // Baihgui bol shine mur nemne.
            int newRowIndex = cartDataGrid.Rows.Add();
            DataGridViewRow newRow = cartDataGrid.Rows[newRowIndex];
            newRow.Cells["ItemName"].Value = product.Name;
            newRow.Cells["UnitPrice"].Value = product.Price;
            newRow.Cells["Quantity"].Value = 1;
            newRow.Cells["Total"].Value = product.Price;
            CalculateGrandTotal();
        }


        /// <summary>
        /// Calculates the total amount in the cart and updates the label.
        /// </summary>
        private void CalculateGrandTotal()
        {
            double total = 0;
            foreach (DataGridViewRow row in cartDataGrid.Rows)
            {
                if (row.Cells["Total"].Value != null)
                {
                    total += Convert.ToDouble(row.Cells["Total"].Value);
                }
            }
            label1.Text = $"Total: {total:C}";
        }


        private void UpdateTotal(int rowIndex)
        {
            int quantity = Convert.ToInt32(cartDataGrid.Rows[rowIndex].Cells[2].Value);
            double price = Convert.ToDouble(cartDataGrid.Rows[rowIndex].Cells[4].Value);
            double discount = Convert.ToDouble(cartDataGrid.Rows[rowIndex].Cells[5].Value);

            double total = quantity * price * (1 - (discount / 100));
            cartDataGrid.Rows[rowIndex].Cells[6].Value = Math.Round(total, 2);
        }

        private void cartDataGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Handle minus button click (column index 1)
            if (e.ColumnIndex == 1 && e.RowIndex >= 0)
            {
                int currentQuantity = Convert.ToInt32(cartDataGrid.Rows[e.RowIndex].Cells[2].Value);
                if (currentQuantity > 0)
                {
                    currentQuantity--;
                    cartDataGrid.Rows[e.RowIndex].Cells[2].Value = currentQuantity;
                    UpdateTotal(e.RowIndex);
                }
            }
            // Handle plus button click (column index 3)
            else if (e.ColumnIndex == 3 && e.RowIndex >= 0)
            {
                int currentQuantity = Convert.ToInt32(cartDataGrid.Rows[e.RowIndex].Cells[2].Value);
                currentQuantity++;
                cartDataGrid.Rows[e.RowIndex].Cells[2].Value = currentQuantity;
                UpdateTotal(e.RowIndex);
            }
        }
    }
}
