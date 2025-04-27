using System;
using System.Windows.Forms;
using POSLib.Services;
using POSLib.Models;
using POSLib.Repositories;

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
                var productService = new ProductService(productRepo, user);
                Application.Run(new MainForm(loginForm.User));
            }
        }
    }
}
