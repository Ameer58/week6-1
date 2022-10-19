using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }
}
