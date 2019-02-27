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
    public class SalesActivitiesController : Controller
    {
        private JobDBContainer db = new JobDBContainer();

        // GET: SalesActivities
        public ActionResult Index()
        {
            var salesActivities = db.SalesActivities.Include(s => s.SalesActCode).Include(s => s.SalesLead).Include(s => s.SalesActStatu);
            return View(salesActivities.ToList());
        }

        // GET: SalesActivities/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SalesActivity salesActivity = db.SalesActivities.Find(id);
            if (salesActivity == null)
            {
                return HttpNotFound();
            }
            return View(salesActivity);
        }

        // GET: SalesActivities/Create
        public ActionResult Create()
        {
            ViewBag.SalesActCodeId = new SelectList(db.SalesActCodes, "Id", "Name");
            ViewBag.SalesLeadId = new SelectList(db.SalesLeads, "Id", "Details");
            ViewBag.SalesActStatusId = new SelectList(db.SalesActStatus, "Id", "Name");
            return View();
        }

        // POST: SalesActivities/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,SalesLeadId,SalesActCodeId,Particulars,DtActivity,SalesActStatusId,DtEntered,EnteredBy")] SalesActivity salesActivity)
        {
            if (ModelState.IsValid)
            {
                db.SalesActivities.Add(salesActivity);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.SalesActCodeId = new SelectList(db.SalesActCodes, "Id", "Name", salesActivity.SalesActCodeId);
            ViewBag.SalesLeadId = new SelectList(db.SalesLeads, "Id", "Details", salesActivity.SalesLeadId);
            ViewBag.SalesActStatusId = new SelectList(db.SalesActStatus, "Id", "Name", salesActivity.SalesActStatusId);
            return View(salesActivity);
        }

        // GET: SalesActivities/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SalesActivity salesActivity = db.SalesActivities.Find(id);
            if (salesActivity == null)
            {
                return HttpNotFound();
            }
            ViewBag.SalesActCodeId = new SelectList(db.SalesActCodes, "Id", "Name", salesActivity.SalesActCodeId);
            ViewBag.SalesLeadId = new SelectList(db.SalesLeads, "Id", "Details", salesActivity.SalesLeadId);
            ViewBag.SalesActStatusId = new SelectList(db.SalesActStatus, "Id", "Name", salesActivity.SalesActStatusId);
            return View(salesActivity);
        }

        // POST: SalesActivities/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,SalesLeadId,SalesActCodeId,Particulars,DtActivity,SalesActStatusId,DtEntered,EnteredBy")] SalesActivity salesActivity)
        {
            if (ModelState.IsValid)
            {
                db.Entry(salesActivity).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.SalesActCodeId = new SelectList(db.SalesActCodes, "Id", "Name", salesActivity.SalesActCodeId);
            ViewBag.SalesLeadId = new SelectList(db.SalesLeads, "Id", "Details", salesActivity.SalesLeadId);
            ViewBag.SalesActStatusId = new SelectList(db.SalesActStatus, "Id", "Name", salesActivity.SalesActStatusId);
            return View(salesActivity);
        }

        // GET: SalesActivities/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SalesActivity salesActivity = db.SalesActivities.Find(id);
            if (salesActivity == null)
            {
                return HttpNotFound();
            }
            return View(salesActivity);
        }

        // POST: SalesActivities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SalesActivity salesActivity = db.SalesActivities.Find(id);
            db.SalesActivities.Remove(salesActivity);
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
