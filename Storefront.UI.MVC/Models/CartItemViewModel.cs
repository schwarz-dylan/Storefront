using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StoreFront.DATA.EF;

namespace Storefront.UI.MVC.Models
{
    public class CartItemViewModel
    {
        public int Qty { get; set; }
        public Product Product { get; set; }


        public CartItemViewModel(int qty, Product product)
        {
            Qty = qty;
            Product = product;


        }//end


    }//end class
}//end namespace