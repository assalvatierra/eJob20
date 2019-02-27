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
    public class InvCarRecordTypesController : Controller
    {
        private JobDBContainer db = new JobDBContainer();

        // GET: InvCarRecordTypes
        public ActionResult Index()
        {
            return View(db.InvCarRecordTypes.ToList());
        }

        // GET: InvCarRecordTypes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InvCarRecordType invCarRecordType = db.InvCarRecordTypes.Find(id);
            if (invCarRecordType == null)
            {
                return HttpNotFound();
            }
            return View(invCarRecordType);
        }

        // GET: InvCarRecordTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: InvCarRecordTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Description,SysCode,OdoInterval,DaysInterval")] InvCarRecordType invCarRecordType)
        {
            if (ModelState.IsValid)
            {
                db.InvCarRecordTypes.Add(invCarRecordType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(invCarRecordType);
        }

        // GET: InvCarRecordTypes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InvCarRecordType invCarRecordType = db.InvCarRecordTypes.Find(id);
            if (invCarRecordType == null)
            {
                return HttpNotFound();
            }
            return View(invCarRecordType);
        }

        // POST: InvCarRecordTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Description,SysCode,OdoInterval,DaysInterval")] InvCarRecordType invCarRecordType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(invCarRecordType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(invCarRecordType);
        }

        // GET: InvCarRecordTypes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InvCarRecordType invCarRecordType = db.InvCarRecordTypes.Find(id);
            if (invCarRecordType == null)
            {
                return HttpNotFound();
            }
            return View(invCarRecordType);
        }

        // POST: InvCarRecordTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            InvCarRecordType invCarRecordType = db.InvCarRecordTypes.Find(id);
            db.InvCarRecordTypes.Remove(invCarRecordType);
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
