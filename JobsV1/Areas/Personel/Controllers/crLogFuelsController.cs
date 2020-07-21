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
    public class crLogFuelsController : Controller
    {
        private CarRentalLogDBContainer db = new CarRentalLogDBContainer();

        // GET: Personel/crLogFuels
        public ActionResult Index()
        {
            var crLogFuels = db.crLogFuels.Include(c => c.crLogUnit).Include(c => c.crLogDriver);
            return View(crLogFuels.ToList());
        }

        // GET: Personel/crLogFuels/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            crLogFuel crLogFuel = db.crLogFuels.Find(id);
            if (crLogFuel == null)
            {
                return HttpNotFound();
            }
            return View(crLogFuel);
        }

        // GET: Personel/crLogFuels/Create
        public ActionResult Create()
        {
            ViewBag.crLogUnitId = new SelectList(db.crLogUnits, "Id", "Description");
            ViewBag.crLogDriverId = new SelectList(db.crLogDrivers, "Id", "Name");
            return View();
        }

        // POST: Personel/crLogFuels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,dtRequest,Amount,crLogUnitId,crLogDriverId,dtFillup,odoFillup,orAmount")] crLogFuel crLogFuel)
        {
            if (ModelState.IsValid)
            {
                db.crLogFuels.Add(crLogFuel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.crLogUnitId = new SelectList(db.crLogUnits, "Id", "Description", crLogFuel.crLogUnitId);
            ViewBag.crLogDriverId = new SelectList(db.crLogDrivers, "Id", "Name", crLogFuel.crLogDriverId);
            return View(crLogFuel);
        }

        // GET: Personel/crLogFuels/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            crLogFuel crLogFuel = db.crLogFuels.Find(id);
            if (crLogFuel == null)
            {
                return HttpNotFound();
            }
            ViewBag.crLogUnitId = new SelectList(db.crLogUnits, "Id", "Description", crLogFuel.crLogUnitId);
            ViewBag.crLogDriverId = new SelectList(db.crLogDrivers, "Id", "Name", crLogFuel.crLogDriverId);
            return View(crLogFuel);
        }

        // POST: Personel/crLogFuels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,dtRequest,Amount,crLogUnitId,crLogDriverId,dtFillup,odoFillup,orAmount")] crLogFuel crLogFuel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(crLogFuel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.crLogUnitId = new SelectList(db.crLogUnits, "Id", "Description", crLogFuel.crLogUnitId);
            ViewBag.crLogDriverId = new SelectList(db.crLogDrivers, "Id", "Name", crLogFuel.crLogDriverId);
            return View(crLogFuel);
        }

        // GET: Personel/crLogFuels/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            crLogFuel crLogFuel = db.crLogFuels.Find(id);
            if (crLogFuel == null)
            {
                return HttpNotFound();
            }
            return View(crLogFuel);
        }

        // POST: Personel/crLogFuels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            crLogFuel crLogFuel = db.crLogFuels.Find(id);
            db.crLogFuels.Remove(crLogFuel);
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
