using POSLib.Models;

namespace POSLib.Repositories
{
    public interface IProductRepository
    {
        /// <summary>
        /// Get all products
        /// </summary>
        /// <returns>A list of all products</returns>
        List<Product> GetAll();

        /// <summary>
        /// Get all products by category
        /// </summary>
        /// <param name="categoryId">The id of the category</param>
        /// <returns>A list of all products in the category</returns>
        List<Product> GetAllByCategory(int categoryId);

        /// <summary>
        /// Get a product by barcode
        /// </summary>
        /// <param name="barcode">The barcode of the product</param>
        /// <returns>The product if found, otherwise null</returns>
        Product? GetByBarcode(string barcode);

        /// <summary>
        /// Get all categories
        /// </summary>
        /// <returns>A list of all categories</returns>
        List<ProductCategory> GetAllCategories();

        /// <summary>
        /// Add a category
        /// </summary>
        /// <param name="category">The category to add</param>
        void AddCategory(ProductCategory category);

        /// <summary>
        /// Delete a category
        /// </summary>
        /// <param name="categoryId">The id of the category to delete</param>
        void DeleteCategory(int categoryId);

        /// <summary>
        /// Add a product
        /// </summary>
        void Add(Product product);

        /// <summary>
        /// Delete a product
        /// </summary>
        /// <param name="productId">The id of the product to delete</param>
        void DeleteProduct(int productId);

        /// <summary>
        /// Update a product
        /// </summary>
        /// <param name="product">The product to update</param>
        void UpdateProduct(Product product);
    }
}
