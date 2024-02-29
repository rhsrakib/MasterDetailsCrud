using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MasterDetailsCrud.Models
{
    public class SalesDB:DbContext
    {
        public DbSet<SaleMaster> SaleMasters { get; set; }
        public DbSet<SaleDetail> SaleDetails { get; set; }
    }
}