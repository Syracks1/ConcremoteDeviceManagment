//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;
//using System.Data.Entity;
//using System.Linq;
//using System.Web;

//namespace ConcremoteDeviceManagment.Models
//{
//    [Table("pricelist")]
//    public class Pricelist
//    {
//        [Key]
//        public int Price_id { get; set; }
//        public string id_cat { get; set; }
//        public string id_subcat { get; set; }
//        public decimal price { get; set; }
//        [StringLength(255, MinimumLength = 1)]
//        public string art_lev_nr { get; set; }
//    }
//    public class PriceManager : DbContext
//    {
//       public DbSet<Pricelist> Pricelist { get; set; }
//    }
//}