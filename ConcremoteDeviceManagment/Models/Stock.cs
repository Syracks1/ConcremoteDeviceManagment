using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ConcremoteDeviceManagment.Models
{
    [Table("Stock")]
    public class Stock
    {
        [Key]
        public int id { get; set; }
        public int Price_id { get; set; }
        [StringLength(60, MinimumLength = 3)]
        public string bas_art_nr { get; set; }
        public int stock_amount { get; set; }
        public int min_stock { get; set; }
        public int max_stock { get; set; }
        [StringLength(255, MinimumLength = 1)]
        public string description { get; set; }
    }
        [Table("pricelist")]
        public class Pricelist
        {
            [Key]
            public int Price_id { get; set; }
            public string id_cat { get; set; }
            public string id_subcat { get; set; }
            public float price { get; set; }
            [StringLength(255, MinimumLength = 1)]
            public string art_lev_nr { get; set; }
        }

        [Table("DeviceConfig")]
        public class DeviceConfig
        {
            [Key]
            public int device_type_id { get; set; }
            public int Price_id { get; set; }
            public decimal amount { get; set; }
        }
    public class DeviceType
    {
        [Key]
        public int device_type_id { get; set; }
        public string name { get; set; }
    }
    [Table("ConcremoteDevice")]
    public class ConcremoteDevice
    {
        [Key]
        public int id { get; set; }
        public int device_id { get; set; }
        public string imei { get; set; }
        public bool active { get; set; }
        public int oldsystem_concremote { get; set; }
        public bool Allowvalidation { get; set;}
        public int device_type_id { get; set; }
    }
    public class ConcremoteDeviceManagment : DbContext
        {
            public DbSet<Stock> Stock { get; set; }
            public DbSet<Pricelist> pricelist { get; set; }
            public DbSet<DeviceConfig> DeviceConfig { get; set; }
            public DbSet<DeviceType> DeviceType { get; set; }  
            public DbSet<ConcremoteDevice> ConcremoteDevice { get; set; }     
        }
    }


