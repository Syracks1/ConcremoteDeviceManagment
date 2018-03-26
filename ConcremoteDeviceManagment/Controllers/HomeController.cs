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
            var Device = (from d in db.Device_Pricelist
                              //where d.Device_config_id ==
                          select d).DistinctBy(p => p.Device_config_id).ToList();

            return View(Device);
        }

        [Authorize(Roles = "Assembly, Admin")]
        public ActionResult Edit(int? Id)
        {
            var Device_Pricelist = new List<Device_Pricelist>(db.Device_Pricelist.Where(r => r.DeviceConfig.Device_config_id == Id));

            var SelectedCMI = (from d in db.pricelist
                                        join r in db.Device_Pricelist on d.Price_id equals r.Price_id
                                 //   where d.Price_id == r.Price_id
                               orderby d.Price_id
                               select new { Id = d.Price_id, Value = d.bas_art_nr }).Distinct();

            ViewBag.SelectedCMI = new SelectList(SelectedCMI.Distinct(), "Id", "Value");

            return View(Device_Pricelist);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(List<Device_Pricelist> Device_Pricelist, Pricelist pricelist)
        {
            if (ModelState.IsValid)
            {
                foreach (var item in Device_Pricelist)
                {
                    db.Entry(Device_Pricelist).State = EntityState.Modified;
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