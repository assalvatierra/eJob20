using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using JobsV1.Areas.AutoCare.Data;
using JobsV1.Models;
using Microsoft.Ajax.Utilities;

namespace JobsV1.Areas.AutoCare.Controllers
{
    public class AppointmentSlotsController : Controller
    {
        private AppointmentDBContainer db = new AppointmentDBContainer();

        // GET: AutoCare/AppointmentSlots
        public ActionResult Index()
        {
            return View(db.AppointmentSlots.ToList());
        }

        // GET: AutoCare/AppointmentSlots/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AppointmentSlot appointmentSlot = db.AppointmentSlots.Find(id);
            if (appointmentSlot == null)
            {
                return HttpNotFound();
            }
            return View(appointmentSlot);
        }

        // GET: AutoCare/AppointmentSlots/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AutoCare/AppointmentSlots/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Description")] AppointmentSlot appointmentSlot)
        {
            if (ModelState.IsValid && InputValidation(appointmentSlot))
            {
                db.AppointmentSlots.Add(appointmentSlot);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(appointmentSlot);
        }

        // GET: AutoCare/AppointmentSlots/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AppointmentSlot appointmentSlot = db.AppointmentSlots.Find(id);
            if (appointmentSlot == null)
            {
                return HttpNotFound();
            }
            return View(appointmentSlot);
        }

        // POST: AutoCare/AppointmentSlots/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Description")] AppointmentSlot appointmentSlot)
        {
            if (ModelState.IsValid && InputValidation(appointmentSlot))
            {
                db.Entry(appointmentSlot).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(appointmentSlot);
        }

        // GET: AutoCare/AppointmentSlots/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AppointmentSlot appointmentSlot = db.AppointmentSlots.Find(id);
            if (appointmentSlot == null)
            {
                return HttpNotFound();
            }
            return View(appointmentSlot);
        }

        // POST: AutoCare/AppointmentSlots/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AppointmentSlot appointmentSlot = db.AppointmentSlots.Find(id);
            db.AppointmentSlots.Remove(appointmentSlot);
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



        public bool InputValidation(AppointmentSlot appointmentSlot)
        {
            bool isValid = true;

            if (appointmentSlot.Description.IsNullOrWhiteSpace())
            {
                ModelState.AddModelError("Description", "Invalid Description");
                isValid = false;
            }


            return isValid;
        }
    }
}
