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
    public class DeviceController : Controller
    {
        private Models.BasDbContext db = new Models.BasDbContext();

        // GET: Device
        public ActionResult Index(string sortOrder, string PriceCMI, string searchStringPrice)
        {
            var SelectedLeverancier = (from r in db.pricelist
                                       select r.Leverancier).Distinct();
            ViewBag.SelectedLeverancier = SelectedLeverancier;
            ViewBag.CMISortParm = String.IsNullOrEmpty(sortOrder) ? "CMI_desc" : "";
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
                if (!string.IsNullOrEmpty(searchStringPrice))
                {
                    pricelist = pricelist.Where(s => s.Leverancier.Equals(SelectedLeverancier));
                }

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

        // GET: Device/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Device/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Price_id,CategoryId,SubCategoryId,price,art_lev_nr,bas_art_nr,Leverancier,description,active")] Pricelist pricelist)
        {
            if (ModelState.IsValid)
            {
                db.pricelist.Add(pricelist);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(pricelist);

        }
        // GET: Device/Edit/5
        public ActionResult Edit(int? id)
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

        // POST: Device/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Price_id,CategoryId,SubCategoryId,price,art_lev_nr,bas_art_nr,Leverancier,description,active")] Pricelist pricelist)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pricelist).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(pricelist);
        }

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
