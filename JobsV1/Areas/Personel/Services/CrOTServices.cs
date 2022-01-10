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
    public class CrOTServices
    {

        private CarRentalLogDBContainer db = new CarRentalLogDBContainer();
        private DateClass dt = new DateClass();

        public CrOTServices(CarRentalLogDBContainer _contextdb)
        {
            db = _contextdb;
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


    }
}