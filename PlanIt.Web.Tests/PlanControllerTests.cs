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
using System.Security.Principal;
using PlanIt.Web.Models;

namespace PlanIt.Services.Tests
{
    [TestClass]
    public class PlanControllerTests
    {
        private Mock<IPlanService> _mockPlanService;
        private Mock<IUserService> _mockUserService;
        private Mock<ISharingService> _mockSharingService;
        private Mock<IPlanItemService> _mockPlanItemService;
        private Mock<INotificationHub> _mockNotificationHub;
        private PlanController _planController;
        private IList<User> _users;

        [TestInitialize]
        public void Initialize()
        {
            _mockPlanService = new Mock<IPlanService>();
            _mockUserService = new Mock<IUserService>();
            _mockSharingService = new Mock<ISharingService>();
            _mockPlanItemService = new Mock<IPlanItemService>();
            _mockNotificationHub = new Mock<INotificationHub>();
            _planController = new PlanController(_mockPlanService.Object, _mockUserService.Object, _mockSharingService.Object,
                _mockNotificationHub.Object, _mockPlanItemService.Object);

            _users = new List<User>
            {
                new User {Id = 1, Email = "123@gmil.com", IsEmailConfirmed = true, Password = "asdasdsa", ProfileId = 1 },
                new User {Id = 2, Email = "124@gmil.com", IsEmailConfirmed = true, Password = "65e84be33532fb784c48129675f9eff3a682b27168c0ea744b2cf58ee02337c5", ProfileId = 2 },
                new User {Id = 3, Email = "125@gmil.com", IsEmailConfirmed = true, Password = "asdnytasdsa", ProfileId = 3 },
            };

            var mocks = new MockRepository(MockBehavior.Default);
            Mock<IPrincipal> mockPrincipal = mocks.Create<IPrincipal>();
            mockPrincipal.SetupGet(p => p.Identity.Name).Returns(_users[0].Email);
            mockPrincipal.Setup(p => p.IsInRole("User")).Returns(true);
            var mockContext = new Mock<ControllerContext>();
            mockContext.SetupGet(p => p.HttpContext.User).Returns(mockPrincipal.Object);
            mockContext.SetupGet(p => p.HttpContext.User.Identity.IsAuthenticated).Returns(true);
            _planController.ControllerContext = mockContext.Object;

        }

        [TestMethod]
        public void TestIndexForGuest()
        {
            var actionResult = _planController.Index() as ActionResult;
            Assert.IsInstanceOfType(actionResult, typeof(RedirectToRouteResult));
            RedirectToRouteResult routeResult = actionResult as RedirectToRouteResult;
            Assert.AreEqual(routeResult.RouteValues["action"], "LogIn");
        }

        [TestMethod]
        public void TestIndexForUSer()
        {
            var plans = new List<Plan>
            {
                new Plan { Id = 1, Title = "Title1", Description = "Description1", Begin = new DateTime(), End = new DateTime(), StatusId = 1, IsDeleted = false, UserId = 1},
                new Plan { Id = 2, Title = "Title2", Description = "Description2", Begin = new DateTime(), End = new DateTime(), StatusId = 2, IsDeleted = false, UserId = 1},
                new Plan { Id = 3, Title = "Title3", Description = "Description3", Begin = new DateTime(), End = new DateTime(), StatusId = 2, IsDeleted = true, UserId = 1}
            };
            _mockUserService.Setup(ser => ser.GetUserByEmail(It.IsAny<string>())).Returns(_users[0]);
            _mockPlanService.Setup(ser => ser.GetPlansByUserId(It.IsAny<int>())).Returns(plans);
            var viewResult = _planController.Index() as ViewResult;
            Assert.IsNotNull(viewResult);
            var model = viewResult.Model as PlanIndexViewModel;
            Assert.IsNotNull(model);
            Assert.AreEqual(plans, model.Plans);
        }
    }
}
