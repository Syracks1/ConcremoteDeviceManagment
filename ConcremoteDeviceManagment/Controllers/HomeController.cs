using ConcremoteDeviceManagment.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using Microsoft.Ajax.Utilities;

namespace ConcremoteDeviceManagment.Controllers
{
    public class HomeController : Controller
    {
        private BasDbContext db = new BasDbContext();
        [Authorize(Roles = "Assembly, Admin")]
        public ActionResult Index()
        {
            //var SelectedDevices = new SelectList(db.DeviceType.Select(r => r.name).ToList());
            //ViewBag.SelectedDevice = SelectedDevices;
            // ViewData["SelectedDevice"] = new SelectList(db.DeviceType.Select(r => r.name).Distinct().ToList());

            var Device = (from d in db.Device_Pricelist
                              //where d.Device_config_id == 
                          select d).DistinctBy(p => p.Device_config_id).ToList();
            return View(Device);

        }  
        [Authorize(Roles = "Assembly, Admin")]
        public ActionResult Edit(int? id, string Device)
        {
            var title = from d in db.Device_Pricelist
                        
                        select d;
            ViewBag.Title = title;
            var Device_Pricelist  = new List<Device_Pricelist>(db.Device_Pricelist.Where(r => r.DeviceConfig.Device_config_id == id));
        //    for (id = 0; id < dev.; id++)
            {
            //    Device_Pricelist.Add(new Device_Pricelist());
            }


            return View(Device_Pricelist);
        }
    
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(List<Device_Pricelist> Device_Pricelist)
        {
            if (ModelState.IsValid)
            {
                foreach (var item in Device_Pricelist)
                {
                    //db.Entry(Device_Pricelist).State = EntityState.Modified;
                    db.Entry(item).State = EntityState.Modified;
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(Device_Pricelist);
        }
        //}
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult GetDevice(List<Device_Pricelist> ci)
        //{

        //    if (ModelState.IsValid)
        //    {
        //        foreach (var x in ci)
        //        {
        //            db.Device_Pricelist.Add(x);
        //        }
        //        db.SaveChanges();
        //        return RedirectToAction("CreateTry");
        //    }
        //    ViewBag.questions_id = new SelectList(db.Device_Pricelist, "questions_id", "questions_string");
        //    return View(ci);
        //}
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
