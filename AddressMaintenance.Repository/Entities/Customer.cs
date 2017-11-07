using System;
using System.ComponentModel.DataAnnotations;

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

    }
}
