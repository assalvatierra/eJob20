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
    public class JobPickupsController : Controller
    {
        private JobDBContainer db = new JobDBContainer();

        // GET: JobPickups
        public ActionResult Index()
        {
            var jobPickups = db.JobPickups.Include(j => j.JobMain);
            return View(jobPickups.ToList());
        }

        // GET: JobPickups/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobPickup jobPickup = db.JobPickups.Find(id);
            if (jobPickup == null)
            {
                return HttpNotFound();
            }
            return View(jobPickup);
        }

        // GET: JobPickups/Create
        public ActionResult Create()
        {
            ViewBag.JobMainId = new SelectList(db.JobMains, "Id", "Description");
            return View();
        }

        // POST: JobPickups/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,JobMainId,puDate,puTime,puLocation,ContactName,ContactNumber")] JobPickup jobPickup)
        {
            if (ModelState.IsValid)
            {
                db.JobPickups.Add(jobPickup);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.JobMainId = new SelectList(db.JobMains, "Id", "Description", jobPickup.JobMainId);
            return View(jobPickup);
        }

        // GET: JobPickups/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobPickup jobPickup = db.JobPickups.Find(id);
            if (jobPickup == null)
            {
                return HttpNotFound();
            }
            ViewBag.JobMainId = new SelectList(db.JobMains, "Id", "Description", jobPickup.JobMainId);
            return View(jobPickup);
        }

        // POST: JobPickups/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,JobMainId,puDate,puTime,puLocation,ContactName,ContactNumber")] JobPickup jobPickup)
        {
            if (ModelState.IsValid)
            {
                db.Entry(jobPickup).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.JobMainId = new SelectList(db.JobMains, "Id", "Description", jobPickup.JobMainId);
            return View(jobPickup);
        }

        // GET: JobPickups/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobPickup jobPickup = db.JobPickups.Find(id);
            if (jobPickup == null)
            {
                return HttpNotFound();
            }
            return View(jobPickup);
        }

        // POST: JobPickups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            JobPickup jobPickup = db.JobPickups.Find(id);
            db.JobPickups.Remove(jobPickup);
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
