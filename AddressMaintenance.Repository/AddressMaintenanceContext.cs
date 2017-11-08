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
            }

            
        }


    }
}
