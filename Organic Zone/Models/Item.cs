using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Organic_Zone.Models
{
    public class Item
    {
        [Key]
        [Required]
        [MaxLength(50)]
        public string ItemName { get; set; }

        public string Thumbnail { get; set; }

        public string Description { get; set; }

        [Required]
        public double Price { get; set; }

        public double Discount { get; set; } = 0;
    }
}