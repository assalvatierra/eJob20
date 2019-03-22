using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using JobsV1.Areas.Accounting.Models;

namespace JobsV1.Areas.Accounting.Controllers
{
    public class AccntCategoriesController : Controller
    {
        private AccountingDBContainer db = new AccountingDBContainer();

        // GET: Accounting/AccntCategories
        public ActionResult Index()
        {
            var accntCategories = db.AccntCategories.Include(a => a.AccntType);
            return View(accntCategories.ToList());                       
        }

        // GET: Accounting/AccntCategories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AccntCategory accntCategory = db.AccntCategories.Find(id);
            if (accntCategory == null)
            {
                return HttpNotFound();
            }
            return View(accntCategory);
        }

        // GET: Accounting/AccntCategories/Create
        public ActionResult Create()
        {
            ViewBag.AccntTypeId = new SelectList(db.AccntTypes, "Id", "Code");
            return View();
        }

        // POST: Accounting/AccntCategories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Code,Description,OrderNo,AccntTypeId")] AccntCategory accntCategory)
        {
            if (ModelState.IsValid)
            {
                db.AccntCategories.Add(accntCategory);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AccntTypeId = new SelectList(db.AccntTypes, "Id", "Code", accntCategory.AccntTypeId);
            return View(accntCategory);
        }

        // GET: Accounting/AccntCategories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AccntCategory accntCategory = db.AccntCategories.Find(id);
            if (accntCategory == null)
            {
                return HttpNotFound();
            }
            ViewBag.AccntTypeId = new SelectList(db.AccntTypes, "Id", "Code", accntCategory.AccntTypeId);
            return View(accntCategory);
        }

        // POST: Accounting/AccntCategories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Code,Description,OrderNo,AccntTypeId")] AccntCategory accntCategory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(accntCategory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AccntTypeId = new SelectList(db.AccntTypes, "Id", "Code", accntCategory.AccntTypeId);
            return View(accntCategory);
        }

        // GET: Accounting/AccntCategories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AccntCategory accntCategory = db.AccntCategories.Find(id);
            if (accntCategory == null)
            {
                return HttpNotFound();
            }
            return View(accntCategory);
        }

        // POST: Accounting/AccntCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AccntCategory accntCategory = db.AccntCategories.Find(id);
            db.AccntCategories.Remove(accntCategory);
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
