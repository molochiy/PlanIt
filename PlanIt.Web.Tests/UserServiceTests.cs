using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PlanIt.Entities;
using PlanIt.Repositories.Abstract;
using PlanIt.Services.Concrete;

namespace PlanIt.Web.Tests
{
    [TestClass]
    public class UserServiceTests
    {

        private IList<User> _users = new List<User>()
        {
            new User { Id = 1, Email = "e1@e.e", IsEmailConfirmed = true, Password = "pass1", ProfileId = 1},
            new User { Id = 2, Email = "ee2@e.e", IsEmailConfirmed = true, Password = "pass2", ProfileId = 2},
            new User { Id = 3, Email = "eee3@e.e", IsEmailConfirmed = false, Password = "pass3", ProfileId = 3},
            new User { Id = 4, Email = "eeee4@e.e", IsEmailConfirmed = false, Password = "pass4", ProfileId = 4},
            new User { Id = 5, Email = "eeeee5@e.e", IsEmailConfirmed = false, Password = "pass5", ProfileId = 5}
        };

        [TestMethod]
        public void GetUserByIdExistTest()
        {
            var mock = new Mock<IRepository>();

            mock.Setup(rep => rep.GetSingle<User>(It.IsAny<Func<User, bool>>())).Returns(_users[0]);

            var userService = new UserService(mock.Object);

            Assert.AreEqual(_users[0], userService.GetUserById(1));
        }

        [TestMethod]
        public void GetUserByIdNullTest()
        {
            var mock = new Mock<IRepository>();

            mock.Setup(rep => rep.GetSingle<User>(It.IsAny<Func<User, bool>>())).Returns((User)null);

            var userService = new UserService(mock.Object);

            Assert.AreEqual(null, userService.GetUserById(0));
        }

        [TestMethod]
        public void GetUserExistByEmailTest()
        {
            var mock = new Mock<IRepository>();

            mock.Setup(rep => rep.GetSingle<User>(It.IsAny<Func<User, bool>>())).Returns((User)null);

            var userService = new UserService(mock.Object);

            Assert.AreEqual(null, userService.GetUserExistByEmail("e1@e.e"));
        }

        [TestMethod]
        public void GetUserExistByEmailNullTest()
        {
            var mock = new Mock<IRepository>();

            mock.Setup(rep => rep.GetSingle<User>(It.IsAny<Func<User, bool>>())).Returns((User)null);

            var userService = new UserService(mock.Object);

            Assert.AreEqual(null, userService.GetUserExistByEmail(""));
        }

        [TestMethod]
        public void GetUserIdByEmailTest()
        {
            var mock = new Mock<IRepository>();

            mock.Setup(rep => rep.GetSingle<User>(It.IsAny<Func<User, bool>>())).Returns(_users[0]);

            var userService = new UserService(mock.Object);

            Assert.AreEqual(1, userService.GetUserIdByEmail("e1@e.e"));
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void GetUserIdByEmailNullRefExceptionTest()
        {
            var mock = new Mock<IRepository>();

            mock.Setup(rep => rep.GetSingle<User>(It.IsAny<Func<User, bool>>())).Returns((User)null);

            var userService = new UserService(mock.Object);

            userService.GetUserIdByEmail("");
        }

        [TestMethod]
        public void GetUsersEmailsByEmailSubstringExceptCurrentUserTest()
        {
            var mock = new Mock<IRepository>();

            mock.Setup(rep => rep.Get<User>(It.IsAny<Func<User, bool>>())).Returns((List<User>) _users);
            var currentUserEmail = "ee2@e.e";
            var expectedResult = _users.Select(u => u.Email).Where(email => email != currentUserEmail).ToList();

            var userService = new UserService(mock.Object);

            var actualResult = userService.GetUsersEmailsByEmailSubstringExceptCurrentUser("", currentUserEmail);

            Assert.IsTrue(expectedResult.SequenceEqual(actualResult));
        }

        [TestMethod]
        public void AddUserTest()
        {
            var mock = new Mock<IRepository>();

            mock.Setup(rep => rep.Insert<User>(It.IsAny<User>())).Returns(_users[0]);

            var userService = new UserService(mock.Object);

            Assert.AreEqual(_users[0], userService.AddUser(_users[0]));
        }

        [TestMethod]
        public void UserExistsByEmailTrueTest()
        {
            var mock = new Mock<IRepository>();

            mock.Setup(rep => rep.Exists<User>(It.IsAny<Func<User, bool>>())).Returns(true);

            var userService = new UserService(mock.Object);

            Assert.IsTrue(userService.UserExistsByEmail("e1@e.e"));
        }

        [TestMethod]
        public void UserExistsByEmailFalseTest()
        {
            var mock = new Mock<IRepository>();

            mock.Setup(rep => rep.Exists<User>(It.IsAny<Func<User, bool>>())).Returns(false);

            var userService = new UserService(mock.Object);

            Assert.IsFalse(userService.UserExistsByEmail(""));
        }
    }
}
