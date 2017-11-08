using AddressMaintenance.Model;
using AddressMaintenance.Model.Paging;
using AddressMaintenance.Repository.Entities;
using System;
using System.ComponentModel;

namespace AddressMaintenance.Repository
{
    public interface IAddressMaintenanceRepository
    {

        PagedList<Customer> GetAllCustomers(
            int pageNumber, 
            int pageSize, 
            CustomerSortField customerSortField, 
            ListSortDirection listSortDirection,
            string searchQuery);

        Customer GetCustomer(Guid id);

        Guid AddCustomer(Customer customer);

        void UpdateCustomer(Customer customer);

        void RemoveCustomer(Guid id);

    }
}
