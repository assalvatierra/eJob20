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
    public class CustCategoriesController : Controller
    {
        private JobDBContainer db = new JobDBContainer();

        // GET: CustCategories
        public ActionResult Index()
        {
            return View(db.CustCategories.ToList());
        }

        // GET: CustCategories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustCategory custCategory = db.CustCategories.Find(id);
            if (custCategory == null)
            {
                return HttpNotFound();
            }
            return View(custCategory);
        }

        // GET: CustCategories/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CustCategories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,iconPath")] CustCategory custCategory)
        {
            if (ModelState.IsValid)
            {
                db.CustCategories.Add(custCategory);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(custCategory);
        }

        // GET: CustCategories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustCategory custCategory = db.CustCategories.Find(id);
            if (custCategory == null)
            {
                return HttpNotFound();
            }
            return View(custCategory);
        }

        // POST: CustCategories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,iconPath")] CustCategory custCategory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(custCategory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(custCategory);
        }

        // GET: CustCategories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustCategory custCategory = db.CustCategories.Find(id);
            if (custCategory == null)
            {
                return HttpNotFound();
            }
            return View(custCategory);
        }

        // POST: CustCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CustCategory custCategory = db.CustCategories.Find(id);
            db.CustCategories.Remove(custCategory);
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
