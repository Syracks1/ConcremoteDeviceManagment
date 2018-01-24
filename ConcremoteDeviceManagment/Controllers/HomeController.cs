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
          //  var DeviceType = db.DeviceType.OrderBy(q => q.name).ToList().OrderBy(x => x.device_type_id);
        //    ViewBag.SelectedDevice = new SelectList(DeviceType, "device_type_id", "name", SelectedDevice);
            //   var StockVM = from d in db.DeviceConfig
            //               select d;
            // foreach (var item in SelectedDevice)
               List<SelectListItem> query = db.DeviceType.OrderBy(c => c.name).Select(c => new SelectListItem { Text = c.name, Value = c.name}).ToList();
            ViewBag.SelectedDevice = query;
            return View();
        }
        public PartialViewResult GetDevice(int device_config_id)
        {
            var model = db.DeviceConfig.Find(device_config_id);
            return PartialView("PartialView", model);
        }
        //public PartialViewResult GetDevice(int device_type_id)
        //{
        //    var model = db.DeviceType.Find(device_type_id);
        //    return PartialView("PartialView", model);
        //}
    }
}
