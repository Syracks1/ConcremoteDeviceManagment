using ConcremoteDeviceManagment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ConcremoteDeviceManagment.Controllers
{
    public class HomeController : Controller
    {
        private Models.ConcremoteDeviceManagment db = new Models.ConcremoteDeviceManagment();

        List<Stock> Stock = new List<Stock>();
        List<Pricelist> StockInfo = new List<Pricelist>();
        public ActionResult Index()
        {
            var StockVM = from d in db.DeviceConfig
                          select d;
                         
            return View(StockVM);
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
    }
}