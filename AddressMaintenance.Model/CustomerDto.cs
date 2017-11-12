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
        public Guid Id { get; set; } = Guid.NewGuid();

        [DataMember]
        [Required]
        public string FirstName { get; set; }

        [DataMember]
        [Required]
        public string LastName { get; set; }

        [DataMember]
        public IList<AddressDto> Addresses { get; set; } = new List<AddressDto>();

        public AddressDto CurrentAddress
        {
            get
            {
                return Addresses.FirstOrDefault();
            }
        }

        public override string ToString()
        {
            return FirstName + " " + LastName;
        }
    }
}
