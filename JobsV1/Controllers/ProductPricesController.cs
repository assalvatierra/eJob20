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
    public class ProductPricesController : Controller
    {
        private JobDBContainer db = new JobDBContainer();
        private List<SelectListItem> uomList = new List<SelectListItem>
        {
            new SelectListItem { Value="PAX", Text="Pax" },
            new SelectListItem { Value="UNIT", Text="Unit" }
        };

        // GET: ProductPrices
        public ActionResult Index(int? ProductId)
        {
            if (ProductId != null)
                TempData["PRODUCTID"] = (int)ProductId;
            else
                ProductId = (int)TempData.Peek("PRODUCTID");


            var productPrices = db.ProductPrices.Include(p => p.Product).Where(d => d.ProductId == (int)ProductId).OrderBy(s => new { s.Uom, s.Qty});
            return View(productPrices.ToList());
        }

        // GET: ProductPrices/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductPrice productPrice = db.ProductPrices.Find(id);
            if (productPrice == null)
            {
                return HttpNotFound();
            }
            return View(productPrice);
        }

        // GET: ProductPrices/Create
        public ActionResult Create()
        {
            int ProductId = (int)TempData.Peek("PRODUCTID");
            ViewBag.ProductId = new SelectList(db.Products, "Id", "Code", ProductId);
            ViewBag.Uom = new SelectList(this.uomList, "Value", "Text");
            return View();
        }

        // POST: ProductPrices/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ProductId,Uom,Qty,Rate,Rate1")] ProductPrice productPrice)
        {
            if (ModelState.IsValid)
            {
                db.ProductPrices.Add(productPrice);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ProductId = new SelectList(db.Products, "Id", "Code", productPrice.ProductId);
            return View(productPrice);
        }

        // GET: ProductPrices/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductPrice productPrice = db.ProductPrices.Find(id);
            if (productPrice == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProductId = new SelectList(db.Products, "Id", "Code", productPrice.ProductId);
            return View(productPrice);
        }

        // POST: ProductPrices/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ProductId,Uom,Qty,Rate,Rate1")] ProductPrice productPrice)
        {
            if (ModelState.IsValid)
            {
                db.Entry(productPrice).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProductId = new SelectList(db.Products, "Id", "Code", productPrice.ProductId);
            return View(productPrice);
        }

        // GET: ProductPrices/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductPrice productPrice = db.ProductPrices.Find(id);
            if (productPrice == null)
            {
                return HttpNotFound();
            }
            return View(productPrice);
        }

        // POST: ProductPrices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ProductPrice productPrice = db.ProductPrices.Find(id);
            db.ProductPrices.Remove(productPrice);
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
