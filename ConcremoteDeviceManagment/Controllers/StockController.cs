using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using ConcremoteDeviceManagment.Models;

namespace ConcremoteDeviceManagment.Controllers
{
    public class StockController : Controller
    {
        private BasDbContext db = new BasDbContext();
        // GET: Stock
        public ActionResult Index(string StockCMI, string searchString)
        {
            //var Stock = from i in db.Stock
            //            select i;

            //  var Pricelist = db.pricelist.Include(d => d.Price_id);
            //   var Stock = db.Stock.Include(c => c.Pr)
            var stock = from d in db.Stock
                        //    from b in db.pricelist
                            where d.Pricelist.Active == true
                        select d;
            //foreach(Stock d in Stock)
            //{
            //    foreach(Pricelist in d.Pricelist1)
            //    {
            //        Pricelist.Add()
            //    }
            //}
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
        public ActionResult Create([Bind(Include = "id,Price_id,stock_amount,min_stock,max_stock")] Stock stock)
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
        public ActionResult Edit([Bind(Include = "Price_id,description,art_lev_nr,stock_amount,min_stock,max_stock,Active")] Stock stock, Pricelist pricelist )
        {
            if (ModelState.IsValid)
            {
                db.Entry(pricelist).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(pricelist);
        }
        // GET: Stock/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pricelist pricelist = db.pricelist.Find(id);
            if (pricelist == null)
            {
                return HttpNotFound();
            }
            return View(pricelist);
        }
       // POST: Stock/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Pricelist pricelist = db.pricelist.Find(id);
            db.pricelist.Remove(pricelist);
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
