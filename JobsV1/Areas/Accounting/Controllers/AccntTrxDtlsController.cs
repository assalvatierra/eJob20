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
    public class AccntTrxDtlsController : Controller
    {
        private AccountingDBContainer db = new AccountingDBContainer();

        // GET: Accounting/AccntTrxDtls
        public ActionResult Index()
        {
            var accntTrxDtls = db.AccntTrxDtls.Include(a => a.AccntTrxHdr).Include(a => a.AccntLedger);
            return View(accntTrxDtls.ToList());
        }

        // GET: Accounting/AccntTrxDtls/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AccntTrxDtl accntTrxDtl = db.AccntTrxDtls.Find(id);
            if (accntTrxDtl == null)
            {
                return HttpNotFound();
            }
            return View(accntTrxDtl);
        }

        // GET: Accounting/AccntTrxDtls/Create
        public ActionResult Create(int? hdrId)
        {
            if (hdrId == null)
            {
                hdrId = 1;
            }
            var product = db.AccntLedgers.Select(x => new
             {
                 Id = x.Id,
                 Name = x.Name + " - " + x.Code
             });
            
            ViewBag.AccntTrxHdrId = new SelectList(db.AccntTrxHdrs, "Id", "Remarks", hdrId);
            ViewBag.AccntLedgerId = new SelectList(product, "Id", "Name");
            return View();
        }

        // POST: Accounting/AccntTrxDtls/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,AccntTrxHdrId,Remarks,DbAmt,CrAmt,AccntLedgerId")] AccntTrxDtl accntTrxDtl)
        {
            if (ModelState.IsValid)
            {
                db.AccntTrxDtls.Add(accntTrxDtl);
                db.SaveChanges();

                //return RedirectToAction("Index");
                return RedirectToAction("Details","AccntTrxHdrs", new { Id = accntTrxDtl.AccntTrxHdrId });
            }

            ViewBag.AccntTrxHdrId = new SelectList(db.AccntTrxHdrs, "Id", "Remarks", accntTrxDtl.AccntTrxHdrId);
            ViewBag.AccntLedgerId = new SelectList(db.AccntLedgers, "Id", "Name", accntTrxDtl.AccntLedgerId);
            return View(accntTrxDtl);
        }

        // GET: Accounting/AccntTrxDtls/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AccntTrxDtl accntTrxDtl = db.AccntTrxDtls.Find(id);
            if (accntTrxDtl == null)
            {
                return HttpNotFound();
            }
            ViewBag.AccntTrxHdrId = new SelectList(db.AccntTrxHdrs, "Id", "Remarks", accntTrxDtl.AccntTrxHdrId);
            ViewBag.AccntLedgerId = new SelectList(db.AccntLedgers, "Id", "Name", accntTrxDtl.AccntLedgerId);
            return View(accntTrxDtl);
        }

        // POST: Accounting/AccntTrxDtls/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,AccntTrxHdrId,Remarks,DbAmt,CrAmt,AccntLedgerId")] AccntTrxDtl accntTrxDtl)
        {
            if (ModelState.IsValid)
            {
                db.Entry(accntTrxDtl).State = EntityState.Modified;
                db.SaveChanges();
               // return RedirectToAction("Index")
                return RedirectToAction("Details", "AccntTrxHdrs", new { Id = accntTrxDtl.AccntTrxHdrId });
            }
            ViewBag.AccntTrxHdrId = new SelectList(db.AccntTrxHdrs, "Id", "Remarks", accntTrxDtl.AccntTrxHdrId);
            ViewBag.AccntLedgerId = new SelectList(db.AccntLedgers, "Id", "Name", accntTrxDtl.AccntLedgerId);
            return View(accntTrxDtl);
        }

        // GET: Accounting/AccntTrxDtls/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AccntTrxDtl accntTrxDtl = db.AccntTrxDtls.Find(id);
            if (accntTrxDtl == null)
            {
                return HttpNotFound();
            }
            return View(accntTrxDtl);
        }

        // POST: Accounting/AccntTrxDtls/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AccntTrxDtl accntTrxDtl = db.AccntTrxDtls.Find(id);
            db.AccntTrxDtls.Remove(accntTrxDtl);
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
