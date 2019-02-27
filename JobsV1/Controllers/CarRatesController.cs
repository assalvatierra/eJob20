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
    public class CarRatesController : Controller
    {
        private JobDBContainer db = new JobDBContainer();

        // GET: CarRates
        public ActionResult Index()
        {
            var carRates = db.CarRates.Include(c => c.CarUnit);
            return View(carRates.ToList());
        }

        // GET: CarRates/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CarRate carRate = db.CarRates.Find(id);
            if (carRate == null)
            {
                return HttpNotFound();
            }
            return View(carRate);
        }

        // GET: CarRates/Create
        public ActionResult Create()
        {
            ViewBag.CarUnitId = new SelectList(db.CarUnits, "Id", "Description");
            return View();
        }

        // POST: CarRates/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Daily,Weekly,Monthly,KmFree,KmRate,CarUnitId,OtRate")] CarRate carRate)
        {
            if (ModelState.IsValid)
            {
                db.CarRates.Add(carRate);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CarUnitId = new SelectList(db.CarUnits, "Id", "Description", carRate.CarUnitId);
            return View(carRate);
        }

        // GET: CarRates/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CarRate carRate = db.CarRates.Find(id);
            if (carRate == null)
            {
                return HttpNotFound();
            }
            ViewBag.CarUnitId = new SelectList(db.CarUnits, "Id", "Description", carRate.CarUnitId);
            return View(carRate);
        }

        // POST: CarRates/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Daily,Weekly,Monthly,KmFree,KmRate,CarUnitId,OtRate")] CarRate carRate)
        {
            if (ModelState.IsValid)
            {
                db.Entry(carRate).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CarUnitId = new SelectList(db.CarUnits, "Id", "Description", carRate.CarUnitId);
            return View(carRate);
        }

        // GET: CarRates/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CarRate carRate = db.CarRates.Find(id);
            if (carRate == null)
            {
                return HttpNotFound();
            }
            return View(carRate);
        }

        // POST: CarRates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CarRate carRate = db.CarRates.Find(id);
            db.CarRates.Remove(carRate);
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
