using ConcremoteDeviceManagment.Models;
using Microsoft.Ajax.Utilities;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace ConcremoteDeviceManagment.Controllers
{
    public class HomeController : Controller
    {
        private BasDbContext db = new BasDbContext();

        [Authorize(Roles = "Assembly, Admin")]
        public ActionResult Index()
        {
            //var SelectedDevices = new SelectList(db.DeviceType.Select(r => r.name).ToList());
            //ViewBag.SelectedDevice = SelectedDevices;
            // ViewData["SelectedDevice"] = new SelectList(db.DeviceType.Select(r => r.name).Distinct().ToList());

            var Device = (from d in db.Device_Pricelist
                              //where d.Device_config_id ==
                          select d).DistinctBy(p => p.Device_config_id).ToList();
            return View(Device);
        }

        [Authorize(Roles = "Assembly, Admin")]
        public ActionResult Edit(int? Id )
        {
            var Device_Pricelist = new List<Device_Pricelist>(db.Device_Pricelist.Where(r => r.DeviceConfig.Device_config_id == Id));

            var SelectedCMI = (from d in db.pricelist
                           //   where d.Price_id == d.Price_id
                              orderby d.Price_id
                              select new { Id = d.Price_id, Value = d.bas_art_nr }).Distinct();

            ViewBag.SelectedCMI = new SelectList(SelectedCMI.Distinct(), "Id", "Value");

            return View(Device_Pricelist);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(List<Device_Pricelist> Device_Pricelist, Pricelist pricelist, FormCollection formCollection)
        //   public ActionResult Edit([Bind(Include = "id, Device_config_id,Price_id,amount,assembly_order")] List<Device_Pricelist> device_Pricelist, FormCollection formCollection)
        {
            pricelist.Price_id = int.Parse(formCollection["SelectedCMI"]);
            //.Price_id = int.Parse(formCollection["SelectedCMI"]);
            if (ModelState.IsValid)
            {
                foreach (var item in Device_Pricelist)
                {
                    //db.Entry(Device_Pricelist).State = EntityState.Modified;
                    db.Entry(item).State = EntityState.Modified;
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(Device_Pricelist);
        }
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}