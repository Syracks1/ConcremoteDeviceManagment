using System;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using ConcremoteDeviceManagment.Models;


namespace ConcremoteDeviceManagment.Controllers
{
    [HandleError]
     
    public class StockController : Controller
    {
        private BasDbContext db = new BasDbContext();
        // GET: Stock
        public ActionResult Index(string sortOrder, string StockCMI, string searchString)
        {
            //sorteerparameters
            ViewBag.CMISortParm = String.IsNullOrEmpty(sortOrder) ? "CMI_desc" : "";
            ViewBag.DescriptionSortParm = sortOrder == "Description" ? "Desc_desc" : "Description";
            ViewBag.CurrentStockSort = sortOrder == "current" ? "current_desc" : "current";
            ViewBag.MinStockSort = sortOrder == "min_stock" ? "min_stock_desc" : "min_stock";
            ViewBag.MaxStockSort = sortOrder == "max_stock" ? "max_stock_desc" : "max_stock";
            //messages voor meldingen?
            
            var stock = from d in db.Stock
                            where d.Pricelist.Active == true
                        select d;
            switch(sortOrder)
            {
                case "CMI_desc":
                    stock = stock.OrderByDescending(s => s.Pricelist.bas_art_nr);
                    break;
                case "Description":
                    stock = stock.OrderBy(s => s.Pricelist.description);
                    break;
                case "Desc_desc":
                    stock = stock.OrderByDescending(s => s.Pricelist.description);
                    break;
                case "current":
                    stock = stock.OrderBy(s => s.stock_amount);
                    break;
                case "current_desc":
                    stock = stock.OrderByDescending(s => s.stock_amount);
                    break;
                case "min_stock":
                    stock = stock.OrderBy(s => s.min_stock);
                    break;
                case "min_stock_desc":
                    stock = stock.OrderByDescending(s => s.min_stock);
                    break;
                case "max_stock":
                    stock = stock.OrderBy(s => s.max_stock);
                    break;
                case "max_stock_desc":
                    stock = stock.OrderByDescending(s => s.max_stock);
                    break;
                default:
                    stock = stock.OrderBy(s => s.Pricelist.bas_art_nr);
                    break;


            }
            foreach (var item in stock)
            {
                if (!string.IsNullOrEmpty(searchString))
                {
                    stock = stock.Where(s => s.Pricelist.description.Contains(searchString));
                }
            }

            if (!string.IsNullOrEmpty(StockCMI))
            {
                stock = stock.Where(x => x.Pricelist.bas_art_nr.Contains(StockCMI));
            }
            return View(stock);
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
        [Authorize(Roles = "Admin")]
        // GET: Stock/Create
        public ActionResult Create()
        {
            var SelectedCMI = from d in db.pricelist
                              where d.Price_id == d.Price_id
                              orderby d.Price_id
                              select new { Id = d.Price_id, Value = d.bas_art_nr };
            ViewBag.SelectedCMI = new SelectList(SelectedCMI.Distinct(), "Id", "Value");
          //  var SelectedCMI = new SelectList(db.pricelist.Select(r => r.Price_id, ).Distinct().ToList());
          //  ViewBag.SelectedCMI = SelectedCMI;
            return View();
        }
        // POST: Stock/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,Price_id,stock_amount,min_stock,max_stock")] Stock stock, FormCollection formCollection)
        {
           // ManageMessageId? message;
            stock.Price_id = int.Parse(formCollection["SelectedCMI"]);
            if (ModelState.IsValid)
            {
                db.Stock.Add(stock);
                db.SaveChanges();
                TempData["AlertMessage"] = "Article Added Sucessfully";

                return RedirectToAction("Index");
            }
             //message = ManageMessageId.RemoveLoginSuccess;

            return View(stock);
        }
        [Authorize(Roles = "Admin")]
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
        // To protect from overposting attacks, please enable the specific properties you want to bind to
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,Price_id,stock_amount,min_stock,max_stock")] Stock stock )
        {
            if (ModelState.IsValid)
            {
                db.Entry(stock).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");

            }

            return View(stock);
        }
        [Authorize(Roles = "Admin")]
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
