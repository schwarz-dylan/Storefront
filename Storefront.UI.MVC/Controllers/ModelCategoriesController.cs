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
    public class ModelCategoriesController : Controller
    {
        private StoreFrontEntities db = new StoreFrontEntities();

       

        // GET: ModelCategories
        public ActionResult Index()
        {
            var modelCategories = db.ModelCategories.Include(m => m.Brand);
            return View(modelCategories.ToList());
        }

        // GET: ModelCategories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ModelCategory modelCategory = db.ModelCategories.Find(id);
            if (modelCategory == null)
            {
                return HttpNotFound();
            }
            return View(modelCategory);
        }

        // GET: ModelCategories/Create
        public ActionResult Create()
        {
            ViewBag.BrandID = new SelectList(db.Brands, "BrandID", "BrandName");
            return View();
        }

        // POST: ModelCategories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ModelID,BrandID,ModelName")] ModelCategory modelCategory)
        {
            if (ModelState.IsValid)
            {
                db.ModelCategories.Add(modelCategory);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.BrandID = new SelectList(db.Brands, "BrandID", "BrandName", modelCategory.BrandID);
            return View(modelCategory);
        }

        // GET: ModelCategories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ModelCategory modelCategory = db.ModelCategories.Find(id);
            if (modelCategory == null)
            {
                return HttpNotFound();
            }
            ViewBag.BrandID = new SelectList(db.Brands, "BrandID", "BrandName", modelCategory.BrandID);
            return View(modelCategory);
        }

        // POST: ModelCategories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ModelID,BrandID,ModelName")] ModelCategory modelCategory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(modelCategory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BrandID = new SelectList(db.Brands, "BrandID", "BrandName", modelCategory.BrandID);
            return View(modelCategory);
        }

        // GET: ModelCategories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ModelCategory modelCategory = db.ModelCategories.Find(id);
            if (modelCategory == null)
            {
                return HttpNotFound();
            }
            return View(modelCategory);
        }

        // POST: ModelCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ModelCategory modelCategory = db.ModelCategories.Find(id);
            db.ModelCategories.Remove(modelCategory);
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
