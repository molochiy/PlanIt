using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PlanIt.Entities;
using PlanIt.Repositories.Abstract;
using PlanIt.Services.Abstract;
using PlanIt.Services.Concrete;

namespace PlanIt.Services.Tests
{
    [TestClass]
    public class UserServiceTests
    {
        private Mock<IRepository> _mockRepository;
        private IUserService _userService;
        private IList<User> _users;

        [TestInitialize]
        public void Initialize()
        {
            _mockRepository = new Mock<IRepository>();
            _userService = new UserService(_mockRepository.Object);
            _users = new List<User>
            {
                new User { Id = 1, Email = "e1@e.e", IsEmailConfirmed = true, Password = "pass1", ProfileId = 1},
                new User { Id = 2, Email = "ee2@e.e", IsEmailConfirmed = true, Password = "pass2", ProfileId = 2},
                new User { Id = 3, Email = "eee3@e.e", IsEmailConfirmed = false, Password = "pass3", ProfileId = 3},
                new User { Id = 4, Email = "eeee4@e.e", IsEmailConfirmed = false, Password = "pass4", ProfileId = 4},
                new User { Id = 5, Email = "eeeee5@e.e", IsEmailConfirmed = false, Password = "pass5", ProfileId = 5}
            };
        }

        [TestMethod]
        public void GetUserByIdExistTest()
        {
            // Arrange
            _mockRepository.Setup(rep => rep.GetSingle(It.IsAny<Func<User, bool>>())).Returns(_users[0]);

            // Act
            var actualResult = _userService.GetUserById(1);

            // Assert
            Assert.AreEqual(_users[0], actualResult);
        }

        [TestMethod]
        public void GetUserByIdNullTest()
        {
            // Arrange
            _mockRepository.Setup(rep => rep.GetSingle(It.IsAny<Func<User, bool>>())).Returns((User)null);

            // Act
            var actualResult = _userService.GetUserById(0);

            // Assert
            Assert.AreEqual(null, actualResult);
        }

        [TestMethod]
        public void GetUserExistByEmailTest()
        {
            // Arrange
            _mockRepository.Setup(rep => rep.GetSingle(It.IsAny<Func<User, bool>>())).Returns((User)null);

            // Act
            var actualResult = _userService.GetUserExistByEmail("e1@e.e");

            // Assert
            Assert.AreEqual(null, actualResult);
        }

        [TestMethod]
        public void GetUserExistByEmailNullTest()
        {
            // Arrange
            _mockRepository.Setup(rep => rep.GetSingle(It.IsAny<Func<User, bool>>())).Returns((User)null);

            // Act
            var actualResult = _userService.GetUserExistByEmail("");

            // Assert
            Assert.AreEqual(null, actualResult);
        }

        [TestMethod]
        public void GetUserIdByEmailTest()
        {
            // Arrange
            _mockRepository.Setup(rep => rep.GetSingle(It.IsAny<Func<User, bool>>())).Returns(_users[0]);

            // Act
            var actualResult = _userService.GetUserIdByEmail("e1@e.e");

            // Assert
            Assert.AreEqual(1, actualResult);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void GetUserIdByEmailNullRefExceptionTest()
        {
            // Arrange
            _mockRepository.Setup(rep => rep.GetSingle(It.IsAny<Func<User, bool>>())).Returns((User)null);

            // Act
            _userService.GetUserIdByEmail("");
        }

        [TestMethod]
        public void GetUsersEmailsByEmailSubstringExceptCurrentUserTest()
        {
            // Arrange
            _mockRepository.Setup(rep => rep.Get(It.IsAny<Func<User, bool>>())).Returns((List<User>)_users);
            var currentUserEmail = "ee2@e.e";
            var expectedResult = _users.Select(u => u.Email).Where(email => email != currentUserEmail).ToList();

            // Act
            var actualResult = _userService.GetUsersEmailsByEmailSubstringExceptCurrentUser("", currentUserEmail);

            // Assert
            Assert.IsTrue(expectedResult.SequenceEqual(actualResult));
        }

        [TestMethod]
        public void AddUserTest()
        {
            // Arrange
            _mockRepository.Setup(rep => rep.Insert(It.IsAny<User>())).Returns(_users[0]);

            // Act
            var actualResult = _userService.AddUser(_users[0]);

            // Assert
            Assert.AreEqual(_users[0], actualResult);
        }

        [TestMethod]
        public void UserExistsByEmailTrueTest()
        {
            // Arrange
            _mockRepository.Setup(rep => rep.Exists(It.IsAny<Func<User, bool>>())).Returns(true);

            // Act
            var actualResult = _userService.UserExistsByEmail("e1@e.e");

            // Assert
            Assert.IsTrue(actualResult);
        }

        [TestMethod]
        public void UserExistsByEmailFalseTest()
        {
            // Arrange
            _mockRepository.Setup(rep => rep.Exists(It.IsAny<Func<User, bool>>())).Returns(false);

            // Act
            var actualResult = _userService.UserExistsByEmail("");

            // Assert
            Assert.IsFalse(actualResult);
        }
    }
}
