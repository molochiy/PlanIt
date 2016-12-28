using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
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
                new SharedPlanUser { Id = 2, PlanId = 2, UserOwnerId = 1, UserReceiverId = 2, SharingStatusId= 2, OwnerWasNotified = true, UserOwner = new User(), UserReceiver = new User(), Plan = new Plan(), SharingStatus = new SharingStatus() { Name = String.Empty }  }
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
            Assert.AreEqual(2, model.Notifications.Count());
        }

        [TestMethod]
        public void IndexRedirectToActionTest()
        {
            _mockSharingService.Setup(ss => ss.GetSharingInfoForNotifications(It.IsAny<string>())).Returns(new List<SharedPlanUser>());

            var result = _userInteractionsController.Index() as RedirectToRouteResult;

            Assert.IsNotNull(result);
            Assert.AreEqual("User", result.RouteValues["controller"]);
            Assert.AreEqual("LogIn", result.RouteValues["action"]);
        }

        [TestMethod]
        public void GetUsersByPartOfEmailsExceptCurrentUserTest()
        {
            var controllerContext = new Mock<ControllerContext>();
            var principal = new Mock<IPrincipal>();
            principal.SetupGet(x => x.Identity.Name).Returns("email");
            controllerContext.SetupGet(x => x.HttpContext.User).Returns(principal.Object);
            _userInteractionsController.ControllerContext = controllerContext.Object;
            var emails = new List<string>() { "email1", "email2" };

            _mockUserService.Setup(ss => ss.GetEmailsForSharing(It.IsAny<string>(), It.IsAny<string>())).Returns(emails);

            var actualEmails = _userInteractionsController.GetUsersByPartOfEmailsExceptCurrentUser("email");

            Assert.AreEqual(JsonConvert.SerializeObject(emails), actualEmails);
        }

        [TestMethod]
        public void ChangeSharedPlanUserStatusTest()
        {
            int actualSharedPlanUserIdChangeSharingStatus = 0;
            string actualNewSharingStatus = null;
            _mockSharingService.Setup(ss => ss.ChangeSharingStatus(It.IsAny<int>(), It.IsAny<string>())).Returns(new SharedPlanUser()).Callback<int, string>(
                (sharedPlanUserId, newSharingStatus) =>
                {
                    actualSharedPlanUserIdChangeSharingStatus = sharedPlanUserId;
                    actualNewSharingStatus = newSharingStatus;
                });

            int actualSharedPlanUserIdChangeOwnerWasNotifiedProperty = 0;
            bool actualNewValue = true;
            _mockSharingService.Setup(ss => ss.ChangeOwnerWasNotifiedProperty(It.IsAny<int>(), It.IsAny<bool>())).Returns(new SharedPlanUser()).Callback<int, bool>(
                (sharedPlanUserId, newValue) =>
                {
                    actualSharedPlanUserIdChangeOwnerWasNotifiedProperty = sharedPlanUserId; actualNewValue = newValue;
                });

            string expectedUserEmailForNotification = "userEmailForNotification";

            int actualSharedPlanUserIdCGetOwnerEmailBySharingInfoId = 0;
            _mockSharingService.Setup(ss => ss.GetOwnerEmailBySharingInfoId(It.IsAny<int>())).Returns(expectedUserEmailForNotification).Callback<int>(
                (sharedPlanUserId) =>
                {
                    actualSharedPlanUserIdCGetOwnerEmailBySharingInfoId = sharedPlanUserId;
                });

            List<string> actualReceivers = null;
            _mockNotificationHub.Setup(nh => nh.UpdateNotification(It.IsAny<List<string>>())).Callback<List<string>>(
                receivers =>
                {
                    actualReceivers = receivers;
                });

            int expectedSharedPlanUserId = 1;
            string expectedNewStatus = "NewStatus";

            var result = _userInteractionsController.ChangeSharedPlanUserStatus(expectedSharedPlanUserId, expectedNewStatus) as RedirectToRouteResult;

            Assert.IsNotNull(result);
            Assert.AreEqual("UserInteractions", result.RouteValues["controller"]);
            Assert.AreEqual("Index", result.RouteValues["action"]);
            Assert.AreEqual(expectedSharedPlanUserId, actualSharedPlanUserIdChangeSharingStatus);
            Assert.AreEqual(expectedSharedPlanUserId, actualSharedPlanUserIdChangeOwnerWasNotifiedProperty);
            Assert.AreEqual(expectedSharedPlanUserId, actualSharedPlanUserIdCGetOwnerEmailBySharingInfoId);
            Assert.AreEqual(false, actualNewValue);
            Assert.AreEqual(expectedNewStatus, actualNewSharingStatus);
            Assert.IsTrue(new List<string> {expectedUserEmailForNotification}.SequenceEqual(actualReceivers));
        }

        [TestMethod]
        public void ChangeOwnerWasNotifiedPropertyTest()
        {
            int actualSharedPlanUserIdChangeOwnerWasNotifiedProperty = 0;
            bool actualNewValue = true;
            _mockSharingService.Setup(ss => ss.ChangeOwnerWasNotifiedProperty(It.IsAny<int>(), It.IsAny<bool>())).Returns(new SharedPlanUser()).Callback<int, bool>(
                (sharedPlanUserId, newValue) =>
                {
                    actualSharedPlanUserIdChangeOwnerWasNotifiedProperty = sharedPlanUserId; actualNewValue = newValue;
                });

            int expectedSharedPlanUserId = 1;
            string expectedNewStatus = "NewStatus";

            var result = _userInteractionsController.ChangeOwnerWasNotifiedProperty(expectedSharedPlanUserId, false) as RedirectToRouteResult;

            Assert.IsNotNull(result);
            Assert.AreEqual("UserInteractions", result.RouteValues["controller"]);
            Assert.AreEqual("Index", result.RouteValues["action"]);
            Assert.AreEqual(expectedSharedPlanUserId, actualSharedPlanUserIdChangeOwnerWasNotifiedProperty);
            Assert.AreEqual(false, actualNewValue);
        }

        [TestMethod]
        public void SharePlanWithYourselfTest()
        {
            var expectedSuccess = false;
            var expectedMessage = "You can't share plan with yourself.";
            var toUserEmail = "email";

            var controllerContext = new Mock<ControllerContext>();
            var principal = new Mock<IPrincipal>();
            principal.SetupGet(x => x.Identity.Name).Returns(toUserEmail);
            controllerContext.SetupGet(x => x.HttpContext.User).Returns(principal.Object);
            _userInteractionsController.ControllerContext = controllerContext.Object;

            var actualResult = _userInteractionsController.SharePlan(0, toUserEmail);

            Assert.AreEqual(new { success = expectedSuccess, message = expectedMessage}.ToString(), actualResult.Data.ToString());
        }

        [TestMethod]
        public void SharePlanUserDontExistTest()
        {
            var expectedSuccess = false;
            var expectedMessage = "User with enetered email not found!";
            var toUserEmail = "useremail";

            var controllerContext = new Mock<ControllerContext>();
            var principal = new Mock<IPrincipal>();
            principal.SetupGet(x => x.Identity.Name).Returns("email");
            controllerContext.SetupGet(x => x.HttpContext.User).Returns(principal.Object);
            _userInteractionsController.ControllerContext = controllerContext.Object;

            _mockUserService.Setup(us => us.UserWithSpecificEmailExists(It.IsAny<string>())).Returns(false);

            var actualResult = _userInteractionsController.SharePlan(0, toUserEmail);

            Assert.AreEqual(new { success = expectedSuccess, message = expectedMessage }.ToString(), actualResult.Data.ToString());
        }

        [TestMethod]
        public void SharePlanSuccessTest()
        {
            var expectedSuccess = true;
            var expectedMessage = "Plan was successfully shared!";
            var toUserEmail = "useremail";

            var controllerContext = new Mock<ControllerContext>();
            var principal = new Mock<IPrincipal>();
            principal.SetupGet(x => x.Identity.Name).Returns("email");
            controllerContext.SetupGet(x => x.HttpContext.User).Returns(principal.Object);
            _userInteractionsController.ControllerContext = controllerContext.Object;

            _mockUserService.Setup(us => us.UserWithSpecificEmailExists(It.IsAny<string>())).Returns(true);
            _mockSharingService.Setup(ss => ss.SharePlan(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>()));
            _mockNotificationHub.Setup(nh => nh.UpdateNotification(It.IsAny<List<string>>()));

            var actualResult = _userInteractionsController.SharePlan(0, toUserEmail);

            Assert.AreEqual(new { success = expectedSuccess, message = expectedMessage }.ToString(), actualResult.Data.ToString());
        }

        [TestMethod]
        public void SharePlanInternalServerErrorTest()
        {
            var expectedSuccess = false;
            var expectedMessage = "Server error! Plan wasn't shared.";
            var toUserEmail = "useremail";

            var controllerContext = new Mock<ControllerContext>();
            var principal = new Mock<IPrincipal>();
            principal.SetupGet(x => x.Identity.Name).Returns("email");
            controllerContext.SetupGet(x => x.HttpContext.User).Returns(principal.Object);
            _userInteractionsController.ControllerContext = controllerContext.Object;

            _mockUserService.Setup(us => us.UserWithSpecificEmailExists(It.IsAny<string>())).Throws<InvalidOperationException>();

            var actualResult = _userInteractionsController.SharePlan(0, toUserEmail);

            Assert.AreEqual(new { success = expectedSuccess, message = expectedMessage }.ToString(), actualResult.Data.ToString());
        }

        [TestMethod]
        public void RemoveParticipantSuccessTest()
        {
            //Arrange
            var expectedSuccess = true;
            var expectedMessage = "Participant was deleted!";
            var participantEmail = "participantemail";

            var controllerContext = new Mock<ControllerContext>();
            var principal = new Mock<IPrincipal>();
            principal.SetupGet(x => x.Identity.Name).Returns("email");
            controllerContext.SetupGet(x => x.HttpContext.User).Returns(principal.Object);
            _userInteractionsController.ControllerContext = controllerContext.Object;

            _mockSharingService.Setup(s => s.RemoveParticipant(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>())).Returns(new SharedPlanUser());

            //Act
            var actualResult = _userInteractionsController.RemoveParticipant(participantEmail, 0);

            //Assert
            Assert.AreEqual(new { success = expectedSuccess, message = expectedMessage }.ToString(), actualResult.Data.ToString());
        }

        [TestMethod]
        public void RemoveParticipantInternalServerErrorTest()
        {
            var expectedSuccess = false;
            var expectedMessage = "Server error! Participant wasn't deleted.";
            var participantEmail = "participantemail";

            var controllerContext = new Mock<ControllerContext>();
            var principal = new Mock<IPrincipal>();
            principal.SetupGet(x => x.Identity.Name).Returns("email");
            controllerContext.SetupGet(x => x.HttpContext.User).Returns(principal.Object);
            _userInteractionsController.ControllerContext = controllerContext.Object;

            _mockSharingService.Setup(s => s.RemoveParticipant(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>())).Throws<InvalidOperationException>();

            //Act
            var actualResult = _userInteractionsController.RemoveParticipant(participantEmail, 0);

            Assert.AreEqual(new { success = expectedSuccess, message = expectedMessage }.ToString(), actualResult.Data.ToString());
        }

        [TestMethod]
        public void GetNumberOfNotificationsTest()
        {
            var controllerContext = new Mock<ControllerContext>();
            var principal = new Mock<IPrincipal>();
            principal.SetupGet(x => x.Identity.Name).Returns("email");
            controllerContext.SetupGet(x => x.HttpContext.User).Returns(principal.Object);
            _userInteractionsController.ControllerContext = controllerContext.Object;

            _mockSharingService.Setup(ss => ss.GetNumberOfNotifications(It.IsAny<string>())).Returns(1);

            var actualResult = _userInteractionsController.GetNumberOfNotifications();

            Assert.AreEqual(new { numberOfNotifications = 1 }.ToString(), actualResult.Data.ToString());
        }

        [TestMethod]
        public void GetReceiversEmailsOfCurrentPlanTest()
        {
            //Arrange
            var controllerContext = new Mock<ControllerContext>();
            var principal = new Mock<IPrincipal>();
            principal.SetupGet(x => x.Identity.Name).Returns("email");
            controllerContext.SetupGet(x => x.HttpContext.User).Returns(principal.Object);
            _userInteractionsController.ControllerContext = controllerContext.Object;

            List<string> expected = new List<string> { "email@gmail.com" };
            _mockSharingService.Setup(ss => ss.GetReceiversEmailsByPlanId(It.IsAny<int>())).Returns(expected);

            //Act
            var actual = _userInteractionsController.GetReceiversEmailsOfCurrentPlan(1);

            //Assert
            Assert.AreEqual(JsonConvert.SerializeObject(expected), actual);
        }
    }
}
