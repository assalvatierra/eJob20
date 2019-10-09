﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.Entity;
using JobsV1.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Data.Entity.Core.Objects;
using Newtonsoft.Json;

namespace JobsV1.Controllers
{
    #region support classes
    public class cJobOrder
    {
        [Key]
        public int Id { get; set; }
        public Models.JobMain Main { get; set; }
        public List<cJobService> Services { get; set; }
        public List<cjobCounter> ActionCounter { get; set; }
        public cjobIncome PostedIncome { get; set; }
        public decimal Payment { get; set; }
        public decimal Expenses { get; set; }
        public string  Company { get; set; }
        public bool isPosted { get; set; }
    }

    public class cJobService
    {
        [Key]
        public int Id { get; set; }
        public Models.JobServices Service { get; set; }
        public IQueryable<Models.JobAction> Actions { get; set; }
        public IQueryable<Models.SrvActionItem> SvcActions { get; set; }
        public IQueryable<Models.SrvActionItem> SvcActionsDone { get; set; }
        public IQueryable<Models.JobServiceItem> SvcItems { get; set; }
        public IQueryable<Models.SupplierPoDtl> SupplierPos { get; set; }
    }

    public class cjobCounter
    {
        public int JobId { get; set; }
        public int? CodeId { get; set; }
        public string CodeDesc { get; set; }
        public int CntItem { get; set; }
        public int CntDone { get; set; }
    }

    public class cjobItems
    {
        public int Id { get; set; }
        public string itemCode { get; set; }
        public string Name { get; set; }
        public string icon { get; set; }
        public string remarks { get; set; }
        public int orderNo { get; set; }
    }

    public class cjobIncome
    {
        public int Id { get; set; }
        public decimal Tour { get; set; }
        public decimal Car { get; set; }
        public decimal Others {get;set;}
    }
    
        public class cjobUnitIncome
        {
            public int Id { get; set; }
            public DateTime JobDate { get; set; }
            public List<cUnitList>  Unit { get; set; }
            public string Description { get; set; }
            public decimal Quoted { get; set; }
            public decimal Collected { get; set; }
            public decimal Payment { get; set; }
            public decimal Expenses { get; set; }
            public decimal Tour { get; set; }
            public decimal Car { get; set; }
            public decimal Others { get; set; }
            public bool isPosted { get; set; }

        }

        public class cUnitList
        {
            public int Id { get; set; }
            public string Unit { get; set; }
            public string Code { get; set; }
            public string ViewInfo { get; set; }
        }
    #endregion

    public class JobOrderController : Controller
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
        // GET: JobOrder
        public ActionResult Index(int? sortid, int? serviceId, int? mainid)
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
            
            if (Session["FilterID"] == null)
            {
                Session["FilterID"] = 1;
            }
            
            var data = getJobData((int)sortid); // get job list data

            List<Customer> customers = db.Customers.ToList();
            ViewBag.companyList = customers;

            var jobmainId = serviceId != null ? db.JobServices.Find(serviceId).JobMainId : 0;
            jobmainId = mainid != null ? (int)mainid : jobmainId;
            ViewBag.mainId = jobmainId;

