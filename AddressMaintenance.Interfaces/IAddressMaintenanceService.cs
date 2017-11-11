using AddressMaintenance.Model;
using System;
using System.ComponentModel;
using System.ServiceModel;

namespace AddressMaintenance.Interfaces
{
    [ServiceContract]
    public interface IAddressMaintenanceService
    {

        #region Customer

        [OperationContract]
        CustomerPagedList GetAllCustomers(
            int pageNumber = 1, 
            CustomerSortField customerSortField = CustomerSortField.LastName,
            ListSortDirection listSortDirection = ListSortDirection.Ascending,
            string SearchQuery = "");

        [OperationContract]
        CustomerDto GetCustomer(Guid id);

        [OperationContract]
        Guid AddCustomer(CustomerDto customer);

        [OperationContract]
        void UpdateCustomer(CustomerDto customer);

        [OperationContract]
        void RemoveCustomer(Guid id);

        #endregion

    }
}
