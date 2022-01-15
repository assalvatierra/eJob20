using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JobsV1.Models;
using ApModels.Models;
using ArModels.Models;
using JobsV1.Areas.Personel.Services;
using JobsV1.Areas.Personel.Models;
using System.Data.Entity;

namespace JobsV1.Models.Class
{
    public class DashboardServices
    {
        private JobDBContainer db;
        private ApDBContainer apdb = new ApDBContainer();
        private ArDBContainer ardb = new ArDBContainer();
        private DateClass dt = new DateClass();
        private CarRentalLogDBContainer crdb;
        private CrLogServices crLog;

        public DashboardServices()
        {
            db = new JobDBContainer(); 
            crdb = new CarRentalLogDBContainer();
            crLog = new CrLogServices(crdb);
        }

        public decimal GetSalesByMonthNo(int month)
        {
            try
            {
                var salesTotal = ardb.ArTransactions
                    .Where(t=>t.DtInvoice.Month == month)
                    .ToList().Select(t => t.Amount).Sum();

                return salesTotal;
            }
            catch
            {
                return 0;
            }
        }

        public decimal GetExpensesByMonthNo(int month)
        {
            try
            {
                var salesTotal = apdb.ApTransactions
                    .Where(t => t.DtInvoice.Month == month)
                    .ToList().Select(t => t.Amount).Sum();

                return salesTotal;
            }
            catch
            {
                return 0;
            }
        }

        public int GetActiveJobsToday()
        {
            try
            {

                JobOrderClass jobSvc = new JobOrderClass();

                var activejobCount = jobSvc.GetJobOrderListing(1).Count();

                return activejobCount;
            }
            catch
            {
                return 0;
            }
        }

        public List<DashboardViewModel.JobOrders> GetJobOrders()
        {
            try
            {
                List<DashboardViewModel.JobOrders> jobOrders = new List<DashboardViewModel.JobOrders>();

                JobOrderClass jobSvc = new JobOrderClass();

                var activejobs = jobSvc.GetJobOrderListing(1);

                activejobs.ForEach(j => {
                    jobOrders.Add(new DashboardViewModel.JobOrders
                    {
                        Id = j.Id,
                        Description = j.Company + " / " + j.Customer,
                        Amount = j.Amount.ToString(),
                        StartDate = j.DtStart.ToString("MMM dd"),
                        EndDate = j.DtEnd.ToString("MMM dd"),
                        Status = jobSvc.GetJobStatus(j.Status)
                    });
                });

                return jobOrders;
            }
            catch
            {
                return new List<DashboardViewModel.JobOrders>();
            }
        }


        public List<DashboardViewModel.TripLogs> GetTripLogsToday()
        {
            try
            {
                List<DashboardViewModel.TripLogs> tripLogs = new List<DashboardViewModel.TripLogs>();

                var today = dt.GetCurrentDate();
                var tripLogsToday = crdb.crLogTrips.Where(c=> DbFunctions.TruncateTime(c.DtTrip) == today).ToList();

                tripLogsToday.ForEach(t =>
                {
                    tripLogs.Add(new DashboardViewModel.TripLogs
                    {
                        Id = t.Id,
                        Description = t.crLogCompany.Name,
                        Driver = t.crLogDriver.Name,
                        JobId = t.crLogTripJobMains.FirstOrDefault() == null ? 0 : t.crLogTripJobMains.FirstOrDefault().JobMainId,
                        Unit = t.crLogUnit.Description
                    });
                });

                return tripLogs;
            }
            catch
            {
                return new List<DashboardViewModel.TripLogs>();
            }
        }

        public void GetNotificationsToday()
        {

        }

        public void GetReceivablesToday()
        {

        }

        public void GetPayablesToday()
        {

        }

    }
}