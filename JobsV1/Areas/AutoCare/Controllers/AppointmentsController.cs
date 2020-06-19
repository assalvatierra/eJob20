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
    public class AppointmentsController : Controller
    {
        private AppointmentDBContainer db = new AppointmentDBContainer();

        // GET: AutoCare/Appointments
        public ActionResult Index()
        {
            var appointments = db.Appointments.Include(a => a.AppointmentSlot).Include(a => a.AppointmentStatu);
            return View(appointments.ToList());
        }

        // GET: AutoCare/Appointments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Appointment appointment = db.Appointments.Find(id);
            if (appointment == null)
            {
                return HttpNotFound();
            }
            return View(appointment);
        }

        // GET: AutoCare/Appointments/Create
        public ActionResult Create()
        {
            ViewBag.AppointmentSlotId = new SelectList(db.AppointmentSlots, "Id", "Description");
            ViewBag.AppointmentStatusId = new SelectList(db.AppointmentStatus, "Id", "Status");
            return View();
        }

        // POST: AutoCare/Appointments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,DtEntered,Customer,Contact,CustCode,Plate,Conduction,Request,Remarks,AppointmentStatusId,AppointmentSlotId,AppointmentDate")] Appointment appointment)
        {
            if (ModelState.IsValid && InputValidation(appointment))
            {
                db.Appointments.Add(appointment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AppointmentSlotId = new SelectList(db.AppointmentSlots, "Id", "Description", appointment.AppointmentSlotId);
            ViewBag.AppointmentStatusId = new SelectList(db.AppointmentStatus, "Id", "Status", appointment.AppointmentStatusId);
            return View(appointment);
        }

        // GET: AutoCare/Appointments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Appointment appointment = db.Appointments.Find(id);
            if (appointment == null)
            {
                return HttpNotFound();
            }
            ViewBag.AppointmentSlotId = new SelectList(db.AppointmentSlots, "Id", "Description", appointment.AppointmentSlotId);
            ViewBag.AppointmentStatusId = new SelectList(db.AppointmentStatus, "Id", "Status", appointment.AppointmentStatusId);
            return View(appointment);
        }

        // POST: AutoCare/Appointments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,DtEntered,Customer,Contact,CustCode,Plate,Conduction,Request,Remarks,AppointmentStatusId,AppointmentSlotId,AppointmentDate")] Appointment appointment)
        {
            if (ModelState.IsValid && InputValidation(appointment))
            {
                db.Entry(appointment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AppointmentSlotId = new SelectList(db.AppointmentSlots, "Id", "Description", appointment.AppointmentSlotId);
            ViewBag.AppointmentStatusId = new SelectList(db.AppointmentStatus, "Id", "Status", appointment.AppointmentStatusId);
            return View(appointment);
        }

        // GET: AutoCare/Appointments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Appointment appointment = db.Appointments.Find(id);
            if (appointment == null)
            {
                return HttpNotFound();
            }
            return View(appointment);
        }

        // POST: AutoCare/Appointments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Appointment appointment = db.Appointments.Find(id);
            db.Appointments.Remove(appointment);
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



        public bool InputValidation(Appointment appointment)
        {
            bool isValid = true;

            if (appointment.Customer.IsNullOrWhiteSpace())
            {
                ModelState.AddModelError("Customer", "Invalid Customer");
                isValid = false;
            }

            if (appointment.Contact.IsNullOrWhiteSpace())
            {
                ModelState.AddModelError("Contact", "Invalid Contact");
                isValid = false;
            }

            if (appointment.Plate.IsNullOrWhiteSpace())
            {
                ModelState.AddModelError("Plate", "Invalid Plate");
                isValid = false;
            }

            if (appointment.CustCode.IsNullOrWhiteSpace())
            {
                ModelState.AddModelError("CustCode", "Invalid CustCode");
                isValid = false;
            }

            if (appointment.Request.IsNullOrWhiteSpace())
            {
                ModelState.AddModelError("Request", "Invalid Request");
                isValid = false;
            }

            if (appointment.Remarks.IsNullOrWhiteSpace())
            {
                ModelState.AddModelError("Remarks", "Invalid Remarks");
                isValid = false;
            }


            return isValid;
        }
    }
}
