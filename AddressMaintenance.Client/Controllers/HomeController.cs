using AddressMaintenance.Client.Helpers;
using AddressMaintenance.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AddressMaintenance.Client.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {

            return View();            
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult CustomerData(int pageNumber, string orderBy, bool isDesc, string searchTerm)
        {
            var customerSortField = (orderBy == "firstname") ? CustomerSortField.FirstName : CustomerSortField.LastName;
            var listSortDirection = (isDesc) ? ListSortDirection.Descending : ListSortDirection.Ascending;
            var customersPagedList = AddressMaintenanceChannel.Instance.Service.GetAllCustomers(
                pageNumber, 
                customerSortField,
                listSortDirection,
                searchTerm);

            return PartialView(customersPagedList);
        }
    }
}