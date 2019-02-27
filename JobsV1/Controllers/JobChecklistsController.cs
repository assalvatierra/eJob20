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
    public class JobChecklistsController : Controller
    {
        private JobDBContainer db = new JobDBContainer();

        // GET: JobChecklists
        public ActionResult Index()
        {
            return View(db.JobChecklists.ToList());
        }

        // GET: JobChecklists/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobChecklist jobChecklist = db.JobChecklists.Find(id);
            if (jobChecklist == null)
            {
                return HttpNotFound();
            }
            return View(jobChecklist);
        }

        // GET: JobChecklists/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: JobChecklists/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,dtEntered,dtDue,dtNotification,Description,Remarks,RefId,Status")] JobChecklist jobChecklist)
        {
            if (ModelState.IsValid)
            {
                db.JobChecklists.Add(jobChecklist);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(jobChecklist);
        }

        // GET: JobChecklists/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobChecklist jobChecklist = db.JobChecklists.Find(id);
            if (jobChecklist == null)
            {
                return HttpNotFound();
            }
            return View(jobChecklist);
        }

        // POST: JobChecklists/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,dtEntered,dtDue,dtNotification,Description,Remarks,RefId,Status")] JobChecklist jobChecklist)
        {
            if (ModelState.IsValid)
            {
                db.Entry(jobChecklist).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(jobChecklist);
        }

        // GET: JobChecklists/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobChecklist jobChecklist = db.JobChecklists.Find(id);
            if (jobChecklist == null)
            {
                return HttpNotFound();
            }
            return View(jobChecklist);
        }

        // POST: JobChecklists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            JobChecklist jobChecklist = db.JobChecklists.Find(id);
            db.JobChecklists.Remove(jobChecklist);
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
