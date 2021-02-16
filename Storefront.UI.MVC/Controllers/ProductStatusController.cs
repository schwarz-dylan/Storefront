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
