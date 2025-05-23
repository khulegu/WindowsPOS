namespace POSLib.Models
{
    public class User
    {
        /// <summary>
        /// The id of the user
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The username of the user
        /// </summary>
        public required string Username { get; set; }

        /// <summary>
        /// The password of the user
        /// </summary>
        public required string Password { get; set; }

        /// <summary>
        /// The role of the user
        /// </summary>
        public required Role Role { get; set; }

        /// <summary>
        /// The permissions of the user
        /// </summary>
        public required List<Permission> Permissions { get; set; } = [];
    }
}
