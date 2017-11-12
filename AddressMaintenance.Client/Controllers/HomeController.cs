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

        public ActionResult EditCustomer(string id)
        {
            CustomerDto customerDto = null;
            if (!String.IsNullOrEmpty(id))
            {
                customerDto = AddressMaintenanceChannel.Instance.Service.GetCustomer(Guid.Parse(id));
            }
            return View(customerDto);
        }

        public ActionResult AddCustomer(FormCollection form)
        {
            var customerDto = new CustomerDto() {
                FirstName = form["firstname"],
                LastName = form["lastname"]
            };
            var addressDto = new AddressDto()
            {
                AddressLine1 = form["addressline1"],
                AddressLine2 = form["addressline2"],
                AddressLine3 = form["addressline3"],
                PostCode = form["postcode"]
            };
            customerDto.Addresses.Add(addressDto);
            AddressMaintenanceChannel.Instance.Service.AddCustomer(customerDto);
            return Json("Customer " + customerDto.FirstName + " " + customerDto.LastName + " successfully added.");
        }

        public ActionResult SaveCustomer(FormCollection form)
        {
            //We don't pass address stuff on, so we need to refresh
            var customerDto = AddressMaintenanceChannel.Instance.Service.GetCustomer(Guid.Parse(form["customerid"]));
            if (Convert.ToBoolean(form["ischangeaddress"]))
            {
                var date = DateTime.Now;
                customerDto.CurrentAddress.ValidUntil = date;
                var address = new AddressDto()
                {
                    AddressLine1 = form["addressline1"],
                    AddressLine2 = form["addressline2"],
                    AddressLine3 = form["addressline3"],
                    PostCode = form["postcode"],
                    ValidFrom = date
                };
                var addresses = customerDto.Addresses.ToList();
                addresses.Insert(0, address);
                customerDto.Addresses = addresses;
            }
            customerDto.FirstName = form["firstname"];
            customerDto.LastName = form["lastname"];


            AddressMaintenanceChannel.Instance.Service.UpdateCustomer(customerDto);
            ViewBag.Message = "Customer " + customerDto.ToString() + " successfully updated.";
            return View("editcustomer", customerDto);
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