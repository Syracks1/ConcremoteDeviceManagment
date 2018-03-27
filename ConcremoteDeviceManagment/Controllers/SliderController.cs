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
        public ActionResult Index()
        {
            using (BasDbContext db = new BasDbContext())
            {
                return View(db.Galleries.ToList());
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
                // You can skip this block, because it is only to force the user to upload specific resolution pics
                System.Drawing.Image img = System.Drawing.Image.FromStream(ImagePath.InputStream);
                if ((img.Width != 800) || (img.Height != 356))
                {
                    ModelState.AddModelError("", "Image resolution must be 800 x 356 pixels");
                    return View();
                }

                // Upload your pic
                string pic = System.IO.Path.GetFileName(ImagePath.FileName);
                string path = System.IO.Path.Combine(Server.MapPath("~/Shared/images"), pic);
                ImagePath.SaveAs(path);
                using (BasDbContext db = new BasDbContext())
                {
                    Gallery gallery = new Gallery { ImagePath = "~/Shared/images/" + pic };
                    db.Galleries.Add(gallery);
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
                return View(db.Galleries.ToList());
            }
        }

        [HttpPost]
        public ActionResult DeleteImages(IEnumerable<int> ImagesIDs)
        {
            using (BasDbContext db = new BasDbContext())
            {
                foreach (var id in ImagesIDs)
                {
                    var image = db.Galleries.Single(s => s.ID == id);
                    string imgPath = Server.MapPath(image.ImagePath);
                    db.Galleries.Remove(image);
                    if (System.IO.File.Exists(imgPath))
                        System.IO.File.Delete(imgPath);
                }
                db.SaveChanges();
            }
            return RedirectToAction("DeleteImages");
        }
    }
}
