using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using Microsoft.AspNet.Identity;
using Organic_Zone.Models;

namespace Organic_Zone.Controllers
{
    [Authorize]
    public class CheckoutController : Controller
    {
        OrganicZoneDBContext OZDB = new OrganicZoneDBContext();


        int DeliveryCharges = 10;


        // GET: /Checkout/AddressAndPayment
        public ActionResult AddressAndPayment()
        {
            ViewData["DeliveryCharges"] = DeliveryCharges;
            return View();
        }

        // POST: /Checkout/AddressAndPayment
        [HttpPost]
        public ActionResult AddressAndPayment([Bind(Include = "Phone,ShippingAddress")] Address address)
        {
            string username = User.Identity.GetUserName();

            var add = OZDB.Addresses.SingleOrDefault(a => a.Username == username);

            if(add == null)
            {
                address.Username = username;
                OZDB.Addresses.Add(address);
            }
            else
            {
                add.Phone = address.Phone;
                add.ShippingAddress = add.ShippingAddress;
            }
            OZDB.SaveChanges();

            var order = new Order();

            try
            {
                // Adding 20% tax and delivery charges.
                order.Username = User.Identity.Name;
                order.OrderDate = DateTime.Now;
                order.DeliveryCharges = DeliveryCharges;
                order.Tax = 20.0;
                order.Status = "Waiting Confirmation";

                // Process the order
                var cart = ShoppingCart.GetCart(this.HttpContext);
                cart.CreateOrder(order);

                ViewBag.Message = "";

                return View("Complete", order);
            }
            catch
            {
                ViewBag.Message = "Sorry! Something went wrong. Please try later.";
                return View(order);
            }
        }

        // GET: /Checkout/Complete
        public ActionResult Complete(Order order)
        {
            // validate customer owns this order

            bool isValid = OZDB.Orders.Any(o => o.OrderID == order.OrderID && o.Username == User.Identity.Name);

            if (isValid)
            {
                return View(order);
            }
            else
            {
                return View("Error");
            }
        }

    }
}