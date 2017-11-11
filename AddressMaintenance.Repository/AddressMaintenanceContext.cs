using AddressMaintenance.Repository.Entities;
using CsvHelper;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddressMaintenance.Repository
{
    public class AddressMaintenanceContext : DbContext
    {

        public DbSet<Customer> Customers { get; set; }

        public DbSet<Address> Addresses { get; set; }

        public AddressMaintenanceContext()
        {


            if (Customers.Count() == 0)
            {
                var addresses = new List<Address>();
                using (TextReader fileReader = File.OpenText(@"Test Data/address-data.csv"))
                {
                    var csv = new CsvReader(fileReader);
                    csv.Configuration.HasHeaderRecord = true;
                    //Skip header
                    csv.Read();
                    while (csv.Read())
                    {
                        var address = new Address();
                        address.AddressLine1 = csv.GetField<string>(0) + " " + csv.GetField<string>(1);
                        address.AddressLine2 = csv.GetField<string>(2);
                        address.AddressLine3 = csv.GetField<string>(3);
                        address.PostCode = csv.GetField<string>(4);
                        addresses.Add(address);
                    }
                }

                //Create test data for first usage
                using (TextReader fileReader = File.OpenText(@"Test Data/customer-test-data.csv"))
                {
                    var csv = new CsvReader(fileReader);
                    csv.Configuration.HasHeaderRecord = true;
                    csv.Configuration.MissingFieldFound = null;
                    csv.Configuration.HeaderValidated = null;
                    var customers = csv.GetRecords<Customer>();

                    int index = 0;
                    foreach (var customer in customers) {
                        var address = addresses[index++];
                        Addresses.Add(address);
                        customer.Addresses = new List<Address>() { address };
                        Customers.Add(customer);
                    }
                    SaveChanges();
                } 
            }

            
        }


    }
}
