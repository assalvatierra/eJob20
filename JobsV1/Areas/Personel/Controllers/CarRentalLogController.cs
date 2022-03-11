using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using JobsV1.Areas.Personel.Models;
using Microsoft.Ajax.Utilities;
using JobsV1.Areas.Personel.Services;

namespace JobsV1.Areas.Personel.Controllers
{
    public class CarRentalLogController : Controller
    {
        private CarRentalLogDBContainer db = new CarRentalLogDBContainer();
        private CrDataLayer dl = new CrDataLayer();
        private DateClass dt = new DateClass();
        private crDriverData dd = new crDriverData();
        private CrLogServices crServices;
        private CrOTServices otServices;

        // GET: Personel/CarRentalLog
        public ActionResult Index(string startDate, string endDate, string unit, string driver, string company, string sortby, string owner)
        {
            #region Session
            if (!startDate.IsNullOrWhiteSpace())
            {
                Session["triplog-startDate"] = startDate;
            }
            else
            {
                if (Session["triplog-startDate"] != null)
                {
                    startDate = Session["triplog-startDate"].ToString();
                }
            }
               
            if (!endDate.IsNullOrWhiteSpace())
            {
                Session["triplog-endDate"] = endDate;
            }
            else
            {
                if(Session["triplog-endDate"] != null)
                {
                    endDate = Session["triplog-endDate"].ToString();
                }
            }
               
            if (!unit.IsNullOrWhiteSpace())
            {
                Session["triplog-unit"] = unit;
            }
            else
            {
                if(Session["triplog-unit"] != null)
                {
                    unit = Session["triplog-unit"].ToString();
                }
            }
               
            if (!driver.IsNullOrWhiteSpace())
            {
                Session["triplog-driver"] = driver;
            }
            else
            {
                if(Session["triplog-driver"] != null)
                {
                    driver = Session["triplog-driver"].ToString();
                }
            }
               
            if (!company.IsNullOrWhiteSpace())
            {
                Session["triplog-company"] = company;
            }
            else
            {
                if(Session["triplog-company"] != null)
                {
                    company = Session["triplog-company"].ToString();
                }
            }

            if (!owner.IsNullOrWhiteSpace())
            {
                Session["triplog-owner"] = owner;
            }
            else
            {
                if (Session["triplog-owner"] != null)
                {
                    owner = Session["triplog-owner"].ToString();
                }
            }

            #endregion

            crServices = new CrLogServices(db);

            var tripLogs = crServices.GetTripLogs(startDate, endDate, unit, driver, company, sortby, owner);

            //get summary
            var logSummary = crServices.GetCrLogSummary(tripLogs);

            ViewBag.DriversLogSummary = logSummary.CrDrivers ?? new List<CrDriverLogs>();
            ViewBag.CompaniesLogSummary = logSummary.CrCompanies ?? new List<CrCompanyLogs>();
            ViewBag.UnitsLogSummary = logSummary.CrUnits ?? new List<CrUnitLogs>();

            ViewBag.FilteredsDate = startDate;
            ViewBag.FilteredeDate = endDate;
            ViewBag.FilteredUnit = unit ?? "all";
            ViewBag.FilteredDriver = driver ?? "all";
            ViewBag.FilteredCompany = company ?? "all";
            ViewBag.SortBy = sortby ?? "Date";
            ViewBag.Owner = owner ?? "all";

            ViewBag.crLogUnitList    = dl.GetUnits().ToList();
            ViewBag.crLogDriverList  = dl.GetDrivers().ToList();
            ViewBag.crLogCompanyList = dl.GetCompanies().ToList();
            ViewBag.crLogOwnerList   = dl.GetOwners().ToList();

            ViewBag.IsAdmin = User.IsInRole("Admin");

            //check and finalize trip every 6pm
            //if (string.IsNullOrEmpty(startDate))
            //{
                crServices.CheckTripFinalizeRange(tripLogs);
            //}

            return View(tripLogs);

        }

        // GET: Personel/CarRentalLog/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            crLogTrip crLogTrip = db.crLogTrips.Find(id);
            if (crLogTrip == null)
            {
                return HttpNotFound();
            }
            return View(crLogTrip);
        }

