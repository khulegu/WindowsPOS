using POSLib.Exceptions;
using POSLib.Models;
using POSLib.Repositories;
using POSLib.Services;

internal class Program
{
    private static void Main(string[] args)
    {
        var connStr = "Data Source=pos.db";

        DatabaseInitializer.InitializeDatabase(connStr);


        var userRepo = new UserRepository(connStr);
        var productRepo = new ProductRepository(connStr);

        var authService = new AuthService(userRepo);
        var cart = new Cart();

        var user = authService.Login("manager", "1234");

        if (user == null)
        {
            Console.WriteLine("Login failed");
            return;
        }

        Console.WriteLine($"Welcome {user.Username}!");

        var productService = new ProductService(productRepo, user);

        try
        {
            productService.AddProduct(
              new Product
              {
                  Name = "Jam",
                  Price = 5.0,
                  Barcode = "555",
                  Category = new ProductCategory { Id = 1, Name = "Food" },
              }
            );
        }
        catch (ForbiddenException ex)
        {
            Console.WriteLine(ex.Message);
        }
        catch (BarcodeAlreadyExistsException ex)
        {
            Console.WriteLine(ex.Message);
        }

        var prod = productService.GetProductByBarcode("555");

        if (prod == null)
        {
            Console.WriteLine("Product not found");
            return;
        }
        else
        {
            cart.AddItem(new ProductCartItem
            {
                Product = prod,
                Quantity = 1
            });
        }

        var total = cart.Total;
        Console.WriteLine($"Total: {total}");
    }
}