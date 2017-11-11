using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;

namespace AddressMaintenance.Model
{
    [DataContract]
    public class AddressDto
    {

        [DataMember]
        public Guid Id { get; set; } = Guid.NewGuid();

        [DataMember]
        [Required]
        public string AddressLine1 { get; set; }

        [DataMember]
        public string AddressLine2 { get; set; }

        [DataMember]
        public string AddressLine3 { get; set; }

        [DataMember]
        public string PostCode { get; set; }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder(AddressLine1);
            if (!String.IsNullOrEmpty(AddressLine2))
            {
                stringBuilder.Append(AddressLine2);
            }
            if (!String.IsNullOrEmpty(AddressLine3))
            {
                stringBuilder.Append(AddressLine3);
            }
            return stringBuilder.ToString();
        }

    }

}
