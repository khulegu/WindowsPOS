using POSLib.Models;
using POSLib.Repositories;

namespace POSLib.Services
{
    public class AuthService(IUserRepository userRepo)
    {
        private readonly IUserRepository _userRepo = userRepo;

        public User? Login(string username, string password)
        {
            return _userRepo.GetUser(username, password);
        }
    }
}
