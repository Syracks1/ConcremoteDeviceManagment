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

namespace ConcremoteDeviceManagment.Controllers
{
    public class HomeController : Controller
    {
        private BasDbContext db = new BasDbContext();

        public ActionResult Index()
        {
            var SelectedDevices = new SelectList(db.DeviceType.Select(r => r.name).ToList());
            ViewBag.SelectedDevice = SelectedDevices;
            // ViewData["SelectedDevice"] = new SelectList(db.DeviceType.Select(r => r.name).Distinct().ToList());

            //var Device = from d in db.DeviceType
            //             select d.name;
            return View();
        }

        //[HttpGet]
        //public PartialViewResult GetDevice(string Device)
        //{
        //    List<Device_Pricelist> ci = new List<Device_Pricelist>(db.Device_Pricelist.Where(c => c.DeviceConfig.DeviceType.name == Device && c.DeviceConfig.Active == true).OrderBy(c => c.assembly_order));



        //    return PartialView(ci);
        //}
        public ActionResult GetDevice()
        {
            var Device_Pricelist = new List<Device_Pricelist>();
            for (int i = 0; i < 4; i++)
            {
                Device_Pricelist.Add(new Device_Pricelist());
            }

            ViewBag.questions_id = new SelectList(db.Device_Pricelist, "questions_id", "questions_string");

            return View(Device_Pricelist);
        }
    
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetDevice(List<Device_Pricelist> questions_choices)
        {
            if (ModelState.IsValid)
            {
                foreach (var x in questions_choices)
                {
                    db.Device_Pricelist.Add(x);
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.questions_id = new SelectList(db.Device_Pricelist, "questions_id", "questions_string");
            return View(questions_choices);
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
