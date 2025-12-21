using NUnit.Framework;
using Moq;
using BusinessLogicLayer;
using DataAccessLayer;
using DomainModels;
using System.Collections.Generic;

namespace PharmaPro.Tests
{
    [TestFixture]
    public class UserManagerTests
    {
        private Mock<IDal> dalMock;
        private BLL bll;

        [SetUp]
        public void Setup()
        {
            dalMock = new Mock<IDal>();
            bll = new BLL(dalMock.Object);
        }

        [Test]
        public void ViewAllUsers_ReturnsListOfUsers()
        {
            // Arrange
            var fakeUsers = new List<User>
            {
                new User { Username = "user1", UserType = "customer" },
                new User { Username = "user2", UserType = "admin" }
            };
            dalMock.Setup(d => d.ViewAllUsers()).Returns(fakeUsers);

            // Act
            var users = bll.ViewAllUsers();

            // Assert
            Assert.That(users.Count, Is.EqualTo(2));
            Assert.That(users[0].Username, Is.EqualTo("user1"));
        }

        [Test]
        public void SearchUser_WithValidIdAndName_ReturnsMatchingUsers()
        {
            // Arrange
            var fakeUsers = new List<User>
            {
                new User { UserId = 1, Username = "john" }
            };
            dalMock.Setup(d => d.SearchUser(1, "john")).Returns(fakeUsers);

            // Act
            var users = bll.SearchUser(1, "john");

            // Assert
            Assert.That(users, Is.Not.Null);
            Assert.That(users[0].Username, Is.EqualTo("john"));
        }

        [Test]
        public void AddUser_CallsDalInsert()
        {
            // Arrange
            dalMock.Setup(d => d.AddUser(It.IsAny<User>()));

            // Act
            bll.AddUser("newuser", "pass", "email@test.com", "Name", "Address", "12345", "customer");

            // Assert
            dalMock.Verify(d => d.AddUser(It.Is<User>(u => u.Username == "newuser")), Times.Once);
        }

        [Test]
        public void UpdateUser_CallsDalUpdate()
        {
            // Arrange
            dalMock.Setup(d => d.UpdateUser(It.IsAny<User>()));

            // Act
            bll.UpdateUser(1, "updated", "pass", "email@test.com", "Name", "Address", "12345", "customer");

            // Assert
            dalMock.Verify(d => d.UpdateUser(It.Is<User>(u => u.UserId == 1 && u.Username == "updated")), Times.Once);
        }

        [Test]
        public void DeleteUser_CallsDalDelete()
        {
            // Arrange
            dalMock.Setup(d => d.DeleteUser(It.IsAny<int>()));

            // Act
            bll.DeleteUser(1);

            // Assert
            dalMock.Verify(d => d.DeleteUser(1), Times.Once);
        }
    }
}