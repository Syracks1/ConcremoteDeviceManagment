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
    public class DeviceConfigController : Controller
    {
        private Models.ConcremoteDeviceManagment db = new Models.ConcremoteDeviceManagment();

        // GET: DeviceConfig
        public ActionResult Index()
        {
            var Query = from d in db.DeviceConfig
                        orderby d.device_type_id
                        select d.device_type_id;
                        
            return View(db.DeviceConfig.ToList());
        }

        //// GET: DeviceConfig/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Stock stock = db.DeviceConfig.Find(id);
        //    if (stock == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(stock);
        //}

        //// GET: DeviceConfig/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: DeviceConfig/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "id,Price_id,bas_art_nr,stock_amount,min_stock,max_stock,description")] Stock stock)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Stock.Add(stock);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    return View(stock);
        //}

        //// GET: DeviceConfig/Edit/5
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

        //// POST: DeviceConfig/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "id,Price_id,bas_art_nr,stock_amount,min_stock,max_stock,description")] Stock stock)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(stock).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return View(stock);
        //}

        //// GET: DeviceConfig/Delete/5
        //public ActionResult Delete(int? id)
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

        //// POST: DeviceConfig/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Stock stock = db.Stock.Find(id);
        //    db.Stock.Remove(stock);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

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
