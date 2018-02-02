using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ConcremoteDeviceManagment.Models;
using System.Data.SqlClient;

namespace ConcremoteDeviceManagment.Controllers
{
    public class DeviceConfigController : Controller
    {
        private Models.BasDbContext db = new Models.BasDbContext();
   //     private object i;

        // GET: DeviceConfig2
        public ActionResult Index()
        {
            var SelectedDevices = new SelectList(db.DeviceType.Select(c => c.name).Distinct().ToList());
            //from d in db.DeviceType
            //join c in db.DeviceConfig.Distinct()
            //on d.device_type_id equals c.device_type_id
            //select d.name);
            ViewBag.SelectedDevice = SelectedDevices;

            return View();
        }
        public PartialViewResult CreateDevice(string Device)
        {
          //  var EditDevice = new SelectList(db.pricelist.Select(c => c.bas_art_nr).Distinct().ToList());

           // ViewBag.SelectedDevice = EditDevice;
            return PartialView("CreateDevice", db.DeviceConfig.Where(c => c.DeviceType.name == Device).OrderBy(c => c.assembly_order));
        }

        // GET: DeviceConfig2/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    DeviceConfig deviceConfig = db.DeviceConfig.Find(id);
        //    if (deviceConfig == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(deviceConfig);
        //}
        // GET: DeviceConfig2/Create
        public ActionResult Create()
        {
            return View();
        }
        // POST: DeviceConfig2/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Device_config_id,device_type_id,Price_id,amount")] DeviceConfig deviceConfig)
        {
            if (ModelState.IsValid)
            {
                db.DeviceConfig.Add(deviceConfig);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(deviceConfig);
        }
        // GET: DeviceConfig2/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DeviceConfig deviceConfig = db.DeviceConfig.Find(id);
            if (deviceConfig == null)
            {
                return HttpNotFound();
            }
            return View(deviceConfig);
        }

        // POST: DeviceConfig2/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Device_config_id,device_type_id,Price_id,amount")] DeviceConfig deviceConfig)
        {
            if (ModelState.IsValid)
            {
                db.Entry(deviceConfig).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(deviceConfig);
        }

        // GET: DeviceConfig2/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DeviceConfig deviceConfig = db.DeviceConfig.Find(id);
            if (deviceConfig == null)
            {
                return HttpNotFound();
            }
            return View(deviceConfig);
        }

        // POST: DeviceConfig2/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DeviceConfig deviceConfig = db.DeviceConfig.Find(id);
            db.DeviceConfig.Remove(deviceConfig);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
