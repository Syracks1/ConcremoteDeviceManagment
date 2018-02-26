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
            return View();
        }

        [HttpGet]
        public PartialViewResult GetDevice(string Device)
        {
          //  List<Device_Pricelist> ci = new List<Device_Pricelist>(db.Device_Pricelist.Where(c => c.DeviceConfig.DeviceType.name == Device).OrderBy(c => c.assembly_order));
            List<Device_Pricelist> ci = new List<Device_Pricelist>(db.Device_Pricelist.Where(c => c.DeviceConfig.DeviceType.name == Device && c.DeviceConfig.Active == true).OrderBy(c => c.assembly_order));

            return PartialView(ci);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetDevice()
        {
            var Device_Pricelist = new List<Device_Pricelist>();
            for (int i = 0; i < 4; i++)
            {
                Device_Pricelist.Add(new Device_Pricelist());
            }

            //ViewBag.questions_id = new SelectList(db.Device_Pricelist, "questions_id", "questions_string");

            return View(Device_Pricelist);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateTry(List<Device_Pricelist> Device_Pricelist)
        {
            if (ModelState.IsValid)
            {
                foreach (var x in Device_Pricelist)
                {
                    db.Device_Pricelist.Add(x);
                }
                db.SaveChanges();
                return RedirectToAction("CreateTry");
            }
           // ViewBag.questions_id = new SelectList(db.Device_Pricelist, "questions_id", "questions_string");
            return View(Device_Pricelist);
        }
        public ActionResult Create()
        {
            ViewBag.device_type_id = new SelectList(db.DeviceType.Select(r => r.name).Distinct().ToList());
            //var EditDevice = new SelectList(db.pricelist.Select(c => c.bas_art_nr).Distinct().ToList());
            ViewBag.EditDevice = new SelectList(db.pricelist.Select(c => c.bas_art_nr).Distinct().ToList());
            return View();  
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,Device_config_id,device_type_id,Price_id,amount,assembly_order,Datum")] DeviceConfig DeviceConfig)
        {

          //  ViewBag.EditDevice = EditDevice;
            try
            {
                if (ModelState.IsValid)
                {
                    db.DeviceConfig.Add(DeviceConfig);
                   // TempData["AlertMessage"] = "Changes saved succesfully";
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                ViewBag.device_type_id = new SelectList(db.DeviceType, "device_type_id", "device_type", DeviceConfig.device_type_id);
                //ViewBag.EditDevice = new SelectList(db.pricelist, "Price_id", "bas_art_nr", Device_Pricelist.Price_id);

            }
            catch (RetryLimitExceededException)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again");
            }
            //        PopulateDeviceDropDownList(DeviceConfig.device_type_id);

            return View(DeviceConfig);
        }
        // GET: DeviceConfig/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DeviceConfig DeviceConfig = db.DeviceConfig.Find(id);
            if (DeviceConfig == null)
            {
                return HttpNotFound();
            }
            return View(DeviceConfig);
        }

        // POST: DeviceConfig/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DeviceConfig DeviceConfig = db.DeviceConfig.Find(id);
            db.DeviceConfig.Remove(DeviceConfig);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        private void PopulateDeviceDropDownList()
        {
            var SelectedDevices = new SelectList(db.DeviceType.Select(r => r.name).Distinct().ToList());
            //ViewBag.SelectedDevices = new SelectList(db.DeviceType.Select(r => r.name).Distinct().ToList());
            ViewBag.SelectedDevice = SelectedDevices;
            // ViewData["SelectedDevice"] = SelectedDevices;
            //if (SelectedDevice == null)
            //{
            //    SelectedDevice = HtmlHelper.GetSelectData(db.DeviceType);
            //    usedViewData = true;
            //}
        }
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
