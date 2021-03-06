﻿using ConcremoteDeviceManagment.Models;
using System.Web.Mvc;

namespace ConcremoteDeviceManagment.Controllers
{
    public class NavigationController : Controller
    {
        // GET: Navigation
        public ActionResult TopNav()
        {
            var nav = new Navbar();
            return PartialView("_topNav", nav.NavbarTop());
        }
    }
}