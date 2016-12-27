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
    public class UserControllerTests
    {
        private UserController _userController;
        private IList<User> _users;
        private IList<Profile> _profiles;
        private Mock<IUserService> _mockUserService;
        private Mock<IProfileService> _mockProfileService;

        [TestInitialize]
        public void Initialize()
        {
            _mockUserService = new Mock<IUserService>();
            _mockProfileService = new Mock<IProfileService>();

            //mock for HttpContext.User.Identity.IsAuthenticated
            var mocks = new MockRepository(MockBehavior.Default);
            Mock<IPrincipal> mockPrincipal = mocks.Create<IPrincipal>();
            mockPrincipal.SetupGet(p => p.Identity.Name).Returns("testUser");
            mockPrincipal.Setup(p => p.IsInRole("User")).Returns(true);
            var mockContext = new Mock<ControllerContext>();
            mockContext.SetupGet(p => p.HttpContext.User).Returns(mockPrincipal.Object);
            mockContext.SetupGet(p => p.HttpContext.User.Identity.IsAuthenticated).Returns(true);
            _userController = new UserController(_mockUserService.Object, _mockProfileService.Object) { ControllerContext = mockContext.Object };


            _users = new List<User>
            {
                new User {Id = 1, Email = "123@gmil.com", IsEmailConfirmed = true, Password = "asdasdsa", ProfileId = 1 },
                new User {Id = 2, Email = "124@gmil.com", IsEmailConfirmed = true, Password = "65e84be33532fb784c48129675f9eff3a682b27168c0ea744b2cf58ee02337c5", ProfileId = 2 },
                new User {Id = 3, Email = "125@gmil.com", IsEmailConfirmed = true, Password = "asdnytasdsa", ProfileId = 3 },
            };

            _profiles = new List<Profile>
            {
                new Profile {Id = 1, FirstName = "User1_First", LastName = "User1_Last", Phone = "380502902591"},
                new Profile {Id = 2, FirstName = "User2_First", LastName = "User2_Last", Phone = "380502902592"},
                new Profile {Id = 3, FirstName = "User3_First", LastName = "User3_Last", Phone = "380502902593"},
                new Profile {Id = 4, FirstName = "User4_First", LastName = "User4_Last", Phone = "380502902594"},
                new Profile {Id = 5, FirstName = "User5_First", LastName = "User5_Last", Phone = "380502902595"}
            };
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
        public void SingUpPostFoundUserTest()
        {
            SignUpDataViewModel model = new SignUpDataViewModel()
            {
                Email = _users[0].Email,
                ConfirmPassword = _users[0].Password,
                Password = _users[0].Password
            };

            _mockUserService.Setup(ser => ser.GetUserByEmail(It.IsAny<string>())).Returns(_users[0]);

            ActionResult result = _userController.SignUp(model);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void SingUpPostNotFoundUserTest()
        {
            SignUpDataViewModel model = new SignUpDataViewModel()
            {
                Email = _users[0].Email,
                ConfirmPassword = _users[0].Password,
                Password = _users[0].Password
            };

            _mockUserService.Setup(ser => ser.GetUserByEmail(It.IsAny<string>())).Returns((User)null);
            _mockUserService.Setup(ser => ser.AddUser(It.IsAny<User>())).Returns(_users[0]);

            ActionResult actionResult = _userController.SignUp(model);
            Assert.IsInstanceOfType(actionResult, typeof(RedirectToRouteResult));
            RedirectToRouteResult routeResult = actionResult as RedirectToRouteResult;
            Assert.AreEqual(routeResult.RouteValues["action"], "Index");
            Assert.AreEqual(routeResult.RouteValues["controller"], "Home");
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
            _userController = new UserController(_mockUserService.Object, _mockProfileService.Object) { ControllerContext = mockContext.Object };

            ActionResult result = _userController.LogIn();
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        //[TestMethod]
        //public void LogInPostValidTest()
        //{
        //    LogInDataViewModel model = new LogInDataViewModel()
        //    {
        //        Email = _users[1].Email,
        //        Password = "qwerty"
        //    };
        //    _mockUserService.Setup(ser => ser.GetUserByEmail(It.IsAny<string>())).Returns(_users[1]);
        //    var actionResult = _userController.LogIn(model) as ActionResult;
        //    Assert.IsInstanceOfType(actionResult, typeof(RedirectToRouteResult));
        //    RedirectToRouteResult routeResult = actionResult as RedirectToRouteResult;
        //    Assert.AreEqual(routeResult.RouteValues["action"], "Index");
        //    Assert.AreEqual(routeResult.RouteValues["controller"], "Home");
        //}

        [TestMethod]
        public void LogInPostNotValidTest()
        {
            LogInDataViewModel model = new LogInDataViewModel()
            {
                Email = _users[0].Email,
                Password = _users[0].Password
            };
            _mockUserService.Setup(ser => ser.GetUserByEmail(It.IsAny<string>())).Returns(_users[0]);
            ActionResult result = _userController.LogIn(model);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void EditProfileGetAuthenticatedTest()
        {
            
            _mockUserService.Setup(ser => ser.GetUserByEmail(It.IsAny<string>())).Returns(_users[0]);
            _mockProfileService.Setup(ser => ser.GetProfileById(It.IsAny<int>())).Returns(_profiles[0]);

            ActionResult result = _userController.EditProfile();
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void EditProfileGetNotAuthenticatedTest()
        {
            var mocks = new MockRepository(MockBehavior.Default);
            Mock<IPrincipal> mockPrincipal = mocks.Create<IPrincipal>();
            mockPrincipal.SetupGet(p => p.Identity.Name).Returns("testUser");
            mockPrincipal.Setup(p => p.IsInRole("User")).Returns(true);
            var mockContext = new Mock<ControllerContext>();
            mockContext.SetupGet(p => p.HttpContext.User).Returns(mockPrincipal.Object);
            mockContext.SetupGet(p => p.HttpContext.User.Identity.IsAuthenticated).Returns(false);
            _userController = new UserController(_mockUserService.Object, _mockProfileService.Object) { ControllerContext = mockContext.Object };

            var actionResult = _userController.EditProfile() as ActionResult;
            Assert.IsInstanceOfType(actionResult, typeof(RedirectToRouteResult));
            RedirectToRouteResult routeResult = actionResult as RedirectToRouteResult;
            Assert.AreEqual(routeResult.RouteValues["action"], "Index");
            Assert.AreEqual(routeResult.RouteValues["controller"], "Home");
        }

        [TestMethod]
        public void EditProfilePostAuthenticatedTest()
        {
            ProfileEditProfileViewModel model = new ProfileEditProfileViewModel()
            {
                FirstName = _profiles[0].FirstName,
                LastName = _profiles[0].LastName,
                PhoneNumber = _profiles[0].Phone
            };

            _mockUserService.Setup(ser => ser.GetUserByEmail(It.IsAny<string>())).Returns(_users[0]);
            _mockProfileService.Setup(ser => ser.GetProfileById(It.IsAny<int>())).Returns(_profiles[0]);
            _mockProfileService.Setup(ser => ser.UpdateProfile(It.IsAny<Profile>())).Returns(_profiles[0]);

            var actionResult = _userController.EditProfile(model) as ActionResult;
            Assert.IsInstanceOfType(actionResult, typeof(RedirectToRouteResult));
            RedirectToRouteResult routeResult = actionResult as RedirectToRouteResult;
            Assert.AreEqual(routeResult.RouteValues["action"], "Index");
            Assert.AreEqual(routeResult.RouteValues["controller"], "Home");
        }

        [TestMethod]
        public void EditProfilePostNotAuthenticatedTest()
        {
            ProfileEditProfileViewModel model = new ProfileEditProfileViewModel()
            {
                FirstName = _profiles[0].FirstName,
                LastName = _profiles[0].LastName,
                PhoneNumber = _profiles[0].Phone
            };

            var mocks = new MockRepository(MockBehavior.Default);
            Mock<IPrincipal> mockPrincipal = mocks.Create<IPrincipal>();
            mockPrincipal.SetupGet(p => p.Identity.Name).Returns("testUser");
            mockPrincipal.Setup(p => p.IsInRole("User")).Returns(true);
            var mockContext = new Mock<ControllerContext>();
            mockContext.SetupGet(p => p.HttpContext.User).Returns(mockPrincipal.Object);
            mockContext.SetupGet(p => p.HttpContext.User.Identity.IsAuthenticated).Returns(false);
            _userController = new UserController(_mockUserService.Object, _mockProfileService.Object) { ControllerContext = mockContext.Object };

            var actionResult = _userController.EditProfile(model) as ActionResult;
            Assert.IsInstanceOfType(actionResult, typeof(RedirectToRouteResult));
            RedirectToRouteResult routeResult = actionResult as RedirectToRouteResult;
            Assert.AreEqual(routeResult.RouteValues["action"], "Index");
            Assert.AreEqual(routeResult.RouteValues["controller"], "Home");
        }

    }
}
