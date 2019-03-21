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
    public class AccntMainsController : Controller
    {
        private AccountingDBContainer db = new AccountingDBContainer();

        // GET: Accounting/AccntMains
        public ActionResult Index()
        {
            var accntMains = db.AccntMains.Include(a => a.AccntType);
            return View(accntMains.ToList());
        }

        // GET: Accounting/AccntMains/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AccntMain accntMain = db.AccntMains.Find(id);
            if (accntMain == null)
            {
                return HttpNotFound();
            }
            return View(accntMain);
        }

        // GET: Accounting/AccntMains/Create
        public ActionResult Create()
        {
            ViewBag.AccntTypeId = new SelectList(db.AccntTypes, "Id", "Code");
            return View();
        }

        // POST: Accounting/AccntMains/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Code,Name,Remarks,AccntTypeId")] AccntMain accntMain)
        {
            if (ModelState.IsValid)
            {
                db.AccntMains.Add(accntMain);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AccntTypeId = new SelectList(db.AccntTypes, "Id", "Code", accntMain.AccntTypeId);
            return View(accntMain);
        }

        // GET: Accounting/AccntMains/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AccntMain accntMain = db.AccntMains.Find(id);
            if (accntMain == null)
            {
                return HttpNotFound();
            }
            ViewBag.AccntTypeId = new SelectList(db.AccntTypes, "Id", "Code", accntMain.AccntTypeId);
            return View(accntMain);
        }

        // POST: Accounting/AccntMains/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Code,Name,Remarks,AccntTypeId")] AccntMain accntMain)
        {
            if (ModelState.IsValid)
            {
                db.Entry(accntMain).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AccntTypeId = new SelectList(db.AccntTypes, "Id", "Code", accntMain.AccntTypeId);
            return View(accntMain);
        }

        // GET: Accounting/AccntMains/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AccntMain accntMain = db.AccntMains.Find(id);
            if (accntMain == null)
            {
                return HttpNotFound();
            }
            return View(accntMain);
        }

        // POST: Accounting/AccntMains/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AccntMain accntMain = db.AccntMains.Find(id);
            db.AccntMains.Remove(accntMain);
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
