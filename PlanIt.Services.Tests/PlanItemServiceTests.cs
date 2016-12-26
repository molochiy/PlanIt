using System;
using System.Collections.Generic;
using Moq;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PlanIt.Entities;
using PlanIt.Repositories.Abstract;
using PlanIt.Services.Abstract;
using PlanIt.Services.Concrete;

namespace PlanIt.Services.Tests
{
    [TestClass]
    public class PlanItemServiceTests
    {
        private Mock<IRepository> _mockRepository;
        private IPlanItemService _planItemService;
        private Plan _plan;
        private IList<PlanItem> _planItems;

        [TestInitialize]
        public void Initialize()
        {
            _mockRepository = new Mock<IRepository>();
            _planItemService = new PlanItemService(_mockRepository.Object);
            _planItems = new List<PlanItem>
            {
                new PlanItem {Id = 1, PlanId = 1, Title = "Item1"},
                new PlanItem {Id = 2, PlanId = 1, Title = "Item2"},
                new PlanItem {Id = 3, PlanId = 1, Title = "Item3"}
            };
            _plan = new Plan { Id = 1, UserId = 1, Title = "Plan", PlanItems = _planItems };

        }

        [TestMethod]
        public void GetPlanItemByIdFoundTest()
        {
            // Arrange
            _mockRepository.Setup(rep => rep.GetSingle(It.IsAny<Func<PlanItem, bool>>())).Returns(_planItems[0]);

            // Act
            var actualResult = _planItemService.GetPlanItemById(1);

            // Assert
            Assert.AreEqual(_planItems[0], actualResult);
        }

        [TestMethod]
        public void GetPlanItemByIdNotFoundTest()
        {
            // Arrange
            _mockRepository.Setup(rep => rep.GetSingle(It.IsAny<Func<PlanItem, bool>>())).Returns((PlanItem)null);

            // Act
            var actualResult = _planItemService.GetPlanItemById(0);

            // Assert
            Assert.AreEqual(null, actualResult);
        }

        [TestMethod]
        public void UpdatePlanItemTest()
        {
            // Arrange
            _mockRepository.Setup(rep => rep.Update(It.IsAny<PlanItem>())).Returns<PlanItem>(u => u);

            // Act
            var actualResult = _planItemService.UpdatePlanItem(_planItems[0]);

            // Assert
            Assert.AreEqual(_planItems[0], actualResult);
        }

        [TestMethod]
        public void InsertPlanItemTest()
        {
            // Arrange
            _mockRepository.Setup(rep => rep.Insert(It.IsAny<PlanItem>())).Returns<PlanItem>(u => u);

            // Act
            var actualResult = _planItemService.SavePlanItem(_planItems[0]);

            // Assert
            Assert.AreEqual(_planItems[0], actualResult);
        }

        [TestMethod]
        public void GetPlanItemsByPlanIdFoundTest()
        {
            // Arrange
            _mockRepository.Setup(rep => rep.Get(It.IsAny<Func<PlanItem, bool>>())).Returns((List<PlanItem>)_planItems);

            // Act
            var actualResult = _planItemService.GetPlanItemsByPlanId(1);

            // Assert
            Assert.AreEqual(_plan.PlanItems, actualResult);
        }

        [TestMethod]
        public void GetPlanItemsByPlanIdNotFoundTest()
        {
            // Arrange
            _mockRepository.Setup(rep => rep.Get(It.IsAny<Func<PlanItem, bool>>())).Returns((List<PlanItem>)null);

            // Act
            var actualResult = _planItemService.GetPlanItemsByPlanId(2);

            // Assert
            Assert.AreEqual(null, actualResult);
        }

    }
}
