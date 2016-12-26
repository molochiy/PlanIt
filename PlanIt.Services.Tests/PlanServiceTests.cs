using System;
using System.Text;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PlanIt.Entities;
using PlanIt.Repositories.Abstract;
using PlanIt.Services.Abstract;
using PlanIt.Services.Concrete;

namespace PlanIt.Services.Tests
{
    [TestClass]
    public class PlanServiceTests
    {
        private Mock<IRepository> _mockRepository;
        private IPlanService _planService;
        private List<Plan> _plans;

        [TestInitialize]
        public void Initialize()
        {
            _mockRepository = new Mock<IRepository>();
            _planService = new PlanService(_mockRepository.Object);
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
        public void FilterPlanItemsTest()
        {
            //Arrange
            IList<PlanItem> planItems = new List<PlanItem>
            {
                new PlanItem {Id = 1, PlanId = 1, Title = "item1", IsDeleted = false },
                new PlanItem {Id = 2, PlanId = 1, Title = "item2", IsDeleted = true },
                new PlanItem {Id = 3, PlanId = 1, Title = "item3", IsDeleted = false }

            };

            //Act
            List<PlanItem> actual = _planService.FilterPlanItems(planItems);
            
            //Assert
            Assert.AreEqual(2, actual.Count);
            Assert.AreEqual(1, actual[0].Id);
            Assert.AreEqual(3, actual[1].Id);
        }

        [TestMethod]
        public void GetPlanByIdFoundTest()
        {
            // Arrange
            _mockRepository.Setup(rep => rep.GetSingle<Plan>(It.IsAny<Func<Plan, bool>>(), 
                It.IsAny<Expression<Func<Plan, object>>[]>())).Returns(_plans[0]);

            // Act
            var actualResult = _planService.GetPlanById(1);

            // Assert
            Assert.AreEqual(_plans[0], actualResult);
        }

        [TestMethod]
        public void GetPlanByIdNotFoundTest()
        {
            // Arrange
            _mockRepository.Setup(rep => rep.GetSingle<Plan>(It.IsAny<Func<Plan, bool>>(),
                It.IsAny<Expression<Func<Plan, object>>[]>())).Returns((Plan)null);

            // Act
            var actualResult = _planService.GetPlanById(1);

            // Assert
            Assert.AreEqual(null, actualResult);
        }

        [TestMethod]
        public void GetPlanByUserIdTest()
        {
            //Arrange
            List<Plan> plans = _plans.Where(p => p.UserId == 1).ToList();
            _mockRepository.Setup(rep => rep.Get<Plan>(It.IsAny<Func<Plan, bool>>(),
                It.IsAny<Expression<Func<Plan, object>>[]>())).Returns(plans);
            //Act
            List<Plan> actual = _planService.GetPlansByUserId(1).ToList();

            //Assert
            Assert.AreEqual(plans.Count, actual.Count);
            Assert.AreEqual(plans[0].Id, actual[0].Id);
            Assert.AreEqual(plans[1].Id, actual[1].Id);
            Assert.AreEqual(plans[2].Id, actual[2].Id);
        }

        [TestMethod]
        public void SavePlanTest()
        {
            // Arrange
            _mockRepository.Setup(rep => rep.Insert(It.IsAny<Plan>())).Returns<Plan>(u => u);

            // Act
            var actualResult = _planService.SavePlan(_plans[0]);

            // Assert
            Assert.AreEqual(_plans[0], actualResult);
        }

        [TestMethod]
        public void SaveCommentTest()
        {
            Comment comment = new Comment { Id = 1, PlanId = 1, Text = "SomeText", UserId = 1 };
            // Arrange
            _mockRepository.Setup(rep => rep.Insert(It.IsAny<Comment>())).Returns<Comment>(c => c);

            // Act
            var actualResult = _planService.SaveComment(comment);

            // Assert
            Assert.AreEqual(comment, actualResult);
        }

        [TestMethod]
        public void UpdatePlanTest()
        {
            // Arrange
            _mockRepository.Setup(rep => rep.Update(It.IsAny<Plan>())).Returns<Plan>(u => u);

            // Act
            var actualResult = _planService.UpdatePlan(_plans[0]);

            // Assert
            Assert.AreEqual(_plans[0], actualResult);
        }

        [TestMethod]
        public void GetAllPublicPlansByUserIdTest()
        {
            //Arrange
            SharingStatus status = new SharingStatus();
            List<Plan> plans = new List<Plan>();
            List<SharedPlanUser> sharingInfo = new List<SharedPlanUser>();

            _mockRepository.Setup(rep => rep.Get<Plan>(It.IsAny<Func<Plan, bool>>(),
               It.IsAny<Expression<Func<Plan, object>>[]>())).Returns(plans);
            _mockRepository.Setup(rep => rep.Get<SharedPlanUser>(It.IsAny<Func<SharedPlanUser, bool>>())).Returns(sharingInfo);
            _mockRepository.Setup(rep => rep.GetSingle<SharingStatus>(It.IsAny<Func<SharingStatus, bool>>())).Returns(status);

            //Act
            List<Plan> actual = _planService.GetAllPublicPlansByUserId(1);

            //Assert
            Assert.AreEqual(plans.Count, actual.Count);
            Assert.IsTrue(actual.Count == 0);
        }
    }
}