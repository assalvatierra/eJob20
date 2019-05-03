using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using JobsV1.Areas.Personel.Models;

namespace JobsV1.Areas.Personel.Controllers
{
    public class HrDtrsController : Controller
    {
        private HrisDBContainer db = new HrisDBContainer();

        // GET: Personel/HrDtrs
        public ActionResult Index()
        {
            var hrDtrs = db.HrDtrs.Include(h => h.HrDtrStatu).Include(h => h.HrPersonel).Include(h => h.HrSalary);
            return View(hrDtrs.ToList());
        }

        // GET: Personel/HrDtrs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HrDtr hrDtr = db.HrDtrs.Find(id);
            if (hrDtr == null)
            {
                return HttpNotFound();
            }
            return View(hrDtr);
        }

        // GET: Personel/HrDtrs/Create
        public ActionResult Create()
        {
            ViewBag.HrDtrStatusId = new SelectList(db.HrDtrStatus, "Id", "Desc");
            ViewBag.HrPersonelId = new SelectList(db.HrPersonels, "Id", "Name");
            ViewBag.HrSalaryId = new SelectList(db.HrSalaries, "Id", "Id");
            return View();
        }

        // POST: Personel/HrDtrs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,HrSalaryId,DtrDate,HrDtrStatusId,HrPersonelId,TimeIn,TimeOut,ActualHrs,RoundHrs,HrPayrollId")] HrDtr hrDtr)
        {
            if (ModelState.IsValid)
            {
                db.HrDtrs.Add(hrDtr);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.HrDtrStatusId = new SelectList(db.HrDtrStatus, "Id", "Desc", hrDtr.HrDtrStatusId);
            ViewBag.HrPersonelId = new SelectList(db.HrPersonels, "Id", "Name", hrDtr.HrPersonelId);
            ViewBag.HrSalaryId = new SelectList(db.HrSalaries, "Id", "Id", hrDtr.HrSalaryId);
            return View(hrDtr);
        }

        // GET: Personel/HrDtrs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HrDtr hrDtr = db.HrDtrs.Find(id);
            if (hrDtr == null)
            {
                return HttpNotFound();
            }
            ViewBag.HrDtrStatusId = new SelectList(db.HrDtrStatus, "Id", "Desc", hrDtr.HrDtrStatusId);
            ViewBag.HrPersonelId = new SelectList(db.HrPersonels, "Id", "Name", hrDtr.HrPersonelId);
            ViewBag.HrSalaryId = new SelectList(db.HrSalaries, "Id", "Id", hrDtr.HrSalaryId);
            return View(hrDtr);
        }

        // POST: Personel/HrDtrs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,HrSalaryId,DtrDate,HrDtrStatusId,HrPersonelId,TimeIn,TimeOut,ActualHrs,RoundHrs,HrPayrollId")] HrDtr hrDtr)
        {
            if (ModelState.IsValid)
            {
                db.Entry(hrDtr).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.HrDtrStatusId = new SelectList(db.HrDtrStatus, "Id", "Desc", hrDtr.HrDtrStatusId);
            ViewBag.HrPersonelId = new SelectList(db.HrPersonels, "Id", "Name", hrDtr.HrPersonelId);
            ViewBag.HrSalaryId = new SelectList(db.HrSalaries, "Id", "Id", hrDtr.HrSalaryId);
            return View(hrDtr);
        }

        // GET: Personel/HrDtrs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HrDtr hrDtr = db.HrDtrs.Find(id);
            if (hrDtr == null)
            {
                return HttpNotFound();
            }
            return View(hrDtr);
        }

        // POST: Personel/HrDtrs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            HrDtr hrDtr = db.HrDtrs.Find(id);
            db.HrDtrs.Remove(hrDtr);
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
