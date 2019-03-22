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
    public class AccntLedgersController : Controller
    {
        private AccountingDBContainer db = new AccountingDBContainer();

        // GET: Accounting/AccntLedgers
        public ActionResult Index()
        {
            var accntLedgers = db.AccntLedgers.Include(a => a.AccntMain);
            return View(accntLedgers.ToList());
        }

        // GET: Accounting/AccntLedgers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AccntLedger accntLedger = db.AccntLedgers.Find(id);
            if (accntLedger == null)
            {
                return HttpNotFound();
            }
            return View(accntLedger);
        }

        // GET: Accounting/AccntLedgers/Create
        public ActionResult Create()
        {
            ViewBag.AccntMainId = new SelectList(db.AccntMains, "Id", "Code");
            return View();
        }

        // POST: Accounting/AccntLedgers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Code,Name,Remarks,AccntMainId")] AccntLedger accntLedger)
        {
            if (ModelState.IsValid)
            {
                db.AccntLedgers.Add(accntLedger);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AccntMainId = new SelectList(db.AccntMains, "Id", "Code", accntLedger.AccntMainId);
            return View(accntLedger);
        }

        // GET: Accounting/AccntLedgers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AccntLedger accntLedger = db.AccntLedgers.Find(id);
            if (accntLedger == null)
            {
                return HttpNotFound();
            }
            ViewBag.AccntMainId = new SelectList(db.AccntMains, "Id", "Code", accntLedger.AccntMainId);
            return View(accntLedger);
        }

        // POST: Accounting/AccntLedgers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Code,Name,Remarks,AccntMainId")] AccntLedger accntLedger)
        {
            if (ModelState.IsValid)
            {
                db.Entry(accntLedger).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AccntMainId = new SelectList(db.AccntMains, "Id", "Code", accntLedger.AccntMainId);
            return View(accntLedger);
        }

        // GET: Accounting/AccntLedgers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AccntLedger accntLedger = db.AccntLedgers.Find(id);
            if (accntLedger == null)
            {
                return HttpNotFound();
            }
            return View(accntLedger);
        }

        // POST: Accounting/AccntLedgers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AccntLedger accntLedger = db.AccntLedgers.Find(id);
            db.AccntLedgers.Remove(accntLedger);
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
