using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using JobsV1.Areas.Personel.Models;
using JobsV1.Models;
using Microsoft.Ajax.Utilities;

namespace JobsV1.Areas.Personel.Services
{
    public class CrLogServices
    {

        private CarRentalLogDBContainer db = new CarRentalLogDBContainer();
        private DateClass dt = new DateClass();

        public CrLogServices(CarRentalLogDBContainer _contextdb)
        {
            db = _contextdb;
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

            //HttpCookie shuttle_cookie = HttpContext.Request.Cookies.Get("shuttle_cookie");

            //if (shuttle_cookie != null)
            //{
            //    if (shuttle_cookie.Value == "1")
            //    {
            //        crLogTrips = crLogTrips.Where(c => c.crLogCompany.IsShuttle);
            //    }
            //}

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
                DateTime sdate = DateTime.ParseExact(startDate, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                DateTime edate =  DateTime.ParseExact(endDate, "MM/dd/yyyy", CultureInfo.InvariantCulture);

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
                    crLogTrips = crLogTrips.OrderBy(c => DbFunctions.TruncateTime(c.DtTrip))
                        .ThenBy(c => c.crLogUnit.OrderNo).ThenBy(c => c.crLogCompany.Name).ThenBy(c => c.crLogDriver.OrderNo) ;
                    break;
                case "Date-Desc":
                    crLogTrips = crLogTrips.OrderByDescending(c => DbFunctions.TruncateTime(c.DtTrip)).ThenBy(c => c.crLogCompany.Name).ThenBy(c => c.crLogDriver.OrderNo);
                    break;
                default:
                    crLogTrips = crLogTrips.OrderBy(c => DbFunctions.TruncateTime(c.DtTrip))
                        .ThenBy(c => c.crLogUnit.OrderNo).ThenBy(c => c.crLogCompany.Name).ThenBy(c => c.crLogDriver.OrderNo);
                    break;
            }

            //HttpCookie shuttle_cookie = HttpContext.Request.Cookies.Get("shuttle_cookie");

            //if (shuttle_cookie != null)
            //{
            //    if (shuttle_cookie.Value == "1")
            //    {
            //        crLogTrips = crLogTrips.Where(c => c.crLogCompany.IsShuttle);
            //    }
            //}

            return crLogTrips.ToList();

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

        //Check if Unit is encoded in trip logs for the selected date
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

        //Check if Driver is encoded in trip logs for the selected date
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

        public int GenerateClosingId()
        {
            crLogClosing ctrx = new crLogClosing()
            { 
                dtClose = System.DateTime.Now 
            };

            db.crLogClosings.Add(ctrx);
            db.SaveChanges();

            return ctrx.Id;
        }

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

        //Finalize Trip
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

        public bool CheckTripFinalizeRange(List<crLogTrip> trips)
        {
            try
            {
                var dateTimeNow = dt.GetCurrentDateTime();

                //18 == 6:00pm
                //finalize trips
                if (dateTimeNow.Hour >= 17)
                {
                    var result = false;
                    foreach (var trip in trips)
                    {
                        result = SetTripFinal(trip);
                    }

                    if (result)
                    {
                        //save
                       SaveDbChanges();
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Param: id = TripId
        public bool DeleteTripPassengers(int? id)
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

        public bool DeleteTripJobLink(int? id)
        {
            try
            {
                if (id != null)
                {
                    var joblink = db.crLogTripJobMains.Where(c => c.crLogTripId == id).ToList();
                    if (joblink.Count() > 0)
                    {
                        db.crLogTripJobMains.RemoveRange(joblink);
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

        public crLogCompany GetcrLogCompanyByName(string company)
        {
            try
            {
                crLogCompany logcompany = db.crLogCompanies.Where(c => c.Name == company).FirstOrDefault();

                if (logcompany != null)
                {
                    return logcompany;
                }

                return null;
            }
            catch
            {
                return null;
            }
        }

        public List<string> GetAvailableUnitsOnDate(List<crLogTrip> crLogTrips)
        {
            List<string> unitsAvailable = new List<string>();

            var tripUnits = crLogTrips.Select(c => c.crLogUnit).ToList();
            var RealBreezeUnits = db.crLogUnits.Where(c => c.crLogOwner.Name == "Realbreeze" && c.Status == "ACT").ToList();

            RealBreezeUnits.ForEach(rbUnit => {
                if (!tripUnits.Contains(rbUnit))
                {
                    unitsAvailable.Add(rbUnit.Description);
                }
            });

            return unitsAvailable;
        }

        public int SaveDbChanges()
        {
           return db.SaveChanges();
        }
    }
}