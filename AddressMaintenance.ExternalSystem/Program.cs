using AddressMaintenance.Interfaces;
using AddressMaintenance.Model;
using System;
using System.Configuration;
using System.ServiceModel;

namespace AddressMaintenance.ExternalSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            BasicHttpBinding binding = new BasicHttpBinding();

            //Create EndPoint address  
            EndpointAddress endpointAddress = new EndpointAddress(ConfigurationManager.AppSettings["baseAddress"]);

            //Pass Binding and EndPoint address to ChannelFactory  
            var channelFactory = new ChannelFactory<IAddressMaintenanceService>(binding, endpointAddress);

            //Now create the new channel as below  
            IAddressMaintenanceService channel = channelFactory.CreateChannel();

            CustomerPagedList customerPagedList;
            int currentPage = 1;
            //Call the service method on this channel as below
            do
            {
                
                customerPagedList = channel.GetAllCustomers(currentPage++);
                foreach (var customer in customerPagedList.Customers)
                {
                    Console.WriteLine(customer.ToString() + "," + customer.CurrentAddress.ToString());
                }
            } while (customerPagedList.CurrentPage < customerPagedList.TotalPages);

        }
    }
}
