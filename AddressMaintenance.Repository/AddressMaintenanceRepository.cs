using AddressMaintenance.Model;
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

        public PagedList<Customer> GetAllCustomers(
            int pageNumber, 
            int pageSize, 
            CustomerSortField customerSortField, 
            ListSortDirection listSortDirection,
            string searchQuery)
        {
            using (var context = new AddressMaintenanceContext())
            {
                //TODO: Make this more heavyweight/structured when we have more to sort on
                IQueryable<Customer> customersBeforePaging = (customerSortField == CustomerSortField.FirstName) ?
                    ((listSortDirection == ListSortDirection.Ascending) ? context.Customers.Include("Addresses").OrderBy(c => c.FirstName) : context.Customers.Include("Addresses").OrderByDescending(c => c.FirstName)) :
                    ((listSortDirection == ListSortDirection.Ascending) ? context.Customers.Include("Addresses").OrderBy(c => c.LastName) : context.Customers.Include("Addresses").OrderByDescending(c => c.LastName));

                if (!String.IsNullOrEmpty(searchQuery))
                {
                    var searchQueryForWhereClause = searchQuery.Trim().ToLowerInvariant();
                    customersBeforePaging = customersBeforePaging
                        .Where(u =>
                            (u.FirstName.Contains(searchQueryForWhereClause) ||
                              u.LastName.Contains(searchQueryForWhereClause)));
                }


                var pagedList =  PagedList<Customer>.Create(customersBeforePaging, pageNumber, pageSize);
                foreach (var customer in pagedList)
                {
                    SortCustomerAddresses(customer);
                }
                return pagedList;
            
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
                foreach (var address in customer.Addresses.ToList())
                {
                    context.Addresses.Add(address);
                    customer.Addresses.Add(address);
                }
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
                UpdateAddresses(context, matchingCustomer, customer);
                context.SaveChanges();
            }
        }

        public void RemoveCustomer(Guid id)
        {
            using (var context = new AddressMaintenanceContext())
            {
                var matchingCustomer = GetCustomerOrFault(context, id);
                foreach (var address in matchingCustomer.Addresses.ToList())
                {
                    context.Addresses.Remove(address);
                }
                context.Customers.Remove(matchingCustomer);
                context.SaveChanges();
            }
        }

        private void UpdateAddresses(AddressMaintenanceContext context, Customer updateCustomer, Customer newCustomer)
        {
            foreach (var address in newCustomer.Addresses.ToList())
            {
                var matchingAddress = context.Addresses.FirstOrDefault(a => a.Id == address.Id);
                if (matchingAddress != null)
                {
                    //The only field that could change is a valid until field
                    matchingAddress.ValidUntil = address.ValidUntil;
                } else
                {
                     
                    context.Addresses.Add(address);
                    updateCustomer.Addresses.Add(address);
                }
            }
        }

        private void SortCustomerAddresses(Customer customer)
        {
            if (customer.Addresses.Count > 1)
            {
                //Sort address if necessary
                customer.Addresses = customer.Addresses.OrderByDescending(a => a.ValidFrom).ToList();
            }
        }

        private Customer GetCustomerOrFault(AddressMaintenanceContext context, Guid id)
        {
            var customer = context.Customers.Include("Addresses").FirstOrDefault(c => c.Id == id);
            if (customer == null)
            {
                throw new FaultException("No customer found for id=" + customer.Id);
            }
            SortCustomerAddresses(customer);
            return customer;
        }

        #endregion

    }
}
