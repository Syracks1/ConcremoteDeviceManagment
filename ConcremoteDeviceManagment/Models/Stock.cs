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
            [Required(ErrorMessage = "Voer een aantal in")]
            public int stock_amount { get; set; }
            [Required(ErrorMessage = "Voer een aantal in, Het getal mag niet hoger zijn dan maximale hoeveelheid")]
            //[Required(ErrorMessage = "Het getal mag niet hoger zijn dan maximale hoeveelheid")]
             public int min_stock { get; set; }
            [Required(ErrorMessage = "Voer een aantal in")]
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
        [Table("DeviceType")]
        public class DeviceType
        {
        [Key]
        public int device_type_id { get; set; }
        [Required(ErrorMessage = "Voer een Naam in")]
        [StringLength(255, MinimumLength = 1)]
        public string name { get; set; }
        }
        [Table("DeviceConfig")]
        public class DeviceConfig
        {
        [Key]
        public int Device_config_id { get; set; }
            public int device_type_id { get; set; }
            public bool Active { get; set; }
            public int? VersionNr { get; set; }
//        [DisplayFormat("dd-mm-yyy")]
            public DateTime? Date { get; set; }
            public virtual DeviceType DeviceType { get; set; }
        }
        [Table("ConcremoteDevice")]
        public class ConcremoteDevice
        {
            [Key]
            public int id { get; set; }
            public bool Active { get; set; }
        }
        [Table("Devicestatus")]
        public class DeviceStatus
        {
        [Key]
        public int id { get; set; }
            public int ConcremoteDevice_id { get; set; }
            public int Device_statustypes_id { get; set; }
            public string Employee_1 { get; set; }
            public string Employee_2 { get; set; }
            public DateTime Sign_Date { get; set; }
            public virtual DeviceConfig DeviceConfig { get; set; }
            public virtual ConcremoteDevice ConcremoteDevice { get; set; }
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
        [Table("Device_Pricelist")]
        public class Device_Pricelist
        {
        [Key]
        public int id { get; set; }
            public int Device_config_id { get; set; }
            public int Price_id { get; set; }
            public decimal amount { get; set; }
            public int? assembly_order { get; set; }
            public virtual DeviceConfig DeviceConfig { get; set; }
            public virtual Pricelist Pricelist { get; set; }
        }
        [Table("DeviceConfig_ExtraInfo")]
        public class DeviceConfig_ExtraInfo
        {
            public int id { get; set; }
            public int DeviceConfig_id { get; set; }
            public string Label { get; set; }
            public bool Label_req { get; set; }
            public virtual DeviceConfig DeviceConfig { get; set; }
        }
        [Table("DeviceStatus_ExtraInfo")]
        public class DeviceStatus_ExtraInfo
        {
            public int id { get; set; }
            public int DeviceStatus_id { get; set; }
            public int DeviceConfig_ExtraInfo_id { get; set; }
            public string name { get; set; }
            public virtual DeviceConfig_ExtraInfo DeviceConfig_ExtraInfo { get; set; }
            public virtual DeviceStatus DeviceStatus { get; set; }
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
    [Table("Account")]
    public class Account
    {
        [Key]
        public int Contact_id { get; set; }
        [Display(Name = "Password")]
        [Required, DataType(DataType.Password)]
        public string Password { get; set; }
        public DateTimeOffset LastLogin { get; set; }
        public string PasswordForgotToken { get; set;}
        public DateTimeOffset? PasswordForgotTokenCreatedOn { get; set; }
        public int AccountType { get; set; }
        public bool Active { get; set; }
        public virtual Contact Contact { get; set; }
    }
    [Table("Contact")]
    public class Contact
    {
        public int Id { get; set; }
        public bool Active { get; set; }
        //[EmailAddress]
        //[Required(ErrorMessage = "Field can't be empty")]
        //[RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail is not valid")]
        public string Email { get; set; }
        public int Language_Id { get; set; }
        public string Initials { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Prefix { get; set; }
        public string Titles { get; set; }
        public string Suffix { get; set; }
        public string Gender { get; set; }
        public bool Welder { get; set; }
        public bool Inspector { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Function { get; set; }
        public string Website { get; set; }
        public string Remarks { get; set; }
        public string Cellphone { get; set; }
        public string PhonePrivate { get; set; }
        public string EmailPrivate { get; set; }
        public string Address { get; set; }
        public string Zipcode { get; set; }
        public string City { get; set; }
        public int County_Id { get; set; }
        public string Salutation { get; set; }
        public string SalutationFormally { get; set; }
        public string SalutationInFormally { get; set; }
        public int CreatedBy_Contact_Id { get; set; }
        public string OldSystem_Contactpersoon_PersonID { get; set; }
        public string OldSystem_Contactpersoon_Email { get; set; }
        public string OldSystem_Personen_PersonID { get; set; }
        public string OldSystem_Personen_Actueel { get; set; }
        public string OldSystem_Personen_roleID { get; set; }
        public string OldSystem_Useraccounts_useraccountID { get; set; }
        public string OldSystem_Useraccounts_PersonID_sh { get; set; }
        public string OldSystem_Useraccounts_PersonID { get; set; }
        public string OldSystem_Useraccounts_roleID { get; set; }
        public string OldSystem_Useraccounts_password_changed { get; set; }
        public string OldSystem_Useraccounts_Online_username { get; set; }
        public string OldSystem_Useraccounts_online_autorisatie { get; set; }
        public string OldSystem_Useraccounts_lasserlijst_notify { get; set; }
        public string OldSystem_Useraccounts_richtproces_notify { get; set; }
        public string OldSystem_Useraccounts_guestUser_notify { get; set; }
        public string OldSystem_Useraccounts_dashboard_profile { get; set; }
        public string OldSystem_Useraccounts_Is_Temp_Client { get; set; }
        public string OldSystem_Useraccounts_Admindashboard_profile { get; set; }
        public string OldSystem_Useraccounts_Rightpanel_profile { get; set; }
        public string Oldsystem_Useraccounts_IsLoggedIn { get; set; }
        public string OldSystem_Useraccounts_CompanyFilter { get; set; }
        public string OldSystem_Useraccounts_dokagebruiker { get; set; }
        public string CultureCode { get; set; }
        public int CompressiveStrengthUnit { get; set;}
        public int TemperatureUnit { get; set; }
        public string Abbreviation { get; set; }
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
        }
    public class PO3DbContext : DbContext
    {
        public DbSet<Pricelist2> Pricelist { get; set; }
        public DbSet<Account> Account { get; set; }
        public DbSet<Contact> Contact { get; set; }
    }
}


