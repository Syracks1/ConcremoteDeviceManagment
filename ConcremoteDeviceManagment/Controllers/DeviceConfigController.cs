using ConcremoteDeviceManagment.Models;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace ConcremoteDeviceManagment.Controllers
{
    public class DeviceConfigController : Controller
    {
        //call in databse connection
        private BasDbContext db = new BasDbContext();

        //check if logged in user is Assembly or Admin
        //if false, return to login
        [Authorize(Roles = "Assembly, Admin")]
        public ActionResult Index()
        {
            var SelectedDevices = new SelectList(db.DeviceType.Select(r => r.name).ToList());
            ViewBag.SelectedDevice = SelectedDevices;
            return View();
        }

        public PartialViewResult CreateDevice(string Device)
        {
            {
                List<Device_Pricelist> ci = new List<Device_Pricelist>(db.Device_Pricelist.Where(pl => pl.Device_config_id == db.DeviceConfig.Where(dc => dc.DeviceType.name == Device).OrderByDescending(dc => dc.VersionNr).FirstOrDefault().Device_config_id));
                //    ViewBag.Max = ci.Max(c => c.DeviceConfig.VersionNr);
                ViewBag.Total = ci.Sum(x => x.amount * x.Pricelist.Price);

                return PartialView("CreateDevice", ci);
            }
        }

        public ActionResult Create()
        {
            return View();
        }

        // POST: DeviceConfig2/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Device_config_id,device_type_id,Price_id,amount,Datum,Active")] DeviceConfig deviceConfig)
        {
            return View(deviceConfig);
        }

        // GET: DeviceConfig2/Edit/5
        public ActionResult Edit(int? id)
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

        // POST: DeviceConfig2/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Device_config_id,device_type_id,Price_id,amount")] DeviceConfig deviceConfig)
        {
            if (ModelState.IsValid)
            {
                db.Entry(deviceConfig).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(deviceConfig);
        }

        [HttpPost]
        public ActionResult DeviceSteps()
        {
            ViewBag.value1 = Request["createAmount"];

            var z = Request["createAmount"];

            //List<Device_Pricelist> ci = new List<Device_Pricelist>(db.Device_Pricelist.OrderBy(c => c.assembly_order));
            //ViewBag.Total = ci.Sum(x => x.amount * x.Pricelist.Price);

            //return PartialView("CreateDevice", ci);
            return View();
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