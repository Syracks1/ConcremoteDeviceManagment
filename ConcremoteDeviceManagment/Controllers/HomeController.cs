using ConcremoteDeviceManagment.Models;
using Microsoft.Ajax.Utilities;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace ConcremoteDeviceManagment.Controllers
{
    public class HomeController : Controller
    {
        private BasDbContext db = new BasDbContext();

        [Authorize(Roles = "Assembly, Admin")]
        public ActionResult Index()
        {
            var Device = (from d in db.DeviceConfig
                              //where d.Device_config_id ==
                          select d).DistinctBy(p => p.Device_config_id).ToList();

            return View(Device);
        }

       
        [Authorize(Roles = "Assembly, Admin")]

        public ActionResult Create()
        {
            //dropdownlist for Device
            var SelectedDevice = from d in db.DeviceType
                                     //    where d.Price_id == d.Price_id
                                 orderby d.name
                                 select new { Id = d.device_type_id, Value = d.name };
            ViewBag.SelectedDevice = new SelectList(SelectedDevice.Distinct(), "Id", "Value");
            return View();
        }

        // POST: Device/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Device_config_id,device_type_id,Active,VersionNr,Date")] DeviceConfig deviceConfig, Device_Pricelist device_Pricelist)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //Add data to DeviceConfig table
                    db.DeviceConfig.Add(deviceConfig);
                    db.Device_Pricelist.Add(device_Pricelist);
                    //save changes to database
                    db.SaveChanges();
                    //Temp message when article is added succesfully
                    TempData["SuccesMessage"] = "Config for " + deviceConfig.DeviceType.name + " Added Successfully.";
                    return RedirectToAction("Edit");
                }
                catch
                {
                    TempData["AlertMessage"] = "Creating Config went wrong, " + "please try again";
                }
            }
            return View(deviceConfig);
        }
        [Authorize(Roles = "Assembly, Admin")]
        public ActionResult Edit(int? Id)
        {
            var Device_Pricelist = new List<Device_Pricelist>(db.Device_Pricelist.Where(r => r.DeviceConfig.Device_config_id == Id));

            var SelectedCMI = (from d in db.pricelist
                               where d.Price_id == d.Price_id
                               orderby d.Price_id
                               select new { d.Price_id, Value = d.bas_art_nr }).Distinct();

            ViewBag.SelectedCMI = new SelectList(SelectedCMI.Distinct(), "Price_id", "Value");

            return View(Device_Pricelist);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,Device_config_id,Price_id,amount,assembly_order")]List<Device_Pricelist> Device_Pricelist)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    foreach (var item in Device_Pricelist)
                    {
                        db.Entry(item).State = EntityState.Added;
                    }
                    db.SaveChanges();
                    TempData["SuccesMessage"] = "Data is Succesfully saved";
                    return RedirectToAction("Index");
                }
                catch
                {
                    TempData["AlertMessage"] = "Saving Data Failed, " + "Try Again";
                }
            }
            // ViewBag.SelectedCMI = new SelectList(db.pricelist.Distinct(), "Price_id");
            return View(Device_Pricelist);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}