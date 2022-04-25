using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using JobsV1.Areas.Personel.Models;
using Microsoft.Ajax.Utilities;

namespace JobsV1.Areas.Personel.Controllers
{
    public class crLogDriverTermsController : Controller
    {
        private CarRentalLogDBContainer db = new CarRentalLogDBContainer();

        // GET: Personel/crLogDriverTerms
        public ActionResult Index()
        {
            var crLogDriverTerms = db.crLogDriverTerms.Include(c => c.crLogDriver);
            return View(crLogDriverTerms.ToList());
        }

        // GET: Personel/crLogDriverTerms/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            crLogDriverTerm crLogDriverTerm = db.crLogDriverTerms.Find(id);
            if (crLogDriverTerm == null)
            {
                return HttpNotFound();
            }
            return View(crLogDriverTerm);
        }

        // GET: Personel/crLogDriverTerms/Create
        public ActionResult Create(int id)
        {
            ViewBag.crLogDriverId = new SelectList(db.crLogDrivers, "Id", "Name", id);
            return View();
        }

        // POST: Personel/crLogDriverTerms/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Description,crLogDriverId,Date")] crLogDriverTerm crLogDriverTerm)
        {
            if (ModelState.IsValid)
            {
                crLogDriverTerm.Date = new DateTime();
                db.crLogDriverTerms.Add(crLogDriverTerm);
                db.SaveChanges();
                //return RedirectToAction("Index");
                return RedirectToAction("DriverSummary", "crlogDrivers", new { id = crLogDriverTerm.crLogDriverId });
            }

            ViewBag.crLogDriverId = new SelectList(db.crLogDrivers, "Id", "Name", crLogDriverTerm.crLogDriverId);
            return View(crLogDriverTerm);
        }

        // GET: Personel/crLogDriverTerms/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            crLogDriverTerm crLogDriverTerm = db.crLogDriverTerms.Find(id);
            if (crLogDriverTerm == null)
            {
                return HttpNotFound();
            }
            ViewBag.crLogDriverId = new SelectList(db.crLogDrivers, "Id", "Name", crLogDriverTerm.crLogDriverId);
            return View(crLogDriverTerm);
        }

        // POST: Personel/crLogDriverTerms/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Description,crLogDriverId,Date")] crLogDriverTerm crLogDriverTerm)
        {
            if (ModelState.IsValid)
            {
                crLogDriverTerm.Date = new DateTime();
                db.Entry(crLogDriverTerm).State = EntityState.Modified;
                db.SaveChanges();
                //return RedirectToAction("Index");
                return RedirectToAction("DriverSummary", "crlogDrivers", new { id = crLogDriverTerm.Id });
            }
            ViewBag.crLogDriverId = new SelectList(db.crLogDrivers, "Id", "Name", crLogDriverTerm.crLogDriverId);
            return View(crLogDriverTerm);
        }

        // GET: Personel/crLogDriverTerms/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            crLogDriverTerm crLogDriverTerm = db.crLogDriverTerms.Find(id);
            if (crLogDriverTerm == null)
            {
                return HttpNotFound();
            }
            return View(crLogDriverTerm);
        }

        // POST: Personel/crLogDriverTerms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            crLogDriverTerm crLogDriverTerm = db.crLogDriverTerms.Find(id);
            db.crLogDriverTerms.Remove(crLogDriverTerm);
            db.SaveChanges();
            //return RedirectToAction("Index");
            return RedirectToAction("DriverSummary", "crlogDrivers", new { id = crLogDriverTerm.Id });
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
