using ConcremoteDeviceManagment.Models;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace ConcremoteDeviceManagment.Controllers
{
    [HandleError]
    public class ArticleController : Controller
    {
        private BasDbContext db = new BasDbContext();

        // GET: Device
        public ActionResult Index(string sortOrder, string PriceCMI, FormCollection formCollection)
        {
            var SelectedLeverancier = (from d in db.pricelist
                                       select new { Id = d.Leverancier, Value = d.Leverancier }).Distinct();

            ViewBag.SelectedLeverancier = new SelectList(SelectedLeverancier.Distinct(), "Id", "Value");
            //        ViewBag.SelectedLeverancier = SelectedLeverancier;
            ViewBag.CMISortParm = string.IsNullOrEmpty(sortOrder) ? "CMI_desc" : "";
            ViewBag.ActiveSortParm = sortOrder == "Active" ? "Active_desc" : "Active";
            ViewBag.PriceSortParm = sortOrder == "Price" ? "Price_desc" : "Price";
            ViewBag.LevSortParm = sortOrder == "Leverancier" ? "Lev_desc" : "Leverancier";
            ViewBag.ArtSortParm = sortOrder == "lev_art" ? "lev_art_desc" : "lev_art";
            ViewBag.DescrSortParm = sortOrder == "descripton" ? "descripton_desc" : "descripton";
            var pricelist = from d in db.pricelist
                            select d;
            switch (sortOrder)
            {
                case "CMI_desc":
                    pricelist = pricelist.OrderByDescending(s => s.bas_art_nr);
                    break;

                case "Active":
                    pricelist = pricelist.OrderBy(s => s.Active);
                    break;

                case "Active_desc":
                    pricelist = pricelist.OrderByDescending(s => s.Active);
                    break;

                case "Price":
                    pricelist = pricelist.OrderBy(s => s.Price);
                    break;

                case "Price_desc":
                    pricelist = pricelist.OrderByDescending(s => s.Price);
                    break;

                case "Leverancier":
                    pricelist = pricelist.OrderBy(s => s.Leverancier);
                    break;

                case "Lev_desc":
                    pricelist = pricelist.OrderByDescending(s => s.Leverancier);
                    break;

                case "lev_art":
                    pricelist = pricelist.OrderBy(s => s.art_lev_nr);
                    break;

                case "lev_art_desc":
                    pricelist = pricelist.OrderByDescending(s => s.art_lev_nr);
                    break;

                case "descripton":
                    pricelist = pricelist.OrderBy(s => s.description);
                    break;

                case "descripton_desc":
                    pricelist = pricelist.OrderByDescending(s => s.description);
                    break;

                default:
                    pricelist = pricelist.OrderBy(s => s.bas_art_nr);
                    break;

            }

            // return View(db.pricelist.ToList());
            foreach (var item in pricelist)
            {
                //if (item.Leverancier.Equals(ViewBag.SelectedLeverancier))
                //{
                //   //(formCollection["SelectedLeverancier"]);
                //    pricelist = pricelist.Where(s => s.Leverancier.Equals(SelectedLeverancier));
                //}
                //if (!string.IsNullOrEmpty(ViewBag.SelectedLeverancier))
                //{
                //    pricelist = pricelist.Where(s => s.Leverancier.Equals(SelectedLeverancier));
                //}
                if (!string.IsNullOrEmpty(PriceCMI))
                {
                    pricelist = pricelist.Where(s => s.bas_art_nr.Contains(PriceCMI));
                }
            }
            return View(pricelist);
        }

        // GET: Device/Details/5
        public ActionResult Details(int? id)
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

        [Authorize(Roles = "Assembly,Admin")]

        // GET: Device/Create
        public ActionResult Create()
        {
            var SelectedLeverancier = from d in db.pricelist
                          //    where d.Price_id == d.Price_id
                              orderby d.Leverancier
                              select new { Id = d.Leverancier, Value = d.Leverancier };
            ViewBag.SelectedLeverancier = new SelectList(SelectedLeverancier.Distinct(), "Id", "Value");
            return View();
        }

        // POST: Device/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Price_id,CategoryId,SubCategoryId,price,art_lev_nr,bas_art_nr,Leverancier,description,active")] Pricelist pricelist )
        {
            //pricelist.Leverancier = (formCollection["SelectedLeverancier"]);
            //var SelectedLeverancier = new SelectList(db.pricelist.Select(r => r.Leverancier).Distinct().ToList());
          //  ViewBag.SelectedLeverancier = SelectedLeverancier;
            if (ModelState.IsValid)
            {
                db.pricelist.Add(pricelist);
                db.SaveChanges();
                TempData["AlertMessage"] = "Article Added Successfully";
                return RedirectToAction("Index");
            }

            return View(pricelist);
        }

        [Authorize(Roles = "Assembly,Admin")]

        // GET: Device/Edit/5
        public ActionResult Edit(int? id)
        {
            var SelectedLeverancier = from d in db.pricelist
                                          //    where d.Price_id == d.Price_id
                                      orderby d.Leverancier
                                      select new { Id = d.Leverancier, Value = d.Leverancier };
            ViewBag.SelectedLeverancier = new SelectList(SelectedLeverancier.Distinct(), "Id", "Value");
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

        // POST: Device/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Price_id,CategoryId,SubCategoryId,price,art_lev_nr,bas_art_nr,Leverancier,description,active")] Pricelist pricelist)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pricelist).State = EntityState.Modified;
                db.SaveChanges();
                TempData["AlertMessage"] = "Article Edited Successfully";
                return RedirectToAction("Index");
            }
            return View(pricelist);
        }

        [Authorize(Roles = "Admin")]
        // GET: Device/Delete/5
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

        // POST: Device/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Pricelist pricelist = db.pricelist.Find(id);
            db.pricelist.Remove(pricelist);
            db.SaveChanges();
            TempData["AlertMessage"] = "Article Deleted Successfully";
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