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
        private BasDbContext db = new BasDbContext();

        public BasDbContext Db { get => db; set => db = value; }

        //List<Stock> Stock = new List<Stock>();
        // List<Pricelist> StockInfo = new List<Pricelist>();
        public ActionResult Index(int? SelectedDevice)
        {
            Populate();
            return View();
        }
        public PartialViewResult GetDevice(string device_type_id)
        {

            var model = from d in db.DeviceConfig
                            //where d.device_type_id == b.device_type_id
                        select d;
            //     var model = db.DeviceConfig.Find(device_type_id);
            //      string SelectedDevice = db.DeviceConfig = device_type_id(;)
            return PartialView("GetDevice", model);
            
        }
        private void Populate()
        {
            List<SelectListItem> query = db.DeviceType.OrderBy(x => x.device_type_id).Select(c => new SelectListItem { Text = c.name, Value = c.name }).ToList();
            ViewBag.SelectedDevice = query;
        }
    }
}
