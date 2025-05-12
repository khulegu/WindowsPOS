using System.ComponentModel;
using POSForm.Controls;
using POSLib.Models;
using POSLib.Repositories;
using POSLib.Controllers;

namespace POSForm
{
    public partial class MainForm : Form
    {
        private static readonly string connStr = "Data Source=pos.db";
        private static readonly ProductRepository productRepo = new(connStr);

        private readonly User _user;
        private readonly ProductController _productService;
        private readonly Cart cart = new();

        public MainForm(User user)
        {
            _user = user;
            _productService = new ProductController(productRepo, _user);

            InitializeComponent();
            InitializeMenuStrip();

            loggedUserLabel.Text = $"User: {_user.Username} ({_user.Role})";

            _productService.ProductsFiltered.ListChanged += (s, e) =>
            {
                switch (e.ListChangedType)
                {
                    case ListChangedType.ItemAdded:
                        productsLayout.Controls.Add(CreateProductControl(_productService.ProductsFiltered[e.NewIndex]));
                        break;
                    case ListChangedType.ItemDeleted:
                        productsLayout.Controls.RemoveAt(e.NewIndex);
                        break;
                    case ListChangedType.ItemChanged:
                        productsLayout.Controls[e.NewIndex].Refresh();
                        break;
                    case ListChangedType.Reset:
                        productsLayout.Controls.Clear();
                        foreach (var product in _productService.ProductsFiltered)
                        {
                            productsLayout.Controls.Add(CreateProductControl(product));
                        }
                        break;
                }
            };

            _productService.Categories.ListChanged += (s, e) =>
            {
                switch (e.ListChangedType)
                {
                    case ListChangedType.ItemAdded:
                        categoriesLayout.Controls.Add(CreateCategoryControl(_productService.Categories[e.NewIndex]));
                        break;
                    case ListChangedType.ItemDeleted:
                        categoriesLayout.Controls.RemoveAt(e.NewIndex);
                        break;
                    case ListChangedType.ItemChanged:
                        categoriesLayout.Controls[e.NewIndex].Refresh();
                        break;
                    case ListChangedType.Reset:
                        categoriesLayout.Controls.Clear();
                        foreach (var category in _productService.Categories)
                        {
                            categoriesLayout.Controls.Add(CreateCategoryControl(category));
                        }
                        break;
                }
            };

            _productService.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(_productService.SelectedCategory))
                {
                    foreach (ProductCategoryControl categoryControl in categoriesLayout.Controls)
                    {
                        categoryControl.IsSelected = categoryControl.Category?.Id == _productService.SelectedCategory?.Id;
                    }
                }
            };

            _productService.InitializeProducts();
            _productService.SelectedCategory = _productService.Categories.FirstOrDefault();

            /// Cart data grid
            cartDataGrid.AutoGenerateColumns = false;
            cartDataGrid.DataSource = new BindingSource(cart.CartItems, null);

            /// Grand total label
            Binding grandTotalBinding = new("Text", cart, "Total");
            grandTotalBinding.Format += (s, e) =>
            {
                if (e.Value is double total)
                {
                    e.Value = $"Total: {total:C}";
                }
            };
            grandTotalLabel.DataBindings.Add(grandTotalBinding);

            /// Date label
            UpdateDateLabel();
            System.Windows.Forms.Timer timer = new();
            timer.Interval = 1000; // 1 second
            timer.Tick += (s, e) =>
            {
                UpdateDateLabel();
            };
            timer.Start();
        }

        private void InitializeMenuStrip()
        {
            menuStrip.Items.Clear();


            var permissions = _user.Permissions;
            var permissionGroups = permissions.Select(p => p.GetPermissionGroup()).Distinct().ToList();

            foreach (var permissionGroup in permissionGroups)
            {
                var permissionGroupMenuItem = new ToolStripMenuItem(permissionGroup);

                foreach (var permission in permissions)
                {
                    if (permission.GetPermissionGroup() == permissionGroup)
                    {
                        permissionGroupMenuItem.DropDownItems.Add(new ToolStripMenuItem(permission.GetPermissionDescription()));
                    }
                }

                menuStrip.Items.Add(permissionGroupMenuItem);
            }

            var helpMenuItem = new ToolStripMenuItem("Help");
            helpMenuItem.Click += HelpMenuItem_Click;
            menuStrip.Items.Add(helpMenuItem);
        }

        private void ViewProductsMenuItem_Click(object? sender, EventArgs e)
        {
            // TODO: Implement logic to show a product management/view form/dialog
            MessageBox.Show("Product View/Management clicked.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ProductCategoriesMenuItem_Click(object? sender, EventArgs e)
        {
            // TODO: Implement logic to show a product category management form/dialog
            MessageBox.Show("Product Categories clicked.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void HelpMenuItem_Click(object? sender, EventArgs e)
        {
            // TODO: Implement logic to show help information
            MessageBox.Show("Help clicked.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }


        private void UpdateDateLabel()
        {
            dateLabel.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }
        private ProductControl CreateProductControl(Product product)
        {
            ProductControl productControl = new();
            productControl.InitializeProduct(product);
            productControl.ProductClicked += (sender, p) =>
            {
                cart.AddItem(new ProductCartItem
                {
                    Product = product,
                    Quantity = 1
                });
            };
            return productControl;
        }

        private ProductCategoryControl CreateCategoryControl(ProductCategory category)
        {
            ProductCategoryControl categoryControl = new();
            categoryControl.InitializeCategory(category);
            categoryControl.ProductCategoryClicked += (sender, c) =>
            {
                _productService.SelectedCategory = category;
            };
            categoryControl.IsSelected = category?.Id == _productService.SelectedCategory?.Id;
            return categoryControl;
        }

        private void PayButton_Click(object sender, EventArgs e)
        {
            ReceiptPrinter printer = new();
            printer.PrintReceipt(cart.CartItems.ToList());
            cart.CartItems.Clear();
        }

        private void BarcodeTextBox_KeyPress(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string barcode = textBoxBarcode.Text;
                Product product = _productService.GetProductByBarcode(barcode);
                if (product != null)
                {
                    cart.AddItem(new ProductCartItem
                    {
                        Product = product,
                        Quantity = 1
                    });
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
                /// Header or empty cell clicked
                return;
            }

            string columnName = cartDataGrid.Columns[e.ColumnIndex].Name;
            ICartItem cartItem = cart.CartItems.ElementAt(e.RowIndex);

            if (columnName == "Decrement")
            {
                cart.UpdateItemQuantity(cartItem, -1);
            }
            else if (columnName == "Increment")
            {
                cart.UpdateItemQuantity(cartItem, 1);
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }
    }
}
