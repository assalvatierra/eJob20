using JobsV1.Areas.AutoCare.Data;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JobsV1.Models.Class;
using System.Net;
using JobsV1.Models;

namespace JobsV1.Controllers
{
    public class PublicController : Controller
    {
        private AppointmentDBContainer db = new AppointmentDBContainer();
        private AppointmentClass apClass = new AppointmentClass();
        private SysAccessLayer dal = new SysAccessLayer();
        private DateClass dt = new DateClass();

        // GET: Public
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Appointment(int? SlotId, string Date)
        {
            var slotDesc = "";
            var DateString = "";

            Appointment appointment = new Appointment();

            if (SlotId != null  && !Date.IsNullOrWhiteSpace())
            {
                appointment.AppointmentSlotId = (int)SlotId;
                appointment.AppointmentDate = Date;

                slotDesc = db.AppointmentSlots.Find((int)SlotId).Description.ToString();
                DateString = Date;

            }

            ViewBag.Slot = slotDesc;
            ViewBag.SlotId = SlotId ?? 0;
            ViewBag.DateString = DateString;

            ViewBag.AppointmentSlotId = new SelectList(db.AppointmentSlots, "Id", "Description", SlotId);
            ViewBag.AppointmentStatusId = new SelectList(db.AppointmentStatus, "Id", "Status", 1);
            ViewBag.AppointmentRequestId = new SelectList(db.AppointmentRequests.OrderBy(a => a.OrderNo), "Id", "Description");
            ViewBag.Schedules = apClass.GetAppoinmentSchedules();
            ViewBag.IsNotValid = false;
            ViewBag.CompanyLogo = dal.getSysSetting("ICON");

            return View(appointment);
        }


        // POST: AutoCare/Appointments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Appointment([Bind(Include = "Id,DtEntered,Customer,Contact,CustCode,Plate,Conduction,Request,Remarks,AppointmentStatusId,AppointmentSlotId,AppointmentDate,AppointmentRequestId,Unit")] Appointment appointment)
        {
            try
            {

                var slotDesc = "";
                var DateString = "";
                if (ModelState.IsValid && AppointmentValidation(appointment))
                {
                    appointment.DtEntered = dt.GetCurrentDateTime();

                    appointment.AppointmentAcctTypeId = 1;
                    db.Appointments.Add(appointment);
                    db.SaveChanges();
                    return RedirectToAction("AppointmentSuccess");
                }


                if (!appointment.AppointmentDate.IsNullOrWhiteSpace())
                {
                    slotDesc = db.AppointmentSlots.Find(appointment.AppointmentSlotId).Description.ToString();
                    DateString = appointment.AppointmentDate;
                }

                ViewBag.Slot = slotDesc;
                ViewBag.SlotId = appointment.AppointmentSlotId;
                ViewBag.DateString = DateString;

                ViewBag.AppointmentSlotId = new SelectList(db.AppointmentSlots, "Id", "Description", appointment.AppointmentSlotId);
                ViewBag.AppointmentStatusId = new SelectList(db.AppointmentStatus, "Id", "Status", appointment.AppointmentStatusId);
                ViewBag.AppointmentRequestId = new SelectList(db.AppointmentRequests.OrderBy(a => a.OrderNo), "Id", "Description", appointment.AppointmentRequestId);
                ViewBag.Schedules = apClass.GetAppoinmentSchedules();
                ViewBag.IsNotValid = true;
                ViewBag.CompanyLogo = dal.getSysSetting("ICON");
                return View(appointment);

            }
            catch
            {
               
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }


        public ActionResult AppointmentSuccess()
        {
            ViewBag.CompanyLogo = dal.getSysSetting("ICON");
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
                ModelState.AddModelError("Contact", "Invalid Mobile");
                isValid = false;
            }

            if (appointment.Unit.IsNullOrWhiteSpace())
            {
                ModelState.AddModelError("Unit", "Invalid Unit");
                isValid = false;
            }

            if (appointment.Plate.IsNullOrWhiteSpace())
            {
                ModelState.AddModelError("Plate", "Invalid Plate");
                isValid = false;
            }


            return isValid;
        }

    }
}