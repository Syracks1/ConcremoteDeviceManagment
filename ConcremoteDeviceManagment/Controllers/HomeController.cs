using ConcremoteDeviceManagment.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ConcremoteDeviceManagment.Controllers
{
    public class HomeController : Controller
    {
        private BasDbContext db = new BasDbContext();

        public ActionResult Index()
        {
            var SelectedDevices = new SelectList(db.DeviceType.Select(c => c.name).Distinct().ToList());
            ViewBag.SelectedDevice = SelectedDevices;
                
            return View();
        }

        [HttpGet]
        public PartialViewResult GetDevice(string Device)
        {
            var EditDevice = new SelectList(db.pricelist.Select(c => c.bas_art_nr).Distinct().ToList());

            ViewBag.EditDevice = EditDevice;
            return PartialView("GetDevice",  db.DeviceConfig.Where(c => c.DeviceType.name == Device == c.Pricelist.Active == true).OrderBy(c => c.assembly_order));
        }
        public ActionResult Create()
        {
            PopulateDeviceDropDownList();
            return View();  
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Device_config_id,device_type_id,Price_id,amount,assembly_order")] DeviceConfig DeviceConfig)
        {
            var EditDevice = new SelectList(db.pricelist.Select(c => c.bas_art_nr).Distinct().ToList());

            ViewBag.EditDevice = EditDevice;
            try
            {
                if (ModelState.IsValid)
                {
                    db.DeviceConfig.Add(DeviceConfig);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch(RetryLimitExceededException)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again");
            }
            PopulateDeviceDropDownList(DeviceConfig.device_type_id);
            return View(DeviceConfig);
        }
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Stock stock = db.Stock.Find(id);
        //    if (stock == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(stock);
        //}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Device_config_id,device_type_id,Price_id,amount,assembly_order,device_type")] DeviceConfig DeviceConfig)
        {
            if (ModelState.IsValid)
            {
                db.Entry(DeviceConfig).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
        
            return View();
        }
        // GET: DeviceConfig/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DeviceConfig DeviceConfig = db.DeviceConfig.Find(id);
            if (DeviceConfig == null)
            {
                return HttpNotFound();
            }
            return View(DeviceConfig);
        }

        // POST: DeviceConfig/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]

        public ActionResult DeleteConfirmed(int id)
        {
            DeviceConfig DeviceConfig = db.DeviceConfig.Find(id);
            db.DeviceConfig.Remove(DeviceConfig);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        private void PopulateDeviceDropDownList(object SelectedDevice = null)
        {
            var SelectedDevices = new SelectList(db.DeviceType.Select(r => r.name).Distinct().ToList());

            ViewBag.SelectedDevice = SelectedDevices;
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
