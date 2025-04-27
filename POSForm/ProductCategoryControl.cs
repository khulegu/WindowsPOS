using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using POSLib.Models;

namespace POSForm.Controls
{
    public partial class ProductCategoryControl : UserControl
    {
        private ProductCategory? _category;
        private bool _isSelected = false;

        public ProductCategory? Category => _category;
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
        public ProductCategoryControl()
        {
            InitializeComponent();
        }
        public void InitializeCategory(ProductCategory category)
        {
            this.BackColor = SystemColors.Control;
            _category = category;
            nameLabel.Text = category.Name;
            this.IsSelected = false;
            nameLabel.Click += ProductCategoryPanel_Click;
            this.Click += ProductCategoryPanel_Click;
            this.DoubleClick += ProductCategoryPanel_Click;
            nameLabel.DoubleClick += ProductCategoryPanel_Click;
        }

        public event EventHandler<ProductCategory>? ProductCategoryClicked;
        private void ProductCategoryPanel_Click(object? sender, EventArgs e)
        {
            ProductCategoryClicked?.Invoke(this, _category!);
        }
    }
}
