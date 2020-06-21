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
    public class CarRentalCashReleaseController : Controller
    {
        private CarRentalLogDBContainer db = new CarRentalLogDBContainer();

        // GET: Personel/CarRentalCashRelease
        public ActionResult Index()
        {
            var crLogCashReleases = db.crLogCashReleases.Include(c => c.crLogDriver).Include(c => c.crLogClosing);
            return View(crLogCashReleases.ToList());
        }

        // GET: Personel/CarRentalCashRelease/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            crLogCashRelease crLogCashRelease = db.crLogCashReleases.Find(id);
            if (crLogCashRelease == null)
            {
                return HttpNotFound();
            }
            return View(crLogCashRelease);
        }

        // GET: Personel/CarRentalCashRelease/Create
        public ActionResult Create()
        {
            crLogCashRelease crtrx = new crLogCashRelease();
            crtrx.DtRelease = System.DateTime.Now;
            
            ViewBag.crLogDriverId = new SelectList(db.crLogDrivers, "Id", "Name");
            ViewBag.crLogClosingId = new SelectList(db.crLogClosings, "Id", "Id");
            return View(crtrx);
        }

        // POST: Personel/CarRentalCashRelease/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,DtRelease,Amount,Remarks,crLogDriverId,crLogClosingId")] crLogCashRelease crLogCashRelease)
        {
            if (ModelState.IsValid)
            {
                db.crLogCashReleases.Add(crLogCashRelease);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.crLogDriverId = new SelectList(db.crLogDrivers, "Id", "Name", crLogCashRelease.crLogDriverId);
            ViewBag.crLogClosingId = new SelectList(db.crLogClosings, "Id", "Id", crLogCashRelease.crLogClosingId);
            return View(crLogCashRelease);
        }

        // GET: Personel/CarRentalCashRelease/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            crLogCashRelease crLogCashRelease = db.crLogCashReleases.Find(id);
            if (crLogCashRelease == null)
            {
                return HttpNotFound();
            }
            ViewBag.crLogDriverId = new SelectList(db.crLogDrivers, "Id", "Name", crLogCashRelease.crLogDriverId);
            ViewBag.crLogClosingId = new SelectList(db.crLogClosings, "Id", "Id", crLogCashRelease.crLogClosingId);
            return View(crLogCashRelease);
        }

        // POST: Personel/CarRentalCashRelease/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,DtRelease,Amount,Remarks,crLogDriverId,crLogClosingId")] crLogCashRelease crLogCashRelease)
        {
            if (ModelState.IsValid)
            {
                db.Entry(crLogCashRelease).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.crLogDriverId = new SelectList(db.crLogDrivers, "Id", "Name", crLogCashRelease.crLogDriverId);
            ViewBag.crLogClosingId = new SelectList(db.crLogClosings, "Id", "Id", crLogCashRelease.crLogClosingId);
            return View(crLogCashRelease);
        }

        // GET: Personel/CarRentalCashRelease/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            crLogCashRelease crLogCashRelease = db.crLogCashReleases.Find(id);
            if (crLogCashRelease == null)
            {
                return HttpNotFound();
            }
            return View(crLogCashRelease);
        }

        // POST: Personel/CarRentalCashRelease/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            crLogCashRelease crLogCashRelease = db.crLogCashReleases.Find(id);
            db.crLogCashReleases.Remove(crLogCashRelease);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult DriverSummary(int id)
        {
            return RedirectToAction("CarRentalLog", "DriverSummary", new { id = id });
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
