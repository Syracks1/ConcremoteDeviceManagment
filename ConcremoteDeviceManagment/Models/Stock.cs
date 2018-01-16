using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ConcremoteDeviceManagment.Models
{
    [Table("Stock")]
    public class Stock
    {
        [Key]
        public int id { get; set; }      
        [Required(ErrorMessage = "Voer een Prijs ID in")]
        public int Price_id { get; set; }
        [StringLength(60, MinimumLength = 3)]
        [Required(ErrorMessage = "Voer een BAS artikelnummer in")]
        public string bas_art_nr { get; set; }
        [Required(ErrorMessage ="Voer een aantal in")]
        public int stock_amount { get; set; }
        [Required(ErrorMessage = "Voer een aantal in")]
        public int min_stock { get; set; }
        [Required(ErrorMessage = "Voer een aantal in")]
        public int max_stock { get; set; }
        [StringLength(255, MinimumLength = 1)]
        public string description { get; set; }

    }
    [Table("pricelist")]
    public class Pricelist
    {
        [Key]
        [Required]
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
        [Required(ErrorMessage = "Voer een Prijs ID in")]
        public int Price_id { get; set; }
        [Required(ErrorMessage = "Voer een hoeveelheid in")]
        public decimal amount { get; set; }
    }
    public class DeviceType
    {
        [Key]
        public int device_type_id { get; set; }
        public string name { get; set; }
        public string SelectedName { get; set; }
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
     //       [ForeignKey("id")]
            public DbSet<Pricelist> pricelist { get; set; }
    //        [ForeignKey("device_type_id")]
            public DbSet<DeviceConfig> DeviceConfig { get; set; }
            public DbSet<DeviceType> DeviceType { get; set; }  
            public DbSet<ConcremoteDevice> ConcremoteDevice { get; set; }     
        }
    }


