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
    public class PreDefinedNotesController : Controller
    {
        private JobDBContainer db = new JobDBContainer();

        // GET: PreDefinedNotes
        public ActionResult Index()
        {
            return View(db.PreDefinedNotes.ToList());
        }

        // GET: PreDefinedNotes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PreDefinedNote preDefinedNote = db.PreDefinedNotes.Find(id);
            if (preDefinedNote == null)
            {
                return HttpNotFound();
            }
            return View(preDefinedNote);
        }

        // GET: PreDefinedNotes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PreDefinedNotes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Note")] PreDefinedNote preDefinedNote)
        {
            if (ModelState.IsValid)
            {
                db.PreDefinedNotes.Add(preDefinedNote);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(preDefinedNote);
        }

        // GET: PreDefinedNotes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PreDefinedNote preDefinedNote = db.PreDefinedNotes.Find(id);
            if (preDefinedNote == null)
            {
                return HttpNotFound();
            }
            return View(preDefinedNote);
        }

        // POST: PreDefinedNotes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Note")] PreDefinedNote preDefinedNote)
        {
            if (ModelState.IsValid)
            {
                db.Entry(preDefinedNote).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(preDefinedNote);
        }

        // GET: PreDefinedNotes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PreDefinedNote preDefinedNote = db.PreDefinedNotes.Find(id);
            if (preDefinedNote == null)
            {
                return HttpNotFound();
            }
            return View(preDefinedNote);
        }

        // POST: PreDefinedNotes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PreDefinedNote preDefinedNote = db.PreDefinedNotes.Find(id);
            db.PreDefinedNotes.Remove(preDefinedNote);
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
