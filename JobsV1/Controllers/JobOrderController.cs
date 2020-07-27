using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.Entity;
using JobsV1.Models;
using JobsV1.Models.Class;
using System.ComponentModel.DataAnnotations;
using System.Net;
using Newtonsoft.Json;
using PayPal.Api;
using System.Configuration;
using Microsoft.Ajax.Utilities;

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

        private string SITECONFIG = ConfigurationManager.AppSettings["SiteConfig"].ToString();

        private JobDBContainer db = new JobDBContainer();
        private DBClasses dbc = new DBClasses();
        private ActionTrailClass trail = new ActionTrailClass();
        private DateClass dt = new DateClass();
        private JobOrderClass jo = new JobOrderClass();
        private JobVehicleClass jvc = new JobVehicleClass();


        // GET: JobOrder
        public ActionResult Index(int? sortid, int? serviceId, int? mainid, string search)
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
            ViewBag.JobVehicle = jvc.GetJobVehicle(jobmainId);
            ViewBag.SiteConfig = SITECONFIG;

            var isAllowedPayment = false;
            if (User.IsInRole("Admin") || User.IsInRole("Accounting"))
            {
                isAllowedPayment = true;
            }
            ViewBag.IsAdmin = isAllowedPayment;
            ViewBag.SrchValue = search;

            //SEARCH
            if (search != null)
            {
                //var srchData = data.Where(d => d.Main.Id.ToString() == search || d.Main.Description.ToLower().ToString().Contains(search.ToLower()) ||
                //d.Main.Customer.Name.ToLower().ToString().Contains(search.ToLower()) || d.Company.ToLower().ToString().Contains(search.ToLower()));

                var srchData = data.Where(d => d.Main.Id.ToString() == search || d.Main.Description.ToLower().ToString().Contains(search.ToLower()) ||
                d.Main.Customer.Name.ToLower().ToString().Contains(search.ToLower()) || d.Company.ToLower().ToString().Contains(search.ToLower()));
                if (srchData != null)
                    data = srchData.ToList();
            }

            if (sortid == 1)
            {
                return View(data.OrderBy(d => d.Main.JobDate));
            }
            else
            {
                return View(data.OrderByDescending(d => d.Main.JobDate));
            }
        }

        // GET: JobOrder
        public ActionResult IndexSimplified(int? sortid, int? serviceId, int? mainid, string search, string page)
        {
            Session["FilterID"] = 1;
            if (sortid != null)
                Session["FilterID"] = (int)sortid;
            else
            {
                if (Session["FilterID"] != null)
                    sortid = (int)Session["FilterID"];
                else
                    sortid = 1;
            }

            //get job main Id
            ViewBag.mainId = jo.GetJobMainId(serviceId, mainid);

            // get job list data
            var data = jo.getJobData((int)sortid); 

            //SEARCH
            if (search != null)
            {
                //data = data.Where(d => d.Main.Id.ToString() == search || d.Main.Description.ToLower().ToString().Contains(search.ToLower()) || d.Main.Customer.Name.ToLower().ToString().Contains(search.ToLower())).ToList();
            }

            if (sortid == 1)
            {
                return View(data.OrderBy(d => d.JobDate));
            }
            else
            {
                return View(data.OrderByDescending(d => d.JobDate));
            }
        }

        //GET : return list of jobs
        public List<cJobOrder> getJobData(int sortid)
        {  

            var confirmed = dbc.getJobConfirmedList(sortid).Select(s=>s.Id);

            IEnumerable<Models.JobMain> jobMains = db.JobMains.Where(j=> confirmed.Contains(j.Id))
                .Include(j => j.Customer)
                .Include(j => j.Branch)
                .Include(j => j.JobStatus)
                .Include(j => j.JobThru)
                ;

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
                joTmp.Company = GetJobCompanyName(main.Id);


                List<Models.JobServices> joSvc = db.JobServices.Where(d => d.JobMainId == main.Id).OrderBy(s => s.DtStart).ToList();
                foreach (var svc in joSvc)
                {
                    cJobService cjoTmp = new cJobService();
                    cjoTmp.Service = svc;
                    var ActionDone = db.JobActions.Where(d => d.JobServicesId == svc.Id ).Select(s => s.SrvActionItemId);
                    cjoTmp.SvcActionsDone = db.SrvActionItems.Where(d => d.ServicesId == svc.ServicesId && ActionDone.Contains(d.Id)).Include(d => d.SrvActionCode);
                    cjoTmp.SvcActions = db.SrvActionItems.Where(d => d.ServicesId == svc.ServicesId && !ActionDone.Contains(d.Id)).Include(d => d.SrvActionCode);
                    cjoTmp.Actions = db.JobActions.Where(d => d.JobServicesId == svc.Id).Include(d => d.SrvActionItem);
                    cjoTmp.SvcItems = db.JobServiceItems.Where(d => d.JobServicesId == svc.Id).Include(d => d.InvItem);
                    cjoTmp.SupplierPos = db.SupplierPoDtls.Where(d => d.JobServicesId == svc.Id).Include(i => i.SupplierPoHdr);
                    joTmp.Main.AgreedAmt += svc.ActualAmt;

                    joTmp.Services.Add(cjoTmp);
                }


                //get min job date
                if (sortid == 1)
                {
                    joTmp.Main.JobDate = TempJobDate(joTmp.Main.Id);
                    //joTmp.Main.JobDate = MinJobDate(joTmp.Main.Id);
                }
                if (sortid == 2)
                {
                    //joTmp.Main.JobDate = TempJobDate(joTmp.Main.Id);
                    //joTmp.Main.JobDate = MinJobDate(joTmp.Main.Id);
                }
                else
                {
                    joTmp.Main.JobDate = MinJobDate(joTmp.Main.Id);
                }

                //add payments
                List<Models.JobPayment> jobPayment = db.JobPayments.Where(d => d.JobMainId == main.Id).ToList();
                foreach (var payment in jobPayment)
                {
                    //add payments except discount (JobPaymentTypeId = 4)
                    if (payment.JobPaymentTypeId != 4)
                    {
                        joTmp.Payment += payment.PaymentAmt;
                    }
                }

                //add discounts
                //subtract discount amount
                joTmp.Main.AgreedAmt += jo.GetJobDiscountAmount(main.Id);


                data.Add(joTmp);
            }
            
            switch (sortid)
            {
                case 1: //OnGoing
                    data = (List<cJobOrder>)data
                       .Where(d => d.Main.JobDate.CompareTo(today.Date) >= 0).ToList();

                    break;
                case 2: //prev
                    data = (List<cJobOrder>)data
                        .Where(p => DateTime.Compare(p.Main.JobDate.Date, today.Date) < 0)
                        .ToList();
                    break;
                case 3: //close
                    data = (List<cJobOrder>)data
                        .Where(p => p.Main.JobDate.Date > today.Date.AddDays(-150)).ToList();
                    break;
                case 4: //cancelled
                    data = (List<cJobOrder>)data
                        .Where(p => p.Main.JobDate.Date > today.Date.AddDays(-150)).ToList();
                    break;

                default:
                    data = (List<cJobOrder>)data.ToList();
                    break;
            }

            return data;
        }

        // JobOrder/JobStatus
        // Return list of jobs with status of the jobs
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

            //Get List of Customers
            List<Customer> customers = db.Customers.ToList();
            ViewBag.companyList = customers;

            //Handle jobmainId
            var jobmainId = serviceId != null ? db.JobServices.Find(serviceId).JobMainId : 0;
            jobmainId = mainid != null ? (int)mainid : jobmainId;
            ViewBag.mainId = jobmainId;

            //job sorting order
            if (sortid == 1) {
                return View(data.OrderBy(d => d.Main.JobDate));
            }else{
                return View(data.OrderByDescending(d => d.Main.JobDate));
            }
            
        }

        //GET : return customer email
        public string getCustomerEmail(int id)
        {
            string custEmail = db.Customers.Find(id).Email;
            return custEmail;
        }

        //GET : return customer contact number
        public string getCustomerNumber(int id)
        {
            string custNum = db.Customers.Find(id).Contact1;
            string custNum2 = db.Customers.Find(id).Contact2;
            return custNum2;
        }

        //GET : return customer company name
        public string getCustomerCompany(int id)
        {
            var company = db.CustEntities.Where(s => s.CustomerId == id).FirstOrDefault();
            string companyNum = company != null ?  company.CustEntMainId.ToString() : " ";
            return companyNum;
        }

        private string GetJobCompanyName(int jobId)
        {
            var jobmain = db.JobMains.Find(jobId);

            var jobEntMainQuery = jobmain.JobEntMains.OrderByDescending(j => j.Id).FirstOrDefault();
            if (jobEntMainQuery != null)
            {
                return jobEntMainQuery.CustEntMain.Name;
            }

            return "N/A";
        }

        //GET : return date of the job based on the date today
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
                main.JobDate = minDate;
            }

            return main.JobDate;
            //return minDate;
        }

        //GET : the lastest date of the job based on the date today
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

        //GET : return the list of jobcount 
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

        // JobOrder/JobListing 
        // List of jobs by date with minimal information
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
            var confirmed = dbc.getJobConfirmedListing((int)sortid).Select(s => s.Id);

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

                joTmp.ActionCounter = jobActionCntr.Where(d => d.JobId == joTmp.Main.Id).ToList();
                
                joTmp.Main.JobDate = TempJobDate(joTmp.Main.Id);

                //job payments
                joTmp.Payment = 0;
                List<Models.JobPayment> jobPayment = db.JobPayments.Where(d => d.JobMainId == main.Id).ToList();
                foreach (var payment in jobPayment)
                {
                    //add payments except discount (JobPaymentTypeId = 4)
                    if (payment.JobPaymentTypeId != 4)
                    {
                        joTmp.Payment += payment.PaymentAmt;
                    }
                }

                //add discounts
                //subtract discount amount
                joTmp.Main.AgreedAmt += jo.GetJobDiscountAmount(main.Id);

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
            ViewBag.JobMainId = mainId;
            return View(gret.ItemSched);
        }

        public bool SelectItemSchedule(int? jsId, int? itemId, DateTime jsDate)
        {
            try
            {
                if (jsId == null || itemId == null || jsDate == null)
                {
                    return false;
                }

                var jobService = db.JobServices.Find(jsId);

                if(jobService == null)
                {
                    return false;
                }

                //add item to jobservice
                JobServiceItem jsItem = new JobServiceItem() { 
                    InvItemId = (int)itemId,
                    JobServicesId = (int)jsId
                };
                db.JobServiceItems.Add(jsItem);

                //edit date of jobservice
                jobService.DtStart = jsDate.Add(new TimeSpan(8, 0, 0)); //8AM
                jobService.DtEnd = jsDate.Add(new TimeSpan(17, 0, 0)); //5PM

                db.Entry(jobService).State = EntityState.Modified;

                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
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
            ViewBag.JobMainId = mainId;
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
                if(db.InvItems.Where(s => s.Description == "UnAssigned").FirstOrDefault() != null) { 
                    var unassigned = db.InvItems.Where(s => s.Description == "UnAssigned").FirstOrDefault().Id;
                    RemoveUnassignedItem(unassigned,serviceId);
                }

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
                if (db.InvItems.Where(s => s.Description == "UnAssigned").FirstOrDefault() != null)
                {
                    var unassigned = db.InvItems.Where(s => s.Description == "UnAssigned").FirstOrDefault().Id;
                    RemoveUnassignedItem(unassigned, serviceId);
                }
            }

            var itemName = db.InvItems.Find(itemId);
            var service = db.JobServices.Find(serviceId);

            //job trail
            trail.recordTrail("JobOrder/JobServices", HttpContext.User.Identity.Name, "Assign "+ itemName.Description + " to jobID "+ service.JobMainId + " ", serviceId.ToString());
            
            var mainId = db.JobServices.Find(serviceId).JobMainId;
            return RedirectToAction("JobServices", new { JobMainId = mainId });

        }

        public ActionResult RemoveItem(int itemId, int serviceId)
        {
            string sqlstr = "Delete from JobServiceItems where JobServicesId = " + serviceId.ToString()
                + " AND InvItemId = " + itemId.ToString();

            db.Database.ExecuteSqlCommand(sqlstr);

            var item = db.InvItems.Find(itemId);
            var job = db.JobServices.Find(serviceId).JobMain;

            //job trail
            trail.recordTrail("Remove Item", HttpContext.User.Identity.Name, "Remove Item " + item.Description +" from " + job.Description, serviceId.ToString());

            return RedirectToAction("InventoryItemList", new { serviceId = serviceId });
        }

        public ActionResult JsRemoveItem(int itemId, int serviceId)
        {
            string sqlstr = "Delete from JobServiceItems where JobServicesId = " + serviceId.ToString()
                + " AND InvItemId = " + itemId.ToString();

            db.Database.ExecuteSqlCommand(sqlstr);

            var item = db.InvItems.Find(itemId);
            var job = db.JobServices.Find(serviceId).JobMain;

            //job trail
            trail.recordTrail("Remove Item", HttpContext.User.Identity.Name, "Remove Item " + item.Description + " from " + job.Description, serviceId.ToString());

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

            ViewBag.mainid = jobid;
            ViewBag.CompanyList = db.CustEntMains.ToList() ?? new List<CustEntMain>();
            ViewBag.CustomerList = db.Customers.Where(s => s.Status == "ACT").ToList() ?? new List<Customer>();
            ViewBag.CustomerId  = new SelectList(db.Customers.Where(d => d.Status == "ACT"), "Id", "Name", jobMain.CustomerId);
            ViewBag.BranchId    = new SelectList(db.Branches, "Id", "Name", jobMain.BranchId);
            ViewBag.JobStatusId = new SelectList(db.JobStatus, "Id", "Status", jobMain.JobStatusId);
            ViewBag.JobThruId = new SelectList(db.JobThrus, "Id", "Desc", jobMain.JobThruId);
            ViewBag.CompanyId = new SelectList(db.CustEntMains, "Id", "Name", companyId);
            ViewBag.AssignedTo = new SelectList(dbc.getUsers(), "UserName", "UserName", jobMain.AssignedTo);
            ViewBag.JobPaymentStatusId = new SelectList(db.JobPaymentStatus, "Id", "Status", jo.GetLastJobPaymentStatusId((int)jobid));
            ViewBag.SiteConfig = SITECONFIG;

            return View(jobMain);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult JobDetails([Bind(Include = "Id,JobDate,CompanyId,CustomerId,Description,NoOfPax,NoOfDays,JobRemarks,JobStatusId,StatusRemarks,BranchId,JobThruId,CustContactEmail,CustContactNumber,AssignedTo")] JobMain jobMain, int? CompanyId, decimal? AgreedAmt, int? JobPaymentStatusId)
        {
            if (ModelState.IsValid)
            {
                if (JobCreateValidation(jobMain))
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

                    //Edit job payment status
                    if (JobPaymentStatusId != null)
                    {
                        EditJobPaymentStatus((int)JobPaymentStatusId, jobMain.Id);
                    }

                    //job trail
                    trail.recordTrail("JobOrder/JobServices", HttpContext.User.Identity.Name, "Edit Saved", jobMain.Id.ToString());

                    //add job post record when job is closed (4 = CLOSED)
                    if (jobMain.JobStatusId == 4)
                    {
                        if (CreateJobPostSalesRecord(jobMain.Id))
                        {
                            return RedirectToAction("JobServices", new { JobMainId = jobMain.Id });
                        }
                        else
                        {
                            ModelState.AddModelError("", "Unable to Add Job Post Sale Schedule");
                        }

                    }
                    else
                    {
                        return RedirectToAction("JobServices", new { JobMainId = jobMain.Id });
                    }
                        
                }
            }


            ViewBag.mainid = jobMain.Id;
            ViewBag.CompanyList = db.CustEntMains.ToList() ?? new List<CustEntMain>();
            ViewBag.CustomerList = db.Customers.Where(s => s.Status == "ACT").ToList() ?? new List<Customer>();
            ViewBag.CustomerId = new SelectList(db.Customers.Where(d => d.Status != "INC"), "Id", "Name", jobMain.CustomerId);
            ViewBag.BranchId = new SelectList(db.Branches, "Id", "Name", jobMain.BranchId);
            ViewBag.JobStatusId = new SelectList(db.JobStatus, "Id", "Status", jobMain.JobStatusId);
            ViewBag.JobThruId = new SelectList(db.JobThrus, "Id", "Desc", jobMain.JobThruId);
            ViewBag.CompanyId = new SelectList(db.CustEntMains, "Id", "Name", CompanyId);
            ViewBag.AssignedTo = new SelectList(dbc.getUsers(), "UserName", "UserName", jobMain.AssignedTo);
            ViewBag.JobPaymentStatusId = new SelectList(db.JobPaymentStatus, "Id", "Status", (int)JobPaymentStatusId);
            ViewBag.SiteConfig = SITECONFIG;

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
            job.JobDate = today.GetCurrentDateTime().AddDays(0);
            job.NoOfDays = 1;
            job.NoOfPax = 1;
            job.AgreedAmt = 0;

            if(SITECONFIG == "AutoCare")
            {
                job.JobRemarks = " ";
                job.Description = "GMS AutoCare";
            }

            if (id == null)
            {
                ViewBag.CustomerId = new SelectList(db.Customers.Where(d => d.Status == "ACT" ), "Id", "Name", NewCustSysId);
            }
            else
            {
                ViewBag.CustomerId = new SelectList(db.Customers.Where(d => d.Status == "ACT"), "Id", "Name", id);
            }

            ViewBag.CompanyList = db.CustEntMains.OrderBy(s=>s.Name).ToList() ?? new List<CustEntMain>();
            ViewBag.CustomerList = db.Customers.Where(s => s.Status == "ACT").OrderBy(s=>s.Name).ToList() ?? new List<Customer>();
            ViewBag.CompanyId = new SelectList(db.CustEntMains, "Id", "Name");
            ViewBag.BranchId = new SelectList(db.Branches, "Id", "Name",2);
            ViewBag.JobStatusId = new SelectList(db.JobStatus, "Id", "Status", JOBCONFIRMED);
            ViewBag.JobThruId = new SelectList(db.JobThrus, "Id", "Desc");
            ViewBag.AssignedTo = new SelectList(dbc.getUsers(), "UserName", "UserName");
            ViewBag.JobPaymentStatusId = new SelectList(db.JobPaymentStatus, "Id", "Status", 2);
            ViewBag.SiteConfig = SITECONFIG;


            return View(job);
        }

        
        // POST: JobMains/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult jobCreate([Bind(Include = "Id,JobDate,CustomerId,Description,NoOfPax,NoOfDays,AgreedAmt,JobRemarks,JobStatusId,StatusRemarks,BranchId,JobThruId,CustContactEmail,CustContactNumber,AssignedTo")] JobMain jobMain, int? CompanyId, int? JobPaymentStatusId)
        {
            
            if (ModelState.IsValid)
            {
                if (JobCreateValidation(jobMain))
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

                    if(JobPaymentStatusId != null)
                    {
                        AddJobPaymentStatus((int)JobPaymentStatusId, jobMain.Id);
                    }

                    dbc.addEncoderRecord("joborder", jobMain.Id.ToString(), HttpContext.User.Identity.Name, "Create New Job");
                    return RedirectToAction("JobServices", "JobOrder", new { JobMainId = jobMain.Id });

                }
            }

            ViewBag.CompanyList = db.CustEntMains.ToList() ?? new List<CustEntMain>();
            ViewBag.CustomerList = db.Customers.Where(s => s.Status == "ACT").ToList() ?? new List<Customer>();
            ViewBag.CompanyId = new SelectList(db.CustEntMains, "Id", "Name");
            ViewBag.CustomerId = new SelectList(db.Customers.Where(d => d.Status != "INC"), "Id", "Name", jobMain.CustomerId);
            ViewBag.BranchId = new SelectList(db.Branches, "Id", "Name", jobMain.BranchId);
            ViewBag.JobStatusId = new SelectList(db.JobStatus, "Id", "Status", jobMain.JobStatusId);
            ViewBag.JobThruId = new SelectList(db.JobThrus, "Id", "Desc", jobMain.JobThruId);
            ViewBag.AssignedTo = new SelectList(dbc.getUsers(), "UserName", "UserName", jobMain.AssignedTo);
            ViewBag.JobPaymentStatusId = new SelectList(db.JobPaymentStatus, "Id", "Status",(int)JobPaymentStatusId);
            ViewBag.SiteConfig = SITECONFIG;

            return View(jobMain);
        }


        public bool JobCreateValidation(JobMain jobMain)
        {
            bool isValid = true;

            if (jobMain.JobDate == null )
            {
                ModelState.AddModelError("JobDate", "Invalid JobDate");
                isValid = false;
            }

            if (jobMain.Description.IsNullOrWhiteSpace())
            {
                ModelState.AddModelError("Description", "Invalid Description");
                isValid = false;
            }


            return isValid;
        }

        public void AddjobCompany(int jobId, int companyId)
        {
            JobEntMain jobCompany = new JobEntMain();
            jobCompany.JobMainId = jobId;
            jobCompany.CustEntMainId = companyId;

            db.JobEntMains.Add(jobCompany);
            db.SaveChanges();
        }

        public bool AddJobPaymentStatus(int id, int jobId)
        {
            try
            {

                JobMainPaymentStatus paymentStatus = new JobMainPaymentStatus();
                paymentStatus.JobMainId = jobId;
                paymentStatus.JobPaymentStatusId = id;

                db.JobMainPaymentStatus.Add(paymentStatus);
                db.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }


        public bool EditJobPaymentStatus(int id, int jobId)
        {
            try
            {
                //get latest job payment status
                var tempPaymentStatus = db.JobMainPaymentStatus.Where(j => j.JobMainId == jobId);

                if (tempPaymentStatus.FirstOrDefault() == null)
                {
                    //add job payment status
                    AddJobPaymentStatus(id, jobId);
                }
                else
                {
                    //check if prev status is not current status
                    if(jo.GetLastJobPaymentStatusId(jobId) != id)
                    {
                        //add job payment status
                        AddJobPaymentStatus(id, jobId);
                    }
                }


                return true;
            }
            catch
            {
                return false;
            }
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

            var siteConfig = SITECONFIG;
            if (siteConfig == "AutoCare")
            {
                js.Remarks = " ";
            }

            //modify SupplierItem
            var supItemsActive = db.SupplierItems.Where(s => s.Status != "INC").ToList();
            var SuppliersActive = db.Suppliers.Where(s => s.Status != "INC").ToList();
            
            ViewBag.id = JobMainId;
            ViewBag.JobMainId = new SelectList(db.JobMains, "Id", "Description",job.Description);
            ViewBag.SupplierId = new SelectList(SuppliersActive, "Id", "Name");
            ViewBag.SupplierItemId = new SelectList(supItemsActive, "Id", "Description");
            ViewBag.ServicesId = new SelectList(db.Services.Where(s=>s.Status == "1").ToList(), "Id", "Name");
            return View(js);
        }

        [HttpGet]
        public string GetVehicleOilRemarks(int jobmainId)
        {
            try { 
                var vehicle = db.JobVehicles.Where(j=>j.JobMainId == jobmainId).OrderByDescending(j=>j.Id).FirstOrDefault();
            
                if (vehicle != null) { 
                    var vehicleModel = vehicle.Vehicle.VehicleModel;
                    string MotorOil = "";
                    string GearOil = "";
                    string TransmissionOil = "";

                    if (vehicleModel.MotorOil != null)
                    {
                        MotorOil = " Motor Oil: " + vehicleModel.MotorOil.ToString() + " L, ";
                    }
                    else
                    {
                        MotorOil = " Motor Oil: 0 L, " ;
                    }


                    if (vehicleModel.GearOil != null)
                    {
                        GearOil = " Gear Oil: " + vehicleModel.GearOil.ToString() + " L, ";
                    }
                    else
                    {
                        GearOil = " Gear Oil: 0 L, ";
                    }


                    if (vehicleModel.TransmissionOil != null)
                    {
                        TransmissionOil = " Transmission Oil: " + vehicleModel.TransmissionOil.ToString() + " L ";
                    }
                    else
                    {
                        TransmissionOil = " Transmission Oil: 0 L ";
                    }

                    string OilString = MotorOil + GearOil + TransmissionOil;

                    return OilString;

                }

                return "Oil : No Assigned Vehicle";
            }
            catch
            {
                return "Oil : N/A ";
            }
        }

        // POST: JobServices/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult JobServiceAdd([Bind(Include = "Id,JobMainId,ServicesId,SupplierId,DtStart,DtEnd,Particulars,QuotedAmt,SupplierAmt,ActualAmt,Remarks,SupplierItemId")] JobServices jobServices)
        {
            if (ModelState.IsValid)
            {
                if (jobServices.QuotedAmt == null)
                {
                    jobServices.QuotedAmt = 0;
                    jobServices.ActualAmt = 0;
                }
                else
                {

                    jobServices.ActualAmt = jobServices.QuotedAmt;
                }



                jobServices.DtEnd = ((DateTime)jobServices.DtEnd).Add(new TimeSpan(23, 59, 59));
                db.JobServices.Add(jobServices);
                db.SaveChanges();

                try { 
                    //set initial unit as unassigned
                    int UnassignedId = db.InvItems.Where(u => u.Description == "UnAssigned").FirstOrDefault().Id;
                    AddUnassignedItem(UnassignedId, jobServices.Id);
                }
                catch
                { }
            }

            var supItemsActive = db.SupplierItems.Where(s => s.Status != "INC").ToList();
            var SuppliersActive = db.Suppliers.Where(s => s.Status != "INC").ToList();

            ViewBag.id = jobServices.JobMainId;
            ViewBag.JobMainId = new SelectList(db.JobMains, "Id", "Description", jobServices.JobMainId);
            ViewBag.SupplierId = new SelectList(SuppliersActive, "Id", "Name", jobServices.SupplierId);
            ViewBag.SupplierItemId = new SelectList(supItemsActive, "Id", "Description", jobServices.SupplierItemId);
            ViewBag.ServicesId = new SelectList(db.Services.Where(s => s.Status == "1").ToList(), "Id", "Name", jobServices.ServicesId);

            dbc.addEncoderRecord("jobOrder/jobservice", jobServices.Id.ToString(), HttpContext.User.Identity.Name, "Create New Job Service");

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
            ViewBag.Sdate = jobServices.DtStart.ToString();
            ViewBag.Edate = jobServices.DtEnd.ToString();
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

                if (jobServices.QuotedAmt == null)
                {
                    jobServices.QuotedAmt = 0;
                    jobServices.ActualAmt = 0;
                }
                else
                {
                    jobServices.ActualAmt = jobServices.QuotedAmt;
                }


                //db.SaveChanges();
                updateJobDate(jobServices.JobMainId);
                db.SaveChanges();
                
                //job trail
                trail.recordTrail("JobOrder/JobServiceEdit", HttpContext.User.Identity.Name, "JobService Edit Saved", jobServices.Id.ToString());
            

            }

            var supItemsActive = db.SupplierItems.Where(s => s.Status != "INC").ToList();
            var SuppliersActive = db.Suppliers.Where(s => s.Status != "INC").ToList();


            ViewBag.JobMainId = new SelectList(db.JobMains, "Id", "Description", jobServices.JobMainId);
            ViewBag.SupplierId = new SelectList(SuppliersActive, "Id", "Name", jobServices.SupplierId);
            ViewBag.ServicesId = new SelectList(db.Services.Where(s => s.Status == "1").ToList(), "Id", "Name", jobServices.ServicesId);
            ViewBag.SupplierItemId = new SelectList(supItemsActive, "Id", "Description", jobServices.SupplierItemId);

            dbc.addEncoderRecord("jobOrder/jobservice", jobServices.Id.ToString(), HttpContext.User.Identity.Name, "Edit Job Service");


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

                dbc.addEncoderRecord("JobPickup", jobServicePickup.JobService.JobMainId.ToString(), HttpContext.User.Identity.Name, "Add Job Pickup Details, service id " + jobServicePickup.Id);
                
                return RedirectToAction("JobServices", new { JobMainId = jobServicePickup.JobService.JobMainId });
            }
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


            var Job = db.JobMains.Where(d => d.Id == JobMainId).FirstOrDefault();

            var jobServices = db.JobServices.Include(j => j.JobMain).Include(j => j.Supplier).Include(j => j.Service)
                .Include(j => j.SupplierItem).Include(j => j.JobServicePickups).Where(d => d.JobMainId == JobMainId);

            System.Collections.ArrayList providers = new System.Collections.ArrayList();
            foreach (var item in jobServices)
            {
                if (item.Supplier != null)
                {
                    string sTmp = "";
                    try
                    {
                        sTmp = item.Supplier.Name;
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

            var jobTrailsEncoder = db.JobTrails.Where(s => s.RefTable == "joborder" && s.RefId == JobMainId.ToString());

            if (jobTrailsEncoder.FirstOrDefault() != null)
            {
                ViewBag.JobEncoder = jobTrailsEncoder.FirstOrDefault();
            }
            else {
                ViewBag.JobEncoder = new JobTrail { Id = 0, Action = "Create", user = "none", dtTrail = DateTime.Now, RefId = "0", RefTable = "none" };
            }


            var isAllowedPayment = false;
            if(User.IsInRole("Admin") || User.IsInRole("Accounting"))
            {
                isAllowedPayment = true;
            }

            ViewBag.IsAdmin = isAllowedPayment;
            ViewBag.JobMainId = (int)JobMainId;
            ViewBag.JobOrder = Job;
            ViewBag.JobItems = jobServices;
            ViewBag.Providers = providers;
            ViewBag.JobStatus = Job.JobStatus.Status;
            ViewBag.Itineraries = db.JobItineraries.Where(d => d.JobMainId == JobMainId).ToList();
            ViewBag.sortid = sortid;
            ViewBag.jobAction = action;
            ViewBag.user = HttpContext.User.Identity.Name;
            ViewBag.Vehicles = jvc.GetCustomerVehicleList((int)JobMainId);
            ViewBag.JobVehicle = jvc.GetJobVehicle((int)JobMainId);
            ViewBag.SiteConfig = SITECONFIG;

            var veh = jvc.GetCustomerVehicleList((int)JobMainId);
            return View(jobServices.OrderBy(d => d.DtStart).ToList());

        }

        public ActionResult BookingRedirect(int id,string month, string day, string year,string rName) {
            String DateBook = month + "/" + day + "/" + year;
            DateTime booking = DateTime.Parse(DateBook);
            int iMonth = int.Parse(month);
            int iday = int.Parse(day);
            int iyear = int.Parse(year);

            JobMain job = db.JobMains.Where(j => j.Id == id).
                Where(j => j.JobDate.Month == iMonth).
                Where(j => j.JobDate.Day == iday).
                Where(j => j.JobDate.Year == iyear).
                Where(j => j.Description == rName).
                FirstOrDefault();
            
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
            string sLine1 = "Door 1 Travelers Inn, Matina Pangi Rd. Matina Crossing, Davao City ";
            string sLine2 = "Tel# (+63)82 333-5157; (+63)9167558473; (+63)9330895358 ";
            string sLine3 = "Email: ajdavao88@gmail.com; Website: http://www.AJDavaoCarRental.com/";
            string sLine4 = "TIN: 414-880-772-001 (non-Vat)";
            string sLogo = "LOGO_AJRENTACAR.jpg";
            Bank bank = db.Banks.Find(2);

            if (jobMain.Branch.Name == "RealBreeze")
            {
                sCompany = "Real Breeze Travel & Tours - Davao City";
                sLine1 = "Door 1 Travelers Inn, Matina Pangi Rd. Matina Crossing, Davao City";
                sLine2 = "Tel# (+63)82 333-5157; (+63)916-755-8473; (+63)933-089-5358 ";
                sLine3 = "Email: RealBreezeDavao@gmail.com; Website: http://www.realbreezedavaotours.com//";
                sLine4 = "TIN: 414-880-772-000 (non-Vat)";
                sLogo = "RealBreezeLogo01.png";
                bank = db.Banks.Find(3);
            }

            if (jobMain.Branch.Name == "AJ88")
            {
                sCompany = "AJ88 Car Rental Services";
                sLine1 = "Door 1 Travelers Inn, Matina Pangi Rd. Matina Crossing, Davao City";
                sLine2 = "Tel# (+63)82 333-5157; (+63)9167558473; (+63)9330895358 ";
                sLine3 = "Email: ajdavao88@gmail.com; Website: http://www.AJDavaoCarRental.com/";
                sLine4 = "TIN: 414-880-772-001 (non-Vat)";
                sLogo = "LOGO_AJRENTACAR.jpg";
                bank = db.Banks.Find(2);
            }

            if (jobMain.Branch.Name == "RealWheels")
            {
                sCompany = "RealWheels Davao ";
                sLine1 = "Door 1 Travelers Inn, Matina Pangi Rd. Matina Crossing, Davao City";
                sLine2 = "Tel# (+63)82 333-5157; (+63)9167558473; (+63)9330895358 ";
                sLine3 = "Email: inquiries.realwheels@gmail.com; Website: http://www.Realwheelsdavao.com/";
                sLine4 = "TIN: 414-880-772-001 (non-Vat)";
                sLogo = "";
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

            ViewBag.isPaymentValid = jobMain.JobDate.Date == today ? "True" : "False";


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

            ViewBag.custCompany = custCompany;

            ViewBag.DateNow = getDateTimeToday().Date.ToString();

            //filter name and jobname if the same or personal account
            var filteredName = "";

            if (jobMain.Customer.Name == "Personal Account")
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
            string sLine1 = "Door 1 Travelers Inn Bldg., Matina Crossing Rd., Matina Pangi, Davao City, 8000 ";
            string sLine2 = "Tel# (+63)82 333-5157; (+63)9167558473; (+63)9330895358 ";
            string sLine3 = "Email: ajdavao88@gmail.com; Website: http://www.AJDavaoCarRental.com/";
            string sLine4 = "TIN: 414-880-772-001 (non-Vat)";
            string sLogo = "LOGO_AJRENTACAR.jpg";
            Bank bank = db.Banks.Find(2);

            if (jobMain.Branch.Name == "RealBreeze")
            {
                sCompany = "Real Breeze Travel & Tours - Davao City";
                sLine1 = "Door 1 Travelers Inn Bldg., Matina Crossing Rd., Matina Pangi, Davao City, 8000";
                sLine2 = "Tel# (+63)82 333-5157; (+63)916-755-8473; (+63)933-089-5358 ";
                sLine3 = "Email: RealBreezeDavao@gmail.com; Website: http://www.realbreezedavaotours.com//";
                sLine4 = "TIN: 414-880-772-000 (non-Vat)";
                sLogo = "RealBreezeLogo01.png";
                bank = db.Banks.Find(3);
            }

            if (jobMain.Branch.Name == "AJ88")
            {
                sCompany = "AJ88 Car Rental Services";
                sLine1 = "Door 1 Travelers Inn Bldg., Matina Crossing Rd., Matina Pangi, Davao City, 8000";
                sLine2 = "Tel# (+63)82 333-5157; (+63)9167558473; (+63)9330895358 ";
                sLine3 = "Email: ajdavao88@gmail.com; Website: http://www.AJDavaoCarRental.com/";
                sLine4 = "TIN: 414-880-772-001 (non-Vat)";
                sLogo = "LOGO_AJRENTACAR.jpg";
                bank = db.Banks.Find(2);
            }

            if (jobMain.Branch.Name == "RealWheels")
            {
                sCompany = "RealWheels Davao Car Rental";
                sLine1 = "Door 1 Travelers Inn Bldg., Matina Crossing Rd., Matina Pangi, Davao City, 8000";
                sLine2 = "Tel# (+63)82 333-5157; (+63)9954508517; (+63)9193812657 ";
                sLine3 = "Email: inquiries.realwheels@gmail.com; Website: https://realwheelsdavao.com/";
                sLine4 = " ";
                sLogo = "Logo_Realwheels.png";
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
            PaypalAccount paypal = new PaypalAccount();
            if (db.PaypalAccounts.Where(p => p.SysCode.Equals("RealWheels")).FirstOrDefault() != null)
                paypal = db.PaypalAccounts.Where(p => p.SysCode.Equals("RealWheels")).FirstOrDefault();
            if (paypal != null)
            {
                ViewBag.key = paypal.Key ?? "NA";
            }
            ViewBag.key = "NA";

            ViewBag.isPaymentValid = jobMain.JobDate.Date == today ? "True" : "False" ;


            //handle prepared by
            var encoder = db.JobTrails.Where(s => s.RefTable == "joborder" && s.RefId == jobMain.Id.ToString()).FirstOrDefault();
            ViewBag.StaffName = getStaffName(encoder.user);
            ViewBag.Sign = getStaffSign(encoder.user);
            ViewBag.DateNow = getDateTimeToday().Date.ToString();

            //filter name and jobname if the same or personal account
            var filteredName = "";

            if (jobMain.Customer.Name == "Personal Account")
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
            else if (iType != null && (int)iType == 2)
            { //Trip Voucher
                ViewBag.DateNow = getDateTimeToday().Date.ToString();
                return View("Details_Voucher", jobMain);
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
                case "jecca.realbreeze@gmail.com":
                    return "Jecca Bilason";
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
                case "jecca.realbreeze@gmail.com":
                    return "/Images/Signature/Jecca.Sign.jpg";
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
                if (svc.JobMain.NoOfPax != 0 )
                sData += "\nNo.Pax:  " + svc.JobMain.NoOfPax;

                sData += "\n\nThank you for Trusting \n" + custName;
            }

            ViewBag.StrData = sData;
            return View();
        }

        public ActionResult TextConfirmation(int? id)
        {
            string sData = "\n";
            decimal totalAmount = 0;
            Models.JobServiceItem svcpu;
            Models.JobMain jobmain = db.JobMains.Find(id);
            var svc = db.JobServices.Where(j=>j.JobMainId == id).ToList();
            string custName = jobmain.Branch.Name;
            int pickupCount = 0;

            sData += "Booking Confirmation";

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
                            sData += "\n\nPickup: ";
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

                if (jobmain.JobRemarks != null)
                {
                    sData += "\nRemarks: " + jobmain.JobRemarks;
                }

                if (jobmain.NoOfPax != 0)
                    sData += "\nNo.Pax:  " + jobmain.NoOfPax;
                sData += "\n\nThank you and have a nice day.\n";
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
            string sData = "\nBooking Details";
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
                if (jobmain.JobRemarks != null)
                {
                    sData += "\nRemarks: " + jobmain.JobRemarks;
                }

                if (jobmain.NoOfPax != 0)
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

        public ActionResult CloseJobStatus(int? id)
        {
            var Job = db.JobMains.Find(id);
            Job.JobStatusId = 4;
            db.Entry(Job).State = EntityState.Modified;
            db.SaveChanges();

            //job trail
            trail.recordTrail("JobOrder/JobServices", HttpContext.User.Identity.Name, "Job Status changed to CONFIRMED", id.ToString());

            return RedirectToAction("JobServices", "JobOrder", new { JobMainId = id });
        }


        public ActionResult ConfirmJobStatus(int? id)
        {
            var Job = db.JobMains.Find(id);
            Job.JobStatusId = 3;
            db.Entry(Job).State = EntityState.Modified;
            db.SaveChanges();

            //job trail
            trail.recordTrail("JobOrder/JobServices", HttpContext.User.Identity.Name, "Job Status changed to CONFIRMED", id.ToString());

            return RedirectToAction("JobServices", "JobOrder", new { JobMainId = id });
        }
        
        public ActionResult ReserveJobStatus(int? id)
        {
            var Job = db.JobMains.Find(id);
            Job.JobStatusId = 2;
            db.Entry(Job).State = EntityState.Modified;
            db.SaveChanges();

            //job trail
            trail.recordTrail("JobOrder/JobServices", HttpContext.User.Identity.Name, "Job Status changed to RESERVED", id.ToString());

            return RedirectToAction("JobServices", "JobOrder", new { JobMainId = id });
        }


        //GET : payment transction history
        //[HttpGet]
        public string GetJobServices(int? jobId)
        {

            List<cJobPayment> paymentList = new List<cJobPayment>();

            //get transactions
            List<JobPayment> jobTrans = db.JobPayments.Where(j => j.JobMainId == jobId).ToList();
            foreach (var payment in jobTrans)
            {
                paymentList.Add(new cJobPayment
                {
                    Id = payment.Id,
                    DtPayment = payment.DtPayment,
                    Amount = payment.PaymentAmt,
                    Type = payment.Bank.BankName
                });

            }
            return JsonConvert.SerializeObject(paymentList, Formatting.Indented);
        }

        public string CloseJob(int id)
        {
            try
            {

                var Job = db.JobMains.Find(id);
                Job.JobStatusId = 4;
                db.Entry(Job).State = EntityState.Modified;
                db.SaveChanges();

                //job trail
                trail.recordTrail("JobOrder/JobServices", HttpContext.User.Identity.Name, "Job Status changed to CLOSED", id.ToString());

                if(CreateJobPostSalesRecord(id))
                    return "OK";
                return "Error";
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
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
                return RedirectToAction("Payments", "JobPayments", new { id = jobPayment.JobMainId });
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

            DateClass today = new DateClass();
            DateTime month = today.GetCurrentDate();
            return View(db.JobTrails.OrderByDescending(s=>s.dtTrail).Where(s=>s.dtTrail.Month == month.Month).ToList());
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
                    case "RESERVED":
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
        
        #region Paypal Transaction

        //PAYPAL PAYMENT
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

        public string AddPaypalActivity(int id, string activity, decimal amount, string transId, string trxDate)
        {
            try
            {
                //Create Transaction History
                PaypalTransaction trans = new PaypalTransaction();
                trans.JobId   = id;
                trans.TrxDate =  DateTime.Parse(trxDate);
                trans.TrxId   = transId;
                trans.Status  = activity;
                trans.Remarks = " ";
                trans.Amount  =  amount;
                trans.DatePosted = dt.GetCurrentDateTime();

                db.PaypalTransactions.Add(trans);
                db.SaveChanges();

                return "200";

            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
        #endregion

        #region ajax calls

        [HttpGet]
        public string GetJobServicesData(int id)
        {
            var servicesList = db.JobServices.Where(s => s.JobMainId == id).ToList();
            List<cJobOrderServices> services = new List<cJobOrderServices>();

            foreach (var svc in servicesList)
            {
                List<cUnitList> units = new List<cUnitList>();


                var svcUnits = db.JobServiceItems.Where(s => s.JobServicesId == svc.Id);
                foreach (var unit in svcUnits)
                {
                    units.Add(new cUnitList {
                        Id = unit.Id,
                        Unit = unit.InvItem.Description,
                        Code = unit.InvItem.ItemCode,
                        ViewInfo = unit.InvItem.ViewLabel
                    });
                }

                services.Add(new cJobOrderServices
                {
                    Id = svc.Id,
                    Particulars = svc.Particulars,
                    DtStart = (DateTime)svc.DtStart,
                    DtEnd = (DateTime)svc.DtEnd,
                    Remarks = svc.Remarks,
                    Supplier = svc.Supplier.Name,
                    SupplierItem = svc.SupplierItem.Description,
                    cUnits = units,
                    ActualAmt = 1000,
                    ServiceType = svc.Service.Name,
                    JobPickup = GetJobServicePickup(svc.Id)

                });
            }

            return JsonConvert.SerializeObject(services, Formatting.Indented);
        }


        public string GetJobServicePickup(int svcId)
        {

            var pickup = db.JobServicePickups.Where(s => s.JobServicesId == svcId) != null ? db.JobServicePickups.Where(s => s.JobServicesId == svcId).FirstOrDefault() : null;

            string pickupInstructions = "";
            if (pickup != null)
            {
                pickupInstructions = " [  <b>Time:</b> " + pickup.JsTime + " " + pickup.JsLocation + " ] &nbsp;" ;
                pickupInstructions += " [ <b>Contact:</b> " + pickup.ClientContact + " / " + pickup.ClientName + " ] &nbsp;";
                pickupInstructions += " [ <b>InCharge:</b> " + pickup.ProviderContact + " / " + pickup.ProviderName + " ] &nbsp;";
            }

            return pickupInstructions;

        }
        #endregion

        #region Vehicles
        public bool AddJobVehicle(int? jobMainId, int? vehicleId, int? mileage)
        {

            try
            {

                if (jobMainId == null || vehicleId == null || mileage == null )
                {
                   return false;
                }

                return jvc.AddJobVehicle(jobMainId, vehicleId, mileage);

                
            }
            catch 
            {
                return false;
            }
        }

        #endregion

        #region Job Post Sales
        public bool CreateJobPostSalesRecord(int jobMainId)
        {
            var jobServiceList = db.JobServices.Where(j=>j.JobMainId == jobMainId).ToList();

            if (jobServiceList != null)
            {
                var AddResult = false;
                foreach (var service in jobServiceList)
                {
                    AddResult = AddJobPostSales(service.Id);

                    if (AddResult == false)
                    {
                        return AddResult;
                    }
                }

                return AddResult;
            }
            return false;
        }

        public bool AddJobPostSales(int jobServiceId)
        {
            try
            {
                var User = HttpContext.User.Identity.Name;
                var jobServices = db.JobServices.Find(jobServiceId);
                var PostSalesInterval = jobServices.SupplierItem.Interval == null ? 60 : (int)jobServices.SupplierItem.Interval;
                var PostSalesDate = ((DateTime)jobServices.DtStart).AddDays(PostSalesInterval);

                JobPostSale postSale = new JobPostSale()
                {
                    DtPost = PostSalesDate,
                    DoneBy = User,
                    JobServicesId = jobServiceId,
                    Remarks = ""
                };

                db.JobPostSales.Add(postSale);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion

        public ActionResult ErrorPage()
        {
            return View();
        }
    }
}