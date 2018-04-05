using ConcremoteDeviceManagment.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
            //from d in db.DeviceType
            //join c in db.DeviceConfig.Distinct()
            //on d.device_type_id equals c.device_type_id
            //select d.name);
            ViewBag.SelectedDevice = SelectedDevices;
            return View();
        }

        public class CommentViewModel
        {
            [Required]
            public string Name { get; set; }

            [Required]
            public string Email { get; set; }

            [Required]
            [Display(Name = "Enter a comment")]
            public string Comment { get; set; }
        }

        [HttpPost]
            public ActionResult CreateComment(CommentViewModel model)
            {
                //model not valid, do not save, but return current Umbraco page
                if (!ModelState.IsValid)
                {
                    //Perhaps you might want to add a custom message to the ViewBag
                    //which will be available on the View when it renders (since we're not 
                    //redirecting)	    	
                    return View("DeviceSteps");
                }

                //if validation passes perform whatever logic
                //In this sample we keep it empty, but try setting a breakpoint to see what is posted here

                //Perhaps you might want to store some data in TempData which will be available 
                //in the View after the redirect below. An example might be to show a custom 'submit
                //successful' message on the View, for example:
                TempData.Add("CustomMessage", "Your form was successfully submitted at " + DateTime.Now);

                //redirect to current page to clear the form
                return View("Create");

                //Or redirect to specific page
                //return RedirectToUmbracoPage(12345)
            }

        public PartialViewResult CreateDevice(string Device)
        {
            {
                List<Device_Pricelist> ci = new List<Device_Pricelist>(db.Device_Pricelist.Where(c => c.DeviceConfig.DeviceType.name == Device && c.DeviceConfig.Active == true && c.DeviceConfig.VersionNr == 1).OrderBy(c => c.assembly_order));
                ViewBag.Total = ci.Sum(x => x.amount * x.Pricelist.Price);

                return PartialView("CreateDevice", ci);
            }
        }

        // GET: DeviceConfig2/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    DeviceConfig deviceConfig = db.DeviceConfig.Find(id);
        //    if (deviceConfig == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(deviceConfig);
        //}
        // GET: DeviceConfig2/Create

        public ActionResult Create()
        {
            //int Device_amount = 0;
           // return RedirectToAction("DeviceSteps");
            return View();
        }



        // POST: DeviceConfig2/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Device_config_id,device_type_id,Price_id,amount,Datum,Active")] DeviceConfig deviceConfig)
        {
            //if (ModelState.IsValid)
            //{
            //    db.DeviceConfig.Add(deviceConfig);
            //    db.SaveChanges();
            //    return RedirectToAction("Index");
            //}
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

        public ActionResult DeviceSteps()
        {
            return View();
        }

        //[HttpGet]
        //public ActionResult Index(string createAmount)
        //{
        //    return Content("Hello {createAmount}");
        //}


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