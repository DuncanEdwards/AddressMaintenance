using AddressMaintenance.Model;
using AddressMaintenance.Model.Paging;
using AddressMaintenance.Repository.Entities;
using CsvHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
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
                              u.LastName.Contains(searchQueryForWhereClause) ||
                              u.Addresses.FirstOrDefault(a => (!a.ValidUntil.HasValue)).AddressLine1.Contains((searchQueryForWhereClause)) ||
                              u.Addresses.FirstOrDefault(a => (!a.ValidUntil.HasValue)).AddressLine2.Contains((searchQueryForWhereClause)) ||
                              u.Addresses.FirstOrDefault(a => (!a.ValidUntil.HasValue)).AddressLine3.Contains((searchQueryForWhereClause)) ||
                              u.Addresses.FirstOrDefault(a => (!a.ValidUntil.HasValue)).PostCode.Contains((searchQueryForWhereClause))
                              ));
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
                    address.ValidFrom = DateTime.Now;
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

        public void CreateTestData()
        {
            using (var context = new AddressMaintenanceContext())
            {
                if (context.Customers.Count() == 0)
                {
                    var addresses = new List<Address>();
                    using (TextReader fileReader = File.OpenText(@"Test Data/address-data.csv"))
                    {
                        var csv = new CsvReader(fileReader);
                        csv.Configuration.HasHeaderRecord = true;
                        //Skip header
                        csv.Read();
                        while (csv.Read())
                        {
                            var address = new Address();
                            address.ValidFrom = DateTime.Now.Subtract(TimeSpan.FromDays(500)); //Purely for testing really
                            address.AddressLine1 = csv.GetField<string>(0) + " " + csv.GetField<string>(1);
                            address.AddressLine2 = csv.GetField<string>(2);
                            address.AddressLine3 = csv.GetField<string>(3);
                            address.PostCode = csv.GetField<string>(4);
                            addresses.Add(address);
                        }
                    }

                    //Create test data for first usage
                    using (TextReader fileReader = File.OpenText(@"Test Data/customer-test-data.csv"))
                    {
                        var csv = new CsvReader(fileReader);
                        csv.Configuration.HasHeaderRecord = true;
                        csv.Configuration.MissingFieldFound = null;
                        csv.Configuration.HeaderValidated = null;
                        var customers = csv.GetRecords<Customer>();

                        int index = 0;
                        foreach (var customer in customers)
                        {
                            var address = addresses[index++];
                            context.Addresses.Add(address);
                            customer.Addresses = new List<Address>() { address };
                            context.Customers.Add(customer);
                        }
                        context.SaveChanges();
                    }
                }
            }
        }

        #endregion

    }
}
