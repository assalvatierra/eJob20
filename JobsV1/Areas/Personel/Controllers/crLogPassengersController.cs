using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ArModels.Models;
using JobsV1.Areas.Personel.Models;
using JobsV1.Models;
using Microsoft.Ajax.Utilities;

namespace JobsV1.Areas.Personel.Controllers
{
    public class crLogPassengersController : Controller
    {
        private CarRentalLogDBContainer db = new CarRentalLogDBContainer();
        private DateClass dt = new DateClass();
        private CrDataLayer dl = new CrDataLayer();

        // GET: Personel/crLogPassengers
        public ActionResult Index()
        {
            var crLogPassengers = db.crLogPassengers.Include(c => c.crLogPassStatu).Include(c => c.crLogTrip);
            return View(crLogPassengers.ToList());
        }

        // GET: Personel/crLogPassengers
        public ActionResult TripPassengers(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.tripId = (int)id;
            ViewBag.TripDetails = db.crLogTrips.Find(id);
            var crLogPassengers = db.crLogPassengers.Where(c=>c.crLogTripId == id).Include(c => c.crLogPassStatu).Include(c => c.crLogTrip);
            return View(crLogPassengers.ToList());
        }


        // GET: Personel/crLogPassengers
        public ActionResult DriversTripPassengers(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var today = dt.GetCurrentDate();
            var tripToday = db.crLogTrips.Where(c => c.crLogDriverId == id && c.DtTrip == today).FirstOrDefault();
            if (tripToday == null)
            {
                return View(new List<crLogPassenger>());
            }

            var crLogPassengers = db.crLogPassengers.Where(c => c.crLogTripId == tripToday.Id);
            return View(crLogPassengers.ToList());
        }

        // GET: Personel/crLogPassengers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            crLogPassenger crLogPassenger = db.crLogPassengers.Find(id);
            if (crLogPassenger == null)
            {
                return HttpNotFound();
            }
            return View(crLogPassenger);
        }


        // GET: Personel/crLogPassengers/Create
        public ActionResult Create()
        {
            crLogPassenger passenger = new crLogPassenger();
            
            ViewBag.crLogPassStatusId = new SelectList(db.crLogPassStatus, "Id", "Status");
            ViewBag.crLogTripId = new SelectList(db.crLogTrips, "Id", "DtTrip");
            return View();
        }

        // POST: Personel/crLogPassengers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Contact,PassAddress,PickupPoint,PickupTime,DropPoint,DropTime,timeContacted,timeBoarded,timeDelivered,Remarks,crLogPassStatusId,crLogTripId")] crLogPassenger crLogPassenger)
        {
            if (ModelState.IsValid)
            {
                db.crLogPassengers.Add(crLogPassenger);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.crLogPassStatusId = new SelectList(db.crLogPassStatus, "Id", "Status", crLogPassenger.crLogPassStatusId);
            ViewBag.crLogTripId = new SelectList(db.crLogTrips, "Id", "Remarks", crLogPassenger.crLogTripId);
            return View(crLogPassenger);
        }


        // GET: Personel/crLogPassengers/Create
        public ActionResult CreatePassTrip(int id)
        {
            crLogPassenger passenger = new crLogPassenger();
            passenger.timeContacted = " ";
            passenger.timeBoarded = " ";
            passenger.timeDelivered = " ";

            ViewBag.tripId = id;
            ViewBag.crLogPassStatusId = new SelectList(db.crLogPassStatus, "Id", "Status");
            ViewBag.crLogTripId = new SelectList(db.crLogTrips, "Id", "DtTrip", id);
            return View(passenger);
        }

        // POST: Personel/crLogPassengers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreatePassTrip([Bind(Include = "Id,Name,Contact,PassAddress,PickupPoint,PickupTime,DropPoint,DropTime,timeContacted,timeBoarded,timeDelivered,Remarks,crLogPassStatusId,crLogTripId")] crLogPassenger crLogPassenger)
        {
            if (ModelState.IsValid && CreatePassValidation(crLogPassenger))
            {
                db.crLogPassengers.Add(crLogPassenger);
                db.SaveChanges();
                return RedirectToAction("TripPassengers" , new { id = crLogPassenger.crLogTripId });
            }

            ViewBag.crLogPassStatusId = new SelectList(db.crLogPassStatus, "Id", "Status", crLogPassenger.crLogPassStatusId);
            ViewBag.crLogTripId = new SelectList(db.crLogTrips, "Id", "Remarks", crLogPassenger.crLogTripId);
            return View(crLogPassenger);
        }


        // GET: Personel/crLogPassengers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            crLogPassenger crLogPassenger = db.crLogPassengers.Find(id);
            if (crLogPassenger == null)
            {
                return HttpNotFound();
            }
            ViewBag.crLogPassStatusId = new SelectList(db.crLogPassStatus, "Id", "Status", crLogPassenger.crLogPassStatusId);
            ViewBag.crLogTripId = new SelectList(db.crLogTrips, "Id", "Remarks", crLogPassenger.crLogTripId);
            return View(crLogPassenger);
        }

