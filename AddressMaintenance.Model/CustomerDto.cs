using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;

namespace AddressMaintenance.Model
{
    [DataContract]
    public class CustomerDto
    {
        [DataMember]
        public Guid Id { get; set; }

        [DataMember]
        [Required]
        public string FirstName { get; set; }

        [DataMember]
        [Required]
        public string LastName { get; set; }

        [DataMember]
        public IList<AddressDto> Addresses { get; set; }

        public AddressDto CurrentAddress
        {
            get
            {
                if (Addresses.Count == 1)
                {
                    return Addresses.First();
                }
                return Addresses.FirstOrDefault(a => a.ValidFrom < DateTime.Now);
            }
        }

    }
}
