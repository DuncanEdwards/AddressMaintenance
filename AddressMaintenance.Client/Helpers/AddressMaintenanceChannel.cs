using AddressMaintenance.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.ServiceModel;
using System.Web;
using System.Web.Configuration;

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
            EndpointAddress endpointAddress = new EndpointAddress(WebConfigurationManager.AppSettings["baseAddress"]);

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