﻿using ConcremoteDeviceManagment.Models;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
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
        public ActionResult Index(string sortOrder)
        {
            ViewBag.MaxStockSort = sortOrder == "Active" ? "Not Active" : "Active";
            var Device = (from d in db.DeviceConfig
                              //where d.Device_config_id ==
                          select d).DistinctBy(p => p.Device_config_id);
            switch (sortOrder)
            {
                //order CMI descending
                case "Not Active":
                    Device = Device.Where(s => s.Active == false);
                    break;

                default:
                    Device = Device.Where(s => s.Active == true);
                    break;
            }
            //create list to display in View

            return View(Device);
        }

        //check if logged in user is Assembly or Admin
        //if false, return to login
        [Authorize(Roles = "Assembly, Admin")]
        [HttpGet]
        public ActionResult Create()
        {
            //dropdownlist for Device

            var SelectedDevice = from d in db.DeviceType
                                     //where d.Price_id == d.Price_id
                                 orderby d.device_type_id
                                 select new { Id = d.device_type_id, Value = d.name };
            ViewBag.SelectedDevice = new SelectList(SelectedDevice.Distinct(), "Id", "Value");
            return PartialView("Create");
        }

        // POST: Device/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Create([Bind(Include = "Device_config_id,device_type_id")] DeviceConfig deviceConfig, Device_Pricelist Device_Parts)
        {
            //check if modelstate is valid
            if (ModelState.IsValid)
            {
                //if modelstate is valid, try this
                try
                {
                    db.DeviceConfig.Add(deviceConfig).device_type = null;
                    db.DeviceConfig.Add(deviceConfig).Date = DateTime.Now;
                    db.DeviceConfig.Add(deviceConfig).VersionNr = 0;
                    db.DeviceConfig.Add(deviceConfig).Active = true;

                    //Add data to DeviceConfig table
                    //dummy data to get Edit working
                    db.DeviceConfig.Add(deviceConfig);
                    db.Device_Parts.Add(Device_Parts).Price_id = 1;
                    db.Device_Parts.Add(Device_Parts).amount = 1;
                    db.Device_Parts.Add(Device_Parts).assembly_order = 1;
                    //save changes to database
                    db.SaveChanges();
                    //Temp message when article is added succesfully
                    return Json(new { success = true });
                }
                //if try failed, catch tempData
                catch (Exception ex)
                {
                    Trace.TraceError(ex.Message + " Something went wrong.");
                }
            }
            else
            {
            }
            return Json(deviceConfig, JsonRequestBehavior.AllowGet);
        }

        //check if logged in user is Assembly or Admin
        //if false, return to login
        [Authorize(Roles = "Assembly, Admin")]
        public ActionResult Edit(int? Id)
        {
            //create new list
            var Device_Parts = new List<Device_Pricelist>(db.Device_Parts.Where(r => r.DeviceConfig.Device_config_id == Id));

            //create new SelectList in pricelist
            //based on Price_id on values in List<Device_Parts>
            //I know this is wrong and not working

            var SelectedCMI = from Item in db.pricelist
                              where Item.Price_id == Item.Price_id
                              orderby Item.Price_id
                              select new { Id = Item.Price_id, Name = Item.bas_art_nr };
            ViewBag.SelectedCMI = new SelectList(SelectedCMI.Distinct(), "Id", "Name");

            //call viewbag based on SelectedCMI query
            //  ViewBag.SelectedCMI = new SelectList(SelectedCMI);
            // ViewBag.SelectedCMI = new SelectList(SelectedCMI, "Value", "Text");

            return View(Device_Parts);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,Device_config_id,Price_id,amount,assembly_order")]List<Device_Pricelist> Device_Parts)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var deviceConfig = db.DeviceConfig.Find(Device_Parts.First().Device_config_id);
                    //    deviceConfig.Device_config_id++;
                    //deviceConfig.device_type_id = 13;
                    deviceConfig.Active = true;
                    deviceConfig.VersionNr++;
                    deviceConfig.Date = DateTime.Now;

                    db.DeviceConfig.Add(deviceConfig);

                    foreach (var item in Device_Parts)
                    {
                        item.Device_config_id = deviceConfig.Device_config_id;
                    }

                    db.Device_Parts.AddRange(Device_Parts);

                    db.SaveChanges();
                    TempData["SuccesMessage"] = "Data is Succesfully saved";

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                      TempData["AlertMessage"] = "Saving Data Failed, " + "Try Again";
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
            return PartialView("Delete", deviceConfig);
        }

        // POST: Stock/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? id)
        {
            try
            {
                BasDbContext dc = new BasDbContext();
                //    int q = Convert.ToInt32(id);
                var Config = from emps in dc.DeviceConfig
                             join depts in dc.Device_Parts
                             on emps.Device_config_id equals depts.Device_config_id
                             where id == emps.Device_config_id
                             select emps;

                dc.DeviceConfig.RemoveRange(Config);
                // dc.Device_Parts.Remove();
                dc.SaveChanges();
                //return "ok";
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
            }
            return Json(new { success = true, message = "test" }, JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}