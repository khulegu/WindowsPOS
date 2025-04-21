using POSLib.Models;

namespace POSLib.Repositories
{
    public interface IProductRepository
    {
        List<Product> GetAll();
        List<Product> GetAllByCategory(int categoryId);
        Product? GetByBarcode(string barcode);

        void AddCategory(ProductCategory category);
        void UpdateCategory(ProductCategory category);
        void DeleteCategory(int id);
        void Add(Product product);
        void Update(Product product);
        void Delete(int id);
    }
}
