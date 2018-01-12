using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ConcremoteDeviceManagment.Models
{
    public class menu
    {
        public int device_type_id { get; set; }
        public string name { get; set; }
    }
    public class DeviceMenu : DbContext
    {
        public DbSet<menu>DeviceType { get; set; }
    }
}