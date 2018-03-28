using ConcremoteDeviceManagment.Models;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace ConcremoteDeviceManagment.Controllers
{
    public class Device_statustypesController : Controller
    {
        // call in database connection
        private BasDbContext db = new BasDbContext();

        // GET: Device_statustypes
        public ActionResult Index()
        {
            //return view with given query
            return View(db.Device_statustypes.ToList());
        }

        // GET: Device_statustypes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Device_statustypes device_statustypes = db.Device_statustypes.Find(id);
            if (device_statustypes == null)
            {
                return HttpNotFound();
            }
            return View(device_statustypes);
        }

        [Authorize(Roles = "Assembly,Admin")]
        // GET: Device_statustypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Device_statustypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,name")] Device_statustypes device_statustypes)
        {
            if (ModelState.IsValid)
            {
                db.Device_statustypes.Add(device_statustypes);
                db.SaveChanges();
                TempData["AlertMessage"] = "Device Status Added Successfully";
                return RedirectToAction("Index");
            }

            return View(device_statustypes);
        }

        [Authorize(Roles = "Assembly,Admin")]
        // GET: Device_statustypes/Edit/5
        public ActionResult Edit(int? id)
        {
            //if id is null, return BadRequest
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Find id in Device_statustypes
            Device_statustypes device_statustypes = db.Device_statustypes.Find(id);
            //if id is not found
            if (device_statustypes == null)
            {
                //return this
                return HttpNotFound();
            }
            return View(device_statustypes);
        }

        // POST: Device_statustypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to,
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,name")] Device_statustypes device_statustypes)
        {
            if (ModelState.IsValid)
            {
                db.Entry(device_statustypes).State = EntityState.Modified;
                db.SaveChanges();
                TempData["AlertMessage"] = "Device Status Edited  Successfully";
                return RedirectToAction("Index");
            }
            return View(device_statustypes);
        }

        [Authorize(Roles = "Admin")]
        // GET: Device_statustypes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Device_statustypes device_statustypes = db.Device_statustypes.Find(id);
            if (device_statustypes == null)
            {
                return HttpNotFound();
            }
            return View(device_statustypes);
        }

        // POST: Device_statustypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Device_statustypes device_statustypes = db.Device_statustypes.Find(id);
            db.Device_statustypes.Remove(device_statustypes);
            db.SaveChanges();
            TempData["AlertMessage"] = "Device Status Deleted Successfully";
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