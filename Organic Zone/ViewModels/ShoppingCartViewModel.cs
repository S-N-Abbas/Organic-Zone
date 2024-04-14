using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Organic_Zone.Models;

namespace Organic_Zone.ViewModels
{
    public class ShoppingCartViewModel
    {
        public List<Cart> CartItems { get; set; }
        public double CartTotal { get; set; }
    }
}