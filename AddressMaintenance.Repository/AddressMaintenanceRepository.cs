using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AddressMaintenance.Repository.Entities;
using System.ServiceModel;

namespace AddressMaintenance.Repository
{
    public class AddressMaintenanceRepository : IAddressMaintenanceRepository
    {

        #region Customer

        public List<Customer> GetAllCustomers()
        {
            using (var context = new AddressMaintenanceContext())
            {
                return context.Customers.ToList();
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
