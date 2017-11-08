﻿using AddressMaintenance.Model;
using AddressMaintenance.Model.Paging;
using AddressMaintenance.Repository.Entities;
using System;
using System.ComponentModel;
using System.Linq;
using System.ServiceModel;

namespace AddressMaintenance.Repository
{
    public class AddressMaintenanceRepository : IAddressMaintenanceRepository
    {

        #region Customer

        public PagedList<Customer> GetAllCustomers(int pageNumber, int pageSize, CustomerSortField customerSortField, ListSortDirection listSortDirection)
        {
            using (var context = new AddressMaintenanceContext())
            {
                //TODO: Make this more heavyweight/structured when we have more to sort on
                var customersBeforePaging = (customerSortField == CustomerSortField.FirstName) ?
                    ((listSortDirection == ListSortDirection.Ascending) ? context.Customers.OrderBy(c => c.FirstName) : context.Customers.OrderByDescending(c => c.FirstName)) :
                    ((listSortDirection == ListSortDirection.Ascending) ? context.Customers.OrderBy(c => c.LastName) : context.Customers.OrderByDescending(c => c.LastName));
                return PagedList<Customer>.Create(customersBeforePaging, pageNumber, pageSize);
            }
        }


        public Customer GetCustomer(Guid id)
        {
            using (var context = new AddressMaintenanceContext())
            {
                return GetCustomerOrFault(context, id);
            }
        }

        public Guid AddCustomer(Customer customer)
        {
            using (var context = new AddressMaintenanceContext())
            {
                context.Customers.Add(customer);
                context.SaveChanges();
                return customer.Id;
            }
        }

        public void UpdateCustomer(Customer customer)
        {
            using (var context = new AddressMaintenanceContext())
            {
                var matchingCustomer = GetCustomerOrFault(context, customer.Id);
                matchingCustomer.FirstName = customer.FirstName;
                matchingCustomer.LastName = customer.LastName;
                context.SaveChanges();
            }
        }

        public void RemoveCustomer(Guid id)
        {
            using (var context = new AddressMaintenanceContext())
            {
                var matchingCustomer = GetCustomerOrFault(context, id);
                context.Customers.Remove(matchingCustomer);
                context.SaveChanges();
            }
        }

        private Customer GetCustomerOrFault(AddressMaintenanceContext context, Guid id)
        {
            var customer = context.Customers.FirstOrDefault(c => c.Id == id);
            if (customer == null)
            {
                throw new FaultException("No customer found for id=" + customer.Id);
            }
            return customer;
        }

        #endregion

    }
}