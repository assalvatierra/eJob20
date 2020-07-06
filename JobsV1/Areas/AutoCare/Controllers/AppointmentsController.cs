using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using JobsV1.Areas.AutoCare.Data;
using JobsV1.Models;
using JobsV1.Models.Class;
using Microsoft.Ajax.Utilities;

namespace JobsV1.Areas.AutoCare.Controllers
{
    public class AppointmentsController : Controller
    {
        private JobDBContainer jdb = new JobDBContainer();
        private AppointmentDBContainer db = new AppointmentDBContainer();
        private AppointmentClass apClass = new AppointmentClass();
        private DateClass date = new DateClass();
        // GET: AutoCare/Appointments
        public ActionResult Index()
        {
            var today = date.GetCurrentDate();
            var appointments = db.Appointments.Include(a => a.AppointmentSlot).Include(a => a.AppointmentStatu)
                .OrderByDescending(a=>a.AppointmentDate).Where(a=>a.AppointmentStatusId < 3);
            return View(appointments.ToList().Where(a => DateTime.Parse(a.AppointmentDate).Date >= today.Date));
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
            ViewBag.CustomerList = jdb.Customers.Where(s => s.Status == "ACT").OrderBy(s => s.Name).ToList() ?? new List<Customer>();
            ViewBag.AppointmentSlotId = new SelectList(db.AppointmentSlots, "Id", "Description");
            ViewBag.AppointmentStatusId = new SelectList(db.AppointmentStatus, "Id", "Status");
            ViewBag.AppointmentRequestId = new SelectList(db.AppointmentRequests.OrderBy(a => a.OrderNo), "Id", "Description");
            ViewBag.AppointmentAcctTypeId = new SelectList(db.AppointmentAcctTypes, "Id", "Description");
            return View();
        }

        // POST: AutoCare/Appointments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,DtEntered,Customer,Contact,CustCode,Plate,Conduction,Request,Remarks,AppointmentStatusId,AppointmentSlotId,AppointmentDate,AppointmentRequestId,Unit,AppointmentAcctTypeId")] Appointment appointment)
        {
            if (ModelState.IsValid && InputValidation(appointment))
            {
                db.Appointments.Add(appointment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CustomerList = jdb.Customers.Where(s => s.Status == "ACT").OrderBy(s => s.Name).ToList() ?? new List<Customer>();
            ViewBag.AppointmentSlotId = new SelectList(db.AppointmentSlots, "Id", "Description", appointment.AppointmentSlotId);
            ViewBag.AppointmentStatusId = new SelectList(db.AppointmentStatus, "Id", "Status", appointment.AppointmentStatusId);
            ViewBag.AppointmentRequestId = new SelectList(db.AppointmentRequests.OrderBy(a => a.OrderNo), "Id", "Description", appointment.AppointmentRequestId);
            ViewBag.AppointmentAcctTypeId = new SelectList(db.AppointmentAcctTypes, "Id", "Description", appointment.AppointmentAcctTypeId);
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
            ViewBag.CustomerList = jdb.Customers.Where(s => s.Status == "ACT").ToList() ?? new List<Customer>();
            ViewBag.AppointmentSlotId = new SelectList(db.AppointmentSlots, "Id", "Description", appointment.AppointmentSlotId);
            ViewBag.AppointmentStatusId = new SelectList(db.AppointmentStatus, "Id", "Status", appointment.AppointmentStatusId);
            ViewBag.AppointmentRequestId = new SelectList(db.AppointmentRequests.OrderBy(a => a.OrderNo), "Id", "Description", appointment.AppointmentRequestId);
            ViewBag.AppointmentAcctTypeId = new SelectList(db.AppointmentAcctTypes, "Id", "Description", appointment.AppointmentAcctTypeId);
            return View(appointment);
        }

        // POST: AutoCare/Appointments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,DtEntered,Customer,Contact,CustCode,Plate,Conduction,Request,Remarks,AppointmentStatusId,AppointmentSlotId,AppointmentDate,AppointmentRequestId,Unit,AppointmentAcctTypeId")] Appointment appointment)
        {
            if (ModelState.IsValid && InputValidation(appointment))
            {
                db.Entry(appointment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CustomerList = jdb.Customers.Where(s => s.Status == "ACT").ToList() ?? new List<Customer>();
            ViewBag.AppointmentSlotId = new SelectList(db.AppointmentSlots, "Id", "Description", appointment.AppointmentSlotId);
            ViewBag.AppointmentStatusId = new SelectList(db.AppointmentStatus, "Id", "Status", appointment.AppointmentStatusId);
            ViewBag.AppointmentRequestId = new SelectList(db.AppointmentRequests.OrderBy(a => a.OrderNo), "Id", "Description", appointment.AppointmentRequestId);
            ViewBag.AppointmentAcctTypeId = new SelectList(db.AppointmentAcctTypes, "Id", "Description", appointment.AppointmentAcctTypeId);
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



            return isValid;
        }


        public ActionResult PublicAppointment()
        {
            ViewBag.AppointmentSlotId = new SelectList(db.AppointmentSlots, "Id", "Description",1);
            ViewBag.AppointmentStatusId = new SelectList(db.AppointmentStatus, "Id", "Status",1);
            return View();
        }

        public ActionResult AppointmentSelect()
        {
            var apSchedules = apClass.GetAppoinmentSchedules();
            return View(apSchedules);
        }

        public ActionResult Availability()
        {
            var apSchedules = apClass.GetAppoinmentSchedules();
            return View(apSchedules);
        }

        [HttpGet]
        public JsonResult GetAppointments(int id, string date)
        {
            var appointmentList = db.Appointments.ToList().Select(s => new {s.Id, s.Customer, s.CustCode, s.Unit, s.Plate, s.AppointmentRequest.Description, s.Request, s.Remarks, s.AppointmentSlotId , s.AppointmentDate });
            appointmentList = appointmentList.Where(a => a.AppointmentSlotId == id && DateTime.Parse(a.AppointmentDate).Date == DateTime.Parse(date).Date).ToList();

            var apptDetails = new List<cAppointmentDetails>();
            foreach (var appt in appointmentList)
            {
                var tempDetails = new cAppointmentDetails();

                tempDetails.Date = appt.AppointmentDate;
                tempDetails.Id = appt.Id;
                tempDetails.Customer = appt.Customer;
                tempDetails.Unit = appt.Unit;
                tempDetails.Plate = appt.Plate;
                tempDetails.Description = appt.Description;
                tempDetails.Request = appt.Request;
                tempDetails.Remarks = appt.Remarks;
                tempDetails.SlotId = appt.AppointmentSlotId;
                tempDetails.Company = GetCustomerCompany(appt.CustCode);

                apptDetails.Add(tempDetails);
            }

            return Json(apptDetails, JsonRequestBehavior.AllowGet);
        }


        private string GetCustomerCompany(string customerCode)
        {

            int customerId = 0;
            if (Int32.TryParse(customerCode, out customerId))
            {
                var custEntity = jdb.CustEntities.Where(s => s.CustomerId == customerId).FirstOrDefault();
                if (custEntity != null)
                {
                    return custEntity.CustEntMain.Name;
                }
            }

            return null;
        }
    }

    public class cAppointmentDetails
    {
        public int Id { get; set; }
        public string Customer { get; set; }
        public string Company { get; set; }
        public string Unit { get; set; }
        public string Plate { get; set; }
        public string Request { get; set; }
        public string Description { get; set; }
        public string Remarks { get; set; }
        public int SlotId { get; set; }
        public string Date { get; set; }
    }
}

