using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Organic_Zone.Models
{
    public class Cart
    {
        [Key]
        public int RecordID { get; set; }
        public string CartID { get; set; }
        public string ItemName { get; set; }
        public int Count { get; set; }
        public DateTime DateCreated { get; set; }

        public virtual Item Item { get; set; }
    }
}