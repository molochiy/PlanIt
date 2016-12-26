using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PlanIt.Entities;
using PlanIt.Repositories.Abstract;
using PlanIt.Services.Abstract;
using PlanIt.Services.Concrete;

namespace PlanIt.Web.Tests
{
    [TestClass]
    public class ProfileServiceTests
    {
        private Mock<IRepository> _mockRepository;
        private IProfileService _profileService;
        private IList<Profile> _profiles;

        [TestInitialize]
        public void Initialize()
        {
            _mockRepository = new Mock<IRepository>();
            _profileService = new ProfileService(_mockRepository.Object);
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
        public void GetProfileByIdFoundTest()
        {
            // Arrange
            _mockRepository.Setup(rep => rep.GetSingle(It.IsAny<Func<Profile, bool>>())).Returns(_profiles[0]);

            // Act
            var actualResult = _profileService.GetProfileById(1);

            // Assert
            Assert.AreEqual(_profiles[0], actualResult);
        }

        [TestMethod]
        public void GetProfileByIdNotFoundTest()
        {
            // Arrange
            _mockRepository.Setup(rep => rep.GetSingle(It.IsAny<Func<Profile, bool>>())).Returns((Profile)null);

            // Act
            var actualResult = _profileService.GetProfileById(0);

            // Assert
            Assert.AreEqual(null, actualResult);
        }

        [TestMethod]
        public void UpdateProfileTest()
        {
            // Arrange
            _mockRepository.Setup(rep => rep.Insert(It.IsAny<Profile>())).Returns<Profile>(u => u);

            // Act
            var actualResult = _profileService.UpdateProfile(_profiles[0]);

            // Assert
            Assert.AreEqual(_profiles[0], actualResult);
        }
    }
}
