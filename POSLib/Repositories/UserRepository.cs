using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using POSLib.Models;

namespace POSLib.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly string _connStr;
        public UserRepository(string connStr) => _connStr = connStr;

        public User GetUser(string username, string password)
        {
            using var conn = new SQLiteConnection(_connStr);
            conn.Open();
            var cmd = new SQLiteCommand("SELECT * FROM Users WHERE username=@u AND password=@p", conn);
            cmd.Parameters.AddWithValue("@u", username);
            cmd.Parameters.AddWithValue("@p", password);
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new User
                {
                    Id = Convert.ToInt32(reader["id"]),
                    Username = reader["username"].ToString(),
                    Password = reader["password"].ToString(),
                    Role = reader["role"].ToString()
                };
            }
            return null;
        }
    }

}
