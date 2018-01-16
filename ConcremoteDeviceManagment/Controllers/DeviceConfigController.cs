﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ConcremoteDeviceManagment.Models;

namespace ConcremoteDeviceManagment.Controllers
{
    public class DeviceConfigController : Controller
    {
        private Models.ConcremoteDeviceManagment db = new Models.ConcremoteDeviceManagment();
        // GET: DeviceConfig2
        public ActionResult Index()
        {
            return View(db.DeviceConfig.ToList());
        }

        // GET: DeviceConfig2/Details/5
        public ActionResult Details(int? id)
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
        // GET: DeviceConfig2/Create
        public ActionResult Create()
        {
            return View();
        }
        // POST: DeviceConfig2/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "device_type_id,Price_id,amount")] DeviceConfig deviceconfig)
        {
            if (ModelState.IsValid)
            {
                db.DeviceConfig.Add(deviceconfig);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(deviceconfig);
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
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "device_type_id,Price_id,amount")] DeviceConfig deviceConfig)
        {
            if (ModelState.IsValid)
            {
                db.Entry(deviceConfig).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(deviceConfig);
        }

        // GET: DeviceConfig2/Delete/5
        public ActionResult Delete(int? id)
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

        // POST: DeviceConfig2/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DeviceConfig deviceConfig = db.DeviceConfig.Find(id);
            db.DeviceConfig.Remove(deviceConfig);
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
    }
}
