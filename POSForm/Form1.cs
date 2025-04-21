using System;
using System.Drawing;
using System.Windows.Forms;

namespace POSForm
{
    public partial class MainForm : Form
    {
        private decimal totalAmount = 0.00M;
        private DataGridView cartGrid;
        private Label totalAmountLabel;
        private TabControl categoryTabs;

        // Use system fonts
        private Font defaultFont;
        private Font boldFont;
        private Font largeFont;
        private Font largeBoldFont;

        public MainForm()
        {
            InitializeFonts();
            InitializeComponent();
            LoadProductData();
            UpdateTotal();
        }

        private void InitializeFonts()
        {
            // Get the system's default font for dialog boxes
            defaultFont = SystemFonts.DefaultFont;

            // Create a bold version of the default font
            boldFont = new Font(defaultFont, FontStyle.Bold);

            // Create a larger version for headings
            largeFont = new Font(defaultFont.FontFamily, defaultFont.Size * 1.2f);

            // Create a larger, bold font for important elements
            largeBoldFont = new Font(defaultFont.FontFamily, defaultFont.Size * 1.2f, FontStyle.Bold);
        }

        private void InitializeComponent()
        {
            this.Text = "KhuleguPOS";
            this.Size = new Size(2000, 900);
            this.StartPosition = FormStartPosition.CenterScreen;

            SplitContainer mainSplitContainer = new SplitContainer
            {
                Dock = DockStyle.Fill,
                Orientation = Orientation.Vertical,
                SplitterDistance = 0,
                BackColor = Color.LightGray,
            };
            this.Controls.Add(mainSplitContainer);

            Panel leftPanel = new Panel
            {
                Dock = DockStyle.Fill
            };
            mainSplitContainer.Panel1.Controls.Add(leftPanel);

            // Cart/Items grid
            cartGrid = new DataGridView
            {
                Location = new Point(10, 50),
                Size = new Size(580, 500),
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                ReadOnly = true,
                ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
            };

            // Add columns to grid
            cartGrid.Columns.Add("ItemName", "Item Name");
            cartGrid.Columns.Add("Quantity", "Quantity");
            cartGrid.Columns.Add("UnitPrice", "U/Price");
            cartGrid.Columns.Add("Discount", "Dis%");
            cartGrid.Columns.Add("Total", "Total");

            cartGrid.Columns[0].Width = 150;
            leftPanel.Controls.Add(cartGrid);

            // Total section
            Label totalLabel = new Label
            {
                Text = "Total Price:",
                Font = new Font("Arial", 14, FontStyle.Bold),
                Location = new Point(10, 360),
                Size = new Size(120, 30)
            };
            leftPanel.Controls.Add(totalLabel);

            totalAmountLabel = new Label
            {
                Text = "$0.00",
                Font = new Font("Arial", 14, FontStyle.Bold),
                Location = new Point(130, 360),
                Size = new Size(150, 30)
            };
            leftPanel.Controls.Add(totalAmountLabel);

            // Numpad
            TableLayoutPanel numpad = new TableLayoutPanel
            {
                Location = new Point(10, 400),
                Size = new Size(300, 250),
                ColumnCount = 4,
                RowCount = 4,
                CellBorderStyle = TableLayoutPanelCellBorderStyle.Single
            };

            // Set column widths
            numpad.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            numpad.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            numpad.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            numpad.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));

            // Set row heights
            for (int i = 0; i < 4; i++)
            {
                numpad.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            }

            // Add numpad buttons
            string[] buttonLabels = { "7", "8", "9", "X", "4", "5", "6", "+", "1", "2", "3", "-", "0", "*", "Back", "C" };
            for (int i = 0; i < 16; i++)
            {
                Button btn = new Button
                {
                    Text = buttonLabels[i],
                    Dock = DockStyle.Fill,
                    Font = new Font("Arial", 14, FontStyle.Bold),
                    FlatStyle = FlatStyle.Flat
                };
                btn.Click += NumpadButton_Click;
                numpad.Controls.Add(btn, i % 4, i / 4);
            }
            leftPanel.Controls.Add(numpad);

            // Pay button
            Button payButton = new Button
            {
                Text = "Pay",
                Location = new Point(320, 400),
                Size = new Size(270, 50),
                Font = new Font("Arial", 14, FontStyle.Bold),
                BackColor = Color.Green,
                ForeColor = Color.White
            };
            payButton.Click += PayButton_Click;
            leftPanel.Controls.Add(payButton);

            // Right panel - Product categories and items
            Panel rightPanel = new Panel
            {
                Dock = DockStyle.Fill
            };
            mainSplitContainer.Panel2.Controls.Add(rightPanel);

            // Search bar
            TextBox searchBox = new TextBox
            {
                Location = new Point(10, 10),
                Size = new Size(400, 30),
                Font = new Font("Arial", 12),
                PlaceholderText = "Search products"
            };
            rightPanel.Controls.Add(searchBox);

            // Categories using TabControl
            categoryTabs = new TabControl
            {
                Location = new Point(10, 50),
                Size = new Size(550, 500),
                Font = new Font("Arial", 10)
            };

