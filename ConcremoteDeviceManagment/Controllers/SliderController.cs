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
        //Checking if logged in user is Assembly or Admin
        [Authorize(Roles = "Assembly,Admin")]
        public ActionResult Index()
        {
            //using Database connection
            using (BasDbContext db = new BasDbContext())
            {
                //post all item in gallery table to View
                return View(db.gallery.ToList());
            }
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
                // Upload your image, only jpg, jpeg, png allowed
                string pic = System.IO.Path.GetFileName(ImagePath.FileName);
                //Path giving to image
                string path = System.IO.Path.Combine((@"\\WEBSERVER03\WEBDEV$\cdm\Content\images\"), pic);
                //Save image with given path
                ImagePath.SaveAs(path);
                //using Database connection
                using (BasDbContext db = new BasDbContext())
                {
                    //path to save image to folder and Database
                    gallery gallery = new gallery { ImagePath = @"\\WEBSERVER03\WEBDEV$\cdm\Content\images\" + pic };
                    //Adding image to Gallery table
                    db.gallery.Add(gallery);
                    //Image saved to Gallery table
                    db.SaveChanges();
                }
            }
            return RedirectToAction("Index");
        }

        // Delete Multiple Images
        public ActionResult DeleteImages()
        {
            //using Database connection
            using (BasDbContext db = new BasDbContext())
            {
                //Get al rows from Gallery table to list in View
                return View(db.gallery.ToList());
            }
        }

        [HttpPost]
        public ActionResult DeleteImages(IEnumerable<int> ImagesIDs)
        {
            //using Database connection
            using (BasDbContext db = new BasDbContext())
            {
                //try this. If it is not working, code goes to catch
                try
                { 
                    foreach (var id in ImagesIDs)
                    {
                        var image = db.gallery.Single(s => s.ID == id);
                        string imgPath = Server.MapPath(image.ImagePath);
                        //remove selected image
                        db.gallery.Remove(image);
                        //if path exists
                        if (System.IO.File.Exists(imgPath))
                            System.IO.File.Delete(imgPath);
                    }
                    //save changes to database
                    db.SaveChanges();
                    //Temporarily message to inform user Image is deleted succesfully
                    TempData["SuccesMessage"] = "Image Deleted Succesfully";
                }
                //If try doesn't work, this code will be executed to prefent Error dropout
                catch
                {
                    //Temporarily message to inform user something failed
                    TempData["AlertMessage"] = "Select an image to delete";
                }
            }
            return RedirectToAction("DeleteImages");
        }
    }
}