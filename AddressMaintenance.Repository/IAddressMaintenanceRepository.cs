using AddressMaintenance.Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddressMaintenance.Repository
{
    public interface IAddressMaintenanceRepository
    {

        List<Customer> GetAllCustomers();

        Customer GetCustomer(Guid id);

        Guid AddCustomer(Customer customer);

        void UpdateCustomer(Customer customer);

        void RemoveCustomer(Guid id);

    }
}
