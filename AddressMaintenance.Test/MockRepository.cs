using AddressMaintenance.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AddressMaintenance.Model.Paging;
using AddressMaintenance.Repository.Entities;
using System.ComponentModel;
using Moq;

namespace AddressMaintenance.Test
{
    class MockRepository : IAddressMaintenanceRepository
    {
        private List<Customer> GetTestData()
        {
            return new List<Customer>()
            {
                new Customer() { FirstName = "Duncan", LastName = "Edwards"},
                new Customer() { FirstName = "Daniel", LastName = "Edwards"},
                new Customer() { FirstName = "Paul", LastName = "Smith"},
                new Customer() { FirstName = "David", LastName = "Clark"},
                new Customer() { FirstName = "Debbie", LastName = "Jones"}

            };
        }

        public Guid AddCustomer(Customer customer)
        {
            throw new NotImplementedException();
        }

        public void CreateTestData()
        {
            //Do nothing in test harness
        }

        public PagedList<Customer> GetAllCustomers(int pageNumber, int pageSize, Model.CustomerSortField customerSortField, ListSortDirection listSortDirection, string searchQuery)
        {
            var testCustomers = GetTestData().Take(pageSize).ToList();
            
            var pagedList = new PagedList<Customer>(
                testCustomers,
                GetTestData().Count, 
                pageNumber, 
                pageSize);

            return pagedList;
        }

        public Customer GetCustomer(Guid id)
        {
            throw new NotImplementedException();
        }

        public void RemoveCustomer(Guid id)
        {
            throw new NotImplementedException();
        }

        public void UpdateCustomer(Customer customer)
        {
            throw new NotImplementedException();
        }
    }
}
