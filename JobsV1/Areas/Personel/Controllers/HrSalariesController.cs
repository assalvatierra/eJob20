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
    public class HrSalariesController : Controller
    {
        private HrisDBContainer db = new HrisDBContainer();

        // GET: Personel/HrSalaries
        public ActionResult Index()
        {
            var hrSalaries = db.HrSalaries.Include(h => h.HrPersonel);
            return View(hrSalaries.ToList());
        }

        // GET: Personel/HrSalaries/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HrSalary hrSalary = db.HrSalaries.Find(id);
            if (hrSalary == null)
            {
                return HttpNotFound();
            }
            return View(hrSalary);
        }

        // GET: Personel/HrSalaries/Create
        public ActionResult Create()
        {
            ViewBag.HrPersonelId = new SelectList(db.HrPersonels, "Id", "Name");
            return View();
        }

        // POST: Personel/HrSalaries/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,HrPersonelId,DtStart,RatePerHr")] HrSalary hrSalary)
        {
            if (ModelState.IsValid)
            {
                db.HrSalaries.Add(hrSalary);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.HrPersonelId = new SelectList(db.HrPersonels, "Id", "Name", hrSalary.HrPersonelId);
            return View(hrSalary);
        }

        // GET: Personel/HrSalaries/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HrSalary hrSalary = db.HrSalaries.Find(id);
            if (hrSalary == null)
            {
                return HttpNotFound();
            }
            ViewBag.HrPersonelId = new SelectList(db.HrPersonels, "Id", "Name", hrSalary.HrPersonelId);
            return View(hrSalary);
        }

        // POST: Personel/HrSalaries/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,HrPersonelId,DtStart,RatePerHr")] HrSalary hrSalary)
        {
            if (ModelState.IsValid)
            {
                db.Entry(hrSalary).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.HrPersonelId = new SelectList(db.HrPersonels, "Id", "Name", hrSalary.HrPersonelId);
            return View(hrSalary);
        }

        // GET: Personel/HrSalaries/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HrSalary hrSalary = db.HrSalaries.Find(id);
            if (hrSalary == null)
            {
                return HttpNotFound();
            }
            return View(hrSalary);
        }

        // POST: Personel/HrSalaries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            HrSalary hrSalary = db.HrSalaries.Find(id);
            db.HrSalaries.Remove(hrSalary);
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
