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
    public class HrPayrollsController : Controller
    {
        private HrisDBContainer db = new HrisDBContainer();

        // GET: Personel/HrPayrolls
        public ActionResult Index()
        {
            var hrPayrolls = db.HrPayrolls.Include(h => h.HrPersonel);
            return View(hrPayrolls.ToList());
        }

        // GET: Personel/HrPayrolls/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HrPayroll hrPayroll = db.HrPayrolls.Find(id);
            if (hrPayroll == null)
            {
                return HttpNotFound();
            }
            return View(hrPayroll);
        }

        // GET: Personel/HrPayrolls/Create
        public ActionResult Create()
        {
            ViewBag.HrPersonelId = new SelectList(db.HrPersonels, "Id", "Name");
            return View();
        }

        // POST: Personel/HrPayrolls/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,HrPersonelId,DtStart,DtEnd,Salary,Allowance,Deduction,Yearno,Monthno,Status")] HrPayroll hrPayroll)
        {
            if (ModelState.IsValid)
            {
                db.HrPayrolls.Add(hrPayroll);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.HrPersonelId = new SelectList(db.HrPersonels, "Id", "Name", hrPayroll.HrPersonelId);
            return View(hrPayroll);
        }

        // GET: Personel/HrPayrolls/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HrPayroll hrPayroll = db.HrPayrolls.Find(id);
            if (hrPayroll == null)
            {
                return HttpNotFound();
            }
            ViewBag.HrPersonelId = new SelectList(db.HrPersonels, "Id", "Name", hrPayroll.HrPersonelId);
            return View(hrPayroll);
        }

        // POST: Personel/HrPayrolls/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,HrPersonelId,DtStart,DtEnd,Salary,Allowance,Deduction,Yearno,Monthno,Status")] HrPayroll hrPayroll)
        {
            if (ModelState.IsValid)
            {
                db.Entry(hrPayroll).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.HrPersonelId = new SelectList(db.HrPersonels, "Id", "Name", hrPayroll.HrPersonelId);
            return View(hrPayroll);
        }

        // GET: Personel/HrPayrolls/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HrPayroll hrPayroll = db.HrPayrolls.Find(id);
            if (hrPayroll == null)
            {
                return HttpNotFound();
            }
            return View(hrPayroll);
        }

        // POST: Personel/HrPayrolls/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            HrPayroll hrPayroll = db.HrPayrolls.Find(id);
            db.HrPayrolls.Remove(hrPayroll);
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
