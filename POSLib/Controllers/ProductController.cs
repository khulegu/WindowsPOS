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
        /// Initialize the products and categories
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
        /// The selected category
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
        /// Get a product by barcode
        /// </summary>
        /// <param name="barcode">The barcode of the product</param>
        /// <returns>The product if found, otherwise null</returns>
        public Product? GetProductByBarcode(string barcode) => _productRepo.GetByBarcode(barcode);

        /// <summary>
        /// Add a product
        /// </summary>
        /// <param name="product">The product to add</param>
        public void AddProduct(Product product)
        {
            if (_user.Permissions.Contains(Permission.AddProducts) == false)
                throw new UnauthorizedAccessException("Танд бараа нэмэх эрх байхгүй.");
            _productRepo.Add(product);
        }

        /// <summary>
        /// Fill the products by category
        /// </summary>
        /// <param name="category">The category to fill the products by</param>
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

        /// <summary>
        /// Notify the UI that a property has changed
        /// </summary>
        /// <param name="property">The property that has changed</param>
        protected virtual void OnPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
