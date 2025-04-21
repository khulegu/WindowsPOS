using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POSLib.Models;
using POSLib.Repositories;

namespace POSLib.Services
{
    public class AuthService
    {
        private readonly IUserRepository _userRepo;
        public AuthService(IUserRepository userRepo) => _userRepo = userRepo;

        public User Login(string username, string password)
        {
            return _userRepo.GetUser(username, password);
        }
    }
}
