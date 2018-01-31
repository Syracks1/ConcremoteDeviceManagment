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

        public ActionResult Index()
        {
            var SelectedDevices = new SelectList(db.DeviceConfig.Select(r => r.DeviceType.name).Distinct().ToList());

            ViewBag.SelectedDevice = SelectedDevices;

            return View();
        }
        [HttpGet]
        public PartialViewResult GetDevice(string Device)
        {
           
           return PartialView("GetDevice",  db.DeviceConfig.Where(c => c.DeviceType.name == Device).OrderBy(c => c.assembly_order));
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Device_config_id,device_type_id,Price_id,amount,assembly_arder,device_type")] DeviceConfig DeviceConfig)
        {
            if (ModelState.IsValid)
            {
                db.DeviceConfig.Add(DeviceConfig);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(DeviceConfig);
        }
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
