using System;
using System.Collections.Generic;
using Moq;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PlanIt.Entities;
using PlanIt.Repositories.Abstract;
using PlanIt.Services.Abstract;
using PlanIt.Services.Concrete;

namespace PlanIt.Web.Tests
{
    [TestClass]
    public class SharingServiceTests
    {
        private Mock<IRepository> _mockRepository;
        private ISharingService _sharingService;

        [TestInitialize]
        public void Initialize()
        {
            _mockRepository = new Mock<IRepository>();
            _sharingService = new SharingService(_mockRepository.Object);
        }

        [TestMethod]
        public void SharePlanTest()
        {
            // Arrange
            Plan plan = new Plan { Id = 1, Title = "Title", UserId = 1 };
            User owner = new User { Id = 1, Email = "owner@gmail.com", IsEmailConfirmed = true, Password = "pass1", ProfileId = 1 };
            User receiver = new User { Id = 2, Email = "receiver@gmail.com", IsEmailConfirmed = true, Password = "pass2", ProfileId = 2 };
            SharingStatus pendingStatus = new SharingStatus { Id = 1, Name = "Pending" };

            _mockRepository
                .SetupSequence(rep => rep.GetSingle<User>(It.IsAny<Func<User, bool>>()))
                .Returns(owner)
                .Returns(receiver);
            _mockRepository.Setup(rep => rep.GetSingle<SharingStatus>(It.IsAny<Func<SharingStatus, bool>>())).Returns(pendingStatus);
            _mockRepository.Setup(rep => rep.Insert(It.IsAny<SharedPlanUser>())).Returns<SharedPlanUser>(u => u);

            var sharingDateTime = DateTime.Now;
            SharedPlanUser expectedSharingInfo = new SharedPlanUser
            {
                PlanId = plan.Id,
                SharingDateTime = sharingDateTime,
                SharingStatusId = pendingStatus.Id,
                UserOwnerId = owner.Id,
                UserReceiverId = receiver.Id
            };

            // Act
            var sharingInfo = _sharingService.SharePlan(plan.Id, owner.Email, receiver.Email);

            // Assert
            Assert.AreEqual(expectedSharingInfo.PlanId, sharingInfo.PlanId);
            Assert.IsTrue(Math.Abs((expectedSharingInfo.SharingDateTime - sharingInfo.SharingDateTime).TotalSeconds) < 1);
            Assert.AreEqual(expectedSharingInfo.SharingStatusId, sharingInfo.SharingStatusId);
            Assert.AreEqual(expectedSharingInfo.UserOwnerId, sharingInfo.UserOwnerId);
            Assert.AreEqual(expectedSharingInfo.UserReceiverId, sharingInfo.UserReceiverId);
        }

        [TestMethod]
        public void GetSharingInfoForNotificationsTest()
        {
            User user = new User { Id = 1, Email = "user@gmail.com", IsEmailConfirmed = true, ProfileId = 1 };
            _mockRepository.Setup(rep => rep.GetSingle<User>(It.IsAny<Func<User, bool>>())).Returns(user);
            _mockRepository.Setup(rep => rep.GetSingle<SharingStatus>(It.IsAny<Func<SharingStatus, bool>>())).Returns(new SharingStatus());
            List<SharedPlanUser> repositoryInfo = new List<SharedPlanUser>();
            _mockRepository.Setup(rep => rep.Get<SharedPlanUser>(It.IsAny<Func<SharedPlanUser, bool>>(),
                                                                 It.IsAny<Expression<Func<SharedPlanUser, object>>[]>())).Returns(repositoryInfo);

            //Act
            List<SharedPlanUser> realInfo = _sharingService.GetSharingInfoForNotifications(user.Email);

            //Assert
            Assert.IsTrue(realInfo.Count == 0);
        }

        [TestMethod]
        public void GetNumberOfNotificationsTest()
        {
            User user = new User { Id = 1, Email = "user@gmail.com", IsEmailConfirmed = true, ProfileId = 1 };
            _mockRepository.Setup(rep => rep.GetSingle<User>(It.IsAny<Func<User, bool>>())).Returns(user);
            _mockRepository.Setup(rep => rep.GetSingle<SharingStatus>(It.IsAny<Func<SharingStatus, bool>>())).Returns(new SharingStatus());
            List<SharedPlanUser> repositoryInfo = new List<SharedPlanUser>();
            _mockRepository.Setup(rep => rep.Get<SharedPlanUser>(It.IsAny<Func<SharedPlanUser, bool>>(),
                                                                 It.IsAny<Expression<Func<SharedPlanUser, object>>[]>())).Returns(repositoryInfo);

            //Act
            int actualNumber = _sharingService.GetNumberOfNotifications(user.Email);

            //Assert
            Assert.AreEqual(repositoryInfo.Count, actualNumber);
        }

