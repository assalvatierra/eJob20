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
    public class crLogPassengerMastersController : Controller
    {
        private CarRentalLogDBContainer db = new CarRentalLogDBContainer();

        // GET: Personel/crLogPassengerMasters
        public ActionResult Index()
        {
            return View(db.crLogPassengerMasters.ToList());
        }

        // GET: Personel/crLogPassengerMasters/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            crLogPassengerMaster crLogPassengerMaster = db.crLogPassengerMasters.Find(id);
            if (crLogPassengerMaster == null)
            {
                return HttpNotFound();
            }
            return View(crLogPassengerMaster);
        }

        // GET: Personel/crLogPassengerMasters/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Personel/crLogPassengerMasters/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Contact,PassAddress,PickupPoint,PickupTime,DropPoint,DropTime,Remarks")] crLogPassengerMaster crLogPassengerMaster)
        {
            if (ModelState.IsValid)
            {
                db.crLogPassengerMasters.Add(crLogPassengerMaster);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(crLogPassengerMaster);
        }

        // GET: Personel/crLogPassengerMasters/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            crLogPassengerMaster crLogPassengerMaster = db.crLogPassengerMasters.Find(id);
            if (crLogPassengerMaster == null)
            {
                return HttpNotFound();
            }
            return View(crLogPassengerMaster);
        }

        // POST: Personel/crLogPassengerMasters/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Contact,PassAddress,PickupPoint,PickupTime,DropPoint,DropTime,Remarks")] crLogPassengerMaster crLogPassengerMaster)
        {
            if (ModelState.IsValid)
            {
                db.Entry(crLogPassengerMaster).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(crLogPassengerMaster);
        }

        // GET: Personel/crLogPassengerMasters/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            crLogPassengerMaster crLogPassengerMaster = db.crLogPassengerMasters.Find(id);
            if (crLogPassengerMaster == null)
            {
                return HttpNotFound();
            }
            return View(crLogPassengerMaster);
        }

        // POST: Personel/crLogPassengerMasters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            crLogPassengerMaster crLogPassengerMaster = db.crLogPassengerMasters.Find(id);
            db.crLogPassengerMasters.Remove(crLogPassengerMaster);
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
