using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using JobsV1.Areas.Products.Models;

namespace JobsV1.Areas.Products.Controllers
{
    public class SmCategoriesController : Controller
    {
        private ProdDBContainer db = new ProdDBContainer();

        // GET: Products/SmCategories
        public ActionResult Index()
        {
            return View(db.SmCategories.ToList());
        }

        // GET: Products/SmCategories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SmCategory smCategory = db.SmCategories.Find(id);
            if (smCategory == null)
            {
                return HttpNotFound();
            }
            return View(smCategory);
        }

        // GET: Products/SmCategories/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Products/SmCategories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] SmCategory smCategory)
        {
            if (ModelState.IsValid)
            {
                db.SmCategories.Add(smCategory);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(smCategory);
        }

        // GET: Products/SmCategories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SmCategory smCategory = db.SmCategories.Find(id);
            if (smCategory == null)
            {
                return HttpNotFound();
            }
            return View(smCategory);
        }

        // POST: Products/SmCategories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] SmCategory smCategory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(smCategory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(smCategory);
        }

        // GET: Products/SmCategories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SmCategory smCategory = db.SmCategories.Find(id);
            if (smCategory == null)
            {
                return HttpNotFound();
            }
            return View(smCategory);
        }

        // POST: Products/SmCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SmCategory smCategory = db.SmCategories.Find(id);
            db.SmCategories.Remove(smCategory);
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
