using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ConcremoteDeviceManagment.Models;
using System.Xml.Linq;

namespace ConcremoteDeviceManagment.Controllers
{
    public class StockController : Controller
    {
        private Models.ConcremoteDeviceManagment db = new Models.ConcremoteDeviceManagment();
        // GET: Stock
        public ActionResult Index(string StockCMI, string searchString)
        {
            //var Stock = from i in db.Stock
            //            select i;

             var Pricelist = db.pricelist.Include(d => d.Price_id);
         //   var Stock = db.Stock.Include(c => c.Pr)
            var Stock = from d in db.Stock
                      //  from p in db.pricelist
                        //where d.Price_id == p.Price_id
                        select d;
            //foreach(Stock d in Stock)
            //{
            //    foreach(Pricelist in d.Pricelist1)
            //    {
            //        Pricelist.Add()
            //    }
            //}
            foreach (var item in Stock)

                if (!string.IsNullOrEmpty(searchString))
                {
                    Stock = Stock.Where(s => s.description.Contains(searchString));
                }
            if (!string.IsNullOrEmpty(StockCMI))
            {
                Stock = Stock.Where(x => x.bas_art_nr.Contains(StockCMI));
            }
            return View(Stock);
        }
        // GET: Stock/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Stock stock = db.Stock.Find(id);
            if (stock == null)
            {
                return HttpNotFound();
            }
            return View(stock);
        }
        // GET: Stock/Create
        public ActionResult Create()
        {
            return View();
        }
        // POST: Stock/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,Price_id,bas_art_nr,stock_amount,min_stock,max_stock,description")] Stock stock)
        {
            if (ModelState.IsValid)
            {
                db.Stock.Add(stock);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(stock);
        }
        // GET: Stock/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Stock stock = db.Stock.Find(id);
            if (stock == null)
            {
                return HttpNotFound();
            }
            return View(stock);
        }
        // POST: Stock/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,Price_id,bas_art_nr,stock_amount,min_stock,max_stock,description")] Stock stock)
        {
            if (ModelState.IsValid)
            {
                db.Entry(stock).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(stock);
        }
        // GET: Stock/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Stock stock = db.Stock.Find(id);
            if (stock == null)
            {
                return HttpNotFound();
            }
            return View(stock);
        }
        // POST: Stock/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Stock stock = db.Stock.Find(id);
            db.Stock.Remove(stock);
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
