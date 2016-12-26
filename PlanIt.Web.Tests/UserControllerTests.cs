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

namespace PlanIt.Web.Tests
{
    [TestClass]
    public class UserControllerTests
    {
        private Mock<IRepository> _mockRepository;
        private IUserService _userService;
        private IProfileService _profileService;
        private UserController _userController;
        private IList<User> _users;


        [TestInitialize]
        public void Initialize()
        {
            _mockRepository = new Mock<IRepository>();
            _profileService = new ProfileService(_mockRepository.Object);
            _userService = new UserService(_mockRepository.Object);

            //mock for HttpContext.User.Identity.IsAuthenticated
            var mocks = new MockRepository(MockBehavior.Default);
            Mock<IPrincipal> mockPrincipal = mocks.Create<IPrincipal>();
            mockPrincipal.SetupGet(p => p.Identity.Name).Returns("testUser");
            mockPrincipal.Setup(p => p.IsInRole("User")).Returns(true);
            var mockContext = new Mock<ControllerContext>();
            mockContext.SetupGet(p => p.HttpContext.User).Returns(mockPrincipal.Object);
            mockContext.SetupGet(p => p.HttpContext.User.Identity.IsAuthenticated).Returns(true);
            _userController = new UserController(_userService, _profileService) { ControllerContext = mockContext.Object };


            _users = new List<User>
            {
                new User {Id = 1, Email = "123@gmil.com", IsEmailConfirmed = true, Password = "asdasdsa", ProfileId = 1 },
                new User {Id = 2, Email = "124@gmil.com", IsEmailConfirmed = true, Password = "fdsfs", ProfileId = 2 },
                new User {Id = 3, Email = "125@gmil.com", IsEmailConfirmed = true, Password = "asdnytasdsa", ProfileId = 3 },
            };
            HttpContext.Current = new HttpContext(new HttpRequest(null, "https://localhost:44310/", null), new HttpResponse(null));
        }

        [TestMethod]
        public void IndexTest()
        {
            ActionResult result = _userController.Index();
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void SingUpGetTest()
        {
            ActionResult result = _userController.SignUp();
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void SingUpPostTest()
        {

        }

        [TestMethod]
        public void LogInGetAuthenticatedTest()
        {
            var actionResult = _userController.LogIn() as ActionResult;
            Assert.IsInstanceOfType(actionResult, typeof(RedirectToRouteResult));
            RedirectToRouteResult routeResult = actionResult as RedirectToRouteResult;
            Assert.AreEqual(routeResult.RouteValues["action"], "Index");
            Assert.AreEqual(routeResult.RouteValues["controller"], "Plan");
        }

        [TestMethod]
        public void LogInGetNotAuthenticatedTest()
        {
            var mocks = new MockRepository(MockBehavior.Default);
            Mock<IPrincipal> mockPrincipal = mocks.Create<IPrincipal>();
            mockPrincipal.SetupGet(p => p.Identity.Name).Returns("testUser");
            mockPrincipal.Setup(p => p.IsInRole("User")).Returns(true);
            var mockContext = new Mock<ControllerContext>();
            mockContext.SetupGet(p => p.HttpContext.User).Returns(mockPrincipal.Object);
            mockContext.SetupGet(p => p.HttpContext.User.Identity.IsAuthenticated).Returns(false);
            _userController = new UserController(_userService, _profileService) { ControllerContext = mockContext.Object };

            ActionResult result = _userController.LogIn();
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void LogInPostTest()
        {

        }

        [TestMethod]
        public void EditProfileGetAuthenticatedTest()
        {    
            var actionResult = _userController.EditProfile() as ActionResult;
            Assert.IsInstanceOfType(actionResult, typeof(RedirectToRouteResult));
            RedirectToRouteResult routeResult = actionResult as RedirectToRouteResult;
            Assert.AreEqual(routeResult.RouteValues["action"], "Index");
            Assert.AreEqual(routeResult.RouteValues["controller"], "Home");
        }

        [TestMethod]
        public void EditProfileGetNotAuthenticatedTest()
        {
        }

        [TestMethod]
        public void EditProfilePostAuthenticatedTest()
        {
        }

        [TestMethod]
        public void EditProfilePostNotAuthenticatedTest()
        {
        }

    }
}
