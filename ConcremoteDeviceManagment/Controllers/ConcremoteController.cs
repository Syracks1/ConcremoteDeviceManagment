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

        // GET: Concremote
        public ActionResult Index()
        {
            //  var extradevice = db.Device_Extra.Include(d => d.ConcremoteDevice);
            //var DeviceStatus = db.ConcremoteDevice.Include(c => c.id);

            //var query = (from d in db.DeviceStatus
            //            join st in db.ConcremoteDevice on d.ConcremoteDevice_id equals st.id
            //            join dt in db.DeviceConfig on d.DeviceConfig_id equals dt.Device_config_id
            //            join sd in db.Device_statustypes on d.Device_statustypes_id equals sd.id
            //            select new { d.Sign_Date, st.Active, dt.VersionNr, dt.Date, sd.name  });
            var query = from d in db.DeviceStatus
                        select d;
            return View(query);

        }

        ////// GET: Concremote/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //     ConcremoteDevice concremoteDevice = db.ConcremoteDevice.Find(id);
        //    if (concremoteDevice == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(concremoteDevice);
        //}

        //// GET: Concremote/Create
        //public ActionResult Create()
        //{
        //    ViewBag.device_type_id = new SelectList(db.DeviceType.Select(r => r.name).Distinct().ToList());
        //    return View();
        //}

        //// POST: Concremote/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "id,device_id,imei,active,oldsystem_concremote,Allowvalidation,device_type_id")] ConcremoteDevice concremoteDevice)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.ConcremoteDevice.Add(concremoteDevice);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    ViewBag.device_type_id = new SelectList(db.DeviceType, "device_type_id", "device_type", concremoteDevice.id);
        //    return View(concremoteDevice);
        //}

        //// GET: Concremote/Edit/5
        //public ActionResult Edit(int? id)
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
        //    ViewBag.device_type_id = new SelectList(db.DeviceType, "device_type_id", "device_type", concremoteDevice.id);
        //    return View(concremoteDevice);
        //}

        //// POST: Concremote/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "id,device_id,imei,active,oldsystem_concremote,Allowvalidation,device_type_id")] ConcremoteDevice concremoteDevice)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(concremoteDevice).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.device_type_id = new SelectList(db.DeviceType, "device_type_id", "device_type", concremoteDevice.id);
        //    return View(concremoteDevice);
        //}

        //// GET: Concremote/Delete/5
        //public ActionResult Delete(int? id)
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

        //// POST: Concremote/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    ConcremoteDevice concremoteDevice = db.ConcremoteDevice.Find(id);
        //    db.ConcremoteDevice.Remove(concremoteDevice);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}

        ////// GET: Concremote/Detail/2
        ////public ActionResult Detail(int? id)
        ////{
        ////    if (id == null)
        ////    {
        ////        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        ////    }
        ////    ConcremoteDevice concremoteDevice = db.ConcremoteDevice.Find(id);
        ////    if (concremoteDevice == null)
        ////    {
        ////        return HttpNotFound();
        ////    }
        ////    return View(concremoteDevice);
        ////}

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
