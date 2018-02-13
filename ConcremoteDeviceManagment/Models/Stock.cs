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
        //[StringLength(60, MinimumLength = 3)]
        //[Required(ErrorMessage = "Voer een aantal in")]
        public int stock_amount { get; set; }
    //    [Required(ErrorMessage = "Voer een aantal in")]
        public int min_stock { get; set; }
   //     [Required(ErrorMessage = "Voer een aantal in")]
        public int max_stock { get; set; }
        public virtual Pricelist Pricelist { get; set; }

    }
    [Table("pricelist")]
    public class Pricelist
    {
        [Key]
        [Required]
        public int Price_id { get; set; }
         public string CategoryId { get; set; }
         public string SubCategoryId { get; set; }
        public string AdminDescription { get; set; }
        public string AdminDescriptionShort { get; set; }
        public string Standard { get; set; }
        public string Unit { get; set; }
        public decimal? Quantity { get; set; }
        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public Decimal Price { get; set; }
        [StringLength(255, MinimumLength = 1)]
        public string art_lev_nr { get; set; }
        [Required(ErrorMessage = "Voer een BAS artikelnummer in")]
        public string bas_art_nr { get; set; }
        [StringLength(255, MinimumLength = 1)]
        public string description { get; set; }
        public int? PreId { get; set; }
        public decimal? PriceProcurement { get; set; }
        public string Sequence { get; set; }
        public string VAT_Id { get; set; }
        public string VAT_NL_Id { get; set; }
        public string VAT_BE_Id { get; set; }
        public string VAT_DE_Id { get; set; }
        public string Remark { get; set; }
        public bool? Print { get; set; }
        public bool Active { get; set; }
        public bool? Duration { get; set; }
        public bool? Procurement { get; set; }
        public string ProcurementType { get; set; }
        public bool? Sale { get; set; }
        public string PhaseCode { get; set; }
        public int? Supplier_Organization_Id { get; set; }
        public int? ActivityTypeId { get; set; }
        public bool? InStock { get; set; }
        public decimal? ProcurementRebate { get; set; }
        public decimal? Weight { get; set; }
        public string OldSystem_Prijslijst_ItemId { get; set; }
     //   public virtual Stock Stock { get; set; }
    }
    [Table("DeviceConfig")]
    public class DeviceConfig
    {
        [Key]    
        public int Device_config_id { get; set; }  
        public int device_type_id { get; set; }
        [Required(ErrorMessage = "Voer een Prijs ID in")]
        public int Price_id { get; set; }
        [Required(ErrorMessage = "Voer een hoeveelheid in")]
        public decimal amount { get; set; }
        public int assembly_order { get; set; }
        public string device_type { get; set; }
        public bool Active { get; set; }
        public string VersieNummer { get; set; }
        public DateTime Datum { get; set; }
        public virtual Pricelist Pricelist { get; set; }
        public virtual DeviceType DeviceType { get; set; }

    }
   [Table("DeviceType")]
    public class DeviceType
    {
        [Key]
        public int device_type_id { get; set; }
        public string device_type { get; set; }
        [Display(Name = "DeviceType")]
        public string name { get; set; }
    //    public IEnumerable<SelectListItem> name { get; set; }
      }
    [Table("ConcremoteDevice")]
    public class ConcremoteDevice
    {
        [Key]
        public int id { get; set; }

        public bool active { get; set; }
        public int oldsystem_concremote { get; set; }
        public bool Allowvalidation { get; set;}
        public int Device_type_id { get; set; }
        public virtual DeviceType DeviceType { get; set; }
   //     public virtual Device_extra Device_Extra { get; set; }


    }
    [Table("Devicestatus")]
    public class Devicestatus
    {
        [Key]
        public int Device_status_id { get; set; }
        public string employee_1 { get; set; }
        public string employee_2 { get; set; }
        public DateTime Sign_Date { get; set; }
        public int ConcremoteDevice_id { get; set; }
        public int Device_statustypes_id { get; set; }
        public virtual Device_statusypes Device_statustypes { get; set; }
        public virtual ConcremoteDevice ConcremoteDevice { get; set; }
    }
    [Table("Device_statustypes")]
    public class Device_statusypes
    {
        [Key]
        public int id { get; set; }
        public string description { get; set; }
    }
    //[Table("Device_Pricelist")]
    //public class Device_Pricelist
    //{
    //    [Key]
    //    public int id { get; set; }
    //   public int Device_Config_id { get; set; }
    //    public int Price_id { get; set; }
    //    public virtual DeviceConfig DeviceConfig { get; set; }
    //    public virtual Pricelist Pricelist { get; set; }
    //}
    [Table("Device_extra_info")]
    public class Device_extra
    {
        [Key]
        public int id { get; set; }
        public int ConcremoteDevice_id { get; set; }
        public int Price_id { get; set; }
       // public string SIMnr { get; set; }
       // public string Bluetooth_id { get; set; }
       // public string imei { get; set; }
        public DateTime Datum { get; set; }
        public bool Active { get; set; }
        public string Eigenschap_id { get; set; }
        public virtual Pricelist Pricelist { get; set; }
        public virtual ConcremoteDevice ConcremoteDevice { get; set; }
    }
    
    public class BasDbContext : DbContext
        {
            [ForeignKey("Price_id")]
            public DbSet<Stock> Stock { get; set; }
          // [ForeignKey("id")]
            public DbSet<Pricelist> pricelist { get; set; }
     //       [ForeignKey("device_type_id")]
            public DbSet<DeviceConfig> DeviceConfig { get; set; }
            public DbSet<DeviceType> DeviceType { get; set; }  
            public DbSet<ConcremoteDevice> ConcremoteDevice { get; set; }     
            public DbSet<Device_extra> Device_Extra { get; set; }
        }
    }


