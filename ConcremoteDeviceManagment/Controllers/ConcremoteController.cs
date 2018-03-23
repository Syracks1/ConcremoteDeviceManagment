using ConcremoteDeviceManagment.Models;
using System;
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
            ViewBag.DeviceParm = String.IsNullOrEmpty(sortOrder) ? "ConcremoteDevice_desc" : "";
            ViewBag.DeviceTypeSortParm = sortOrder == "DeviceType" ? "DeviceType_desc" : "DeviceType";
            ViewBag.ActiveSort = sortOrder == "Active" ? "Active_desc" : "Active";
            ViewBag.VersionSort = sortOrder == "ConfigVersion" ? "ConfigVersion_desc" : "ConfigVersion";
            ViewBag.ConfigDateSort = sortOrder == "ConfigDate" ? "ConfigDate_desc" : "ConfigDate";
            ViewBag.StatusSort = sortOrder == "Status" ? "Status_desc" : "Status";
            var query = from d in db.DeviceStatus
                        select d;
            switch (sortOrder)
            {
                case "ConcremoteDevice_desc":
                    query = query.OrderByDescending(s => s.ConcremoteDevice.id);
                    break;

                case "DeviceType":
                    query = query.OrderBy(s => s.DeviceConfig.DeviceType.name);
                    break;

                case "DeviceType_desc":
                    query = query.OrderByDescending(s => s.DeviceConfig.DeviceType.name);
                    break;

                case "Active":
                    query = query.OrderBy(s => s.ConcremoteDevice.Active);
                    break;

                case "Active_desc":
                    query = query.OrderByDescending(s => s.ConcremoteDevice.Active);
                    break;

                case "ConfigVersion":
                    query = query.OrderBy(s => s.DeviceConfig.VersionNr);
                    break;

                case "ConfigVersion_desc":
                    query = query.OrderBy(s => s.DeviceConfig.VersionNr);
                    break;

                case "ConfigDate":
                    query = query.OrderBy(s => s.DeviceConfig.Date);
                    break;

                case "ConfigDate_desc":
                    query = query.OrderByDescending(s => s.DeviceConfig.Date);
                    break;

                case "Status":
                    query = query.OrderBy(s => s.Device_Statustypes.id);
                    break;

                case "Status_desc":
                    query = query.OrderByDescending(s => s.Device_Statustypes.id);
                    break;

                default:
                    query = query.OrderBy(s => s.ConcremoteDevice.id);
                    break;
            }
            foreach (var item in query)
            {
                if (!string.IsNullOrEmpty(searchString))
                {
                    query = query.Where(s => s.ConcremoteDevice.id.Contains(searchString));
                }
            }
            return View(query);
        }

        [Authorize(Roles = "Assembly, Admin")]
        // GET: Concremote/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DeviceStatus deviceStatus = db.DeviceStatus.Find(id);
            if (deviceStatus == null)
            {
                return HttpNotFound();
            }
            return View(deviceStatus);
        }

        [HttpGet]
        public PartialViewResult ConfigPartial()
        {
            //var query = from d in db.Device_Pricelist
            //            select d;
            //var Device_Pricelist = new List<Device_Pricelist>(db.Device_Pricelist.Where(r => r.DeviceConfig.Device_config_id == Id));

            return PartialView();
        }

        [Authorize(Roles = "Admin")]

        // GET: Concremote/Create
        public ActionResult Create()
        {
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
            var StatusList = from d in db.Device_statustypes
                             orderby d.id
                             select new { Id = d.id, Value = d.name };

            ViewBag.StatusList = new SelectList(StatusList.Distinct(), "Id", "Value");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DeviceStatus deviceStatus = db.DeviceStatus.Find(id);
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
                db.Entry(deviceStatus).State = EntityState.Modified;
                db.Entry(concremoteDevice).State = EntityState.Modified;
                db.SaveChanges();
                TempData["AlertMessage"] = "Device Edited Successfully";
                return RedirectToAction("Index");
            }
            return View(deviceStatus);
        }

        // GET: Concremote/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ConcremoteDevice concremoteDevice = db.ConcremoteDevice.Find(id);
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
            ConcremoteDevice concremoteDevice = db.ConcremoteDevice.Find(id);
            db.ConcremoteDevice.Remove(concremoteDevice);
            db.SaveChanges();
            TempData["AlertMessage"] = "Device Deleted Successfully";
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