﻿using ConcremoteDeviceManagment.Models;
using System;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace ConcremoteDeviceManagment.Controllers
{
    [HandleError]
    public class ArticleController : Controller
    {
        //call in database connection
        private BasDbContext db = new BasDbContext();

        // GET: Device
        public ActionResult Index(string sortOrder, string PriceCMI, string SelectedLeverancier)
        {
            //dropdown for "Leverancier"

            ViewBag.SelectedLeverancier = new SelectList(db.pricelist.Select(d => d.Leverancier).Distinct());
            //sort order different parameters
            ViewBag.CMISortParm = string.IsNullOrEmpty(sortOrder) ? "CMI_desc" : "";
            ViewBag.ActiveSortParm = sortOrder == "Active" ? "Active_desc" : "Active";
            ViewBag.PriceSortParm = sortOrder == "Price" ? "Price_desc" : "Price";
            ViewBag.LevSortParm = sortOrder == "Leverancier" ? "Lev_desc" : "Leverancier";
            ViewBag.ArtSortParm = sortOrder == "lev_art" ? "lev_art_desc" : "lev_art";
            ViewBag.DescrSortParm = sortOrder == "descripton" ? "descripton_desc" : "descripton";

            //pricelist query to call in all data
            var pricelist = from d in db.pricelist
                            select d;
            switch (sortOrder)
            {
                // order CMI descending
                case "CMI_desc":
                    pricelist = pricelist.OrderByDescending(s => s.bas_art_nr);
                    break;
                //order Active ascending
                case "Active":
                    pricelist = pricelist.Where(s => s.Active == true);
                    break;
                //order Active descending
                case "Active_desc":
                    pricelist = pricelist.Where(s => s.Active == false);
                    break;
                //order Price ascending
                case "Price":
                    pricelist = pricelist.OrderBy(s => s.Price);
                    break;
                //order Price descending
                case "Price_desc":
                    pricelist = pricelist.OrderByDescending(s => s.Price);
                    break;
                //order Leverancier ascending by name
                case "Leverancier":
                    pricelist = pricelist.OrderBy(s => s.Leverancier);
                    break;
                //order Leverancier descending by name
                case "Lev_desc":
                    pricelist = pricelist.OrderByDescending(s => s.Leverancier);
                    break;
                //order art_lev_nr ascending by name
                case "lev_art":
                    pricelist = pricelist.OrderBy(s => s.art_lev_nr);
                    break;
                //order art_lev_nr descending by name
                case "lev_art_desc":
                    pricelist = pricelist.OrderByDescending(s => s.art_lev_nr);
                    break;
                //order description ascending by name
                case "descripton":
                    pricelist = pricelist.OrderBy(s => s.description);
                    break;
                //order description descending by name
                case "descripton_desc":
                    pricelist = pricelist.OrderByDescending(s => s.description);
                    break;
                //default order CMI ascending
                default:
                    pricelist = pricelist.OrderBy(s => s.bas_art_nr);
                    break;
            }

            foreach (var item in pricelist)
            {
                if (!string.IsNullOrEmpty(SelectedLeverancier))
                {
                    //filter item in pricelist where Dropdown contains Leverancier

                    pricelist = pricelist.Where(s => s.Leverancier.Contains(SelectedLeverancier));
                }
                if (!string.IsNullOrEmpty(PriceCMI))
                {
                    //filter item in pricelist where textbox contains CMI
                    pricelist = pricelist.Where(s => s.bas_art_nr.Contains(PriceCMI));
                }
            }
            return View(pricelist);
        }

        // GET: Device/Details/5
        public ActionResult Details(int? id)
        {
            // if id is null, return BadRequest
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //find selected ID in pricelist
            Pricelist pricelist = db.pricelist.Find(id);
            if (pricelist == null)
            {
                //if not found, return this
                return HttpNotFound();
            }
            return PartialView("Details", pricelist);
        }

        //Check if user is Assembly or Admin
        //else redirect te login
        [Authorize(Roles = "Assembly,Admin")]
        // GET: Device/Create
        public ActionResult Create()
        {
            //dropdownlist for Leverancier
            var SelectedLeverancier = from d in db.pricelist
                                          //    where d.Price_id == d.Price_id
                                      orderby d.Leverancier
                                      select new { Id = d.Leverancier, Value = d.Leverancier };
            ViewBag.SelectedLeverancier = new SelectList(SelectedLeverancier.Distinct(), "Id", "Value");
            return PartialView("Create");
        }

        // POST: Device/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Create(Pricelist pricelist)
        {
            if (ModelState.IsValid)
            {
                try
                { 
                    //Add data to pricelist table
                    db.pricelist.Add(pricelist);
                    //save changes to database
                    db.SaveChanges();
                    //Temp message when article is added succesfully
                    return Json(new { success = true });
                }
                catch (Exception ex)
                {
                    Trace.TraceError(ex.Message + " SendGrid probably not configured correctly.");
                }
            }
            return Json(pricelist, JsonRequestBehavior.AllowGet);
        }

        //Check if user is Assembly or Admin
        //else redirect to login
        [Authorize(Roles = "Assembly,Admin")]

        // GET: Device/Edit/5
        public ActionResult Edit(int? id)
        {
            //dropdownlist for Leverancier
            var SelectedLeverancier = from d in db.pricelist
                                          //    where d.Price_id == d.Price_id
                                      orderby d.Leverancier
                                      select new { Id = d.Leverancier, Value = d.Leverancier };
            ViewBag.SelectedLeverancier = new SelectList(SelectedLeverancier.Distinct(), "Id", "Value");
            //if ID is null, return BadRequest
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //find selected ID in pricelist
            Pricelist pricelist = db.pricelist.Find(id);
            if (pricelist == null)
            {
                //if Data is not found, return this
                return HttpNotFound();
            }
            return PartialView("Edit", pricelist);
        }

        // POST: Device/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Price_id,price,art_lev_nr,bas_art_nr,Leverancier,description,active")] Pricelist pricelist)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //check if row is modified
                    db.Entry(pricelist).State = EntityState.Modified;
                    //save changes to database
                    db.SaveChanges();
                    //Temp message when Article is succesfully edited
                    return Json(new { success = true, message = "test" }, JsonRequestBehavior.AllowGet);
                }
                catch
                {
                    //Temp message to inform user saving article failed
                    TempData["AlertMessage"] = "Article " + pricelist.bas_art_nr + " Edited Failed.";
                }
            }//Temp message to inform user something went wrong
            TempData["AlertMessage"] = "Something went wrong, " + "contact support or try again later";

            return PartialView("Edit", pricelist);
        }

        //Check if user is Assembly or Admin
        //else redirect te login
        [Authorize(Roles = "Assembly,Admin")]

        // GET: Device/Delete/5
        public ActionResult Delete(int? id)
        {
            //if no ID is selected, return BadRequest
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pricelist pricelist = db.pricelist.Find(id);
            if (pricelist == null)
            {
                //if ID is not found, return this
                return HttpNotFound();
            }
            return PartialView("Delete", pricelist);
        }

        // POST: Device/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            // find id in pricelist
            Pricelist pricelist = db.pricelist.Find(id);
            //delete id from pricelist
            db.pricelist.Remove(pricelist);
            //save changes
            db.SaveChanges();
            //Temp message when Article is deleted succesfully
            return Json(new { success = true, message = "test" }, JsonRequestBehavior.AllowGet);
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