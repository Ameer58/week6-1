
using NUnit.Framework;
using NorthwindBusiness;
using NorthwindData;
using System.Linq;
using System.Collections.Generic;

namespace NorthwindTests
{
    public class CustomerTests
    {
        CustomerManager _customerManager;
        IQueryable<Customer> _selectedCustomers;

        [SetUp]
        public void Setup()
        {
            _customerManager = new CustomerManager();
            // remove test entry in DB if present
            using (var db = new NorthwindContext())
            {
                _selectedCustomers =
                from c in db.Customers
                where c.CustomerId == "MANDA"
                select c;

                db.Customers.RemoveRange(_selectedCustomers);
                db.SaveChanges();
            }
        }

        [Test]
        public void WhenANewCustomerIsAdded_TheNumberOfCustemersIncreasesBy1()
        {
            using (var db = new NorthwindContext())
            {
                var numberOfCustomersBefore = db.Customers.Count();
                _customerManager.Create("MANDA", "Nish Mandal", "Sparta Global");
                var numberOfCustomersAfter = db.Customers.Count();

                Assert.That(numberOfCustomersBefore + 1, Is.EqualTo(numberOfCustomersAfter));
            }
        }

        [Test]
        public void WhenANewCustomerIsAdded_TheirDetailsAreCorrect()
        {
            using (var db = new NorthwindContext())
            {
                _customerManager.Create("MANDA", "Nish Mandal", "Sparta Global");
                var selectedCustomer = db.Customers.Find("MANDA");
                Assert.That(selectedCustomer.ContactName, Is.EqualTo("Nish Mandal"));
                Assert.That(selectedCustomer.CompanyName, Is.EqualTo("Sparta Global"));
            }
        }

        [Test]
        public void WhenACustomersDetailsAreChanged_TheDatabaseIsUpdated()
        {
            using (var db = new NorthwindContext())
            {
                _customerManager.Create("MANDA", "Nish Mandal", "Sparta Global", "Paris");
                _customerManager.Update("MANDA", "Nish Mandal", "UK", "Birmingham", null);
                var updatedCustomer = db.Customers.Find("MANDA");
                Assert.That(updatedCustomer.City, Is.EqualTo("Birmingham"));
            }
        }

        [Test]
        public void WhenACustomerIsUpdated_SelectedCustomerIsUpdated()
        {
            _customerManager.Create("MANDA", "Nish Mandal", "Sparta Global", "Paris");
            _customerManager.Update("MANDA", "Nish Mandal", "UK", "Birmingham", null);
            Assert.That(_customerManager.SelectedCustomer.City, Is.EqualTo("Birmingham"));
        }

        [Test]
        public void WhenACustomerIsNotInTheDatabase_Update_ReturnsFalse()
        {
            using (var db = new NorthwindContext())
            {
                var result = _customerManager.Update("MANDA", contactName: "Nish Mandal", country: "UK", city: "Birmingham", postcode: "AB2 2DE");
                Assert.That(result, Is.False);
            }
        }

        [Test]
        public void WhenACustomerIsRemoved_TheNumberOfCustomersDecreasesBy1()
        {
            using (var db = new NorthwindContext())
            {
                var newCustomer = new Customer()
                {
                    CustomerId = "MANDA",
                    ContactName = "Nish Mandal",
                    CompanyName = "Sparta Global"
                };

                db.Customers.Add(newCustomer);
                db.SaveChanges();
                var numberOfCustomersBefore = db.Customers.Count();
                _customerManager.Delete("MANDA");
                var numberOfCustomersAfter = db.Customers.Count();
                Assert.That(numberOfCustomersBefore - 1, Is.EqualTo(numberOfCustomersAfter));
            }
        }

        [Test]
        public void WhenACustomerIsRemoved_TheyAreNoLongerInTheDatabase()
        {
            using (var db = new NorthwindContext())
            {
                var newCustomer = new Customer()
                {
                    CustomerId = "MANDA",
                    ContactName = "Nish Mandal",
                    CompanyName = "Sparta Global"
                };

                db.Customers.Add(newCustomer);
                db.SaveChanges();
                var numberOfCustomersBefore = db.Customers.Count();
                _customerManager.Delete("MANDA");
                var numberOfCustomersAfter = db.Customers.Count();
                Assert.AreEqual(numberOfCustomersBefore - 1, numberOfCustomersAfter);
            }
        }

        [TearDown]
        public void TearDown()
        {
            using (var db = new NorthwindContext())
            {
                var selectedCustomers =
                from c in db.Customers
                where c.CustomerId == "MANDA"
                select c;

                db.Customers.RemoveRange(selectedCustomers);
                db.SaveChanges();
            }
        }
    }
}