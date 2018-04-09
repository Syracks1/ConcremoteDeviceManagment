using ConcremoteDeviceManagment.Models;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace ConcremoteDeviceManagment.Controllers
{
    [HandleError]
    public class HomeController : Controller
    {
        //call in database connection
        private BasDbContext db = new BasDbContext();

        //check if logged in user is Assembly or Admin
        //if false, return to login
        [Authorize(Roles = "Assembly, Admin")]
        public ActionResult Index()
        {
            //create list to display in View
            var Device = (from d in db.DeviceConfig
                              //where d.Device_config_id ==
                          select d).DistinctBy(p => p.Device_config_id);

            return View(Device);
        }

        //check if logged in user is Assembly or Admin
        //if false, return to login
        [Authorize(Roles = "Assembly, Admin")]
        public ActionResult Create()
        {

            //var DeviceType = from d in db.DeviceType
            //                              //    where d.Price_id == d.Price_id
            //                          orderby d.device_type_id
            //                          select new { Id = d.device_type_id, Value = d.name };

            //dropdownlist for Device
            //var SelectedDevice = from c in db.DeviceType
            //                     orderby c.device_type_id
            //                     select new { Id = c.device_type_id, Value = c.name };
            //ViewBag.SelectedDevice = new SelectList(SelectedDevice.Distinct(), "Id", "Value");
            ViewBag.SelectedDevice = new SelectList(db.DeviceType.Distinct(), "device_type_id", "name");
            return View();
        }

        // POST: Device/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Device_config_id,device_type_id")] DeviceConfig deviceConfig, Device_Pricelist device_Pricelist)
        {            
                //check if modelstate is valid
                if (ModelState.IsValid)
                //model.DeviceList = new SelectList(db.DeviceType, "device_type_id", "name");
            {
                //if modelstate is valid, try this
                try
                {
                    db.DeviceConfig.Add(deviceConfig).Date = DateTime.Now;
                    db.DeviceConfig.Add(deviceConfig).VersionNr = 0;
                    db.DeviceConfig.Add(deviceConfig).Active = true;

                    //Add data to DeviceConfig table
                    //dummy data to get Edit working
                    db.DeviceConfig.Add(deviceConfig);
                    db.Device_Pricelist.Add(device_Pricelist).Price_id = 1;
                    db.Device_Pricelist.Add(device_Pricelist).amount = 1;
                    db.Device_Pricelist.Add(device_Pricelist).assembly_order = 1;
                    //save changes to database
                    db.SaveChanges();
                    //Temp message when article is added succesfully
                    TempData["SuccesMessage"] = "Config for " + deviceConfig.DeviceType.name + " Added Successfully.";
                    return RedirectToAction("Edit");
                }
                //if try failed, catch tempData
                catch
                {
                    TempData["AlertMessage"] = "Creating Config went wrong, " + "please try again";
                }
            }
            else
            {
                TempData["AlertMessage"] = "Something went wrong, " + "please try again";
            }
            return View(deviceConfig);
        }

        //check if logged in user is Assembly or Admin
        //if false, return to login
        [Authorize(Roles = "Assembly, Admin")]
        public ActionResult Edit(int? Id)
        {
            //create new list
            var Device_Pricelist = new List<Device_Pricelist>(db.Device_Pricelist.Where(r => r.DeviceConfig.Device_config_id == Id));

            //create new SelectList in Device_Pricelist
            var SelectedCMI = (from d in db.pricelist 
                               select new { d.Price_id, Value = d.bas_art_nr }).Distinct();

            //call viewbag based on SelectedCMI query
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
                    var deviceConfig = db.DeviceConfig.Find(Device_Pricelist.First().Device_config_id);
                //    deviceConfig.Device_config_id++;
                    //deviceConfig.device_type_id = 13;
                    deviceConfig.Active = true;
                    deviceConfig.VersionNr++;
                    deviceConfig.Date = DateTime.Now;

                    db.DeviceConfig.Add(deviceConfig);

                    foreach (var item in Device_Pricelist)
                    {
                        item.Device_config_id = deviceConfig.Device_config_id;
                    }

                    db.Device_Pricelist.AddRange(Device_Pricelist);

                    db.SaveChanges();
                    TempData["SuccesMessage"] = "Data is Succesfully saved";

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    //  TempData["AlertMessage"] = "Saving Data Failed, " + "Try Again";
                    Trace.TraceError(ex.Message + " SendGrid probably not configured correctly.");

                }

            }
            return View();
        }
        [Authorize(Roles = "Assembly, Admin")]
        // GET: Stock/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DeviceConfig deviceConfig = db.DeviceConfig.Find(id);
            if (deviceConfig == null)
            {
                return HttpNotFound();
            }
            return View(deviceConfig);
        }

        // POST: Stock/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DeviceConfig deviceConfig = db.DeviceConfig.Find(id);
            db.DeviceConfig.Remove(deviceConfig);
         //   db.Device_Pricelist.Remove(Device_config_id);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}