        // GET: Personel/CarRentalLog/Create
        public ActionResult Create()
        {
            crLogTrip trip = new crLogTrip();
            trip.DtTrip = System.DateTime.Now;
            trip.Rate = 0;
            trip.Expenses = 0;
            trip.Addon = 0;
            trip.DriverFee = 0;
            trip.DriverOTRate = 50;
            trip.OTRate = 200;
            trip.IsFinal = false;
            trip.AllowEdit = false;

            var unitList = dl.GetUnits().Select(c => new {
                Id = c.Id,
                Description = c.Description + " (" + c.crLogOwner.Name + ")"
            }).ToList();
            ViewBag.crLogDriverId = new SelectList(dl.GetDrivers(), "Id", "Name");
            ViewBag.crLogUnitId = new SelectList(unitList, "Id", "Description");
            ViewBag.crLogCompanyId = new SelectList(dl.GetCompanies(), "Id", "Name");
            //ViewBag.crLogClosingId = new SelectList(db.crLogClosings, "Id", "Id");
            return View(trip);
        }

        // POST: Personel/CarRentalLog/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,crLogDriverId,crLogUnitId,crLogCompanyId,DtTrip,Rate,Addon,Expenses," +
            "DriverFee,Remarks, OdoStart, OdoEnd, DriverOt, TripHours, StartTime, EndTime, OTRate, DriverOTRate, AddonOT, " +
            "IsFinal, AllowEdit")] crLogTrip crLogTrip)
        { 
            if (ModelState.IsValid)
            {
                db.crLogTrips.Add(crLogTrip);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.crLogDriverId = new SelectList(dl.GetDrivers(), "Id", "Name", crLogTrip.crLogDriverId);
            ViewBag.crLogUnitId = new SelectList(dl.GetUnits(), "Id", "Description", crLogTrip.crLogUnitId);
            ViewBag.crLogCompanyId = new SelectList(dl.GetCompanies(), "Id", "Name", crLogTrip.crLogCompanyId);
            //ViewBag.crLogClosingId = new SelectList(db.crLogClosings, "Id", "Id", crLogTrip.crLogClosingId);
            return View(crLogTrip);
        }

        // GET: Personel/CarRentalLog/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            crLogTrip crLogTrip = db.crLogTrips.Find(id);

            if (crLogTrip == null)
            {
                return HttpNotFound();
            }

            if (crLogTrip.OTRate == null)
            {
                crLogTrip.OTRate = 200;
            }
            

            if (crLogTrip.DriverOTRate == null)
            {
                crLogTrip.DriverOTRate = 50;
            }

            var unitList = dl.GetUnits().Select(c => new {
                Id = c.Id,
                Description = c.Description + " (" + c.crLogOwner.Name + ")"
            }).ToList();

            ViewBag.crLogDriverId = new SelectList(dl.GetDrivers(), "Id", "Name", crLogTrip.crLogDriverId);
            ViewBag.crLogUnitId = new SelectList(unitList, "Id", "Description", crLogTrip.crLogUnitId);
            ViewBag.crLogCompanyId = new SelectList(dl.GetCompanies(), "Id", "Name", crLogTrip.crLogCompanyId);
            //ViewBag.crLogClosingId = new SelectList(db.crLogClosings, "Id", "Id", crLogTrip.crLogClosingId);
            return View(crLogTrip);
        }

        // POST: Personel/CarRentalLog/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,crLogDriverId,crLogUnitId,crLogCompanyId,DtTrip,Rate,Addon,Expenses," +
            "DriverFee,Remarks, OdoStart, OdoEnd, crLogClosingId, DriverOt, TripHours, StartTime, EndTime, OTRate, DriverOTRate, " +
            "AddonOT, IsFinal, AllowEdit, TripTicket")] crLogTrip crLogTrip)
        {
            if (ModelState.IsValid)
            {
                otServices = new CrOTServices(db);
                //caculate OT
                var OTRate = otServices.GetTripLogOTRate(crLogTrip);

                if (OTRate > 0 && crLogTrip.DriverOT == 0 )
                {
                    crLogTrip.DriverOT = (decimal)OTRate;
                }

                if (crLogTrip.IsFinal)
                {
                    //disable edit on finalized
                    crLogTrip.AllowEdit = false;
                
                }

                db.Entry(crLogTrip).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            var unitList = dl.GetUnits().Select(c => new {
                Id = c.Id,
                Description = c.Description + " ("+ c.crLogOwner.Name +")"
            }).ToList();

            ViewBag.crLogDriverId = new SelectList(dl.GetDrivers(), "Id", "Name", crLogTrip.crLogDriverId);
            ViewBag.crLogUnitId = new SelectList(unitList, "Id", "Description", crLogTrip.crLogUnitId);
            ViewBag.crLogCompanyId = new SelectList(dl.GetCompanies(), "Id", "Name", crLogTrip.crLogCompanyId);
            //ViewBag.crLogClosingId = new SelectList(db.crLogClosings, "Id", "Id", crLogTrip.crLogClosingId);
            return View(crLogTrip);
        }

        // GET: Personel/CarRentalLog/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            crLogTrip crLogTrip = db.crLogTrips.Find(id);
            if (crLogTrip == null)
            {
                return HttpNotFound();
            }
            return View(crLogTrip);
        }

        // POST: Personel/CarRentalLog/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            crServices = new CrLogServices(db);
            //remove passengers from trip
            crServices.DeleteTripPassengers(id);

            //delete trip record
            crLogTrip crLogTrip = db.crLogTrips.Find(id);
            db.crLogTrips.Remove(crLogTrip);
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

        public ActionResult CopyTrip()
        {
            ViewBag.isSubmitted = false;
            ViewBag.companyId = new SelectList(dl.GetCompanies(), "Id", "Name");
            return View(new List<crLogTrip>());
        }


        #region Reports 
        public ActionResult ReportFilter(string reportby, string startDate, string endDate, string unit, string driver, string company, string sortby, string buffer)
        {

            ViewBag.ReportBy = reportby ?? "Company";
            ViewBag.FilteredsDate = startDate;
            ViewBag.FilteredeDate = endDate;
            ViewBag.FilteredUnit = unit ?? "all";
            ViewBag.FilteredDriver = driver ?? "all";
            ViewBag.FilteredCompany = company ?? "all";
            ViewBag.SortBy = sortby?? "Date";
            ViewBag.crLogUnitList = dl.GetUnits().ToList();
            ViewBag.crLogDriverList = dl.GetDrivers().ToList();
            ViewBag.crLogCompanyList = dl.GetCompanies().ToList();

            return View();
        }

        [HttpPost]
        public ActionResult ReportFilter(string reportby, string startDate, string endDate, string unit, string driver, string company, string sortby)
        {
            switch (reportby)
            {
                case "Company":
                        return RedirectToAction("ReportByCompany", new
                        {
                            startDate = startDate,
                            endDate = endDate,
                            unit = unit,
                            driver = driver,
                            company = company,
                            sortby = sortby
                        });
                case "Driver":
                        return RedirectToAction("ReportByDriver", new
                        {
                            startDate = startDate,
                            endDate = endDate,
                            unit = unit,
                            driver = driver,
                            company = company,
                            sortby = sortby
                        });
                case "Unit":
                  
                        return RedirectToAction("ReportByUnit", new
                        {
                            startDate = startDate,
                            endDate = endDate,
                            unit = unit,
                            driver = driver,
                            company = company,
                            sortby = sortby
                        });
                default:
                    break;
            }
            ViewBag.ReportBy = reportby;
            ViewBag.FilteredsDate = startDate;
            ViewBag.FilteredeDate = endDate;
            ViewBag.FilteredUnit = unit ?? "all";
            ViewBag.FilteredDriver = driver ?? "all";
            ViewBag.FilteredCompany = company ?? "all";
            ViewBag.SortBy = sortby ?? "Date";
            ViewBag.crLogUnitList = dl.GetUnits().ToList();
            ViewBag.crLogDriverList = dl.GetDrivers().ToList();
            ViewBag.crLogCompanyList = dl.GetCompanies().ToList();

            return View();
        }

        public ActionResult ReportByCompany(string startDate, string endDate, string unit, string driver, string company, string sortby)
        {
            crServices = new CrLogServices(db);

            var tripLogs = crServices.GetFilteredTripLogs(startDate, endDate, unit, driver, company, sortby).ToList();

            List<CrReports.ReportByCompany> reportResult = new List<CrReports.ReportByCompany>();
            decimal runningRate = 0;
            foreach (var trip in tripLogs)
            {
                var rate = trip.Rate + trip.Addon;
                runningRate += rate;

                CrReports.ReportByCompany report = new CrReports.ReportByCompany();
                report.Company = trip.crLogCompany.Name;
                report.Driver = trip.crLogDriver.Name;
                report.Vehicle = trip.crLogUnit.Description;
                report.DtTrip = trip.DtTrip;
                report.Rate = rate;
                report.Running = runningRate;
                report.OdoStart = trip.OdoStart ?? 0;
                report.OdoEnd = trip.OdoEnd ?? 0;

                reportResult.Add(report);
            }

            ViewBag.CompanyDetails = company;

            ViewBag.ReportBy = "Company";
            ViewBag.FilteredsDate = startDate;
            ViewBag.FilteredeDate = endDate;
            ViewBag.FilteredUnit = unit ?? "all";
            ViewBag.FilteredDriver = driver ?? "all";
            ViewBag.FilteredCompany = company ?? "all";
            ViewBag.SortBy = sortby ?? "Date";

            return View(reportResult);
        }

        public ActionResult ReportByDriver(string startDate, string endDate, string unit, string driver, string company, string sortby)
        {
            crServices = new CrLogServices(db);

            var tripLogs = crServices.GetFilteredTripLogs(startDate, endDate, unit, driver, company, sortby).ToList();

            List<CrReports.ReportByDriver> reportResult = new List<CrReports.ReportByDriver>();
            decimal runningRate = 0;
            foreach (var trip in tripLogs)
            {
                runningRate += trip.DriverFee;

                CrReports.ReportByDriver report = new CrReports.ReportByDriver();
                report.Company = trip.crLogCompany.Name;
                report.Driver = trip.crLogDriver.Name;
                report.Vehicle = trip.crLogUnit.Description;
                report.DtTrip = trip.DtTrip;
                report.Rate = trip.DriverFee;
                report.Running = runningRate;
                report.OdoStart = trip.OdoStart ?? 0;
                report.OdoEnd = trip.OdoEnd ?? 0;

                reportResult.Add(report);
            }

            ViewBag.DriverDetails = driver;

            ViewBag.ReportBy = "Driver";
            ViewBag.FilteredsDate = startDate;
            ViewBag.FilteredeDate = endDate;
            ViewBag.FilteredUnit = unit ?? "all";
            ViewBag.FilteredDriver = driver ?? "all";
            ViewBag.FilteredCompany = company ?? "all";
            ViewBag.SortBy = sortby ?? "Date";

            return View(reportResult);
        }

        public ActionResult ReportByUnit(string startDate, string endDate, string unit, string driver, string company, string sortby)
        {
            crServices = new CrLogServices(db);

            var tripLogs = crServices.GetFilteredTripLogs(startDate, endDate, unit, driver, company, sortby).ToList();

            List<CrReports.ReportByUnit> reportResult = new List<CrReports.ReportByUnit>();
            decimal runningRate = 0;
            foreach (var trip in tripLogs)
            {
                var rate = ( trip.Rate + trip.Addon ) - (trip.DriverFee + trip.Expenses);
                runningRate += rate;

                CrReports.ReportByUnit report = new CrReports.ReportByUnit();
                report.Company = trip.crLogCompany.Name;
                report.Driver = trip.crLogDriver.Name;
                report.Vehicle = trip.crLogUnit.Description;
                report.DtTrip = trip.DtTrip;
                report.Rate = rate;
                report.Running = runningRate;
                report.OdoStart = trip.OdoStart ?? 0;
                report.OdoEnd = trip.OdoEnd ?? 0;

                reportResult.Add(report);
            }

            ViewBag.UnitDetails = unit;

            ViewBag.ReportBy = "Unit";
            ViewBag.FilteredsDate = startDate;
            ViewBag.FilteredeDate = endDate;
            ViewBag.FilteredUnit = unit ?? "all";
            ViewBag.FilteredDriver = driver ?? "all";
            ViewBag.FilteredCompany = company ?? "all";
            ViewBag.SortBy = sortby ?? "Date";

            return View(reportResult);
        }

        #endregion

        #region CarRentalLogs APIs

        // GET: Personel/CarRentalLog/CopyTripSubmit/5
        [HttpPost]
        public bool CopyTripSubmit(int? id, string copyDate, int? copyPassengers)
        {
            try
            {
                if (copyDate.IsNullOrWhiteSpace() || id == null)
                {
                    return false;
                }

                var cDate = DateTime.ParseExact(copyDate, "MM/dd/yyyy", CultureInfo.InvariantCulture);

                //find logs by id
                var cLogTrip = db.crLogTrips.Find(id);

                if (cLogTrip == null)
                {
                    return false;
                }

                crLogTrip newCrLogTrip = new crLogTrip();
                newCrLogTrip.crLogCompanyId = cLogTrip.crLogCompanyId;
                newCrLogTrip.crLogDriverId = cLogTrip.crLogDriverId;
                newCrLogTrip.crLogUnitId = cLogTrip.crLogUnitId;
                newCrLogTrip.DriverFee = cLogTrip.DriverFee;
                newCrLogTrip.Expenses = cLogTrip.Expenses;
                newCrLogTrip.Rate = cLogTrip.Rate;
                newCrLogTrip.Remarks = cLogTrip.Remarks;
                newCrLogTrip.Addon = cLogTrip.Addon;
                newCrLogTrip.DtTrip = cDate.AddTicks(cLogTrip.DtTrip.TimeOfDay.Ticks);

                db.crLogTrips.Add(newCrLogTrip);
                db.SaveChanges();

                if (copyPassengers == 1)
                {
                    CopyPassTripSubmit((int)id, newCrLogTrip.Id);
                }

                return true;
            }
            catch
            {

                return false;
            }
        }

        // GET: Personel/CarRentalLog/CopyPassTripSubmit/5
        [HttpPost]
        public bool CopyPassTripSubmit(int id, int destTripId)
        {
            try
            {
                //find logs by id
                var cLogTrip = db.crLogTrips.Find(id);

                if (cLogTrip == null)
                {
                    return false;
                }

                //get list of passengers from log trip id
                var passengersList = db.crLogPassengers.Where(p => p.crLogTripId == id).ToList();

                if (passengersList != null)
                {
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
                return false;
            }
            catch
            {
                return false;
            }
        }

        // POST : /Personel/CarRentalLog/UpdateTripOTRate
        [HttpPost]
        public HttpResponseMessage UpdateTripOTRate(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpResponseMessage(HttpStatusCode.BadRequest);
                }

                //get trip
                var trip = db.crLogTrips.Find(id);

                var OTRate = otServices.GetTripLogOTRate(trip);
                if (OTRate > 0 && trip.DriverOT == 0)
                {
                    trip.DriverOT = (decimal)OTRate;
                }

                trip.AllowEdit = false;
                trip.IsFinal = true;
                trip.Remarks += "TT";

                db.Entry(trip).State = EntityState.Modified;
                db.SaveChanges();

                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            catch
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
        }

        // GET: Personel/CarRentalLog/GetTripOdo/5
        // id = crLogTripId
        [HttpGet]
        public JsonResult GetTripOT(int? id)
        {
            if (id == null)
            {
                return null;
            }

            crLogTrip crLogTrip = db.crLogTrips.Find(id);

            if (crLogTrip == null)
            {
                return null;
            }

            //default
            crLogCompanyRate companyRate = new crLogCompanyRate();
            companyRate.TripHours = 10;
            companyRate.DriverOTRate = 50;

            if (crLogTrip.crLogCompany.crLogCompanyRates.Count() > 0)
            {
                companyRate = crLogTrip.crLogCompany.crLogCompanyRates.FirstOrDefault();
            }

            //get trip log
            var triplog = db.crLogTrips.Find(id);

            TripOTRequest odoDetails = new TripOTRequest();
            odoDetails.Date = triplog.DtTrip.ToShortDateString();
            odoDetails.Unit = triplog.crLogUnit.Description;
            odoDetails.Driver = triplog.crLogDriver.Name;
            odoDetails.Company = triplog.crLogCompany.Name;

            odoDetails.StartTime = triplog.StartTime;
            odoDetails.EndTime = triplog.EndTime;
            odoDetails.TripHours = companyRate.TripHours;
            odoDetails.OTRate = triplog.OTRate ?? 200;
            odoDetails.DriverOTRate = companyRate.DriverOTRate;
            odoDetails.Remarks = triplog.Remarks;


            return Json(odoDetails, JsonRequestBehavior.AllowGet);
        }

        //POST: CarRentalLog/SetTripOT
        [HttpPost]
        public bool SetTripOT(int? Id, string StartTime, string EndTime, int? TripHours, 
            Decimal? OTRate, Decimal? DriverOTRate, string Remarks)
        {
            if (Id != null)
            {
                otServices = new CrOTServices(db);
                crLogTrip crLogTrip = db.crLogTrips.Find(Id);

                if (crLogTrip == null)
                    return false;

                //update odo values
                crLogTrip.StartTime = StartTime;
                crLogTrip.EndTime = EndTime;
                crLogTrip.TripHours = TripHours;
                crLogTrip.OTRate = OTRate;
                crLogTrip.DriverOTRate = DriverOTRate;
                crLogTrip.DriverOT = (decimal)otServices.GetTripOTRate(Id);
                crLogTrip.AddonOT = (decimal)otServices.GetTripOTAddon(Id);
                crLogTrip.Remarks = Remarks;

                //save changes
                db.Entry(crLogTrip).State = EntityState.Modified;
                db.SaveChanges();

                return true;
            }

            return false;
        }

        // GET: Personel/CarRentalLog/GetTripOdo/5
        // id = crLogTripId
        [HttpGet]
        public JsonResult GetTripOdo(int? id)
        {
            if (id == null)
            {
                return null;
            }

            crLogTrip crLogTrip = db.crLogTrips.Find(id);

            if (crLogTrip == null)
            {
                return null;
            }

            //get trip log
            var triplog = db.crLogTrips.Find(id);

            TripOdoRequest odoDetails = new TripOdoRequest();
            odoDetails.Start = triplog.OdoStart ?? 0;
            odoDetails.End = triplog.OdoEnd ?? 0;
            odoDetails.Date = triplog.DtTrip.ToShortDateString();
            odoDetails.Unit = triplog.crLogUnit.Description;
            odoDetails.Driver = triplog.crLogDriver.Name;
            odoDetails.Company = triplog.crLogCompany.Name;


            return Json(odoDetails, JsonRequestBehavior.AllowGet);
        }

        //POST: CarRentalLog/SetTripOdo
        [HttpPost]
        public bool SetTripOdo(int? Id, int? OdoStart, int? OdoEnd)
        {
            if (Id != null)
            {
                crLogTrip crLogTrip = db.crLogTrips.Find(Id);

                if (crLogTrip == null)
                    return false;

                //update odo values
                crLogTrip.OdoStart = OdoStart ?? 0;
                crLogTrip.OdoEnd = OdoEnd ?? 0;

                db.Entry(crLogTrip).State = EntityState.Modified;
                db.SaveChanges();
                return true;
            }

            return false;
        }


        [HttpPost]
        public HttpResponseMessage SetTripExpense(int id, decimal expense)
        {
            try
            {
                //find trip
                var triplog = db.crLogTrips.Find(id);

                //set trip as final, cannot be edited
                triplog.Expenses = expense;

                //save changes
                db.Entry(triplog).State = EntityState.Modified;
                db.SaveChanges();

                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            catch
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
        }

        //POST: CarRentalLog/SetTripFinalize
        [HttpPost]
        public HttpResponseMessage SetTripFinalize(int id)
        {
            try
            {
                //find trip
                var triplog = db.crLogTrips.Find(id);

                //set trip as final, cannot be edited
                triplog.IsFinal = true;

                //save changes
                db.Entry(triplog).State = EntityState.Modified;
                db.SaveChanges();

                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            catch
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
        }

        //POST: CarRentalLog/SetTripFinal
        [HttpPost]
        public bool SetTripFinal(crLogTrip triplog)
        {
            try
            {
                //set trip as final, cannot be edited
                triplog.IsFinal = true;

                //save changes
                db.Entry(triplog).State = EntityState.Modified;

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //GET: CarRentalLog/GetTripWarningByUnit
        [HttpGet]
        public bool GetTripWarningByUnit(int id, string date)
        {
            var trip = db.crLogTrips.Find(id);
            var sdate = DateTime.Parse(date);
            if (GetUnitIsInTripByDate(trip.crLogUnitId, sdate) && trip.crLogUnit.Description != "Office")
            {
                return true;
            }

            return false;
        }

        //GET: CarRentalLog/GetTripWarningByDriver
        [HttpGet]
        public bool GetTripWarningByDriver(int id, string date)
        {
            var trip = db.crLogTrips.Find(id);
            var sdate = DateTime.Parse(date);
            if (GetDriverIsInTripByDate(trip.crLogDriverId, sdate))
            {
                return true;
            }

            return false;
        }

        //POST: Personel/CarRentalLog/SetLinkTriplogJobs/{triplogId:int, jobmainId:int}
        [HttpPost]
        public HttpResponseMessage SetLinkTriplogJobs(int triplogId, int jobmainId)
        {
            try
            {
                //new entry
                crLogTripJobMain logTripJobMain = new crLogTripJobMain();
                logTripJobMain.crLogTripId = triplogId;
                logTripJobMain.JobMainId = jobmainId;

                db.crLogTripJobMains.Add(logTripJobMain);
                db.SaveChanges();

                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
            catch
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
        }

        //GET: Personel/CarRentalLog/GetLinkTriplogJobs/{triplogId:int}
        [HttpGet]
        public int GetLinkTriplogJobs(int triplogId)
        {
            try
            {
                //find existing triplog-jobmain link
                var link = db.crLogTripJobMains.Where(c => c.crLogTripId == triplogId);

                if (link.FirstOrDefault() == null)
                {
                    return 0;
                }

                return link.FirstOrDefault().JobMainId;
            }
            catch 
            {
                return 0;
            }
        }

        //POST: Personel/CarRentalLog/DeleteLinkTriplogJobs/{triplogId:int, jobmainId:int}
        [HttpPost]
        public bool DeleteLinkTriplogJobs(int triplogId, int jobmainId)
        {
            try
            {
                //new entry
                var logTripJobMain = db.crLogTripJobMains.Where(c => c.crLogTripId == triplogId && c.JobMainId == jobmainId);

                if (logTripJobMain != null)
                {

                    db.crLogTripJobMains.Remove(logTripJobMain.FirstOrDefault());
                    db.SaveChanges();
                    return true;
                }

                return false;
            }
            catch 
            {
                return false;
            }
        }

        //GET: CarRentalLog/GetTripIdLinkCountToday
        [HttpGet]
        public JsonResult GetTripIdLinkCountToday(int? jobId)
        {
            try
            {
                if (jobId == null)
                {
                    return Json(0, JsonRequestBehavior.AllowGet);
                }

                var today = dt.GetCurrentDate();

                var LinktripCount = db.crLogTripJobMains
                    .Where(c => c.JobMainId == jobId && DbFunctions.TruncateTime(c.crLogTrip.DtTrip) == today)
                    .Count();

                return Json(LinktripCount, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }

        //POST: Personel/CarRentalLog/UpdateDriverTime
        [HttpPost]
        public bool UpdateDriverTime(int tripId, string time, int typeId)
        {
            try
            {
                var trip = db.crLogTrips.Find(tripId);

                if (trip == null)
                {
                    return false;
                }

                //TIME-IN
                if (typeId == 1)
                {
                    trip.StartTime = time;
                }

                //TIME-OUT
                if (typeId == 2)
                {
                    trip.EndTime = time;
                }

                db.Entry(trip).State = EntityState.Modified;
                db.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        // GET: Personel/CarRentalLog/RequestPOFuel
        [HttpGet]
        public bool RequestPOFuel(int driverId, int unitId)
        {
            try
            {
                if (driverId == 0 || unitId == 0)
                {
                    return false;
                }

                var today = dt.GetCurrentDateTime();

                crLogFuel fuelReq = new crLogFuel();
                fuelReq.crLogDriverId = driverId;
                fuelReq.crLogUnitId = unitId;
                fuelReq.crLogTypeId = 1;
                fuelReq.crLogPaymentTypeId = 3;
                fuelReq.dtFillup = today;
                fuelReq.dtRequest = today;
                fuelReq.Amount = 2500;
                fuelReq.isFullTank = true;
                fuelReq.Remarks = "";
                fuelReq.crLogFuelStatus.Add(
                    new crLogFuelStatus
                    {
                        crCashReqStatusId = 1,
                        crLogFuelId = fuelReq.Id,
                        dtStatus = today
                    });

                db.crLogFuels.Add(fuelReq);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        // GET: Personel/CarRentalLog/ReqCopyTripuestPOFuel
        [HttpPost]
        public ActionResult CopyTrip(string srchDate, int? companyId)
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
                    return View(crLogTrips.ToList());
                }

                ViewBag.companyId = new SelectList(dl.GetCompanies(), "Id", "Name");
                ViewBag.isSubmitted = false;
                return View(new List<crLogTrip>());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //POST : /Personel/CarRentalLog/AllowEditTripLog
        //SUMMARY: Update AllowEdit Flag on TripLogs to edit past / locked triplogs entry
        [HttpPost]
        public bool AllowEditTripLog(int? id)
        {
            try
            {
                if (id == null)
                {
                    return false;
                }

                var triplog = db.crLogTrips.Find(id);

                if (triplog == null)
                {
                    return false;
                }

                triplog.AllowEdit = true;
                db.Entry(triplog).State = EntityState.Modified;
                db.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        //POST : /Personel/CarRentalLog/CloseForEditTripLog
        //SUMMARY: Update AllowEdit Flag on TripLogs to edit past / locked triplogs entry
        [HttpPost]
        public bool CloseForEditTripLog(int? id)
        {
            try
            {
                if (id == null)
                {
                    return false;
                }

                var triplog = db.crLogTrips.Find(id);

                if (triplog == null)
                {
                    return false;
                }

                triplog.AllowEdit = false;
                db.Entry(triplog).State = EntityState.Modified;
                db.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        // GET: /Areas/Personal/CarRentalLog/GetDriverIsInTripToday?driverId={driverId}
        // Used for checking duplicate entries for driver on encoding/Create and in triplog/index
        [HttpGet]
        public bool GetDriverIsInTripToday(int driverId)
        {
            var today = dt.GetCurrentDate();

            var isInTripToday = db.crLogTrips.Where(c => c.crLogDriverId == driverId &&
                                        DbFunctions.TruncateTime(c.DtTrip) == today
                                    ).ToList();

            if (isInTripToday.Count() > 0)
            {
                return true;
            }

            return false;
        }

        // GET: /Areas/Personal/CarRentalLog/GetUnitIsInTripToday?unitId={unitId}
        // Used for checking duplicate entries for Unit/Vehicle on encoding/Create and in triplog/index
        [HttpGet]
        public bool GetUnitIsInTripToday(int unitId)
        {
            var today = dt.GetCurrentDate();

            var isInTripToday = db.crLogTrips.Where(c => c.crLogUnitId == unitId &&
                                        DbFunctions.TruncateTime(c.DtTrip) == today
                                    ).ToList();

            if (isInTripToday.Count() > 0)
            {
                return true;
            }

            return false;
        }

        //POST: CarRentalLog/GetUnitIsInTripByDate
        [HttpGet]
        public bool GetUnitIsInTripByDate(int unitId, DateTime? date)
        {
            var today = dt.GetCurrentDate();

            if (date != null)
            {
                today = (DateTime)date;
            }

            var isInTripToday = db.crLogTrips.Where(c => c.crLogUnitId == unitId &&
                                        DbFunctions.TruncateTime(c.DtTrip) == today
                                    ).ToList();

            if (isInTripToday.Count() > 1)
            {
                return true;
            }

            return false;
        }

        //GET: CarRentalLog/GetDriverIsInTripByDate
        [HttpGet]
        public bool GetDriverIsInTripByDate(int driverId, DateTime? date)
        {
            var today = dt.GetCurrentDate();

            if (date != null)
            {
                today = (DateTime)date;
            }

            var isInTripToday = db.crLogTrips.Where(c => c.crLogDriverId == driverId &&
                                        DbFunctions.TruncateTime(c.DtTrip) == today
                                    ).ToList();

            if (isInTripToday.Count() > 1)
            {
                return true;
            }

            return false;
        }

        //POST
        [HttpPost]
        public HttpResponseMessage PostTripTicketFlag(int id)
        {
            try
            {
                var crLogTrip = db.crLogTrips.Find(id);

                if (crLogTrip == null)
                {
                    return new HttpResponseMessage(HttpStatusCode.NotFound);
                }

                crLogTrip.TripTicket = true;

                db.Entry(crLogTrip).State = EntityState.Modified;
                db.SaveChanges();

                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            catch
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
        }

        #endregion

        private class TripOdoRequest
        {
            public string Date { get; set; }
            public string Driver { get; set; }
            public string Unit { get; set; }
            public string Company { get; set; }
            public int Start { get; set; }
            public int End { get; set; }
        }

        private class TripOTRequest
        {
            public string Date { get; set; }
            public string Driver { get; set; }
            public string Unit { get; set; }
            public string Company { get; set; }
            public string StartTime { get; set; }
            public string EndTime { get; set; }
            public int TripHours { get; set; }
            public decimal OTRate { get; set; }
            public decimal DriverOTRate { get; set; }
            public string Remarks { get; set; }

        }
    }

}