        // POST: Personel/crLogPassengers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Contact,PassAddress,PickupPoint,PickupTime,DropPoint,DropTime,timeContacted,timeBoarded,timeDelivered,Remarks,crLogPassStatusId,crLogTripId")] crLogPassenger crLogPassenger)
        {
            if (ModelState.IsValid && CreatePassValidation(crLogPassenger))
            {
                db.Entry(crLogPassenger).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.crLogPassStatusId = new SelectList(db.crLogPassStatus, "Id", "Status", crLogPassenger.crLogPassStatusId);
            ViewBag.crLogTripId = new SelectList(db.crLogTrips, "Id", "Remarks", crLogPassenger.crLogTripId);
            return View(crLogPassenger);
        }

        // GET: Personel/crLogPassengers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            crLogPassenger crLogPassenger = db.crLogPassengers.Find(id);
            if (crLogPassenger == null)
            {
                return HttpNotFound();
            }
            return View(crLogPassenger);
        }

        // POST: Personel/crLogPassengers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            crLogPassenger crLogPassenger = db.crLogPassengers.Find(id);
            db.crLogPassengers.Remove(crLogPassenger);
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


        public bool CreatePassValidation(crLogPassenger passenger)
        {
            bool isValid = true;

            if (passenger.Name.IsNullOrWhiteSpace())
            {
                ModelState.AddModelError("Name", "Invalid Name");
                isValid = false;
            }

            if (passenger.Contact.IsNullOrWhiteSpace())
            {
                ModelState.AddModelError("Contact", "Invalid Contact");
                isValid = false;
            }

            if (passenger.PickupPoint.IsNullOrWhiteSpace())
            {
                ModelState.AddModelError("PickupPoint", "Invalid PickupPoint");
                isValid = false;
            }

            if (passenger.PickupTime.IsNullOrWhiteSpace())
            {
                ModelState.AddModelError("PickupTime", "Invalid PickupTime");
                isValid = false;
            }

            if (passenger.DropPoint.IsNullOrWhiteSpace())
            {
                ModelState.AddModelError("DropPoint", "Invalid DropPoint");
                isValid = false;
            }

            if (passenger.DropTime.IsNullOrWhiteSpace())
            {
                ModelState.AddModelError("DropTime", "Invalid DropTime");
                isValid = false;
            }


            return isValid;
        }



        public ActionResult CopyPassTrip()
        {
            List<SelectListItem> tripLogList = new List<SelectListItem> {
                new SelectListItem { Value = "1", Text = "NA" },
            };

            var today = dt.GetCurrentDate();
            var scrLogTrips = db.crLogTrips.Include(c => c.crLogDriver).Include(c => c.crLogUnit).Include(c => c.crLogCompany).Include(c => c.crLogClosing);
            scrLogTrips = scrLogTrips.Where(c => c.DtTrip >= today);

            var sfilteredLogs = new List<crLogTrip>();
            foreach (var logs in scrLogTrips.ToList())
            {
                if (GetTripPassengersCount(logs.Id) == 0)
                {
                    tripLogList.Add(new SelectListItem
                    {
                        Value = "1",
                        Text = logs.DtTrip.ToShortDateString() + " - " + logs.crLogDriver.Name + " / " + logs.crLogUnit.Description + " / " + logs.crLogCompany
                    });
                }
            }


            ViewBag.isSubmitted = false;
            ViewBag.companyId = new SelectList(dl.GetCompanies(), "Id", "Name");
            ViewBag.tripId = new SelectList(tripLogList, "Value", "Text");
            return View(new List<crLogTrip>());
        }

