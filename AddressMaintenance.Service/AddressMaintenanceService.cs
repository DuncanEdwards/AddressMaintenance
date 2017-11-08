using AddressMaintenance.Interfaces;
using AddressMaintenance.Model;
using AddressMaintenance.Model.Paging;
using AddressMaintenance.Repository;
using AddressMaintenance.Repository.Entities;
using AutoMapper;
using Castle.Windsor;
using Castle.Windsor.Configuration.Interpreters;
using DevTrends.WCFDataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;

namespace AddressMaintenance.Service
{

    [ValidateDataAnnotationsBehavior]
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in both code and config file together.
    public class AddressMaintenanceService : IAddressMaintenanceService
    {
        readonly private IAddressMaintenanceRepository _repository;

        public AddressMaintenanceService()
        {
            var container = new WindsorContainer(new XmlInterpreter());
            _repository = container.Resolve<IAddressMaintenanceRepository>();

            //Register AutoMapper
            Mapper.Initialize(config => {
                config.CreateMap<Customer, CustomerDto>();
                config.CreateMap<CustomerDto, Customer>();
            });
        }

        public Guid AddCustomer(CustomerDto customer)
        {
            var customerEntity = Mapper.Map<Customer>(customer);
            return _repository.AddCustomer(customerEntity);
        }

        public void UpdateCustomer(CustomerDto customer)
        {
            var customerEntity = Mapper.Map<Customer>(customer);
            _repository.UpdateCustomer(customerEntity);
        }

        public CustomerPagedList GetAllCustomers(
            int pageNumber, 
            CustomerSortField customerSortField,
            ListSortDirection listSortDirection,
            string searchQuery)
        {
            var customers = _repository.GetAllCustomers(
                pageNumber, 
                Convert.ToInt32(ConfigurationManager.AppSettings["CustomerPageSize"]), 
                customerSortField,
                listSortDirection,
                searchQuery);
            //Construct and return paging of write types
            var customerPagingList = customers.GetCustomerPagingList();
            customerPagingList.Customers = Mapper.Map<List<CustomerDto>>(customers);
            return customerPagingList;
        }

        public CustomerDto GetCustomer(Guid id)
        {
            return Mapper.Map<CustomerDto>(_repository.GetCustomer(id));
        }

        public void RemoveCustomer(Guid id)
        {
            _repository.RemoveCustomer(id);
        }
    }
}
