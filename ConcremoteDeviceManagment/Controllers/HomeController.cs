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

namespace ConcremoteDeviceManagment.Controllers
{
    public class HomeController : Controller
    {
        private BasDbContext db = new BasDbContext();

        public ActionResult Index()
        {
            var SelectedDevices = new SelectList(db.DeviceType.Select(r => r.name).Distinct().ToList());
            ViewBag.SelectedDevice = SelectedDevices;
           // ViewData["SelectedDevice"] = new SelectList(db.DeviceType.Select(r => r.name).Distinct().ToList());
            return View();
        }

        //}
        [HttpGet]
        public JsonResult FillDeviceInfo(decimal Price, string description)
        {
            ;
            var ret = (from e in db.pricelist
                       join c in db.pricelist on e.Price_id equals c.Price_id
                     //  where e.Price_id == Price && e.Price_id == description
                       select new Pricelist
                       {
                           Price = e.Price,
                           description = e.description

                       }).FirstOrDefault();
            return Json(ret);
        }
        [HttpGet]
        public PartialViewResult GetDevice(string Device)
        {
            List<DeviceConfig> ci = new List<DeviceConfig>(db.DeviceConfig.Where(c => c.DeviceType.name == Device == c.Pricelist.Active == true).OrderBy(c => c.assembly_order));
            return PartialView("GetDevice", ci);
        }
        //public ActionResult Save(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    DeviceConfig DeviceConfig = db.DeviceConfig.Find(id);
        //    if (DeviceConfig == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(DeviceConfig);
        //}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetDevice(List<DeviceConfig> ci)
        {
            if (ModelState.IsValid)
            {
                using (BasDbContext db = new BasDbContext())
                {
                    try
                    {
                        foreach (var i in ci)
                        {
                            db.DeviceConfig.Add(i);
                        }
                        db.SaveChanges();
                        ViewBag.Message = "Data successfully saved!";
                        ModelState.Clear();
                    }
                    catch
                    {
                        ModelState.AddModelError("", "Unable to save changes. Try again");
                    }
                   // ci = new List<DeviceConfig>(db.DeviceConfig.Where(c => c.Active == true).OrderBy(c => c.assembly_order));
                }
            }
            return View(ci);
        }
        //private static HttpStatusCode GetBadRequest()
        //{
        //    return HttpStatusCode.BadRequest;
        //}

        //     public ActionResult EditCMI(string CMI)

        public ActionResult Create()
        {
   //         PopulateDeviceDropDownList();
            return View();  
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Device_config_id,device_type_id,Price_id,amount,assembly_order")] DeviceConfig DeviceConfig)
        {
                var EditDevice = new SelectList(db.pricelist.Select(c => c.bas_art_nr).Distinct().ToList());

            ViewBag.EditDevice = EditDevice;
            try
            {
                if (ModelState.IsValid)
                {
                    db.DeviceConfig.Add(DeviceConfig);
                    TempData["AlertMessage"] = "Changes saved succesfully";
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch(RetryLimitExceededException)
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
