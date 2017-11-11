using System;
using System.ComponentModel.DataAnnotations;

namespace AddressMaintenance.Repository.Entities
{
    public class Address
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(300)]
        public string AddressLine1 { get; set; }

        [MaxLength(300)]
        public string AddressLine2 { get; set; }

        [MaxLength(300)]
        public string AddressLine3 { get; set; }

        [Required]
        [MaxLength(8)]
        public string PostCode { get; set; }

        public DateTime? ValidUntil { get; set; }

        public DateTime? ValidFrom { get; set; }

    }
}
