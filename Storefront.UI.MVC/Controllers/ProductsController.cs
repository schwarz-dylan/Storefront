using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Storefront.UI.MVC.Utilities;
using StoreFront.DATA.EF;
using PagedList;
using PagedList.Mvc;
using Storefront.UI.MVC.Models;


namespace Storefront.UI.MVC.Controllers
{
    public class ProductsController : Controller
    {
        private StoreFrontEntities db = new StoreFrontEntities();

        [HttpPost]
        public ActionResult AddToCart(int qty, int productID)
        {
            //Create an empty version of the LOCAL shopping cart (dictionary collection)
            Dictionary<int, CartItemViewModel> shoppingCart = null;

            //check the cart in session (GLOBAL)
            //if the cart has stuff in it then assign its value to the local dictionary
            if (Session["cart"] != null)
            {
                shoppingCart = (Dictionary<int, CartItemViewModel>)Session["cart"];

            }//end if


            //if the GLOBAL version is empty
            else
            {
                //create an empty instance of the LOCAL (dictionary)
                shoppingCart = new Dictionary<int, CartItemViewModel>();
            }//end else


            //get the product object being added - FirstOrDefault() allows for a null value
            //.Single() would fail if the book was not found - Runtime Exception
            Product product = db.Products.Where(b => b.ProductID == productID).Include("ModelCategory").FirstOrDefault();

            //if the productID (bookID) is null, return them to the books index
            if (product == null)
            {
                return RedirectToAction("Index");
            }//end if

            //product is valid
            else
            {

                //Create the shoppingCartViewModel Object
                CartItemViewModel item = new CartItemViewModel(qty, product);

                //if the productID is represented in the shopping cart, increase of qty
                if (shoppingCart.ContainsKey(product.ProductID))
                {
                    shoppingCart[product.ProductID].Qty += qty;

                }//end if

                //if the produc tisnt in the cart...add it.
                else
                {
                    shoppingCart.Add(product.ProductID, item);
                }//end else

                //update the GLOBAL (session) cart with the values from our LOCAL (dictionary)
                Session["cart"] = shoppingCart;

            }//end else

            //as long as the product was added - then we will redirect to the ShoppingCart Index
            return RedirectToAction("Index", "ShoppingCart");

        }//end result



        private string imgName;

        // GET: Products
        public ActionResult Index()
        {
            var products = db.Products.Include(p => p.ModelCategory).Include(p => p.ProductStatus);
            return View(products.ToList());
        }

        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            ViewBag.ModelID = new SelectList(db.ModelCategories, "ModelID", "ModelName");
            ViewBag.ProductStatusID = new SelectList(db.ProductStatuses, "ProductStatusID", "ProductStatusName");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductID,ModelID,Price,UnitsSold,ProductStatusID,Description,ImageUrl")] Product product, HttpPostedFileBase ballPicture)
        {
            if (ModelState.IsValid)
            {
                //Only Processing the file upload if all other validation has been met (Modelstate IS Valid)
                #region File Upload Utility
                /*
                 *File Upload completion includes: 
                 * 1- Prepare the view
                 *      -Code the BeginForm() -Create.cshtml(books) line:9
                 *      -Add the file Input (replace the EditorFor()/ValidationMessageFor()) -Create.cshtml(books) line: 86
                 * 2- Pepare the Create Post Action
                 *      -Recieve an HTTPPostedFileBase - Its name (variable) MUST match the name property of the input (casing doesnt matter)
                 *      -Process the FileUpload using the ImageService Utility (add the Using for the new Namespace creates (Utilities folder))
                 *      
                 * 3- Move to Edit Post and code. 
                 * 
                 * 
                 */

                //process the file upload using the utility
                //Use Default image if one is not provided (in this case we have noImage.png)
                string imgName = "NoImage.png";


                //check the HPFB to ensure its not null
                if (ballPicture != null)
                {
                    //if not null
                    //retrieve the image from the HPFB and assign to the img variable
                    imgName = ballPicture.FileName;

                    //declare and assign the extension
                    string ext = imgName.Substring(imgName.LastIndexOf('.'));//.ext

                    //for images, we want a good list of proper extensions to be accepted
                    //for pdfs, no collection is needed.
                    string[] goodExts = { ".jpg", ".jpeg", ".gif", ".png" };

                    //check the ext varaiable against the valid list AND make sur ethe file size is NOT to large.
                    //(Max default ASP.NET size is ~ 4mb -expressed in bytes 4194304)


                    if (goodExts.Contains(ext.ToLower()) && (ballPicture.ContentLength <= 4194304))
                    //code for the pdf check:
                    // if (ext.ToLower()) == ".pdf" && (bookCover.ContentLength <= 4194304))
                    {
                        //if both are good we will rename the file using a guid and then we will concatenate the extension
                        //GUID - Global Unique Identifier
                        //-other ways to create unique ids (make sure your DB field accommodates the size)
                        //substring the book title (first 10/20 characters, concatonate the current userID, datetimestamp)
                        //GUID works weel but its not the only option
                        imgName = Guid.NewGuid() + ext.ToLower();
                        //could use an alternate methodology to rename
                        //ex: imgName = book.BookTitle.Substring(0, 10) + User.Identity.Name + DateTime.Now;

                        //Resize the image
                        //provide all requirments to call the ResizeImage() from the utitlity. SavePath, Image, MaxThumbSize
                        string savePath = Server.MapPath("~/Content/img/Product_Pics/");
                        Image convertedImage = Image.FromStream(ballPicture.InputStream);
                        int maxImageSize = 500;
                        int maxThumbSize = 100;

                        //call the imageService.ResizeImage
                        ImageService.ResizeImage(savePath, imgName, convertedImage, maxImageSize, maxThumbSize);


                    }//end if


                    //either file size or extension are bad
                    else
                    {
                        //default to the noImage value
                        imgName = "NoImage.png";
                    }//end else

                }//end if

                //no matter what - add the imageName property of the book object to send to the DB.
                product.ImageUrl = imgName;

                #endregion




                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }//end if

            ViewBag.ModelID = new SelectList(db.ModelCategories, "ModelID", "ModelName", product.ModelID);
            ViewBag.ProductStatusID = new SelectList(db.ProductStatuses, "ProductStatusID", "ProductStatusName", product.ProductStatusID);
            return View(product);
        }

