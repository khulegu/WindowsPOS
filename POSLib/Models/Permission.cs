namespace POSLib.Models
{
    /// <summary>
    /// The permission of the user
    /// </summary>
    public enum Permission
    {
        /// <summary>
        /// View products
        /// </summary>
        ViewProducts,

        /// <summary>
        /// Add products
        /// </summary>
        AddProducts,

        /// <summary>
        /// Edit products
        /// </summary>
        EditProducts,

        /// <summary>
        /// Delete products
        /// </summary>
        DeleteProducts,

        /// <summary>
        /// Edit categories
        /// </summary>
        EditCategories,
        ViewCategories,
        AddCategories,
        DeleteCategories,
        ViewHelp,
    }

    public static class PermissionExtensions
    {
        public static string GetPermissionGroup(this Permission permission)
        {
            return permission switch
            {
                Permission.ViewProducts => "Бараа",
                Permission.AddProducts => "Бараа",
                Permission.EditProducts => "Бараа",
                Permission.DeleteProducts => "Бараа",
                Permission.EditCategories => "Ангилал",
                Permission.ViewCategories => "Ангилал",
                Permission.AddCategories => "Ангилал",
                Permission.DeleteCategories => "Ангилал",
                Permission.ViewHelp => "Тусламж",
                _ => throw new ArgumentOutOfRangeException(nameof(permission), permission, null),
            };
        }

        public static string GetPermissionDescription(this Permission permission)
        {
            return permission switch
            {
                Permission.ViewProducts => "_Бараа харах",
                Permission.AddProducts => "Бараа нэмэх",
                Permission.EditProducts => "Бараа засах",
                Permission.DeleteProducts => "Бараа устгах",
                Permission.EditCategories => "_Ангилал засах",
                Permission.ViewCategories => "_Ангилал харах",
                Permission.AddCategories => "_Ангилал нэмэх",
                Permission.DeleteCategories => "Ангилал устгах",
                Permission.ViewHelp => "Тусламж",
                _ => throw new ArgumentOutOfRangeException(nameof(permission), permission, null),
            };
        }
    }
}
