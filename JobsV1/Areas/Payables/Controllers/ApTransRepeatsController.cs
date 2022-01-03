using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ApModels.Models;

namespace JobsV1.Areas.Payables.Controllers
{
    public class ApTransRepeatsController : Controller
    {
        private ApDBContainer db = new ApDBContainer();

        // GET: Payables/ApTransRepeats
        public ActionResult Index()
        {
            return View(db.ApTransRepeats.ToList());
        }

        // GET: Payables/ApTransRepeats/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApTransRepeat apTransRepeat = db.ApTransRepeats.Find(id);
            if (apTransRepeat == null)
            {
                return HttpNotFound();
            }
            return View(apTransRepeat);
        }

        // GET: Payables/ApTransRepeats/Create
        public ActionResult Create(int id)
        {

            ViewBag.Transaction = db.ApTransactions.Find(id).Description;

            ApTransRepeat apTransRepeat = new ApTransRepeat();
            apTransRepeat.ApTransactionId = id;
            apTransRepeat.Interval = 30;
            apTransRepeat.RepeatCount = 12;
            apTransRepeat.RepeatNo = 1;

            ViewBag.TransId = id;

            return View(apTransRepeat);
        }

        // POST: Payables/ApTransRepeats/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,RepeatCount,RepeatNo,NextRef,PrevRef,ApTransactionId,Interval")] ApTransRepeat apTransRepeat)
        {
            if (ModelState.IsValid)
            {
                if (apTransRepeat.ApTransactionId == 0)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                ApTransaction apTransaction = db.ApTransactions.Find(apTransRepeat.ApTransactionId);
                if (apTransaction == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                apTransRepeat.ApTransaction = apTransaction;

                db.ApTransRepeats.Add(apTransRepeat);
                db.SaveChanges();
                return RedirectToAction("Details", "ApTransactions",new {  id = apTransRepeat.ApTransactionId  });
                //return RedirectToAction("Index");
            }

            return View(apTransRepeat);
        }

        // GET: Payables/ApTransRepeats/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApTransRepeat apTransRepeat = db.ApTransRepeats.Find(id);
            if (apTransRepeat == null)
            {
                return HttpNotFound();
            }
            return View(apTransRepeat);
        }

        // POST: Payables/ApTransRepeats/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,RepeatCount,RepeatNo,NextRef,PrevRef,ApTransactionId,Interval")] ApTransRepeat apTransRepeat)
        {
            if (ModelState.IsValid)
            {
                db.Entry(apTransRepeat).State = EntityState.Modified;
                db.SaveChanges();
                //return RedirectToAction("Index");
                return RedirectToAction("Details", "ApTransactions", new { id = apTransRepeat.ApTransactionId });
            }
            return View(apTransRepeat);
        }

        // GET: Payables/ApTransRepeats/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApTransRepeat apTransRepeat = db.ApTransRepeats.Find(id);
            if (apTransRepeat == null)
            {
                return HttpNotFound();
            }
            return View(apTransRepeat);
        }

        // POST: Payables/ApTransRepeats/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ApTransRepeat apTransRepeat = db.ApTransRepeats.Find(id);
            db.ApTransRepeats.Remove(apTransRepeat);
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
