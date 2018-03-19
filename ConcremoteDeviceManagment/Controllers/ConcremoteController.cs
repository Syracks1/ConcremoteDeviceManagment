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

        //[Authorize(Roles = "Assembly, Admin")]
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
        public PartialViewResult ConfigPartial(string Device)
        {
            var query = from d in db.Device_Pricelist
                        select d;
            // List<Device_Pricelist> ci = new List<Device_Pricelist>(db.Device_Pricelist.Where(r => r.Device_config_id == r.DeviceConfig.Device_config_id));

            return PartialView("ConfigPartial", query);
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

        [Authorize(Roles = "Admin")]

        // GET: Concremote/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DeviceStatus deviceStatus_ExtraInfo = db.DeviceStatus.Find(id);
            if (deviceStatus_ExtraInfo == null)
            {
                return HttpNotFound();
            }
            var StatusQuery = from d in db.DeviceStatus
                              where d.Device_statustypes_id == d.Device_Statustypes.id
                              orderby d.Device_Statustypes.id
                              select new { Id = d.Device_Statustypes.id, Value = d.Device_Statustypes.name };
            ViewBag.StatusList = new SelectList(StatusQuery.Distinct(), "Id", "Value");
            return View(deviceStatus_ExtraInfo);
        }

        //var Users = (from d in db.AspNetUserRoles
        //             join st in db.AspNetUsers on d.UserId equals st.Id
        //             join dt in db.AspNetRoles on d.RoleId equals dt.Id
        //             where d.RoleId == d.AspNetRoles.Id && d.UserId == d.AspNetUsers.Id
        //             select new { st.Email, st.LockoutEndDateUtc, dt.Name }).ToList();

        // POST: Concremote/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,DeviceConfig_id,Device_statustypes_id,ConcremoteDevice_id,Employee_1,Employee_2,Sign_Date,Active")] DeviceStatus deviceStatus, ConcremoteDevice concremoteDevice)
        {
            var Conn = (from d in db.DeviceStatus
                        join s in db.Device_statustypes on d.Device_statustypes_id equals s.id
                        join b in db.ConcremoteDevice on d.ConcremoteDevice_id equals b.id
                        join c in db.DeviceConfig on d.DeviceConfig_id equals c.Device_config_id
                        select new { s.id, /*d.Device_statustypes_id*/ Model = d.id });

            if (ModelState.IsValid)
            {
                db.Entry(deviceStatus).State = EntityState.Modified;
         //       db.Entry(concremoteDevice).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            //    ViewBag.device_type_id = new SelectList(db.DeviceType, "device_type_id", "device_type", concremoteDevice.id);
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

        //// GET: Concremote/Detail/2
        //public ActionResult Detail(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    ConcremoteDevice concremoteDevice = db.ConcremoteDevice.Find(id);
        //    if (concremoteDevice == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(concremoteDevice);
        //}

        //// POST: Concremote/Detail/2
        //[HttpPost, ActionName("Detail")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DetailConfirmed(int id)
        //{
        //    ConcremoteDevice concremoteDevice = db.ConcremoteDevice.Find(id);
        //    db.ConcremoteDevice.Remove(concremoteDevice);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}
    }
}