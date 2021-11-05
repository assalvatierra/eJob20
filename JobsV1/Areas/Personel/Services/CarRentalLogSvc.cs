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
using JobsV1.Models;
using Microsoft.Ajax.Utilities;

namespace JobsV1.Areas.Personel.Services
{
    public class CarRentalLogSvc
    {

        private CarRentalLogDBContainer db = new CarRentalLogDBContainer();
        private DateClass dt = new DateClass();

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