        [TestMethod]
        public void ChangeSharingStatusTest()
        {
            //Arrange
            SharedPlanUser sharingInfo = new SharedPlanUser { Id = 1, PlanId = 1, UserOwnerId = 1, UserReceiverId = 2, SharingStatusId = 1, OwnerWasNotified = false };
            SharingStatus newSharingStatus = new SharingStatus { Id = 2, Name = "Accepted" };
            _mockRepository.Setup(rep => rep.GetSingle<SharedPlanUser>(It.IsAny<Func<SharedPlanUser, bool>>())).Returns(sharingInfo);
            _mockRepository.Setup(rep => rep.GetSingle<SharingStatus>(It.IsAny<Func<SharingStatus, bool>>())).Returns(newSharingStatus);
            sharingInfo.SharingStatusId = newSharingStatus.Id;
            sharingInfo.SharingDateTime = DateTime.Now;
            _mockRepository.Setup(rep => rep.Update(It.IsAny<SharedPlanUser>())).Returns<SharedPlanUser>(u => u);
            SharedPlanUser expectedInfo = new SharedPlanUser { Id = 1, PlanId = 1, UserOwnerId = 1, UserReceiverId = 2, SharingDateTime = DateTime.Now, SharingStatusId = newSharingStatus.Id, OwnerWasNotified = false };

            //Act
            var actualInfo = _sharingService.ChangeSharingStatus(sharingInfo.Id, newSharingStatus.Name);

            //Assert
            Assert.AreEqual(expectedInfo.Id, actualInfo.Id);
            Assert.AreEqual(expectedInfo.PlanId, actualInfo.PlanId);
            Assert.AreEqual(expectedInfo.UserOwnerId, actualInfo.UserOwnerId);
            Assert.AreEqual(expectedInfo.UserReceiverId, actualInfo.UserReceiverId);
            Assert.AreEqual(expectedInfo.SharingStatus, actualInfo.SharingStatus);
            Assert.IsTrue(Math.Abs((expectedInfo.SharingDateTime - actualInfo.SharingDateTime).TotalSeconds) < 1);
        }

        [TestMethod]
        public void ChangeOwnerWasNotifiedPropertyTest()
        {
            //Arrange
            SharedPlanUser sharingInfo = new SharedPlanUser { Id = 1, PlanId = 1, UserOwnerId = 1, UserReceiverId = 2, SharingStatusId = 1, OwnerWasNotified = false };
            _mockRepository.Setup(rep => rep.GetSingle<SharedPlanUser>(It.IsAny<Func<SharedPlanUser, bool>>())).Returns(sharingInfo);
            bool ownerWasNotified = true;
            sharingInfo.OwnerWasNotified = ownerWasNotified;
            _mockRepository.Setup(rep => rep.Update(It.IsAny<SharedPlanUser>())).Returns<SharedPlanUser>(u => u);
            SharedPlanUser expectedInfo = new SharedPlanUser { Id = 1, PlanId = 1, UserOwnerId = 1, UserReceiverId = 2, SharingStatusId = 1, OwnerWasNotified = ownerWasNotified };

            //Act
            var actualInfo = _sharingService.ChangeOwnerWasNotifiedProperty(sharingInfo.Id, ownerWasNotified);

            //Assert
            Assert.AreEqual(expectedInfo.OwnerWasNotified, actualInfo.OwnerWasNotified);
        }

        [TestMethod]
        public void GetSharingStatusByIdTest()
        {
            //Arrange
            SharingStatus newSharingStatus = new SharingStatus { Id = 2, Name = "Accepted" };
            _mockRepository.Setup(rep => rep.GetSingle<SharingStatus>(It.IsAny<Func<SharingStatus, bool>>())).Returns(newSharingStatus);
            string expectedName = "Accepted";

            //Act
            string actualName = _sharingService.GetSharingStatusById(newSharingStatus.Id);

            //Assert
            Assert.AreEqual(expectedName, actualName);
        }

        [TestMethod]
        public void GetUsersEmailsForNotificationTest()
        {
            //Arrange

            //Act

            //Assert
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void GetUsersEmailsWhoShouldGetCommentTest()
        {
            //Arrange
            SharingStatus sharingStatus = new SharingStatus { Id = 2, Name = "Accepted" };
            _mockRepository.Setup(rep => rep.GetSingle<SharingStatus>(It.IsAny<Func<SharingStatus, bool>>())).Returns(sharingStatus);
            Plan plan = new Plan { Id = 1, UserId = 1 };
            User owner = new User { Id = 1, Email = "user1@gmail.com", IsEmailConfirmed = true, Password = "pass1", ProfileId = 1 };
            List<User> receivers = new List<User>
            {
               new User { Id = 2, Email = "user2@gmail.com", IsEmailConfirmed = true, Password = "pass2", ProfileId = 2 },
             new User { Id = 3, Email = "user3@gmail.com", IsEmailConfirmed = true, Password = "pass3", ProfileId = 3 }
            };
            List<SharedPlanUser> sharingInfoList = new List<SharedPlanUser>
            {
                new SharedPlanUser {Id=1, PlanId=1, UserOwnerId=1, UserReceiverId=2, SharingStatusId=2, OwnerWasNotified = false },
                new SharedPlanUser {Id=2, PlanId=1, UserOwnerId=1, UserReceiverId=3, SharingStatusId=2, OwnerWasNotified = false }
            };
            _mockRepository.Setup(rep => rep.Get<SharedPlanUser>(It.IsAny<Func<SharedPlanUser, bool>>())).Returns(sharingInfoList);
            _mockRepository.Setup(rep => rep.Get<User>(It.IsAny<Func<User, bool>>())).Returns(receivers);
            _mockRepository.Setup(rep => rep.GetSingle<User>(It.IsAny<Func<User, bool>>())).Returns(owner);
            _mockRepository.Setup(rep => rep.GetSingle<Plan>(It.IsAny<Func<Plan, bool>>())).Returns(plan);

            //Act
            List<string> actualUserEmails = _sharingService.GetUsersEmailsWhoShouldGetComment(plan.Id);

            //Assert
            Assert.IsTrue(actualUserEmails.Count == 3);
            CollectionAssert.Contains(actualUserEmails, "user1@gmail.com");
            CollectionAssert.Contains(actualUserEmails, "user2@gmail.com");
            CollectionAssert.Contains(actualUserEmails, "user3@gmail.com");
        }
    }
}
