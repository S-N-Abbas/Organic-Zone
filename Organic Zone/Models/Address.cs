using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Organic_Zone.Models
{
    public class Address
    {
        [Key]
        public int ID { get; set; }

        [ScaffoldColumn(false)]
        public string Username { get; set; }

        [Required]
        [Phone]
        public string Phone { get; set; }

        [Required]
        [Display(Name = "Address")]
        public string ShippingAddress { get; set; }
    }
}