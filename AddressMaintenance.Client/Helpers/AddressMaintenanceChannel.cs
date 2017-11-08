using AddressMaintenance.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;

namespace AddressMaintenance.Client.Helpers
{
    public class AddressMaintenanceChannel
    {
        private static AddressMaintenanceChannel instance = null;
        private static readonly object padlock = new object();

        public IAddressMaintenanceService Service { get; set; }

        AddressMaintenanceChannel()
        {
        }

        private void CreateChannel()
        {
            BasicHttpBinding binding = new BasicHttpBinding();

            //Create EndPoint address  
            EndpointAddress endpointAddress = new EndpointAddress("http://localhost:8733/Design_Time_Addresses/AddressMaintenance.Service/AddressMaintenanceService/");

            //Pass Binding and EndPoint address to ChannelFactory  
            var channelFactory = new ChannelFactory<IAddressMaintenanceService>(binding, endpointAddress);

            //Now create the new channel as below  
            Service = channelFactory.CreateChannel();
        }

        public static AddressMaintenanceChannel Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new AddressMaintenanceChannel();
                        instance.CreateChannel();
                    }
                    return instance;
                }
            }
        }

    }
}