        [HttpPost]
        public ActionResult CopyPassTrip(string srchDate, int? companyId)
        {
            try
            {
                if (!srchDate.IsNullOrWhiteSpace())
                {
                    var tempDate = DateTime.Parse(srchDate);

                    var crLogTrips = db.crLogTrips.Include(c => c.crLogDriver).Include(c => c.crLogUnit).Include(c => c.crLogCompany).Include(c => c.crLogClosing);

                    crLogTrips = crLogTrips.Where(c => c.DtTrip.Day == tempDate.Day &&
                                                        c.DtTrip.Month == tempDate.Month &&
                                                        c.DtTrip.Year == tempDate.Year);

                    if (companyId != null)
                    {
                        crLogTrips = crLogTrips.Where(c => c.crLogCompanyId == companyId);
                    }

                    ViewBag.companyId = new SelectList(dl.GetCompanies(), "Id", "Name");
                    ViewBag.isSubmitted = true;

                    var filteredLogs = new List<crLogTrip>();
                    foreach (var logs in crLogTrips.ToList())
                    {
                        if (GetTripPassengersCount(logs.Id) > 0)
                        {
                            filteredLogs.Add(logs);
                        }

                    }


                    return View(filteredLogs);
                }

                List<SelectListItem> tripLogList = new List<SelectListItem> {
                        new SelectListItem { Value = "1", Text = "NA" },
                    };

                var today = dt.GetCurrentDate();
                var scrLogTrips = db.crLogTrips.Where(c => c.DtTrip >= today);

                if (companyId != null)
                {
                    scrLogTrips = scrLogTrips.Where(c => c.crLogCompanyId == companyId);
                }

                foreach (var logs in scrLogTrips.ToList())
                {
                    //if (GetTripPassengersCount(logs.Id) == 0)
                    //{
                        tripLogList.Add(new SelectListItem { Value = logs.Id.ToString(), 
                            Text = logs.DtTrip.ToShortDateString() + " - " + logs.crLogDriver.Name + " / " + logs.crLogUnit.Description + " / " + logs.crLogCompany });
                    //}
                }
                if (tripLogList == null)
                {
                    tripLogList = new List<SelectListItem> {
                        new SelectListItem { Value = "0", Text = "NA" },
                    };
                }


                ViewBag.companyId = new SelectList(dl.GetCompanies(), "Id", "Name");
                ViewBag.tripId = new SelectList(tripLogList, "Value", "Text");
                ViewBag.isSubmitted = false;
                return View(new List<crLogTrip>());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //CopyPassTripSubmit

        public bool CopyPassTripSubmit(int? id, int? destTripId)
        {
            try
            {
                if (destTripId == null || id == null)
                {
                    return false;
                }

                //find logs by id
                var cLogTrip = db.crLogTrips.Find(id);

                if (cLogTrip == null)
                {
                    return false;
                }

                //get list of passengers from log trip id
                var passengersList = db.crLogPassengers.Where(p => p.crLogTripId == id).ToList();
                foreach (var pass in passengersList)
                {
                    crLogPassenger copy_pass = pass;
                    copy_pass.timeContacted = " ";
                    copy_pass.timeBoarded = " ";
                    copy_pass.timeDelivered = " ";
                    copy_pass.Remarks = " ";
                    copy_pass.crLogPassStatusId = 1;     //new status
                    copy_pass.crLogTripId = (int)destTripId;  //new trip log

                    db.crLogPassengers.Add(copy_pass);
                }
                db.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }


        public int GetTripPassengersCount(int id)
        {
            try
            {

                var crLogPassengers = db.crLogPassengers.Where(c => c.crLogTripId == id);

                return crLogPassengers.Count();

            }
            catch
            {
                return 0;
            }
        }

    }
}
