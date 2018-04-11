using ConcremoteDeviceManagment.Models;
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
            //dropdownlist for Device

            var SelectedDevice = from d in db.DeviceType
                                     //where d.Price_id == d.Price_id
                                 orderby d.device_type_id
                                 select new { Id = d.device_type_id, Value = d.name };
            ViewBag.SelectedDevice = new SelectList(SelectedDevice.Distinct(), "Id", "Value");
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
                    db.Device_Pricelist.Add(device_Pricelist).Price_id = 1;
                    db.Device_Pricelist.Add(device_Pricelist).amount = 1;
                    db.Device_Pricelist.Add(device_Pricelist).assembly_order = 1;
                    //save changes to database
                    db.SaveChanges();
                    //Temp message when article is added succesfully
                    TempData["SuccesMessage"] = "Config Added Successfully.";
                    return RedirectToAction("Index");
                }
                //if try failed, catch tempData
                catch (Exception ex)
                {
                    TempData["AlertMessage"] = "Creating Config went wrong, " + "please try again";
                    Trace.TraceError(ex.Message + " Something went wrong.");
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

            //create new SelectList in pricelist
            //based on Price_id on values in List<Device_Pricelist>
            //I know this is wrong and not working

            foreach (var item in (db.Device_Pricelist.Where(r => r.DeviceConfig.Device_config_id == Id))) 
            {
                var SelectedCMI = from pl in db.pricelist
                                 where pl.Price_id == pl.Price_id     //join dl in Device_Pricelist o
                                  orderby pl.Price_id
                                  select new SelectListItem { Value = pl.Price_id.ToString(), Text = pl.bas_art_nr };
                ViewBag.SelectedCMI = new SelectList(SelectedCMI, "Value", "Text");
            }
            //call viewbag based on SelectedCMI query
            //  ViewBag.SelectedCMI = new SelectList(SelectedCMI);
           // ViewBag.SelectedCMI = new SelectList(SelectedCMI, "Value", "Text");
            
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
            try
            {
                int q = Convert.ToInt32(id);
                var deviceConfig = from dc in db.DeviceConfig
                                   join pl in db.Device_Pricelist
                                   on dc.Device_config_id equals pl.Device_config_id
                                   where q == dc.Device_config_id
                                   select dc;
                //db.SaveChanges();
                // db.DeviceConfig.Remove();
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message + " SendGrid probably not configured correctly.");
            }
            return View("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}