using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

    }
}
