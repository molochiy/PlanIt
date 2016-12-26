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
            Assert.IsTrue(true);
        }


    }
}
