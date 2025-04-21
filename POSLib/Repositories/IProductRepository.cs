using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POSLib.Models;

namespace POSLib.Repositories
{
    public interface IProductRepository
    {
        List<Product> GetAll();
        Product GetByBarcode(string barcode);
        void Add(Product product);
        void Update(Product product);
        void Delete(int id);
    }
}
