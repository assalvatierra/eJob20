﻿using JobsV1.Models;
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
        private int NewCustSysId = 1;
        // Job Status
        private int JOBINQUIRY = 1;
        private int JOBRESERVATION = 2;
        private int JOBCONFIRMED = 3;
        private int JOBCLOSED = 4;
        private int JOBCANCELLED = 5;
        private int JOBTEMPLATE = 6;
        
        private JobDBContainer db = new JobDBContainer();
        private DBClasses dbc = new DBClasses();

        // GET: Reporting
        public ActionResult Index(int? id, string reportkey)
        {
            if(id != null)
            {
                ViewBag.Id = id;
            }
            //pass reporting key
            ViewBag.reportkey = reportkey != null ? reportkey : "job";

            ViewBag.Packages = db.CarRatePackages.ToList();
            ViewBag.Group = db.RateGroups.ToList();
            ViewBag.Units = db.CarUnits.ToList();

            return View();
        }


        #region Joblisting

        public PartialViewResult JobListing(int? id,string sDate, string eDate, int? sortid, int? serviceId, int? mainid, string company, string unitDriver)
        {
            var data = getJobData(id, sDate, eDate, sortid, serviceId, mainid, company, unitDriver);

            var jobmainId = serviceId != null ? db.JobServices.Find(serviceId).JobMainId : 0;
            jobmainId = mainid != null ? (int)mainid : jobmainId;

            ViewBag.mainId = jobmainId;
            ViewBag.sDate = sDate;
            ViewBag.eDate = eDate;
            
            if ( id != null )
            {
                return PartialView("JobListing", data.Where(c=>c.Main.Id == id));
            }
            return PartialView("JobListing", data.OrderByDescending(d => d.Main.JobDate));
            
        }

        private List<cJobOrder> getJobData(int? id, string sDate, string eDate,int? sortid, int? serviceId, int? mainid, string company, string unitDriver)
        {

            if (sortid != null)
                Session["FilterID"] = (int)sortid;
            else
            {
                if (Session["FilterID"] != null)
                    sortid = (int)Session["FilterID"];
                else
                    sortid = 1;
            }

            IEnumerable<Models.JobMain> jobMains = db.JobMains
                .Include(j => j.Customer)
                .Include(j => j.Branch)
                .Include(j => j.JobStatus)
                .Include(j => j.JobThru)
                ;

            List<cjobCounter> jobActionCntr = getJobActionCount(jobMains.Select(d => d.Id).ToList());
            var data = new List<cJobOrder>();

            DateTime today = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Singapore Standard Time"));

            ViewBag.today = today;
            today = today.Date;
            
            foreach (var main in jobMains)
            {
                cJobOrder joTmp = new cJobOrder();
                joTmp.Main = main;
                joTmp.Services = new List<cJobService>();
                joTmp.Main.AgreedAmt = 0;
                joTmp.Payment = 0;

                List<Models.JobServices> joSvc = db.JobServices.Where(d => d.JobMainId == main.Id).OrderBy(s => s.DtStart).ToList();
                foreach (var svc in joSvc)
                {
                    cJobService cjoTmp = new cJobService();
                    cjoTmp.Service = svc;

                    //var ActionDone = db.JobActions.Where(d => d.JobServicesId == svc.Id).Select(s => s.SrvActionItemId);

                    //cjoTmp.SvcActions = db.SrvActionItems.Where(d => d.ServicesId == svc.ServicesId && !ActionDone.Contains(d.Id)).Include(d => d.SrvActionCode);
                    //cjoTmp.Actions = db.JobActions.Where(d => d.JobServicesId == svc.Id).Include(d => d.SrvActionItem);
                    cjoTmp.SvcItems = db.JobServiceItems.Where(d => d.JobServicesId == svc.Id).Include(d => d.InvItem);
                    cjoTmp.SupplierPos = db.SupplierPoDtls.Where(d => d.JobServicesId == svc.Id).Include(i => i.SupplierPoHdr);
                    joTmp.Main.AgreedAmt += svc.ActualAmt;

                    joTmp.Expenses += dbc.getJobExpensesBySVC(svc.Id);

                    joTmp.Services.Add(cjoTmp);
                    
                }

                joTmp.ActionCounter = jobActionCntr.Where(d => d.JobId == joTmp.Main.Id).ToList();
                
                //filter unit Driver
                if (unitDriver != "all")
                {
                    var jsvc = db.JobServices.Where(s => s.JobMainId == main.Id)
                        .Where(js=>js.JobServiceItems
                        .Where(j=>j.InvItem.Description.ToLower().Contains(unitDriver.ToLower()) || j.InvItem.ItemCode.ToLower().Contains(unitDriver.ToLower()))
                        .FirstOrDefault().JobServicesId == js.Id);

                        foreach (var temp_items in jsvc)
                        {
                            
                            data.Add(joTmp);
                            break;
                            
                        }
                }
                else
                {
                    //add all/ no filter
                    data.Add(joTmp);
                }
               
                List<Models.JobPayment> jobPayment = db.JobPayments.Where(d => d.JobMainId == main.Id).ToList();
                foreach (var payment in jobPayment)
                {
                    joTmp.Payment += payment.PaymentAmt;
                }
            }

            //company filter
            if (company != null)
            {

                if (company != "all")
                {
                    data = (List<cJobOrder>)data
                        .Where(p => p.Main.Description.ToLower().Contains(company.ToLower()) || p.Main.Customer.Name.ToLower().Contains(company.ToLower()))
                        .ToList();

                }
            }
            /*
            if (unitDriver != null)
            {
                if (unitDriver != "all" || unitDriver != "")
                {
                    data = (List<cJobOrder>)data
                        .Where(j => j.Services.Where(s => s.Service.Service.JobServices.Where(js => js.JobServiceItems.Where(jsi => jsi.InvItem.Description.ToLower().Contains(unitDriver.ToLower())).ToList() != null) != null) != null)
                        .ToList();
                }
            }
            */
            //Date filter
            if (sDate != null || eDate != null)
            {
                DateTime startDateRange = DateTime.Parse(sDate.ToString());
                DateTime endDateRange = DateTime.Parse(eDate.ToString());
                data = (List<cJobOrder>)data
                    .Where(p => (DateTime.Compare( MinJobDate(p.Main.Id).Date, startDateRange.Date) >= 0 && DateTime.Compare(MinJobDate(p.Main.Id).Date, endDateRange.Date) <= 0) ||
                                (DateTime.Compare(MaxJobDate(p.Main.Id).Date, startDateRange.Date) >= 0 && DateTime.Compare(MaxJobDate(p.Main.Id).Date, endDateRange.Date) <= 0)) 
                    .ToList();

            }
            else
            {
                
                data = (List<cJobOrder>)data
                    .Where(p => (DateTime.Compare(MinJobDate(p.Main.Id).Date, today) >= 0 && DateTime.Compare(MinJobDate(p.Main.Id).Date, today) <= 0) ||
                                (DateTime.Compare(MaxJobDate(p.Main.Id).Date, today) >= 0 && DateTime.Compare(MaxJobDate(p.Main.Id).Date, today) <= 0))
                    .ToList();
            }


            return data;
        }

        public DateTime MinJobDate(int mainId)
        {
            //update jobdate
            var main = db.JobMains.Where(j => mainId == j.Id).FirstOrDefault();

            DateTime minDate = new DateTime(9999, 12, 30);
            
            //loop though all jobservices in the jobmain
            //to get the latest date
            foreach (var svc in db.JobServices.Where(s => s.JobMainId == mainId).OrderBy(s => s.DtStart))
            {
                var svcDtStart = (DateTime)svc.DtStart;
                var svcDtEnd = (DateTime)svc.DtEnd;
                //get min date
                // minDate = (DateTime)svc.DtStart;
                if (DateTime.Compare(minDate, svcDtStart.Date) >= 0)
                {
                    minDate = svcDtStart.Date; //if minDate > Dtstart
                }
            }
            
            //return main.JobDate;
            return minDate;
        }
        
        public DateTime MaxJobDate(int mainId)
        {
            //update jobdate
            var main = db.JobMains.Where(j => mainId == j.Id).FirstOrDefault();
            
            DateTime maxDate = new DateTime(1, 1, 1);

            //loop though all jobservices in the jobmain
            //to get the latest date
            foreach (var svc in db.JobServices.Where(s => s.JobMainId == mainId).OrderBy(s => s.DtStart))
            {
                var svcDtStart = (DateTime)svc.DtStart;
                var svcDtEnd = (DateTime)svc.DtEnd;

                //get max date
                if (DateTime.Compare(maxDate, svcDtEnd.Date) <= 0)
                {
                    maxDate = svcDtEnd.Date;
                }
            }
            
            //return main.JobDate;
            return maxDate;
        }
        
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

        /**
         *  Joblist Printing View after filter
         */ 
        public ActionResult JobListingPrint(int? id, string sDate, string eDate, int? sortid, int? serviceId, int? mainid, string company,string unitDriver)
        {
            var data = getJobData(id, sDate, eDate, sortid, serviceId, mainid, company, unitDriver);

            List<Customer> customers = db.Customers.ToList();
            ViewBag.companyList = customers;

            var jobmainId = serviceId != null ? db.JobServices.Find(serviceId).JobMainId : 0;
            jobmainId = mainid != null ? (int)mainid : jobmainId;

            ViewBag.mainId = jobmainId;
            ViewBag.sDate = sDate;
            ViewBag.eDate = eDate;

            if (id != null)
            {
                return View("JobListingPrint", data.Where(c => c.Main.Id == id));
            }
            return View("JobListingPrint", data.OrderByDescending(d => d.Main.JobDate));

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


        #region Income
        public PartialViewResult JobIncome(int? id, string sDate, string eDate, int? sortid, int? serviceId, int? mainid, string company, string unitDriver)
        {

            //Old Open jobs
            var closedJobsIds = dbc.getAllClosedJobs(sDate,eDate).Select(s => s.Id);  //get list of older jobs that are not closed
            var closedJobsList = getJobListing(closedJobsIds);


            DateClass localTime = new DateClass();
            ViewBag.sDate = localTime.GetCurrentDate().AddMonths(-1);
            ViewBag.eDate = localTime.GetCurrentDate();

            return PartialView(closedJobsList);
        }

        private IEnumerable<cJobOrder> getJobListing(IEnumerable<int> joblist)
        {
            IEnumerable<Models.JobMain> jobMains = db.JobMains.Where(j => joblist.Contains(j.Id))
                .Include(j => j.Customer)
                .Include(j => j.Branch)
                .Include(j => j.JobStatus)
                .Include(j => j.JobThru)
                .Include(j => j.JobEntMains)
                ;
            
            List<cjobCounter> jobActionCntr = getJobActionCount(jobMains.Select(d => d.Id).ToList());
            var data = new List<cJobOrder>();

            DateClass localTime = new DateClass();
            DateTime today = new DateTime();
            ViewBag.today = today;
            today = localTime.GetCurrentDate();

            foreach (var main in jobMains)
            {
                cJobOrder joTmp = new cJobOrder();
                joTmp.Main = main;
                joTmp.Services = new List<cJobService>();
                joTmp.Main.AgreedAmt = 0;
                joTmp.Payment = 0;
                joTmp.Id = main.Id;

                List<Models.JobServices> joSvc = db.JobServices.Where(d => d.JobMainId == main.Id).OrderBy(s => s.DtStart).ToList();
                foreach (var svc in joSvc)
                {
                    cJobService cjoTmp = new cJobService();
                    cjoTmp.Service = svc;
                    
                    joTmp.Main.AgreedAmt += svc.ActualAmt != null ? svc.ActualAmt : 0;
                    joTmp.Company = db.JobEntMains.Where(j => j.JobMainId == svc.JobMainId).FirstOrDefault() != null ? db.JobEntMains.Where(j => j.JobMainId == svc.JobMainId).FirstOrDefault().CustEntMain.Name : "";
                    joTmp.Expenses += dbc.getJobExpensesBySVC(svc.Id);
                    
                    joTmp.Services.Add(cjoTmp);
                }

                cjobIncome cIncome = new cjobIncome();
                cIncome.Car = 0;
                cIncome.Tour = 0;
                cIncome.Others = 0;

                var latestPosted = db.JobPosts.Where(j => j.JobMainId == main.Id).OrderByDescending(s => s.Id).FirstOrDefault();
               
                if (latestPosted == null)
                {
                    joTmp.isPosted = false;
                }
                else
                {
                    cIncome.Car    = latestPosted.CarRentalInc;
                    cIncome.Tour   = latestPosted.TourInc ;
                    cIncome.Others = latestPosted.OthersInc;
                    joTmp.isPosted = true;
                }

                joTmp.PostedIncome = cIncome;

                //joTmp.Payment = latestPosted != null ? latestPosted.PaymentAmt : 0;

                joTmp.PostedIncome = cIncome;
                //joTmp.ActionCounter = jobActionCntr.Where(d => d.JobId == joTmp.Main.Id).ToList();

                //joTmp.Main.JobDate = TempJobDate(joTmp.Main.Id);

                List<Models.JobPayment> jobPayment = db.JobPayments.Where(d => d.JobMainId == main.Id).ToList();
                foreach (var payment in jobPayment)
                {
                    joTmp.Payment += payment.PaymentAmt;
                }

                data.Add(joTmp);

            }
            return data.OrderBy(d => d.Main.JobDate).OrderByDescending(d => d.Main.JobDate);
        }

        #endregion
    }
}