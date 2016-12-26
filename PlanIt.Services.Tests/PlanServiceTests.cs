using System;
using System.Text;
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
        private IList<Plan> _plans;

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

        //GetPlanById
        [TestMethod]
        public void GetPlanById()
        {
            // Arrange
            _mockRepository.Setup(rep => rep.GetSingle(It.IsAny<Func<Plan, bool>>())).Returns(_plans[0]);

            // Act
            var actualResult = _planService.GetPlanById(0);

            // Assert
            Assert.AreEqual(_plans[0], actualResult);
        }
        //GetPlansByUserId
        //SavePlan
        //SaveComment
        //UpdatePlan
        //GetAllCommentsByPlanId
        //GetAllPublicPlansByUserId
        //PlanIsPublic


        [TestMethod]
        public void Test()
        {
            // Arrange
            _mockRepository.Setup(rep => rep.GetSingle(It.IsAny<Func<Plan, bool>>())).Returns(_plans[0]);

            // Act

            // Assert
            Assert.AreEqual(1, 1);
        }
    }
}
