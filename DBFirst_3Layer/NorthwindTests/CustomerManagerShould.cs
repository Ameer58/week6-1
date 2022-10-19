using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Moq;
using NorthwindBusiness;
using NorthwindData;
using NorthwindData.Services;
using NUnit.Framework;

namespace NorthwindTests
{
    public class CustomerManagerShould
    {
        private CustomerManager _sut;

        [Ignore("Should fail")]
        [Test]
        public void BeAbleToConstructCustomerManager()
        {
            _sut = new CustomerManager(null);
            Assert.That(_sut, Is.InstanceOf<CustomerManager>());
        }
        //Dummy
        [Test]
        public void BeAbleToConstruct_UsingMoq()
        {
            var mockObject = new Mock<ICustomerService>();
            _sut = new CustomerManager(mockObject.Object);
            Assert.That(_sut, Is.InstanceOf<CustomerManager>());
        }

        //Stub
        [Category("Happy Path")]
        [Test]
        public void ReturnTrue_WhenUpdateIsCalled_WithValidId()
        {
            //Arrange
            var mockObject = new Mock<ICustomerService>();
            var originalCustomer = new Customer
            {
                CustomerId = "MANDA"
            };
            mockObject.Setup(cs => cs.GetCustomerById("MANDA")).Returns(originalCustomer);

            _sut = new CustomerManager(mockObject.Object);
            //Act
            var result = _sut.Update("MANDA", It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>());
            Assert.That(result);
        }

        [Category("Happy Path")]
        [Test]
        public void UpdateSelectedCustomer_WhenUpdateIsCalled_WithValidId()
        {
            //Arrange
            var mockObject = new Mock<ICustomerService>();
            var originalCustomer = new Customer
            {
                CustomerId = "MANDA",
                ContactName = "Nish Mandal",
                CompanyName = "Sparta Global",
                City = "Birmnigham"
            };
            mockObject.Setup(cs => cs.GetCustomerById("MANDA")).Returns(originalCustomer);

            _sut = new CustomerManager(mockObject.Object);
            //Act
            var result = _sut.Update("MANDA", "Nish Mandal", "UK", "London", null);
            Assert.That(_sut.SelectedCustomer.ContactName, Is.EqualTo("Nish Mandal"));
            Assert.That(_sut.SelectedCustomer.Country, Is.EqualTo("UK"));
            Assert.That(_sut.SelectedCustomer.City, Is.EqualTo("London"));
        }

        [Category("Sad Path")]
        [Test]
        public void RetursFalse_WhenUpdateIsCalled_WithInvalidId()
        {
            //Arrange
            var mockObject = new Mock<ICustomerService>();
            mockObject.Setup(cs => cs.GetCustomerById(It.IsAny<string>())).Returns((Customer)null);
            _sut = new CustomerManager(mockObject.Object);
            //Act
            var result = _sut.Update(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>());
            //Assert
            Assert.That(result, Is.False);
        }

        [Category("Sad Path")]
        [Test]
        public void NotChangeTheSelectedCustomer_WhenUpdateIsCalled_WithValidId()
        {
            var mockObject = new Mock<ICustomerService>();
            mockObject.Setup(cs => cs.GetCustomerById("MANDA")).Returns((Customer)null);
            var originalCustomer = new Customer
            {
                CustomerId = "MANDA",
                ContactName = "Nish Mandal",
                CompanyName = "Sparta Global",
                City = "Birmingham"
            };
            _sut = new CustomerManager(mockObject.Object);
            _sut.SetSelectedCustomer(originalCustomer);

            var result = _sut.Update("MANDA", "Nish Mandal", "UK", "London", null);
            Assert.That(_sut.SelectedCustomer.ContactName, Is.EqualTo("Nish Mandal"));
            Assert.That(_sut.SelectedCustomer.Country, Is.EqualTo(null));
            Assert.That(_sut.SelectedCustomer.City, Is.EqualTo("Birmingham"));
        }

        [Test]
        public void ReturnTrue_WhenDeleteIsCalledWithValidId()
        {
            // Arrange
            var mockCustomerService = new Mock<ICustomerService>();
            var customer = new Customer()
            {
                CustomerId = "ROCK",
            };
            mockCustomerService.Setup(cs => cs.GetCustomerById("ROCK")).Returns(customer);
            _sut = new CustomerManager(mockCustomerService.Object);
            // Act
            var result = _sut.Delete("ROCK");

            // Assert
            Assert.That(result);
        }

