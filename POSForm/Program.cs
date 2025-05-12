using POSLib.Repositories;
using POSLib.Controllers;
using POSLib.Services;

namespace POSForm
{
    internal static class Program
    {
        static readonly string connStr = "Data Source=pos.db";



        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.SetCompatibleTextRenderingDefault(false);

            DatabaseInitializer.InitializeDatabase(connStr);

            UserRepository userRepo = new(connStr);
            AuthService authService = new(userRepo);
            ProductRepository productRepo = new(connStr);

            LoginForm loginForm = new(authService);
            Application.Run(loginForm);

            if (loginForm.DialogResult == DialogResult.OK && loginForm.User != null)
            {
                var user = loginForm.User;
                var productService = new ProductController(productRepo, user);
                Application.Run(new MainForm(loginForm.User));
            }
        }
    }
}
