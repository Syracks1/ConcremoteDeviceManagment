using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ConcremoteDeviceManagment.Models;

namespace ConcremoteDeviceManagment.Controllers
{
     
    public class Device_extraController : Controller
    {
        private BasDbContext db = new BasDbContext();

        // GET: Device_extra
        public ActionResult Index()
        {
            //var device_Extra = db.Device_extra_info.Include(d => d.Pricelist);
            ////var device_Extra_Concremote = db.Device_Extra.Include(b => b.ConcremoteDevice);
            var device_Extra = from d in db.DeviceConfig_ExtraInfo
                               select d;
            return View();
        }

        // GET: Device_extra/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //      Device_extra_info device_extra = db.Device_extra_info.Find(id);
            DeviceConfig_ExtraInfo device_Extra_Info = db.DeviceConfig_ExtraInfo.Find(id);
            //if (device_extra == null)
            //{
            //    return HttpNotFound();
            //}
            return View(device_Extra_Info);
        }

        // GET: Device_extra/Create
        public ActionResult Create()
        {
            ViewBag.Price_id = new SelectList(db.pricelist, "Price_id", "CategoryId");
            return View();
        }

        // POST: Device_extra/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,ConcremoteDevice_id,Price_id,Datum,Active,Eigenschap_id")] DeviceConfig_ExtraInfo device_extra)
        {
            if (ModelState.IsValid)
            {
                db.DeviceConfig_ExtraInfo.Add(device_extra);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

        //    ViewBag.Price_id = new SelectList(db.pricelist, "Price_id", "CategoryId", device_extra.Price_id);
            return View(device_extra);
        }

        // GET: Device_extra/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DeviceConfig_ExtraInfo device_extra = db.DeviceConfig_ExtraInfo.Find(id);
            if (device_extra == null)
            {
                return HttpNotFound();
            }
            ViewBag.Price_id = new SelectList(db.pricelist, "Price_id", "CategoryId", device_extra.id);
            return View(device_extra);
        }

        // POST: Device_extra/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,ConcremoteDevice_id,Price_id,Datum,Active,Eigenschap_id")] DeviceConfig_ExtraInfo device_extra)
        {
            if (ModelState.IsValid)
            {
                db.Entry(device_extra).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Price_id = new SelectList(db.pricelist, "Price_id", "CategoryId", device_extra.id);
            return View(device_extra);
        }

        // GET: Device_extra/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DeviceConfig_ExtraInfo device_extra = db.DeviceConfig_ExtraInfo.Find(id);
            if (device_extra == null)
            {
                return HttpNotFound();
            }
            return View(device_extra);
        }

        // POST: Device_extra/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DeviceConfig_ExtraInfo device_extra = db.DeviceConfig_ExtraInfo.Find(id);
            db.DeviceConfig_ExtraInfo.Remove(device_extra);
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
