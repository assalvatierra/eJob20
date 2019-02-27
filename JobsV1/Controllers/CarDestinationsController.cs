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
    public class CarDestinationsController : Controller
    {
        private JobDBContainer db = new JobDBContainer();

        // GET: CarDestinations
        public ActionResult Index()
        {
            var carDestinations = db.CarDestinations.Include(c => c.City);
            return View(carDestinations.ToList());
        }

        // GET: CarDestinations/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CarDestination carDestination = db.CarDestinations.Find(id);
            if (carDestination == null)
            {
                return HttpNotFound();
            }
            return View(carDestination);
        }

        // GET: CarDestinations/Create
        public ActionResult Create()
        {
            ViewBag.CityId = new SelectList(db.Cities, "Id", "Name");
            return View();
        }

        // POST: CarDestinations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,CityId,Description,Kms")] CarDestination carDestination)
        {
            if (ModelState.IsValid)
            {
                db.CarDestinations.Add(carDestination);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CityId = new SelectList(db.Cities, "Id", "Name", carDestination.CityId);
            return View(carDestination);
        }

        // GET: CarDestinations/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CarDestination carDestination = db.CarDestinations.Find(id);
            if (carDestination == null)
            {
                return HttpNotFound();
            }
            ViewBag.CityId = new SelectList(db.Cities, "Id", "Name", carDestination.CityId);
            return View(carDestination);
        }

        // POST: CarDestinations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CityId,Description,Kms")] CarDestination carDestination)
        {
            if (ModelState.IsValid)
            {
                db.Entry(carDestination).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CityId = new SelectList(db.Cities, "Id", "Name", carDestination.CityId);
            return View(carDestination);
        }

        // GET: CarDestinations/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CarDestination carDestination = db.CarDestinations.Find(id);
            if (carDestination == null)
            {
                return HttpNotFound();
            }
            return View(carDestination);
        }

        // POST: CarDestinations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CarDestination carDestination = db.CarDestinations.Find(id);
            db.CarDestinations.Remove(carDestination);
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
