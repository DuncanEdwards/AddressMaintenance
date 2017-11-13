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

        }


    }
}
