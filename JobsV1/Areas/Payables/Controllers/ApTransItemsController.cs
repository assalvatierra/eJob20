using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ApModels.Models;

namespace Payable.Areas.Payables.Controllers
{
    public class ApTransItemsController : Controller
    {
        private ApDBContainer db = new ApDBContainer();

        // GET: Payables/ApTransItems
        public ActionResult Index()
        {
            var apTransItems = db.ApTransItems.Include(a => a.ApTransaction);
            return View(apTransItems.ToList());
        }

        // GET: Payables/ApTransItems/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApTransItems apTransItems = db.ApTransItems.Find(id);
            if (apTransItems == null)
            {
                return HttpNotFound();
            }
            return View(apTransItems);
        }

        // GET: Payables/ApTransItems/Create
        public ActionResult Create(int? transId)
        {
            ViewBag.ApTransactionId = new SelectList(db.ApTransactions, "Id", "InvoiceNo", transId);
            ViewBag.TransId = transId;
            return View();
        }

        // POST: Payables/ApTransItems/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Description,Amount,Remarks,ApTransactionId")] ApTransItems apTransItems)
        {
            if (ModelState.IsValid)
            {
                db.ApTransItems.Add(apTransItems);
                db.SaveChanges();

                //redirect to transction details
                return RedirectToAction("Details", "ApTransactions", new { id = apTransItems.ApTransactionId });

                //return RedirectToAction("Index");
            }

            ViewBag.ApTransactionId = new SelectList(db.ApTransactions, "Id", "InvoiceNo", apTransItems.ApTransactionId);
            return View(apTransItems);
        }

        // GET: Payables/ApTransItems/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApTransItems apTransItems = db.ApTransItems.Find(id);
            if (apTransItems == null)
            {
                return HttpNotFound();
            }
            ViewBag.ApTransactionId = new SelectList(db.ApTransactions, "Id", "InvoiceNo", apTransItems.ApTransactionId);
            ViewBag.TransId = apTransItems.ApTransactionId;
            return View(apTransItems);
        }

        // POST: Payables/ApTransItems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Description,Amount,Remarks,ApTransactionId")] ApTransItems apTransItems)
        {
            if (ModelState.IsValid)
            {
                db.Entry(apTransItems).State = EntityState.Modified;
                db.SaveChanges();
                //redirect to transction details
                return RedirectToAction("Details", "ApTransactions", new { id = apTransItems.ApTransactionId });

                //return RedirectToAction("Index");
            }
            ViewBag.ApTransactionId = new SelectList(db.ApTransactions, "Id", "InvoiceNo", apTransItems.ApTransactionId);
            return View(apTransItems);
        }

        // GET: Payables/ApTransItems/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApTransItems apTransItems = db.ApTransItems.Find(id);
            if (apTransItems == null)
            {
                return HttpNotFound();
            }
            return View(apTransItems);
        }

        // POST: Payables/ApTransItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ApTransItems apTransItems = db.ApTransItems.Find(id);
            db.ApTransItems.Remove(apTransItems);
            db.SaveChanges();
            //redirect to transction details
            return RedirectToAction("Details", "ApTransactions", new { id = apTransItems.ApTransactionId });

            //return RedirectToAction("Index");
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
