using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using ArModels.Models;
using JobsV1.Areas.Personel.Models;
using JobsV1.Models;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;

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
        public ActionResult TripPassengers(int? id, string sortBy)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //get list of pasengers per trip
            var crLogPassengers = db.crLogPassengers.Where(c=>c.crLogTripId == id)
                .Include(c => c.crLogPassStatu).Include(c => c.crLogTrip);

            //sort by pickup time
            var sorted_Passengers = crLogPassengers.ToList();

            if(!String.IsNullOrEmpty(sortBy))
            {
                if (sortBy == "Area")
                {
                    sorted_Passengers = sorted_Passengers
                         .OrderBy(c => c.Area)
                         .ThenBy(c => c.NextDay)
                         .ThenBy(c => DateTime.Parse(c.PickupTime).TimeOfDay)
                         .ToList();
                }else if (sortBy == "PickupTime")
                {
                    sorted_Passengers = sorted_Passengers
                         .OrderBy(c => c.NextDay)
                         .ThenBy(c => DateTime.Parse(c.PickupTime).TimeOfDay)
                         .ThenBy(c => c.Area)
                         .ToList();
                }
                else
                {
                    //default
                    sorted_Passengers = sorted_Passengers
                         .OrderBy(c => c.NextDay)
                         .ThenBy(c => DateTime.Parse(c.PickupTime).TimeOfDay)
                         .ThenBy(c => c.Area)
                         .ToList();
                }
            }
            else
            {
                //default
                sorted_Passengers = sorted_Passengers
                        .OrderBy(c => c.NextDay)
                        .ThenBy(c => DateTime.Parse(c.PickupTime).TimeOfDay)
                        .ThenBy(c => c.Area)
                        .ToList();
            }

            ViewBag.SortBy = sortBy;
            ViewBag.DateTimeNow = dt.GetCurrentDateTime();
            ViewBag.tripId = (int)id;
            ViewBag.TripDetails = db.crLogTrips.Find(id);
            ViewBag.tripList = GetPrevTripLogs_withPass_PrevDay() ?? new List<crLogTrip>();
            ViewBag.passMasters = GetPassengersNotInTrip((int)id);

            return View(sorted_Passengers);
        }

        // GET: Personel/DriversTripPassengers/{driversId}
        public ActionResult DriversTripList(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var today = dt.GetCurrentDate().AddDays(-7);

            var driverTrips = db.crLogTrips.Where(c=>c.crLogDriverId == id &&
                 DbFunctions.TruncateTime(c.DtTrip) >= today )
                .OrderByDescending(c=>c.DtTrip).ToList();

            if (driverTrips == null)
            {
                ViewBag.Driver = db.crLogDrivers.Find(id).Name;
                return View(new List<crLogTrip>());
            }

            ViewBag.Driver = db.crLogDrivers.Find(id).Name;
            return View(driverTrips);
        }


        // GET: Personel/DriversTripPassengers/{tripId}
        public ActionResult DriversTripPassengers(int? id, string sortBy)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var today = dt.GetCurrentDate();

            var tripToday = db.crLogTrips.Find(id);
            if (tripToday == null)
            {
                ViewBag.TripId = 0;
                ViewBag.Driver = tripToday.crLogDriver.Name;
                return View(new List<crLogPassenger>());
            }

            var crLogPassengers = db.crLogPassengers.Where(c => c.crLogTripId == tripToday.Id);
            var tripDetails = "";
            if (tripToday != null)
            {
                tripDetails = tripToday.DtTrip.ToString("MMM dd yyyy") + " - " + tripToday.crLogCompany.Name;
            }
            else
            {

                tripDetails = "No trip found";
            }

            //sort by time
            var sorted_Passengers = new List<crLogPassenger>();

            if (!String.IsNullOrEmpty(sortBy))
            {
                if (sortBy == "Area")
                {
                    sorted_Passengers = crLogPassengers.ToList()
                         .OrderBy(c => c.Area)
                         .ThenBy(c => c.NextDay)
                         .ThenBy(c => DateTime.Parse(c.PickupTime).TimeOfDay)
                         .ToList();
                }
                else if (sortBy == "PickupTime")
                {
                    sorted_Passengers = crLogPassengers.ToList()
                         .OrderBy(c => c.NextDay)
                         .ThenBy(c => DateTime.Parse(c.PickupTime).TimeOfDay)
                         .ThenBy(c => c.Area)
                         .ToList();
                }
                else
                {
                    //default
                    sorted_Passengers = crLogPassengers.ToList()
                         .OrderBy(c => c.NextDay)
                         .ThenBy(c => DateTime.Parse(c.PickupTime).TimeOfDay)
                         .ThenBy(c => c.Area)
                         .ToList();
                }
            }
            else
            {
                //default
                sorted_Passengers = crLogPassengers.ToList()
                         .OrderBy(c => c.NextDay)
                         .ThenBy(c => DateTime.Parse(c.PickupTime).TimeOfDay)
                         .ThenBy(c => c.Area)
                         .ToList();
            }

            ViewBag.DateTimeNow = dt.GetCurrentDateTime();
            ViewBag.TripDetails = tripDetails;
            ViewBag.TripId = tripToday.Id;
            ViewBag.Driver = tripToday.crLogDriver.Name;
            ViewBag.DriversId = tripToday.crLogDriverId;
            ViewBag.UnitDetails = tripToday.crLogUnit.Description;
            return View(sorted_Passengers);
        }

        [HttpGet]
        public string GetTripPassList(int id)
        {
            var passengers = db.crLogPassengers.Where(c => c.crLogTripId == id).Select(c => new { c.Id, c.Name, c.Contact, c.PassAddress, c.PickupPoint, c.PickupTime, c.crLogPassStatu.Status, c.crLogPassStatusId }).ToList();

            //return Json(passengers, JsonRequestBehavior.AllowGet);
            return JsonConvert.SerializeObject(passengers, Formatting.Indented);
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
            passenger.PickupTime = "7:30 PM";
            passenger.DropTime = "9:00 PM";

            ViewBag.tripId = id;
            ViewBag.crLogPassStatusId = new SelectList(db.crLogPassStatus, "Id", "Status");
            ViewBag.crLogTripId = new SelectList(db.crLogTrips, "Id", "DtTrip", id);
            ViewBag.Area = new SelectList(db.crLogPassengerAreas.OrderBy(a => a.Name), "Name", "Name");
            ViewBag.PassengerList = GetPassengersNotInTrip(id);
            return View(passenger);
        }

        // POST: Personel/crLogPassengers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreatePassTrip([Bind(Include = "Id,Name,Contact,PassAddress,PickupPoint,PickupTime,DropPoint,DropTime,timeContacted,timeBoarded,timeDelivered,Remarks,crLogPassStatusId,crLogTripId,Area,NextDay")] crLogPassenger crLogPassenger)
        {
            if (ModelState.IsValid && CreatePassValidation(crLogPassenger))
            {
                db.crLogPassengers.Add(crLogPassenger);
                db.SaveChanges();

                //check passenger
                if (CheckPassengerNameOnMaster(crLogPassenger))
                {
                    SavePassengerToMasterList(crLogPassenger);
                }
                return RedirectToAction("TripPassengers" , new { id = crLogPassenger.crLogTripId });
            }

            ViewBag.tripId = crLogPassenger.crLogTripId;
            ViewBag.crLogPassStatusId = new SelectList(db.crLogPassStatus, "Id", "Status", crLogPassenger.crLogPassStatusId);
            ViewBag.crLogTripId = new SelectList(db.crLogTrips, "Id", "Remarks", crLogPassenger.crLogTripId);
            ViewBag.Area = new SelectList(db.crLogPassengerAreas.OrderBy(a => a.Name), "Name", "Name", crLogPassenger.Area);
            ViewBag.PassengerList = GetPassengersNotInTrip(crLogPassenger.crLogTripId);
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
        public ActionResult Edit([Bind(Include = "Id,Name,Contact,PassAddress,PickupPoint,PickupTime,DropPoint,DropTime,timeContacted,timeBoarded,timeDelivered,Remarks,crLogPassStatusId,crLogTripId,Area")] crLogPassenger crLogPassenger)
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

        // GET: Personel/crLogPassengers/EditPasstrip/5
        public ActionResult EditPasstrip(int? id)
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
            ViewBag.tripId = crLogPassenger.crLogTripId;
            ViewBag.crLogPassStatusId = new SelectList(db.crLogPassStatus, "Id", "Status", crLogPassenger.crLogPassStatusId);
            ViewBag.crLogTripId = new SelectList(db.crLogTrips, "Id", "Remarks", crLogPassenger.crLogTripId);
            ViewBag.PassengerList = db.crLogPassengerMasters.ToList();
            ViewBag.Area = new SelectList(db.crLogPassengerAreas.OrderBy(a => a.Name), "Name", "Name", crLogPassenger.Area);
            return View(crLogPassenger);
        }

        // POST: Personel/crLogPassengers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPasstrip([Bind(Include = "Id,Name,Contact,PassAddress,PickupPoint,PickupTime,DropPoint,DropTime,timeContacted,timeBoarded,timeDelivered,Remarks,crLogPassStatusId,crLogTripId,Area,NextDay")] crLogPassenger crLogPassenger)
        {
            if (ModelState.IsValid && CreatePassValidation(crLogPassenger))
            {
                db.Entry(crLogPassenger).State = EntityState.Modified;
                db.SaveChanges();

                //edit master passenger with same name
                UpdatePassengerMaster(crLogPassenger);

                return RedirectToAction("TripPassengers", new { id = crLogPassenger.crLogTripId });
            }
            ViewBag.tripId = crLogPassenger.crLogTripId;
            ViewBag.crLogPassStatusId = new SelectList(db.crLogPassStatus, "Id", "Status", crLogPassenger.crLogPassStatusId);
            ViewBag.crLogTripId = new SelectList(db.crLogTrips, "Id", "Remarks", crLogPassenger.crLogTripId);
            ViewBag.PassengerList = db.crLogPassengerMasters.ToList();
            ViewBag.Area = new SelectList(db.crLogPassengerAreas.OrderBy(a => a.Name), "Name", "Name", crLogPassenger.Area);
            return View(crLogPassenger);
        }

        public bool UpdatePassengerMaster(crLogPassenger crLogPassenger)
        {
            try
            {
                var passengerMaster = db.crLogPassengerMasters.Where(c => c.Name == crLogPassenger.Name);
                if (passengerMaster.FirstOrDefault() != null)
                {
                    var passDetails = passengerMaster.FirstOrDefault();
                    passDetails.PickupPoint = crLogPassenger.PickupPoint;
                    passDetails.PickupTime = crLogPassenger.PickupTime;
                    passDetails.DropPoint = crLogPassenger.DropPoint;
                    passDetails.DropTime = crLogPassenger.DropTime;

                    db.Entry(passDetails).State = EntityState.Modified;
                    db.SaveChanges();
                }

                return true;

            } catch
            {
                return false;
            }
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



        // GET: Personel/crLogPassengers/Delete/5
        public ActionResult DeletePassTrip(int? id)
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
        [HttpPost, ActionName("DeletePassTrip")]
        [ValidateAntiForgeryToken]
        public ActionResult DeletePassTrip(int id)
        {
            crLogPassenger crLogPassenger = db.crLogPassengers.Find(id);
            db.crLogPassengers.Remove(crLogPassenger);
            db.SaveChanges();
            return RedirectToAction("TripPassengers", new { id = crLogPassenger.crLogTripId });
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

                List<SelectListItem> tripLogList = new List<SelectListItem> {};

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

                    //get valid trips 
                    tripLogList = GetActivePassTripLogs((int)companyId);

                    ViewBag.tripId = new SelectList(tripLogList, "Value", "Text");

                    return View(filteredLogs);
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

        public List<SelectListItem> GetActivePassTripLogs(int companyId)
        {
            try
            {
                List<SelectListItem> tripLogList = new List<SelectListItem> { };
                //Create List of 
                var today = dt.GetCurrentDate();
                var scrLogTrips = db.crLogTrips.Where(c => c.DtTrip >= today);

                if (companyId != 0)
                {
                    scrLogTrips = scrLogTrips.Where(c => c.crLogCompanyId == companyId);
                }

                foreach (var logs in scrLogTrips.ToList())
                {
                    if (GetTripPassengersCount(logs.Id) == 0)
                    {
                        tripLogList.Add(new SelectListItem
                        {
                            Value = logs.Id.ToString(),
                            Text = logs.DtTrip.ToShortDateString() + " - " + logs.crLogDriver.Name + " / " + logs.crLogUnit.Description + " / " + logs.crLogCompany.Name
                        });
                    }
                }

                if (tripLogList == null)
                {
                    tripLogList = new List<SelectListItem> {};
                }

                return tripLogList;
            }
            catch
            {
                return new List<SelectListItem> {};
            }
        }


        public List<crLogTrip> GetPrevTripLogs_withPass()
        {
            try
            {
                //Create List of 
                var today = dt.GetCurrentDate();
                var today_seven_days_before = dt.GetCurrentDate().AddDays(-7);
                var scrLogTrips = db.crLogTrips.Where(c => c.DtTrip >= today_seven_days_before).ToList();
                var tripsWithPass = new List<crLogTrip>();

                foreach (var logs in scrLogTrips)
                {
                   tripsWithPass.Add(logs);
                }

                tripsWithPass = tripsWithPass.OrderByDescending(c => c.DtTrip).ToList();

                return tripsWithPass;
            }
            catch 
            {
                return new List<crLogTrip>();
            }
        }


        public List<crLogTrip> GetPrevTripLogs_withPass_PrevDay()
        {
            try
            {
                //Create List of 
                var today = dt.GetCurrentDate();
                var today_seven_days_before = dt.GetCurrentDate().AddDays(-1);
                var scrLogTrips = db.crLogTrips.Where(c => c.DtTrip >= today_seven_days_before).ToList();
                var tripsWithPass = new List<crLogTrip>();

                foreach (var logs in scrLogTrips)
                {
                    tripsWithPass.Add(logs);
                }

                tripsWithPass = tripsWithPass.OrderByDescending(c => c.DtTrip).ToList();

                return tripsWithPass;
            }
            catch
            {
                return new List<crLogTrip>();
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

        public bool PassengerStatusUpdate(int id, int actId, string time, string reason)
        {
            try
            {
                var passenger = db.crLogPassengers.Find(id);
                passenger.crLogPassStatusId = actId;
                passenger.Remarks = reason;

                switch (actId)
                {
                    case 2:
                        //Contacted
                        passenger.timeContacted = time;
                        break;
                    case 3:
                        //boarded
                        passenger.timeBoarded = time;
                        break;
                    case 4:
                        //dropped
                        passenger.timeDelivered = time;
                        break;
                    case 5:
                        //declined
                        passenger.timeContacted = time;
                        break;
                    case 6:
                        //failed
                        passenger.timeContacted = time;
                        break;
                }

                db.Entry(passenger).State = EntityState.Modified;
                db.SaveChanges();

                return true;
            }
            catch 
            {
               
                return false;
            }
        }

        public ActionResult DriversPortal()
        {
            ViewBag.DriversId = new SelectList(db.crLogDrivers.ToList(), "Id", "Name");
            return View();
        }

        [HttpPost]
        public ActionResult DriversPortal(int DriversId)
        {
            ViewBag.DriversId = new SelectList(db.crLogDrivers.ToList(), "Id", "Name");
            return RedirectToAction("DriversTripList", new { id = DriversId });
        }

        public string GetPassengerList(int? id)
        {
            if (id == null)
            {
                return null;
            }

            var passengerList = db.crLogPassengers.Where(c => c.crLogTripId == id).ToList().Select(c=> new { c.Id, c.Name });

            return JsonConvert.SerializeObject(passengerList, Formatting.Indented);
        }

        [HttpPost]
        public bool CopyTripPassengers(int destTripId, int passengerId)
        {
            try
            {
                var CopiedPassenger = db.crLogPassengers.Find(passengerId);
                CopiedPassenger.crLogTripId = destTripId;
                CopiedPassenger.crLogPassStatusId = 1;
                CopiedPassenger.timeBoarded = " ";
                CopiedPassenger.timeContacted = " ";
                CopiedPassenger.timeDelivered = " ";
                CopiedPassenger.Remarks = " ";

                db.crLogPassengers.Add(CopiedPassenger);
                db.SaveChanges();

                return true;
            }
            catch
            {
               
                return false;
            }
        }

        public bool CheckPassengerNameOnMaster(crLogPassenger passenger)
        {
            var isNotEncoded = db.crLogPassengerMasters.Where(p => passenger.Name == p.Name);
            if (isNotEncoded.FirstOrDefault() == null)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public bool SavePassengerToMasterList(crLogPassenger passenger)
        {
            try
            {
                if (passenger == null)
                {
                    return false;
                }

                crLogPassengerMaster passengerMaster = new crLogPassengerMaster();
                passengerMaster.Name = passenger.Name;
                passengerMaster.Contact = passenger.Contact;
                passengerMaster.PassAddress = passenger.PassAddress;
                passengerMaster.PickupPoint = passenger.PickupPoint;
                passengerMaster.PickupTime = passenger.PickupTime;
                passengerMaster.DropPoint = passenger.DropPoint;
                passengerMaster.DropTime = passenger.DropTime;
                passengerMaster.Area = passenger.Area;
                passengerMaster.NextDay = passenger.NextDay;

                db.crLogPassengerMasters.Add(passengerMaster);
                db.SaveChanges();
                return true;
            }
            catch 
            {
                return false;
            }
        }


        public ActionResult CheckAddPassengerToMasterList(int? passId)
        {
            try
            {
                if (passId == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                var passenger = db.crLogPassengers.Find(passId);

                if (passenger == null)
                {
                    return HttpNotFound();
                }

                //check passenger
                if (CheckPassengerNameOnMaster(passenger))
                {
                    SavePassengerToMasterList(passenger);
                }

                return RedirectToAction("Index","crLogPassengerMasters",null);
            }
            catch 
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }


        public List<crLogPassengerMaster> GetPassengersNotInTrip(int tripId)
        {

            //get passengers of the trip 
            var tripPassengers = db.crLogPassengers.Where(p => p.crLogTripId == tripId)
                .ToList().Select(p => p.Name);

            //get excluded passengers from masters list 
            var excludedPassengers = db.crLogPassengerMasters
                .Where(p => tripPassengers.Contains(p.Name))
                .ToList().Select(p => p.Id);

            //get passengers not in the excluded list
            var passengerList = db.crLogPassengerMasters
                .Where(p => !excludedPassengers.Contains(p.Id))
                .OrderBy(p => p.Name).ThenBy(p => p.Area)
                .ToList();

            return passengerList;
        }

        public string GetPassengerMasterData(int passId)
        {
            var passenger = db.crLogPassengerMasters.Find(passId);

            return JsonConvert.SerializeObject(passenger, Formatting.Indented);
        }

        public ActionResult CopyPassengersFromMaster(int tripId)
        {
            try
            {
                var tripPassengers = GetPassengersNotInTrip(tripId).Select(p=>p.Id);

                var masterList = db.crLogPassengerMasters.Where(p => tripPassengers.Contains(p.Id) ).ToList();

                foreach (var passengers in masterList)
                {
                    crLogPassenger psgr = new crLogPassenger();
                    psgr.Name = passengers.Name;
                    psgr.Contact = passengers.Contact;
                    psgr.PassAddress = passengers.PassAddress;
                    psgr.PickupPoint = passengers.PickupPoint;
                    psgr.PickupTime = passengers.PickupTime;
                    psgr.DropPoint = passengers.DropPoint;
                    psgr.DropTime = passengers.DropTime;
                    psgr.Area = passengers.Area;
                    psgr.Remarks = " ";
                    psgr.timeBoarded = " ";
                    psgr.timeContacted = " ";
                    psgr.timeDelivered = " ";
                    psgr.crLogPassStatusId = 1;
                    psgr.crLogTripId = tripId;
                    psgr.NextDay = passengers.NextDay;

                    db.crLogPassengers.Add(psgr);
                    db.SaveChanges();
                }

                return RedirectToAction("TripPassengers", new { id = tripId });
            }
            catch (Exception ex)
            {
                throw ex;
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        [HttpPost]
        public bool TransferPassenger(int tripId, int passId)
        {
            try
            {
                var passenger = db.crLogPassengers.Find(passId);
                passenger.crLogTripId = tripId;

                db.Entry(passenger).State = EntityState.Modified;
                db.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool CopyPassengerFromMaster(int tripId, int passId)
        {
            try
            {
                var passengers = db.crLogPassengerMasters.Find(passId);

                if (passengers == null)
                {
                    return false;
                }

                crLogPassenger psgr = new crLogPassenger();
                psgr.Name = passengers.Name;
                psgr.Contact = passengers.Contact;
                psgr.PassAddress = passengers.PassAddress;
                psgr.PickupPoint = passengers.PickupPoint;
                psgr.PickupTime = passengers.PickupTime;
                psgr.DropPoint = passengers.DropPoint;
                psgr.DropTime = passengers.DropTime;
                psgr.Area = passengers.Area;
                psgr.Remarks = " ";
                psgr.timeBoarded = " ";
                psgr.timeContacted = " ";
                psgr.timeDelivered = " ";
                psgr.crLogPassStatusId = 1;
                psgr.crLogTripId = tripId;
                psgr.NextDay = passengers.NextDay;

                db.crLogPassengers.Add(psgr);
                db.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
