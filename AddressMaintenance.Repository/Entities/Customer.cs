using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace AddressMaintenance.Repository.Entities
{
    public class Customer
    {

        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(300)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(300)]
        public string LastName { get; set; }

        public IList<Address> Addresses { get; set; }

    }
}
