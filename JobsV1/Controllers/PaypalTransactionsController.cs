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
    public class PaypalTransactionsController : Controller
    {
        private JobDBContainer db = new JobDBContainer();

        // GET: PaypalTransactions
        public ActionResult Index()
        {
            return View(db.PaypalTransactions.ToList());
        }

        // GET: PaypalTransactions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PaypalTransaction paypalTransaction = db.PaypalTransactions.Find(id);
            if (paypalTransaction == null)
            {
                return HttpNotFound();
            }
            return View(paypalTransaction);
        }

        // GET: PaypalTransactions/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PaypalTransactions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,TrxId,JobId,TrxDate,DatePosted,Status,Remarks,Amount")] PaypalTransaction paypalTransaction)
        {
            if (ModelState.IsValid)
            {
                db.PaypalTransactions.Add(paypalTransaction);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(paypalTransaction);
        }

        // GET: PaypalTransactions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PaypalTransaction paypalTransaction = db.PaypalTransactions.Find(id);
            if (paypalTransaction == null)
            {
                return HttpNotFound();
            }
            return View(paypalTransaction);
        }

        // POST: PaypalTransactions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,TrxId,JobId,TrxDate,DatePosted,Status,Remarks,Amount")] PaypalTransaction paypalTransaction)
        {
            if (ModelState.IsValid)
            {
                db.Entry(paypalTransaction).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(paypalTransaction);
        }

        // GET: PaypalTransactions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PaypalTransaction paypalTransaction = db.PaypalTransactions.Find(id);
            if (paypalTransaction == null)
            {
                return HttpNotFound();
            }
            return View(paypalTransaction);
        }

        // POST: PaypalTransactions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PaypalTransaction paypalTransaction = db.PaypalTransactions.Find(id);
            db.PaypalTransactions.Remove(paypalTransaction);
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
