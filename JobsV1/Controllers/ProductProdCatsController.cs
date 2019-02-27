using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using JobsV1.Models;

namespace JobsV1.Controllers
{
    public class ProductProdCatsController : Controller
    {
        private JobDBContainer db = new JobDBContainer();

        // GET: ProductProdCats
        public ActionResult Index(int? ProductId)
        {
            if (ProductId != null)
                TempData["PRODUCTID"] = (int)ProductId;
            else
                ProductId = (int)TempData.Peek("PRODUCTID");


            var productProdCats = db.ProductProdCats.Include(p => p.ProductCategory).Include(p => p.Product).Where(d => d.ProductId == (int)ProductId).OrderBy(s =>  s.ProductCategoryId );
            return View(productProdCats.ToList());
        }

        // GET: ProductProdCats/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductProdCat productProdCat = db.ProductProdCats.Find(id);
            if (productProdCat == null)
            {
                return HttpNotFound();
            }
            return View(productProdCat);
        }

        // GET: ProductProdCats/Create
        public ActionResult Create()
        {
            int ProductId = (int)TempData.Peek("PRODUCTID");

            Models.ProductProdCat newCat = new ProductProdCat();
            newCat.ProductId = ProductId;

            ViewBag.ProductCategoryId = new SelectList(db.ProductCategories, "Id", "Name");
            //ViewBag.ProductId = new SelectList(db.Products, "Id", "Code", ProductId);

            return View(newCat);
        }

        // POST: ProductProdCats/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ProductCategoryId,ProductId")] ProductProdCat productProdCat)
        {
            if (ModelState.IsValid)
            {
                db.ProductProdCats.Add(productProdCat);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ProductCategoryId = new SelectList(db.ProductCategories, "Id", "Name", productProdCat.ProductCategoryId);
            ViewBag.ProductId = new SelectList(db.Products, "Id", "Code", productProdCat.ProductId);
            return View(productProdCat);
        }

        // GET: ProductProdCats/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductProdCat productProdCat = db.ProductProdCats.Find(id);
            if (productProdCat == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProductCategoryId = new SelectList(db.ProductCategories, "Id", "Name", productProdCat.ProductCategoryId);
            ViewBag.ProductId = new SelectList(db.Products, "Id", "Code", productProdCat.ProductId);
            return View(productProdCat);
        }

        // POST: ProductProdCats/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ProductCategoryId,ProductId")] ProductProdCat productProdCat)
        {
            if (ModelState.IsValid)
            {
                db.Entry(productProdCat).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProductCategoryId = new SelectList(db.ProductCategories, "Id", "Name", productProdCat.ProductCategoryId);
            ViewBag.ProductId = new SelectList(db.Products, "Id", "Code", productProdCat.ProductId);
            return View(productProdCat);
        }

        // GET: ProductProdCats/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductProdCat productProdCat = db.ProductProdCats.Find(id);
            if (productProdCat == null)
            {
                return HttpNotFound();
            }
            return View(productProdCat);
        }

        // POST: ProductProdCats/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ProductProdCat productProdCat = db.ProductProdCats.Find(id);
            db.ProductProdCats.Remove(productProdCat);
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
