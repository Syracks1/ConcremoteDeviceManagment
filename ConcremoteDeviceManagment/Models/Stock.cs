using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace ConcremoteDeviceManagment.Models
{
    [Table("Stock")]
    public class Stock
    {
        [Key]
        public int id { get; set; }

        [Required(ErrorMessage = "Voer een Prijs ID in")]
        public int Price_id { get; set; }

        [Required(ErrorMessage = "Voer een aantal in")]
        public int Stock_amount { get; set; }

        [Required(ErrorMessage = "Voer een aantal in, Het getal mag niet hoger zijn dan maximale hoeveelheid")]
        public int min_stock { get; set; }

        [Required(ErrorMessage = "Voer een aantal in")]
        public int max_stock { get; set; }

        [ForeignKey("Price_id")]
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
        [Required(ErrorMessage = "Fill in Price")]
        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal Price { get; set; }
        [Required(ErrorMessage = "Fill in")]
        [StringLength(255, MinimumLength = 1)]
        public string art_lev_nr { get; set; }

        [Required(ErrorMessage = "Insert BAS CMI")]
        public string bas_art_nr { get; set; }
        [Required(ErrorMessage = "Insert Something")]
        [StringLength(255, MinimumLength = 1)]
        public string Leverancier { get; set; }
        [Required(ErrorMessage = "Insert Description")]
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

    [Table("DeviceType")]
    public class DeviceType
    {
        [Key]
        [Required]
        public int device_type_id { get; set; }

        [Required(ErrorMessage = "Voer een Naam in")]
        [StringLength(255, MinimumLength = 1)]
        public string name { get; set; }
    }

    [Table("DeviceConfig")]
    public class DeviceConfig
    {
        [Key]
        [Required]
        public int Device_config_id { get; set; }

        [Required(ErrorMessage = "Voer een Prijs ID in")]
        public int device_type_id { get; set; }

        public string device_type { get; set; }
        public bool Active { get; set; }
        public int VersionNr { get; set; }

        [DataType(DataType.Date)]
        public DateTime? Date { get; set; }

        [ForeignKey("device_type_id")]
        public virtual DeviceType DeviceType { get; set; }
    }

    [Table("ConcremoteDevice")]
    public class ConcremoteDevice
    {
        [Key]
        public string id { get; set; }

        public bool Active { get; set; }
    }

    [Table("Devicestatus")]
    public class DeviceStatus
    {
        [Key]
        [Column(Order = 0)]
        public int id { get; set; }

        public int Device_statustypes_id { get; set; }
        public string ConcremoteDevice_id { get; set; }
        public int DeviceConfig_id { get; set; }
        public string Employee_1 { get; set; }
        public string Employee_2 { get; set; }

        //[Required(ErrorMessage = "Selecteer een datum")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? Sign_Date { get; set; }

        [ForeignKey("DeviceConfig_id")]
        public virtual DeviceConfig DeviceConfig { get; set; }

        [ForeignKey("ConcremoteDevice_id")]
        public virtual ConcremoteDevice ConcremoteDevice { get; set; }

        [ForeignKey("Device_statustypes_id")]
        public virtual Device_statustypes Device_Statustypes { get; set; }
    }

    [Table("Device_statustypes")]
    public class Device_statustypes
    {
        [Key]
        public int id { get; set; }

        [Required(ErrorMessage = "Voer een Naam in")]
        [StringLength(255, MinimumLength = 1)]
        public string name { get; set; }
    }

    [Table("gallery")]
    public class Gallery
    {
        public int ID { get; set; }

        // public int device_type_id { get; set; }
        public string ImagePath { get; set; }

        public DateTime? DateAdded { get; set; }
        public int? device_type_id { get; set; }

        [ForeignKey("device_type_id")]
        public virtual DeviceType DeviceType { get; set; }
    }

    [Table("Device_Pricelist")]
    public class Device_Pricelist
    {
        [Key]
        // [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        //  [Key]
        //   [Column(Order = 1)]
        public int Device_config_id { get; set; }

        // [Key]
        // [Column(Order = 2)]
        [Required(ErrorMessage = "Voer een onderdeel in")]
        public int Price_id { get; set; }

        [Required(ErrorMessage = "Voer een aantal in")]
        public decimal amount { get; set; }

        [Required(ErrorMessage = "Voer een bouwvolgorde in")]
        public int assembly_order { get; set; }

        [ForeignKey("Device_config_id")]
        public virtual DeviceConfig DeviceConfig { get; set; }

        [ForeignKey("Price_id")]
        public virtual Pricelist Pricelist { get; set; }
    }

    [Table("DeviceConfig_ExtraInfo")]
    public class DeviceConfig_ExtraInfo
    {
        public int id { get; set; }
        public int DeviceConfig_id { get; set; }
        public string Label { get; set; }
        public bool Label_req { get; set; }

        [ForeignKey("DeviceConfig_id")]
        public virtual DeviceConfig DeviceConfig { get; set; }
    }

    [Table("DeviceStatus_ExtraInfo")]
    public class DeviceStatus_ExtraInfo
    {
        public int id { get; set; }

        //     public int DeviceStatus_id { get; set; }
        public int DeviceConfig_ExtraInfo_id { get; set; }

        public string name { get; set; }

        [ForeignKey("DeviceConfig_ExtraInfo_id")]
        public virtual DeviceConfig_ExtraInfo DeviceConfig_ExtraInfo { get; set; }

        //   public virtual ICollection<Enrollment> Enrollments { get; set; }

        //[ForeignKey("DeviceStatus_id")]
        //    public virtual DeviceStatus DeviceStatus { get; set; }
    }

    [Table("Pricelist")]
    public class Pricelist2
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
        public string Leverancier { get; set; }

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

    [Table("AspNetUsers")]
    public class AspNetUsers
    {
        [Key]
        public string Id { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public bool EmailConfirmed { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }

        //       [DataType(DataType.Date)]
        public DateTime? LockoutEndDateUtc { get; set; }

        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }
        //public string Username { get; set; }
    }

    [Table("AspNetUserRoles")]
    public class AspNetUserRoles
    {
        [Key]
        [Column(Order = 0)]
        public string UserId { get; set; }

        [Column(Order = 1)]
        public string RoleId { get; set; }

        [ForeignKey("UserId")]
        public virtual AspNetUsers AspNetUsers { get; set; }

        [ForeignKey("RoleId")]
        public virtual AspNetRoles AspNetRoles { get; set; }
    }

    [Table("AspNetRoles")]
    public class AspNetRoles
    {
        [Key]
        public string Id { get; set; }

        public string Name { get; set; }
    }

    public class BasDbContext : DbContext
    {
        //[ForeignKey("Price_id")]
        public DbSet<Stock> Stock { get; set; }

        public DbSet<Pricelist> pricelist { get; set; }
        public DbSet<DeviceConfig> DeviceConfig { get; set; }
        public DbSet<DeviceType> DeviceType { get; set; }
        public DbSet<ConcremoteDevice> ConcremoteDevice { get; set; }
        public DbSet<DeviceStatus> DeviceStatus { get; set; }
        public DbSet<Device_statustypes> Device_statustypes { get; set; }
        public DbSet<Device_Pricelist> Device_Pricelist { get; set; }
        public DbSet<DeviceConfig_ExtraInfo> DeviceConfig_ExtraInfo { get; set; }
        public DbSet<DeviceStatus_ExtraInfo> DeviceStatus_ExtraInfo { get; set; }
        public DbSet<AspNetUsers> AspNetUsers { get; set; }
        public DbSet<AspNetUserRoles> AspNetUserRoles { get; set; }
        public DbSet<AspNetRoles> AspNetRoles { get; set; }
        public DbSet<Gallery> gallery { get; set; }
    }

    public class PO3DbContext : DbContext
    {
        public DbSet<Pricelist2> Pricelist { get; set; }
    }
}