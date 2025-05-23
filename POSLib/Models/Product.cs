namespace POSLib.Models
{
    public class Product
    {
        /// <summary>
        /// The id of the product
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The name of the product
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// The price of the product
        /// </summary>
        public double Price { get; set; }

        /// <summary>
        /// The barcode of the product
        /// </summary>
        public required string Barcode { get; set; }

        /// <summary>
        /// The category of the product
        /// </summary>
        public required ProductCategory Category { get; set; }

        /// <summary>
        /// The image url of the product
        /// </summary>
        public string? ImageUrl { get; set; }
    }
}
