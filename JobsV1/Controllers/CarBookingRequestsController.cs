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
    public class CarBookingRequestsController : Controller
    {
        private JobDBContainer db = new JobDBContainer();

        // GET: CarBookingRequests
        public ActionResult Index()
        {
            return View(db.CarBookingRequests.ToList());
        }

        // GET: CarBookingRequests/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CarBookingRequest carBookingRequest = db.CarBookingRequests.Find(id);
            if (carBookingRequest == null)
            {
                return HttpNotFound();
            }
            return View(carBookingRequest);
        }

        // GET: CarBookingRequests/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CarBookingRequests/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,DtEncoded,DtBooking,Name,Mobile,Email,Unit,Destinations,Duration")] CarBookingRequest carBookingRequest)
        {
            if (ModelState.IsValid)
            {
                db.CarBookingRequests.Add(carBookingRequest);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(carBookingRequest);
        }

        // GET: CarBookingRequests/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CarBookingRequest carBookingRequest = db.CarBookingRequests.Find(id);
            if (carBookingRequest == null)
            {
                return HttpNotFound();
            }
            return View(carBookingRequest);
        }

        // POST: CarBookingRequests/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,DtEncoded,DtBooking,Name,Mobile,Email,Unit,Destinations,Duration")] CarBookingRequest carBookingRequest)
        {
            if (ModelState.IsValid)
            {
                db.Entry(carBookingRequest).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(carBookingRequest);
        }

        // GET: CarBookingRequests/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CarBookingRequest carBookingRequest = db.CarBookingRequests.Find(id);
            if (carBookingRequest == null)
            {
                return HttpNotFound();
            }
            return View(carBookingRequest);
        }

        // POST: CarBookingRequests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CarBookingRequest carBookingRequest = db.CarBookingRequests.Find(id);
            db.CarBookingRequests.Remove(carBookingRequest);
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
