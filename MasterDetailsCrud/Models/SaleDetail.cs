using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MasterDetailsCrud.Models
{
    public class SaleDetail
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SaleDetailId { get; set; }
        [ForeignKey("SaleMaster")]
        public int? SaleId { get; set; }
        public virtual SaleMaster SaleMaster { get; set; }
        public string ProductName { get; set; }
        public int? UnitPrice { get; set; }
        public int? Quantity { get; set; }
        public int? TotalPrice { get; set; }
    }
}