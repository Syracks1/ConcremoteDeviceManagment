using ConcremoteDeviceManagment.Models;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace ConcremoteDeviceManagment.Controllers
{
    public class DeviceTypesController : Controller
    {
        private BasDbContext db = new BasDbContext();

        // GET: DeviceTypes
        public ActionResult Index()
        {
            //return View with given query
            //displays all DeviceTypes on page
            return View(db.DeviceType.ToList());
        }

        // GET: DeviceTypes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DeviceType deviceType = db.DeviceType.Find(id);
            if (deviceType == null)
            {
                return HttpNotFound();
            }
            return View(deviceType);
        }

        //check if logged in user is Assembly or Admin
        //if false, return to login
        [Authorize(Roles = "Assembly, Admin")]

        // GET: DeviceTypes/Create
        public ActionResult Create()
        {
            //return Page
            return PartialView("Create");
        }

        //check if logged in user is Assembly or Admin
        //if false, return to login
        [Authorize(Roles = "Assembly, Admin")]

        // POST: DeviceTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "device_type_id,name")] DeviceType deviceType)
        {
            if (ModelState.IsValid)
            {
                db.DeviceType.Add(deviceType);
                db.SaveChanges();
                TempData["AlertMessage"] = "Device " + deviceType.name + " Added Successfully.";
                return Json(new { success = true });
            }
            return Json(deviceType, JsonRequestBehavior.AllowGet);
        }

        //check if logged in user is Assembly or Admin
        //if false, return to login
        [Authorize(Roles = "Assembly, Admin")]

        // GET: DeviceTypes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DeviceType deviceType = db.DeviceType.Find(id);
            if (deviceType == null)
            {
                return HttpNotFound();
            }
            return PartialView("Edit", deviceType);
        }

        //check if logged in user is Assembly or Admin
        //if false, return to login
        [Authorize(Roles = "Assembly, Admin")]

        // POST: DeviceTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "device_type_id,name")] DeviceType deviceType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(deviceType).State = EntityState.Modified;
                db.SaveChanges();
                TempData["AlertMessage"] = "Device " + deviceType.name + " Edited Successfully.";
                return RedirectToAction("Index");
            }
            return View(deviceType);
        }

        //check if logged in user is Assembly or Admin
        //if false, return to login
        [Authorize(Roles = "Assembly, Admin")]

        // GET: DeviceTypes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DeviceType deviceType = db.DeviceType.Find(id);
            if (deviceType == null)
            {
                return HttpNotFound();
            }
            return PartialView("Delete", deviceType);
        }

        // POST: DeviceTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DeviceType deviceType = db.DeviceType.Find(id);
            db.DeviceType.Remove(deviceType);
            db.SaveChanges();
            TempData["AlertMessage"] = "Device " + deviceType.name + " Deleted Successfully.";
            return Json(new { success = true });
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