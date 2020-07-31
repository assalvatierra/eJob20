using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using JobsV1.Areas.Personel.Models;
using Microsoft.Ajax.Utilities;

namespace JobsV1.Areas.Personel.Controllers
{
    public class CarRentalLogController : Controller
    {
        private CarRentalLogDBContainer db = new CarRentalLogDBContainer();
        private CrDataLayer dl = new CrDataLayer();

        // GET: Personel/CarRentalLog
        public ActionResult Index(string startDate, string endDate, string unit, string driver, string company, string sortby)
        {

            try
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

                if (!String.IsNullOrEmpty(unit) && unit != "all" )
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

                var tripLogs = crLogTrips.ToList();

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

                ViewBag.crLogUnitList = dl.GetUnits().ToList();
                ViewBag.crLogDriverList = dl.GetDrivers().ToList();
                ViewBag.crLogCompanyList = dl.GetCompanies().ToList();
            return View(tripLogs);

            }
            catch
            {
                return View(new List<crLogTrip>());
            }
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


            ViewBag.crLogDriverId = new SelectList(dl.GetDrivers(), "Id", "Name");
            ViewBag.crLogUnitId = new SelectList(dl.GetUnits(), "Id", "Description");
            ViewBag.crLogCompanyId = new SelectList(db.crLogCompanies, "Id", "Name");
            //ViewBag.crLogClosingId = new SelectList(db.crLogClosings, "Id", "Id");
            return View(trip);
        }

        // POST: Personel/CarRentalLog/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,crLogDriverId,crLogUnitId,crLogCompanyId,DtTrip,Rate,Addon,Expenses,DriverFee,Remarks, OdoStart, OdoEnd")] crLogTrip crLogTrip)
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
        public ActionResult Edit([Bind(Include = "Id,crLogDriverId,crLogUnitId,crLogCompanyId,DtTrip,Rate,Addon,Expenses,DriverFee,Remarks, OdoStart, OdoEnd")] crLogTrip crLogTrip)
        {
            if (ModelState.IsValid)
            {
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
            crLogTrip crLogTrip = db.crLogTrips.Find(id);
            db.crLogTrips.Remove(crLogTrip);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        // GET: Personel/CarRentalCashRelease/Edit/5
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

        // POST: Personel/CarRentalCashRelease/Edit/5
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


        // GET: Personel/CarRentalLog/Edit/5
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

        // POST: Personel/CarRentalLog/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditOdo(int? id, int? OdoStart, int? OdoEnd)
        {
            if (id != null )
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


        public ActionResult DriverSummary(int id, int? reqStatus)
        {
            crLogDriver driver = db.crLogDrivers.Find(id);
            List<crLogTrip> trips = db.crLogTrips.Where(d => d.crLogDriverId == id && d.crLogClosingId==null).OrderBy(s=>s.DtTrip).ToList();
            List<crLogCashRelease> cashtrx = db.crLogCashReleases.Where(d => d.crLogDriverId == id && d.crLogCashTypeId == 2).OrderBy(s=>s.DtRelease).ToList();
            List<crLogCashRelease> payments = db.crLogCashReleases.Where(d => d.crLogDriverId == id && d.crLogCashTypeId == 3).OrderBy(s => s.DtRelease).ToList();
            List<crLogCashRelease> noStatus = db.crLogCashReleases.Where(d => d.crLogDriverId == id && d.crLogClosing == null && d.crLogCashTypeId == 1).OrderBy(s => s.DtRelease).ToList();


            crDriverSummary driversummary = new crDriverSummary();
            driversummary.Driver = driver;
            driversummary.DriverTrips = trips;
            driversummary.DriverCash = cashtrx;
            driversummary.DriverPayments = payments;
            driversummary.NoStatus = noStatus;

            ViewBag.DriverId = id;
            ViewBag.crLogDriverId = new SelectList(db.crLogDrivers, "Id", "Name", id);
            ViewBag.reqStatus = reqStatus ?? 0;
            ViewBag.IsAdmin = User.IsInRole("Admin");
            return View(driversummary);
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

        public bool CopyTripSubmit(int?  id, string copyDate)
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

                return true;
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

                //check and find lastest trip
                var crLogTripLatest = db.crLogTrips.Where(c => c.crLogUnitId == crLogUnitId && c.crLogDriverId == crLogDriverId && c.crLogCompanyId == crLogCompanyId).OrderByDescending(c => c.DtTrip).FirstOrDefault();
                if (crLogTripLatest == null)
                {
                    ModelState.AddModelError("OdoEnd", "No Trips Found");
                }else
                {
                    //update odo and remarks
                    crLogTripLatest.OdoStart = OdoStart;
                    crLogTripLatest.OdoEnd = OdoEnd;
                    crLogTripLatest.Remarks = Remarks;

                    db.Entry(crLogTripLatest).State = EntityState.Modified;
                    db.SaveChanges();
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
    }
}
