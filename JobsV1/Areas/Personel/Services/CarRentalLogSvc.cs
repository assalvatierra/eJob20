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

namespace JobsV1.Areas.Personel.Services
{
    public class CarRentalLogSvc
    {

        private CarRentalLogDBContainer db;
        private DateClass dt = new DateClass();

        public CarRentalLogSvc(CarRentalLogDBContainer _contextdb)
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




        public int SaveDbChanges()
        {
           return db.SaveChanges();
        }
    }
}