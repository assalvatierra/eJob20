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
    public class AccntTrxHdrsController : Controller
    {
        private AccountingDBContainer db = new AccountingDBContainer();

        // GET: Accounting/AccntTrxHdrs
        public ActionResult Index()
        {
            var accntTrxHdrs = db.AccntTrxHdrs.Include(a => a.AccntTrxType);
            return View(accntTrxHdrs.ToList());
        }

        // GET: Accounting/AccntTrxHdrs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AccntTrxHdr accntTrxHdr = db.AccntTrxHdrs.Find(id);
            if (accntTrxHdr == null)
            {
                return HttpNotFound();
            }

            ViewBag.TrxDtls = db.AccntTrxDtls.Where(c => c.AccntTrxHdrId == id).ToList();


            return View(accntTrxHdr);
        }

        public ActionResult Details_Jobs(int id)
        {
            var TrxDtls = db.AccntTrxDtls.Where(c => c.AccntTrxHdrId == id).OrderByDescending(s=>s.Id).ToList();

            return View(TrxDtls);
        }

        // GET: Accounting/AccntTrxHdrs/Create
        public ActionResult Create()
        {
            AccntTrxHdr trxHdr = new AccntTrxHdr();
            trxHdr.DtTrx = getDateTimeToday();

            ViewBag.AccntTrxTypeId = new SelectList(db.AccntTrxTypes, "Id", "Remarks");
            return View(trxHdr);
        }

        // POST: Accounting/AccntTrxHdrs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,AccntTrxTypeId,DtTrx,Remarks")] AccntTrxHdr accntTrxHdr)
        {
            if (ModelState.IsValid)
            {
                db.AccntTrxHdrs.Add(accntTrxHdr);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AccntTrxTypeId = new SelectList(db.AccntTrxTypes, "Id", "Remarks", accntTrxHdr.AccntTrxTypeId);
            return View(accntTrxHdr);
        }

        // GET: Accounting/AccntTrxHdrs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AccntTrxHdr accntTrxHdr = db.AccntTrxHdrs.Find(id);
            if (accntTrxHdr == null)
            {
                return HttpNotFound();
            }
            ViewBag.AccntTrxTypeId = new SelectList(db.AccntTrxTypes, "Id", "Remarks", accntTrxHdr.AccntTrxTypeId);
            return View(accntTrxHdr);
        }

        // POST: Accounting/AccntTrxHdrs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,AccntTrxTypeId,DtTrx,Remarks")] AccntTrxHdr accntTrxHdr)
        {
            if (ModelState.IsValid)
            {
                db.Entry(accntTrxHdr).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", new { id = accntTrxHdr.Id });
            }
            ViewBag.AccntTrxTypeId = new SelectList(db.AccntTrxTypes, "Id", "Remarks", accntTrxHdr.AccntTrxTypeId);
            return View(accntTrxHdr);
        }

        // GET: Accounting/AccntTrxHdrs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AccntTrxHdr accntTrxHdr = db.AccntTrxHdrs.Find(id);
            if (accntTrxHdr == null)
            {
                return HttpNotFound();
            }
            return View(accntTrxHdr);
        }

        // POST: Accounting/AccntTrxHdrs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AccntTrxHdr accntTrxHdr = db.AccntTrxHdrs.Find(id);
            db.AccntTrxHdrs.Remove(accntTrxHdr);
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

        //get utc date time today (singapore standard time) gmt + 8
        public DateTime getDateTimeToday()
        {
            DateTime today = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Singapore Standard Time"));
            return today;
        }
    }
}