            if (sortid == 1)
            {
                return View(data.OrderBy(d => d.Main.JobDate));
            }
            else
            {
                return View(data.OrderByDescending(d => d.Main.JobDate));
            }
        }
        
        public List<cJobOrder> getJobData(int sortid)
        {
            //IEnumerable<Models.JobMain> jobMains = db.JobMains
            //    .Include(j => j.Customer)
            //    .Include(j => j.Branch)
            //    .Include(j => j.JobStatus)
            //    .Include(j => j.JobThru)
            //    ;

            var confirmed = dbc.getJobConfirmedList(sortid).Select(s=>s.Id);

            IEnumerable<Models.JobMain> jobMains = db.JobMains.Where(j=> confirmed.Contains(j.Id))
                .Include(j => j.Customer)
                .Include(j => j.Branch)
                .Include(j => j.JobStatus)
                .Include(j => j.JobThru)
                ;

            List<cjobCounter> jobActionCntr = getJobActionCount(jobMains.Select(d => d.Id).ToList());
            var data = new List<cJobOrder>();

            DateTime today = new DateTime();
            ViewBag.today = getDateTimeToday();
            today = getDateTimeToday().Date;
            
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

                    var ActionDone = db.JobActions.Where(d => d.JobServicesId == svc.Id).Select(s => s.SrvActionItemId);

                    cjoTmp.SvcActions = db.SrvActionItems.Where(d => d.ServicesId == svc.ServicesId && !ActionDone.Contains(d.Id)).Include(d => d.SrvActionCode);
                    cjoTmp.SvcActionsDone = db.SrvActionItems.Where(d => d.ServicesId == svc.ServicesId && ActionDone.Contains(d.Id)).Include(d => d.SrvActionCode);
                    cjoTmp.Actions = db.JobActions.Where(d => d.JobServicesId == svc.Id).Include(d => d.SrvActionItem);
                    cjoTmp.SvcItems = db.JobServiceItems.Where(d => d.JobServicesId == svc.Id).Include(d => d.InvItem);
                    cjoTmp.SupplierPos = db.SupplierPoDtls.Where(d => d.JobServicesId == svc.Id).Include(i => i.SupplierPoHdr);
                    joTmp.Main.AgreedAmt += svc.ActualAmt;

                    joTmp.Services.Add(cjoTmp);
                }

                joTmp.ActionCounter = jobActionCntr.Where(d => d.JobId == joTmp.Main.Id).ToList();

                //get min job date
                if (sortid == 1)
                {
                    joTmp.Main.JobDate = TempJobDate(joTmp.Main.Id);
                    //joTmp.Main.JobDate = MinJobDate(joTmp.Main.Id);
                }
                else
                {
                    joTmp.Main.JobDate = MinJobDate(joTmp.Main.Id);
                }

                List<Models.JobPayment> jobPayment = db.JobPayments.Where(d => d.JobMainId == main.Id).ToList();
                foreach (var payment in jobPayment)
                {
                    joTmp.Payment += payment.PaymentAmt;
                }

                data.Add(joTmp);
            }
            
            switch (sortid)
            {
                case 1: //OnGoing
                    data = (List<cJobOrder>)data
                        .Where(d => (d.Main.JobStatusId == JOBINQUIRY || d.Main.JobStatusId == JOBRESERVATION || d.Main.JobStatusId == JOBCONFIRMED)).ToList()
                       .Where(d => d.Main.JobDate.CompareTo(today.Date) >= 0).ToList();

                    break;
                case 2: //prev
                    data = (List<cJobOrder>)data
                        .Where(d => (d.Main.JobStatusId == JOBINQUIRY || d.Main.JobStatusId == JOBRESERVATION || d.Main.JobStatusId == JOBCONFIRMED)).ToList()
                        .Where(p => DateTime.Compare(p.Main.JobDate.Date, today.Date) < 0)

                        .ToList();

                    break;
                case 3: //close
                    data = (List<cJobOrder>)data
                        .Where(d => (d.Main.JobStatusId == JOBCLOSED || d.Main.JobStatusId == JOBCANCELLED)).ToList()
                        .Where(p => p.Main.JobDate.Date > today.Date.AddDays(-60)).ToList();
                    break;

                default:
                    data = (List<cJobOrder>)data.ToList();
                    break;
            }

            return data;
        }


        public ActionResult JobStatus(int? sortid, int? serviceId, int? mainid)
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

            if (Session["FilterID"] == null)
            {
                Session["FilterID"] = 1;
            }

            var data = getJobData((int)sortid); // get job list data


            List<Customer> customers = db.Customers.ToList();
            ViewBag.companyList = customers;

            var jobmainId = serviceId != null ? db.JobServices.Find(serviceId).JobMainId : 0;
            jobmainId = mainid != null ? (int)mainid : jobmainId;
            ViewBag.mainId = jobmainId;

            if (sortid == 1)
            {

                return View(data.OrderBy(d => d.Main.JobDate));
            }
            else
            {
                return View(data.OrderByDescending(d => d.Main.JobDate));

            }
        }

        public string getCustomerEmail(int id)
        {
            string custEmail = db.Customers.Find(id).Email;

            return custEmail;
        }

        public string getCustomerNumber(int id)
        {
            string custNum = db.Customers.Find(id).Contact1;

            return custNum;
        }

        public string getCustomerCompany(int id)
        {
            //int companyNum = db.CustEntities.Where(s=>s.CustomerId == id).LastOrDefault() != null ? db.CustEntities.Where(s => s.CustomerId == id).LastOrDefault().CustEntMain.Id : 1;
            var company = db.CustEntities.Where(s => s.CustomerId == id).FirstOrDefault();
            string companyNum = company != null ?  company.CustEntMainId.ToString() : " ";
            return companyNum;
        }

        public DateTime TempJobDate(int mainId)
        {
            //update jobdate
            var main = db.JobMains.Where(j => mainId == j.Id).FirstOrDefault();
            DateTime minDate = db.JobMains.Where(j => mainId == j.Id).FirstOrDefault().JobDate.Date;
            DateTime maxDate = new DateTime(1,1,1);

            DateTime today = new DateTime();
            today = getDateTimeToday().Date;

            //loop though all jobservices in the jobmain
            //to get the latest date
            var counter = 1;
            foreach (var svc in db.JobServices.Where(s => s.JobMainId == mainId).OrderBy(s=>s.DtStart))
            {
                var firstService =(DateTime) db.JobServices.Where(s => s.JobMainId == mainId).OrderBy(s => s.DtStart).FirstOrDefault().DtStart;
                var svcDtStart = (DateTime)svc.DtStart;
                var svcDtEnd = (DateTime)svc.DtEnd;
                //get min date
                if (counter == 1)
                {
                    minDate = firstService;
                }

                // minDate >= (DateTime)svc.DtStart;
                if (DateTime.Compare(minDate, svcDtStart.Date) >= 0) {
                    minDate = svcDtStart.Date; //if minDate > Dtstart
                }

                if (DateTime.Compare(today, svcDtStart.Date) >= 0 && DateTime.Compare(today, svcDtEnd.Date) <= 0)
                {
                    minDate = today; //latest date is today or within the range of start date and end date
                    //skip
                } else {
                    if (DateTime.Compare(today, svcDtStart.Date) < 0 && DateTime.Compare(today, minDate) > 0)
                    {
                        minDate = svcDtStart.Date; //if Today < Dtstart but today is greater than smallest date
                    }
                }
                
                //get max date
                if (DateTime.Compare(maxDate, svcDtEnd.Date) <= 0)
                {
                    maxDate = svcDtEnd.Date; 
                }
            }

            //today is equal to smallest start date
            if (DateTime.Compare(today, minDate) == 0)
            {
                main.JobDate = minDate;
            }

            //today is equal to highest end date
            if (DateTime.Compare(today, maxDate) == 0)
            {
                main.JobDate = maxDate;
            }
            
            //today is < smallest date
            if (DateTime.Compare(today, minDate) < 0) {
                main.JobDate = minDate;
            }

            //today is greater than the smallest date
            //job is currently on going, adjust date
            if (DateTime.Compare(today, minDate) > 0)
            {
                if (DateTime.Compare(today, maxDate) < 0)
                {
                    main.JobDate = today;
                }

                if (DateTime.Compare(today, maxDate) > 0)
                {
                    main.JobDate = minDate;
                }
            }

            if (minDate == new DateTime(9999, 12, 30)) {
                // main.JobDate = db.JobMains.Where(j => mainId == j.Id).FirstOrDefault().JobDate;
                main.JobDate = minDate;
            }

            return main.JobDate;
            //return minDate;
        }


        public DateTime MinJobDate(int mainId)
        {
            //update jobdate
            var main = db.JobMains.Where(j => mainId == j.Id).FirstOrDefault();

            DateTime minDate = main.JobDate;
            DateTime maxDate = new DateTime(1, 1, 1);

            DateTime today = new DateTime();
            today = getDateTimeToday().Date;

            //loop though all jobservices in the jobmain
            //to get the latest date
            foreach (var svc in db.JobServices.Where(s => s.JobMainId == mainId).OrderBy(s => s.DtStart))
            {
             
                var svcDtStart = (DateTime)svc.DtStart;
                var svcDtEnd = (DateTime)svc.DtEnd;
                //get min date

                // minDate >= (DateTime)svc.DtStart;
                if (DateTime.Compare(minDate, svcDtStart.Date) >= 0)
                {
                    minDate = svcDtStart.Date; //if minDate > Dtstart
                }

                if (DateTime.Compare(today, svcDtStart.Date) >= 0 && DateTime.Compare(today, svcDtEnd.Date) <= 0)
                {
                    minDate = today; //latest date is today or within the range of start date and end date
                    //skip
                }
                else
                {
                    if (DateTime.Compare(today, svcDtStart.Date) < 0 && DateTime.Compare(today, minDate) > 0)
                    {
                        minDate = svcDtStart.Date; //if Today < Dtstart but today is greater than smallest date
                    }
                }


                //get max date
                if (DateTime.Compare(maxDate, svcDtEnd.Date) <= 0)
                {
                    maxDate = svcDtEnd.Date;
                }
            }

            //return main.JobDate;
            return minDate;
        }

        public List<cjobCounter> getJobActionCount(List<Int32> jobidlist )
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
            List<cjobCounter> jobcntr = db.Database.SqlQuery<cjobCounter>(sqlstr).Where(d=>jobidlist.Contains(d.JobId)).ToList();
            return jobcntr;
        }


        public ActionResult JobListing(int? sortid, int? serviceId, int? mainid)
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
            
            //get date fom SQL query
            var confirmed = dbc.getJobConfirmedList((int)sortid).Select(s => s.Id);

            IEnumerable<Models.JobMain> jobMains = db.JobMains.Where(j => confirmed.Contains(j.Id))
                .Include(j => j.Customer)
                .Include(j => j.Branch)
                .Include(j => j.JobStatus)
                .Include(j => j.JobThru)
                .Include(j => j.JobEntMains)
                ;
            List<cjobCounter> jobActionCntr = getJobActionCount(jobMains.Select(d => d.Id).ToList());
            var data = new List<cJobOrder>();

            DateTime today = new DateTime();
            ViewBag.today = today;
            today = getDateTimeToday().Date;

            decimal totalRate = 0;
            decimal totalPayment = 0;

            foreach (var main in jobMains)
            {
                cJobOrder joTmp = new cJobOrder();
                joTmp.Main = main;
                joTmp.Services = new List<cJobService>();
                joTmp.Main.AgreedAmt = 0;
                joTmp.Payment = 0;
                joTmp.Expenses = 0;
                
                List<Models.JobServices> joSvc = db.JobServices.Where(d => d.JobMainId == main.Id).OrderBy(s => s.DtStart).ToList();
                foreach (var svc in joSvc)
                {
                    cJobService cjoTmp = new cJobService();
                    cjoTmp.Service = svc;

                    //var ActionDone = db.JobActions.Where(d => d.JobServicesId == svc.Id).Select(s => s.SrvActionItemId);

                    //cjoTmp.SvcActions = db.SrvActionItems.Where(d => d.ServicesId == svc.ServicesId && !ActionDone.Contains(d.Id)).Include(d => d.SrvActionCode);
                    //cjoTmp.Actions = db.JobActions.Where(d => d.JobServicesId == svc.Id).Include(d => d.SrvActionItem);
                    //cjoTmp.SvcItems = db.JobServiceItems.Where(d => d.JobServicesId == svc.Id).Include(d => d.InvItem);
                    //cjoTmp.SupplierPos = db.SupplierPoDtls.Where(d => d.JobServicesId == svc.Id).Include(i => i.SupplierPoHdr);

                    joTmp.Main.AgreedAmt += svc.ActualAmt != null ? svc.ActualAmt : 0;
                    joTmp.Company = db.JobEntMains.Where(j => j.JobMainId == svc.JobMainId).FirstOrDefault() != null ? db.JobEntMains.Where(j=>j.JobMainId == svc.JobMainId).FirstOrDefault().CustEntMain.Name: "";
                    joTmp.Expenses += getJobExpensesBySVC(svc.Id);

                    joTmp.Services.Add(cjoTmp);

                    //calculate total rate and payment
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
                    cIncome.Car = latestPosted.CarRentalInc;
                    cIncome.Tour = latestPosted.TourInc;
                    cIncome.Others = latestPosted.OthersInc;
                    joTmp.isPosted = true;
                }

                joTmp.PostedIncome = cIncome;

                //joTmp.Expenses = getJobExpense(joTmp);

                joTmp.ActionCounter = jobActionCntr.Where(d => d.JobId == joTmp.Main.Id).ToList();
                
                joTmp.Main.JobDate = TempJobDate(joTmp.Main.Id);

                joTmp.Payment = 0;
                List<Models.JobPayment> jobPayment = db.JobPayments.Where(d => d.JobMainId == main.Id).ToList();
                foreach (var payment in jobPayment)
                {
                    joTmp.Payment += payment.PaymentAmt;
                }

                data.Add(joTmp);

            }
            
            switch (sortid)
            {
                case 1: //OnGoing
                    data = (List<cJobOrder>)data
                        .Where(d => (d.Main.JobStatusId == JOBINQUIRY || d.Main.JobStatusId == JOBRESERVATION || d.Main.JobStatusId == JOBCONFIRMED)).ToList()
                        .Where(d => d.Main.JobDate.CompareTo(today.Date) >= 0).ToList();
                    break;
                case 2: //prev
                    data = (List<cJobOrder>)data
                        .Where(d => (d.Main.JobStatusId == JOBINQUIRY || d.Main.JobStatusId == JOBRESERVATION || d.Main.JobStatusId == JOBCONFIRMED)).ToList()
                        .Where(p => DateTime.Compare(p.Main.JobDate.Date, today.Date) < 0).ToList();

                    //Closed and Current Month List
                    var currentMonthIds  = dbc.currentJobsMonth().Select(s => s.Id);   //get list if job ids of current month fom SQL query
                    var currentMonthJobs = getJobListing(currentMonthIds);
                    ViewBag.CurrentMonth = currentMonthJobs;
                    

                    //Old Open jobs
                    var olderJobsIds = dbc.olderOpenJobs().Select(s => s.Id);  //get list of older jobs that are not closed
                    var OldJobs = getJobListing(olderJobsIds);
                    ViewBag.olderOpenJobs = OldJobs;
                    

                    break;
                case 3: //close
                    data = (List<cJobOrder>)data
                        .Where(d => (d.Main.JobStatusId == JOBCLOSED || d.Main.JobStatusId == JOBCANCELLED)).ToList()
                        .Where(p => p.Main.JobDate.AddDays(60).Date > today.Date).ToList();
                    break;

                default:
                    data = (List<cJobOrder>)data.ToList();
                    break;
            }
            
            List<Customer> customers = db.Customers.ToList();
            ViewBag.companyList = customers;

            var jobmainId = serviceId != null ? db.JobServices.Find(serviceId).JobMainId : 0;
            jobmainId = mainid != null ? (int)mainid : jobmainId;
            ViewBag.mainId = jobmainId;
            
            if (sortid == 1)
            {

                return View(data.OrderBy(d => d.Main.JobDate));
            }
            else
            {
                return View(data.OrderByDescending(d => d.Main.JobDate));

            }
        }

        private decimal getTotalRate(IEnumerable<cJobOrder> filteredJob)
        {
            decimal totalRate = 0;
            foreach (var job in filteredJob as IEnumerable<cJobOrder>) {
                    totalRate += job.Main.AgreedAmt != null ? (decimal)job.Main.AgreedAmt : 0 ;
            }
            
            return totalRate;
        }

        private decimal getTotalPayment(IEnumerable<cJobOrder> filteredJob)
        {
            decimal totalPayment = 0;
            foreach (var job in filteredJob as IEnumerable<cJobOrder>)
            {
                totalPayment += job.Payment;
            }

            return totalPayment;
        }

        private decimal getTotalExpenses(IEnumerable<cJobOrder> filteredJob)
        {
            decimal totalExpense = 0;
            List<int> SvcID = new List<int>();

            foreach (var job in filteredJob as IEnumerable<cJobOrder>)
            {
                if (job.Services.Contains(null)) { 
                    foreach (var svc in job as IEnumerable<cJobService>)
                    {
                        SvcID.Add(svc.Id);
                    }
                }
            }

            var expensesList = db.JobExpenses.Where(s=> SvcID.Contains(s.JobServicesId)).ToList();

            foreach (var expense in expensesList as IEnumerable<JobExpenses>)
            {
                totalExpense += expense.Amount;
            }
            
            return totalExpense;
        }

        private decimal getJobExpense(cJobOrder cjob)
        {
            List<int> SvcID = new List<int>();
            var svc = db.JobServices.Where(s => s.JobMainId == cjob.Main.Id).ToList();
            decimal totalExpenses = 0;

            //get service id
            foreach (var svcItems in svc as IEnumerable<JobServices>)
            {
                SvcID.Add(svcItems.Id);
            }

            var expensesList = db.JobExpenses.Where(s=> SvcID.Contains(s.JobServicesId)).ToList();

            foreach (var exp in expensesList as IEnumerable<JobExpenses>)
            {
                totalExpenses += exp.Amount;
            }
            
            return totalExpenses;
        }

        public decimal getJobExpensesBySVC(int svcId)
        {
            decimal total = 0;
            var expense = db.JobExpenses.Where(s => s.JobServicesId == svcId).ToList();
            foreach (var items in expense as IEnumerable<JobExpenses>)
            {
                total += items.Amount;
            }
            return total;
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

            DateTime today = new DateTime();
            ViewBag.today = today;
            today = getDateTimeToday().Date;

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
                    
                    joTmp.Main.AgreedAmt += svc.ActualAmt != null ? svc.ActualAmt : 0 ;
                    joTmp.Company = db.JobEntMains.Where(j => j.JobMainId == svc.JobMainId).FirstOrDefault() != null ? db.JobEntMains.Where(j => j.JobMainId == svc.JobMainId).FirstOrDefault().CustEntMain.Name : "";

                    joTmp.Expenses += getJobExpensesBySVC(svc.Id);

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
                    cIncome.Car = latestPosted.CarRentalInc;
                    cIncome.Tour = latestPosted.TourInc;
                    cIncome.Others = latestPosted.OthersInc;
                    joTmp.isPosted = true;
                }

                joTmp.PostedIncome = cIncome;


                joTmp.ActionCounter = jobActionCntr.Where(d => d.JobId == joTmp.Main.Id).ToList();

                joTmp.Main.JobDate = TempJobDate(joTmp.Main.Id);

                List<Models.JobPayment> jobPayment = db.JobPayments.Where(d => d.JobMainId == main.Id).ToList();
                foreach (var payment in jobPayment)
                {
                    joTmp.Payment += payment.PaymentAmt;
                }

                data.Add(joTmp);

            }
            return data.OrderBy(d => d.Main.JobDate).OrderByDescending(d => d.Main.JobDate);
        }

        //get utc date time today (singapore standard time) gmt + 8
        public DateTime getDateTimeToday()
        {
            DateTime today = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Singapore Standard Time"));

            return today;
        }

        #region Inventory Items
        public ActionResult InventoryItemList(int serviceId)
        {
            var data = db.JobServiceItems.Where(d => d.JobServicesId == serviceId).Include(j => j.InvItem).ToList();
            ViewBag.hdrdata = db.JobServices.Find(serviceId);
            ViewBag.svcId = serviceId;
            return View(data); 
        }

        public ActionResult BrowseInvItem_withSchedule(int JobServiceId)
        {
            DBClasses dbclass = new DBClasses();
            Models.getItemSchedReturn gret = dbclass.ItemSchedules();
            var mainId = db.JobServices.Find(JobServiceId).JobMainId;
            ViewBag.mainId = mainId;
            ViewBag.dtLabel = gret.dLabel;
            ViewBag.serviceId = JobServiceId;
            return View(gret.ItemSched);
        }
        
        //GET : Ajax - getMoreItems()
        //Get the list of InvItems of Order No greater than 110
        [HttpGet]
        public string getMoreItems()
        {
            //get items list of order number greater than 110
            var InvItems = db.InvItems.Where(s => s.OrderNo > 110).Include(s=>s.InvItemCategories).ToList().OrderBy(s => s.OrderNo);
            List<cjobItems> items = new List<cjobItems>();

            foreach ( var inv in InvItems)
            {
                var invIcon = db.InvItemCategories.Where(s => s.InvItemId == inv.Id).FirstOrDefault();
                var imglength = invIcon.InvItemCat.ImgPath.Length;
                items.Add(new cjobItems {
                    Id = inv.Id,
                    itemCode = inv.ItemCode,
                    Name = inv.Description,
                    remarks = inv.Remarks,
                    orderNo = (int)inv.OrderNo,
                    icon = invIcon.InvItemCat.ImgPath.Remove(0,1)
                });
            }
            
            //convert list to json object
            return JsonConvert.SerializeObject(items, Formatting.Indented);
        }
        
        public ActionResult BrowseInvItem_withScheduleJS(int JobServiceId)
        {
            DBClasses dbclass = new DBClasses();
            Models.getItemSchedReturn gret = dbclass.ItemSchedules();
            var mainId = db.JobServices.Find(JobServiceId).JobMainId;
            ViewBag.mainId = mainId;
            ViewBag.dtLabel = gret.dLabel;
            ViewBag.serviceId = JobServiceId;
            return View(gret.ItemSched);
        }

        public void AddUnassignedItem(int itemId, int serviceId)
        {
            string sqlstr = "Insert Into JobServiceItems([JobServicesId],[InvItemId]) values(" + serviceId.ToString() + "," + itemId.ToString() + ")";
            db.Database.ExecuteSqlCommand(sqlstr);
            var mainId = db.JobServices.Find(serviceId).JobMainId;
           
        }

        public void RemoveUnassignedItem(int itemId, int serviceId)
        {
            string sqlstr = "Delete from JobServiceItems where JobServicesId = " + serviceId.ToString()
                + " AND InvItemId = " + itemId.ToString();

            db.Database.ExecuteSqlCommand(sqlstr);
            
        }
        public ActionResult AddItem(int itemId, int serviceId)
        {
            string sqlstr = "Insert Into JobServiceItems([JobServicesId],[InvItemId]) values(" + serviceId.ToString() + "," + itemId.ToString() + ")";
            db.Database.ExecuteSqlCommand(sqlstr);

            //remove unassigned
            var jscount = db.JobServiceItems.Where(s=>s.JobServicesId == serviceId).Count();

            if (jscount > 1)
            {
                var unassigned = db.InvItems.Where(s => s.Description == "UnAssigned").FirstOrDefault().Id;
                RemoveUnassignedItem(unassigned,serviceId);

            }

            var mainId = db.JobServices.Find(serviceId).JobMainId;
            return RedirectToAction("Index", new { serviceId = serviceId });

        }
        public ActionResult JSAddItem(int itemId, int serviceId)
        {
            string sqlstr = "Insert Into JobServiceItems([JobServicesId],[InvItemId]) values(" + serviceId.ToString() + "," + itemId.ToString() + ")";
            db.Database.ExecuteSqlCommand(sqlstr);

            //remove unassigned
            var jscount = db.JobServiceItems.Where(s => s.JobServicesId == serviceId).Count();
            if (jscount > 1)
            {
                var unassigned = db.InvItems.Where(s => s.Description == "UnAssigned").FirstOrDefault().Id;
                RemoveUnassignedItem(unassigned, serviceId);

            }

            var mainId = db.JobServices.Find(serviceId).JobMainId;
            return RedirectToAction("JobServices", new { JobMainId = mainId });

        }

        public ActionResult RemoveItem(int itemId, int serviceId)
        {
            string sqlstr = "Delete from JobServiceItems where JobServicesId = " + serviceId.ToString()
                + " AND InvItemId = " + itemId.ToString();

            db.Database.ExecuteSqlCommand(sqlstr);

            return RedirectToAction("InventoryItemList", new { serviceId = serviceId });
        }

        public ActionResult JsRemoveItem(int itemId, int serviceId)
        {
            string sqlstr = "Delete from JobServiceItems where JobServicesId = " + serviceId.ToString()
                + " AND InvItemId = " + itemId.ToString();

            db.Database.ExecuteSqlCommand(sqlstr);

            var mainId = db.JobServices.Find(serviceId).JobMainId;
            return RedirectToAction("JobServices", new { JobMainId = mainId });
        }
        #endregion

        #region Customer Detail
        private List<SelectListItem> StatusList = new List<SelectListItem> {
                new SelectListItem { Value = "ACT", Text = "Active" },
                new SelectListItem { Value = "INC", Text = "Inactive" },
                new SelectListItem { Value = "BAD", Text = "Bad Account" }
                };

        public ActionResult CompanyDetail(int jobid, int custid)
        {
            var data = db.Customers.Find(custid);

            if (data.Name == "<< New Customer >>")
            {
                return RedirectToAction("CreateCustomer", new { CreateCustJobId = jobid } );
            }

            ViewBag.Status = new SelectList(StatusList, "value", "text", data.Status);
            ViewBag.mainid = jobid;
            return View(data);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CompanyDetail([Bind(Include = "Id,Name,Email,Contact1,Contact2,Remarks,Status")] Customer customer, int mainid, int? sortid)
        {
            
            if (ModelState.IsValid)
            {
                db.Entry(customer).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index", new { mainid = mainid});
            }

            ViewBag.Status = new SelectList(StatusList, "value", "text", customer.Status);

            return View(customer);
        }
        public ActionResult CreateCustomer(int CreateCustJobId)
        {
            var jobCust = db.JobMains.Find(CreateCustJobId);
            var data = new Models.Customer();
            data.Name = jobCust.Description;
            data.Email = jobCust.CustContactEmail;
            data.Contact1 = jobCust.CustContactNumber;

            data.Status = "ACT";

            ViewBag.Status = new SelectList(StatusList, "value", "text", data.Status);
            ViewBag.Status = new SelectList("JobStatusId", "Id", "text", data.Status);
            ViewBag.jobOrderId = CreateCustJobId;
          
            return View(data);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateCustomer([Bind(Include = "Id,Name,Email,Contact1,Contact2,Remarks,Status")] Customer customer, int? sortid)
        {

            if (ModelState.IsValid)
            {
                if (customer.Status == null || customer.Status.Trim() == "") customer.Status = "ACT";

                db.Customers.Add(customer);
                db.SaveChanges();
                
                string JobId = Request.Form["jobOrderId"];
                db.Database.ExecuteSqlCommand(@"
                    Update JobMains set CustomerId=" + customer.Id + " where Id=" + JobId + ";"
                    );

                return RedirectToAction("Index");
            }

            ViewBag.Status = new SelectList(StatusList, "value", "text", customer.Status);
            return View(customer);
        }
        #endregion

        #region jobMain
        public ActionResult JobDetails(int jobid)
        {
            var jobMain = db.JobMains.Find(jobid);
            var companyId = db.JobEntMains.Where(s => s.JobMainId == jobMain.Id).FirstOrDefault() != null ?
                db.JobEntMains.Where(s => s.JobMainId == jobMain.Id).FirstOrDefault().CustEntMainId : 1 ;
            //var companyId = db.JobEntMains.Where(s => s.JobMainId == jobid).FirstOrDefault().Id;
            ViewBag.mainid = jobid;
            ViewBag.CustomerId  = new SelectList(db.Customers.Where(d => d.Status == "ACT"), "Id", "Name", jobMain.CustomerId);
            ViewBag.BranchId    = new SelectList(db.Branches, "Id", "Name", jobMain.BranchId);
            ViewBag.JobStatusId = new SelectList(db.JobStatus, "Id", "Status", jobMain.JobStatusId);
            ViewBag.JobThruId = new SelectList(db.JobThrus, "Id", "Desc", jobMain.JobThruId);
            ViewBag.CompanyId = new SelectList(db.CustEntMains, "Id", "Name", companyId);
            //jobMain.AgreedAmt = 2000;
            return View(jobMain);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult JobDetails([Bind(Include = "Id,JobDate,CompanyId,CustomerId,Description,NoOfPax,NoOfDays,JobRemarks,JobStatusId,StatusRemarks,BranchId,JobThruId,CustContactEmail,CustContactNumber")] JobMain jobMain, int? CompanyId, decimal? AgreedAmt)
        {
            if (ModelState.IsValid)
            {
                if (jobMain.CustContactEmail == null && jobMain.CustContactNumber == null)
                {
                    var cust = db.Customers.Find(jobMain.CustomerId);
                    jobMain.CustContactEmail = cust.Email;
                    jobMain.CustContactNumber = cust.Contact1;
                }

                //Console.WriteLine("AgreedAmt: "+AgreedAmt);
                System.Diagnostics.Debug.WriteLine("AgreedAmt job: " + jobMain.AgreedAmt);
                System.Diagnostics.Debug.WriteLine("AgreedAmt: " + AgreedAmt);

                jobMain.AgreedAmt = AgreedAmt;
                db.Entry(jobMain).State = EntityState.Modified;
                db.SaveChanges();

                System.Diagnostics.Debug.WriteLine("----" );
                System.Diagnostics.Debug.WriteLine("AgreedAmt job: " + jobMain.AgreedAmt);
                System.Diagnostics.Debug.WriteLine("AgreedAmt: " + AgreedAmt);
                EditjobCompany(jobMain.Id, (int)CompanyId);
               
                return RedirectToAction("JobServices", new { JobMainId = jobMain.Id });
            }
            
          
            ViewBag.CustomerId = new SelectList(db.Customers.Where(d => d.Status != "INC"), "Id", "Name", jobMain.CustomerId);
            ViewBag.BranchId = new SelectList(db.Branches, "Id", "Name", jobMain.BranchId);
            ViewBag.JobStatusId = new SelectList(db.JobStatus, "Id", "Status", jobMain.JobStatusId);
            ViewBag.JobThruId = new SelectList(db.JobThrus, "Id", "Desc", jobMain.JobThruId);
            ViewBag.CompanyId = new SelectList(db.CustEntMains, "Id", "Name", CompanyId);
            return View(jobMain);
        }

        public void EditjobCompany(int jobId, int companyId)
        {
            //AddjobCompany(jobId, companyId);
            if (db.JobEntMains.Where(j => j.JobMainId == jobId).FirstOrDefault() == null )
            {
                //add if entry does not exist
                AddjobCompany(jobId,companyId);
            }
            else
            {
                //save changes if entry does not exist
                JobEntMain jobCompany = db.JobEntMains.Where(j => j.JobMainId == jobId).FirstOrDefault();
                jobCompany.JobMainId = jobId;
                jobCompany.CustEntMainId = companyId;

                db.Entry(jobCompany).State = EntityState.Modified;
                db.SaveChanges();
            }

        }

        // GET: JobMains/jobCreate
        public ActionResult jobCreate(int? id)
        {

            DateClass today = new DateClass();

            JobMain job = new JobMain();
            job.JobDate = today.GetCurrentDateTime().AddDays(1);
            job.NoOfDays = 1;
            job.NoOfPax = 1;
            job.AgreedAmt = 0;

            if (id == null)
            {
                ViewBag.CustomerId = new SelectList(db.Customers.Where(d => d.Status == "ACT" ), "Id", "Name", NewCustSysId);
            }
            else
            {
                ViewBag.CustomerId = new SelectList(db.Customers.Where(d => d.Status == "ACT"), "Id", "Name", id);
            }

            ViewBag.CustomerList = db.Customers.Where(s => s.Status == "ACT").ToList();
            ViewBag.CompanyId = new SelectList(db.CustEntMains, "Id", "Name");
            ViewBag.BranchId = new SelectList(db.Branches, "Id", "Name",2);
            ViewBag.JobStatusId = new SelectList(db.JobStatus, "Id", "Status", JOBCONFIRMED);
            ViewBag.JobThruId = new SelectList(db.JobThrus, "Id", "Desc");
            
            return View(job);
        }

        
        // POST: JobMains/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult jobCreate([Bind(Include = "Id,JobDate,CustomerId,Description,NoOfPax,NoOfDays,AgreedAmt,JobRemarks,JobStatusId,StatusRemarks,BranchId,JobThruId,CustContactEmail,CustContactNumber")] JobMain jobMain, int? CompanyId)
        {
            if (ModelState.IsValid)
            {
                if (jobMain.CustContactEmail == null && jobMain.CustContactNumber == null)
                {
                    var cust = db.Customers.Find(jobMain.CustomerId);
                    jobMain.CustContactEmail = cust.Email;
                    jobMain.CustContactNumber = cust.Contact1;
                }

                db.JobMains.Add(jobMain);
                db.SaveChanges();

                if (CompanyId != null)
                {
                    AddjobCompany(jobMain.Id, (int)CompanyId);
                }

                dbc.addEncoderRecord("joborder", jobMain.Id.ToString(), HttpContext.User.Identity.Name, "Create New Job");
                return RedirectToAction("JobServices", "JobOrder", new { JobMainId = jobMain.Id });

            }

            ViewBag.CompanyId = new SelectList(db.CustEntMains, "Id", "Name");
            ViewBag.CustomerId = new SelectList(db.Customers.Where(d => d.Status != "INC"), "Id", "Name", jobMain.CustomerId);
            ViewBag.BranchId = new SelectList(db.Branches, "Id", "Name", jobMain.BranchId);
            ViewBag.JobStatusId = new SelectList(db.JobStatus, "Id", "Status", jobMain.JobStatusId);
            ViewBag.JobThruId = new SelectList(db.JobThrus, "Id", "Desc", jobMain.JobThruId);
            return View(jobMain);
        }

        public void AddjobCompany(int jobId, int companyId)
        {
            JobEntMain jobCompany = new JobEntMain();
            jobCompany.JobMainId = jobId;
            jobCompany.CustEntMainId = companyId;

            db.JobEntMains.Add(jobCompany);
            db.SaveChanges();
        }
        

        public ActionResult ChangeCompany(int id , int newId) {

            JobMain jobMain = db.JobMains.Find(id);
            jobMain.CustomerId = newId;
            db.Entry(jobMain).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Index", new { mainid = jobMain.Id});
        }
        #endregion

        #region Supplier Po
        public ActionResult InitializePO(int srcId)
        {
            var srcdata = db.JobServices.Find(srcId);
            var tmp = new Models.SupplierPoHdr();
            tmp.PoDate = DateTime.Now;
            tmp.Remarks = srcdata.Particulars;
            tmp.RequestBy = User.Identity.Name;
            tmp.DtRequest = DateTime.Now;

            ViewBag.SupplierId = new SelectList(db.Suppliers, "Id", "Name");
            ViewBag.SupplierPoStatusId = new SelectList(db.SupplierPoStatus, "Id", "Status");
            ViewBag.SrcId = srcId;

            return View(tmp);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult InitializePO([Bind(Include = "Id,PoDate,Remarks,SupplierId,SupplierPoStatusId,RequestBy,DtRequest,ApprovedBy,DtApproved")] SupplierPoHdr supplierPoHdr)
        {
            string strSrcId = Request.Form["SrcId"];
            int SrcId = Int32.Parse(strSrcId);
            string strAmt = Request.Form["Amount"];
            decimal PoAmt = decimal.Parse(strAmt);

            if (ModelState.IsValid)
            {
                db.SupplierPoHdrs.Add(supplierPoHdr);
                db.SaveChanges();

                #region Add Details
                var tmp = new Models.SupplierPoDtl();
                tmp.SupplierPoHdrId = supplierPoHdr.Id;
                tmp.Remarks = supplierPoHdr.Remarks;
                tmp.JobServicesId = SrcId;
                tmp.Amount = PoAmt;
                db.SupplierPoDtls.Add(tmp);
                db.SaveChanges();
                #endregion

                return RedirectToAction("Index", new { sortid= 1, serviceId = SrcId });
            }

            ViewBag.SupplierId = new SelectList(db.Suppliers, "Id", "Name", supplierPoHdr.SupplierId);
            ViewBag.SupplierPoStatusId = new SelectList(db.SupplierPoStatus, "Id", "Status", supplierPoHdr.SupplierPoStatusId);
            return View(supplierPoHdr);
        }

        public ActionResult AddSupplierPODetails(int srcId, int pohdrId)
        {
            var srcdata = db.JobServices.Find(srcId);
            var tmp = new Models.SupplierPoDtl();
            tmp.SupplierPoHdrId = pohdrId;
            tmp.Remarks = srcdata.Particulars;
            tmp.JobServicesId = srcId;
            tmp.Amount = 0;

            return View(tmp);
        }

        #endregion

        #region Services
        public ActionResult JobServiceAdd(int? JobMainId) {
            Models.JobMain job = db.JobMains.Find((int)JobMainId);
            Models.JobServices js = new JobServices();
            js.JobMainId = (int)JobMainId;

            DateTime dtTmp = new DateTime(job.JobDate.Year, job.JobDate.Month, job.JobDate.Day, 8, 0, 0);
            js.DtStart = dtTmp;
            js.DtEnd = dtTmp.AddDays(job.NoOfDays - 1).AddHours(10);
            js.Remarks = "10hrs use per day P300/hr in excess, Driver and Fuel Included";
            js.ActualAmt = 0;
            js.QuotedAmt = 0;
            js.SupplierAmt = 0;
            
            //modify SupplierItem
            var supItemsActive = db.SupplierItems.Where(s => s.Status != "INC").ToList();
            var SuppliersActive = db.Suppliers.Where(s => s.Status != "INC").ToList();
            
            ViewBag.id = JobMainId;
            ViewBag.JobMainId = new SelectList(db.JobMains, "Id", "Description",job.Description);
            ViewBag.SupplierId = new SelectList(SuppliersActive, "Id", "Name");
            ViewBag.SupplierItemId = new SelectList(supItemsActive, "Id", "Description");
            ViewBag.ServicesId = new SelectList(db.Services.Where(s=>s.Status == "1").ToList(), "Id", "Name");
            //ViewBag.SupplierItemId = new SelectList(db.SupplierItems, "Id", "Description");
            return View(js);
        }


        // POST: JobServices/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult JobServiceAdd([Bind(Include = "Id,JobMainId,ServicesId,SupplierId,DtStart,DtEnd,Particulars,QuotedAmt,SupplierAmt,ActualAmt,Remarks,SupplierItemId")] JobServices jobServices)
        {
            if (ModelState.IsValid)
            {
                jobServices.DtEnd = ((DateTime)jobServices.DtEnd).Add(new TimeSpan(23, 59, 59));
                db.JobServices.Add(jobServices);
                db.SaveChanges();

                //set initial unit as unassigned
                int UnassignedId = db.InvItems.Where(u => u.Description == "UnAssigned").FirstOrDefault().Id;
                AddUnassignedItem(UnassignedId, jobServices.Id);
            }

            var supItemsActive = db.SupplierItems.Where(s => s.Status != "INC").ToList();
            var SuppliersActive = db.Suppliers.Where(s => s.Status != "INC").ToList();


            ViewBag.JobMainId = new SelectList(db.JobMains, "Id", "Description", jobServices.JobMainId);
            ViewBag.SupplierId = new SelectList(SuppliersActive, "Id", "Name", jobServices.SupplierId);
            ViewBag.ServicesId = new SelectList(db.Services.Where(s => s.Status == "1").ToList(), "Id", "Name", jobServices.ServicesId);
            ViewBag.SupplierItemId = new SelectList(supItemsActive, "Id", "Description", jobServices.SupplierItemId);

            dbc.addEncoderRecord("jobservice", jobServices.Id.ToString(), HttpContext.User.Identity.Name, "Create New Job Service");

            return RedirectToAction("JobServices", "JobOrder", new { JobMainId = jobServices.JobMainId });
        }

        // GET: JobServices/Edit/5
        public ActionResult JobServiceEdit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobServices jobServices = db.JobServices.Find(id);
            if (jobServices == null)
            {
                return HttpNotFound();
            }

            var supItemsActive = db.SupplierItems.Where(s => s.Status != "INC").ToList();
            var SuppliersActive = db.Suppliers.Where(s => s.Status != "INC").ToList();

            ViewBag.svcId = jobServices.Id;

            ViewBag.JobMainId = new SelectList(db.JobMains, "Id", "Description", jobServices.JobMainId);
            ViewBag.SupplierId = new SelectList(SuppliersActive, "Id", "Name", jobServices.SupplierId);
            ViewBag.ServicesId = new SelectList(db.Services.Where(s => s.Status == "1").ToList(), "Id", "Name", jobServices.ServicesId);
            ViewBag.SupplierItemId = new SelectList(supItemsActive, "Id", "Description", jobServices.SupplierItemId);
            return View(jobServices);
        }

        // POST: JobServices/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult JobServiceEdit([Bind(Include = "Id,JobMainId,ServicesId,SupplierId,DtStart,DtEnd,Particulars,QuotedAmt,SupplierAmt,ActualAmt,Remarks,SupplierItemId")] JobServices jobServices)
        {
            if (ModelState.IsValid)
            {
                jobServices.DtEnd = ((DateTime)jobServices.DtEnd).Add(new TimeSpan(23, 59, 59));
                db.Entry(jobServices).State = EntityState.Modified;

                DateTime dtSvc = (DateTime)jobServices.DtStart;
                List<Models.JobItinerary> iti = db.JobItineraries.Where(d => d.JobMainId == jobServices.JobMainId && d.SvcId == jobServices.Id).ToList();
                foreach (var ititmp in iti)
                {
                    int iHr = dtSvc.Hour, iMn = dtSvc.Minute;
                    if (ititmp.ItiDate != null)
                    {
                        DateTime dtIti = (DateTime)ititmp.ItiDate;
                        iHr = dtIti.Hour;
                        iMn = dtIti.Minute;
                    }
                    ititmp.ItiDate = new DateTime(dtSvc.Year, dtSvc.Month, dtSvc.Day, iHr, iMn, 0);
                    db.Entry(ititmp).State = EntityState.Modified;
                }

                //db.SaveChanges();
                updateJobDate(jobServices.JobMainId);
                db.SaveChanges();

            }

            var supItemsActive = db.SupplierItems.Where(s => s.Status != "INC").ToList();
            var SuppliersActive = db.Suppliers.Where(s => s.Status != "INC").ToList();


            ViewBag.JobMainId = new SelectList(db.JobMains, "Id", "Description", jobServices.JobMainId);
            ViewBag.SupplierId = new SelectList(SuppliersActive, "Id", "Name", jobServices.SupplierId);
            ViewBag.ServicesId = new SelectList(db.Services.Where(s => s.Status == "1").ToList(), "Id", "Name", jobServices.ServicesId);
            ViewBag.SupplierItemId = new SelectList(supItemsActive, "Id", "Description", jobServices.SupplierItemId);

            return RedirectToAction("JobServices", "JobOrder", new { JobMainId = jobServices.JobMainId });

        }

        public void updateJobDate(int mainId) {
            
            //update jobdate
            var main = db.JobMains.Where(j => mainId == j.Id).FirstOrDefault();

            DateTime today = new DateTime();
            today = getDateTimeToday().Date;

            //loop though all jobservices in the jobmain
            //to get the latest date
            foreach (var svc in db.JobServices.Where(s => s.JobMainId == main.Id))
            {

                //get least latest date
                if (DateTime.Compare(today, (DateTime)svc.DtStart) >= 0)   //if today is later than datestart, assign datestart to jobdate, 
                {

                    if (DateTime.Compare(today, (DateTime)svc.DtEnd) <= 0) //if today earlier than date end, assign jobdate today
                    {
                        //assign date today
                        main.JobDate = DateTime.Today;
                        db.Entry(main).State = EntityState.Modified;
                    }
                    else
                    {
                        //assign latest basin on today
                        main.JobDate = (DateTime)svc.DtStart;
                        db.Entry(main).State = EntityState.Modified;
                    }

                }
                else  //if today is earlier than datestart, assign datestart to jobdate, 
                {
                    //main.JobDate = (DateTime)svc.DtStart;
                    //db.Entry(main).State = EntityState.Modified;
                }
                //db.SaveChanges();
            }
        }

        // GET: JobServices/Details/5
        public ActionResult JobServiceDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobServices jobServices = db.JobServices.Find(id);
            if (jobServices == null)
            {
                return HttpNotFound();
            }
            ViewBag.svcId = id;
            return View(jobServices);
        }


        public ActionResult JobSvcDelete(int? id) {

            JobServices jobServices = db.JobServices.Find(id);
            int jId = jobServices.JobMainId;

            //remove jobservice pickup on job service pickups
            JobServicePickup jobpickup = db.JobServicePickups.Where(j => j.JobServicesId == id).FirstOrDefault();

            if (jobpickup != null) {
                db.JobServicePickups.Remove(jobpickup);
                db.SaveChanges();
            }


            //remove jobservice items
            var jobitems = db.JobServiceItems.Where(i => i.JobServicesId == id).ToList();
            if (jobitems != null) {
                db.JobServiceItems.RemoveRange(jobitems);
                db.SaveChanges();
            }
            
            db.JobServices.Remove(jobServices);
            db.SaveChanges();

            return RedirectToAction("JobServices", "JobOrder", new { JobMainId = jobServices.JobMainId});
        }

        public ActionResult notify() {
            DBClasses dbc = new DBClasses();
            dbc.addNotification("Job Order","Test");
            return RedirectToAction("Index", "JobOrder", new { sortid = 1 });
        }


        public ActionResult InitServicePickup(int? id)
        {
            Models.JobServicePickup svcpu;

            Models.JobServices svc = db.JobServices.Find(id);
            if (svc.JobServicePickups.FirstOrDefault() == null)
            {
                svcpu = new JobServicePickup();
                svcpu.JobServicesId = svc.Id;
                svcpu.JsDate = svc.JobMain.JobDate;
                svcpu.JsTime = svc.JobMain.JobDate.ToString("hh:mm:00");
                svcpu.ClientName = svc.JobMain.Description;
                svcpu.ClientContact = svc.JobMain.CustContactNumber;
                svcpu.ProviderName = svc.SupplierItem.InCharge;
                svcpu.ProviderContact = svc.SupplierItem.Tel1
                    + (svc.SupplierItem.Tel2 == null ? "" : "/" + svc.SupplierItem.Tel2)
                    + (svc.SupplierItem.Tel3 == null ? "" : "/" + svc.SupplierItem.Tel3);

                db.JobServicePickups.Add(svcpu);
                db.SaveChanges();
            }
            else
            {
                svcpu = svc.JobServicePickups.FirstOrDefault();
            }

            return RedirectToAction("JobServicePickup", new { id = svcpu.Id });
        }


        public ActionResult JobServicePickup(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobServicePickup jobServicePickup = db.JobServicePickups.Find(id);
            if (jobServicePickup == null)
            {
                return HttpNotFound();
            }

            ViewBag.Contact = db.JobContacts.Where(d => d.ContactType == "100").ToList();
            ViewBag.svcId = jobServicePickup.JobServicesId;
            return View(jobServicePickup);
        }


        [HttpPost, ActionName("JobServicePickup")]
        public ActionResult JobServicePickup([Bind(Include = "Id,JobServicesId,JsDate,JsTime,JsLocation,ClientName,ClientContact,ProviderName,ProviderContact")] JobServicePickup jobServicePickup)
        {
            if (ModelState.IsValid)
            {
                db.Entry(jobServicePickup).State = EntityState.Modified;
                db.SaveChanges();

                int ij = db.JobServices.Find(jobServicePickup.JobServicesId).JobMainId;

                return RedirectToAction("JobServices", new { JobMainId = jobServicePickup.JobService.JobMainId });
            }
            //ViewBag.JobServicesId = new SelectList(db.JobServices, "Id", "Particulars", jobServicePickup.JobServicesId);

            return View(jobServicePickup);

        }


        public ActionResult JobServices(int? JobMainId, int? serviceId, int? sortid, string action)
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

            ViewBag.JobMainId = JobMainId;

            var Job = db.JobMains.Where(d => d.Id == JobMainId).FirstOrDefault();
            ViewBag.JobOrder = Job;

            var jobServices = db.JobServices.Include(j => j.JobMain).Include(j => j.Supplier).Include(j => j.Service).Include(j => j.SupplierItem).Include(j => j.JobServicePickups).Where(d => d.JobMainId == JobMainId);

            System.Collections.ArrayList providers = new System.Collections.ArrayList();
            foreach (var item in jobServices)
            {
                if (item.JobServicePickups != null)
                {
                    string sTmp = "";
                    try
                    {
                        sTmp = item.JobServicePickups.FirstOrDefault().ProviderName;
                    }
                    catch
                    {
                        sTmp = "Pickup Details / Provider not defined.";
                    }

                    if (!providers.Contains(sTmp))
                    {
                        providers.Add(sTmp);
                    }
                }
            }

            if (db.JobTrails.Where(s => s.RefTable == "joborder" && s.RefId == JobMainId.ToString()).FirstOrDefault() != null)
            {
                ViewBag.JobEncoder = db.JobTrails.Where(s => s.RefTable == "joborder" && s.RefId == JobMainId.ToString()).FirstOrDefault();
            }
            else {
                ViewBag.JobEncoder = new JobTrail { Id = 0, Action = "Create", user = "none", dtTrail = DateTime.Now, RefId = "0", RefTable = "none" };
            }

            ViewBag.JobItems = jobServices;

            ViewBag.Providers = providers;

            ViewBag.JobStatus = db.JobMains.Where(j=>j.Id == JobMainId).FirstOrDefault().JobStatus.Status.ToString();

            ViewBag.Itineraries = db.JobItineraries.Where(d => d.JobMainId == JobMainId).ToList();

            ViewBag.sortid = sortid;

            ViewBag.jobAction = action;

            return View(jobServices.OrderBy(d => d.DtStart).ToList());

        }

        public ActionResult BookingRedirect(int id,string month, string day, string year,string rName) {
            String DateBook = month + "/" + day + "/" + year;
            DateTime booking = DateTime.Parse(DateBook);
            int iMonth = int.Parse(month);
            int iday = int.Parse(day);
            int iyear = int.Parse(year);

            JobMain job = db.JobMains.Where(j => j.Id == id).
                //Where(j=> j.JobDate.Month == iMonth && j.JobDate.Day == iday && j.JobDate.Year == iyear).FirstOrDefault();
                Where(j => j.JobDate.Month == iMonth).
                Where(j => j.JobDate.Day == iday).
                Where(j => j.JobDate.Year == iyear).
                Where(j => j.Description == rName).
                FirstOrDefault();

            //return RedirectToAction("BookingDetails", new { id = job.Id , iType = 1});
              
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var jobMain = job;
            if (jobMain == null)
            {
                return HttpNotFound();
            }

            ViewBag.Services = db.JobServices.Include(j => j.JobServicePickups).Where(j => j.JobMainId == jobMain.Id).OrderBy(s => s.DtStart);
            ViewBag.Itinerary = db.JobItineraries.Include(j => j.Destination).Where(j => j.JobMainId == jobMain.Id);
            ViewBag.Payments = db.JobPayments.Where(j => j.JobMainId == jobMain.Id);
            ViewBag.jNotes = db.JobNotes.Where(d => d.JobMainId == jobMain.Id).OrderBy(s => s.Sort);

            //Default form
            string sCompany = "AJ88 Car Rental Services";
            string sLine1 = "2nd Floor J. Sulit Bldg. Mac Arthur Highway, Matina Davao City ";
            string sLine2 = "Tel# (+63)822971831; (+63)9167558473; (+63)9330895358 ";
            string sLine3 = "Email: ajdavao88@gmail.com; Website: http://www.AJDavaoCarRental.com/";
            string sLine4 = "TIN: 414-880-772-001 (non-Vat)";
            string sLogo = "LOGO_AJRENTACAR.jpg";
            Bank bank = db.Banks.Find(2);

            if (jobMain.Branch.Name == "RealBreeze")
            {
                sCompany = "Real Breeze Travel & Tours - Davao City";
                sLine1 = "2nd Floor J. Sulit Bldg. Mac Arthur Highway, Matina Davao City ";
                sLine2 = "Tel# (+63)822971831; (+63)916-755-8473; (+63)933-089-5358 ";
                sLine3 = "Email: RealBreezeDavao@gmail.com; Website: http://www.realbreezedavaotours.com//";
                sLine4 = "TIN: 414-880-772-000 (non-Vat)";
                sLogo = "RealBreezeLogo01.png";
                bank = db.Banks.Find(3);
            }

            if (jobMain.Branch.Name == "AJ88")
            {
                sCompany = "AJ88 Car Rental Services";
                sLine1 = "2nd Floor J. Sulit Bldg. Mac Arthur Highway, Matina Davao City ";
                sLine2 = "Tel# (+63)822971831; (+63)9167558473; (+63)9330895358 ";
                sLine3 = "Email: ajdavao88@gmail.com; Website: http://www.AJDavaoCarRental.com/";
                sLine4 = "TIN: 414-880-772-001 (non-Vat)";
                sLogo = "LOGO_AJRENTACAR.jpg";
                bank = db.Banks.Find(2);
            }

            ViewBag.sCompany = sCompany;
            ViewBag.sLine1 = sLine1;
            ViewBag.sLine2 = sLine2;
            ViewBag.sLine3 = sLine3;
            ViewBag.sLine4 = sLine4;
            ViewBag.sLogo = sLogo;

            ViewBag.BankName = bank.BankName;
            ViewBag.BankBranch = bank.BankBranch;
            ViewBag.AccName = bank.AccntName;
            ViewBag.AccNum = bank.AccntNo;

            ViewBag.rsvId = id;
            ViewBag.CarDesc = "Test Unit";
            ViewBag.ReservationType = "Rental";
            ViewBag.Amount = 1000;
            
            string custCompany = "";
            //check customer if assigned to a company
            if (jobMain.Customer.CustEntities.Where(c => c.CustomerId == jobMain.CustomerId).FirstOrDefault() != null)
            {
                custCompany = jobMain.Customer.CustEntities.Where(c => c.CustomerId == jobMain.CustomerId).FirstOrDefault().CustEntMain.Name;

                //if (custCompany == "NEW (not yet defined)")
                //{
                //    custCompany = " ";
                //}

            }

             

            ViewBag.custCompany = custCompany;
            
            //get paypal keys at db
            PaypalAccount paypal = db.PaypalAccounts.Where(p => p.SysCode.Equals("RealWheels")).FirstOrDefault();
            ViewBag.key = paypal.Key;

            DateTime today = new DateTime();
            today = getDateTimeToday().Date;
            ViewBag.DateNow = today.AddMonths(1);
            ViewBag.isPaymentValid = (jobMain.JobDate.Date == today) || (jobMain.JobDate.Date == today.AddDays(1).Date) ? "True" : "False";


            //handle prepared by
            var encoder = db.JobTrails.Where(s => s.RefTable == "joborder" && s.RefId == jobMain.Id.ToString()).FirstOrDefault();
            ViewBag.StaffName = getStaffName(encoder.user);
            ViewBag.Sign = getStaffSign(encoder.user);

            return View("Details_Invoice", jobMain);
        }
        
        // GET: JobMains/Details/5
        public ActionResult BookingDetails(int? id, int? iType)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var jobMain = db.JobMains.Find(id);
            if (jobMain == null)
            {
                return HttpNotFound();
            }

            string custCompany = "";
            //check customer if assigned to a company
            if (jobMain.JobEntMains.Where(c => c.JobMainId == jobMain.Id).FirstOrDefault() != null)
            {
                var company = jobMain.JobEntMains.Where(c => c.JobMainId == jobMain.Id).FirstOrDefault().CustEntMain;

                //hide company name if company is 1 = New (not defined)
                if (company.Id == 1)
                {
                    custCompany = " ";
                }
                else
                {
                    custCompany = jobMain.JobEntMains.Where(c => c.JobMainId == jobMain.Id).FirstOrDefault().CustEntMain.Name;
                }
            }
            
            ViewBag.Services = db.JobServices.Include(j => j.JobServicePickups).Where(j => j.JobMainId == jobMain.Id).OrderBy(s => s.DtStart);
            ViewBag.Itinerary = db.JobItineraries.Include(j => j.Destination).Where(j => j.JobMainId == jobMain.Id);
            ViewBag.Payments = db.JobPayments.Where(j => j.JobMainId == jobMain.Id);
            ViewBag.jNotes = db.JobNotes.Where(d => d.JobMainId == jobMain.Id).OrderBy(s => s.Sort);
            ViewBag.custCompany = custCompany;

            //Default form
            string sCompany = "AJ88 Car Rental Services";
            string sLine1 = "Door 1, RedDoorz Travelers Inn Bldg., Matina Crossing Rd., Matina Pangi, Davao City, 8000 ";
            string sLine2 = "Tel# (+63)822971831; (+63)9167558473; (+63)9330895358 ";
            string sLine3 = "Email: ajdavao88@gmail.com; Website: http://www.AJDavaoCarRental.com/";
            string sLine4 = "TIN: 414-880-772-001 (non-Vat)";
            string sLogo = "LOGO_AJRENTACAR.jpg";
            Bank bank = db.Banks.Find(2);

            if (jobMain.Branch.Name == "RealBreeze")
            {
                sCompany = "Real Breeze Travel & Tours - Davao City";
                sLine1 = "Door 1, RedDoorz Travelers Inn Bldg., Matina Crossing Rd., Matina Pangi, Davao City, 8000";
                sLine2 = "Tel# (+63)822971831; (+63)916-755-8473; (+63)933-089-5358 ";
                sLine3 = "Email: RealBreezeDavao@gmail.com; Website: http://www.realbreezedavaotours.com//";
                sLine4 = "TIN: 414-880-772-000 (non-Vat)";
                sLogo = "RealBreezeLogo01.png";
                bank = db.Banks.Find(3);
            }

            if (jobMain.Branch.Name == "AJ88")
            {
                sCompany = "AJ88 Car Rental Services";
                sLine1 = "Door 1, RedDoorz Travelers Inn Bldg., Matina Crossing Rd., Matina Pangi, Davao City, 8000";
                sLine2 = "Tel# (+63)822971831; (+63)9167558473; (+63)9330895358 ";
                sLine3 = "Email: ajdavao88@gmail.com; Website: http://www.AJDavaoCarRental.com/";
                sLine4 = "TIN: 414-880-772-001 (non-Vat)";
                sLogo = "LOGO_AJRENTACAR.jpg";
                bank = db.Banks.Find(2);
            }

            ViewBag.sCompany = sCompany;
            ViewBag.sLine1 = sLine1;
            ViewBag.sLine2 = sLine2;
            ViewBag.sLine3 = sLine3;
            ViewBag.sLine4 = sLine4;
            ViewBag.sLogo = sLogo;

            ViewBag.BankName = bank.BankName;
            ViewBag.BankBranch = bank.BankBranch;
            ViewBag.AccName = bank.AccntName;
            ViewBag.AccNum = bank.AccntNo;

            ViewBag.rsvId = 1;
            ViewBag.CarDesc = "Test Unit";
            ViewBag.ReservationType = "Rental";
            ViewBag.Amount = 1000;

            DateTime today = new DateTime();
            today = getDateTimeToday().Date;

            //get paypal keys at db
            PaypalAccount paypal = db.PaypalAccounts.Where(p => p.SysCode.Equals("RealWheels")).FirstOrDefault();
            ViewBag.key = paypal.Key;

            ViewBag.isPaymentValid = jobMain.JobDate.Date == today ? "True" : "False" ;


            //handle prepared by
            var encoder = db.JobTrails.Where(s => s.RefTable == "joborder" && s.RefId == jobMain.Id.ToString()).FirstOrDefault();
            ViewBag.StaffName = getStaffName(encoder.user);
            ViewBag.Sign = getStaffSign(encoder.user);
            ViewBag.DateNow = getDateTimeToday().Date.ToString();

            //filter name and jobname if the same or personal account
            var filteredName = "";

            if (jobMain.Description == "Personal Account")
            {
                filteredName = jobMain.Description;
            }
            else if (jobMain.Description == jobMain.Customer.Name)
            {
                filteredName = jobMain.Description;
            }
            else
            {
                filteredName = jobMain.Description + " / " + jobMain.Customer.Name;
            }

            ViewBag.JobName = filteredName;

            if (jobMain.JobStatusId == 1)
            { //quotation
                ViewBag.DateNow = getDateTimeToday().AddMonths(1).Date.ToString();
                return View("Details_Quote", jobMain);
            }else if (iType != null && (int)iType == 1)
            { //Invoice
                ViewBag.DateNow = getDateTimeToday().Date.ToString();
                return View("Details_Invoice", jobMain);
            }

            return View(jobMain);
        }
        
        public string getStaffName(string staffLogin)
        {
            switch (staffLogin)
            {
                case "josette.realbreeze@gmail.com":
                    return "Josette Valleser";
                case "mae.realbreeze@gmail.com":
                    return "Cristel Mae Verano";
                case "ramil.realbreeze@gmail.com":
                    return "Ramil Villahermosa";
                case "grace.realbreeze@gmail.com":
                    return "Grace-chell V. Capandac";
                case "assalvatierra@gmail.com":
                    return "Elvie S. Salvatierra ";
                default:
                    return "Elvie S. Salvatierra ";
            }
        }

        public string getStaffSign(string staffLogin)
        {
            switch (staffLogin)
            {
                case "josette.realbreeze@gmail.com":
                    return "/Images/Signature/JoSign.jpg";
                case "mae.realbreeze@gmail.com":
                    return "/Images/Signature/MaeSign.jpg";
                case "ramil.realbreeze@gmail.com":
                    return "/Images/Signature/RamSign.jpg";
                case "grace.realbreeze@gmail.com":
                    return "/Images/Signature/GraceSign.jpg";
                case "assalvatierra@gmail.com":
                    return "/Images/Signature-1.png";
                default:
                    return "/Images/Signature-1.png";
            }
        }

        public ActionResult TextMessage(int? id)
        {
            string sData = "Pickup Details";

            Models.JobServicePickup svcpu;
            Models.JobServices svc = db.JobServices.Find(id);

            string custName = svc.JobMain.Customer.Name;

            switch (custName)
            {
                case "Real Breeze Davao":
                    custName = "Real Breeze Travel & Tours";
                    break;
                case "AJ88 Car Rental":
                    custName = "AJ88 Car Rental";
                    break;
                case "RealWheels Car Rental Davao":
                    custName = "RealWheels Car Rental Davao";
                    break;
                default:
                    custName = "Real Breeze Travel & Tours";
                    break;
            }

            if (svc.JobServicePickups.FirstOrDefault() == null)
            {
                sData += "\nPickup: undefined ";
            }
            else
            {
                Decimal quote = (svc.QuotedAmt == null ? 0 : (decimal)svc.QuotedAmt);

                svcpu = svc.JobServicePickups.FirstOrDefault();
                sData += "\nDate:" + ((DateTime)svc.DtStart).ToString("dd MMM yyyy (ddd)");
                sData += "\nTime:" + svcpu.JsTime ;
                sData += "\nLocation:"  + svcpu.JsLocation;

                sData += "\n\nGuest:" + svcpu.ClientName ;
                sData += "\nContact:" + svcpu.ClientContact;

                sData += "\n  ";
                sData += "\nAssigned:  ";

                foreach (var svi in svc.JobServiceItems ) {
                   sData += "\n" + svi.InvItem.Description + " (" + svi.InvItem.ItemCode + ") / " + svi.InvItem.ContactInfo;
                }
                
                sData += "\n  ";
                sData += "\nRate:P" + quote.ToString("##,###.00");
                sData += "\nParticulars:" + svc.Particulars;
                sData += "\n  " + svc.Remarks;
                sData += "\nNo.Pax:  " + svc.JobMain.NoOfPax;
                sData += "\n\nThank you for Trusting \n" + custName;
            }

            ViewBag.StrData = sData;
            return View();
        }

        public ActionResult TextConfirmation(int? id)
        {
            string sData = "Booking Confirmation";
            decimal totalAmount = 0;
            Models.JobServiceItem svcpu;
            Models.JobMain jobmain = db.JobMains.Find(id);
            var svc = db.JobServices.Where(j=>j.JobMainId == id).ToList();
            string custName = jobmain.Branch.Name;
            int pickupCount = 0;

            switch (custName)
            {
                case "Realbreeze":
                    custName = "Real Breeze Travel & Tours";
                    break;
                case "AJ88":
                    custName = "AJ88 Car Rental";
                    break;
                case "RealWheels":
                    custName = "RealWheels Car Rental Davao";
                    break;
                default:
                    custName = "Real Breeze Travel & Tours";
                    break;
            }
            if (svc.FirstOrDefault() == null)
            {
                sData += "\nServices: undefined ";
            }
            else
            {
                Decimal quote = (jobmain.AgreedAmt == null ? 0 : (decimal)jobmain.AgreedAmt);
                sData += "\n\nGuest:" + jobmain.Description + " " + getCustomerCompany(jobmain.Id);
                sData += "\nContact:" + jobmain.CustContactNumber;
                sData += "\n---------------";

                foreach (var svi in svc)
                {

                    decimal quoted = svi.QuotedAmt != null ? (decimal)svi.QuotedAmt : 0 ;
                    sData += "\n\nDate:" + ((DateTime)svi.DtStart).ToString("dd MMM yyyy (ddd)") + " - " + ((DateTime)svi.DtEnd).ToString("dd MMM yyyy (ddd)");
                    sData += "\nDescription:" + svi.Particulars;
                    sData += "\nVehicle:" + svi.SupplierItem.Description;
                    sData += "\nRate:P" + quoted.ToString("##,###.00");
                    //totalAmount += (decimal)svi.QuotedAmt;
                    totalAmount += quoted;
                    
                    //check pickup details
                    if (svi.JobServicePickups.Count != 0)
                    {
                        foreach (var jobPickup in svi.JobServicePickups)
                        {
                            //Pickup Details
                            sData += "\nPickup: ";
                            sData += " " + jobPickup.JsTime ;
                            sData += " " + jobPickup.JsDate.ToString("MMM dd yyyy");
                            sData += "\nLocation: " + jobPickup.JsLocation;
                            sData += "\nDriver: " + getDriverDetails(svi.Id);
                            pickupCount++;
                        }
                    }


                    sData += "\n---------------";
                }// end of job services

                if(pickupCount == 0) { 
                    //Pickup Details
                    sData += "\n\nPickup Details: TBA";
                    sData += "\nDate: TBA" ;
                    sData += "\nTime: TBA" ;
                    sData += "\nLocation: TBA" ;
                }

                //Summary Details
                sData += "\n  ";
                sData += "\nTotal Rate:P" + totalAmount.ToString("##,###.00");
                sData += "\nRemarks:" ;
                sData += "  " + jobmain.JobRemarks;
                sData += "\nNo.Pax:  " + jobmain.NoOfPax;
                sData += "\n\n Thank you and have a nice day.\n";
                sData += "\n" + custName;
            }

            ViewBag.StrData = sData;

            if (id != null)
            {
              ViewBag.forDriver = textDetailsForDriver((int)id);
            }

            return View();
        }



        private string textDetailsForDriver(int id)
        {
            string sData = "Booking Details";
            decimal totalAmount = 0;
            Models.JobServiceItem svcpu;
            Models.JobMain jobmain = db.JobMains.Find(id);
            var svc = db.JobServices.Where(j => j.JobMainId == id).ToList();
            string custName = jobmain.Branch.Name;
            int pickupCount = 0;

            switch (custName)
            {
                case "RealBreeze":
                    custName = "Real Breeze Travel & Tours";
                    break;
                case "AJ88":
                    custName = "AJ88 Car Rental";
                    break;
                case "RealWheels":
                    custName = "RealWheels Car Rental Davao";
                    break;
                default:
                    custName = "Real Breeze Travel & Tours";
                    break;
            }

            if (svc.FirstOrDefault() == null)
            {
                sData += "\nServices: undefined ";
            }
            else
            {
                Decimal quote = (jobmain.AgreedAmt == null ? 0 : (decimal)jobmain.AgreedAmt);
                sData += "\n\nGuest:" + jobmain.Description + " " + getCustomerCompany(jobmain.Id);
                sData += "\nContact:" + jobmain.CustContactNumber;
                sData += "\n---------------";

                foreach (var svi in svc)
                {

                    decimal quoted = svi.QuotedAmt != null ? (decimal)svi.QuotedAmt : 0;
                    sData += "\n\nDate:" + ((DateTime)svi.DtStart).ToString("MMM dd yyyy (ddd)") + " - " + ((DateTime)svi.DtEnd).ToString("MMM dd yyyy (ddd)");
                    sData += "\nDescription:" + svi.Particulars;
                    sData += "\nVehicle:" + svi.SupplierItem.Description;

                    //sData += "\nRate:P" + quoted.ToString("##,###.00");
                    //totalAmount += (decimal)svi.QuotedAmt;

                    totalAmount += quoted;
                    
                    //check pickup details
                    if (svi.JobServicePickups.Count != 0)
                    {
                        foreach (var jobPickup in svi.JobServicePickups)
                        {
                            if (jobPickup != null) { 
                                //Pickup Details
                                sData += "\n\nPickup: ";
                                sData += " " + jobPickup.JsTime;
                                sData += " " + jobPickup.JsDate.ToString(" MMM dd yyyy");
                                sData += "\nLocation: " + jobPickup.JsLocation;
                                sData += "\nClient: " + jobPickup.ClientName + " / " + jobPickup.ClientContact;
                                sData += "\nDriver: " + getDriverDetails(svi.Id);

                            }
                            pickupCount++;
                        }//end of foreach
                    }


                    //Driver Instructions
                    if (svi.PickupInstructions.Count != 0)
                    {
                        sData += "\n\nDriver Instructions: ";
                        foreach (var ins in svi.PickupInstructions)
                        {
                            sData += "\n" + ins.DriverInstruction.Description;

                        }
                    }

                    sData += "\n---------------";
                }

                if (pickupCount == 0)
                {
                    //Pickup Details
                    sData += "\n\nPickup Details: TBA";
                    sData += "\nDate: TBA";
                    sData += "\nTime: TBA";
                    sData += "\nLocation: TBA";
                }

                //Summary Details
                sData += "\n  ";
                sData += "\nCollectible:P" +  dbc.getJobCollectible(id).ToString("##,###.00");
                sData += "\nRemarks:";
                sData += "  " + jobmain.JobRemarks;
                sData += "\nNo.Pax:  " + jobmain.NoOfPax;
                sData += "\n\n Thank you and have a nice day.\n";
                sData += "\n" + custName;
            }

            return sData;

        }

        private string getDriverDetails(int svcId)
        {
            var driverDetails = "TBA";
            var jobsvc = db.JobServiceItems.Where(s => s.JobServicesId == svcId).ToList();
            
            foreach (var svc in jobsvc)
            {
                if (svc.InvItem.ViewLabel == "Driver" || svc.InvItem.ViewLabel == "DRIVER" )
                {
                    driverDetails = svc.InvItem.Description + " / " + svc.InvItem.ContactInfo;
                }
            }
            return driverDetails;
        }

        //web service call to send notification
        public void Notification(int id)
        {
            SMSWebService ws = new SMSWebService();
            ws.AddNotification(id);
        }
        
        public ActionResult ConfirmJobStatus(int? id)
        {
            var Job = db.JobMains.Find(id);
            Job.JobStatusId = 3;
            db.Entry(Job).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("JobServices", "JobOrder", new { JobMainId = id });
        }
        
        public ActionResult ReserveJobStatus(int? id)
        {
            var Job = db.JobMains.Find(id);
            Job.JobStatusId = 2;
            db.Entry(Job).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("JobServices", "JobOrder", new { JobMainId = id });
        }

        #endregion

        #region supplier
        public ActionResult PoDetails(int? hdrId) {
            var supplierPoDtls = db.SupplierPoDtls.Include(s => s.SupplierPoHdr).Include(s => s.JobService).Where(d => d.SupplierPoHdrId == (int)hdrId);
            SupplierPoDtl supplier = new SupplierPoDtl();
            List<SupplierPoItem> supItems = new List<SupplierPoItem>();
            List<InvItem> invItems = new List<InvItem>();
            var hdr = db.SupplierPoHdrs.Where(h => h.Id == hdrId).ToList();

            supplier = db.SupplierPoDtls.Where(s => s.SupplierPoHdrId == hdrId).FirstOrDefault();
            if (supplier != null)
            {
                supItems = db.SupplierPoItems.Where(s => s.SupplierPoDtlId == supplier.Id).ToList();
            }
            else
            {
                supplier = new SupplierPoDtl
                {
                    Id = 0
                };
            }

            if (supItems == null)
            {
                supItems.Add(new SupplierPoItem
                {
                    Id = 0,
                    InvItem = null,
                    InvItemId = 0,
                    SupplierPoDtl = null,
                    SupplierPoDtlId = 0,
                });
            }

            invItems = db.InvItems.ToList();
            if (invItems == null)
            {
                invItems.Add(new InvItem
                {
                    Id = 0,
                });
            }

            ViewBag.HdrInfo = hdr;
            ViewBag.HdrId = hdrId;
            ViewBag.supplierPoItems = supItems;
            ViewBag.InvItemsList = invItems;
            ViewBag.Id = supplier.Id;

            return View(supplierPoDtls.ToList());
        }



        #endregion
    
        #region Action Items status update
        //Ajax Call
        public ActionResult MarkDone(int SvcId, int ActionId)
        {
            Models.JobAction jaTmp = new JobAction();
            jaTmp.AssignedTo = User.Identity.Name;
            jaTmp.DtAssigned = DateTime.Now;
            jaTmp.DtPerformed = DateTime.Now;
            jaTmp.PerformedBy = User.Identity.Name;
            jaTmp.Remarks = "Done";
            jaTmp.JobServicesId = SvcId;
            jaTmp.SrvActionItemId = ActionId;

            db.JobActions.Add(jaTmp);
            db.SaveChanges();

            return Json( "from MarkDone:" + SvcId.ToString() + "/" + ActionId.ToString(),
                JsonRequestBehavior.AllowGet);
        }

        //ajax test
        public ActionResult AjaxTest()
        {
            return Json("insomia", JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Daily Status Updates
        public ActionResult DailyUpdateList() {

            string Message = "";

               
               var updates = db.Database.SqlQuery<DailyUpdate>(@"
                    
                    select
                    'New SalesLead' as StatusCategory, 
                    a.DtEntered dtTaken,  
                    a.Id refId, a.CustName + ' / ' + a.Details Details 
                    from SalesLeads a
                    where datediff(day, getdate(),a.DtEntered) > - 1

                    union

                    select 
                    'SalesLead Activity' as StatusCategory,
                    a.DtActivity dtTaken,
                    a.SalesLeadId refId,
                    c.Name + ' - ' + a.Particulars Details
                    from 
                    SalesActivities a
                    left join SalesLeads as s on s.Id = a.SalesLeadId 
                    left join Customers as c on c.Id = s.CustomerId 
                    where a.SalesActStatusId=2 AND datediff(day, getdate(),a.DtActivity) > -2

                    union

                    select 
                    'SalesStatus Activity' as StatusCategory,
                    a.DtStatus dtTaken,
                    a.SalesLeadId refId,
                    s.CustName + ' - ' + s.Details + ' - '+ sc.Name as Details
                    from 
                    SalesStatus a
                    left join SalesLeads as s on s.Id = a.SalesLeadId 
                    left join Customers as c on c.Id = s.CustomerId 
                    left join SalesStatusCodes as sc on sc.Id = a.SalesStatusCodeId 
                    where datediff(day, getdate(),a.DtStatus) > -1

                    union

                    select 
                    'New JobOrder' as StatusCategory,
                    j.JobDate as dtTaken,
                    j.Id as refId,
                    c.Name +' - '+ j.Description as Details
                    from 
                    JobMains j 
                    left join Customers as c on c.Id = j.CustomerId 
                    where datediff(day, getdate(),j.JobDate) > - 1

                    union

                    select 
                    'JobOrder Activity' as StatusCategory,
                    j.DtPerformed as dtTaken,
                    j.JobServicesId as refId,
                    cu.Name +' - '+ js.Particulars +' / '+ 
                    c.CatCode +' - '+ j.Remarks as Details
                    from 
                    JobActions j 
                    left join SrvActionItems as s on s.Id = j.SrvActionItemId 
                    left join SrvActionCodes as c on c.Id = s.SrvActionCodeId 
                    left join JobServices as js on js.Id = j.JobServicesId 
                    left join JobMains as m on m.Id = js.JobMainId 
                    left join Customers as cu on cu.Id = m.CustomerId 

                    where datediff(day, getdate(),j.DtPerformed) > - 1

                    union

                    select 
                    'Gate Activity' as StatusCategory,
                    g.dtControl as dtTaken,
                    g.Id as refId,
                    '('+c.ItemCode +')'+c.Description   as Details
                    from 
                    InvCarGateControls g 
                    left join InvItems as c on c.Id = g.InvItemId 
                    where datediff(day, getdate(),g.dtControl) > - 1

                    union

                    select 
                    'Maintenance Activity' as StatusCategory,
                    g.dtControl as dtTaken,
                    g.Id as refId,
                    '('+c.ItemCode +')'+c.Description   as Details
                    from 
                    InvCarGateControls g 
                    left join InvItems as c on c.Id = g.InvItemId 
                    where datediff(day, getdate(),g.dtControl) > - 1

                    Order by dtTaken
                    ;").ToList();

                return View(updates);
  
        }
        #endregion

        #region Payments
        
        // GET: JobPayments
        public ActionResult PaymentIndex()
        {
            var jobPayments = db.JobPayments.Include(j => j.JobMain).Include(j => j.Bank);
            return View(jobPayments.ToList());
        }

        public ActionResult PaymentAdvanceList()
        {
            var payments = db.JobPayments.Include(j => j.JobMain).Include(j => j.Bank)
                .Where(d => d.JobMain.JobStatusId != 4 && d.JobMain.JobStatusId != 5)
                .OrderBy(d => d.DtPayment);
            return View(payments);
        }

        public ActionResult Payments(int? id)
        {
            ViewBag.JobMainId = id;

            var Job = db.JobMains.Where(d => d.Id == id).FirstOrDefault();
            ViewBag.JobOrder = Job;
            ViewBag.mainid = id;

            var jobPayments = db.JobPayments.Include(j => j.JobMain).Include(j => j.Bank).Where(d => d.JobMainId == id);
            return View("Paymentindex", jobPayments.ToList());
        }
        // GET: JobPayments/Details/5
        public ActionResult PaymentDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobPayment jobPayment = db.JobPayments.Find(id);
            if (jobPayment == null)
            {
                return HttpNotFound();
            }
            return View(jobPayment);
        }

        // GET: JobPayments/Create , remarks = "Partial Payment" 
        public ActionResult PaymentCreate(int? JobMainId, string remarks)
        {
            Models.JobPayment jp = new JobPayment();
            jp.JobMainId = (int)JobMainId;
            jp.DtPayment = DateTime.Now;
            jp.Remarks = remarks;

            ViewBag.JobMainId = new SelectList(db.JobMains, "Id", "Description");
            ViewBag.BankId = new SelectList(db.Banks, "Id", "BankName");

            return View(jp);


        }

        // GET: JobPayments/Create , remarks = "Partial Payment" 
        public ActionResult PaymentCreatePG(int? JobMainId, string remarks)
        {

            JobPayment jobPayment = new JobPayment();
            jobPayment.BankId = 4;
            jobPayment.DtPayment = DateTime.Now;
            jobPayment.JobMainId = (int)JobMainId;
            jobPayment.PaymentAmt = 0;
            jobPayment.Remarks = remarks;

            db.JobPayments.Add(jobPayment);
            db.SaveChanges();

            ViewBag.JobMainId = JobMainId;

            var Job = db.JobMains.Where(d => d.Id == JobMainId).FirstOrDefault();
            ViewBag.JobOrder = Job;

            var jobPayments = db.JobPayments.Include(j => j.JobMain).Include(j => j.Bank).Where(d => d.JobMainId == JobMainId);
            return View("Paymentindex", jobPayments.ToList());
        }

        
        // POST: JobPayments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PaymentCreate([Bind(Include = "Id,JobMainId,DtPayment,PaymentAmt,Remarks,BankId")] JobPayment jobPayment)
        {
            if (ModelState.IsValid)
            {
                db.JobPayments.Add(jobPayment);
                db.SaveChanges();
                return RedirectToAction("Payments", new { id = jobPayment.JobMainId });
            }

            ViewBag.JobMainId = new SelectList(db.JobMains, "Id", "Description", jobPayment.JobMainId);
            ViewBag.BankId = new SelectList(db.Banks, "Id", "BankName", jobPayment.BankId);
            return View(jobPayment);
        }

        // GET: JobPayments/Edit/5
        public ActionResult PaymentEdit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobPayment jobPayment = db.JobPayments.Find(id);
            if (jobPayment == null)
            {
                return HttpNotFound();
            }
            ViewBag.JobMainId = new SelectList(db.JobMains, "Id", "Description", jobPayment.JobMainId);
            ViewBag.BankId = new SelectList(db.Banks, "Id", "BankName", jobPayment.BankId);
            return View(jobPayment);
        }

        // POST: JobPayments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PaymentEdit([Bind(Include = "Id,JobMainId,DtPayment,PaymentAmt,Remarks,BankId")] JobPayment jobPayment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(jobPayment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Payments", new { id = jobPayment.JobMainId });
            }
            ViewBag.JobMainId = new SelectList(db.JobMains, "Id", "Description", jobPayment.JobMainId);
            ViewBag.BankId = new SelectList(db.Banks, "Id", "BankName", jobPayment.BankId);
            return View(jobPayment);
        }

        // GET: JobPayments/Delete/5
        public ActionResult PaymentDelete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobPayment jobPayment = db.JobPayments.Find(id);
            if (jobPayment == null)
            {
                return HttpNotFound();
            }
            return View(jobPayment);
        }

        // POST: JobPayments/Delete/5
        [HttpPost, ActionName("PaymentDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult PaymentDeleteConfirmed(int id)
        {
            JobPayment jobPayment = db.JobPayments.Find(id);
            int tmpId = jobPayment.JobMainId;
            db.JobPayments.Remove(jobPayment);
            db.SaveChanges();
            return RedirectToAction("Payments", new { id = tmpId });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        //GET : payment transction history
        //[HttpGet]
        public string GetPaymentHistory(int? jobId)
        {

            List<cJobPayment> paymentList = new List<cJobPayment>();

            //get transactions
                List<JobPayment> jobTrans = db.JobPayments.Where(j => j.JobMainId == jobId).ToList();
                foreach (var payment in jobTrans ) {
                    paymentList.Add(new cJobPayment {
                        Id = payment.Id,
                        DtPayment = payment.DtPayment,
                        Amount = payment.PaymentAmt,
                        Type = payment.Bank.BankName
                    });

                }

            

            return JsonConvert.SerializeObject(paymentList, Formatting.Indented);
        }
        #endregion

        #region jobTrails

        public ActionResult jobTrails() {

            return View(db.JobTrails.ToList());
        }

        public ActionResult createTrailTest() {

            dbc.addEncoderRecord("Test", "0", HttpContext.User.Identity.Name, "Test Btn");

            return RedirectToAction("JobTrails");
        }

        #endregion

        #region SendMails

        //Handle email sending for Invoice
        public String SendEmail(int jobId, string mailType)
        {
            JobMain jobOrder = db.JobMains.Find(jobId);
            EMailHandler mail = new EMailHandler();

            string siteRedirect = "https://realwheelsdavao.com/invoice/";

            string clientName = jobOrder.Description;
            string companyEmail = "reservation.realwheels@gmail.com"; //realwheelsemail
            string ajdavaoEmail = "ajdavao88@gmail.com"; //
            string adminEmail = "travel.realbreeze@gmail.com";
            string mailResult = "";

            //Send invoice
            mailResult = mail.SendMail(jobId, ajdavaoEmail, "ADMIN-INVOICE-SENT", clientName, siteRedirect);
            mailResult = mail.SendMail(jobId, companyEmail, "ADMIN-INVOICE-SENT", clientName, siteRedirect);
            mailResult = mail.SendMail(jobId, adminEmail, "ADMIN-INVOICE-SENT", clientName, siteRedirect);

            //client
            mailResult = mail.SendMail(jobId, jobOrder.CustContactEmail, mailType, clientName, siteRedirect);
            
            mailResult = mailResult == "success" ? "Email is sent successfully." : "Our System cannot send the email to the client. Please try again.";
            return mailResult;
        }

        //Handle email sending for Reservation and inquiry Quotations
        public string SendEmailBooking(int? jobId, string doctype)
        {
            string mailResult   = "";
            string companyEmail = "reservation.realwheels@gmail.com"; //realwheelsemail
            string ajdavaoEmail = "ajdavao88@gmail.com"; //
            string adminEmail   = "travel.realbreeze@gmail.com";

            if (jobId != null) { 
                JobMain jobOrder = db.JobMains.Find(jobId);
                EMailHandler mail = new EMailHandler();
            
                List<JobServices> jobServices = db.JobServices.Include(j => j.JobMain).Include(j => j.Supplier)
                    .Include(j => j.Service).Include(j => j.SupplierItem).Include(j => j.JobServicePickups)
                    .Where(d => d.JobMainId == jobId).ToList();
            
                string clientName = jobOrder.Description;

                switch (doctype)
                {
                    case "QUOTATION":
                        mailResult = mail.SendMailQuotation((int)jobId, jobOrder.CustContactEmail, jobServices); //client email
                        mailResult = mail.SendMailQuotation((int)jobId, companyEmail, jobServices); //realwheels
                        mailResult = mail.SendMailQuotation((int)jobId, adminEmail, jobServices);   //travel
                        mailResult = mail.SendMailQuotation((int)jobId, ajdavaoEmail, jobServices); //ajdavao
                        break;
                    case "RESERVATION":
                        mailResult = mail.SendMailReservation((int)jobId, jobOrder.CustContactEmail, jobServices); // client email
                        mailResult = mail.SendMailReservation((int)jobId, companyEmail, jobServices); //realwheels
                        mailResult = mail.SendMailReservation((int)jobId, adminEmail, jobServices);   //travel
                        mailResult = mail.SendMailReservation((int)jobId, ajdavaoEmail, jobServices); //ajdavao
                        break;
                }

            }
            mailResult = mailResult == "success" ? "Email is sent successfully." : "Our System cannot send the email to the client. Please try again later. " + mailResult;
            return mailResult;
        }

        public void onPaymentSuccess(int jobId, string mailType)
        {
            JobMain jobOrder = db.JobMains.Find(jobId);
            EMailHandler mail = new EMailHandler();

            string siteRedirect = "https://realwheelsdavao.com/invoice/";

            string clientName = jobOrder.Description;
            string companyEmail = "reservation.realwheels@gmail.com"; //realwheelsemail
            string ajdavaoEmail = "ajdavao88@gmail.com";
            string mailResult = "";
            string adminEmail = "travel.realbreeze@gmail.com";
            
            //Send invoice 
            mailResult = mail.SendMail(jobId, ajdavaoEmail, "ADMIN-PAYMENT-SUCCESS", clientName, siteRedirect);
            mailResult = mail.SendMail(jobId, companyEmail, "ADMIN-PAYMENT-SUCCESS", clientName, siteRedirect);
            mailResult = mail.SendMail(jobId, adminEmail, "ADMIN-PAYMENT-SUCCESS", clientName, siteRedirect);

            //client
            mailResult = mail.SendMail(jobId, jobOrder.CustContactEmail, "CLIENT-PAYMENT-SUCCESS", clientName, siteRedirect);
            
            mailResult = mailResult == "success" ? "Email is sent successfully." : "Our System cannot send the email to the client. Please try again.";

        }
        
        //After Payment Success, send email to client, admin and company emails
        public void PaymentReserveSuccess(int reservationId)
        {
            OnlineReservation reservation = db.OnlineReservations.Find(reservationId);
            EMailHandler mail = new EMailHandler();

            string siteRedirect = "https://realwheelsdavao.com/OnlineReservations/Form";

            string clientName   = reservation.Name;
            string companyEmail = "reservation.realwheels@gmail.com"; //realwheelsemail
            string ajdavaoEmail = "ajdavao88@gmail.com";
            string adminEmail   = "travel.realbreeze@gmail.com";
            string mailResult   = "";

            //Send invoice
            mailResult = mail.SendMail(reservationId, ajdavaoEmail, "ADMIN-PAYMENT-SUCCESS", clientName, siteRedirect);
            mailResult = mail.SendMail(reservationId, companyEmail, "ADMIN-PAYMENT-SUCCESS", clientName, siteRedirect);
            mailResult = mail.SendMail(reservationId, adminEmail, "ADMIN-PAYMENT-SUCCESS", clientName, siteRedirect);

            //client
            mailResult = mail.SendMail(reservationId, reservation.Email, "CLIENT-PAYMENT-SUCCESS", clientName, siteRedirect);

            mailResult = mailResult == "success" ? "Email is sent successfully." : "Our System cannot send the email to the client. Please try again.";
        }

        public void SendEmailAdmin(int jobId, string mailType)
        {
            JobMain jobOrder = db.JobMains.Find(jobId);
            EMailHandler mail = new EMailHandler();

            string clientName = jobOrder.Description;
            string siteRedirect = "https://realwheelsdavao.com/invoice/";

            mail.SendMailPaymentAdvice(jobId, "reservation.realwheels@gmail.com", mailType, clientName, siteRedirect); //reservation gmail
            mail.SendMailPaymentAdvice(jobId, "AJDavao88@gmail.com", mailType, clientName, siteRedirect);      //customer email
        }
        #endregion

        #region Templates
        public ActionResult BrowseTemplate(int? JobMainId)
        {
            //get list of jobs with status TEMPLATE
            var jobTemplates = db.JobMains.Where(j => j.JobStatus.Status == "TEMPLATE").ToList();

            ViewBag.JobMainId = JobMainId != null ? (int)JobMainId : 0;
            
            return View(jobTemplates);
        }
        
        public ActionResult SelectTemplate(int JobMainId, int TemplateId)
        {
            //get template services
            var templateSvc = db.JobServices.Where(s=>s.JobMainId == TemplateId).ToList();


            //copy services from template to current job
            foreach (var svc in templateSvc)
            {
                //copy job service
                JobServices tempsvc = new JobServices();
                tempsvc = svc;
                tempsvc.JobMainId = JobMainId;

                //save new service
                db.JobServices.Add(tempsvc);
                db.SaveChanges();
            }

            //get last job service
            int tempId = db.JobServices.Where(s=>s.JobMainId == JobMainId).FirstOrDefault().Id;
                foreach (var svc in templateSvc)
                {
                    //copy job itineraries
                    var itineraries = db.JobItineraries.Where(s => s.SvcId == svc.Id).ToList();
                
                    foreach (var items in itineraries)
                    {
                        //copy job service
                        JobItinerary itinerary = new JobItinerary();
                        itinerary = items;
                        itinerary.JobMainId = JobMainId;
                        itinerary.SvcId = tempId; //? 
                    
                        //save new service
                        db.JobItineraries.Add(itinerary);
                    }
                }
            
            //db.SaveChanges();

            //copy job notes
            var jobnotes = db.JobNotes.Where(s => s.JobMainId == TemplateId).ToList();
            foreach (var items in jobnotes)
            {
                //copy job service
                JobNote tempNote = new JobNote();
                tempNote = items;
                tempNote.JobMainId = JobMainId;
                tempNote.Note += " - " + tempId;
                //save new service
                db.JobNotes.Add(tempNote);

            }
            db.SaveChanges();

            //setItineraries();

            return RedirectToAction("setItineraries", "JobOrder", new { JobMainId = JobMainId , TemplateId = TemplateId } );
        }

        public ActionResult setItineraries(int JobMainId, int TemplateId)
        {
            //get template services
            var templateSvc = db.JobServices.Where(s => s.JobMainId == TemplateId).ToList();

            //get last job service
            int tempId = db.JobServices.Where(s => s.JobMainId == JobMainId).FirstOrDefault().Id;
            foreach (var svc in templateSvc)
            {
                //copy job itineraries
                var itineraries = db.JobItineraries.Where(s => s.SvcId == svc.Id).ToList();

                foreach (var items in itineraries)
                {
                    //copy job service
                    JobItinerary itinerary = new JobItinerary();
                    itinerary = items;
                    itinerary.JobMainId = JobMainId;
                    itinerary.SvcId = tempId; //? 

                    //save new service
                    db.JobItineraries.Add(itinerary);
                    db.SaveChanges();

                }
                int UnassignedId = db.InvItems.Where(u => u.Description == "UnAssigned").FirstOrDefault().Id;
                AddUnassignedItem(UnassignedId, tempId);
                tempId++;
            }

            return RedirectToAction("JobServices", "JobOrder", new { JobMainId = JobMainId });
            //return RedirectToAction("JobServices", "JobOrder", new { JobMainId = JobMainId });
        }

        #endregion

        #region Driver Instructions

        public ActionResult DriverInstructions(int id, int mainId)
        {
            ViewBag.InstructionsList = db.DriverInstructions.ToList();
            ViewBag.JobServiceId = id;
            ViewBag.mainId = mainId;
            var instructionlist = db.DriverInsJobServices.Where(s => s.JobServicesId == id).ToList();
            return View(instructionlist);
        }

        public ActionResult AddDriverInst(int id, int jsId)
        {
            DriverInsJobService ins = new DriverInsJobService();
            ins.DriverInstructionsId = id;
            ins.JobServicesId = jsId;

            db.DriverInsJobServices.Add(ins);
            db.SaveChanges();

            var mainId = getparentMainId(jsId);

            return RedirectToAction("DriverInstructions","JobOrder" , new { id = jsId , mainId = mainId });
        }


        public ActionResult DeleteDriverInst(int id, int jsId)
        {
            DriverInsJobService ins = db.DriverInsJobServices.Find(id);

            db.DriverInsJobServices.Remove(ins);
            db.SaveChanges();

            var mainId = getparentMainId(jsId);

            return RedirectToAction("DriverInstructions", "JobOrder", new { id = jsId, mainId = mainId });
        }

        private int getparentMainId(int jsId)
        {
            var js = db.JobServices.Find(jsId);
            return js.JobMainId;
        }
        #endregion
    }
}