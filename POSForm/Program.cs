namespace POSForm
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.SetCompatibleTextRenderingDefault(false);

            var connStr = "Data Source=pos.db";
            var userRepo = new UserRepository(connStr);
            var authService = new AuthService(userRepo);

            LoginForm loginForm = new LoginForm(authService);
            Application.Run(loginForm);

            if (loginForm.DialogResult == DialogResult.OK)
            {
                Application.Run(new MainForm(loginForm.User));
            }
        }
    }
}
