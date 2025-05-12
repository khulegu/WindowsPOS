namespace POSLib.Models
{
    public enum Permission
    {
        ViewProducts,
        AddProducts,
        EditProducts,
        DeleteProducts,
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
                Permission.ViewProducts => "Products",
                Permission.AddProducts => "Products",
                Permission.EditProducts => "Products",
                Permission.DeleteProducts => "Products",
                Permission.EditCategories => "Categories",
                Permission.ViewCategories => "Categories",
                Permission.AddCategories => "Categories",
                Permission.DeleteCategories => "Categories",
                Permission.ViewHelp => "Help",
                _ => throw new ArgumentOutOfRangeException(nameof(permission), permission, null)
            };
        }

        public static string GetPermissionDescription(this Permission permission)
        {
            return permission switch
            {
                Permission.ViewProducts => "View Products",
                Permission.AddProducts => "Add Products",
                Permission.EditProducts => "Edit Products",
                Permission.DeleteProducts => "Delete Products",
                Permission.EditCategories => "Edit Categories",
                Permission.ViewCategories => "View Categories",
                Permission.AddCategories => "Add Categories",
                Permission.DeleteCategories => "Delete Categories",
                Permission.ViewHelp => "View Help",
                _ => throw new ArgumentOutOfRangeException(nameof(permission), permission, null)
            };
        }
    }
}