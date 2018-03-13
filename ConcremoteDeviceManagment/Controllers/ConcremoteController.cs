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

    public class ConcremoteController : Controller
    {
        private BasDbContext db = new BasDbContext();
       //[Authorize(Roles = "BAS employee, Assembly, Admin")]
        // GET: Concremote
        public ActionResult Index()
        {
            var query = from d in db.DeviceStatus
                        select d;
            return View(query);

        }
        //[Authorize(Roles = "Assembly, Admin")]
        // GET: Concremote/Details/5
        public ActionResult Details(int? id)
        {
           //var test = from d in db.DeviceStatus
           //           from b in db.Device_Pricelist
           //           where d.DeviceConfig_id = b.Device_config_id
           //           select d
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
             DeviceStatus_ExtraInfo deviceStatus_ExtraInfo = db.DeviceStatus_ExtraInfo.Find(id);
            if (deviceStatus_ExtraInfo == null)
            {
                return HttpNotFound();
            }
            return View(deviceStatus_ExtraInfo);
        }
        [HttpGet]
        public PartialViewResult ConfigPartial(string Device)
        {
            var query = from d in db.Device_Pricelist
                        select d;
            // List<Device_Pricelist> ci = new List<Device_Pricelist>(db.Device_Pricelist.Where(r => r.Device_config_id == r.DeviceConfig.Device_config_id));

            return PartialView("ConfigPartial", query);
        }

        [Authorize(Roles = "Admin")]

        // GET: Concremote/Create
        public ActionResult Create()
        {
            ViewBag.device_type_id = new SelectList(db.DeviceType.Select(r => r.name).Distinct().ToList());
            return View();
        }

        // POST: Concremote/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,device_id,imei,active,oldsystem_concremote,Allowvalidation,device_type_id")] ConcremoteDevice concremoteDevice)
        {
            if (ModelState.IsValid)
            {
                db.ConcremoteDevice.Add(concremoteDevice);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.device_type_id = new SelectList(db.DeviceType, "device_type_id", "device_type", concremoteDevice.id);
            return View(concremoteDevice);
        }
        [Authorize(Roles = "Admin")]

        // GET: Concremote/Edit/5
        public ActionResult Edit(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DeviceStatus deviceStatus_ExtraInfo = db.DeviceStatus.Find(id);
            if (deviceStatus_ExtraInfo == null)
            {
                return HttpNotFound();
            }
            return View(deviceStatus_ExtraInfo);
        }

        // POST: Concremote/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,DeviceConfig_id,Device_statustypes_id,ConcremoteDevice_id,ConcremoteDevice_Active,Employee_1,Employee_2,Sign_Date")] DeviceStatus deviceStatus)
        {
            if (ModelState.IsValid)
            {
                db.Entry(deviceStatus).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
        //    ViewBag.device_type_id = new SelectList(db.DeviceType, "device_type_id", "device_type", concremoteDevice.id);
            return View(deviceStatus);
        }

        // GET: Concremote/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ConcremoteDevice concremoteDevice = db.ConcremoteDevice.Find(id);
            if (concremoteDevice == null)
            {
                return HttpNotFound();
            }
            return View(concremoteDevice);
        }

        // POST: Concremote/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ConcremoteDevice concremoteDevice = db.ConcremoteDevice.Find(id);
            db.ConcremoteDevice.Remove(concremoteDevice);
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

        //// GET: Concremote/Detail/2
        //public ActionResult Detail(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    ConcremoteDevice concremoteDevice = db.ConcremoteDevice.Find(id);
        //    if (concremoteDevice == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(concremoteDevice);
        //}

        //// POST: Concremote/Detail/2
        //[HttpPost, ActionName("Detail")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DetailConfirmed(int id)
        //{
        //    ConcremoteDevice concremoteDevice = db.ConcremoteDevice.Find(id);
        //    db.ConcremoteDevice.Remove(concremoteDevice);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}
    }
}
