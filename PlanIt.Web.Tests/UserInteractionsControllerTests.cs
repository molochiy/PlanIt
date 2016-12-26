using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PlanIt.Entities;
using PlanIt.Services.Abstract;
using PlanIt.Web.Controllers;
using PlanIt.Web.Hubs;
using PlanIt.Web.Models;

namespace PlanIt.Web.Tests
{
    [TestClass]
    public class UserInteractionsControllerTests
    {
        private Mock<IUserService> _mockUserService;
        private Mock<ISharingService> _mockSharingService;
        private Mock<INotificationHub> _mockNotificationHub;
        private UserInteractionsController _userInteractionsController;
        private IList<Plan> _plans;

        [TestInitialize]
        public void Initialize()
        {
            _mockUserService = new Mock<IUserService>();
            _mockSharingService = new Mock<ISharingService>();
            _mockNotificationHub = new Mock<INotificationHub>();

            _userInteractionsController = new UserInteractionsController(_mockUserService.Object, _mockSharingService.Object, _mockNotificationHub.Object);
        }

        [TestMethod]
        public void IndexReturnNotNullValuesTest()
        {
            var sharedPlans = new List<SharedPlanUser>
            {
                new SharedPlanUser { Id = 1, PlanId = 1, UserOwnerId = 1, UserReceiverId = 1, SharingStatusId= 3, OwnerWasNotified = false, UserOwner = new User(), UserReceiver = new User(), Plan = new Plan(), SharingStatus = new SharingStatus() { Name = String.Empty } },
                new SharedPlanUser { Id = 1, PlanId = 2, UserOwnerId = 1, UserReceiverId = 2, SharingStatusId= 2, OwnerWasNotified = true, UserOwner = new User(), UserReceiver = new User(), Plan = new Plan(), SharingStatus = new SharingStatus() { Name = String.Empty }  }
            };

            List<NotificationSummaryModel> notifications = new List<NotificationSummaryModel>();

            foreach (var data in sharedPlans)
            {
                notifications.Add(data);
            }

            var controllerContext = new Mock<ControllerContext>();
            var principal = new Mock<IPrincipal>();
            principal.SetupGet(x => x.Identity.Name).Returns("user");
            controllerContext.SetupGet(x => x.HttpContext.User).Returns(principal.Object);
            _userInteractionsController.ControllerContext = controllerContext.Object;

            _mockSharingService.Setup(ss => ss.GetSharingInfoForNotifications(It.IsAny<string>())).Returns(sharedPlans);

            var result = _userInteractionsController.Index() as ViewResult;
            var model = result.Model as NotificationViewModel;

            Assert.IsNotNull(result);
            Assert.IsNotNull(model);
        }
    }
}
