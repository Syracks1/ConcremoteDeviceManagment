﻿using ConcremoteDeviceManagment.Models;
using System;
using System.Data.Entity;
using System.Linq;
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
                    stock = stock.OrderBy(s => s.Stock_amount);
                    break;
                //order current stock amount descneding
                case "current_desc":
                    stock = stock.OrderByDescending(s => s.Stock_amount);
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

        [Authorize(Roles = "Assembly,Admin")]
        [HttpGet]
        public ActionResult CreatePartial()
        {
            //dropdownlist for CMI
            var SelectedCMI = from d in db.pricelist
                              orderby d.Price_id
                              select new { Id = d.Price_id, Value = d.bas_art_nr };
            ViewBag.SelectedCMI = new SelectList(SelectedCMI.Distinct(), "Id", "Value");
            return PartialView("CreatePartial");
        }

        // POST: /Phone/Create
        [HttpPost]
        public JsonResult CreatePartial(Stock stock)
        {
            if (ModelState.IsValid)
            {
                db.Stock.Add(stock);
                db.SaveChanges();
                return Json(new { success = true });
            }
            return Json(stock, JsonRequestBehavior.AllowGet);
        }

        // GET: Stock/Details/5
        public ActionResult DetailPartial(int id)
        {
            Stock stock = db.Stock.Find(id);
            if (stock == null)
            {
                return HttpNotFound();
            }
            return PartialView("DetailPartial", stock);
        }

        //Check if user is Assembly or Admin
        //else redirect te login
        [Authorize(Roles = "Assembly,Admin")]

        // GET: Stock/Edit/5
        [HttpGet]
        public ActionResult EditPartial(int? id)
        {
            var Stock = db.Stock.Find(id);
            if (Stock == null)
            {
                return HttpNotFound();
            }

            return PartialView("EditPartial", Stock);
        }

        // POST: /Stock/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPartial(Stock stock)
        {
            if (ModelState.IsValid)
            {
                db.Entry(stock).State = EntityState.Modified;
                db.SaveChanges();
                return Json(new { success = true });
            }
            return PartialView("EditPartial", stock);
        }

        [Authorize(Roles = "Assembly,Admin")]
        // GET: Stock/Delete/5
        public ActionResult DeletePartial(int id = 0)
        {
            Stock stock = db.Stock.Find(id);
            if (stock == null)
            {
                return HttpNotFound();
            }
            return PartialView("Delete", stock);
        }

        //
        // POST: /Phone/Delete/5
        [HttpPost, ActionName("DeletePartial")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var phone = db.Stock.Find(id);
            db.Stock.Remove(phone);
            db.SaveChanges();
            return Json(new { success = true });
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