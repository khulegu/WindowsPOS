using POSLib.Models;
using POSLib.Repositories;

namespace POSLib.Services
{
    public class AuthService(IUserRepository userRepo)
    {
        private readonly IUserRepository _userRepo = userRepo;

        public User Login(string username, string password)
        {
            User? user =  _userRepo.GetUser(username, password);
            if (user == null)
            {
                throw new UnauthorizedAccessException("Invalid username or password.");
            }
            return user;
        }
    }
}
