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
        public IEnumerable<NavbarItem> NavbarTop()
        {
            var topNav = new List<NavbarItem>();
            topNav.Add(new NavbarItem() { Id = 1, action = "Index", nameOption = "Stock", controller = "Stock", isParent = false, parentId = -1 });
           // topNav.Add(new NavbarItem() { Id = 2, action = "Create", nameOption = "Create Device", controller = "DeviceConfig", isParent = true, parentId = -1 });
            //End drop down Menu
            // drop down Menu
            topNav.Add(new NavbarItem() { Id = 3, action = "", nameOption = "Create Device", controller = "", isParent = true, parentId = -1 });
            topNav.Add(new NavbarItem() { Id = 4, action = "CSDOKA", nameOption = "583041000 Cable Sensor Doka", controller = "DeviceConfig", isParent = false, parentId = 3 });
            topNav.Add(new NavbarItem() { Id = 5, action = "Index", nameOption = "583041000 Cable Sensor B|A|S", controller = "DeviceConfig", isParent = false, parentId = 3 });
            topNav.Add(new NavbarItem() { Id = 6, action = "Index", nameOption = "583040000 Floor Sensor Doka", controller = "DeviceConfig", isParent = false, parentId = 3 });
            topNav.Add(new NavbarItem() { Id = 6, action = "Index", nameOption = "583040000 Floor Sensor B|A|S", controller = "DeviceConfig", isParent = false, parentId = 3 });
            topNav.Add(new NavbarItem() { Id = 6, action = "Index", nameOption = "583056000 Floor Sensor (new)", controller = "DeviceConfig", isParent = false, parentId = 3 });
            //  End drop down Menu
            topNav.Add(new NavbarItem() { Id = 7, action = "Index", nameOption = "Change Device Configuration", controller = "Home", isParent = false, parentId = -1 });
            return topNav;
        }
    }
}