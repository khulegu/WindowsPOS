namespace POSLib.Models
{
    public class Product
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public double Price { get; set; }
        public required string Barcode { get; set; }
        public required ProductCategory Category { get; set; }
        public string? ImageUrl { get; set; }
    }
}
