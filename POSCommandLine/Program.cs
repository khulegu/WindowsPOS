using POSLib.Models;
using POSLib.Repositories;
using POSLib.Services;

internal class Program
{
  private static void Main(string[] args)
  {
    // Startup
    var connStr = "Data Source=pos.db";

    DatabaseInitializer.InitializeDatabase(connStr);


    var userRepo = new UserRepository(connStr);
    var productRepo = new ProductRepository(connStr);

    var authService = new AuthService(userRepo);
    var productService = new ProductService(productRepo);
    var cartService = new CartService();

    // Login
    var user = authService.Login("manager", "1234");
    if (user != null && user.Role == "Manager") {
        Console.WriteLine($"Welcome {user.Username}!");
        }

    // Бараа нэмэх
    productService.AddProduct(new Product { Name = "Jam", Price = 5.0, Barcode = "555" });

    // Сагсанд бараа нэмэх
    var prod = productService.GetProductByBarcode("555");
    cartService.AddToCart(prod);

    // Сагсны нийт үнэ
    var total = cartService.GetTotal();
  }
}