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
    public class crLogPassengerAreasController : Controller
    {
        private CarRentalLogDBContainer db = new CarRentalLogDBContainer();

        // GET: Personel/crLogPassengerAreas
        public ActionResult Index()
        {
            return View(db.crLogPassengerAreas.ToList().OrderBy(a=>a.Name));
        }

        // GET: Personel/crLogPassengerAreas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            crLogPassengerArea crLogPassengerArea = db.crLogPassengerAreas.Find(id);
            if (crLogPassengerArea == null)
            {
                return HttpNotFound();
            }
            return View(crLogPassengerArea);
        }

        // GET: Personel/crLogPassengerAreas/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Personel/crLogPassengerAreas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] crLogPassengerArea crLogPassengerArea)
        {
            if (ModelState.IsValid)
            {
                db.crLogPassengerAreas.Add(crLogPassengerArea);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(crLogPassengerArea);
        }

        // GET: Personel/crLogPassengerAreas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            crLogPassengerArea crLogPassengerArea = db.crLogPassengerAreas.Find(id);
            if (crLogPassengerArea == null)
            {
                return HttpNotFound();
            }
            return View(crLogPassengerArea);
        }

        // POST: Personel/crLogPassengerAreas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] crLogPassengerArea crLogPassengerArea)
        {
            if (ModelState.IsValid)
            {
                db.Entry(crLogPassengerArea).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(crLogPassengerArea);
        }

        // GET: Personel/crLogPassengerAreas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            crLogPassengerArea crLogPassengerArea = db.crLogPassengerAreas.Find(id);
            if (crLogPassengerArea == null)
            {
                return HttpNotFound();
            }
            return View(crLogPassengerArea);
        }

        // POST: Personel/crLogPassengerAreas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            crLogPassengerArea crLogPassengerArea = db.crLogPassengerAreas.Find(id);
            db.crLogPassengerAreas.Remove(crLogPassengerArea);
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
