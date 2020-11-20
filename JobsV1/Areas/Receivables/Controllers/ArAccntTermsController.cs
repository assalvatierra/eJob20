using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ArModels.Models;

namespace JobsV1.Areas.Receivables.Controllers
{
    public class ArAccntTermsController : Controller
    {
        private ArDBContainer db = new ArDBContainer();

        // GET: Receivables/ArAccntTerms
        public ActionResult Index()
        {
            var arAccntTerms = db.ArAccntTerms.Include(a => a.ArAccount).Include(a => a.ArAccntTermStatu);
            return View(arAccntTerms.ToList());
        }

        // GET: Receivables/ArAccntTerms/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ArAccntTerm arAccntTerm = db.ArAccntTerms.Find(id);
            if (arAccntTerm == null)
            {
                return HttpNotFound();
            }
            return View(arAccntTerm);
        }

        // GET: Receivables/ArAccntTerms/Create
        public ActionResult Create()
        {
            ViewBag.ArAccountId = new SelectList(db.ArAccounts, "Id", "Name");
            ViewBag.ArAccntTermStatusId = new SelectList(db.ArAccntTermStatus, "Id", "Status");
            return View();
        }

        // POST: Receivables/ArAccntTerms/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,dtTerm,NoOfDays,Remarks,ArAccountId,ApprovedBy,ArAccntTermStatusId")] ArAccntTerm arAccntTerm)
        {
            if (ModelState.IsValid)
            {
                db.ArAccntTerms.Add(arAccntTerm);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ArAccountId = new SelectList(db.ArAccounts, "Id", "Name", arAccntTerm.ArAccountId);
            ViewBag.ArAccntTermStatusId = new SelectList(db.ArAccntTermStatus, "Id", "Status", arAccntTerm.ArAccntTermStatusId);
            return View(arAccntTerm);
        }

        // GET: Receivables/ArAccntTerms/Create
        public ActionResult CreateTerms(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ViewBag.ArAccountId = new SelectList(db.ArAccounts, "Id", "Name", id);
            ViewBag.ArAccntTermStatusId = new SelectList(db.ArAccntTermStatus, "Id", "Status");
            return View();
        }

        // POST: Receivables/ArAccntTerms/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateTerms([Bind(Include = "Id,dtTerm,NoOfDays,Remarks,ArAccountId,ApprovedBy,ArAccntTermStatusId")] ArAccntTerm arAccntTerm)
        {
            if (ModelState.IsValid)
            {
                db.ArAccntTerms.Add(arAccntTerm);
                db.SaveChanges();
                return RedirectToAction("Details", "ArAccounts", new { id = arAccntTerm.ArAccountId });
            }

            ViewBag.ArAccountId = new SelectList(db.ArAccounts, "Id", "Name", arAccntTerm.ArAccountId);
            ViewBag.ArAccntTermStatusId = new SelectList(db.ArAccntTermStatus, "Id", "Status", arAccntTerm.ArAccntTermStatusId);
            return View(arAccntTerm);
        }

        // GET: Receivables/ArAccntTerms/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ArAccntTerm arAccntTerm = db.ArAccntTerms.Find(id);
            if (arAccntTerm == null)
            {
                return HttpNotFound();
            }
            ViewBag.ArAccountId = new SelectList(db.ArAccounts, "Id", "Name", arAccntTerm.ArAccountId);
            ViewBag.ArAccntTermStatusId = new SelectList(db.ArAccntTermStatus, "Id", "Status", arAccntTerm.ArAccntTermStatusId);
            return View(arAccntTerm);
        }

        // POST: Receivables/ArAccntTerms/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,dtTerm,NoOfDays,Remarks,ArAccountId,ApprovedBy,ArAccntTermStatusId")] ArAccntTerm arAccntTerm)
        {
            if (ModelState.IsValid)
            {
                db.Entry(arAccntTerm).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", "ArAccounts", new { id = arAccntTerm.ArAccountId });
            }
            ViewBag.ArAccountId = new SelectList(db.ArAccounts, "Id", "Name", arAccntTerm.ArAccountId);
            ViewBag.ArAccntTermStatusId = new SelectList(db.ArAccntTermStatus, "Id", "Status", arAccntTerm.ArAccntTermStatusId);
            return View(arAccntTerm);
        }

        // GET: Receivables/ArAccntTerms/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ArAccntTerm arAccntTerm = db.ArAccntTerms.Find(id);
            if (arAccntTerm == null)
            {
                return HttpNotFound();
            }
            return View(arAccntTerm);
        }

        // POST: Receivables/ArAccntTerms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ArAccntTerm arAccntTerm = db.ArAccntTerms.Find(id);
            db.ArAccntTerms.Remove(arAccntTerm);
            db.SaveChanges();
            return RedirectToAction("Details", "ArAccounts", new { id = arAccntTerm.ArAccountId });
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
