using System.Collections.Generic;
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
            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                //Articles menu, subclasses : Stock, Pricelist

                topNav.Add(new NavbarItem() { Id = 1, action = "", nameOption = "Articles", controller = "", isParent = true, parentId = -1 });
                topNav.Add(new NavbarItem() { Id = 2, action = "Index", nameOption = "Stock", controller = "Stock", isParent = false, parentId = 1 });
                topNav.Add(new NavbarItem() { Id = 3, action = "Index", nameOption = "Pricelist", controller = "Article", isParent = false, parentId = 1 });

                if (HttpContext.Current.User.IsInRole("Admin") || HttpContext.Current.User.IsInRole("Assembly"))
                {
                    //Create Device option
                    topNav.Add(new NavbarItem() { Id = 4, action = "Index", nameOption = "Create Device", controller = "DeviceConfig", isParent = false, parentId = -1 });

                    //Change Device Configuration option
                    topNav.Add(new NavbarItem() { Id = 5, action = "Index", nameOption = "Change Device Configuration", controller = "Home", isParent = false, parentId = -1 });

                    //Device option

                    topNav.Add(new NavbarItem() { Id = 6, action = "Index", nameOption = "Devices", controller = "Concremote", isParent = false, parentId = -1 });

                    //Manage menu, subclasses : Device Type, Status Types, Slider

                    topNav.Add(new NavbarItem() { Id = 7, action = "", nameOption = "Manage Stuff", controller = "", isParent = true, parentId = -1 });
                    topNav.Add(new NavbarItem() { Id = 8, action = "Index", nameOption = "Device Type", controller = "DeviceTypes", isParent = true, parentId = 7 });
                    topNav.Add(new NavbarItem() { Id = 9, action = "Index", nameOption = "Status Types", controller = "Device_statustypes", isParent = false, parentId = 7 });
                    //Link to ImageSlide menu
                    topNav.Add(new NavbarItem() { Id = 10, action = "Index", nameOption = "Slider Images", controller = "Slider", isParent = false, parentId = 7 });
                }
            }
            return topNav;
        }
    }
}