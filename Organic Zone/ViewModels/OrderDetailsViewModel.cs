using Microsoft.Owin.BuilderProperties;
using Organic_Zone.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Organic_Zone.ViewModels
{
    public class OrderDetailsViewModel
    {
        public Order Order { get; set; }
        public Customer Customer { get; set; }
        public Organic_Zone.Models.Address Address { get; set; }
        public List<OrderDetail> OrderDetails { get; set; }
    }
}