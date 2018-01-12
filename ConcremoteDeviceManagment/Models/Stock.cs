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


    public class ConcremoteDeviceManagment : DbContext
    {
        //public DbSet<Stock> Stock { get; set; }
        public DbSet<Stock> Stock { get; set; }
    }
}