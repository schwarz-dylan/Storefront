using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StoreFront.DATA.EF;//Access to EF (db context/book model)
using Storefront.UI.MVC.Models;

namespace Storefront.UI.MVC.Controllers
{
    public class ShoppingCartController : Controller
    {
        // GET: ShoppingCart
        public ActionResult Index()
        {
            var shoppingCart = (Dictionary<int, CartItemViewModel>)Session["cart"];

            if (shoppingCart == null || shoppingCart.Count == 0)
            {
                shoppingCart = new Dictionary<int, CartItemViewModel>();
                //"No cart items" message
                ViewBag.Message = "There are no items in your cart";
            }
            //if there is 'stuff' in the cart
            else
            {
                //null the message
                ViewBag.Message = null;
            }

            return View(shoppingCart);
        }//end action

        [HttpPost]
        public ActionResult UpdateCart(int productID, int qty)
        {
            //if the user zeros out the qty using the update button, remove the book from the cart
            if (qty == 0)
            {
                //removeFromCart()
                RemoveFromCart(productID);

                return RedirectToAction("Index");
            }//end if


            //retrieve the cart from session and assign it to the local dictionary
            Dictionary<int, CartItemViewModel> shoppingCart =
                (Dictionary<int, CartItemViewModel>)Session["cart"];

            //update the quantity in the local storage
            shoppingCart[productID].Qty = qty;

            //return the local cart to session
            Session["cart"] = shoppingCart;

            //logic to display no items in cart message if they update the last item in the cart to 0
            if (shoppingCart.Count == 0)
            {
                ViewBag.Message = "There are no items in your cart.";
            }//end if

            //redirect to the index - we need the logic in the index action to run so that all values are processed (return view wont work here)
            return RedirectToAction("Index");
        }//end result

        public ActionResult RemoveFromCart(int id)
        {
            //cart out of session and into local
            Dictionary<int, CartItemViewModel> shoppingCart =
                (Dictionary<int, CartItemViewModel>)Session["cart"];

            //call the remove() - dictionary method
            shoppingCart.Remove(id);

            //redirect back to the index action (runs all code and repopulates the table)
            return RedirectToAction("Index");


        }//end action result




    }//end class
}//end namespace