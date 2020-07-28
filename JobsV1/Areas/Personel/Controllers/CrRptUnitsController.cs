using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using JobsV1.Areas.Personel.Models;
using JobsV1.Models;

namespace JobsV1.Areas.Personel.Controllers
{
    public class CrRptUnitsController : Controller
    {
        private CarRentalLogDBContainer db = new CarRentalLogDBContainer(); 
        // GET: Personel/CrRptUnits
        public ActionResult Index()
        {
            var crRptUnits = db.CrRptUnits.Include(c => c.crLogUnit).Include(c => c.crRptUnitExpense);
            return View(crRptUnits.ToList());
        }

        // GET: Personel/CrRptUnits/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CrRptUnit crRptUnit = db.CrRptUnits.Find(id);
            if (crRptUnit == null)
            {
                return HttpNotFound();
            }
            return View(crRptUnit);
        }

        // GET: Personel/CrRptUnits/Create
        public ActionResult Create()
        {
            ViewBag.crLogUnitId = new SelectList(db.crLogUnits, "Id", "Description");
            ViewBag.crRptUnitExpenseId = new SelectList(db.crRptUnitExpenses, "Id", "RptName");
            return View();
        }

        // POST: Personel/CrRptUnits/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,crRptUnitExpenseId,crLogUnitId,RptSeqNo")] CrRptUnit crRptUnit)
        {
            if (ModelState.IsValid)
            {
                db.CrRptUnits.Add(crRptUnit);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.crLogUnitId = new SelectList(db.crLogUnits, "Id", "Description", crRptUnit.crLogUnitId);
            ViewBag.crRptUnitExpenseId = new SelectList(db.crRptUnitExpenses, "Id", "RptName", crRptUnit.crRptUnitExpenseId);
            return View(crRptUnit);
        }

        // GET: Personel/CrRptUnits/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CrRptUnit crRptUnit = db.CrRptUnits.Find(id);
            if (crRptUnit == null)
            {
                return HttpNotFound();
            }
            ViewBag.crLogUnitId = new SelectList(db.crLogUnits, "Id", "Description", crRptUnit.crLogUnitId);
            ViewBag.crRptUnitExpenseId = new SelectList(db.crRptUnitExpenses, "Id", "RptName", crRptUnit.crRptUnitExpenseId);
            return View(crRptUnit);
        }

        // POST: Personel/CrRptUnits/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,crRptUnitExpenseId,crLogUnitId,RptSeqNo")] CrRptUnit crRptUnit)
        {
            if (ModelState.IsValid)
            {
                db.Entry(crRptUnit).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.crLogUnitId = new SelectList(db.crLogUnits, "Id", "Description", crRptUnit.crLogUnitId);
            ViewBag.crRptUnitExpenseId = new SelectList(db.crRptUnitExpenses, "Id", "RptName", crRptUnit.crRptUnitExpenseId);
            return View(crRptUnit);
        }

        // GET: Personel/CrRptUnits/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CrRptUnit crRptUnit = db.CrRptUnits.Find(id);
            if (crRptUnit == null)
            {
                return HttpNotFound();
            }
            return View(crRptUnit);
        }

        // POST: Personel/CrRptUnits/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CrRptUnit crRptUnit = db.CrRptUnits.Find(id);
            db.CrRptUnits.Remove(crRptUnit);
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
