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
                //Create test data for first usage
                using (TextReader fileReader = File.OpenText(@"Test Data/customer-test-data.csv"))
                {
                    var csv = new CsvReader(fileReader);
                    csv.Configuration.HasHeaderRecord = true;
                    csv.Configuration.MissingFieldFound = null;
                    csv.Configuration.HeaderValidated = null;
                    var customers = csv.GetRecords<Customer>();
                    Customers.AddRange(customers);
                    SaveChanges();
                } 
            } else
            {
                var address = new Address { AddressLine1 = "64a Lascotts Road", AddressLine2 = "London, London", PostCode = "N22 8JN" };
                var customer = new Customer() { FirstName = "Duncan", LastName = "Edwards", Addresses = new List<Address>() { address } };
                Addresses.Add(address);
                Customers.Add(customer);
                SaveChanges();
            }

            
        }


    }
}
