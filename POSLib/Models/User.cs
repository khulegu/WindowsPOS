namespace POSLib.Models
{
    public class User
    {
        public int Id { get; set; }
        public required string Username { get; set; }
        public required string Password { get; set; }
        public required Role Role { get; set; }
        public required List<Permission> Permissions { get; set; } = [];
    }
}
