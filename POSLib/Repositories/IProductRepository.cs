using POSLib.Models;

namespace POSLib.Repositories
{
    public interface IProductRepository
    {
        List<Product> GetAll();
        List<Product> GetAllByCategory(string category);
        Product GetByBarcode(string barcode);
        void Add(Product product);
        void Update(Product product);
        void Delete(int id);
    }
}
