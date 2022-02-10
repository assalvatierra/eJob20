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
    public class crLogDriverPaymentsController : Controller
    {
        private CarRentalLogDBContainer db = new CarRentalLogDBContainer();

        // GET: Personel/crLogDriverPayments
        public ActionResult Index(int id)
        {
            var crLogDriverPayments = db.crLogDriverPayments.Include(c => c.crLogDriver)
                .Where(c=>c.crLogDriverId == id);

            ViewBag.Driver = db.crLogDrivers.Find(id).Name;
            ViewBag.DriverId = id;
            return View(crLogDriverPayments.ToList());
        }

        // GET: Personel/crLogDriverPayments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            crLogDriverPayment crLogDriverPayment = db.crLogDriverPayments.Find(id);
            if (crLogDriverPayment == null)
            {
                return HttpNotFound();
            }
            return View(crLogDriverPayment);
        }

        // GET: Personel/crLogDriverPayments/Create
        public ActionResult Create()
        {
            ViewBag.crLogDriverId = new SelectList(db.crLogDrivers, "Id", "Name");
            return View();
        }

        // POST: Personel/crLogDriverPayments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Description,Amount,Remarks,crLogDriverId")] crLogDriverPayment crLogDriverPayment)
        {
            if (ModelState.IsValid)
            {
                db.crLogDriverPayments.Add(crLogDriverPayment);
                db.SaveChanges();
                return RedirectToAction("Index", new { id = crLogDriverPayment.crLogDriverId });
            }

            ViewBag.crLogDriverId = new SelectList(db.crLogDrivers, "Id", "Name", crLogDriverPayment.crLogDriverId);
            return View(crLogDriverPayment);
        }

        // GET: Personel/crLogDriverPayments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            crLogDriverPayment crLogDriverPayment = db.crLogDriverPayments.Find(id);
            if (crLogDriverPayment == null)
            {
                return HttpNotFound();
            }
            ViewBag.crLogDriverId = new SelectList(db.crLogDrivers, "Id", "Name", crLogDriverPayment.crLogDriverId);
            return View(crLogDriverPayment);
        }

        // POST: Personel/crLogDriverPayments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Description,Amount,Remarks,crLogDriverId")] crLogDriverPayment crLogDriverPayment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(crLogDriverPayment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { id = crLogDriverPayment.crLogDriverId });
            }
            ViewBag.crLogDriverId = new SelectList(db.crLogDrivers, "Id", "Name", crLogDriverPayment.crLogDriverId);
            return View(crLogDriverPayment);
        }

        // GET: Personel/crLogDriverPayments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            crLogDriverPayment crLogDriverPayment = db.crLogDriverPayments.Find(id);
            if (crLogDriverPayment == null)
            {
                return HttpNotFound();
            }
            return View(crLogDriverPayment);
        }

        // POST: Personel/crLogDriverPayments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            crLogDriverPayment crLogDriverPayment = db.crLogDriverPayments.Find(id);
            db.crLogDriverPayments.Remove(crLogDriverPayment);
            db.SaveChanges();
            return RedirectToAction("Index", new { id = crLogDriverPayment.crLogDriverId });
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
