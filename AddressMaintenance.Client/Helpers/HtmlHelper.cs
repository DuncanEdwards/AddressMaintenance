using AddressMaintenance.Model;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace AddressMaintenance.Client.Helpers
{
    public class MyHtmlHelper
    {
        static public bool IsColumnSelected(
            CustomerSortField customerSortField,
            CustomerSortField expectedSortField,
            ListSortDirection listSortDirection,
            ListSortDirection expectedListSortDirectio)
        {
            return ((customerSortField != expectedSortField) || (listSortDirection == expectedListSortDirectio));
        }

    }
}