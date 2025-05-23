using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using POSLib.Models;

namespace POSLibTest
{
    [TestClass]
    public class PermissionExtensionsTests
    {
        [TestMethod]
        public void GetPermissionGroup_ReturnsCorrectGroup()
        {
            Assert.AreEqual("Products", Permission.ViewProducts.GetPermissionGroup());
            Assert.AreEqual("Products", Permission.AddProducts.GetPermissionGroup());
            Assert.AreEqual("Products", Permission.EditProducts.GetPermissionGroup());
            Assert.AreEqual("Products", Permission.DeleteProducts.GetPermissionGroup());

            Assert.AreEqual("Categories", Permission.ViewCategories.GetPermissionGroup());
            Assert.AreEqual("Categories", Permission.AddCategories.GetPermissionGroup());
            Assert.AreEqual("Categories", Permission.EditCategories.GetPermissionGroup());
            Assert.AreEqual("Categories", Permission.DeleteCategories.GetPermissionGroup());

            Assert.AreEqual("Help", Permission.ViewHelp.GetPermissionGroup());
        }

        [TestMethod]
        public void GetPermissionDescription_ReturnsCorrectDescription()
        {
            Assert.AreEqual("View Products", Permission.ViewProducts.GetPermissionDescription());
            Assert.AreEqual("Add Products", Permission.AddProducts.GetPermissionDescription());
            Assert.AreEqual("Edit Products", Permission.EditProducts.GetPermissionDescription());
            Assert.AreEqual(
                "Delete Products",
                Permission.DeleteProducts.GetPermissionDescription()
            );

            Assert.AreEqual(
                "View Categories",
                Permission.ViewCategories.GetPermissionDescription()
            );
            Assert.AreEqual("Add Categories", Permission.AddCategories.GetPermissionDescription());
            Assert.AreEqual(
                "Edit Categories",
                Permission.EditCategories.GetPermissionDescription()
            );
            Assert.AreEqual(
                "Delete Categories",
                Permission.DeleteCategories.GetPermissionDescription()
            );

            Assert.AreEqual("View Help", Permission.ViewHelp.GetPermissionDescription());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void GetPermissionGroup_ThrowsOnInvalidEnum()
        {
            ((Permission)999).GetPermissionGroup();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void GetPermissionDescription_ThrowsOnInvalidEnum()
        {
            ((Permission)999).GetPermissionDescription();
        }
    }
}
