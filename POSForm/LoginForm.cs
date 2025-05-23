using POSLib.Models;
using POSLib.Repositories;

namespace POSForm
{
    public partial class LoginForm : Form
    {
        private readonly IUserRepository _userRepo;
        public User? User { get; private set; }

        public LoginForm(IUserRepository userRepo)
        {
            _userRepo = userRepo;
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, EventArgs e)
        {
            string username = usernameTextBox.Text;
            string password = passwordTextBox.Text;

            User? user = _userRepo.Login(username, password);

            if (user != null)
            {
                User = user;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Нэвтрэх нэр нууц үг буруу байна.");
            }
        }
    }
}
