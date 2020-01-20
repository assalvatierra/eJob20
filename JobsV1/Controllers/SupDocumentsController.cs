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
    public class SupDocumentsController : Controller
    {
        private JobDBContainer db = new JobDBContainer();

        // GET: SupDocuments
        public ActionResult Index()
        {
            return View(db.SupDocuments.ToList());
        }

        // GET: SupDocuments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SupDocument supDocument = db.SupDocuments.Find(id);
            if (supDocument == null)
            {
                return HttpNotFound();
            }
            return View(supDocument);
        }

        // GET: SupDocuments/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SupDocuments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Description")] SupDocument supDocument)
        {
            if (ModelState.IsValid)
            {
                db.SupDocuments.Add(supDocument);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(supDocument);
        }

        // GET: SupDocuments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SupDocument supDocument = db.SupDocuments.Find(id);
            if (supDocument == null)
            {
                return HttpNotFound();
            }
            return View(supDocument);
        }

        // POST: SupDocuments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Description")] SupDocument supDocument)
        {
            if (ModelState.IsValid)
            {
                db.Entry(supDocument).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(supDocument);
        }

        // GET: SupDocuments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SupDocument supDocument = db.SupDocuments.Find(id);
            if (supDocument == null)
            {
                return HttpNotFound();
            }
            return View(supDocument);
        }

        // POST: SupDocuments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SupDocument supDocument = db.SupDocuments.Find(id);
            db.SupDocuments.Remove(supDocument);
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
