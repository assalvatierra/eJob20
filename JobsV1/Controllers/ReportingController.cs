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
                    DriversRate = job.DriversRate
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

        #region Payments Report
        public PartialViewResult Payments(string sDate, string eDate ,string company, string bank)
        {
            var paymentReport = db.JobPayments.ToList();

            if (company != null)
            {
                if (company != "all")
                {
                    paymentReport = paymentReport
                                        .Where(p => p.JobMain.Description.ToLower().Contains(company.ToLower()))
                                        .ToList();
                }
            }

            if (sDate != null && eDate != null)
            {
                DateTime startDateRange = DateTime.Parse(sDate.ToString());
                DateTime endDateRange = DateTime.Parse(eDate.ToString());
                paymentReport = paymentReport
                    .Where(p => (DateTime.Compare(p.DtPayment.Date, startDateRange.Date) >= 0 && DateTime.Compare(p.DtPayment.Date, endDateRange.Date) <= 0))
                    .ToList();
            }else{
                DateTime today = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Singapore Standard Time"));

                paymentReport = paymentReport
                    .Where(p => (DateTime.Compare(p.DtPayment.Date, today) >= 0 && DateTime.Compare(p.DtPayment.Date, today) <= 0))
                    .ToList();
            }

            if (bank != null)
            {
                if (bank != "all")
                {
                    paymentReport = paymentReport.Where(p => p.JobMain.JobPayments.Where(s=>s.JobMainId == p.JobMainId).FirstOrDefault()
                                        .Bank.BankName.ToLower().Contains(bank.ToLower()))
                                        .ToList();
                }
            }

            ViewBag.sDate = sDate;
            ViewBag.eDate = eDate;

            return PartialView(paymentReport);
        }

        public ActionResult PaymentsPrint(string sDate, string eDate, string company, string bank)
        {
            var paymentReport = db.JobPayments.ToList();

            if (company != null)
            {
                if (company != "all")
                {
                    paymentReport = paymentReport
                                        .Where(p => p.JobMain.Description.ToLower().Contains(company.ToLower()))
                                        .ToList();
                }
            }

            if (sDate != null && eDate != null)
            {
                DateTime startDateRange = DateTime.Parse(sDate.ToString());
                DateTime endDateRange = DateTime.Parse(eDate.ToString());
                paymentReport = paymentReport
                    .Where(p => (DateTime.Compare(p.DtPayment.Date, startDateRange.Date) >= 0 && DateTime.Compare(p.DtPayment.Date, endDateRange.Date) <= 0))
                    .ToList();
            }

            if (bank != null)
            {
                if (bank != "all")
                {
                    paymentReport = paymentReport.Where(p => p.JobMain.JobPayments.Where(s => s.JobMainId == p.JobMainId).FirstOrDefault()
                                        .Bank.BankName.ToLower().Contains(bank.ToLower()))
                                        .ToList();
                }
            }

            return View("Payments", paymentReport);

            //return View(db.JobPayments.ToList());
        }
        #endregion

        #region Packages

        public PartialViewResult PackageRates(string status,string unit, string package, string group)
        {
            var unitPkgList = dbc.getPackageperUnitList(status, package, unit, group);
            
            ViewBag.Packages = db.CarRatePackages.ToList();
            ViewBag.Group = db.RateGroups.ToList();
            ViewBag.Units = db.CarUnits.ToList();

            ViewBag.status = status;
            ViewBag.unit = unit;
            ViewBag.package = package;
            ViewBag.group = group;

            return PartialView(unitPkgList);
        }

        public ActionResult PackageRatesPrint(string status, string unit, string package, string group)
        {
            var unitPkgList = dbc.getPackageperUnitList(status, package, unit, group);

            ViewBag.Packages = db.CarRatePackages.ToList();
            ViewBag.Group = db.RateGroups.ToList();
            ViewBag.Units = db.CarUnits.ToList();

            ViewBag.status = status;
            ViewBag.unit = unit;
            ViewBag.package = package;
            ViewBag.group = group;

            return View("PackageRatesPrint", unitPkgList);
        }

        #endregion

        #region Unit Income
        public PartialViewResult UnitIncome(int? id, string sDate, string eDate, int? sortid, int? serviceId, int? mainid, string type, int unit)
        {
            //Old Open jobs
            var closedJobsIds = dbc.getAllClosedJobs(sDate, eDate, type, unit.ToString()).Select(s => s.Id);  //get list of older jobs that are not closed
            var closedJobsList = getUnitListing(closedJobsIds, unit);

            DateClass localTime = new DateClass();
            ViewBag.sDate = localTime.GetCurrentDate().AddMonths(-1).ToShortDateString();
            ViewBag.eDate = localTime.GetCurrentDate().ToShortDateString();

            ViewBag.unitList = db.InvItems.ToList();

            return PartialView(closedJobsList);
        }

        public PartialViewResult UnitIncomePrint(int? id, string sDate, string eDate, int? sortid, int? serviceId, int? mainid, string type, int unit)
        {
            //Old Open jobs
            var closedJobsIds = dbc.getAllClosedJobs(sDate, eDate, type, unit.ToString()).Select(s => s.Id);  //get list of older jobs that are not closed
            var closedJobsList = getUnitListing(closedJobsIds, unit);

            DateClass localTime = new DateClass();
            ViewBag.sDate = localTime.GetCurrentDate().AddMonths(-1).ToShortDateString();
            ViewBag.eDate = localTime.GetCurrentDate().ToShortDateString();

            ViewBag.unitList = db.InvItems.ToList();

            return PartialView(closedJobsList);
        }

        public PartialViewResult UnitIncomeReport()
        {
                                            
            DateClass localTime = new DateClass();
            ViewBag.sDate = localTime.GetCurrentDate().AddMonths(-1);
            ViewBag.eDate = localTime.GetCurrentDate();

            ViewBag.unitList = db.InvItems.ToList().OrderBy(u=>u.OrderNo);

            return PartialView();
        }

        private IEnumerable<cjobUnitIncome> getUnitListing(IEnumerable<int> joblist, int unit)
        {
            IEnumerable<Models.JobMain> jobMains = db.JobMains.Where(j => joblist.Contains(j.Id))
                .Include(j => j.Customer)
                .Include(j => j.Branch)
                .Include(j => j.JobStatus)
                .Include(j => j.JobThru)
                .Include(j => j.JobEntMains)
                ;

            List<cjobCounter> jobActionCntr = getJobActionCount(jobMains.Select(d => d.Id).ToList());
            var data = new List<cjobUnitIncome>();

            DateClass localTime = new DateClass();
            DateTime today = new DateTime();
            ViewBag.today = today;
            today = localTime.GetCurrentDate();

            cUnitList unitSearch = new cUnitList();
            if (unit != 0) { 
                unitSearch.Id = unit;
                unitSearch.Unit = db.InvItems.Find(unit).Description;
            }
            foreach (var main in jobMains)
            {
                cjobUnitIncome joTmp = new cjobUnitIncome();
                joTmp.Unit = new List<cUnitList>();
                joTmp.Description = "";
                joTmp.Quoted = 0;
                joTmp.Collected = 0;
                joTmp.Expenses = 0;
                joTmp.Payment = 0;
                joTmp.Tour = 0;
                joTmp.Car = 0;
                joTmp.Others = 0;
                joTmp.Id = main.Id;
                joTmp.JobDate = main.JobDate;
                
                List<Models.JobServices> joSvc = db.JobServices.Where(d => d.JobMainId == main.Id).OrderBy(s => s.DtStart).ToList();

                List<cUnitList> unitlist = new List<cUnitList>();

                foreach (var svc in joSvc)
                {
                    if(svc.JobServiceItems.Where(s => s.JobServicesId == svc.Id).ToList() != null)
                    {
                        var svcUnit = svc.JobServiceItems.Where(s => s.JobServicesId == svc.Id).ToList();
                        foreach (var svcUnitList in svcUnit)
                        {
                            unitlist.Add(new cUnitList
                            {
                                Id = svcUnitList.InvItem.Id,
                                Unit = svcUnitList.InvItem.Description,
                                Code = svcUnitList.InvItem.ItemCode,
                                ViewInfo = svcUnitList.InvItem.ViewLabel
                            });
                        }
                    }

                    joTmp.Quoted += svc.ActualAmt != null ? (decimal)svc.ActualAmt : 0;
                    joTmp.Expenses += dbc.getJobExpensesBySVC(svc.Id);
                }

                //unit list
                joTmp.Unit = unitlist;

                //get customer company
                var jobCompany = db.JobEntMains.Where(j => j.JobMainId == main.Id).FirstOrDefault();
                joTmp.Description += jobCompany != null ? jobCompany.CustEntMain.Name + " / " : "";
                joTmp.Description += main.Customer.Name +" / " + main.Description; 
                
                //get lastest posted income
                var latestPosted = db.JobPosts.Where(j => j.JobMainId == main.Id).OrderByDescending(s => s.Id).FirstOrDefault();
                if (latestPosted == null)
                {
                    joTmp.isPosted = false;
                }
                else
                {
                    joTmp.Car = latestPosted.CarRentalInc;
                    joTmp.Tour = latestPosted.TourInc;
                    joTmp.Others = latestPosted.OthersInc;
                    joTmp.isPosted = true;
                }
                
                //get job payment
                List<Models.JobPayment> jobPayment = db.JobPayments.Where(d => d.JobMainId == main.Id).ToList();
                foreach (var payment in jobPayment)
                {
                    joTmp.Payment += payment.PaymentAmt;
                }

                data.Add(joTmp);
            }

            //check unit
            if (unit != 0)
            {
                return data.Where(d => d.Unit.Where(s=>s.Id == unit).FirstOrDefault() != null ).OrderBy(d => d.JobDate).OrderBy(d => d.JobDate);
            }
            else
            {
                return data.OrderBy(d => d.JobDate).OrderBy(d => d.JobDate);
            }
        }
        
        #endregion
    }
}