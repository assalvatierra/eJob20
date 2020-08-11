using System;
using System.Linq;

namespace JobsV1.Areas.Personel.Models
{
    public class crDriverData
    {
        private CarRentalLogDBContainer db = new CarRentalLogDBContainer();

        public decimal GetDriverCA_FromDates(DateTime sDate, DateTime eDate, int driverId)
        {
            try
            {
                if (driverId == 0 || sDate == null || eDate == null)
                {
                    return -1;
                }

                decimal TotalCA = 0;

                var cashTrx = db.crLogCashReleases.Where(c => c.crLogCashTypeId == 2 && c.crLogDriverId == driverId && c.crLogClosingId == null).ToList();

                cashTrx.ForEach(c => {
                    TotalCA += c.Amount;
                });

                return TotalCA;
            }
            catch
            {
                return -1;
            }
        }


        public decimal GetDriverSalary_FromDates(DateTime sDate, DateTime eDate, int driverId)
        {
            try
            {
                if (driverId == 0 || sDate == null || eDate == null)
                {
                    return -1;
                }

                decimal TotalSalary = 0;

                var cashTrx = db.crLogCashReleases.Where(c => c.crLogCashTypeId == 1 && c.crLogDriverId == driverId && c.crLogClosingId != null).ToList();

                cashTrx.ForEach(c => {
                    TotalSalary += c.Amount;
                });

                return TotalSalary;
            }
            catch
            {
                return -1;
            }
        }


        public decimal GetDriverPayments_FromDates(DateTime sDate, DateTime eDate, int driverId)
        {
            try
            {
                if (driverId == 0 || sDate == null || eDate == null)
                {
                    return -1;
                }

                decimal TotalSalary = 0;

                var cashTrx = db.crLogCashReleases.Where(c => c.crLogCashTypeId == 3 && c.crLogDriverId == driverId && c.crLogClosingId == null).ToList();

                cashTrx.ForEach(c => {
                    TotalSalary += c.Amount;
                });

                return TotalSalary;
            }
            catch
            {
                return -1;
            }
        }


    }
}