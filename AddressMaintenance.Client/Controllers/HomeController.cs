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
        public ActionResult Index(string message)
        {
            ViewBag.Message = message;
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

        public ActionResult EditCustomer()
        {

            return View();
        }

        [Route("customer/remove/{id}")]
        public ActionResult RemoveCustomer(string id, string firstName, string lastName)
        {
            var message = "User " + firstName + " " + lastName + " successfully removed.";
            AddressMaintenanceChannel.Instance.Service.RemoveCustomer(Guid.Parse(id));
            return RedirectToAction("Index", "Home", new { message = message });
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
            ViewBag.CustomerSortField = customerSortField;
            ViewBag.ListSortDirection = listSortDirection;
            
            return PartialView(customersPagedList);
        }
    }
}