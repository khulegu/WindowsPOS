using System.ComponentModel;
using System.Diagnostics;
using POSLib.Models;
using POSLib.Repositories;

namespace POSLib.Controllers
{
    public class ProductController(IProductRepository productRepo, User user)
        : INotifyPropertyChanged
    {
        private readonly IProductRepository _productRepo = productRepo;
        private readonly User _user = user;
        private ProductCategory? _selectedCategory;

        public event PropertyChangedEventHandler? PropertyChanged;
        public BindingList<Product> ProductsFiltered { get; } = [];
        public BindingList<ProductCategory> Categories { get; } = [];

        /// <summary>
        /// Бараа болон ангиллыг эхлүүлнэ.
        /// </summary>
        public void InitializeProducts()
        {
            ProductsFiltered.Clear();
            Categories.Clear();
            foreach (ProductCategory category in _productRepo.GetAllCategories())
            {
                Categories.Add(category);
            }
            FillProductsByCategory(_selectedCategory);
        }

        /// <summary>
        /// Сонгогдсон ангилал.
        /// </summary>
        public ProductCategory? SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                if (_selectedCategory != value)
                {
                    _selectedCategory = value;
                    ProductsFiltered.Clear();
                    if (_selectedCategory != null)
                    {
                        FillProductsByCategory(_selectedCategory);
                    }
                    OnPropertyChanged(nameof(SelectedCategory));
                }
            }
        }

        /// <summary>
        /// Баркодоор бараа авах.
        /// </summary>
        /// <param name="barcode">Барааны баркод.</param>
        /// <returns>Олдвол бараа, үгүй бол null.</returns>
        public Product? GetProductByBarcode(string barcode) => _productRepo.GetByBarcode(barcode);

        /// <summary>
        /// Бараа нэмэх.
        /// </summary>
        /// <param name="product">Нэмэх бараа.</param>
        public void AddProduct(Product product)
        {
            if (_user.Permissions.Contains(Permission.AddProducts) == false)
                throw new UnauthorizedAccessException("Танд бараа нэмэх эрх байхгүй.");
            _productRepo.Add(product);
        }

        /// <summary>
        /// Барааг ангилалын дагуу дүүргэх.
        /// </summary>
        /// <param name="category">Барааг дүүргэх ангилал.</param>
        private void FillProductsByCategory(ProductCategory? category)
        {
            ProductsFiltered.Clear();
            if (category == null)
            {
                return;
            }
            foreach (Product product in _productRepo.GetAllByCategory(category.Id))
            {
                ProductsFiltered.Add(product);
            }
        }

        protected virtual void OnPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
