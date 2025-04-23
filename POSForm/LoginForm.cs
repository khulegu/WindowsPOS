using System;
using System.Windows.Forms;
using POSLib.Services;
using POSLib.Models;
using POSLib.Repositories;

namespace POSForm
{
    public partial class LoginForm : Form
    {
        private readonly AuthService _authService;
        public User? User { get; private set; }

        public LoginForm(AuthService authService)
        {
            _authService = authService;
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, EventArgs e)
        {
            string username = usernameTextBox.Text;
            string password = passwordTextBox.Text;

            User? user = _authService.Login(username, password);

            if (user != null)
            {
                User = user;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Login failed. Please check your username and password.");
            }
        }
    }
}
