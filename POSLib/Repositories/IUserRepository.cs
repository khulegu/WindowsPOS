using POSLib.Models;

namespace POSLib.Repositories
{
    public interface IUserRepository
    {
        User? Login(string username, string password);
    }
}
