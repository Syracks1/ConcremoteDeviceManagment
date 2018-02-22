using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ConcremoteDeviceManagment.Models
{
    public class NavbarItem
    {
        public int Id { get; set; }
        public string nameOption { get; set; }
        public string controller { get; set; }
        public string action { get; set; }
        public bool havingImageClass { get; set; }
        public string cssClass { get; set; }
        public int parentId { get; set; }
        public bool isParent { get; set; }
    }
    public class Navbar
    {
        public IEnumerable<NavbarItem>  NavbarTop()
        {
            var topNav = new List<NavbarItem>();
           
            //Onderdelen menu, subklasses : Voorraad en prijslijst
            topNav.Add(new NavbarItem() { Id = 1, action = "", nameOption = "Onderdelen", controller = "", isParent = true, parentId = -1 });
            topNav.Add(new NavbarItem() { Id = 2, action = "Index", nameOption = "Voorraad", controller = "Stock", isParent = false, parentId = 1 });
            topNav.Add(new NavbarItem() { Id = 3, action = "Index", nameOption = "Prijslijst", controller = "Article", isParent = false, parentId = 1 });

            topNav.Add(new NavbarItem() { Id = 4, action = "Index", nameOption = "Create Device", controller = "DeviceConfig", isParent = false, parentId = -1 });
            topNav.Add(new NavbarItem() { Id = 5, action = "Index", nameOption = "Change Device Configuration", controller = "Home", isParent = false, parentId = -1 });
            topNav.Add(new NavbarItem() { Id = 6, action= "Index", nameOption = "Devices", controller="Concremote" ,isParent = false, parentId = -1 });

            topNav.Add(new NavbarItem() { Id = 7, action = "", nameOption = "Beheer", controller = "", isParent = true, parentId = -1 });
            topNav.Add(new NavbarItem() { Id = 8, action = "Index", nameOption = "Device Type", controller = "DeviceTypes", isParent = false, parentId = 7 });
            topNav.Add(new NavbarItem() { Id = 9, action = "Index", nameOption = "Status Types", controller = "Device_statustypes", isParent = false, parentId = 7 });


            return topNav;
        }
    }
}