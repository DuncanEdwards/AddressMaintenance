using AddressMaintenance.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using AddressMaintenance.Repository.Entities;
using AddressMaintenance.Repository;
using Castle.Windsor;
using Castle.Windsor.Configuration.Interpreters;
using AddressMaintenance.Model;
using AutoMapper;
using DevTrends.WCFDataAnnotations;

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

        public IList<CustomerDto> GetAllCustomers()
        {
            var customers = _repository.GetAllCustomers();
            return Mapper.Map<List<CustomerDto>>(customers);
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
