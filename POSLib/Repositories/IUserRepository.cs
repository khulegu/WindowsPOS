using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POSLib.Models;

namespace POSLib.Repositories
{
    public interface IUserRepository
    {
        User GetUser(string username, string password);
    }
}
