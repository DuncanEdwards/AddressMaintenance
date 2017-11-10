using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AddressMaintenance.Model
{
    [DataContract]
    public class CustomerPagedList
    {
        [DataMember]
        public IList<CustomerDto> Customers { get; set; }

        [DataMember]
        public int CurrentPage { get; set; }

        [DataMember]
        public int TotalPages { get; set; }

        [DataMember]
        public int PageSize { get; set; }

        [DataMember]
        public int TotalCount { get; set; }

        public bool IsPrevious
        {
            get
            {
                return CurrentPage > 1;
            }
        }

        public bool IsNext
        {
            get
            {
                return CurrentPage < TotalPages;
            }
        }

    }
}
