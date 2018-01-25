using ConcremoteDeviceManagment.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ConcremoteDeviceManagment.Controllers
{
    public class HomeController : Controller
    {
        private Models.ConcremoteDeviceManagment db = new Models.ConcremoteDeviceManagment();

        //List<Stock> Stock = new List<Stock>();
        // List<Pricelist> StockInfo = new List<Pricelist>();
        public ActionResult Index(int? SelectedDevice)
        {      
            List<SelectListItem> query = db.DeviceType.OrderBy(c => c.name).Select(c => new SelectListItem { Text = c.name, Value = c.name}).ToList();
            ViewBag.SelectedDevice = query;
            return View();
        }
        public PartialViewResult GetDevice(string device_type_id)
        {
            var model = from d in db.DeviceConfig
                         select d;
           // var model = db.DeviceConfig.Find(device_type_id);
            return PartialView("PartialView", model);
        }
    }
}
