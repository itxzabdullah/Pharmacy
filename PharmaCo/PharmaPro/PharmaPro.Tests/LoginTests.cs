using NUnit.Framework;
using Moq;
using BusinessLogicLayer;
using DataAccessLayer;
using DomainModels;

namespace PharmaPro.Tests
{
    [TestFixture]
    public class LoginTests
    {
        private Mock<IDal> dalMock;
        private BLL bll;

        [SetUp]
        public void Setup()
        {
            // Create a mock DAL and inject into BLL
            dalMock = new Mock<IDal>();
            //bll = new BLL(dalMock.Object);
        }

        [Test]
        public void Login_ValidCredentials_ReturnsUser()
        {
            // Arrange: fake DAL says credentials are valid
            dalMock.Setup(d => d.VerifyUser("ali", "password123", "Customer"))
                   .Returns(true);

            // Act
            var user = bll.GetUser("ali", "password123", "Customer");

            // Assert
            Assert.That(user, Is.Not.Null);
            Assert.That(user.UserType.ToLower(), Is.EqualTo("Customer"));
        }

        [Test]
        public void Login_InvalidCredentials_ReturnsNull()
        {
            // Arrange: fake DAL says credentials are invalid
            dalMock.Setup(d => d.VerifyUser("wrong", "wrong", "customer"))
                   .Returns(false);

            // Act
            var user = bll.GetUser("wrong", "wrong", "customer");

            // Assert
            Assert.That(user, Is.Null);
        }

        [Test]
        public void Login_EmptyUsername_ReturnsNull()
        {
            // Arrange: fake DAL says empty username is invalid
            dalMock.Setup(d => d.VerifyUser("", "password123", "customer"))
                   .Returns(false);

            // Act
            var user = bll.GetUser("", "password123", "customer");

            // Assert
            Assert.That(user, Is.Null);
        }

        [Test]
        public void Login_EmptyPassword_ReturnsNull()
        {
            // Arrange: fake DAL says empty password is invalid
            dalMock.Setup(d => d.VerifyUser("customer1", "", "customer"))
                   .Returns(false);

            // Act
            var user = bll.GetUser("customer1", "", "customer");

            // Assert
            Assert.That(user, Is.Null);
        }
    }
}