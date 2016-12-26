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

namespace PlanIt.Services.Tests
{
    [TestClass]
    public class PlanControllerTests
    {
        private Mock<IRepository> _mockRepository;
        private IPlanService _planService;
        private IUserService _userService;
        private ISharingService _sharingService;
        private IPlanItemService _planItemService;
        private INotificationHub _notificationHub;
        private PlanController _planController;
        private IList<Plan> _plans;

        [TestInitialize]
        public void Initialize()
        {
            _mockRepository = new Mock<IRepository>();
            _planService = new PlanService(_mockRepository.Object);
            _userService = new UserService(_mockRepository.Object);
            _sharingService = new SharingService(_mockRepository.Object);
            _planItemService = new PlanItemService(_mockRepository.Object);
            _planController = new PlanController(_planService, _userService, _sharingService, _notificationHub, _planItemService);
            _plans = new List<Plan>
            {
                new Plan { Id = 1, Title = "Title1", Description = "Description1", Begin = new DateTime(), End = new DateTime(), StatusId = 1, IsDeleted = false, UserId = 1},
                new Plan { Id = 2, Title = "Title2", Description = "Description2", Begin = new DateTime(), End = new DateTime(), StatusId = 2, IsDeleted = false, UserId = 1},
                new Plan { Id = 3, Title = "Title3", Description = "Description3", Begin = new DateTime(), End = new DateTime(), StatusId = 2, IsDeleted = true, UserId = 1},
                new Plan { Id = 4, Title = "Title4", Description = "Description4", Begin = new DateTime(), End = new DateTime(), StatusId = 2, IsDeleted = false, UserId = 2},
                new Plan { Id = 5, Title = "Title5", Description = "Description5", Begin = new DateTime(), End = new DateTime(), StatusId = 3, IsDeleted = false, UserId = 2}
            };
        }

        [TestMethod]
        public void TestIndexForGuest()
        {
            var actionResult = _planController.Index() as ActionResult;
            Assert.IsInstanceOfType(actionResult, typeof(RedirectToRouteResult));
            RedirectToRouteResult routeResult = actionResult as RedirectToRouteResult;
            Assert.AreEqual(routeResult.RouteValues["action"], "LogIn");
        }
    }
}
