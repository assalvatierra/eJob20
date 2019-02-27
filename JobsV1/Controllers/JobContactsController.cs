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
    public class JobContactsController : Controller
    {
        private JobDBContainer db = new JobDBContainer();

        // GET: JobContacts
        public ActionResult Index()
        {
            return View(db.JobContacts.ToList());
        }

        // GET: JobContacts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobContact jobContact = db.JobContacts.Find(id);
            if (jobContact == null)
            {
                return HttpNotFound();
            }
            return View(jobContact);
        }

        // GET: JobContacts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: JobContacts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,ShortName,Company,Position,Tel1,Tel2,Email,AddInfo,Remarks,ContactType")] JobContact jobContact)
        {
            if (ModelState.IsValid)
            {
                db.JobContacts.Add(jobContact);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(jobContact);
        }

        // GET: JobContacts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobContact jobContact = db.JobContacts.Find(id);
            if (jobContact == null)
            {
                return HttpNotFound();
            }
            return View(jobContact);
        }

        // POST: JobContacts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,ShortName,Company,Position,Tel1,Tel2,Email,AddInfo,Remarks,ContactType")] JobContact jobContact)
        {
            if (ModelState.IsValid)
            {
                db.Entry(jobContact).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(jobContact);
        }

        // GET: JobContacts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobContact jobContact = db.JobContacts.Find(id);
            if (jobContact == null)
            {
                return HttpNotFound();
            }
            return View(jobContact);
        }

        // POST: JobContacts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            JobContact jobContact = db.JobContacts.Find(id);
            db.JobContacts.Remove(jobContact);
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
