using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AddressMaintenance.Service;
using AddressMaintenance.Model;
using System.ComponentModel;

namespace AddressMaintenance.Test
{
    [TestClass]
    public class AddressMaintenanceServerTests
    {
        [TestMethod]
        public void ExampleTest()
        {
            var service = new AddressMaintenanceService();
            var customers = service.GetAllCustomers(2, CustomerSortField.LastName, ListSortDirection.Ascending, "test");
            //Test paging parameters (or whatever you like)
            Assert.AreEqual(2, customers.PageSize);
            Assert.AreEqual(2, customers.CurrentPage);
            Assert.AreEqual(3, customers.TotalPages);
            Assert.AreEqual(5, customers.TotalCount);
            Assert.IsTrue(customers.IsNext);
            Assert.IsTrue(customers.IsPrevious);
        }
    }
}
