using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using JobsV1.Areas.Products.Models;
using JobsV1.Models;

namespace JobsV1.Areas.Products.Controllers
{
    public class SmProdAdsController : Controller
    {
        private JobDBContainer db = new JobDBContainer();

        // GET: Products/SmProdAds
        public ActionResult Index()
        {
            var smProdAds = db.SmProdAds.Include(s => s.SmCategory).Include(s => s.SmProduct);
            return View(smProdAds.ToList());
        }

        // GET: Products/SmProdAds/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SmProdAds smProdAds = db.SmProdAds.Find(id);
            if (smProdAds == null)
            {
                return HttpNotFound();
            }
            return View(smProdAds);
        }

        // GET: Products/SmProdAds/Create
        public ActionResult Create()
        {
            ViewBag.SmCategoryId = new SelectList(db.SmCategories, "Id", "Name");
            ViewBag.SmProductId = new SelectList(db.SmProducts, "Id", "Code");
            return View();
        }

        // POST: Products/SmProdAds/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Image,Link,SmCategoryId,SmProductId")] SmProdAds smProdAds)
        {
            if (ModelState.IsValid)
            {
                db.SmProdAds.Add(smProdAds);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.SmCategoryId = new SelectList(db.SmCategories, "Id", "Name", smProdAds.SmCategoryId);
            ViewBag.SmProductId = new SelectList(db.SmProducts, "Id", "Code", smProdAds.SmProductId);
            return View(smProdAds);
        }

        // GET: Products/SmProdAds/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SmProdAds smProdAds = db.SmProdAds.Find(id);
            if (smProdAds == null)
            {
                return HttpNotFound();
            }
            ViewBag.SmCategoryId = new SelectList(db.SmCategories, "Id", "Name", smProdAds.SmCategoryId);
            ViewBag.SmProductId = new SelectList(db.SmProducts, "Id", "Code", smProdAds.SmProductId);
            return View(smProdAds);
        }

        // POST: Products/SmProdAds/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Image,Link,SmCategoryId,SmProductId")] SmProdAds smProdAds)
        {
            if (ModelState.IsValid)
            {
                db.Entry(smProdAds).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.SmCategoryId = new SelectList(db.SmCategories, "Id", "Name", smProdAds.SmCategoryId);
            ViewBag.SmProductId = new SelectList(db.SmProducts, "Id", "Code", smProdAds.SmProductId);
            return View(smProdAds);
        }

        // GET: Products/SmProdAds/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SmProdAds smProdAds = db.SmProdAds.Find(id);
            if (smProdAds == null)
            {
                return HttpNotFound();
            }
            return View(smProdAds);
        }

        // POST: Products/SmProdAds/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SmProdAds smProdAds = db.SmProdAds.Find(id);
            db.SmProdAds.Remove(smProdAds);
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
