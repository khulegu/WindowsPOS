using System.ComponentModel;
using System.Data;
using Microsoft.Data.Sqlite;
using POSForm.Controls;
using POSLib.Controllers;
using POSLib.Models;
using POSLib.Repositories;

namespace POSForm
{
    public partial class MainForm : Form
    {
        private static readonly string connStr = "Data Source=pos.db";
        private static readonly ProductRepository productRepo = new(connStr);

        private readonly User _user;
        private readonly ProductController _productService;
        private readonly Cart cart = new();
        private ProductControl? _selectedProductControl;

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
                        productsLayout.Controls.Add(
                            CreateProductControl(_productService.ProductsFiltered[e.NewIndex])
                        );
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
                        categoriesLayout.Controls.Add(
                            CreateCategoryControl(_productService.Categories[e.NewIndex])
                        );
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
                        categoryControl.IsSelected =
                            categoryControl.Category?.Id == _productService.SelectedCategory?.Id;
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
            var permissionGroups = permissions
                .Select(p => p.GetPermissionGroup())
                .Distinct()
                .ToList();

            foreach (var permissionGroup in permissionGroups)
            {
                var permissionGroupMenuItem = new ToolStripMenuItem(permissionGroup);

                foreach (var permission in permissions)
                {
                    if (
                        permission.GetPermissionGroup() == permissionGroup
                        && !permission.GetPermissionDescription().StartsWith("_")
                    )
                    {
                        var permissionMenuItem = new ToolStripMenuItem(
                            permission.GetPermissionDescription()
                        );
                        permissionMenuItem.Click += (s, e) =>
                        {
                            OnPermissionMenuItemClick(permission);
                        };
                        permissionGroupMenuItem.DropDownItems.Add(permissionMenuItem);
                    }
                }

                menuStrip.Items.Add(permissionGroupMenuItem);
            }
        }

        private void OnPermissionMenuItemClick(Permission permission)
        {
            if (permission == Permission.ViewProducts)
            {
                return;
            }
            else if (permission == Permission.AddProducts)
            {
                try
                {
                    using var addForm = new AddProductForm(_productService.Categories.ToList());
                    if (addForm.ShowDialog() == DialogResult.OK && addForm.NewProduct != null)
                    {
                        _productService.AddProduct(addForm.NewProduct);
                    }
                }
                catch (UnauthorizedAccessException ex)
                {
                    MessageBox.Show(
                        ex.Message,
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                }
                catch (DuplicateNameException ex)
                {
                    MessageBox.Show(
                        ex.Message,
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                }
            }
            else if (permission == Permission.EditProducts)
            {
                if (_selectedProductControl == null)
                {
                    MessageBox.Show(
                        "Please select a product to edit.",
                        "Information",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                    return;
                }

                try
                {
                    using var editForm = new EditProductForm(
                        _selectedProductControl.Product!,
                        _productService.Categories.ToList()
                    );
                    if (editForm.ShowDialog() == DialogResult.OK && editForm.EditedProduct != null)
                    {
                        _productService.EditProduct(editForm.EditedProduct);
                    }
                }
                catch (UnauthorizedAccessException ex)
                {
                    MessageBox.Show(
                        ex.Message,
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                }
                catch (DuplicateNameException ex)
                {
                    MessageBox.Show(
                        ex.Message,
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                }
            }
            else if (permission == Permission.DeleteProducts)
            {
                if (_selectedProductControl == null)
                {
                    MessageBox.Show(
                        "Please select a product to delete.",
                        "Information",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );
                    return;
                }

                try
                {
                    _productService.DeleteProduct(_selectedProductControl.Product!.Id);
                    _selectedProductControl = null;
                }
                catch (UnauthorizedAccessException ex)
                {
                    MessageBox.Show(
                        ex.Message,
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                }
            }
            else if (permission == Permission.ViewCategories)
            {
                MessageBox.Show("View Categories clicked.");
            }
            else if (permission == Permission.AddCategories)
            {
                MessageBox.Show("Add Categories clicked.");
            }
            else if (permission == Permission.EditCategories)
            {
                MessageBox.Show("Edit Categories clicked.");
            }
            else if (permission == Permission.DeleteCategories)
            {
                _productService.DeleteCategory(_productService.SelectedCategory!.Id);
            }
            else if (permission == Permission.ViewHelp)
            {
                MessageBox.Show("View Help clicked.");
            }
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
                if (_selectedProductControl != null)
                {
                    _selectedProductControl.IsSelected = false;
                }
                _selectedProductControl = productControl;
                _selectedProductControl.IsSelected = true;
                cart.AddItem(new ProductCartItem { Product = product, Quantity = 1 });
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
            if (cart.CartItems.Count == 0)
            {
                MessageBox.Show("Сагс хоосон байна.");
                return;
            }

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
                    cart.AddItem(new ProductCartItem { Product = product, Quantity = 1 });
                    textBoxBarcode.Clear();
                }
                else
                {
                    MessageBox.Show("Бүтээгдэхүүн олдсонгүй");
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

        private void MainForm_Load(object sender, EventArgs e) { }
    }
}
