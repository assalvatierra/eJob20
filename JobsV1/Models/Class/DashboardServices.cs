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
using System.Data.Entity.Core;

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
        private SysDBContainer sysDb;

        public DashboardServices()
        {
            db = new JobDBContainer(); 
            crdb = new CarRentalLogDBContainer();
            crLog = new CrLogServices(crdb);
            sysDb = new SysDBContainer();
        }

        public decimal GetSalesByMonthNo(int month, int year)
        {
            try
            {

                var salesTotal = ardb.ArTransactions
                    .Where(t => t.ArTransPayments.Select(p => p.ArPayment.DtPayment.Month).FirstOrDefault() == month &&
                                t.ArTransPayments.Select(p => p.ArPayment.DtPayment.Year).FirstOrDefault() == year)
                    .ToList().Select(t => t.ArTransPayments.Sum(p=>p.ArPayment.Amount)).Sum();

                return salesTotal;
            }
            catch
            {
                return 0;
                throw new EntityException("Dashboard Services: Unable to process GetSalesByMonthNo service");
            }
        }

        public decimal GetExpensesByMonthNo(int month, int year)
        {
            try
            {
                //1st rev
                var expensesTotal = apdb.ApTransactions
                    .Where(t => t.DtDue.Month == month && t.DtDue.Year == year 
                           && t.ApTransTypeId != 2)
                    .ToList().Select(t => t.ApTransPayments
                    .Where(c => c.ApPayment.ApPaymentStatusId == 2)
                    .Sum(p => p.ApPayment.Amount))
                    .Sum();

                return expensesTotal;

            }
            catch
            {
                return 0;
                throw new EntityException("Dashboard Services: Unable to process GetExpensesByMonthNo service");
            }
        }

        public int GetActiveJobsToday()
        {
            try
            {

                JobOrderClass jobSvc = new JobOrderClass();

                var activejobCount = jobSvc.GetJobOrderListing(1).Where(c=>c.Status > 1).Count();

                return activejobCount;
            }
            catch
            {
                return 0;
                throw new EntityException("Dashboard Services: Unable to process GetActiveJobsToday service");
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
                        Amount = j.Amount,
                        StartDate = j.DtStart.ToString("MMM dd"),
                        EndDate = j.DtEnd.ToString("MMM dd"),
                        Status = jobSvc.GetJobStatus(j.Status),
                        Order = j.Status
                    });
                });

                return jobOrders.OrderByDescending(c=>c.Order).ToList();
            }
            catch
            {
                return new List<DashboardViewModel.JobOrders>();
                throw new EntityException("Dashboard Services: Unable to process GetJobOrders service");
            }
        }


        public List<DashboardViewModel.TripLogs> GetTripLogsToday()
        {
            try
            {
                List<DashboardViewModel.TripLogs> tripLogs = new List<DashboardViewModel.TripLogs>();

                var today = dt.GetCurrentDate();
                var tripLogsToday = crdb.crLogTrips.Where(c=> DbFunctions.TruncateTime(c.DtTrip) == today)
                    .OrderBy(c=>c.crLogDriver.OrderNo).ToList();

                

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
                
                var unitList = tripLogsToday.Select(t => t.crLogUnitId).ToList();

                GetAvailableUnits(unitList).ForEach( t => {
                    tripLogs.Add(new DashboardViewModel.TripLogs
                    {
                        Id = 0,
                        Description = "Available",
                        Driver = "-",
                        JobId = 0,
                        Unit = t.Description
                    });
                });

                return tripLogs;
            }
            catch
            {
                return new List<DashboardViewModel.TripLogs>();
                throw new EntityException("Dashboard Services: Unable to process GetTripLogsToday service");
            }
        }

        private List<crLogUnit> GetAvailableUnits(List<int> unitIds)
        {

            var unitList = crdb.crLogUnits.Where(c => c.OrderNo == 100).ToList();

            var unitListIds = unitList.Where(c => !unitIds.Contains(c.Id)).ToList();

            return unitListIds;
        }

        public void GetNotificationsToday()
        {

        }

        public List<DashboardViewModel.Receivables> GetReceivablesToday()
        {
            try
            {
                List<DashboardViewModel.Receivables> receivables = new List<DashboardViewModel.Receivables>();
                List<ArTransaction> transactions = new List<ArTransaction>();
                var today = dt.GetCurrentDate();
                
                //get receivables today by invoice date
                var receivablesToday = ardb.ArTransactions
                    .Where(c => DbFunctions.TruncateTime(c.DtInvoice) == today)
                    .OrderByDescending(e => e.DtInvoice)
                    .ToList();
                transactions.AddRange(receivablesToday);

                //get receivables today by date due
                var receivablesDueToday = ardb.ArTransactions
                    .Where(c => DbFunctions.TruncateTime(c.DtDue) == today)
                    .OrderByDescending(e => e.DtInvoice)
                    .ToList();
                transactions.AddRange(receivablesDueToday);

                //get FOR APPROVAL receivables past due date 
                var receivablesForApprovalPast = ardb.ArTransactions
                    .Where(c => DbFunctions.TruncateTime(c.DtDue) < today && c.ArTransStatusId == 2)
                    .OrderByDescending(e => e.DtInvoice)
                    .ToList();

                transactions.AddRange(receivablesForApprovalPast);
                transactions = transactions.Distinct().ToList();

                transactions.ForEach(e => {
                    receivables.Add(new DashboardViewModel.Receivables
                    {
                        Id = e.Id,
                        InvoiceId = e.InvoiceId,
                        Payment = e.ArTransPayments != null ? e.ArTransPayments.Sum( p => p.ArPayment.Amount ) : 0,
                        InvoiceDate = e.DtInvoice,
                        StartDate = e.DtService,
                        EndDate = e.DtServiceTo ?? e.DtService,
                        Amount = e.Amount,
                        Status = e.ArTransStatu.Status,
                        Transaction = e.Description,
                        Account = e.ArAccount.Company ?? "-",
                        Contact = e.ArAccContact.Name ?? "-"
                    });
                });

                return receivables;

            }
            catch
            {
                return new List<DashboardViewModel.Receivables>();
                throw new EntityException("Dashboard Services: Unable to process GetReceivablesToday service");
            }
        }

        public List<DashboardViewModel.Expenses> GetPayablesToday()
        {
            try
            {
                List<DashboardViewModel.Expenses> expenses = new List<DashboardViewModel.Expenses>();

                var today = dt.GetCurrentDate();

                var expenseToday = apdb.ApTransactions
                    .Where(c => DbFunctions.TruncateTime(c.DtDue) == today || DbFunctions.TruncateTime(c.DtInvoice) == today)
                    .OrderByDescending(c => c.DtDue).ToList();

                expenseToday.ForEach(e => {
                    expenses.Add(new DashboardViewModel.Expenses
                    {
                        Id = e.Id,
                        Date = e.DtDue,
                        Amount = e.Amount,
                        Status = e.ApTransStatu.Status,
                        Transaction = e.ApAccount.Name + " " + e.Description
                    });
                });

                return expenses;

            }
            catch
            {
                return new List<DashboardViewModel.Expenses>();
            }
        }

        public List<DashboardViewModel.Notifications> GetNotifications()
        {
            try
            {
                

                List<DashboardViewModel.Notifications> notifications = new List<DashboardViewModel.Notifications>();

                //jobs notifications
                var unassigned = GetUnassignedInActiveJobs();

                if (unassigned > 0)
                {
                    notifications.Add(new DashboardViewModel.Notifications
                    {
                        Date = dt.GetCurrentDate(),
                        Header = "JobOrders",
                        Message = unassigned + " Unassiged Jobs ",
                        Link = "../InvItems/Availability"
                    });
                }


                //triplogs notifications
                var triplogsNoJobs = GetTipLogsNoJobs();

                if (triplogsNoJobs > 0)
                {
                    notifications.Add(new DashboardViewModel.Notifications
                    {
                        Date = dt.GetCurrentDate(),
                        Header = "JobOrders",
                        Message = triplogsNoJobs + " TripLogs with no Jobs Link",
                        Link = "../Personel/CarRentalLog"
                    });
                }

                //expenses notifications
                var expensesNotif = GetExpensesNotification(); 
                if (expensesNotif != null)
                {
                    notifications.Add(expensesNotif);
                }

                //receivables notifications
                var receivablesNotif = GetReceivablesNotification();
                if (receivablesNotif != null)
                {
                    notifications.Add(receivablesNotif);
                }

                return notifications;
            }
            catch
            {
                return new List<DashboardViewModel.Notifications>();
            }
        }

        public int GetUnassignedInActiveJobs()
        {
            try
            {

                JobOrderClass jobSvc = new JobOrderClass();
                var activejobs = jobSvc.GetJobData(1);
                var unassignedCount = 0;

                activejobs.ForEach(c=> {
                    c.Services.ForEach(s=> {
                        var items = s.SvcItems.ToList();
                        items.ForEach(i => {
                            if( i.InvItem.Description.ToLower() == "unassigned" )
                            {
                                unassignedCount++;
                            }
                        });
                    });
                });

                return unassignedCount;
            }
            catch
            {
                return 0;
            }
        }


        public int GetTipLogsNoJobs()
        {
            try
            {
                var today = dt.GetCurrentDate();
                var tripLogsToday = crdb.crLogTrips.Where(c => DbFunctions.TruncateTime(c.DtTrip) == today 
                       && c.crLogUnit.Description != "Office")
                    .OrderBy(c => c.crLogDriver.OrderNo).ToList();

                var unassignedCount = 0;

                unassignedCount = tripLogsToday.Where(c => c.crLogTripJobMains.Count() == 0).Count();

                return unassignedCount;
            }
            catch
            {
                return 0;
            }
        }

        public DashboardViewModel.Notifications GetExpensesNotification()
        {
            try
            {
                var notification = new DashboardViewModel.Notifications();
                var message = "";
                var today = dt.GetCurrentDate();
                var activeExpenses = apdb.ApTransactions.Where(t => t.ApTransStatusId == 1 && t.ApTransStatusId == 6).ToList();
                var pastDueExpenses = apdb.ApTransactions.Where(t => t.ApTransStatusId < 4 && DbFunctions.TruncateTime(t.DtDue) < today ).ToList();

                int  RequestExpense = 0, PastDue = 0;

                RequestExpense = activeExpenses.Where(e => e.ApTransStatusId == 1).Count();
                if (RequestExpense > 0)
                {
                    message += RequestExpense + " Request ";
                }

                PastDue = pastDueExpenses.Count();
                if (PastDue > 0)
                {
                    message += " " + PastDue + " Past Due Date ";
                }

                notification.Date = today;
                notification.Header = "Expenses";
                notification.Message = message;
                notification.Link = "/Payables/ApTransactions";

                return notification;
            }
            catch
            {
                return null;
            }
        }


        public DashboardViewModel.Notifications GetReceivablesNotification()
        {
            try
            {
                var notification = new DashboardViewModel.Notifications();
                var message = "";
                var today = dt.GetCurrentDate();
                var active = ardb.ArTransactions.Where(t => t.ArTransStatusId == 2).ToList();
                var pastDue = ardb.ArTransactions.Where(t => t.ArTransStatusId < 5 && DbFunctions.TruncateTime(t.DtDue) < today).ToList();

                int ForApprovals = 0, PastDue = 0;

                // Get for approval trans
                ForApprovals = active.Where(e => e.ArTransStatusId == 1).Count();
                if (ForApprovals > 0)
                {
                    message += ForApprovals + " For Approval ";
                }

                //get past the due date  trans
                PastDue = pastDue.Count();
                if (PastDue > 0)
                {
                    message += " " + PastDue + " Past Due Date ";
                }

                //create notification msg
                notification.Date = today;
                notification.Header = "Receivables";
                notification.Message = message;
                notification.Link = "/Receivables/ArTransactions";

                if (PastDue == 0 && ForApprovals == 0)
                {
                    return null;
                }

                return notification;
            }
            catch
            {
                return null;
            }
        }

        public List<DashboardViewModel.Notifications> GetNewJobsNotification()
        {
            try
            {
                var notification = new List<DashboardViewModel.Notifications>();

                var today = dt.GetCurrentDate();

                var newJobs = db.JobMains.Where(j => DbFunctions.TruncateTime(j.JobDate) == today).ToList();

                newJobs.ForEach(j =>
                {
                    notification.Add(new DashboardViewModel.Notifications { 
                    Date = j.JobDate,
                    Header = "Job Order",
                    Id = j.Id,
                    Message = "New Job : " + j.Description + " - " + j.JobStatus.Status,
                    Link = "/JobOrder/JobServices?JobMainId=" + j.Id
                    });
                });

                return notification;
            }
            catch
            {
                return null;
            }
        }

        public List<DashboardViewModel.ChartData> GetMonthlyJobsCount()
        {
            try
            {
                List<DashboardViewModel.ChartData> data = new List<DashboardViewModel.ChartData>();

                //get list of months
                var lastSixMonths = Enumerable.Range(0, 6)
                              .Select(i => DateTime.Now.AddMonths(i+1 - 6))
                              .Select(date => date.ToString("MMM"));

                //StartMonth for filter
                var MonthRange = dt.GetCurrentDate().AddMonths(-6);

                //temp job list based on month filter
                var temp = db.JobMains
                    .Where(c => c.JobDate.CompareTo(MonthRange) > 0)
                    .ToList();

                //selected jobs per months count
                var monthlyjobs = temp
                    .GroupBy(j => j.JobDate.ToString("MMM"))
                    .Select(c=> new DashboardViewModel.ChartData {
                    Month = c.Key,
                    Count = c.Count()
                }).ToList();

                //assign months with no jobs with 0
                foreach (var month in lastSixMonths)
                {
                    if (monthlyjobs.Select(c=>c.Month).Contains(month))
                    {
                        var monthJob = monthlyjobs.Where(c => c.Month == month).FirstOrDefault();
                        data.Add(monthJob);
                    }
                    else
                    {
                        data.Add(new DashboardViewModel.ChartData
                        {
                            Month = month,
                            Count = 0
                        });
                    }
                }

                return data;
            }
            catch 
            {
                return new List<DashboardViewModel.ChartData>();
            }
        }


        public List<DashboardViewModel.ChartDataTripLogs> GetMonthlyTripLogs(int month, int year)
        {
            try
            {
                List<DashboardViewModel.ChartDataTripLogs> data = new List<DashboardViewModel.ChartDataTripLogs>();
                var today = dt.GetCurrentDate();


                //get list of months
                var daysOfMonth = Enumerable.Range(1, DateTime.DaysInMonth(year, month))  // Days: 1, 2 ... 31 etc.
                             .Select(day => day.ToString()) // Map each day to a date
                             .ToList();

                var tripForMonth = crdb.crLogTrips
                    .Where(c => c.DtTrip.Month == month && c.DtTrip.Year == year
                            && c.crLogCompany.Name != "Office" 
                            && c.crLogUnit.crLogOwner.Name == "Realbreeze")
                    .GroupBy(c=>c.DtTrip.Day).ToList();
                
                var totalCount = 0;

                daysOfMonth.ForEach(d=> {
                    var trip = tripForMonth.Where(t => t.Key.ToString() == d).FirstOrDefault();
                    if (trip != null)
                    {
                        if (d == trip.Key.ToString())
                        {
                            totalCount += trip.Count();

                            data.Add(new DashboardViewModel.ChartDataTripLogs
                            {
                                Month = dt.GetMonthName(month),
                                Count = totalCount,
                                Day = trip.Key.ToString()
                            });
                        }
                    }
                    else
                    {
                        if (today.Day >= int.Parse(d) && today.Month == month && today.Year == year)
                        {
                            data.Add(new DashboardViewModel.ChartDataTripLogs
                            {
                                Month = dt.GetMonthName(month),
                                Count = totalCount,
                                Day = d
                            });
                        }
                        else
                        {
                            if (today.Month == month && today.Year == year)
                            {
                                data.Add(new DashboardViewModel.ChartDataTripLogs
                                {
                                    Month = dt.GetMonthName(month),
                                    Count = 0,
                                    Day = d
                                });
                            }
                            else
                            {
                                data.Add(new DashboardViewModel.ChartDataTripLogs
                                {
                                    Month = dt.GetMonthName(month),
                                    Count = totalCount,
                                    Day = d
                                });
                            }
                        }
                    }
                });

                return data;
            }
            catch
            {
                return new List<DashboardViewModel.ChartDataTripLogs>();
            }

        }

        public List<SysMenu> GetUserMenu(string user)
        {

            try
            {
                var menuList = sysDb.SysAccessUsers.Where(s => s.UserId == user).Select(c => c.SysMenu).ToList();

                return menuList;
            }
            catch
            {
                return new List<SysMenu>();
                throw new EntityException("Dashboard Services: Unable to process GetUserMenu service");
            }
        }
    }
}