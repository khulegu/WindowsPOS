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
            Assert.AreEqual("Бараа", Permission.ViewProducts.GetPermissionGroup());
            Assert.AreEqual("Бараа", Permission.AddProducts.GetPermissionGroup());
            Assert.AreEqual("Бараа", Permission.EditProducts.GetPermissionGroup());
            Assert.AreEqual("Бараа", Permission.DeleteProducts.GetPermissionGroup());

            Assert.AreEqual("Ангилал", Permission.ViewCategories.GetPermissionGroup());
            Assert.AreEqual("Ангилал", Permission.AddCategories.GetPermissionGroup());
            Assert.AreEqual("Ангилал", Permission.EditCategories.GetPermissionGroup());
            Assert.AreEqual("Ангилал", Permission.DeleteCategories.GetPermissionGroup());

            Assert.AreEqual("Тусламж", Permission.ViewHelp.GetPermissionGroup());
        }

        [TestMethod]
        public void GetPermissionDescription_ReturnsCorrectDescription()
        {
            Assert.AreEqual("_Бараа харах", Permission.ViewProducts.GetPermissionDescription());
            Assert.AreEqual("_Бараа нэмэх", Permission.AddProducts.GetPermissionDescription());
            Assert.AreEqual("Бараа засах", Permission.EditProducts.GetPermissionDescription());
            Assert.AreEqual("Бараа устгах", Permission.DeleteProducts.GetPermissionDescription());

            Assert.AreEqual("_Ангилал харах", Permission.ViewCategories.GetPermissionDescription());
            Assert.AreEqual("_Ангилал нэмэх", Permission.AddCategories.GetPermissionDescription());
            Assert.AreEqual("_Ангилал засах", Permission.EditCategories.GetPermissionDescription());
            Assert.AreEqual(
                "Ангилал устгах",
                Permission.DeleteCategories.GetPermissionDescription()
            );

            Assert.AreEqual("Тусламж", Permission.ViewHelp.GetPermissionDescription());
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
