using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Razor.Parser.SyntaxTree;
using System.Web.UI.WebControls;
using Organic_Zone.Models;
using Organic_Zone.ViewModels;

namespace Organic_Zone.Controllers
{
    public class ShoppingCartController : Controller
    {
        OrganicZoneDBContext OZDB = new OrganicZoneDBContext();
        //
        // GET: /ShoppingCart/
        public ActionResult Index()
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);
            // Set up our ViewModel
            var viewModel = new ShoppingCartViewModel
            {
                CartItems = cart.GetCartItems(),
                CartTotal = cart.GetTotal()
            };
            // Return the view
            return PartialView(viewModel);
        }

        //
        // GET: /Store/AddToCart/Item
        [HttpPost]
        public ActionResult AddToCart(string id)
        {
            // Retrieve the item from the database
            Item addedItem = OZDB.Items
                .Single(item => item.ItemName == id);

            // Add it to the shopping cart
            var cart = ShoppingCart.GetCart(this);
            int itemCount = cart.AddToCart(addedItem);

            int RecordID =  OZDB.Carts.Where(
                c => c.CartID == cart.ShoppingCartID
            && c.ItemName == id).ToList().Last().RecordID;

            var results = new ShoppingCartAddViewModel
            {
                CartTotal = cart.GetTotal(),
                CartCount = cart.GetCount(),
                ItemCount = itemCount,
                AddedName = id,
                RecordID = RecordID,
                Addedli = "",
                Addedfooter = ""
                
            };

            if (results.ItemCount == 1)
            {
                results.Addedli = """
                            <li id="row-@item.RecordID" class="minicart-item">
                            <div class="minicart-details-name">
                                <a class="minicart-name" href="#">@item.Item.ItemName</a>
                            """;
                if(addedItem.Discount > 0)
                {
                    results.Addedli += Environment.NewLine +
                        """
                        <ul class="minicart-attributes">
                            <li>Discount: £@item.Item.Discount</li>
                        </ul>
                        """;


                    results.Addedli = results.Addedli.Replace("@item.Item.Discount", addedItem.Discount.ToString());
                }
                results.Addedli += """
                    </div>
                        <div class="minicart-details-quantity">
                            <span class="minicart-quantity" id="item-count-@item.RecordID">@item.Count</span>
                        </div>
                        <div class="minicart-details-remove">
                            <button type="button" class="minicart-remove RemoveLink" data-id="@item.RecordID">×</button>
                        </div>
                        <div class="minicart-details-price">
                            <span class="minicart-price">£@price</span>
                        </div>
                    </li>
                    """;
            }
            results.Addedli = results.Addedli.Replace("@item.RecordID", results.RecordID.ToString());
            results.Addedli = results.Addedli.Replace("@item.Item.ItemName", addedItem.ItemName);
            results.Addedli = results.Addedli.Replace("@item.Count", results.ItemCount.ToString());
            results.Addedli = results.Addedli.Replace("@price", (addedItem.Price - addedItem.Discount).ToString());

            results.Addedfooter = """
                <div class="minicart-subtotal">
                    Subtotal: £<span id="cart-total">@Model.CartTotal</span> <a href="Checkout/AddressAndPayment">Checkout</a>
                </div>
                """;

            results.Addedfooter = results.Addedfooter.Replace("@Model.CartTotal", results.CartTotal.ToString());

            // Go back to the main store page for more shopping
            return Json(results);
        }
        
        //
        // AJAX: /ShoppingCart/RemoveFromCart/5
        [HttpPost]
        public ActionResult RemoveFromCart(int id)
        {
            // Remove the item from the cart
            var cart = ShoppingCart.GetCart(this.HttpContext);
            // Get the name of the album to display confirmation
            string itemName = OZDB.Carts.Single(item => item.RecordID == id).Item.ItemName;

            // Remove from cart
            int itemCount = cart.RemoveFromCart(id);
            // Display the confirmation message
            var results = new ShoppingCartRemoveViewModel
            {
                Message = Server.HtmlEncode(itemName) +
            " has been removed from your shopping cart.",
                CartTotal = cart.GetTotal(),
                CartCount = cart.GetCount(),
                ItemCount = itemCount,
                DeleteID = id
            };
            return Json(results);
        }
        
        //
        // GET: /ShoppingCart/CartSummary
        public ActionResult CartSummary()
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);
            ViewData["CartCount"] = cart.GetCount();
            return PartialView("CartSummary");
        }
    }
}