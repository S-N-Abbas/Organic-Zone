using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Organic_Zone.Models
{
    public partial class ShoppingCart
    {
        OrganicZoneDBContext OZDB = new OrganicZoneDBContext();

        public string ShoppingCartID { get; set; }
        public const string CartSessionKey = "CartID";

        public static ShoppingCart GetCart (HttpContextBase context)
        {
            var cart = new ShoppingCart();
            cart.ShoppingCartID = cart.GetCartId(context);
            return cart;
        }

        // Helper method to simplify shopping cart calls
        public static ShoppingCart GetCart(Controller controller)
        {
            return GetCart(controller.HttpContext);
        }

        public int AddToCart(Item item)
        {
            // Get the matching cart and album instances
            var cartItem = OZDB.Carts.SingleOrDefault(
            c => c.CartID == ShoppingCartID
            && c.ItemName == item.ItemName);

            if (cartItem == null)
            {
                // Create a new cart item if no cart item exists
                cartItem = new Cart
                {
                    ItemName = item.ItemName,
                    CartID = ShoppingCartID,
                    Count = 1,
                    DateCreated = DateTime.Now
                };
                OZDB.Carts.Add(cartItem);
            }
            else
            {
                // If the item does exist in the cart, then add one to the quantity
                cartItem.Count++;
            }
            
            // Save changes
            OZDB.SaveChanges();
            return cartItem.Count;
        }
        
        public int RemoveFromCart(int id)
        {
            // Get the cart
            var cartItem = OZDB.Carts.Single(
            cart => cart.CartID == ShoppingCartID
            && cart.RecordID == id);
            int itemCount = 0;
            if (cartItem != null)
            {
                if (cartItem.Count > 1)
                {
                    cartItem.Count--;
                    itemCount = cartItem.Count;
                }
                else
                {
                    OZDB.Carts.Remove(cartItem);
                }
        
                // Save changes
                OZDB.SaveChanges();
            }
            return itemCount;
        }
        
        public void EmptyCart()
        {
            var cartItems = OZDB.Carts.Where(cart => cart.CartID == ShoppingCartID);
            foreach (var cartItem in cartItems)
            {
                OZDB.Carts.Remove(cartItem);
            }
            
            // Save changes
            OZDB.SaveChanges();
        }
        
        public List<Cart> GetCartItems()
        {
            return OZDB.Carts.Where(cart => cart.CartID == ShoppingCartID).ToList();
        }
        
        public int GetCount()
        {
            // Get the count of each item in the cart and sum them up
            int? count = (from cartItems in OZDB.Carts
                          where cartItems.CartID == ShoppingCartID
                          select (int?) cartItems.Count).Sum();
            // Return 0 if all entries are null
            return count ?? 0;
        }
        
        public double GetTotal()
        {
            double? total = (from cartItems in OZDB.Carts
                              where cartItems.CartID == ShoppingCartID
                              select (double?)cartItems.Count * ( cartItems.Item.Price - cartItems.Item.Discount)).Sum();
            
            return total ?? 0;
        }
        
        public int CreateOrder(Order order)
        {
            double orderTotal = 0;
            var cartItems = GetCartItems();
            // Iterate over the items in the cart, adding the order details for each
            foreach (var CartItem in cartItems)
            {
                Item item = OZDB.Items.Find(CartItem.ItemName);
                var orderDetail = new OrderDetail
                {
                    ItemName = CartItem.ItemName,
                    OrderID = order.OrderID,
                    Item = item,
                    UnitPrice = OZDB.Items.Find(CartItem.ItemName).Price - OZDB.Items.Find(CartItem.ItemName).Discount,
                    Quantity = CartItem.Count
                };
                // Set the order total of the shopping cart
                orderTotal += (CartItem.Count * (OZDB.Items.Find(CartItem.ItemName).Price - OZDB.Items.Find(CartItem.ItemName).Discount));
                OZDB.OrderDetails.Add(orderDetail);
            }
            // Set the order's total to the orderTotal count
            order.Total = orderTotal;

            order.Total += order.Total * (order.Tax / 100);

            order.Total += order.DeliveryCharges;

            OZDB.Orders.Add(order);

            // Save the order
            OZDB.SaveChanges();
            // Empty the shopping cart
            EmptyCart();
            // Return the OrderId as the confirmation number
            return order.OrderID;
        }
        
        // We're using HttpContextBase to allow access to cookies.
        public string GetCartId(HttpContextBase context)
        {
            if (context.Session[CartSessionKey] == null)
            {
                if (!string.IsNullOrWhiteSpace(context.User.Identity.Name))
                {
                    context.Session[CartSessionKey] = context.User.Identity.Name;
                }
                else
                {
                    // Generate a new random GUID using System.Guid class
                    Guid tempCartId = Guid.NewGuid();
                    // Send tempCartId back to client as a cookie
                    context.Session[CartSessionKey] = tempCartId.ToString();
                }
            }
            return context.Session[CartSessionKey].ToString();
        }
        
        // When a user has logged in, migrate their shopping cart to
        // be associated with their username
        public void MigrateCart(string userName)
        {
            var shoppingCart = OZDB.Carts.Where(c => c.CartID == ShoppingCartID);
            foreach (Cart item in shoppingCart)
            {
                item.CartID = userName;
            }
            OZDB.SaveChanges();
        }
    }
}