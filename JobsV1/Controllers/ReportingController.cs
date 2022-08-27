using JobsV1.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JobsV1.Controllers
{
    public class ReportingController : Controller
    {
        // NEW CUSTOMER Reference ID
        //private int NewCustSysId = 1;
        // Job Status
        //private int JOBINQUIRY = 1;
        //private int JOBRESERVATION = 2;
        //private int JOBCONFIRMED = 3;
        //private int JOBCLOSED = 4;
        //private int JOBCANCELLED = 5;
        //private int JOBTEMPLATE = 6;
        
        private JobDBContainer db = new JobDBContainer();
        private JobOrderClass jobs = new JobOrderClass();
        private DateClass dt = new DateClass();
        private DBClasses dbc = new DBClasses();

        // GET: Reporting
        public ActionResult Index(int? id, string reportkey)
        {
            DateClass localTime = new DateClass();

            if (id != null)
            {
                ViewBag.Id = id;
            }
            //pass reporting key
            ViewBag.reportkey = reportkey != null ? reportkey : "job";

            ViewBag.Packages = db.CarRatePackages.ToList();
            ViewBag.Group = db.RateGroups.ToList();
            ViewBag.Units = db.CarUnits.ToList();

            ViewBag.UnitList = db.InvItems.Where(s=>s.OrderNo <= 120).ToList();
            ViewBag.sDate = localTime.GetCurrentDate().AddMonths(-1);
            ViewBag.eDate = localTime.GetCurrentDate();

            return View();
        }


        #region Joblisting

        public List<cjobCounter> getJobActionCount(List<Int32> jobidlist)
        {
            #region sqlstr
            string sqlstr = @"
select max(x.jobid) JobId, x.Id CodeId, max(x.code) CodeDesc, sum(x.ActionCount) CntItem, sum(x.DoneCount) CntDone
from 
(

select max(a.JobMainId) jobid , d.Id, max(d.CatCode) code, '0' as ActionCount, count(b.Id) DoneCount
from JobServices a
left outer join JobActions b on a.Id = b.JobServicesId
left outer join SrvActionitems c on b.SrvActionItemId = c.Id
left outer join SrvActionCodes d on c.SrvActionCodeId = d.Id
Group by a.JobMainId,d.Id

union

select max(a.JobMainId) jobid , d.Id, max(d.CatCode) code, count(c.Id) as ActionCount, '0' as DoneCount
from JobServices a
left outer join [Services] b on a.ServicesId = b.Id
left outer join SrvActionitems c on b.Id = c.ServicesId
left outer join SrvActionCodes d on c.SrvActionCodeId = d.Id
Group by a.JobMainId,d.Id
)x Group by x.jobid, x.Id
order by x.jobid
;

";
            #endregion
            List<cjobCounter> jobcntr = db.Database.SqlQuery<cjobCounter>(sqlstr).Where(d => jobidlist.Contains(d.JobId)).ToList();
            return jobcntr;
        }

        public ActionResult JobSummary()
        {
            var today = dt.GetCurrentDate();
            var month = today.Month;
            var year = today.Year;
            
            var jobSummary = new List<ReportingViewModels.JobSummary>();

            var jobByMonth= jobs.GetJobOrderListingMonthly((int)month, (int)year);

            jobByMonth.ForEach(job => {
                jobSummary.Add(new ReportingViewModels.JobSummary
                {
                    Id          = job.Id,
                    Description = job.Description,
                    Contact     = job.Customer,
                    Company     = job.Company,
                    StartDate   = job.DtStart.ToString("MMM dd yyyy"),
                    EndDate     = job.DtEnd.ToString("MMM dd yyyy"),
                    Status      = job.StatusString,
                    Amount      = job.Amount,
                    Expenses    = job.ExpenseFromAP,
                    Payment     = job.PaymentFromAR,
                    DriversRate = job.DriversRate

                });
            });

            ViewBag.YearList = dt.GetYearsList();

            return View(jobSummary);
        }

        [HttpPost]
        public ActionResult JobSummary(int? month, int? year)
        {
            var today = dt.GetCurrentDate();
            if (month == null)
            {
                month = today.Month;
            }
            if (year == null)
            {
                year = today.Year;
            }

            var jobSummary = new List<ReportingViewModels.JobSummary>();

            var jobByMonth = jobs.GetJobOrderListingMonthly((int)month, (int)year);

            jobByMonth.ForEach(job => {
                jobSummary.Add(new ReportingViewModels.JobSummary
                {
                    Id = job.Id,
                    Description = job.Description,
                    Contact = job.Customer,
                    Company = job.Company,
                    StartDate = job.DtStart.ToString("MMM dd yyyy"),
                    EndDate = job.DtEnd.ToString("MMM dd yyyy"),
                    Status = job.StatusString,
                    Amount = job.Amount,
                    Expenses = job.ExpenseFromAP,
                    Payment = job.PaymentFromAR,
                    DriversRate = job.DriversRate,
                    Posted = job.IsPosted
                });
            });

            ViewBag.YearList = dt.GetYearsList();

            return View(jobSummary);
        }

        public ActionResult JobSummaryDetails(int id)
        {
            var jobSummary = new ReportingViewModels.JobSummaryDetails();

            var jobdetails = db.JobMains.Find(id);

            if (jobdetails == null)
            {
                return HttpNotFound();
            }

            jobSummary.Id = id;
            jobSummary.Account = jobs.GetJobCompany(id) + " / " + jobdetails.Customer.Name;
            jobSummary.Amount = (decimal)db.JobServices.Where(c=>c.JobMainId == id).Sum(c=>c.ActualAmt);
            jobSummary.TripLogs = jobs.GetTriplogsByJobId(id);
            jobSummary.Expenses = jobs.GetExpensesByJobId(id);
            jobSummary.Receivables = jobs.GetReceivablesByJobId(id);
            jobSummary.Status = jobs.GetJobStatusByJobId(id);

            return View(jobSummary);
        }

        #endregion 

    }
}