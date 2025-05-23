namespace POSLib.Models
{
    public class ProductCategory
    {
        /// <summary>
        /// The id of the category
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The name of the category
        /// </summary>
        public required string Name { get; set; }
    }
}
