using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using StoreFront.DATA.EF;

namespace Storefront.UI.MVC.Controllers
{
    public class ProductStatusController : Controller
    {
        private StoreFrontEntities db = new StoreFrontEntities();

        #region AJAX OPS
        #region AJAX DELETE
        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult AjaxDelete(int id)
        {
            
            ProductStatus prodStat = db.ProductStatuses.Find(id);


            
            db.ProductStatuses.Remove(prodStat);

            
            db.SaveChanges();

            
            var message = $"Deleted the following Product Status from the Database: {prodStat.ProductStatusName}";

            //return the jsonResult
            return Json(new
            {
                id = id,
                message = message
            });

        }//end result


        #endregion

        #region AJAX DETAILS
        [HttpGet]
        public PartialViewResult OrderStatusdetails(int id)
        {
            //retrieve the Product Status by ID
            ProductStatus prodStat = db.ProductStatuses.Find(id);

            
            return PartialView(prodStat);


            //for this view:
            //right click and add a partial view
            //scaffold it to details
            //ProductStatus - model
            //CHECK the partialView Checkbox

        }//end
        #endregion

        #region AJAX CREATE

        public JsonResult AjaxCreate(ProductStatus productStatus)
        {
            //even though this is a json result the VIEW is a partial view
            //so that we can render it in th Index (our div that we created)

            //Hard code that each productStatus will be active (no checkbox in the form)
            //productStatus.IsActive = true;


            db.ProductStatuses.Add(productStatus);
            db.SaveChanges();
            return Json(productStatus);
        }

        #endregion
        #region AJAX EDIT (GET)
        public PartialViewResult PublisherEdit(int id)
        {
            //retrieve the publisher and return the view with data populated for updates
            ProductStatus productStatus = db.ProductStatuses.Find(id);


            return PartialView(productStatus);
        }//end result

        #endregion
        #region AJAX EDIT (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AjaxEdit(ProductStatus productStatus)
        {
            db.Entry(productStatus).State = EntityState.Modified;
            db.SaveChanges();
            return Json(productStatus);
        }

        #endregion
        #endregion

        //Any usused (replaced by ajax) actions below should be commented out or deleted entirely
        //as they still will route to the old views. This can only be achieved by URL Hacking.

        // GET: ProductStatus
        public ActionResult Index()
        {
            return View(db.ProductStatuses.ToList());
        }

        // GET: ProductStatus/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductStatus productStatus = db.ProductStatuses.Find(id);
            if (productStatus == null)
            {
                return HttpNotFound();
            }
            return View(productStatus);
        }

        // GET: ProductStatus/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ProductStatus/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductStatusID,ProductStatusName")] ProductStatus productStatus)
        {
            if (ModelState.IsValid)
            {
                db.ProductStatuses.Add(productStatus);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(productStatus);
        }

        // GET: ProductStatus/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductStatus productStatus = db.ProductStatuses.Find(id);
            if (productStatus == null)
            {
                return HttpNotFound();
            }
            return View(productStatus);
        }

        // POST: ProductStatus/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductStatusID,ProductStatusName")] ProductStatus productStatus)
        {
            if (ModelState.IsValid)
            {
                db.Entry(productStatus).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(productStatus);
        }

        // GET: ProductStatus/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductStatus productStatus = db.ProductStatuses.Find(id);
            if (productStatus == null)
            {
                return HttpNotFound();
            }
            return View(productStatus);
        }

        // POST: ProductStatus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ProductStatus productStatus = db.ProductStatuses.Find(id);
            db.ProductStatuses.Remove(productStatus);
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
    }
}
