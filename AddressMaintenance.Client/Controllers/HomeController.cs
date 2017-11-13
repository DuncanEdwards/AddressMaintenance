using AddressMaintenance.Client.Helpers;
using AddressMaintenance.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.ServiceModel;
using System.Web;
using System.Web.Mvc;

namespace AddressMaintenance.Client.Controllers
{
    public class HomeController : Controller
    {
        /// <summary>
        /// Main customer list page
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Show edit/add customer form
        /// </summary>
        /// <param name="id">Populated for edit and null for add</param>
        /// <returns>EditCustomer form</returns>
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
            //Add new customer (ajax)
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
            bool isSuccess = true;
            var message = "Customer " + customerDto.FirstName + " " + customerDto.LastName + " successfully added.";
            try
            {
                AddressMaintenanceChannel.Instance.Service.AddCustomer(customerDto);
            } catch (FaultException faultException) {
                isSuccess = false;
                message = faultException.Message;
            }
            
            return Json( new { IsSuccess = isSuccess, Message = message });
        }

        public ActionResult SaveCustomer(FormCollection form)
        {
            //Save an existing customer
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
        public ActionResult RemoveCustomer(
            string id, 
            int pageNumber, 
            string orderBy, 
            bool isDesc, 
            string searchTerm)
        {
            AddressMaintenanceChannel.Instance.Service.RemoveCustomer(Guid.Parse(id));
            return RedirectToAction("CustomerData", "Home", new { pageNumber = pageNumber, orderBy = orderBy, isDesc = isDesc, searchTerm = searchTerm });
        }

        public ActionResult CustomerData(int pageNumber, string orderBy, bool isDesc, string searchTerm)
        {
            //Redirect sometimes turns this null
            if (String.IsNullOrEmpty(searchTerm))
            {
                searchTerm = String.Empty;
            }
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