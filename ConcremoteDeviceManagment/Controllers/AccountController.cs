//using System.Linq;
//using System.Web;
//using System.Web.Mvc;
//using Microsoft.Owin.Security.Cookies;
//using Microsoft.Owin.Security.OpenIdConnect;
//using Microsoft.Owin.Security;
//using Microsoft.AspNet.Identity.Owin;
//using ConcremoteDeviceManagment.Models;
//using System.Threading.Tasks;
//using System.Data.SqlClient;
//using System.Configuration;
//using System;
//using System.Data.Entity;
//using System.Collections.Generic;
//using System.Data.Entity.Core.Objects;
//using System.Data.Entity.Infrastructure;
//using System.Net;
//using System.Data;
//using System.Diagnostics;
//using System.Globalization;


//namespace ConcremoteDeviceManagment.Controllers
//{

//    [Authorize]
//    public class AccountController : Controller
//    {
//        public PO3DbContext db = new PO3DbContext();

//        private ApplicationSignInManager _signInManager;
//        private ApplicationUserManager _userManager;

//        public AccountController()
//        {

//        }


//        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
//        {
//            UserManager = userManager;
//            SignInManager = signInManager;
//        }

//        public ApplicationSignInManager SignInManager
//        {
//            get
//            {
//                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
//            }
//            private set
//            {
//                _signInManager = value;
//            }
//        }

//        public ApplicationUserManager UserManager
//        {
//            get
//            {
//                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
//            }
//            private set
//            {
//                _userManager = value;
//            }
//        }

//        //
//        // GET: /Account/Login
//        [AllowAnonymous]
//        public ActionResult SignIn(string returnUrl)
//        {

//            ViewBag.ReturnUrl = returnUrl;
//            return View();
//        }

//        //
//        // POST: /Account/Login

//        [HttpPost]
//        [AllowAnonymous]
//        [ValidateAntiForgeryToken]
//        public async Task<ActionResult> SignIn(Account model, string returnUrl)
//        {

//            if (!ModelState.IsValid)
//            {
//                return View(model);
//            }


//            var result = await SignInManager.PasswordSignInAsync(model.Contact.Email, model.Password, isPersistent: false, shouldLockout: false);
//            switch (result)
//            {
//                case SignInStatus.Success:
//                    return RedirectToAction(returnUrl);
//                case SignInStatus.LockedOut:
//                    return View("Lockout");
//                //case SignInStatus.RequiresVerification:
//                //    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl });
//                case SignInStatus.Failure:
//                default:
//                    ModelState.AddModelError("", "Invalid login attempt.");
//                    return View(model);
//            }
//        }
//        [AllowAnonymous]
//        public ActionResult ForgotPassword()
//        {
//            return View();
//        }

//        //
//        // POST: /Account/ForgotPassword
//        [HttpPost]
//        [AllowAnonymous]
//        [ValidateAntiForgeryToken]
//        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
//        {
//            if (ModelState.IsValid)
//            {
//                var user = await UserManager.FindByNameAsync(model.Email);
//                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
//                {
//                    // Don't reveal that the user does not exist or is not confirmed
//                    return View("Lockout");
//                }

//                // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
//                // Send an email with this link
//                // string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
//                // var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);		
//                // await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
//                // return RedirectToAction("ForgotPasswordConfirmation", "Account");
//            }

//            // If we got this far, something failed, redisplay form
//            return View(model);
//        }
//        public void SignOut()
//        {
//            string callbackUrl = Url.Action("SignOutCallback", "Account", routeValues: null, protocol: Request.Url.Scheme);

//            HttpContext.GetOwinContext().Authentication.SignOut(
//                new AuthenticationProperties { RedirectUri = callbackUrl },
//                OpenIdConnectAuthenticationDefaults.AuthenticationType, CookieAuthenticationDefaults.AuthenticationType);
//        }

//        public ActionResult SignOutCallback()
//        {
//            if (Request.IsAuthenticated)
//            {
//                // Redirect to home page if the user is authenticated.
//                return RedirectToAction("Index", "Home");
//            }

//            return View();
//        }
//        //
//        // GET: /Account/Register
//        //[AllowAnonymous]
//        //public ActionResult Register()
//        //{
//        //    return Redirect("https://login.basrt.eu/General/Login/Login");
//        //}
//    }
//}
using ConcremoteDeviceManagment.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ConcremoteDeviceManagment.Controllers
{ 
public class AccountController : Controller
{
        private PO3DbContext db = new PO3DbContext();
        // GET: /LoginDemo/ 
        public ActionResult SignIn()
        {
            var TestQuery = new SelectList(db.Account.Where( r => r.Contact.Email == "roland.huijskes@rocdeleijgraaf.nl").Select(r => r.Password + r.Contact_id).ToList());
            ViewBag.SelectedDevice = TestQuery;

            return View();
        }
        //[HttpPost]
        //public ActionResult SignIn([Bind(Exclude = "Contact_id")]Account model)
        //{
        //    var type = Request.RequestType;
        //    if (ModelState.Values.ToArray()[0].Errors.Count == 0 && ModelState.Values.ToArray()[1].Errors.Count == 0)
        //    {
        //        if (CheckUser(model.Contact.Id, model.Password))
        //        {
        //            return RedirectToAction("Home", new { username = model.Contact.Email });
        //        }
        //        else
        //        {

        //            ModelState.Clear();
        //            ModelState.AddModelError("", "User Id or Password not exist; try again !!!");
        //            return View(model);
        //        }

        //    }
        //    else
        //    {
        //        return View(model);
        //    }

        //}

        //private bool CheckUser(int Contact_Id, string Password)
        //{
        //var User = db.Account.FirstOrDefault(m => m.Contact.Id == Contact_Id);
        //    if (User != null)
        //    {
        //        if (User.Password == Password)
        //        {
        //            return true;
        //        }
        //        else
        //            return false;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}
    [HttpPost]
    public ActionResult Register(Account model)
    {
        List<string> errorMessage = new List<string>();
        if (ModelState.IsValid)
        {
            db.Account.Add(model);
            db.SaveChanges();
            return Json("", JsonRequestBehavior.AllowGet);
        }
        else
        {
            for (int i = 0; i < ModelState.Values.Count; i++)
            {
                if (ModelState.Values.ToArray()[i].Errors.Count > 0)
                {
                    errorMessage.Add(ModelState.Values.ToArray()[i].Errors[0].ErrorMessage);
                }
                else
                {
                    errorMessage.Add("dammit");
                }
            }

            return Json(new { keys = ModelState.Keys, ErrorMessage = errorMessage }, JsonRequestBehavior.AllowGet);
        }
    }

    public ActionResult Home(string username)
    {
        ViewData.Model = username;
        return View();
    }
    public ActionResult Logout()
    {
        return RedirectToAction("Index");
    }
}
}