        [Test]
        public void SetSelectedCustomerToNull_WhenDeleteIsCalledWithValidId()
        {
            // Arrange
            var mockCustomerService = new Mock<ICustomerService>();
            var customer = new Customer()
            {
                CustomerId = "ROCK",
            };
            _sut.SelectedCustomer = customer;
            mockCustomerService.Setup(cs => cs.GetCustomerById("ROCK")).Returns(customer);
            _sut = new CustomerManager(mockCustomerService.Object);
            // Act
            var result = _sut.Delete("ROCK");

            // Assert
            Assert.That(_sut.SelectedCustomer, Is.Null);
        }

        [Test]
        public void ReturnFalse_WhenDeleteIsCalled_WithInvalidId()
        {
            // Arrange
            var mockCustomerService = new Mock<ICustomerService>();

            mockCustomerService.Setup(cs => cs.GetCustomerById("ROCK")).Returns((Customer)null);
            _sut = new CustomerManager(mockCustomerService.Object);
            // Act
            var result = _sut.Delete("ROCK");

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void NotChangeTheSelectedCustomer_WhenDeleteIsCalled_WithInvalidId()
        {
            // Arrange
            var mockCustomerService = new Mock<ICustomerService>();

            mockCustomerService.Setup(cs => cs.GetCustomerById("ROCK")).Returns((Customer)null);

            var originalCustomer = new Customer()
            {
                CustomerId = "ROCK",
                ContactName = "Rocky Raccoon",
                CompanyName = "Zoo UK",
                City = "Telford"

            };

            _sut = new CustomerManager(mockCustomerService.Object);
            _sut.SelectedCustomer = originalCustomer;
            // Act
            _sut.Delete("ROCK");

            // Assert that SelectedCustomer is unchanged
            Assert.That(_sut.SelectedCustomer.ContactName, Is.EqualTo("Rocky Raccoon"));
            Assert.That(_sut.SelectedCustomer.CompanyName, Is.EqualTo("Zoo UK"));
            Assert.That(_sut.SelectedCustomer.Country, Is.EqualTo(null));
            Assert.That(_sut.SelectedCustomer.City, Is.EqualTo("Telford"));
        }

        //Using Moq to throw exceptions
        [Category("Sad Path")]
        [Category("Update")]
        [Test]
        public void ReturnFalse_WhenUpdateIsCalled_AndDatabaseThrowsException()
        {
            //Arrange
            var mockCustomerService = new Mock<ICustomerService>();
            mockCustomerService.Setup(cs => cs.GetCustomerById(It.IsAny<string>())).Returns(new Customer());
            mockCustomerService.Setup(cs => cs.SaveCustomerChanges()).Throws<DbUpdateConcurrencyException>();
            _sut = new CustomerManager(mockCustomerService.Object);
            //Act
            var result = _sut.Update(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>());
            Assert.That(result, Is.False);
            //Also test the SelectedCustomer property has not changed either
        }
        //Up to now - we have been doing state-based testing (i.e. what is the state of the system after a given action


        //Behaviour based testing
        
        [Test]
        public void CallSaveCustomerChanges_WhenUpdateIsCalled_WithCalidId()
        {
            var mockCustomerService = new Mock<ICustomerService>();
            mockCustomerService.Setup(cs => cs.GetCustomerById(It.IsAny<string>())).Returns(new Customer());
            _sut = new CustomerManager(mockCustomerService.Object);
            var result = _sut.Update(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>());
            //Assert
            mockCustomerService.Verify(cs => cs.SaveCustomerChanges(), Times.Once);
            //mockCustomerService.Verify(cs => cs.SaveCustomerChanges(), Times.Exactly(1));
            //mockCustomerService.Verify(cs => cs.SaveCustomerChanges(), Times.AtMostOnce);
            //mockCustomerService.Verify(cs => cs.GetCustomerList(), Times.Never);
        }

        [Test]
        public void LetsSeeWhatHappens_WhenUpdateIsCalled_IfAllInvocations_ArentSetup()
        {
            var mockCustomerService = new Mock<ICustomerService>(MockBehavior.Strict);
            mockCustomerService.Setup(cs => cs.GetCustomerById(It.IsAny<string>())).Returns(new Customer());
            mockCustomerService.Setup(cs => cs.SaveCustomerChanges());
            _sut = new CustomerManager(mockCustomerService.Object);
            var result = _sut.Update(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>());
            Assert.That(result);
        }

    }
}
