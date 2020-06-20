using JobsV1.Areas.AutoCare.Data;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JobsV1.Controllers
{
    public class PublicController : Controller
    {
        private AppointmentDBContainer db = new AppointmentDBContainer();

        // GET: Public
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Appointment()
        {
            ViewBag.AppointmentSlotId = new SelectList(db.AppointmentSlots, "Id", "Description", 1);
            ViewBag.AppointmentStatusId = new SelectList(db.AppointmentStatus, "Id", "Status", 1);
            return View();
        }


        // POST: AutoCare/Appointments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Appointment([Bind(Include = "Id,DtEntered,Customer,Contact,CustCode,Plate,Conduction,Request,Remarks,AppointmentStatusId,AppointmentSlotId,AppointmentDate")] Appointment appointment)
        {
            if (ModelState.IsValid && AppointmentValidation(appointment))
            {
                appointment.AppointmentDate = DateTime.ParseExact(appointment.AppointmentDate, "MMM dd yyyy",
                                                    CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");

                db.Appointments.Add(appointment);
                db.SaveChanges();
                return RedirectToAction("AppointmentSuccess");
            }

            ViewBag.AppointmentSlotId = new SelectList(db.AppointmentSlots, "Id", "Description", appointment.AppointmentSlotId);
            ViewBag.AppointmentStatusId = new SelectList(db.AppointmentStatus, "Id", "Status", appointment.AppointmentStatusId);
            return View(appointment);
        }


        public ActionResult AppointmentSuccess()
        {
            return View();
        }

        public bool AppointmentValidation(Appointment appointment)
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


            if (appointment.Request.IsNullOrWhiteSpace())
            {
                ModelState.AddModelError("Request", "Invalid Request");
                isValid = false;
            }


            return isValid;
        }

    }
}