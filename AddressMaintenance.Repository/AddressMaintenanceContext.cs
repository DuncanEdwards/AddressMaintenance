using AddressMaintenance.Repository.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
                Customers.Add(new Customer() { FirstName = "Duncan", LastName = "Edwards" });
            }

            SaveChanges();
        }


    }
}