            string[] categoryNames = { "Bakery", "Dairy", "Meat", "Beverages", "Snacks", "Fruits" };
            foreach (string category in categoryNames)
            {
                TabPage tab = new TabPage(category);

                // Product display using FlowLayoutPanel
                FlowLayoutPanel productsPanel = new FlowLayoutPanel
                {
                    Dock = DockStyle.Fill,
                    AutoScroll = true,
                    Padding = new Padding(5)
                };
                tab.Controls.Add(productsPanel);

                categoryTabs.TabPages.Add(tab);
            }
            rightPanel.Controls.Add(categoryTabs);

            // Status bar
            StatusStrip statusStrip = new StatusStrip();
            ToolStripStatusLabel statusLabel = new ToolStripStatusLabel("Ready");
            statusStrip.Items.Add(statusLabel);
            this.Controls.Add(statusStrip);
        }

        private void LoadProductData()
        {
            // Add sample products to each category tab
            // Bakery products
            AddProductToCategory(0, "Croissant", 2.99M);
            AddProductToCategory(0, "Wheat Bread", 2.00M);
            AddProductToCategory(0, "White Bread", 2.00M);
            AddProductToCategory(0, "Choc Chip Cookie", 1.50M);

            // Dairy products
            AddProductToCategory(1, "Milk", 3.50M);
            AddProductToCategory(1, "Yogurt", 1.25M);
            AddProductToCategory(1, "Cheese", 4.99M);
            AddProductToCategory(1, "Butter", 2.75M);

            // Meat products
            AddProductToCategory(2, "Chicken Breast", 6.99M);
            AddProductToCategory(2, "Ground Beef", 5.50M);
            AddProductToCategory(2, "Pork Chops", 7.25M);
            AddProductToCategory(2, "Bacon", 4.99M);

            // Beverages
            AddProductToCategory(3, "Water", 1.50M);
            AddProductToCategory(3, "Soda", 2.25M);
            AddProductToCategory(3, "Coffee", 3.99M);
            AddProductToCategory(3, "Tea", 2.99M);

            // More products can be added as needed
        }

        private void AddProductToCategory(int tabIndex, string productName, decimal price)
        {
            if (tabIndex >= categoryTabs.TabPages.Count) return;

            TabPage tab = categoryTabs.TabPages[tabIndex];
            FlowLayoutPanel productsPanel = tab.Controls[0] as FlowLayoutPanel;

            // Create product panel
            Panel product = new Panel
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
            product.Controls.Add(image);

            // Product name
            Label nameLabel = new Label
            {
                Text = productName,
                Location = new Point(5, 80),
                Size = new Size(90, 20),
                TextAlign = ContentAlignment.MiddleCenter
            };
            product.Controls.Add(nameLabel);

            // Product price
            Label priceLabel = new Label
            {
                Text = $"${price:F2}",
                Location = new Point(5, 100),
                Size = new Size(90, 20),
                TextAlign = ContentAlignment.MiddleCenter
            };
            product.Controls.Add(priceLabel);

            // Add click event to add product to cart
            product.Click += (sender, e) => AddToCart(productName, price);
            image.Click += (sender, e) => AddToCart(productName, price);
            nameLabel.Click += (sender, e) => AddToCart(productName, price);
            priceLabel.Click += (sender, e) => AddToCart(productName, price);

            // Add to the flow panel
            productsPanel.Controls.Add(product);
        }

        private void AddToCart(string productName, decimal price)
        {
            // Check if item already exists in cart
            foreach (DataGridViewRow row in cartGrid.Rows)
            {
                if (row.Cells["ItemName"].Value.ToString() == productName)
                {
                    // Increment quantity
                    int qty = Convert.ToInt32(row.Cells["Quantity"].Value) + 1;
                    row.Cells["Quantity"].Value = qty;
                    row.Cells["Total"].Value = (qty * price).ToString("C2");
                    UpdateTotal();
                    return;
                }
            }

            // Add new item
            cartGrid.Rows.Add(productName, 1, price.ToString("C2"), 0, price.ToString("C2"));
            UpdateTotal();
        }

        private void UpdateTotal()
        {
            totalAmount = 0;

            foreach (DataGridViewRow row in cartGrid.Rows)
            {
                string totalStr = row.Cells["Total"].Value.ToString().Replace("$", "");
                decimal rowTotal;
                if (Decimal.TryParse(totalStr, out rowTotal))
                {
                    totalAmount += rowTotal;
                }
            }

            totalAmountLabel.Text = totalAmount.ToString("C2");
        }

        private void NumpadButton_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            // Handle numpad button clicks
            // For a real application, implement quantity changing or other functionality
            MessageBox.Show($"Numpad button {btn.Text} pressed");
        }

        private void PayButton_Click(object sender, EventArgs e)
        {
            if (totalAmount > 0)
            {
                MessageBox.Show($"Processing payment for {totalAmount:C2}");
                // Clear cart and reset total after payment
                cartGrid.Rows.Clear();
                UpdateTotal();
            }
            else
            {
                MessageBox.Show("No items in cart");
            }
        }
    }
}