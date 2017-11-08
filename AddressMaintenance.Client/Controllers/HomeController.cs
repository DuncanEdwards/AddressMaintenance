using AddressMaintenance.Client.Helpers;
using System;
using System.Collections.Generic;
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

        public ActionResult CustomerData()
        {
            var customersPagedList = AddressMaintenanceChannel.Instance.Service.GetAllCustomers();

            return View(customersPagedList);
        }
    }
}