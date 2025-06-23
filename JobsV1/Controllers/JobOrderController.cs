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
using System.Globalization;
using System.Globalization;
using System.Web.Helpers;

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
        public DateTime DtStart { get; set; }
        public DateTime DtEnd { get; set; }
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
        //private int JOBTEMPLATE = 6;

        private string SITECONFIG = ConfigurationManager.AppSettings["SiteConfig"].ToString();

        private DateClass dt = new DateClass();
        private DBClasses dbc = new DBClasses();
        private JobOrderClass jo = new JobOrderClass();
        private JobDBContainer db = new JobDBContainer();
        private SysAccessLayer sal = new SysAccessLayer();
        private JobVehicleClass jvc = new JobVehicleClass();
        private ActionTrailClass trail = new ActionTrailClass();

        // GET: JobOrder
        public ActionResult Index(int? sortid, int? serviceId, int? mainid, string search)
        {
            #region Session
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
            #endregion

            // get job list data
            var data = jo.GetJobData((int)sortid);

            // Search Filter
            if (!search.IsNullOrWhiteSpace())
            {
                data = jo.GetSearchJobData(search);
            }

            var jobmainId = serviceId != null ? db.JobServices.Find(serviceId).JobMainId : 0;
            jobmainId = mainid != null ? (int)mainid : jobmainId;

            ViewBag.SrchValue = search;
            ViewBag.mainId = jobmainId;
            ViewBag.SiteConfig = SITECONFIG;
            ViewBag.companyList = db.Customers.ToList();
            ViewBag.JobVehicle = jvc.GetJobVehicle(jobmainId);
            ViewBag.IsAdmin = User.IsInRole("Admin") || User.IsInRole("Accounting") ? true : false;

            if (sortid == 1)
            {
                return View(data.OrderBy(d => d.Main.JobDate));
            }
            else
            {
                return View(data.OrderByDescending(d => d.Main.JobDate));
            }
        }

        // GET: JobOrder/JobStatus
        // Return list of jobs with status of the jobs
        public ActionResult JobStatus(int? sortid, int? serviceId, int? mainid)
        {
            #region Session
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
            #endregion

            var data = jo.GetJobData((int)sortid); // get job list data

            //Get List of Customers
            ViewBag.companyList = db.Customers.ToList();

            //Handle jobmainId
            var jobmainId = serviceId != null ? db.JobServices.Find(serviceId).JobMainId : 0;
            jobmainId = mainid != null ? (int)mainid : jobmainId;
            ViewBag.mainId = jobmainId;

            //job sorting order
            if (sortid == 1) {
                return View(data.OrderBy(d => d.Main.JobDate));
            } else {
                return View(data.OrderByDescending(d => d.Main.JobDate));
            }

        }

        // GET: JobOrder/JobListing 
        // List of jobs by date with minimal information
        public ActionResult JobListing(int? sortid, int? serviceId, int? mainid, string search)
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

            var data = new List<cJobOrder>();

            //get date fom SQL query
            var confirmed = dbc.getJobConfirmedListing((int)sortid).Select(s => s.Id);

            IEnumerable<Models.JobMain> jobMains = db.JobMains.Where(j => confirmed.Contains(j.Id))
                .Include(j => j.Customer)
                .Include(j => j.Branch)
                .Include(j => j.JobStatus)
                .Include(j => j.JobThru)
                .Include(j => j.JobEntMains)
                ;
            List<cjobCounter> jobActionCntr = jo.GetJobActionCount(jobMains.Select(d => d.Id).ToList());

            DateTime today = dt.GetCurrentDate();
            ViewBag.today = today;

            foreach (var main in jobMains)
            {
                cJobOrder joTmp = new cJobOrder();
                joTmp.Main = main;
                joTmp.Services = new List<cJobService>();
                joTmp.Main.AgreedAmt = 0;
                joTmp.Payment = 0;
                joTmp.Expenses = 0;
                joTmp.DtStart = jo.GetMinMaxServiceDate(main.Id, "min");
                joTmp.DtEnd = jo.GetMinMaxServiceDate(main.Id, "max");

                List<Models.JobServices> joSvc = db.JobServices.Where(d => d.JobMainId == main.Id).OrderBy(s => s.DtStart).ToList();
                foreach (var svc in joSvc)
                {
                    cJobService cjoTmp = new cJobService();
                    cjoTmp.Service = svc;

                    joTmp.Main.AgreedAmt += svc.ActualAmt != null ? svc.ActualAmt : 0;
                    joTmp.Company = db.JobEntMains.Where(j => j.JobMainId == svc.JobMainId).FirstOrDefault() != null ? db.JobEntMains.Where(j => j.JobMainId == svc.JobMainId).FirstOrDefault().CustEntMain.Name : "";
                    joTmp.Expenses += jo.GetJobExpensesBySVC(svc.Id);

                    joTmp.Services.Add(cjoTmp);

                    //calculate total rate and payment
                }

                cjobIncome cIncome = new cjobIncome();
                cIncome.Car = 0;
                cIncome.Tour = 0;
                cIncome.Others = 0;

                joTmp.isPosted = jo.GetJobPostedInReceivables(joTmp.Main.Id);
                joTmp.PostedIncome = cIncome;
                joTmp.ActionCounter = jobActionCntr.Where(d => d.JobId == joTmp.Main.Id).ToList();
                joTmp.Main.JobDate = jo.TempJobDate(joTmp.Main.Id);

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
                        .Where(d => DateTime.Compare(d.Main.JobDate.Date.AddDays(1), today.Date) >= 0).ToList();
                    break;
                case 2: //prev
                    data = (List<cJobOrder>)data
                        .Where(d => (d.Main.JobStatusId == JOBINQUIRY || d.Main.JobStatusId == JOBRESERVATION || d.Main.JobStatusId == JOBCONFIRMED)).ToList()
                        .Where(p => DateTime.Compare(p.Main.JobDate.Date, today.Date) < 0).ToList();

                    //Closed and Current Month List
                    var currentMonthIds = dbc.currentJobsMonth().Select(s => s.Id);   //get list if job ids of current month fom SQL query
                    var currentMonthJobs = jo.GetJobListing(currentMonthIds);
                    ViewBag.CurrentMonth = currentMonthJobs;


                    //Old Open jobs
                    var olderJobsIds = dbc.olderOpenJobs().Select(s => s.Id);  //get list of older jobs that are not closed
                    var OldJobs = jo.GetJobListing(olderJobsIds).Where(s=>s.DtStart < today);
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

        // GET : JobOrder/GetActiveJobList
        // Get Active Jobs with the specified date of triplogs for 
        // linking triplogs and job thru jobId
        // PARAM: tripdate - Triplog date type of Datetime
        [HttpGet]
        public JsonResult GetActiveJobList(DateTime tripDate)
        {
            if (Session["CachedJobs"] == null)
            {

                var confirmed = jo.GetJobOrderListingQuery(1);

                var cachedJobs = confirmed.GroupBy(c => c.Id,
                    (key, g) => new
                    {
                        Id = key,
                        JobDesc = g.Select(gs => gs.Description),
                        Customer = g.Select(gs => gs.Customer),
                        JobDateStart = jo.GetMinMaxServiceDate(key, "min"),
                        JobDateEnd = jo.GetMinMaxServiceDate(key, "max"),
                        Company = jo.GetJobCompany(key),
                        NoItems = GetJobItemsCount(key)
                    }).Where(j => tripDate >= j.JobDateStart
                               && tripDate <=  j.JobDateEnd)
                             .ToList();

                //Session["CachedJobs"] = cachedJobs;
                return Json(cachedJobs, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var cachedJobs = Session["CachedJobs"];
                return Json(cachedJobs, JsonRequestBehavior.AllowGet);
            }
        }

        // GET : JobOrder/GetActiveJobById
        // PARAM: tripdate - Triplog date type of Datetime
        [HttpGet]
        public JsonResult GetActiveJobById(int jobId)
        {
            var confirmed = jo.GetJobOrderDetailsQuery(jobId);


            if (confirmed != null)
            {

                DateTime jobDateStart= jo.GetMinMaxServiceDate(jobId, "min");
                DateTime jobDateEnd = jo.GetMinMaxServiceDate(jobId,"max");

                return Json(new {
                    Id = confirmed.Id,
                    JobDesc = confirmed.Description,
                    Customer = confirmed.Customer,
                    JobDateStart = jobDateStart,
                    JobDateEnd = jobDateEnd,
                    Company = confirmed.Company != null ? confirmed.Company : "",
                    NoItems = GetJobItemsCount(confirmed.Id)

                }, JsonRequestBehavior.AllowGet);
            }

            return null;
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

                if (jobService == null)
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
            try
            {

                //get items list of order number greater than 110
                var InvItems = db.InvItems.Where(s => s.OrderNo > 110).Include(s => s.InvItemCategories).ToList().OrderBy(s => s.OrderNo);
                List<cjobItems> items = new List<cjobItems>();

                foreach (var inv in InvItems)
                {
                    var invIcon = db.InvItemCategories.Where(s => s.InvItemId == inv.Id).FirstOrDefault();
                    var imgIconPath = "";
                    if (invIcon == null)
                    {
                        imgIconPath = "//Images/CarRental/suv-101.png";
                    }
                    else
                    {
                        imgIconPath = invIcon.InvItemCat.ImgPath;
                    }

                    items.Add(new cjobItems {
                        Id = inv.Id,
                        itemCode = inv.ItemCode,
                        Name = inv.Description,
                        remarks = inv.Remarks,
                        orderNo = (int)inv.OrderNo,
                        icon = imgIconPath.Remove(0, 1)
                    });
                }

                //convert list to json object
                return JsonConvert.SerializeObject(items, Formatting.Indented);

            }
            catch
            {
                return JsonConvert.SerializeObject("Unable to Retrieve items", Formatting.Indented);
            }
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
            var jscount = db.JobServiceItems.Where(s => s.JobServicesId == serviceId).Count();

            if (jscount > 1)
            {
                if (db.InvItems.Where(s => s.Description == "UnAssigned").FirstOrDefault() != null) {
                    var unassigned = db.InvItems.Where(s => s.Description == "UnAssigned").FirstOrDefault().Id;
                    RemoveUnassignedItem(unassigned, serviceId);
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
            trail.recordTrail("JobOrder/JobServices", HttpContext.User.Identity.Name, 
                "Assign " + itemName.Description + " to jobID " + service.JobMainId + " ", 
                serviceId.ToString());

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
            trail.recordTrail("Remove Item", HttpContext.User.Identity.Name, "Remove Item " + item.Description + " from " + job.Description, serviceId.ToString());

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

        private int GetJobItemsCount(int jobId)
        {
            try
            {
                var services = db.JobServices.Where(j => j.JobMainId == jobId).ToList();

                if (services != null)
                {
                   return services.FirstOrDefault().JobServiceItems.Count();
                }

                return 0;
            }
            catch {
                return 0;
            }
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
                return RedirectToAction("CreateCustomer", new { CreateCustJobId = jobid });
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

                return RedirectToAction("Index", new { mainid = mainid });
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
            //ViewBag.Status = new SelectList("JobStatusId", "Id", "text", data.Status);
            ViewBag.jobOrderId = CreateCustJobId;

            return View(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateCustomer([Bind(Include = "Id,Name,Email,Contact1,Contact2,Remarks,Status")] Customer customer, int? sortid, int jobOrderId)
        {

            if (ModelState.IsValid)
            {
                if (customer.Status == null || customer.Status.Trim() == "") customer.Status = "ACT";

                db.Customers.Add(customer);
                db.SaveChanges();

                var job = db.JobMains.Find(jobOrderId);
                job.CustomerId = customer.Id;

                db.Entry(job).State = EntityState.Modified;
                db.SaveChanges();

                //string JobId = Request.Form["jobOrderId"];
                //db.Database.ExecuteSqlCommand(@"
                //    Update JobMains set CustomerId=" + customer.Id + " where Id=" + JobId + ";"
                //    );

                return RedirectToAction("JobServices", new { JobMainId = jobOrderId });
            }

            ViewBag.Status = new SelectList(StatusList, "value", "text", customer.Status);
            return View(customer);
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
            string custNum2 = db.Customers.Find(id).Contact2;
            return custNum2;
        }

        //GET : return customer company name
        public string getCustomerCompany(int id)
        {
            var company = db.CustEntities.Where(s => s.CustomerId == id).FirstOrDefault();
            string companyNum = company != null ? company.CustEntMainId.ToString() : " ";
            return companyNum;
        }
        #endregion

        #region jobMain
        [Authorize]
        public ActionResult JobDetails(int jobid)
        {
            var jobMain = db.JobMains.Find(jobid);
            var companyId = db.JobEntMains.Where(s => s.JobMainId == jobMain.Id).FirstOrDefault() != null ?
                db.JobEntMains.Where(s => s.JobMainId == jobMain.Id).FirstOrDefault().CustEntMainId : 1;

            ViewBag.mainid = jobid;
            ViewBag.CompanyList = db.CustEntMains.ToList() ?? new List<CustEntMain>();
            ViewBag.CustomerList = db.Customers.Where(s => s.Status == "ACT").ToList() ?? new List<Customer>();
            ViewBag.CustomerId = new SelectList(db.Customers.Where(d => d.Status == "ACT"), "Id", "Name", jobMain.CustomerId);
            ViewBag.BranchId = new SelectList(db.Branches, "Id", "Name", jobMain.BranchId);
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
        public ActionResult JobDetails([Bind(Include = "Id,JobDate,CompanyId,CustomerId,Description,NoOfPax,NoOfDays,JobRemarks,JobStatusId,StatusRemarks,BranchId,JobThruId,CustContactEmail,CustContactNumber,AssignedTo,DueDate")] JobMain jobMain,
            int? CompanyId, decimal? AgreedAmt, int? JobPaymentStatusId)
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

                    System.Diagnostics.Debug.WriteLine("----");
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


                    return RedirectToAction("JobServices", new { JobMainId = jobMain.Id });

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
            if (db.JobEntMains.Where(j => j.JobMainId == jobId).FirstOrDefault() == null)
            {
                //add if entry does not exist
                AddjobCompany(jobId, companyId);
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
        [Authorize]
        public ActionResult jobCreate(int? id)
        {
            DateClass today = new DateClass();

            JobMain job = new JobMain();
            job.JobDate = dt.GetCurrentDate();
            job.DueDate = null;
            job.NoOfDays = 1;
            job.NoOfPax = 1;
            job.AgreedAmt = 0;

            if (SITECONFIG == "AutoCare")
            {
                job.JobRemarks = " ";
                job.Description = "GMS AutoCare";
            }

            if (id == null)
            {
                ViewBag.CustomerId = new SelectList(db.Customers.Where(d => d.Status == "ACT"), "Id", "Name", NewCustSysId);
            }
            else
            {
                ViewBag.CustomerId = new SelectList(db.Customers.Where(d => d.Status == "ACT"), "Id", "Name", id);
            }
            var signedUser = HttpContext.User.Identity.Name;
            ViewBag.CompanyList = db.CustEntMains.OrderBy(s => s.Name).ToList() ?? new List<CustEntMain>();
            ViewBag.CustomerList = db.Customers.Where(s => s.Status == "ACT").OrderBy(s => s.Name).ToList() ?? new List<Customer>();
            ViewBag.CompanyId = new SelectList(db.CustEntMains, "Id", "Name");
            ViewBag.BranchId = new SelectList(db.Branches, "Id", "Name", 2);
            ViewBag.JobStatusId = new SelectList(db.JobStatus, "Id", "Status", JOBCONFIRMED);
            ViewBag.JobThruId = new SelectList(db.JobThrus, "Id", "Desc");
            ViewBag.AssignedTo = new SelectList(dbc.getUsers_wdException(), "UserName", "UserName", signedUser);
            ViewBag.JobPaymentStatusId = new SelectList(db.JobPaymentStatus, "Id", "Status", 2);
            ViewBag.SiteConfig = SITECONFIG;

            return View(job);
        }


        // POST: JobMains/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult jobCreate([Bind(Include = "Id,JobDate,CustomerId,Description,NoOfPax,NoOfDays,AgreedAmt,JobRemarks,JobStatusId,StatusRemarks,BranchId,JobThruId,CustContactEmail,CustContactNumber,AssignedTo,DueDate")] JobMain jobMain, int? CompanyId, int? JobPaymentStatusId)
        {

            if (ModelState.IsValid)
            {
                if (JobCreateValidation(jobMain))
                {
                    if (jobMain.Customer == null)
                    {
                        var customerRecord = db.Customers.Find(jobMain.CustomerId);
                        jobMain.Customer = customerRecord;
                    }

                    db.JobMains.Add(jobMain);
                    db.SaveChanges();

                    if (CompanyId != null)
                    {
                        //new company
                        AddjobCompany(jobMain.Id, (int)CompanyId);
                    }

                    if (JobPaymentStatusId != null)
                    {
                        AddJobPaymentStatus((int)JobPaymentStatusId, jobMain.Id);
                    }

                    dbc.addEncoderRecord("joborder", jobMain.Id.ToString(), HttpContext.User.Identity.Name, "Create New Job");
                     

                    if (jobMain.Customer.Name == "<< New Customer >>")
                    {
                        //Create New Customer Account
                        return RedirectToAction("CreateCustomer", new { CreateCustJobId = jobMain.Id });
                    }

                    return RedirectToAction("JobServices", "JobOrder", new { JobMainId = jobMain.Id });

                }
            }

            if (JobPaymentStatusId == null)
            {
                JobPaymentStatusId = 2;
            }


            if (jobMain.Id == 0)
            {
                ViewBag.CustomerId = new SelectList(db.Customers.Where(d => d.Status == "ACT"), "Id", "Name", NewCustSysId);
            }
            else
            {
                ViewBag.CustomerId = new SelectList(db.Customers.Where(d => d.Status == "ACT"), "Id", "Name", jobMain.Id);
            }

            ViewBag.CompanyList = db.CustEntMains.ToList() ?? new List<CustEntMain>();
            ViewBag.CustomerList = db.Customers.Where(s => s.Status == "ACT").ToList() ?? new List<Customer>();
            ViewBag.CompanyId = new SelectList(db.CustEntMains, "Id", "Name");
            ViewBag.CustomerId = new SelectList(db.Customers.Where(d => d.Status != "INC"), "Id", "Name", jobMain.CustomerId);
            ViewBag.BranchId = new SelectList(db.Branches, "Id", "Name", jobMain.BranchId);
            ViewBag.JobStatusId = new SelectList(db.JobStatus, "Id", "Status", jobMain.JobStatusId);
            ViewBag.JobThruId = new SelectList(db.JobThrus, "Id", "Desc", jobMain.JobThruId);
            ViewBag.AssignedTo = new SelectList(dbc.getUsers(), "UserName", "UserName", jobMain.AssignedTo);
            ViewBag.JobPaymentStatusId = new SelectList(db.JobPaymentStatus, "Id", "Status", (int)JobPaymentStatusId);
            ViewBag.SiteConfig = SITECONFIG;

            return View(jobMain);
        }

        public bool JobCreateValidation(JobMain jobMain)
        {
            bool isValid = true;

            if (jobMain.JobDate == null)
            {
                ModelState.AddModelError("JobDate", "Invalid JobDate");
                isValid = false;
            }

            if (jobMain.Description.IsNullOrWhiteSpace())
            {
                ModelState.AddModelError("Description", "Invalid Description");
                isValid = false;
            }


            if (jobMain.CustContactNumber.IsNullOrWhiteSpace())
            {
                ModelState.AddModelError("CustContactNumber", "Invalid Contact Number");
                isValid = false;
            }
            else
            {
                if (jobMain.CustContactNumber.Length < 5)
                {
                    ModelState.AddModelError("CustContactNumber", "Invalid Contact Number");
                    isValid = false;
                }

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
                    if (jo.GetLastJobPaymentStatusId(jobId) != id)
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

        public ActionResult ChangeCompany(int id, int newId) {

            JobMain jobMain = db.JobMains.Find(id);
            jobMain.CustomerId = newId;
            db.Entry(jobMain).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Index", new { mainid = jobMain.Id });
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

                return RedirectToAction("Index", new { sortid = 1, serviceId = SrcId });
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
        [Authorize]
        public ActionResult JobServiceAdd(int? JobMainId) {
            Models.JobMain job = db.JobMains.Find((int)JobMainId);
            Models.JobServices js = new JobServices();
            js.JobMainId = (int)JobMainId;

            DateTime dtTmp = new DateTime(job.JobDate.Year, job.JobDate.Month, job.JobDate.Day, 8, 0, 0);
            js.DtStart = dtTmp;
            js.DtEnd = dtTmp.AddDays(job.NoOfDays - 1).AddHours(10);
            //js.Remarks = "10hrs use per day P300/hr in excess, Driver and Fuel Included";
            js.Remarks = "10hrs use per day P350/hr in excess, Driver Included. Fuel by Renter.";
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
            ViewBag.JobMainId = new SelectList(db.JobMains, "Id", "Description", job.Description);
            ViewBag.SupplierId = new SelectList(SuppliersActive, "Id", "Name");
            ViewBag.SupplierItemId = new SelectList(supItemsActive, "Id", "Description");
            ViewBag.ServicesId = new SelectList(db.Services.Where(s => s.Status == "1").ToList(), "Id", "Name");
            return View(js);
        }

        [HttpGet]
        public string GetVehicleOilRemarks(int jobmainId)
        {
            try {
                var vehicle = db.JobVehicles.Where(j => j.JobMainId == jobmainId).OrderByDescending(j => j.Id).FirstOrDefault();

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
                        MotorOil = " Motor Oil: 0 L, ";
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

                    return MotorOil;

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
        [Authorize]
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
                //jobServices.DtEnd = ((DateTime)jobServices.DtEnd).Add(new TimeSpan(23, 59, 59));
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

            return RedirectToAction("JobServices", "JobOrder", new { JobMainId = jobServices.JobMainId });
        }


        public bool ConfirmJobSvcDelete(int? id)
        {
            try
            {
                if (id == null)
                {
                    return false;
                }


                JobServices jobServices = db.JobServices.Find(id);

                if (jobServices == null)
                {
                    return false;
                }

                int jId = jobServices.JobMainId;

                //remove jobservice pickup on job service pickups
                JobServicePickup jobpickup = db.JobServicePickups.Where(j => j.JobServicesId == id).FirstOrDefault();

                if (jobpickup != null)
                {
                    db.JobServicePickups.Remove(jobpickup);
                    db.SaveChanges();
                }


                //remove jobservice items
                var jobitems = db.JobServiceItems.Where(i => i.JobServicesId == id).ToList();
                if (jobitems != null)
                {
                    db.JobServiceItems.RemoveRange(jobitems);
                    db.SaveChanges();
                }

                db.JobServices.Remove(jobServices);
                db.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public ActionResult notify() {
            DBClasses dbc = new DBClasses();
            dbc.addNotification("Job Order", "Test");
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

            ViewBag.Contact = db.JobContacts.Where(d => d.ContactType == "100").OrderBy(d=>d.ContactType).ToList();
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

        //[Authorize(Roles = "Admin,ServiceAdvisor")]
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

            if (JobMainId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
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
            if (User.IsInRole("Admin") || User.IsInRole("Accounting"))
            {
                isAllowedPayment = true;
            }


            //check previlages
            var isAdmin = User.IsInRole("Admin");
            var isServiceAdvisor = User.IsInRole("ServiceAdvisor");

            ViewBag.IsAdmin = isAllowedPayment;
            ViewBag.IsAllowedEdit = isAdmin || isServiceAdvisor ? true : false;
            ViewBag.isOwner = User.IsInRole("Owner");
            ViewBag.JobMainId = (int)JobMainId;
            ViewBag.JobOrder = Job;
            ViewBag.Company = jo.GetJobCompany((int)JobMainId);
            ViewBag.Providers = providers;
            ViewBag.JobStatus = Job.JobStatus.Status;
            ViewBag.JobStatusId = Job.JobStatusId;
            ViewBag.Itineraries = db.JobItineraries.Where(d => d.JobMainId == JobMainId).ToList();
            ViewBag.sortid = sortid;
            ViewBag.jobAction = action;
            ViewBag.user = HttpContext.User.Identity.Name;
            ViewBag.Vehicles = jvc.GetCustomerVehicleList((int)JobMainId);
            ViewBag.JobVehicle = jvc.GetJobVehicle((int)JobMainId);
            ViewBag.PaymentStatus = jo.GetJobPaymentStatus((int)JobMainId);
            ViewBag.SiteConfig = SITECONFIG;
            ViewBag.IsJobPosted = jo.GetJobPostedInReceivables((int)JobMainId);

            var veh = jvc.GetCustomerVehicleList((int)JobMainId);
            return View(jobServices.OrderBy(d => d.DtStart).ToList());

        }

        public ActionResult BookingRedirect(int id, string month, string day, string year, string rName) {
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

            if (id == 0)
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
            string billingLine1 = "";
            string billingLine2 = "";
            string billingLine3 = "";
            string billingLine4 = "";

            //check customer if assigned to a company
            if (jobMain.JobEntMains.Where(c => c.JobMainId == jobMain.Id).FirstOrDefault() != null)
            {
                var company = jobMain.JobEntMains.Where(c => c.JobMainId == jobMain.Id).FirstOrDefault().CustEntMain;
                custCompany = company.Name;

                if (company.CustEntAddresses != null )
                {
                    var billingdetails = company.CustEntAddresses.Where(c => c.isBilling).FirstOrDefault();
                    if (billingdetails != null)
                    {
                        billingLine1 = billingdetails.Line1;
                        billingLine2 = billingdetails.Line2;
                        billingLine3 = billingdetails.Line3;
                        billingLine4 = billingdetails.Line4;
                    }
                }
            }

            ViewBag.custCompany = custCompany;
            ViewBag.custCompanyAddress = billingLine1 ;
            ViewBag.custCompanyTel = billingLine2;
            ViewBag.custCompanyTIN = billingLine3;
            ViewBag.custCompanyStyle = billingLine4;

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
            var assign = jobMain.AssignedTo;
            if (encoder != null)
            {
                ViewBag.StaffName = getStaffName(assign ?? null);
                ViewBag.Sign = getStaffSign(assign ?? null);
            }
            else
            {
                ViewBag.StaffName = getStaffName(null);
                ViewBag.Sign = getStaffSign(null);
            }

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
            string billingLine1 = "";
            string billingLine2 = "";
            string billingLine3 = "";
            string billingLine4 = "";
            bool isBilling = false;

            //check customer if assigned to a company
            if (jobMain.JobEntMains.Where(c => c.JobMainId == jobMain.Id).FirstOrDefault() != null)
            {
                var company = jobMain.JobEntMains.Where(c => c.JobMainId == jobMain.Id).FirstOrDefault().CustEntMain;
                custCompany = company.Name;
                if (company.CustEntAddresses != null)
                {
                    var billingdetails = company.CustEntAddresses.Where(c => c.isBilling).FirstOrDefault();
                    if (billingdetails != null)
                    {
                        isBilling = true;
                        billingLine1 = billingdetails.Line1;
                        billingLine2 = billingdetails.Line2;
                        billingLine3 = billingdetails.Line3;
                        billingLine4 = billingdetails.Line4;
                    }
                }
            }

            ViewBag.IsBilling = isBilling;
            ViewBag.custCompany = custCompany;
            ViewBag.custCompanyAddress = billingLine1;
            ViewBag.custCompanyTel = billingLine2;
            ViewBag.custCompanyStyle =  billingLine3;
            ViewBag.custCompanyTIN = billingLine4;


            ViewBag.Services = db.JobServices.Include(j => j.JobServicePickups).Where(j => j.JobMainId == jobMain.Id).OrderBy(s => s.DtStart);
            ViewBag.Itinerary = db.JobItineraries.Include(j => j.Destination).Where(j => j.JobMainId == jobMain.Id);
            ViewBag.Payments = db.JobPayments.Where(j => j.JobMainId == jobMain.Id);
            ViewBag.jNotes = db.JobNotes.Where(d => d.JobMainId == jobMain.Id).OrderBy(s => s.Sort);

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
                sLine4 = "TIN: 414-880-772-001 (non-Vat) ; PhilGEPS No.: 241128";
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

            if (jobMain.Branch.Name == "RealBreeze - Cebu")
            {
                sCompany = "Real Breeze Travel & Tours - Cebu City";
                sLine1 = "Tel# (082) 333-5157; (+63) 916 755 8473; ";
                sLine2 = "Email: travel.realbreeze@gmail.com; Website: http://www.realbreezetravel.com/CEBU";
                sLine3 = " ";
                sLine4 = " ";
                sLogo = "RealBreezeLogo01.png";
                bank = db.Banks.Find(3);
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
            ViewBag.CarDesc = "";
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

            ViewBag.isPaymentValid = jobMain.JobDate.Date == today ? "True" : "False";


            //handle prepared by
            var encoder = db.JobTrails.Where(s => s.RefTable == "joborder" && s.RefId == jobMain.Id.ToString()).FirstOrDefault();
            var assign = jobMain.AssignedTo;
            if (encoder != null)
            {
                ViewBag.StaffName = getStaffName(assign ?? null);
                ViewBag.Sign = getStaffSign(assign ?? null);
            }
            else
            {
                ViewBag.StaffName = getStaffName(null);
                ViewBag.Sign = getStaffSign(null);
            }

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
            } else if (iType != null && (int)iType == 1)
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
                case "grace.realbreeze@gmail.com":
                    return "Grace-chell V. Capandac";
                case "jhudy.realbreeze@gmail.com":
                    return "Jhudy Claire D. Molles";
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
                case "grace.realbreeze@gmail.com":
                    return "/Images/Signature/GraceSign.jpg";
                case "jhudy.realbreeze@gmail.com":
                    return "/Images/Signature/JhudySign.jpg";
                case "assalvatierra@gmail.com":
                    return "/Images/Signature-1.png";
                default:
                    return "/Images/Signature-1.png";
            }
        }

        //Param: id = job service ID
        public ActionResult TextMessage(int? id)
        {
            string sData = "Booking Details";

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
                sData += "\nPickup Time:" + svcpu.JsTime;
                sData += "\nLocation:" + svcpu.JsLocation;

                sData += "\n\nGuest:" + svcpu.ClientName;
                sData += "\nContact:" + svcpu.ClientContact;

                sData += "\n  ";
                sData += "\nAssigned:  ";

                foreach (var svi in svc.JobServiceItems) {
                    sData += "\n" + svi.InvItem.Description + " (" + svi.InvItem.ItemCode + ") / " + svi.InvItem.ContactInfo;
                }


                sData += "\n  ";
                sData += "\nRate:P" + quote.ToString("##,###.00");
                sData += "\nParticulars:" + svc.Particulars;
                sData += "\n  " + svc.Remarks;
                if (svc.JobMain.NoOfPax != 0)
                    sData += "\nNo Pax:  " + svc.JobMain.NoOfPax;

                sData += "\n\nThank you for Trusting \n" + custName;
            }

            ViewBag.JobMainId = svc.JobMainId;
            ViewBag.StrData = sData;
            return View();
        }

        public ActionResult TextConfirmation(int? id)
        {
            string sData = "\n";
            decimal totalAmount = 0;
            //Models.JobServiceItem svcpu;
            Models.JobMain jobmain = db.JobMains.Find(id);
            var svc = db.JobServices.Where(j => j.JobMainId == id).ToList();
            string custName = jobmain.Branch.Name;
            int pickupCount = 0;

            sData += "Booking Details";

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
                sData += " ";

                foreach (var svi in svc)
                {

                    decimal quoted = svi.QuotedAmt != null ? (decimal)svi.QuotedAmt : 0;
                    sData += "\n\nDate:" + ((DateTime)svi.DtStart).ToString("MMM dd yyyy (ddd)") + " - " + ((DateTime)svi.DtEnd).ToString("MMM dd yyyy (ddd)");
                    sData += "\nDescription:" + svi.Particulars;
                    sData += "\nRate:P" + quoted.ToString("##,###.00");
                    //totalAmount += (decimal)svi.QuotedAmt;
                    totalAmount += quoted;

                    //check pickup details
                    if (svi.JobServicePickups.Count != 0)
                    {
                        foreach (var jobPickup in svi.JobServicePickups)
                        {
                            //Pickup Details
                            sData += "\n\nPickup Time: ";
                            sData += " " + jobPickup.JsTime;
                            sData += " " + jobPickup.JsDate.ToString("MMM dd yyyy");
                            sData += "\nLocation: " + jobPickup.JsLocation;

                            sData += "\n\nAssigned:";
                            sData += "\nVehicle:" + getUnitDetails(svi.Id);
                            sData += "\nDriver: " + jobPickup.ProviderName + " / " + jobPickup.ProviderContact;
                            pickupCount++;
                        }
                    }


                    sData += " ";
                }// end of job services

                if (pickupCount == 0) {
                    //Pickup Details
                    sData += "\n\nPickup Details: TBA";
                    sData += "\nDate: TBA";
                    sData += "\nTime: TBA";
                    sData += "\nLocation: TBA";
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

            ViewBag.JobMainId = id;
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
            //Models.JobServiceItem svcpu;
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
                sData += " ";

                foreach (var svi in svc)
                {

                    decimal quoted = svi.QuotedAmt != null ? (decimal)svi.QuotedAmt : 0;
                    sData += "\n\nDate:" + ((DateTime)svi.DtStart).ToString("MMM dd yyyy (ddd)") + " - " + ((DateTime)svi.DtEnd).ToString("MMM dd yyyy (ddd)");
                    sData += "\nDescription:" + svi.Particulars;
                    sData += "\nVehicle:" + svi.SupplierItem.Description;

                    totalAmount += quoted;

                    //check pickup details
                    if (svi.JobServicePickups.Count != 0)
                    {
                        foreach (var jobPickup in svi.JobServicePickups)
                        {
                            if (jobPickup != null) {
                                //Pickup Details
                                sData += "\n\nPickup Time: ";
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

                    sData += " ";
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
                sData += "\nCollectible:P" + dbc.getJobCollectible(id).ToString("##,###.00");
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
                if (svc.InvItem.ViewLabel == "Driver" || svc.InvItem.ViewLabel == "DRIVER")
                {
                    driverDetails = svc.InvItem.Description + " / " + svc.InvItem.ContactInfo;
                }
            }
            return driverDetails;
        }

        private string getUnitDetails(int svcId)
        {
            var driverDetails = "TBA";
            var jobsvc = db.JobServiceItems.Where(s => s.JobServicesId == svcId).ToList();

            foreach (var svc in jobsvc)
            {
                if (svc.InvItem.ViewLabel == "Unit" || svc.InvItem.ViewLabel == "UNIT")
                {
                    driverDetails = svc.InvItem.Description + " / " + svc.InvItem.ContactInfo;
                }
            }
            return driverDetails;
        }


        public ActionResult CloseJobStatus(int? id)
        {
            var Job = db.JobMains.Find(id);
            Job.JobStatusId = 4;
            db.Entry(Job).State = EntityState.Modified;
            db.SaveChanges();

            //job trail
            trail.recordTrail("JobOrder/JobServices", HttpContext.User.Identity.Name, "Job Status changed to CONFIRMED", id.ToString());

            //var postSaleRecord = CreateJobPostSalesRecord((int)id);
            return RedirectToAction("JobServices", "JobOrder", new { JobMainId = id });
        }

        public ActionResult CloseJobStatusFromList(int? id)
        {
            var Job = db.JobMains.Find(id);
            Job.JobStatusId = 4;
            db.Entry(Job).State = EntityState.Modified;
            db.SaveChanges();

            //job trail
            trail.recordTrail("JobOrder/JobServices", HttpContext.User.Identity.Name, "Job Status changed to CONFIRMED", id.ToString());

            //var postSaleRecord = CreateJobPostSalesRecord((int)id);

            return RedirectToAction("Index", "JobOrder", new { mainid = id });
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

        public ActionResult CancelJobStatus(int? id)
        {
            var Job = db.JobMains.Find(id);
            Job.JobStatusId = 5;
            db.Entry(Job).State = EntityState.Modified;
            db.SaveChanges();

            //job trail
            trail.recordTrail("JobOrder/JobServices", HttpContext.User.Identity.Name, "Job Status changed to CANCELLED", id.ToString());

            return RedirectToAction("JobServices", "JobOrder", new { JobMainId = id });
        }

        public bool AjaxCloseJobStatus(int? id)
        {
            try
            {
                if (id == null)
                {
                    return false;
                }

                var Job = db.JobMains.Find(id);
                Job.JobStatusId = 4;
                db.Entry(Job).State = EntityState.Modified;
                db.SaveChanges();

                //job trail
                trail.recordTrail("JobOrder/JobServices", HttpContext.User.Identity.Name, "Job Status changed to CLOSED", id.ToString());

                return true;
            }
            catch
            {
                return false;
            }

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

                return "OK";
                //return "Error";
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        [HttpPost]
        public string CancelJob(int id)
        {
            try
            {
                var Job = db.JobMains.Find(id);
                Job.JobStatusId = 5;
                db.Entry(Job).State = EntityState.Modified;
                db.SaveChanges();

                //job trail
                trail.recordTrail("JobOrder/JobServices", HttpContext.User.Identity.Name, "Job Status changed to CANCELLED", id.ToString());

                return "OK";
                //return "Error";
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        //GET: \JobOrder\GetJobRcvDetails
        //Get the JobDetails by jobId
        //Used in JobPostReceivables.js
        [HttpGet]
        public JsonResult GetJobRcvDetails(int id)
        {
            try
            {
                var jobDetails = db.JobMains.Find(id);

                return Json(new
                {
                    jobDetails.Id,
                    jobDetails.Description,
                    jobDetails.JobDate,
                    SvcStart = jo.GetMinMaxServiceDate(id, "min"),
                    SvcEnd = jo.GetMinMaxServiceDate(id, "max"),
                    Amount = jo.GetTotalJobAmount(id),
                    Customer = jobDetails.Customer.Name,
                    Contact = jobDetails.CustContactNumber,
                    Email = jobDetails.CustContactEmail,
                    Company = jo.GetJobCompany(id),

                }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(null , JsonRequestBehavior.AllowGet);
            }
        }

        public bool PostJobReceivables(int id)
        {
            try
            {
                JobPost jobPost = new JobPost();
                jobPost.DtPost = dt.GetCurrentDateTime();

                return true;
            }
            catch
            {
                return false;
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

        #endregion

        #region Daily Status Updates
        public ActionResult DailyUpdateList() {


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
            
            mailResult = mailResult == "success" ? "Email is sent successfully." : "Our System cannot send the email to the client.";
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
            mailResult = mailResult == "success" ? "Email is sent successfully." : "Our System cannot send the email to the client. Please try again later. " ;
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

        //Get the company name of the job in triplogs
        public string GetJobCompanyName(int id)
        {
             return jo.GetJobCompany(id);
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

        [HttpGet]
        public JsonResult GetVehiclePrevOdo(int? vehicleId)
        {
            if (vehicleId == null || vehicleId == 0)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var prevOdo = jvc.GetVehiclePrevOdo((int)vehicleId);
                return Json(prevOdo, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region Job Post Sales
        [HttpPost]
        public bool CreateJobPostSalesRecord(int jobMainId)
        {
            var jobServiceList = db.JobServices.Where(j=>j.JobMainId == jobMainId).ToList();

            if (jobServiceList != null)
            {
                var AddResult = false;
                foreach (var service in jobServiceList)
                {
                    if (!IsJobPostSalesExist(service.Id))
                    {
                       var res = AddJobPostSales(service.Id);
                        if (res)
                        {
                            AddResult = true;
                        }
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
                var PostSalesInterval = jobServices.SupplierItem.Interval == null ? 0 : (int)jobServices.SupplierItem.Interval;
                if (PostSalesInterval > 0)
                {

                    var PostSalesDate = ((DateTime)jobServices.DtStart).AddDays(PostSalesInterval);

                    JobPostSale postSale = new JobPostSale()
                    {
                        DtPost = PostSalesDate,
                        DoneBy = "NA",
                        JobServicesId = jobServiceId,
                        Remarks = "",
                        JobPostSalesStatusId = 1,
                        DtDone = null
                    };

                    db.JobPostSales.Add(postSale);
                    db.SaveChanges();

                    return true;
                }

                return false;
            }
            catch
            {
                return false;
            }
        }

        public bool IsJobPostSalesExist(int jobServiceId)
        {
            var jsPostSalesQuery = db.JobPostSales.Where(j => j.JobServicesId == jobServiceId);

            if (jsPostSalesQuery.FirstOrDefault() != null)
            {
                return true;
            }

            return false;
        }

        #endregion

        #region Jobs Monitor

        public ActionResult JobsMonitor()
        {
            List<AutoCareMonitorJobs> jobList = new List<AutoCareMonitorJobs>();

            DateTime today = dt.GetCurrentDateTime();

            //get jobs from today
            var jobsvc = db.JobServices.Where(j => DbFunctions.TruncateTime(j.DtStart) <= today.Date && DbFunctions.TruncateTime(j.DtEnd) >= today.Date && j.JobMain.JobStatusId < 4 )
                .OrderBy(s => s.DtStart).ToList();

            var bayCategoryIds = db.InvItemCategories.Where(c => c.InvItemCatId == 1).Select(c => c.InvItemId).ToList();

            //get inventory items assigned to category 1
            foreach (var job in jobsvc.GroupBy(s=>s.JobMainId))
            {
                var jobMainDetails = db.JobMains.Find(job.Key);
                AutoCareMonitorJobs _tempjobMonitor = new AutoCareMonitorJobs();
                _tempjobMonitor.Id = jobMainDetails.Id;
                _tempjobMonitor.Customer = jobMainDetails.Customer.Name;
                _tempjobMonitor.Company = jo.GetJobCompany(jobMainDetails.Id);
                _tempjobMonitor.Vehicle = jo.GetJobVehicle(jobMainDetails.Id);
                _tempjobMonitor.Jobdate = jo.TempJobDate(jobMainDetails.Id);
                _tempjobMonitor.Services = new List<string>();
                _tempjobMonitor.AssignedItems = new List<string>();
                _tempjobMonitor.AssignedBay = "No Assigned Bay";

                foreach (var svc in job)
                {
                    var svcItems = svc.JobServiceItems.ToList();
                    var service = svc.Particulars + " ( " + svc.Service.Name + " ) ";

                    _tempjobMonitor.Services.Add(service);
                    _tempjobMonitor.StartDate = (DateTime)svc.DtStart;
                    _tempjobMonitor.EndDate = (DateTime)svc.DtEnd;

                    //assign expted time done
                    var timeDiff = _tempjobMonitor.EndDate - today;
                    if (timeDiff.TotalMinutes <= 0)
                    {
                        timeDiff = new TimeSpan(0);
                    }
                    _tempjobMonitor.ExpectedTimeDone = timeDiff;

                    foreach (var item in svcItems)
                    {

                        //find bay category of item 
                        if (bayCategoryIds.Contains(item.InvItemId))
                        {
                            //var bayDateTime = new DateTime();
                            //assign job to bay
                            _tempjobMonitor.AssignedBay = item.InvItem.Description;
                            _tempjobMonitor.OrderNo = item.InvItem.OrderNo ?? 999;

                        }
                        else
                        {
                            _tempjobMonitor.AssignedItems.Add(item.InvItem.Description);
                        }

                    }


                    //find last service action of svc
                    var svcActions = db.JobActions.Where(j => j.JobServicesId == svc.Id).ToList();
                    if (svcActions.Count() > 0)
                    {
                        foreach (var action in svcActions)
                        {
                            //assign new action
                            if (_tempjobMonitor.JobActionStatusId == 0)
                            {
                                _tempjobMonitor.JobActionStatus = action.SrvActionItem.Desc;
                                _tempjobMonitor.JobActionStatusId = action.SrvActionItem.SortNo;
                            }
                            else
                            {
                                //if action is greater than the current action order, overwrite 
                                if (_tempjobMonitor.JobActionStatusId < action.SrvActionItem.SortNo)
                                {
                                    _tempjobMonitor.JobActionStatus = action.SrvActionItem.Desc;
                                    _tempjobMonitor.JobActionStatusId = action.SrvActionItem.SortNo;
                                }
                            }

                        }
                    }
                    else
                    {
                        //update latest service action
                        _tempjobMonitor.JobActionStatus = "Pending";
                    }

                }

                jobList.Add(_tempjobMonitor);
            }

            //get assigned jobs for each inventory item
            return View(jobList.OrderBy(j=> j.OrderNo));
        }
        #endregion

        [HttpGet]
        public JsonResult CheckAdminPermission(string pass)
        {
            var DBAdminPass = sal.getSysSetting("ADMINPASS");
            //var adminPass = "Admin123!";

            if (DBAdminPass.Equals(pass))
            {
                return Json(true,JsonRequestBehavior.AllowGet);
            }

            return Json(false, JsonRequestBehavior.AllowGet);
        }

        //get utc date time today (singapore standard time) gmt + 8
        private DateTime getDateTimeToday()
        {
            DateTime today = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Singapore Standard Time"));

            return today;
        }

        public ActionResult ErrorPage()
        {
            return View();
        }
    }
}