using AddressMaintenance.Repository.Entities;
using CsvHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddressMaintenance.Repository.Test_Data
{
    static public class TestDataExtensionMethod
    {
        static public void CreateTestData(this AddressMaintenanceContext context)
        {
            if (context.Customers.Count() == 0)
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
                        address.ValidFrom = DateTime.Now.Subtract(TimeSpan.FromDays(500)); //Purely for testing really
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
                    foreach (var customer in customers)
                    {
                        var address = addresses[index++];
                        context.Addresses.Add(address);
                        customer.Addresses = new List<Address>() { address };
                        context.Customers.Add(customer);
                    }
                    context.SaveChanges();
                }
            }
        }
    }
}
