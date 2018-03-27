using ConcremoteDeviceManagment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ConcremoteDeviceManagment.Controllers
{
    public class SliderController : Controller
    {
        // GET: Slider
        [Authorize(Roles = "Assembly,Admin")]
        public ActionResult Index()
        {
            using (BasDbContext db = new BasDbContext())
            {
                return View(db.gallery.ToList());
            }
            //return View();
        }

        //Add Images in slider

        public ActionResult AddImage()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddImage(HttpPostedFileBase ImagePath)
        {
            if (ImagePath != null)
            {
                //System.Drawing.Image img = System.Drawing.Image.FromStream(ImagePath.InputStream);
                //if ((img.Width != 800) || (img.Height != 356))
                //{
                //    ModelState.AddModelError("", "Image resolution must be 800 x 356 pixels");
                //    return View();
                //}
                // Upload your pic
                string pic = System.IO.Path.GetFileName(ImagePath.FileName);
                string path = System.IO.Path.Combine((@"\\WEBSERVER03\WEBDEV$\cdm\Content\images\"), pic);
                ImagePath.SaveAs(path);
                using (BasDbContext db = new BasDbContext())
                {
                    gallery gallery = new gallery { ImagePath = @"\\WEBSERVER03\WEBDEV$\cdm\Content\images\" + pic };
                    db.gallery.Add(gallery);
                    db.SaveChanges();
                }
            }
            return RedirectToAction("Index");
        }

        // Delete Multiple Images
        public ActionResult DeleteImages()
        {
            using (BasDbContext db = new BasDbContext())
            {
                return View(db.gallery.ToList());
            }
        }

        [HttpPost]
        public ActionResult DeleteImages(IEnumerable<int> ImagesIDs)
        {
            using (BasDbContext db = new BasDbContext())
            {
                try { 
                    foreach (var id in ImagesIDs)
                    {
                        var image = db.gallery.Single(s => s.ID == id);
                        string imgPath = Server.MapPath(image.ImagePath);
                        db.gallery.Remove(image);
                        if (System.IO.File.Exists(imgPath))
                            System.IO.File.Delete(imgPath);
                    }
                    db.SaveChanges();
                    TempData["SuccesMessage"] = "Image Deleted Succesfully";
                }
                catch
                {
                    TempData["AlertMessage"] = "Select an image to delete";
                }
            }
            return RedirectToAction("DeleteImages");
        }
    }
}