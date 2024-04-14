using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Organic_Zone.ViewModels
{
    public class ShoppingCartAddViewModel
    {
        public double CartTotal { get; set; }
        public int CartCount { get; set; }
        public int ItemCount { get; set; }
        public string AddedName { get; set; }
        public int RecordID { get; set; }

        // <li> element code will be added in to cart
        public string Addedli { get; set; }

        public string Addedfooter { get; set; }
    }
}