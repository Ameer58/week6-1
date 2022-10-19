using Microsoft.EntityFrameworkCore;
using NorthwindData;
using NorthwindData.Services;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NorthwindTests
{
    public class CustomerServiceTests
    {
        private CustomerService _sut;
        private NorthwindContext _context;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var options = new DbContextOptionsBuilder<NorthwindContext>()
                .UseInMemoryDatabase(databaseName: "Example_DB")
                .Options;
            _context = new NorthwindContext(options);
            _sut = new CustomerService(_context);

            //Seed the database
            _sut.CreateCustomer(new Customer { CustomerId = "PHILL", ContactName = "Philip Windridge", CompanyName = "Sparta Global", City = "Birmingham"});
            _sut.CreateCustomer(new Customer { CustomerId = "MANDA", ContactName = "Nish Mandal", CompanyName = "Sparta Global", City = "Birmingham"});
        }

        [Test]
        public void GivenAValidId_CorrectCustomerIsReturned()
        {
            var result = _sut.GetCustomerById("PHILL");
            Assert.That(result, Is.TypeOf<Customer>());
            Assert.That(result.ContactName, Is.EqualTo("Philip Windridge"));
            Assert.That(result.CompanyName, Is.EqualTo("Sparta Global"));
            Assert.That(result.City, Is.EqualTo("Birmingham"));
        }

        [Test]
        public void GivenANewCustomer_CreateCustomerAddsItToTheDatabase()
        {
            //Arrange
            var numberOfCustomersBefore = _context.Customers.Count();
            var newCustomer = new Customer
            {
                CustomerId = "ODELL",
                ContactName = "Max Odell",
                CompanyName = "Sparta Global",
                City = "Surrey"
            };

            //Act
            _sut.CreateCustomer(newCustomer);
            var numberofCustomersAfter = _context.Customers.Count();
            var result = _sut.GetCustomerById("ODELL");

            //Assert
            Assert.That(numberOfCustomersBefore + 1, Is.EqualTo(numberofCustomersAfter));
            //Assert.That(result, Is.TypeOf<Customer>());
            //Assert.That(result.ContactName, Is.EqualTo("Max Odell"));
            //Assert.That(result.CompanyName, Is.EqualTo("Sparta Global"));
            //Assert.That(result.City, Is.EqualTo("Surrey"));
            Assert.That(newCustomer, Is.EqualTo(result));

            //Clean up
            _context.Customers.Remove(newCustomer);
            _context.SaveChanges();
        }

        [Test]
        public void GetCustomerList_ReturnsAllTheCustomers()
        {
            // Act
            var result = _sut.GetCustomerList();
            // Assert
            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result, Is.InstanceOf<List<Customer>>());
        }

        [Test]
        public void GivenACustomer_Remove_RemovesThemFromTheDatabase()
        {
            // Arrange
            var newCustomer = new Customer
            {
                CustomerId = "MANDA",
                ContactName = "Nish Mandal",
                CompanyName = "Sparta Global",
                City = "Birmingham"
            };
            _context.Add(newCustomer);
            _context.SaveChanges();

            var numberOfCustomersBefore = _context.Customers.Count();

            // Act
            _sut.RemoveCustomer(newCustomer);
            // Assert
            Assert.AreEqual(numberOfCustomersBefore - 1, _context.Customers.Count());
            var customerInDb = _context.Customers.Find("MANDA");
            Assert.That(customerInDb, Is.Null);
        }

    }
}
