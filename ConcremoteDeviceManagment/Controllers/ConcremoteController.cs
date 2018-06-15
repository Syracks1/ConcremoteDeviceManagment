using ConcremoteDeviceManagment.Models;
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
    public class ConcremoteController : Controller
    {
        private BasDbContext db = new BasDbContext();
        //[Authorize(Roles = "BAS employee, Assembly, Admin")]
        // GET: Concremote

        public ActionResult Index(string sortOrder, string searchString)
        {
            //filter parameters on Device Index page
            ViewBag.DeviceParm = String.IsNullOrEmpty(sortOrder) ? "ConcremoteDevice_desc" : "";
            ViewBag.DeviceTypeSortParm = sortOrder == "DeviceType" ? "DeviceType_desc" : "DeviceType";
            ViewBag.ActiveSort = sortOrder == "Active" ? "Active_desc" : "Active";
            ViewBag.VersionSort = sortOrder == "ConfigVersion" ? "ConfigVersion_desc" : "ConfigVersion";
            ViewBag.ConfigDateSort = sortOrder == "ConfigDate" ? "ConfigDate_desc" : "ConfigDate";
            ViewBag.StatusSort = sortOrder == "Status" ? "Status_desc" : "Status";
            ViewBag.StatusDateSort = sortOrder == "StatusDate" ? "StatusDate_desc" : "StatusDate";
            //call in database for data posting
            var query = from d in db.DeviceStatus
                        select d;
            //switch for sort parameters, not going to explain them all
            switch (sortOrder)
            {
                // case 1: ConcremoteDevice ID descending
                case "ConcremoteDevice_desc":
                    query = query.OrderByDescending(s => s.ConcremoteDevice.id);
                    break;
                //case 2: DeviceType name ascending
                case "DeviceType":
                    query = query.OrderBy(s => s.DeviceConfig.DeviceType.name);
                    break;
                //case 3: DeviceType name descending
                case "DeviceType_desc":
                    query = query.OrderByDescending(s => s.DeviceConfig.DeviceType.name);
                    break;
                //case 4: Active ascending
                case "Active":
                    query = query.OrderBy(s => s.ConcremoteDevice.Active);
                    break;
                //case 5: Active descending
                case "Active_desc":
                    query = query.OrderByDescending(s => s.ConcremoteDevice.Active);
                    break;
                //case 6: ConfigVersion ascending
                case "ConfigVersion":
                    query = query.OrderBy(s => s.DeviceConfig.VersionNr);
                    break;
                //case 7: ConfigVersion descending
                case "ConfigVersion_desc":
                    query = query.OrderBy(s => s.DeviceConfig.VersionNr);
                    break;
                //case 8: ConfigDate ascending
                case "ConfigDate":
                    query = query.OrderBy(s => s.DeviceConfig.Date);
                    break;
                //case 9: ConfigDate descending
                case "ConfigDate_desc":
                    query = query.OrderByDescending(s => s.DeviceConfig.Date);
                    break;
                //case 10: DeviceStatusType ascending
                case "Status":
                    query = query.OrderBy(s => s.Device_Statustypes.id);
                    break;
                //case 11: DeviceStatusType descending
                case "Status_desc":
                    query = query.OrderByDescending(s => s.Device_Statustypes.id);
                    break;
                //case 12: StatusDate ascending
                case "StatusDate":
                    query = query.OrderBy(s => s.Sign_Date);
                    break;
                //case 13: StatusDate descending
                case "StatusDate_desc":
                    query = query.OrderByDescending(s => s.Sign_Date);
                    break;
                //case 14: default
                default:
                    query = query.OrderBy(s => s.ConcremoteDevice.id);
                    break;
            }
            //check if item contains searchstring
            foreach (var item in query)
            {
                if (!string.IsNullOrEmpty(searchString))
                {
                    query = query.Where(s => s.ConcremoteDevice.id.Contains(searchString));
                }
            }
            return View(query);
        }

        //check if user is Assembly or Admin
        //if false, user is redirected to Login page
        [Authorize(Roles = "Assembly, Admin")]
        // GET: Concremote/Details/5
        public ActionResult Details(int? id)
        {
            //if ID is null, return BadRequest
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //find ID in DeviceStatus
            DeviceStatus deviceStatus = db.DeviceStatus.Find(id);
            //if nothing is found, return NotFound page
            if (deviceStatus == null)
            {
                return HttpNotFound();
            }
            return View(deviceStatus);
        }

        public PartialViewResult ConfigPartial(int? id)
        {
            //create list "ci2" with query
            //List<Device_Pricelist> ci2 = new List<Device_Pricelist>(db.DeviceStatus.Where(c => c.DeviceConfig_id == c.DeviceConfig.Device_config_id).OrderBy(c => c.DeviceConfig_id));
            List<Device_Pricelist> ci = new List<Device_Pricelist>(db.Device_Parts.Where(pl => pl.Device_config_id == db.DeviceConfig.Where(dc => dc.Device_config_id == id).OrderByDescending(dc => dc.VersionNr).FirstOrDefault().Device_config_id));

            //   var Device_Pricelist = new List<Device_Pricelist>(db.DeviceStatus.Where(r => r.DeviceConfig_id == db.Device_Pr));

            //return PartialView called "ConfigPartial" with query ci2
            return PartialView("ConfigPartial", ci);
        }

        //Check if logged in user is Assembly or Admin
        //if false, return to Login page
        [Authorize(Roles = "Assembly,Admin")]

        // GET: Concremote/Create
        public ActionResult Create()
        {
            //Create list with DeviceType names
            ViewBag.device_type_id = new SelectList(db.DeviceType.Select(r => r.name).Distinct().ToList());
            return View();
        }

        // POST: Concremote/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,device_id,imei,active,oldsystem_concremote,Allowvalidation,device_type_id")] ConcremoteDevice concremoteDevice)
        {
            if (ModelState.IsValid)
            {
                db.ConcremoteDevice.Add(concremoteDevice);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.device_type_id = new SelectList(db.DeviceType, "device_type_id", "device_type", concremoteDevice.id);
            return View(concremoteDevice);
        }

        //Check if logged in user is Assembly or Admin
        //if false, return to Login page
        [Authorize(Roles = "Assembly, Admin")]

        // GET: Concremote/Edit/5
        public ActionResult Edit(int? id)
        {
            //create list with devicestatus_types, based on database data
            var StatusList = from d in db.Device_statustypes
                             orderby d.id
                             select new { Id = d.id, Value = d.name };

            var PersonList = from d in db.AspNetUsers
                             orderby d.Id
                             select new { Id = d.Email, Value = d.Email };
            //pass list to Dropdownlist in Edit view
            ViewBag.StatusList = new SelectList(StatusList.Distinct(), "Id", "Value");
            ViewBag.PersonList = new SelectList(PersonList.Distinct(), "Id", "Value");

            //if ID is null, return BadRequest
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Find Id in deviceStatus
            DeviceStatus deviceStatus = db.DeviceStatus.Find(id);
            //if Id is null, return NotFound
            if (deviceStatus == null)
            {
                return HttpNotFound();
            }

            return View(deviceStatus);
        }

        // POST: Concremote/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,DeviceConfig_id,Device_statustypes_id,ConcremoteDevice_id,Employee_1,Employee_2,Device_statustypes_id,Sign_Date,Active")] DeviceStatus deviceStatus, ConcremoteDevice concremoteDevice)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //set new Sign_Date
                    deviceStatus.Sign_Date = DateTime.Now;

                    //check if deviceStatus is modified
                    db.Entry(deviceStatus).State = EntityState.Modified;

                    //check if concremoteDevice is modified
                    db.Entry(concremoteDevice).State = EntityState.Modified;

                    //save modified changes to database
                    db.SaveChanges();
                    
                    //Temp message when changes were succesfull
                    TempData["AlertMessage"] = deviceStatus.ConcremoteDevice.id + " Edited Successfully";
                    //redirect to Index
                    return RedirectToAction("Index");
                }

                catch (Exception ex)
                {
                    //  TempData["AlertMessage"] = "Saving Data Failed, " + "Try Again";
                    Trace.TraceError(ex.Message + " SendGrid probably not configured correctly.");
                }
            }
            return View(deviceStatus);
        }

        // GET: Concremote/Delete/5
        public ActionResult Delete(int? id)
        {
            //if id is null, return BadRequest
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Find Id in concremoteDevice
            DeviceStatus deviceStatus = db.DeviceStatus.Find(id);
            //if Id is null, return NotFound
            if (deviceStatus == null)
            {
                return HttpNotFound();
            }
            return View(deviceStatus);
        }

        // POST: Concremote/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? id)
        {
            //find id from ConcremoteDevice
            DeviceStatus deviceStatus = db.DeviceStatus.Find(id);
            //get data to be removed
            db.DeviceStatus.Remove(deviceStatus);
            //remove data and save changes to database
            db.SaveChanges();
            //Temp message to inform user that device is deletedd succesfully
            TempData["AlertMessage"] = "Device Deleted Successfully";
            //redirect to index
            return RedirectToAction("Index");
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