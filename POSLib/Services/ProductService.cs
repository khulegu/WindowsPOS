using POSLib.Exceptions;
using POSLib.Models;
using POSLib.Repositories;

namespace POSLib.Services
{
    public class ProductService
    {
        private readonly IProductRepository _productRepo;
        private readonly User _user;
        public ProductService(IProductRepository productRepo, User user)
        {
            _productRepo = productRepo;
            _user = user;
        }

        public List<Product> GetAllProducts() => _productRepo.GetAll();
        public Product GetProductByBarcode(string barcode) => _productRepo.GetByBarcode(barcode);
        public void AddProduct(Product product)
        {
            if (_user.Role != Role.Manager)
                throw new ForbiddenException("Зөвхөн менежер бараа нэмэж болно.");
            _productRepo.Add(product);
        }

        public void UpdateProduct(Product product)
        {
            if (_user.Role != Role.Manager)
                throw new ForbiddenException("Зөвхөн менежер бараа засаж болно.");
            _productRepo.Update(product);
        }

        public void DeleteProduct(int id)
        {
            if (_user.Role != Role.Manager)
                throw new ForbiddenException("Зөвхөн менежер бараа устгаж болно.");
            _productRepo.Delete(id);
        }
    }
}
