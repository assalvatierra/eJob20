﻿using System;
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
using JobsV1.Models;
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
        //private CarRentalLogSvc crServices = new CarRentalLogSvc();

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

            var tripLogs = GetTripLogs(startDate, endDate, unit, driver, company, sortby, owner);

            //get summary
            var logSummary = GetCrLogSummary(tripLogs);
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
            if (string.IsNullOrEmpty(startDate))
            {
                CheckTripFinalizeRange(tripLogs);
            }

            return View(tripLogs);

        }


        public List<crLogTrip> GetTripLogs(string startDate, string endDate, string unit, string driver, string company, string sortby)
        {
            var defaultStartDate = dt.GetCurrentDate();
            //Get Logs
            var crLogTrips = db.crLogTrips.Include(c => c.crLogDriver).Include(c => c.crLogUnit).Include(c => c.crLogCompany).Include(c => c.crLogClosing);

            //Filter
            if (!startDate.IsNullOrWhiteSpace() && !endDate.IsNullOrWhiteSpace())
            {
                var sdate = DateTime.ParseExact(startDate, "MM/dd/yyyy", CultureInfo.InvariantCulture).Date;
                var edate = DateTime.ParseExact(endDate, "MM/dd/yyyy", CultureInfo.InvariantCulture).Date;

                crLogTrips = crLogTrips.Where(c => DbFunctions.TruncateTime(c.DtTrip) >= sdate && DbFunctions.TruncateTime(c.DtTrip) <= edate);
            }
            else
            {
                crLogTrips = crLogTrips.Where(c => DbFunctions.TruncateTime(c.DtTrip) >= defaultStartDate);
            }

            if (!String.IsNullOrEmpty(unit) && unit != "all")
            {
                crLogTrips = crLogTrips.Where(c => c.crLogUnit.Description == unit);
            }

            if (!driver.IsNullOrWhiteSpace() && driver != "all")
            {
                crLogTrips = crLogTrips.Where(c => c.crLogDriver.Name == driver);
            }

            if (!company.IsNullOrWhiteSpace() && company != "all")
            {
                crLogTrips = crLogTrips.Where(c => c.crLogCompany.Name == company);
            }

            //Sorting
            switch (sortby)
            {
                case "Unit":
                    crLogTrips = crLogTrips.OrderBy(c => c.crLogUnit.Description).ThenBy(c => DbFunctions.TruncateTime(c.DtTrip)).ThenBy(c => c.crLogCompany.Name).ThenBy(c => c.crLogDriver.OrderNo);
                    break;
                case "Company":
                    crLogTrips = crLogTrips.OrderBy(c => c.crLogCompany.Name).ThenBy(c => DbFunctions.TruncateTime(c.DtTrip)).ThenBy(c => c.crLogDriver.OrderNo);
                    break;
                case "Driver":
                    crLogTrips = crLogTrips.OrderBy(c => c.crLogDriver.OrderNo).ThenBy(c => DbFunctions.TruncateTime(c.DtTrip));
                    break;
                case "Date":
                    crLogTrips = crLogTrips.OrderBy(c => DbFunctions.TruncateTime(c.DtTrip)).ThenBy(c => c.crLogCompany.Name).ThenBy(c => c.crLogDriver.OrderNo);
                    break;
                case "Date-Desc":
                    crLogTrips = crLogTrips.OrderByDescending(c => DbFunctions.TruncateTime(c.DtTrip)).ThenBy(c => c.crLogCompany.Name).ThenBy(c => c.crLogDriver.OrderNo);
                    break;
                default:
                    crLogTrips = crLogTrips.OrderBy(c => DbFunctions.TruncateTime(c.DtTrip)).ThenBy(c => c.crLogCompany.Name)
                        .ThenBy(c => c.crLogUnit.OrderNo).ThenBy(c => c.crLogUnit.Description).ThenBy(c => c.crLogDriver.OrderNo);
                    break;
            }

            HttpCookie shuttle_cookie = HttpContext.Request.Cookies.Get("shuttle_cookie");

            if (shuttle_cookie != null)
            {
                if (shuttle_cookie.Value == "1")
                {
                    crLogTrips = crLogTrips.Where(c => c.crLogCompany.IsShuttle);
                }
            }

            return crLogTrips.ToList();

        }

        //override
        //with owner filter
        public List<crLogTrip> GetTripLogs(string startDate, string endDate, string unit, string driver, string company, string sortby, string owner)
        {
            var defaultStartDate = dt.GetCurrentDate();
            //Get Logs
            var crLogTrips = db.crLogTrips.Include(c => c.crLogDriver).Include(c => c.crLogUnit).Include(c => c.crLogCompany).Include(c => c.crLogClosing);

            //Filter
            if (!startDate.IsNullOrWhiteSpace() && !endDate.IsNullOrWhiteSpace())
            {
                var sdate = DateTime.ParseExact(startDate, "MM/dd/yyyy", CultureInfo.InvariantCulture).Date;
                var edate = DateTime.ParseExact(endDate, "MM/dd/yyyy", CultureInfo.InvariantCulture).Date;

                crLogTrips = crLogTrips.Where(c => DbFunctions.TruncateTime(c.DtTrip) >= sdate && DbFunctions.TruncateTime(c.DtTrip) <= edate);
            }
            else
            {
                crLogTrips = crLogTrips.Where(c => DbFunctions.TruncateTime(c.DtTrip) >= defaultStartDate);
            }

            if (!String.IsNullOrEmpty(unit) && unit != "all")
            {
                crLogTrips = crLogTrips.Where(c => c.crLogUnit.Description == unit);
            }

            if (!driver.IsNullOrWhiteSpace() && driver != "all")
            {
                crLogTrips = crLogTrips.Where(c => c.crLogDriver.Name == driver);
            }

            if (!company.IsNullOrWhiteSpace() && company != "all")
            {
                crLogTrips = crLogTrips.Where(c => c.crLogCompany.Name == company);
            }

            if (!owner.IsNullOrWhiteSpace() && owner != "all")
            {
                crLogTrips = crLogTrips.Where(c => c.crLogUnit.crLogOwner.Name == owner);
            }


            //Sorting
            switch (sortby)
            {
                case "Unit":
                    crLogTrips = crLogTrips.OrderBy(c => c.crLogUnit.Description).ThenBy(c => DbFunctions.TruncateTime(c.DtTrip)).ThenBy(c => c.crLogCompany.Name).ThenBy(c => c.crLogDriver.OrderNo);
                    break;
                case "Company":
                    crLogTrips = crLogTrips.OrderBy(c => c.crLogCompany.Name).ThenBy(c => DbFunctions.TruncateTime(c.DtTrip)).ThenBy(c => c.crLogDriver.OrderNo);
                    break;
                case "Driver":
                    crLogTrips = crLogTrips.OrderBy(c => c.crLogDriver.OrderNo).ThenBy(c => DbFunctions.TruncateTime(c.DtTrip));
                    break;
                case "Date":
                    crLogTrips = crLogTrips.OrderBy(c => DbFunctions.TruncateTime(c.DtTrip)).ThenBy(c => c.crLogCompany.Name).ThenBy(c => c.crLogDriver.OrderNo);
                    break;
                case "Date-Desc":
                    crLogTrips = crLogTrips.OrderByDescending(c => DbFunctions.TruncateTime(c.DtTrip)).ThenBy(c => c.crLogCompany.Name).ThenBy(c => c.crLogDriver.OrderNo);
                    break;
                default:
                    crLogTrips = crLogTrips.OrderBy(c => DbFunctions.TruncateTime(c.DtTrip)).ThenBy(c => c.crLogCompany.Name)
                        .ThenBy(c => c.crLogUnit.OrderNo).ThenBy(c => c.crLogUnit.Description).ThenBy(c => c.crLogDriver.OrderNo);
                    break;
            }

            HttpCookie shuttle_cookie = HttpContext.Request.Cookies.Get("shuttle_cookie");

            if (shuttle_cookie != null)
            {
                if (shuttle_cookie.Value == "1")
                {
                    crLogTrips = crLogTrips.Where(c => c.crLogCompany.IsShuttle);
                }
            }

            return crLogTrips.ToList();

        }


        public CrLogSummary GetCrLogSummary(List<crLogTrip> tripLogs)
        {
            CrLogSummary logSummary = new CrLogSummary();

            logSummary.CrDrivers = GetDriverLogs(logSummary, tripLogs);
            logSummary.CrCompanies = GetCompanyLogs(logSummary, tripLogs);
            logSummary.CrUnits = GetUnitLogs(logSummary, tripLogs);

            return logSummary;
        }

        public List<CrDriverLogs> GetDriverLogs(CrLogSummary logSummary, List<crLogTrip> tripLogs)
        {
            foreach (var trip in tripLogs)
            {
                //Driver Logs 
                if (logSummary.CrDrivers == null)
                    logSummary.CrDrivers = new List<CrDriverLogs>();

                //check if log for the driver exists
                if (logSummary.CrDrivers.Where(c => c.DriversId == trip.crLogDriverId).FirstOrDefault() == null)
                {
                    //if does not exist

                    CrDriverLogs driverLog = new CrDriverLogs();
                    driverLog.DriversId = trip.crLogDriverId;
                    driverLog.Driver = trip.crLogDriver.Name;
                    driverLog.JobCount = 1;
                    driverLog.TotalDriverFee = trip.DriverFee;

                    logSummary.CrDrivers.Add(driverLog);
                }
                else
                {
                    //if driver log exist
                    //find the log and update
                    var driverLog = logSummary.CrDrivers.Where(c => c.DriversId == trip.crLogDriverId).FirstOrDefault();
                    driverLog.JobCount += 1;
                    driverLog.TotalDriverFee += trip.DriverFee;
                }
            }

            return logSummary.CrDrivers;
        }

        public List<CrCompanyLogs> GetCompanyLogs(CrLogSummary logSummary, List<crLogTrip> tripLogs)
        {

            foreach (var trip in tripLogs)
            {
                //Driver Logs 
                if (logSummary.CrCompanies == null)
                    logSummary.CrCompanies = new List<CrCompanyLogs>();

                //check if log for the driver exists
                if (logSummary.CrCompanies.Where(c => c.CompanyId == trip.crLogCompanyId).FirstOrDefault() == null)
                {
                    //if does not exist

                    CrCompanyLogs companyLogs = new CrCompanyLogs();
                    companyLogs.CompanyId = trip.crLogCompanyId;
                    companyLogs.Company = trip.crLogCompany.Name;
                    companyLogs.JobCount = 1;
                    companyLogs.TotalAmount = trip.Rate + trip.Addon;

                    logSummary.CrCompanies.Add(companyLogs);
                }
                else
                {
                    //if driver log exist
                    //find the log and update
                    var companyLogs = logSummary.CrCompanies.Where(c => c.CompanyId == trip.crLogCompanyId).FirstOrDefault();
                    companyLogs.JobCount += 1;
                    companyLogs.TotalAmount += trip.Rate + trip.Addon;
                }
            }

            return logSummary.CrCompanies;
        }

        public List<CrUnitLogs> GetUnitLogs(CrLogSummary logSummary, List<crLogTrip> tripLogs)
        {

            foreach (var trip in tripLogs)
            {
                //Driver Logs 
                if (logSummary.CrUnits == null)
                    logSummary.CrUnits = new List<CrUnitLogs>();

                //check if log for the driver exists
                if (logSummary.CrUnits.Where(c => c.UnitId == trip.crLogUnitId).FirstOrDefault() == null)
                {
                    //if does not exist

                    CrUnitLogs unitLogs = new CrUnitLogs();
                    unitLogs.UnitId = trip.crLogUnitId;
                    unitLogs.Unit = trip.crLogUnit.Description;
                    unitLogs.JobCount = 1;
                    unitLogs.TotalAmount = trip.Rate + trip.Addon;

                    logSummary.CrUnits.Add(unitLogs);
                }
                else
                {
                    //if driver log exist
                    //find the log and update
                    var unitLogs = logSummary.CrUnits.Where(c => c.UnitId == trip.crLogUnitId).FirstOrDefault();
                    unitLogs.JobCount += 1;
                    unitLogs.TotalAmount += trip.Rate + trip.Addon;
                }
            }

            return logSummary.CrUnits;
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

            ViewBag.crLogDriverId = new SelectList(dl.GetDrivers(), "Id", "Name");
            ViewBag.crLogUnitId = new SelectList(dl.GetUnits(), "Id", "Description");
            ViewBag.crLogCompanyId = new SelectList(dl.GetCompanies(), "Id", "Name");
            //ViewBag.crLogClosingId = new SelectList(db.crLogClosings, "Id", "Id");
            return View(trip);
        }

        // POST: Personel/CarRentalLog/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,crLogDriverId,crLogUnitId,crLogCompanyId,DtTrip,Rate,Addon,Expenses,DriverFee,Remarks, OdoStart, OdoEnd, DriverOt, TripHours, StartTime, EndTime, OTRate, DriverOTRate, AddonOT, IsFinal, AllowEdit")] crLogTrip crLogTrip)
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
                //crLogTrip.OTRate = 200;
            }


            if (crLogTrip.DriverOTRate == null)
            {
                //crLogTrip.DriverOTRate = 50;
            }

            ViewBag.crLogDriverId = new SelectList(dl.GetDrivers(), "Id", "Name", crLogTrip.crLogDriverId);
            ViewBag.crLogUnitId = new SelectList(dl.GetUnits(), "Id", "Description", crLogTrip.crLogUnitId);
            ViewBag.crLogCompanyId = new SelectList(dl.GetCompanies(), "Id", "Name", crLogTrip.crLogCompanyId);
            //ViewBag.crLogClosingId = new SelectList(db.crLogClosings, "Id", "Id", crLogTrip.crLogClosingId);
            return View(crLogTrip);
        }

        // POST: Personel/CarRentalLog/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,crLogDriverId,crLogUnitId,crLogCompanyId,DtTrip,Rate,Addon,Expenses,DriverFee,Remarks, OdoStart, OdoEnd, crLogClosingId, DriverOt, TripHours, StartTime, EndTime, OTRate, DriverOTRate, AddonOT, IsFinal, AllowEdit")] crLogTrip crLogTrip)
        {
            if (ModelState.IsValid)
            {
                //caculate OT
                var OTRate = GetTripLogOTRate(crLogTrip);
                if (OTRate > 0 && crLogTrip.DriverOT == 0)
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
            ViewBag.crLogDriverId = new SelectList(dl.GetDrivers(), "Id", "Name", crLogTrip.crLogDriverId);
            ViewBag.crLogUnitId = new SelectList(dl.GetUnits(), "Id", "Description", crLogTrip.crLogUnitId);
            ViewBag.crLogCompanyId = new SelectList(dl.GetCompanies(), "Id", "Name", crLogTrip.crLogCompanyId);
            //ViewBag.crLogClosingId = new SelectList(db.crLogClosings, "Id", "Id", crLogTrip.crLogClosingId);
            return View(crLogTrip);
        }



        // GET: Personel/CarRentalLog/Edit/5
        public ActionResult DriverSummaryTripEdit(int? id)
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
            ViewBag.crLogDriverId = new SelectList(dl.GetDrivers(), "Id", "Name", crLogTrip.crLogDriverId);
            ViewBag.crLogUnitId = new SelectList(dl.GetUnits(), "Id", "Description", crLogTrip.crLogUnitId);
            ViewBag.crLogCompanyId = new SelectList(dl.GetCompanies(), "Id", "Name", crLogTrip.crLogCompanyId);
            //ViewBag.crLogClosingId = new SelectList(db.crLogClosings, "Id", "Id", crLogTrip.crLogClosingId);
            return View(crLogTrip);
        }

        // POST: Personel/CarRentalLog/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DriverSummaryTripEdit([Bind(Include = "Id,crLogDriverId,crLogUnitId,crLogCompanyId,DtTrip,Rate,Addon,Expenses,DriverFee,Remarks, OdoStart, OdoEnd, crLogClosingId, DriverOt, TripHours, StartTime, EndTime")] crLogTrip crLogTrip)
        {
            if (ModelState.IsValid)
            {
                db.Entry(crLogTrip).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("DriverSummary", new { id = crLogTrip.crLogDriverId });
            }
            ViewBag.crLogDriverId = new SelectList(dl.GetDrivers(), "Id", "Name", crLogTrip.crLogDriverId);
            ViewBag.crLogUnitId = new SelectList(dl.GetUnits(), "Id", "Description", crLogTrip.crLogUnitId);
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
            //remove passengers from trip
            DeleteTripPassengers(id);

            //delete trip record
            crLogTrip crLogTrip = db.crLogTrips.Find(id);
            db.crLogTrips.Remove(crLogTrip);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //Param: id = TripId
        private bool DeleteTripPassengers(int? id)
        {
            try
            {
                if (id != null)
                {
                    var tripPass = db.crLogPassengers.Where(p => p.crLogTripId == id).ToList();
                    if (tripPass.Count() > 0)
                    {
                        db.crLogPassengers.RemoveRange(tripPass);
                    }

                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }


        // GET: Personel/CarRentalLog/EditCashTrx/5
        public ActionResult EditCashTrx(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            crLogCashRelease crLogCashRelease = db.crLogCashReleases.Find(id);
            if (crLogCashRelease == null)
            {
                return HttpNotFound();
            }
            ViewBag.crLogDriverId = new SelectList(dl.GetDrivers(), "Id", "Name", crLogCashRelease.crLogDriverId);
            ViewBag.crLogClosingId = new SelectList(db.crLogClosings, "Id", "Id", crLogCashRelease.crLogClosingId);
            ViewBag.crLogCashTypeId = new SelectList(db.crLogCashTypes, "Id", "Description", crLogCashRelease.crLogCashTypeId);
            return View(crLogCashRelease);
        }

        // POST: Personel/CarRentalLog/EditCashTrx/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditCashTrx([Bind(Include = "Id,DtRelease,Amount,Remarks,crLogDriverId,crLogClosingId,crLogCashTypeId")] crLogCashRelease crLogCashRelease)
        {
            if (ModelState.IsValid)
            {
                db.Entry(crLogCashRelease).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("DriverSummary", new { id = crLogCashRelease.crLogDriverId });
            }
            ViewBag.crLogDriverId = new SelectList(dl.GetDrivers(), "Id", "Name", crLogCashRelease.crLogDriverId);
            ViewBag.crLogClosingId = new SelectList(db.crLogClosings, "Id", "Id", crLogCashRelease.crLogClosingId);
            ViewBag.crLogCashTypeId = new SelectList(db.crLogCashTypes, "Id", "Description", crLogCashRelease.crLogCashTypeId);
            return View(crLogCashRelease);
        }

        
        // GET: Personel/CarRentalLog/EditCashTrx/5
        public ActionResult CloseCashTrx(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            crLogCashRelease crLogCashRelease = db.crLogCashReleases.Find(id);
            if (crLogCashRelease == null)
            {
                return HttpNotFound();
            }
            try
            {
                //create closing Id

                crLogCashRelease.crLogClosingId = generateClosingTrx();
                db.Entry(crLogCashRelease).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("DriverSummary", new { id = crLogCashRelease.crLogDriverId });
            }
            catch
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        public bool CloselogCashRelease(crLogCashRelease crLogCashRelease)
        {
            if (crLogCashRelease == null)
            {
                return false;
            }
            try
            {
                //create closing Id
                crLogCashRelease.crLogClosingId = generateClosingTrx();
                db.Entry(crLogCashRelease).State = EntityState.Modified;
                db.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        /*  id = DriverId
         *  reqStatus = 1 ( Salary Request added)
         *              2 ( CA Request added)
         *              3 ( Payment added)
         *              4 ( Closed All CA & Payments Success )
         *              5 ( Closed All CA & Payments Error )
         * 
         */
        public ActionResult DriverSummary(int id, int? reqStatus, string DtStart, string DtEnd, int? rptId)
        {
            DateTime sDate = new DateTime();
            DateTime eDate = new DateTime();
            var today = dt.GetCurrentDate();

            crLogDriver driver = db.crLogDrivers.Find(id);
            List<crLogTrip> trips = db.crLogTrips.Where(d => d.crLogDriverId == id && d.crLogClosingId==null).OrderBy(s=>s.DtTrip).ToList();

            if (DateTime.TryParse(DtStart, out sDate) && DateTime.TryParse(DtEnd, out eDate))
            {
                TimeSpan duration = new TimeSpan(23, 59, 0); //11:59:0 PM
                eDate = eDate.Add(duration);

                trips = trips.Where(d => d.DtTrip >= sDate && d.DtTrip <= eDate).ToList();
            }


            //if report is generated from unit expenses reports
            if (rptId != null)
            {
                //get unitId List on report
                var unitReport = db.crRptUnitExpenses.Find(rptId);
                if (unitReport == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                var unitList = unitReport.CrRptUnits.Select(c => c.crLogUnitId).ToList();

                trips = trips.Where(t => unitList.Contains(t.crLogUnitId)).ToList();
            }

            var driverCashReleases = db.crLogCashReleases;
            List<crLogCashRelease> cashAdvance = driverCashReleases.Where(d => d.crLogDriverId == id && d.crLogClosing == null && d.crLogCashTypeId == 2).OrderBy(s=>s.DtRelease).ToList();
            List<crLogCashRelease> payments = driverCashReleases.Where(d => d.crLogDriverId == id && d.crLogClosing == null && d.crLogCashTypeId == 3).OrderBy(s => s.DtRelease).ToList();
            List<crLogCashRelease> contributions = driverCashReleases.Where(d => d.crLogDriverId == id && d.crLogClosing == null && d.crLogCashTypeId == 4).OrderBy(s => s.DtRelease).ToList();
            List<crLogCashRelease> noStatus = driverCashReleases.Where(d => d.crLogDriverId == id && d.crLogClosing == null && d.crLogCashTypeId == 1).OrderBy(s => s.DtRelease).ToList();
            List<crLogCashRelease> cashtrx = driverCashReleases.Where(d => d.crLogDriverId == id && d.crLogClosing == null && d.crLogCashTypeId != 1).OrderBy(s => s.DtRelease).ToList();


            crDriverSummary driversummary = new crDriverSummary();
            driversummary.Driver = driver;
            driversummary.DriverTrips = trips;
            driversummary.DriverCash = cashAdvance;
            driversummary.DriverPayments = payments;
            driversummary.NoStatus = noStatus;
            driversummary.DriverTrx = cashtrx;
            driversummary.DriverContributions = contributions;

            var rptName = "";
            if (rptId != null)
            {
                rptName = db.crRptUnitExpenses.Find(rptId).RptName;
            }

            ViewBag.rptId = rptId ?? 0;
            ViewBag.rptName = rptName;
            ViewBag.DtStart = DtStart;
            ViewBag.DtEnd = DtEnd;
            ViewBag.DriverId = id;
            ViewBag.crLogDriverId = new SelectList(db.crLogDrivers.OrderBy(d=>d.OrderNo), "Id", "Name", id);
            ViewBag.reqStatus = reqStatus ?? 0;
            ViewBag.IsAdmin = User.IsInRole("Admin");

            return View(driversummary);
        }

        public ActionResult CloseCashBalance(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            crLogDriver driver = db.crLogDrivers.Find(id);

            //Get CA 
            List<crLogCashRelease> cashAdvance = db.crLogCashReleases
                .Where(d => d.crLogDriverId == id && d.crLogClosing == null && d.crLogCashTypeId == 2)
                .OrderBy(s => s.DtRelease).ToList();
            //Get Payments
            List<crLogCashRelease> payments = db.crLogCashReleases
                .Where(d => d.crLogDriverId == id && d.crLogClosing == null && d.crLogCashTypeId == 3)
                .OrderBy(s => s.DtRelease).ToList();
            //Get Cash
            List<crLogCashRelease> cashtrx = db.crLogCashReleases
                .Where(d => d.crLogDriverId == id && d.crLogClosing == null && d.crLogCashTypeId != 1)
                .OrderBy(s => s.DtRelease).ToList();

            decimal totalCA = 0;
            decimal totalPayments = 0;
            //decimal Balance = 0;

            foreach (var cash in cashAdvance)
            {
                totalCA += cash.Amount;
            }

            foreach (var cash in payments)
            {
                totalPayments += cash.Amount;
            }

            //when balance is Payments >= totalCA
            if (totalPayments >= totalCA)
            {
                //close all trx
                foreach (var trx in cashtrx)
                {
                    //close
                    var result = CloselogCashRelease(trx);

                    if (!result)
                    {
                        return RedirectToAction("DriverSummary", new {  id, reqStatus = 5 });
                    }
                }
            }

            return RedirectToAction("DriverSummary", new { id, reqStatus = 4 });
        }

        public ActionResult PaytoClose(int id, decimal amount)
        {
            crLogDriver driver = db.crLogDrivers.Find(id);
            List<crLogTrip> trips = db.crLogTrips.Where(d => d.crLogDriverId == id && d.crLogClosingId == null).ToList();
            List<crLogCashRelease> cashtrx = db.crLogCashReleases.Where(d => d.crLogDriverId == id && d.crLogClosingId == null).ToList();

            string sErrorMessage = "";
            decimal _tripAmt = 0;
            decimal _cashAmt = 0;
            decimal _paytoCloseAmt = 0;
            foreach (var item in trips)
            {
                _tripAmt += item.DriverFee;
            }
            foreach(var item in cashtrx)
            {
                _cashAmt += item.Amount;
            }
            _paytoCloseAmt = _tripAmt - _cashAmt;

            if (_paytoCloseAmt != amount)
                sErrorMessage = "Amount is not valid to close the transactions.";


            int closingId = this.generateClosingTrx();
            decimal _tripAmt2 = 0;
            try
            {
                foreach (var item in trips)
                {
                    _tripAmt2 += item.DriverFee;
                    item.crLogClosingId = closingId;
                }
                if (_tripAmt2 == _tripAmt)
                    db.SaveChanges();
                else
                    sErrorMessage = "Amount in Trips has changed and not ready for closing. please try again.";
            }
            catch(Exception e)
            {
                sErrorMessage = "Unable to finish closing the trips logs \n\n" + e.Message;
            }

            decimal _cashAmt2 = 0;
            try
            {
                foreach (var item in cashtrx)
                {
                    _cashAmt2 += item.Amount;
                    item.crLogClosingId = closingId;
                }
                if (_cashAmt2 == _cashAmt)
                    db.SaveChanges();
                else
                    sErrorMessage = "Amount in Cash Trx has changed and not ready for closing. please try again.";
            }
            catch (Exception e)
            {
                sErrorMessage = "Unable to finish closing cash trasactions \n\n" + e.Message;
            }


            if (sErrorMessage.Trim() == "")
            {
                ViewBag.ReturnCode = "1";
                ViewBag.ReturnMessage = "Closing successfull...";
            }
            else
            {
                ViewBag.ReturnCode = "-1";
                ViewBag.ReturnMessage = sErrorMessage;
            }

            ViewBag.DriverId = id;
            return View();
        }

        int generateClosingTrx()
        {
            crLogClosing ctrx = new crLogClosing()
            { dtClose = System.DateTime.Now };

            db.crLogClosings.Add(ctrx);
            db.SaveChanges();

            return ctrx.Id;
        }

        public ActionResult DriverTrxHistory(int? id)
        {
            if (id != null)
            {
                var cashLogs = db.crLogCashReleases.Where(c=> c.crLogDriverId == id).ToList();
                
                ViewBag.DriverId = id;
                ViewBag.Driver = db.crLogDrivers.Find(id).Name;

                return View(cashLogs.OrderByDescending(c=>c.DtRelease));
            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        public ActionResult DriverExpenseHistory(int? id)
        {
            if (id != null)
            {
                var expenseLogs = db.crLogFuels.Where(c => c.crLogDriverId == id).ToList();

                var crLogFuels = db.crLogFuels.Include(c => c.crLogUnit).Include(c => c.crLogDriver)
                    .Where(c => c.crLogDriverId == id).OrderBy(c => c.dtRequest);

                List<cCrLogFuel> cCrLogFuel = new List<cCrLogFuel>();

                foreach (var log in crLogFuels.ToList())
                {
                    var status = db.crCashReqStatus.Find(getLatestStatusId(log.Id)).Status;

                    var templog = new Models.cCrLogFuel()
                    {
                        crLogFuel = log,
                        LatestStatusId = getLatestStatusId(log.Id),
                        LatestStatus = status
                    };

                    if (templog.LatestStatusId == 4)
                        cCrLogFuel.Add(templog);
                }


                ViewBag.IsAdmin = User.IsInRole("Admin");
                ViewBag.DriverId = id;
                ViewBag.Driver = db.crLogDrivers.Find(id).Name;

                return View(cCrLogFuel.OrderByDescending(c => c.crLogFuel.dtRequest).ToList());
            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
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
                        crLogTrips = crLogTrips.Where(c => c.crLogCompanyId == companyId );
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


        public bool CopyTripSubmit_old(int? id, string copyDate)
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

                return true;
            }
            catch
            {
                
                return false;
            }
        }

        public bool CopyTripSubmit(int?  id, string copyDate, int? copyPassengers)
        {
            try
            {
                if (copyDate.IsNullOrWhiteSpace() || id == null)
                {
                    return false;
                }

                var cDate = DateTime.ParseExact(copyDate , "MM/dd/yyyy", CultureInfo.InvariantCulture);

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


        public IQueryable<crLogTrip> GetFilteredTripLogs(string startDate, string endDate, string unit, string driver, string company, string sortby)
        {
            //Get Logs
            var crLogTrips = db.crLogTrips.Include(c => c.crLogDriver).Include(c => c.crLogUnit).Include(c => c.crLogCompany).Include(c => c.crLogClosing);

            //Filter
            if (!startDate.IsNullOrWhiteSpace() && !endDate.IsNullOrWhiteSpace())
            {
                var sdate = DateTime.ParseExact(startDate, "MM/dd/yyyy", CultureInfo.InvariantCulture).Date;
                var edate = DateTime.ParseExact(endDate, "MM/dd/yyyy", CultureInfo.InvariantCulture).Date;

                crLogTrips = crLogTrips.Where(c => DbFunctions.TruncateTime(c.DtTrip) >= sdate && DbFunctions.TruncateTime(c.DtTrip) <= edate);
            }

            if (!String.IsNullOrEmpty(unit) && unit != "all")
            {
                crLogTrips = crLogTrips.Where(c => c.crLogUnit.Description == unit);
            }
            else
            {
                crLogTrips = crLogTrips.OrderBy(c => c.crLogUnit.Description);
            }

            if (!driver.IsNullOrWhiteSpace() && driver != "all")
            {
                crLogTrips = crLogTrips.Where(c => c.crLogDriver.Name == driver);
            }
            else
            {
                crLogTrips = crLogTrips.OrderBy(c => c.crLogDriver.Name);
            }

            if (!company.IsNullOrWhiteSpace() && company != "all")
            {
                crLogTrips = crLogTrips.Where(c => c.crLogCompany.Name == company);
            }
            else
            {
                crLogTrips = crLogTrips.OrderBy(c => c.crLogCompany.Name);
            }

            //Sorting
            switch (sortby)
            {
                case "Unit":
                    crLogTrips = crLogTrips.OrderBy(c => c.crLogUnit.Description);
                    break;
                case "Company":
                    crLogTrips = crLogTrips.OrderBy(c => c.crLogCompany.Name);
                    break;
                case "Driver":
                    crLogTrips = crLogTrips.OrderBy(c => c.crLogDriver.Name);
                    break;
                case "Date":
                    crLogTrips = crLogTrips.OrderBy(c => c.DtTrip);
                    break;
                case "Date-Desc":
                    crLogTrips = crLogTrips.OrderByDescending(c => c.DtTrip);
                    break;
                default:
                    crLogTrips = crLogTrips.OrderBy(c => c.DtTrip);
                    break;
            }

           return crLogTrips;

        }


        public int getLatestStatusId(int id)
        {
            var logStatusQuery = db.crLogFuelStatus.Where(c => c.crLogFuelId == id).OrderByDescending(c => c.Id).FirstOrDefault();

            if (logStatusQuery != null)
            {
                return logStatusQuery.crCashReqStatusId;
            }
            else
            {
                //default 
                return 1;
            }
        }

        // GET: Personel/CarRentalLog/RequestPOFuel
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

        //POST : /Personel/CarRentalLog/AllowEditTripLog
        //PARAMETER: id (crLogTrips.Id as nullable int)
        //RETURNS: Boolean (True=Success/False=Error or null)
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

        // GET: /Areas/Personal/CarRentalLog/GetDriverIsInTripToday?driverId={driverId}
        // Returns TRUE if Driver have trip for today,
        // Returnds FALSE if Driver does not have trip for today
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
        // Returns TRUE if Unit/Vehicle have trip for today,
        // Returnds FALSE if Unit/Vehicle does not have trip for today
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


        #region OT 

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

                var OTRate = GetTripLogOTRate(trip);
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

        //GET : /Personel/CarRentalLog/GetTripOTHours/{TripLogId}
        public double GetTripOTHours(int? id)
        {
            if (id == null)
            {
                return 0;
            }

            var trip = db.crLogTrips.Find(id);

            if (trip.StartTime != null && trip.EndTime != null && trip.TripHours != null)
            {
                DateTime StartTime = DateTime.Parse(trip.StartTime);
                DateTime EndTime = DateTime.Parse(trip.EndTime);

                var min = StartTime.Minute;

                //StartTime = convertDateTime(StartTime);
                //EndTime = convertDateTime(EndTime);


                // if night shift
                if (StartTime.Hour > EndTime.Hour)
                {
                    EndTime = EndTime.AddDays(1);
                }

                int HoursPerTrip = trip.TripHours ?? 0;

                //calculate time diff and hours per trip
                double diff = double.Parse((EndTime - StartTime).TotalHours.ToString()) - (double)HoursPerTrip;

                //round off time diff
                diff = ConvertHrsDec(diff);

                //disregard negative time differences
                if (diff < 0)
                {
                    return 0;
                }

                return diff;

            }

            return 0;
        }


        public double GetTripLogOTHours(crLogTrip trip)
        {
            if (trip == null)
            {
                return 0;
            }


            if (trip.StartTime != null && trip.EndTime != null && trip.TripHours != null)
            {
                DateTime StartTime = DateTime.Parse(trip.StartTime);
                DateTime EndTime = DateTime.Parse(trip.EndTime);

                var min = StartTime.Minute;

                //StartTime = convertDateTime(StartTime);
                //EndTime = convertDateTime(EndTime);


                // if night shift
                if (StartTime.Hour > EndTime.Hour)
                {
                    EndTime = EndTime.AddDays(1);
                }

                int HoursPerTrip = trip.TripHours ?? 0;

                //calculate time diff and hours per trip
                double diff = double.Parse((EndTime - StartTime).TotalHours.ToString()) - (double)HoursPerTrip;

                //round off time diff
                diff = ConvertHrsDec(diff);

                //disregard negative time differences
                if (diff < 0)
                {
                    return 0;
                }

                return diff;

            }

            return 0;
        }

        private DateTime convertDateTime(DateTime time)
        {
            var timeTemp = time;

            //round off time start
            if (time.Minute > 40)
            {
                time = time.AddMinutes(60 - time.Minute);
            }
            else if (time.Minute >= 20 && time.Minute <= 40)
            {
                time = time.AddMinutes(30 - time.Minute);
            }
            else
            {
                time = time.AddMinutes(0 - time.Minute);
            }

            return time;
        }

        //Round Off time Difference in Hours
        // if timediff is greater than 0.833 (50 mins), round off to 1 (1 hour)
        //                greater than 0.333 (20 mins) or less than 0.833 (50 mins), round off to 0.5 ()
        //                less than 0.333 (20 mins), do not round off
        private double ConvertHrsDec(double hrsDiff)
        {
            double hrsDiffTemp = (double)Math.Truncate(hrsDiff);
            //round off time start

            double decPart = hrsDiff - (double)Math.Truncate(hrsDiff);

            if (decPart > 0.833)
            {
                hrsDiffTemp = hrsDiffTemp + 1;
            }
            else if (decPart >= 0.333 && decPart <= 0.833)
            {
                hrsDiffTemp = hrsDiffTemp + 0.5;
            }

            return hrsDiffTemp;
        }


        //GET : rate of OT per hour based on OTRate for Driver
        public double GetTripOTRate(int? id)
        {

            if (id == null)
            {
                return 0;
            }

            double OTHours = GetTripOTHours(id);
            double CalcOTRate = 0;

            var trip = db.crLogTrips.Find(id);

            if (trip.DriverOTRate != null)
            {
                CalcOTRate = OTHours * Double.Parse(trip.DriverOTRate.ToString());
            }
            else
            {
                //default 50 per hour overtime rate
                CalcOTRate = OTHours * 50;
            }


            return CalcOTRate;
        }

        //GET : rate of OT per hour based on OTRate for Driver
        public double GetTripLogOTRate(crLogTrip trip)
        {

            if (trip == null)
            {
                return 0;
            }

            double OTHours = GetTripLogOTHours(trip);
            double CalcOTRate = 0;


            if (trip.DriverOTRate != null)
            {
                CalcOTRate = OTHours * Double.Parse(trip.DriverOTRate.ToString());
            }
            else
            {
                //default 50 per hour overtime rate
                CalcOTRate = OTHours * 50;
            }


            return CalcOTRate;
        }


        //GET : rate of OT per hour based on OTRate for Driver
        public double GetTripOTCompanyRate(int? id)
        {

            if (id == null)
            {
                return 0;
            }

            double OTHours = GetTripOTHours(id);
            double CalcOTRate = 0;

            var trip = db.crLogTrips.Find(id);

            if (trip.OTRate != null)
            {
                CalcOTRate = OTHours * Double.Parse(trip.OTRate.ToString());
            }
            else
            {
                //default 50 per hour overtime rate
                CalcOTRate = OTHours * 200;
            }


            return CalcOTRate;
        }

        //GET : rate of OT per hour based on OTRate for Driver
        public decimal GetTripLogOTCompanyRate(crLogTrip trip, double OTHours)
        {

            if (trip == null)
            {
                return 0;
            }

            decimal CalcOTRate = 0;


            if (trip.OTRate != null)
            {
                CalcOTRate = (decimal)OTHours * (decimal)trip.OTRate;
            }
            else
            {
                //default 200 per hour overtime rate
                CalcOTRate = (decimal)OTHours * 200;
            }

            return CalcOTRate;
        }


        //GET : rate of OT per hour based on OTRate for Company
        public double GetTripOTAddon(int? id)
        {
            if (id == null)
            {
                return 0;
            }

            double OTHours = GetTripOTHours(id);
            double CalcOTRate = 0;

            var trip = db.crLogTrips.Find(id);

            if (trip.OTRate != null)
            {
                CalcOTRate = OTHours * Double.Parse(trip.OTRate.ToString());
            }

            return CalcOTRate;
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

            //get trip log
            var triplog = db.crLogTrips.Find(id);

            TripOTRequest odoDetails = new TripOTRequest();
            odoDetails.Date = triplog.DtTrip.ToShortDateString();
            odoDetails.Unit = triplog.crLogUnit.Description;
            odoDetails.Driver = triplog.crLogDriver.Name;
            odoDetails.Company = triplog.crLogCompany.Name;

            odoDetails.StartTime = triplog.StartTime;
            odoDetails.EndTime = triplog.EndTime;
            odoDetails.TripHours = triplog.TripHours ?? 10;
            odoDetails.OTRate = triplog.OTRate ?? 200;
            odoDetails.DriverOTRate = triplog.DriverOTRate ?? 50;


            return Json(odoDetails, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public bool SetTripOT(int? Id, string StartTime, string EndTime, int? TripHours, Decimal? OTRate, Decimal? DriverOTRate)
        {
            if (Id != null)
            {
                crLogTrip crLogTrip = db.crLogTrips.Find(Id);

                if (crLogTrip == null)
                    return false;

                //update odo values
                crLogTrip.StartTime = StartTime;
                crLogTrip.EndTime = EndTime;
                crLogTrip.TripHours = TripHours;
                crLogTrip.OTRate = OTRate;
                crLogTrip.DriverOTRate = DriverOTRate;
                crLogTrip.DriverOT = (decimal)GetTripOTRate(Id);
                crLogTrip.AddonOT = (decimal)GetTripOTAddon(Id);

                //save changes
                db.Entry(crLogTrip).State = EntityState.Modified;
                db.SaveChanges();

                return true;
            }

            return false;
        }
        #endregion

        #region Odo Update

        // GET: Personel/CarRentalLog/EditOdo/5
        public ActionResult EditOdo(int? id)
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

        // POST: Personel/CarRentalLog/EditOdo/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.SetTripOT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditOdo(int? id, int? OdoStart, int? OdoEnd)
        {
            if (id != null)
            {

                crLogTrip crLogTrip = db.crLogTrips.Find(id);

                if (crLogTrip == null)
                    return HttpNotFound();

                //update odo values
                crLogTrip.OdoStart = OdoStart;
                crLogTrip.OdoEnd = OdoEnd;

                db.Entry(crLogTrip).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

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



        public bool CheckTripFinalizeRange(List<crLogTrip> trips)
        {
            try
            {
                var dateTimeNow = dt.GetCurrentDateTime();

                //18 == 6:00pm
                //finalize trips
                if (dateTimeNow.Hour >= 18)
                {
                    var result = false;
                    foreach (var trip in trips)
                    {
                       // result = crServices.SetTripFinal(trip);
                    }
                 
                    if (result)
                    {
                        //save
                        //crServices.SaveDbChanges();
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

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
        }

        #endregion

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
            var tripLogs = GetFilteredTripLogs(startDate, endDate, unit, driver, company, sortby).ToList();

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
            var tripLogs = GetFilteredTripLogs(startDate, endDate, unit, driver, company, sortby).ToList();

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
            var tripLogs = GetFilteredTripLogs(startDate, endDate, unit, driver, company, sortby).ToList();

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
        
        public ActionResult UpdateTripLogOdo()
        {
            ViewBag.crLogDriverId = new SelectList(dl.GetDrivers(), "Id", "Name");
            ViewBag.crLogUnitId = new SelectList(dl.GetUnits(), "Id", "Description");
            ViewBag.crLogCompanyId = new SelectList(dl.GetCompanies(), "Id", "Name");
            return View();
        }

        // POST: Personel/CarRentalLog/UpdateTripLogOdo/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateTripLogOdo(int crLogDriverId, int crLogUnitId, int crLogCompanyId, string Remarks, int? OdoStart, int? OdoEnd)
        {
            try
            {

                OdoStart = OdoStart ?? 0;
                OdoEnd = OdoEnd ?? 0;

                //check and find lastest trip
                var crLogTripLatest = GetTripLogLatestTrip(crLogDriverId, crLogUnitId, crLogCompanyId);
                if (crLogTripLatest == null)
                {
                    ModelState.AddModelError("OdoEnd", "No Trips Found");
                }else
                {
                    //update odo and remarks
                    UpdateTripLogOdoRemarks(crLogTripLatest, (int)OdoStart, (int)OdoEnd, Remarks);

                    return RedirectToAction("Index");
                }
            
            ViewBag.crLogDriverId = new SelectList(dl.GetDrivers(), "Id", "Name", crLogDriverId);
            ViewBag.crLogUnitId = new SelectList(dl.GetUnits(), "Id", "Description",  crLogUnitId);
            ViewBag.crLogCompanyId = new SelectList(dl.GetCompanies(), "Id", "Name", crLogCompanyId);
            return View(crLogTripLatest);

            }
            catch
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }


        #endregion


        #region CarRentalLogs Services

        //Check if Unit is encoded in trip logs for the selected date
        public bool GetUnitIsInTripByDateSvc(int unitId, DateTime? date)
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

        //Check if Driver is encoded in trip logs for the selected date
        public bool GetDriverIsInTripByDateSvc(int driverId, DateTime? date)
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

        public crLogTrip GetTripLogLatestTrip(int unitID, int driverID, int CompanyID)
        {
            return db.crLogTrips
                .Where(c => c.crLogUnitId == unitID && c.crLogDriverId == driverID && c.crLogCompanyId == CompanyID)
                .OrderByDescending(c => c.DtTrip).FirstOrDefault();
        }

        public int UpdateTripLogOdoRemarks(crLogTrip crLogTrip, int OdoStart, int OdoEnd, string Remarks)
        {
            try
            {

                crLogTrip.OdoStart = OdoStart;
                crLogTrip.OdoEnd = OdoEnd;
                crLogTrip.Remarks = Remarks;

                db.Entry(crLogTrip).State = EntityState.Modified;
                return db.SaveChanges();

            }
            catch
            {
                return 0;
            }
        }

        #endregion

        #region CarRentalLogs APIs

        //POST: CarRentalLog/SetTripFinal
        //Finalize Trip
        public bool SetTripFinal(crLogTrip triplog)
        {
            try
            {
                //find trip
                //var triplog = db.crLogTrips.Find(id);

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

        //POST: CarRentalLog/SetLinkTriplogJobs/{triplogId:int, jobmainId:int}
        [HttpPost]
        public bool SetLinkTriplogJobs(int triplogId, int jobmainId)
        {
            try
            {
                //new entry
                crLogTripJobMain logTripJobMain = new crLogTripJobMain();
                logTripJobMain.crLogTripId = triplogId;
                logTripJobMain.JobMainId = jobmainId;

                db.crLogTripJobMains.Add(logTripJobMain);
                db.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                var eexm = ex;
                return false;
            }
        }


        //GET: CarRentalLog/GetLinkTriplogJobs/{triplogId:int}
        [HttpGet]
        public int GetLinkTriplogJobs(int triplogId)
        {
            try
            {
                //find existing triplog-jobmain link
                var link = db.crLogTripJobMains.Where(c => c.crLogTripId == triplogId);

                if (link == null)
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


        //POST: CarRentalLog/DeleteLinkTriplogJobs/{triplogId:int, jobmainId:int}
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


        #endregion
    }

    public class crLogTripBilling
    {
        public string Company { get; set; }
        public List<crBilling_ContractVehicles> ContractVehicles { get; set; }
        public List<crBilling_OT> OTTrips { get; set; }
        public List<crBilling_Sundays> SundayTrips { get; set; }
    }

    public class crBilling_ContractVehicles
    {
        public int Id { get; set; }
        public string Unit { get; set; }
        public string ContractRate { get; set; }
        public string UnitCount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
    
    public class crBilling_OT
    {
        public int     Id { get; set; }
        public string  Driver { get; set; }
        public string  Unit   { get; set; }
        public DateTime DtTrip { get; set; }
        public string  StartTime { get; set; }
        public string  EndTime { get; set; }
        public decimal Rate   { get; set; }
        public decimal CompanyRate { get; set; }
        public double  OTHours    { get; set; }
        public decimal OTRate { get; set; }
    }

    public class crBilling_Sundays
    {
        public int Id { get; set; }
        public string Driver { get; set; }
        public string Unit { get; set; }
        public DateTime DtTrip { get; set; }
        public decimal Rate { get; set; }
     }

    public class cLogTrip_OT : crLogTrip
    {
        public double CalcOTHours { get; set; }
        public double CalcOTRate { get; set; }

        public void CalculateOT()
        {
            var trip = this;

            if (trip.StartTime != null && trip.EndTime != null && trip.TripHours != null)
            {
                DateTime StartTime = DateTime.Parse(trip.StartTime);
                DateTime EndTime = DateTime.Parse(trip.EndTime);

                //round off time start
                if (StartTime.Minute >= 40)
                {
                    StartTime = StartTime.AddHours(1);
                    StartTime = StartTime.AddMinutes(-30);
                }

                int HoursPerTrip = trip.TripHours ?? 0;

                double diff = (EndTime.Hour - StartTime.Hour) - HoursPerTrip;

                if (StartTime.Minute >= 20 && StartTime.Minute < 40)
                {
                    diff -= 0.5;
                }


                //round off time end
                if (EndTime.Minute >= 40)
                {
                    diff += 1;
                }
                if (EndTime.Minute >= 20 && EndTime.Minute < 40)
                {
                    diff += 0.5;
                }

                if (diff < 0)
                {
                    this.CalcOTHours = 0;
                }

                this.CalcOTHours = diff;
            }
            else
            {
                this.CalcOTHours = 0;
            }

            if (this.OTRate != 0)
            {
                this.CalcOTRate = this.CalcOTHours * Double.Parse(this.OTRate.ToString());

            }
            else
            {
                this.CalcOTRate = 0;
            }
        }
    }
}




