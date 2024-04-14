using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Organic_Zone.Models
{
    public class OrderDetail
    {
        [Key]
        public int OrderDetailID { get; set; }
        
        public int OrderID { get; set; }
        
        public string ItemName { get; set; }
        
        public int Quantity { get; set; }
        
        public double UnitPrice { get; set; }

        public virtual Item Item { get; set; }
        public virtual Order Order { get; set; }
    }
}