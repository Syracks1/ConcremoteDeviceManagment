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

        [Authorize(Roles = "Assembly,Admin")]
        // GET: DeviceTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        [Authorize]
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
                TempData["AlertMessage"] = "Device Type Added Successfully";
                return RedirectToAction("Index");
            }

            return View(deviceType);
        }

        [Authorize(Roles = "Assembly,Admin")]
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
            return View(deviceType);
        }

        [Authorize(Roles = "Admin")]
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
                TempData["AlertMessage"] = "Device Type Edited Successfully";
                return RedirectToAction("Index");
            }
            return View(deviceType);
        }

        [Authorize(Roles = "Admin")]
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
            return View(deviceType);
        }

        // POST: DeviceTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DeviceType deviceType = db.DeviceType.Find(id);
            db.DeviceType.Remove(deviceType);
            db.SaveChanges();
            TempData["AlertMessage"] = "Device Type Deleted Successfully";
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