using POSLib.Models;

namespace POSLib.Repositories
{
    public interface IUserRepository
    {
        /// <summary>
        /// Login a user
        /// </summary>
        /// <param name="username">The username of the user</param>
        /// <param name="password">The password of the user</param>
        /// <returns>The user if found, otherwise null</returns>
        User? Login(string username, string password);
    }
}
