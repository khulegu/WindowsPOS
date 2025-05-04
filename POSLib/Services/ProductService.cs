using System.ComponentModel;
using System.Diagnostics;
using POSLib.Exceptions;
using POSLib.Models;
using POSLib.Repositories;

namespace POSLib.Services
{
    public class ProductService(IProductRepository productRepo, User user) : INotifyPropertyChanged
    {
        private readonly IProductRepository _productRepo = productRepo;
        private readonly User _user = user;

        public event PropertyChangedEventHandler? PropertyChanged;

        public BindingList<Product> ProductsFiltered { get; } = [];
        public BindingList<ProductCategory> Categories { get; } = [];

        private ProductCategory? _selectedCategory;

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

        public Product? GetProductByBarcode(string barcode) => _productRepo.GetByBarcode(barcode);

        public void AddProduct(Product product)
        {
            if (_user.Permissions.Contains(Permission.AddProducts) == false)
                throw new ForbiddenException("Танд бараа нэмэх эрх байхгүй.");
            _productRepo.Add(product);
        }

        public void UpdateProduct(Product product)
        {
            if (_user.Permissions.Contains(Permission.EditProducts) == false)
                throw new ForbiddenException("Танд бараа засах эрх байхгүй.");
            _productRepo.Update(product);
        }

        public void DeleteProduct(int id)
        {
            if (_user.Role != Role.Manager)
                throw new ForbiddenException("Танд бараа устгах эрх байхгүй.");
            _productRepo.Delete(id);
        }

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
