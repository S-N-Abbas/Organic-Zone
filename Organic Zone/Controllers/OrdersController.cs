using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Organic_Zone.Models;
using Organic_Zone.ViewModels;

namespace Organic_Zone.Controllers
{
    public class OrdersController : Controller
    {
        OrganicZoneDBContext OZDB = new OrganicZoneDBContext();

        // GET: Orders
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            ViewBag.Message = "";
            var orders = OZDB.Orders.OrderByDescending(o => o.OrderDate).ToList();
            return View(orders);
        }

        [Authorize]
        public ActionResult OrderHistory()
        {
            var myOrders = OZDB.Orders.Where(o => o.Username == User.Identity.Name);
            ViewBag.Title = "My Orders History";
            return View("Index", myOrders);
        }

        // GET: Details
        [Authorize(Roles = "Admin")]
        public ActionResult Details(int id)
        {
            ViewBag.Message = "";

            

            Order order = OZDB.Orders.Single(o => o.OrderID == id);
            var customer = OZDB.Customers.Find(order.Username);
            var address = OZDB.Addresses.Single(a => a.Username == order.Username);
            var orderDetails = OZDB.OrderDetails.Where(o => o.OrderID == id).ToList();

            OrderDetailsViewModel model = new OrderDetailsViewModel();
            model.Order = order;
            model.Customer = customer;
            model.Address = address;
            model.OrderDetails = orderDetails;

            return View(model);
        }

        // GET: DeleteItem
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteItem(int id)
        {
            try
            {
                var OrderDetail = OZDB.OrderDetails.Single(o => o.OrderDetailID == id);
                int qty = OrderDetail.Quantity;
                double amount = OrderDetail.UnitPrice;
                double total = qty * amount;

                OrderDetail.Order.Total -= total;
                OZDB.OrderDetails.Remove(OrderDetail);
                OZDB.SaveChanges();

                ViewBag.Message = "Item Deleted From Order List";
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Item Not Deleted: " + ex.Message;
            }
            return RedirectToAction("Details", new { id = id });
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Confirm(int id)
        {

            var order = OZDB.Orders.Find(id);
            var orders = OZDB.Orders.OrderByDescending(o => o.OrderDate).ToList();
            if (order == null)
            {
                ViewBag.Message = "No such order exists";
                return View("index", orders);
            }
            try
            {
                ViewBag.Message = "Order " + id.ToString() + " has been confirmed";
                order.Status = "Confirmed";
                OZDB.SaveChanges();
                return RedirectToAction("Details", new { id = id });
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Sorry! Order " + id.ToString() + " not confirmed. " + ex.Message;
                return RedirectToAction("Details", new { id = id });
            }
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Cancel(int id)
        {

            var order = OZDB.Orders.Find(id);
            var orders = OZDB.Orders.OrderByDescending(o => o.OrderDate).ToList();
            if (order == null)
            {
                ViewBag.Message = "No such order exists";
                return View("index", orders);
            }
            try
            {
                ViewBag.Message = "Order " + id.ToString() + " has been cancelled";
                order.Status = "Cancelled";
                OZDB.SaveChanges();
                return RedirectToAction("Details", new { id = id });
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Sorry! Order " + id.ToString() + " not cancelled. " + ex.Message;
                return RedirectToAction("Details", new { id = id });
            }
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Fulfill(int id)
        {

            var order = OZDB.Orders.Find(id);
            var orders = OZDB.Orders.OrderByDescending(o => o.OrderDate).ToList();
            if (order == null)
            {
                ViewBag.Message = "No such order exists";
                return View("index", orders);
            }
            try
            {
                ViewBag.Message = "Order " + id.ToString() + " has been marked as Fulfilled";
                order.Status = "Fulfilled";
                OZDB.SaveChanges();
                return RedirectToAction("Details", new { id = id });
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Sorry! Order " + id.ToString() + " not Fulfilled. " + ex.Message;
                return RedirectToAction("Details", new { id = id });
            }
        }
    }
}