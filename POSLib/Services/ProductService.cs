using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POSLib.Models;
using POSLib.Repositories;

namespace POSLib.Services
{
    public class ProductService
    {
        private readonly IProductRepository _productRepo;
        public ProductService(IProductRepository productRepo) => _productRepo = productRepo;

        public List<Product> GetAllProducts() => _productRepo.GetAll();
        public Product GetProductByBarcode(string barcode) => _productRepo.GetByBarcode(barcode);

        public void AddProduct(Product product) => _productRepo.Add(product);
        public void UpdateProduct(Product product) => _productRepo.Update(product);
        public void DeleteProduct(int id) => _productRepo.Delete(id);
    }

}
