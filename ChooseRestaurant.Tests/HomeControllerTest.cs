using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ChooseRestaurant.Controllers;
using System.Web.Mvc;

namespace ChooseRestaurant.Tests
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        public void HomeController_Index()
        {
            HomeController controller = new HomeController();
            ViewResult resultat = (ViewResult)controller.Index();
            Assert.AreEqual(string.Empty, resultat.ViewName);
        }
    }
}
