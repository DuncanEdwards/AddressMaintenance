using AddressMaintenance.Model;
using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace AddressMaintenance.Interfaces
{
    [ServiceContract]
    public interface IAddressMaintenanceService
    {

        #region Customer

        [OperationContract]
        IList<CustomerDto> GetAllCustomers();

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
