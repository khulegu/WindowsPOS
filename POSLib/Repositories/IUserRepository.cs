using POSLib.Models;

namespace POSLib.Repositories
{
    public interface IUserRepository
    {
        User GetUser(string username, string password);
    }
}
