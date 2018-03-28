using ConcremoteDeviceManagment.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
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
                // case 1
                case "ConcremoteDevice_desc":
                    query = query.OrderByDescending(s => s.ConcremoteDevice.id);
                    break;
                //case 2
                case "DeviceType":
                    query = query.OrderBy(s => s.DeviceConfig.DeviceType.name);
                    break;
                //case 3
                case "DeviceType_desc":
                    query = query.OrderByDescending(s => s.DeviceConfig.DeviceType.name);
                    break;
                //case 4
                case "Active":
                    query = query.OrderBy(s => s.ConcremoteDevice.Active);
                    break;
                //case 5
                case "Active_desc":
                    query = query.OrderByDescending(s => s.ConcremoteDevice.Active);
                    break;
                //case 6
                case "ConfigVersion":
                    query = query.OrderBy(s => s.DeviceConfig.VersionNr);
                    break;
                //case 7
                case "ConfigVersion_desc":
                    query = query.OrderBy(s => s.DeviceConfig.VersionNr);
                    break;
                //case 8
                case "ConfigDate":
                    query = query.OrderBy(s => s.DeviceConfig.Date);
                    break;
                //case 9
                case "ConfigDate_desc":
                    query = query.OrderByDescending(s => s.DeviceConfig.Date);
                    break;
                //case 10
                case "Status":
                    query = query.OrderBy(s => s.Device_Statustypes.id);
                    break;
                //case 11
                case "Status_desc":
                    query = query.OrderByDescending(s => s.Device_Statustypes.id);
                    break;
                //case 12
                case "StatusDate":
                    query = query.OrderBy(s => s.Sign_Date);
                    break;
                //case 13
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
            // List<Device_Pricelist> ci = new List<Device_Pricelist>(db.Device_Pricelist.Where(c => c.DeviceConfig.DeviceType.name == Device && c.DeviceConfig.Active == true).OrderBy(c => c.assembly_order));
            
            //create list "ci2" with query
            List<Device_Pricelist> ci2 = new List<Device_Pricelist>(db.Device_Pricelist.Where(c => c.Device_config_id == c.DeviceConfig.Device_config_id).OrderBy(c => c.assembly_order));
            //return PartialView called "ConfigPartial" with query ci2
            return PartialView("ConfigPartial", ci2);
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

        [Authorize(Roles = "Assembly, Admin")]

        // GET: Concremote/Edit/5
        public ActionResult Edit(int? id)
        {
            //create list with devicestatus_types, based on database data
            var StatusList = from d in db.Device_statustypes
                             orderby d.id
                             select new { Id = d.id, Value = d.name };
            //pass list to Dropdownlist in Edit view
            ViewBag.StatusList = new SelectList(StatusList.Distinct(), "Id", "Value");
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
        public ActionResult Edit([Bind(Include = "id,DeviceConfig_id,Device_statustypes_id,ConcremoteDevice_id,Employee_1,Employee_2,Sign_Date,Device_statustypes_id,Active")] DeviceStatus deviceStatus, ConcremoteDevice concremoteDevice)
        {
            if (ModelState.IsValid)
            {
                //check if deviceStatus is modified
                db.Entry(deviceStatus).State = EntityState.Modified;
                //check if concremoteDevice is modified
                db.Entry(concremoteDevice).State = EntityState.Modified;
                //save modified changes to database
                db.SaveChanges();
                //Temp message when changes were succesfull
                TempData["AlertMessage"] = "Device Edited Successfully";
                //redirect to Index
                return RedirectToAction("Index");
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
            ConcremoteDevice concremoteDevice = db.ConcremoteDevice.Find(id);
            //if Id is null, return NotFound
            if (concremoteDevice == null)
            {
                return HttpNotFound();
            }
            return View(concremoteDevice);
        }

        // POST: Concremote/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            //find id from ConcremoteDevice
            ConcremoteDevice concremoteDevice = db.ConcremoteDevice.Find(id);
            //get data to be removed
            db.ConcremoteDevice.Remove(concremoteDevice);
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