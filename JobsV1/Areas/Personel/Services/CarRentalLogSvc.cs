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
    }
}