        // GET: Products/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.ModelID = new SelectList(db.ModelCategories, "ModelID", "ModelName", product.ModelID);
            ViewBag.ProductStatusID = new SelectList(db.ProductStatuses, "ProductStatusID", "ProductStatusName", product.ProductStatusID);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "ProductID,ModelID,Price,UnitsSold,ProductStatusID,Description,ImageUrl")] Product product, HttpPostedFileBase ballPicture)
        {
            if (ModelState.IsValid)
            {
                //Only Processing the file upload if all other validation has been met (Modelstate IS Valid)
                #region File Upload Utility
                /*
                 *File Upload completion includes: 
                 * 1- Prepare the view
                 *      -Code the BeginForm() -Create.cshtml(books) line:9
                 *      -Add the file Input (replace the EditorFor()/ValidationMessageFor()) -Create.cshtml(books) line: 86
                 * 2- Pepare the Create Post Action
                 *      -Recieve an HTTPPostedFileBase - Its name (variable) MUST match the name property of the input (casing doesnt matter)
                 *      -Process the FileUpload using the ImageService Utility (add the Using for the new Namespace creates (Utilities folder))
                 *      
                 * 3- Move to Edit Post and code. 
                 * 
                 * 
                 */

                //No default Image for Edit. All DB records should have an associated default file. 
                //ALL of thos efiles should be represented in the Content/YOURFOLDERNAME folder where you are storing your files.

                //if there is NO file in the input, maintain the existing image (Front End using the Html.HiddenFor())
                //if the inoput isnt Null process the image with the updates
                if (ballPicture != null)
                {
                    //if not null
                    //retrieve the image from the HPFB and assign to the img variable
                    imgName = ballPicture.FileName;

                    //declare and assign the extension
                    string ext = imgName.Substring(imgName.LastIndexOf('.'));//.ext

                    //for images, we want a good list of proper extensions to be accepted
                    //for pdfs, no collection is needed.
                    string[] goodExts = { ".jpg", ".jpeg", ".gif", ".png" };

                    //check the ext varaiable against the list and file size
                    if (goodExts.Contains(ext.ToLower()) && (ballPicture.ContentLength <= 4194304))
                    
                    {
                        //if good - rename the file
                        imgName = Guid.NewGuid() + ext.ToLower();
                        
                        //Resize img code
                        string savePath = Server.MapPath("~/Content/img/Product_Pics/");
                        Image convertedImage = Image.FromStream(ballPicture.InputStream);
                        int maxImageSize = 500;
                        int maxThumbSize = 100;

                        //call the imageService.ResizeImage
                        ImageService.ResizeImage(savePath, imgName, convertedImage, maxImageSize, maxThumbSize);

                        //delete the previous image(s) from the web server as long as its NOT the default image (and not null)
                        if (product.ImageUrl != null && product.ImageUrl != "NoImage.png")
                        {
                            string path = Server.MapPath("~/Content/img/Product_Pics/");
                            ImageService.Delete(path, product.ImageUrl);

                        }//end if

                        //Only if all conditions have been met, save the Image to the Object Property
                        product.ImageUrl = imgName;

                    }//end if


                   

                }//end if

                

                #endregion



                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ModelID = new SelectList(db.ModelCategories, "ModelID", "ModelName", product.ModelID);
            ViewBag.ProductStatusID = new SelectList(db.ProductStatuses, "ProductStatusID", "ProductStatusName", product.ProductStatusID);
            return View(product);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        //Create our own conncection (databaseContext) to the our Data Structure
        private StoreFrontEntities ctx = new StoreFrontEntities();

       

        public ActionResult ClientSide()
        {
            var products = ctx.Products.Include(p => p.ModelCategory)
                                       .Include(p => p.ProductStatus);


            return View(products.ToList());
        }//end action result

        public ActionResult ProductsQS(string searchFilter)
        {
            //2 Options
            //-Search has NOT been used (initial pg demand or subsequent demands)
            //-Search HAS been used and return filtered results

            //get a list of products
            var products = ctx.Products;


            //branch - No filter
            if (string.IsNullOrEmpty(searchFilter))
            {
                //return all results
                return View(products.ToList());

            }//end if



            else
            {


                //keyword syntax
                var filteredProducts = (from p in products
                                        where p.ModelCategory.ModelName.Contains(searchFilter.ToLower())
                                        select p).ToList();







                return View(filteredProducts);
            }//end else




        }//end result


        public ActionResult ProductsMVCPaging(string searchString, int page = 1)
        {
            int pageSize = 5;

            var products = ctx.Products.OrderBy(p => p.ModelCategory.ModelName).ToList();

            #region Search Logic

            if (!string.IsNullOrEmpty(searchString))
            {
                products = products.Where(p => p.ModelCategory.ModelName.ToLower().Contains(searchString.ToLower())).ToList();
            }//end if

            ViewBag.SearchString = searchString;

            #endregion

            return View(products.ToPagedList(page, pageSize));


        }//end action result



    }
}
