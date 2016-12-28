using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PlanIt.Web.Controllers;
using Moq;
using PlanIt.Repositories.Abstract;
using PlanIt.Services.Abstract;
using System.Collections.Generic;
using PlanIt.Entities;
using PlanIt.Services.Concrete;
using PlanIt.Web.Hubs;
using System.Web.Mvc;
using System.Web.Security;
using System.Web;
using System.IO;
using System.Security.Principal;
using PlanIt.Web.Models;

namespace PlanIt.Web.Tests
{
    [TestClass]
    public class HomeControllerTests
    {
        private HomeController _homeController;
        private Mock<IUserService> _mockUserService;

        [TestInitialize]
        public void Initialize()
        {
            _mockUserService = new Mock<IUserService>();

            var mocks = new MockRepository(MockBehavior.Default);
            Mock<IPrincipal> mockPrincipal = mocks.Create<IPrincipal>();
            mockPrincipal.SetupGet(p => p.Identity.Name).Returns("testUser");
            mockPrincipal.Setup(p => p.IsInRole("User")).Returns(true);

            var mockContext = new Mock<ControllerContext>();
            mockContext.SetupGet(p => p.HttpContext.User).Returns(mockPrincipal.Object);
            mockContext.SetupGet(p => p.HttpContext.User.Identity.IsAuthenticated).Returns(true);
            _homeController = new HomeController(_mockUserService.Object) { ControllerContext = mockContext.Object };

        }

        [TestMethod]
        public void AboutTest()
        {
            ActionResult result = _homeController.About();
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }
    }
}
