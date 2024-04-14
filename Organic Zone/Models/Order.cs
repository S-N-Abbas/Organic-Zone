using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace Organic_Zone.Models
{
    public class Order
    {
        [Key]
        [Display(Name = "#")]
        public int OrderID { get; set; }

        public string Username { get; set; }

        [Display(Name = "Delivery Charges")]
        public double DeliveryCharges { get; set; }

        [Display(Name = "Tax (20%)")]
        public double Tax { get; set; }

        [Display(Name = "Total")]
        public double Total { get; set; }

        [Display(Name = "Date")]
        public DateTime OrderDate { get; set; }

        public string Status { get; set; } = "Wating Confirmation";

        public List<OrderDetail> OrderDetails { get; set; }
    }
}