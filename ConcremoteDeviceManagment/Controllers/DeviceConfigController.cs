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
            var DeviceList = new List<int>();
            var DeviceQuery = from d in db.DeviceConfig
                              orderby d.Device_config_id
                              select d.Device_config_id;
            DeviceList.AddRange(DeviceQuery);
            var DeviceConfig = from i in db.DeviceConfig
                               select i;           
            return View(DeviceConfig);
        }
        public ActionResult CSDOKA()
        {
            var PartList = new List<string>();
            //var PartQuery = from d in db.Stock
            //                where d.bas_art_nr == "CMI0101"
            //                orderby d.bas_art_nr
            //                select d.bas_art_nr;
            //PartList.AddRange(PartQuery);
            var DeviceConfig2 = from d in db.pricelist
                              // join b in  
                               select d;
            return View(DeviceConfig2);
        }
        public ActionResult Cable_Sensor_Doka()
        {
            return View();
        }

        // GET: DeviceConfig2/Details/5
        public ActionResult Details(int? id)
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
