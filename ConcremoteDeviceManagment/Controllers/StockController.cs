using ConcremoteDeviceManagment.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

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
            //stock query to call in all data
            var stock = from d in db.Stock
                        where d.Pricelist.Active == true
                        select d;
            switch (sortOrder)
            {
                //order CMI descending
                case "CMI_desc":
                    stock = stock.OrderByDescending(s => s.Pricelist.bas_art_nr);
                    break;
                //order Description ascending
                case "Description":
                    stock = stock.OrderBy(s => s.Pricelist.description);
                    break;
                //order Description descending
                case "Desc_desc":
                    stock = stock.OrderByDescending(s => s.Pricelist.description);
                    break;
                //order current stock amount ascending
                case "current":
                    stock = stock.OrderBy(s => s.stock_amount);
                    break;
                //order current stock amount descneding
                case "current_desc":
                    stock = stock.OrderByDescending(s => s.stock_amount);
                    break;
                //order minimal stock amount ascending
                case "min_stock":
                    stock = stock.OrderBy(s => s.min_stock);
                    break;
                //order minimal stock amount descending
                case "min_stock_desc":
                    stock = stock.OrderByDescending(s => s.min_stock);
                    break;
                //order maximal stock amount ascending
                case "max_stock":
                    stock = stock.OrderBy(s => s.max_stock);
                    break;
                //order maximal stock amount descending
                case "max_stock_desc":
                    stock = stock.OrderByDescending(s => s.max_stock);
                    break;
                // default: order CMI ascending
                default:
                    stock = stock.OrderBy(s => s.Pricelist.bas_art_nr);
                    break;
            }

            foreach (var item in stock)
            {
                if (!string.IsNullOrEmpty(searchString))
                {
                    //filter item in stock where textbox contains description
                    stock = stock.Where(s => s.Pricelist.description.Contains(searchString));
                }
                if (!string.IsNullOrEmpty(StockCMI))
                {
                    //filter item in stock where textbox contains CMI
                    stock = stock.Where(x => x.Pricelist.bas_art_nr.Contains(StockCMI));
                }
            }
            
            return View(stock);
        }

        // GET: Stock/Details/5
        public ActionResult Details(int? id)
        {
            // if id is null, return BadRequest
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //find selected ID in pricelist
            Stock stock = db.Stock.Find(id);
            if (stock == null)
            {
                //if not found, return this
                return HttpNotFound();
            }
            return View(stock);
        }

        //Check if user is Assembly or Admin
        //else redirect te login
        [Authorize(Roles = "Assembly,Admin")]

        // GET: Stock/Create
        public ActionResult Create()
        {
            //dropdownlist for CMI
            var SelectedCMI = from d in db.pricelist
                                  //where d.Price_id == d.Price_id
                              orderby d.Price_id
                              select new { Id = d.Price_id, Value = d.bas_art_nr };
            ViewBag.SelectedCMI = new SelectList(SelectedCMI.Distinct(), "Id", "Value");
            return View();
        }

        // POST: Stock/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,Price_id,stock_amount,min_stock,max_stock")] Stock stock)
        {
            if (ModelState.IsValid)
            {
                db.Stock.Add(stock);
                db.SaveChanges();
                TempData["AlertMessage"] = "Article " + "" + " Deleted Successfully.";

                return RedirectToAction("Index");
            }
            //message = ManageMessageId.RemoveLoginSuccess;

            return View(stock);
        }

        //Check if user is Assembly or Admin
        //else redirect te login
        [Authorize(Roles = "Assembly,Admin")]

        // GET: Stock/Edit/5
        public ActionResult Edit(int? id)
        {
            // if id is null, return BadRequest
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //find selected ID in pricelist
            Stock stock = db.Stock.Find(id);
            if (stock == null)
            {
                //if not found, return this
                return HttpNotFound();
            }
            return View(stock);
        }

        // POST: Stock/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,Price_id,stock_amount,min_stock,max_stock")] Stock stock)
        {
            if (ModelState.IsValid)
            {
                db.Entry(stock).State = EntityState.Modified;
                db.SaveChanges();
                TempData["AlertMessage"] = "Article Edited Successfully";
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
            TempData["AlertMessage"] = "Article " + stock.Price_id.ToString() + " Deleted Successfully.";
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