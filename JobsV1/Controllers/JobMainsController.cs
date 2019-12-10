using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using JobsV1.Models;
using System.Data.Entity.Core.Objects;

namespace JobsV1.Controllers
{
    public class JobMainsController : Controller
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

        // GET: JobMains
        public ActionResult Index(int? Param1 = 1, int? Param2 = 0)
        {
            string sParam = "";

            IQueryable<Models.JobMain> jobMains = db.JobMains.Include(j => j.Customer).Include(j => j.Branch).Include(j => j.JobStatus).Include(j => j.JobThru).OrderBy(d=>d.JobDate);
            // Param1 : Status Option
            //        0: ALL
            //        1: confirmed
            //        2: Closed
            if (Param1 == 0)
            {
                sParam += "[ Status : All ] ";
            }
            if (Param1 == 1)
            {
                jobMains = (IQueryable<Models.JobMain>)jobMains.Where(d => d.JobStatusId == JOBRESERVATION || d.JobStatusId == JOBCONFIRMED);
                sParam += "[ Status : Confirmed ] ";  
            }
            if (Param1 == 2)
            {
                jobMains = (IQueryable<Models.JobMain>)jobMains.Where(d => d.JobStatusId == JOBCLOSED);
                sParam += "[ Status : Closed ] ";
            }

            // Param2 : Date option
            //        0: Today 
            //        1: 7 days
            //        2: next 7 days

            DateTime dt1 = System.DateTime.Now.AddDays(-1);
            DateTime dt2 = System.DateTime.Now.AddDays(1);

            if (Param2 == 0)
            {
                dt1 = new DateTime(System.DateTime.Now.Year, System.DateTime.Now.Month, System.DateTime.Now.Day, 0,0,1); //.AddDays(-1);
                dt2 = new DateTime(System.DateTime.Now.Year, System.DateTime.Now.Month, System.DateTime.Now.Day, 23, 59, 59); //.AddDays(-1);
                sParam += "[ Date : Current ] ";
            }

            if (Param2 == 1)
            {
                dt1 = System.DateTime.Now.AddDays(-30);
                dt2 = System.DateTime.Now;
                sParam += "[ Date : Previous Month till date ] ";
            }
            if (Param2 == 2)
            {
                dt1 = System.DateTime.Now;
                dt2 = System.DateTime.Now.AddDays(90);
                sParam += "[ Date : current and incoming ] ";
            }
                
            List<int> svcs = db.JobServices.Where(d =>
                ((DateTime)d.DtEnd).CompareTo(dt1) >= 0 && ((DateTime)d.DtStart).CompareTo(dt2) <= 0).Select(s => s.JobMainId).ToList();
                
            jobMains = (IQueryable<Models.JobMain>)jobMains.Where( d => 
            (  d.JobDate.CompareTo(dt1) >= 0 && d.JobDate.CompareTo(dt2) <= 0  )
            || ( svcs.Contains( d.Id ) )  
            );

            ViewBag.ListParam = sParam;
            return View(jobMains.ToList());
        }

        public ActionResult ActiveJobs(int? FilterId, string service)
        {
            IQueryable<Models.JobMain> jobMains = db.JobMains.Include(j => j.Customer).Include(j => j.Branch).Include(j => j.JobStatus).Include(j => j.JobThru).OrderBy(d => d.JobDate);
            jobMains = (IQueryable<Models.JobMain>)jobMains.Where(d => d.JobStatusId == JOBRESERVATION || d.JobStatusId == JOBCONFIRMED || d.JobStatusId == JOBINQUIRY);

            var p = jobMains.Select(s => s.Id);

            DateTime today = GetCurrentTime();

            List<JobServices> data = db.JobServices.Where(w => p.Contains(w.JobMainId)).ToList().OrderBy(s => s.DtStart).ToList();

            DateTime tomorrow = today.AddDays(1);
            DateTime after2Days = today.AddDays(2);
            switch (FilterId)
            {
                case 1:
                    data = data.OrderBy(s => s.DtStart).ToList();
                    break;
                case 2:
                    //get jobs from today
                    data = data.Where(w => DateTime.Compare(w.DtStart.Value.Date, today.Date) == 0).OrderBy(s => s.DtStart).ToList();
                    break;
                case 3:
                    //get jobs from tomorrow
                    data = data.Where(w => DateTime.Compare(w.DtStart.Value.Date, tomorrow.Date) == 0).OrderBy(s => s.DtStart).ToList();
                    break;
                case 4:
                    //get jobs from today to 2 days after
                    data = data
                        .Where(w => DateTime.Compare(w.DtStart.Value.Date, after2Days.Date) <= 0 && DateTime.Compare(w.DtStart.Value.Date, today.Date) >= 0).OrderBy(s => s.DtStart)
                        .ToList();
                    break;
                default:
                    //get jobs from today to 2 days after
                    data = data
                        .Where(w => DateTime.Compare(w.DtStart.Value.Date, after2Days.Date) <= 0 && DateTime.Compare(w.DtStart.Value.Date, today.Date) >= 0).OrderBy(s => s.DtStart)
                        .ToList();
                    break;
            }

            if (service != null)
            {
                if (service != "all")
                {
                    data = data.Where(s => s.Service.Name.ToLower().Contains(service.ToLower())).ToList();
                }
            }
            
            ////convert to cJobActives
            //List<cActiveJobs> cdata = new List<cActiveJobs>();

            //foreach (var item in data)
            //{
            //    var pickup = item.JobServicePickups.Where(s => s.JobServicesId == item.Id).FirstOrDefault() ;
            //    TimeSpan timedefault = TimeSpan.Parse("00:00");
            //    DateTime tempDate = (DateTime)item.DtStart;
            //    DateTime sortdate = today;
            //    if (pickup != null)
            //    {
            //        string timeValue = pickup.JsTime;
            //        TimeSpan _time = TimeSpan.Parse(timeValue.Replace(" AM", "").Replace(" PM", ""));

            //        var time = _time;
            //        sortdate = new DateTime(tempDate.Year, tempDate.Month, tempDate.Day, time.Hours, time.Minutes, time.Seconds);
            //    }

            //    var jobs = new cActiveJobs();
               
            //        jobs.Id = item.Id;
            //        jobs.JobMainId = item.JobMainId;
            //        jobs.JobDesc = item.JobMain.Description;
            //        jobs.Particulars = item.Particulars;
            //        jobs.Service = item.Service.Name;
            //        jobs.Customer = item.JobMain.Customer.Name;
            //        jobs.Item = item.SupplierItem.Description;
            //        jobs.DtStart = ((DateTime)item.DtStart).ToString("MMM dd yyyy (ddd)");
            //        jobs.DtEnd = ((DateTime)item.DtEnd).ToString("MMM dd yyyy (ddd)");
            //        jobs.JsDate = pickup != null ? pickup.JsDate.ToString("MMM dd yyyy (ddd)") : "";
            //        jobs.JsTime = pickup != null ? pickup.JsTime : ""; // TimeSpan.Parse(pickup.JsTime.Substring(1,4))
            //        jobs.JsLocation = pickup != null ? pickup.JsLocation : "N/A";
            //        jobs.SORTDATE = sortdate;
            //        jobs.Assigned = item.JobServiceItems;

            //    cdata.Add(jobs);
            //}
            
            ViewBag.Current = this.GetCurrentTime().ToString("MMM dd yyyy (ddd)");
            ViewBag.today = GetCurrentTime();
            
            return View(data);
        }

        public ActionResult ActiveJobs2(int? FilterId, string service)
        {

            int filter = 0;
            if (FilterId != null)
            {
                filter = (int)FilterId; //default
            }

            List<cActiveJobs> data = dbc.getActiveJobs(filter);

            ViewBag.Current = this.GetCurrentTime().ToString("MMM-dd-yyyy (ddd)");
            ViewBag.today = GetCurrentTime();

            return View(data.OrderBy(s=>s.SORTDATE));
        }

        protected DateTime GetCurrentTime()
        {
            DateTime _localTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Singapore Standard Time"));
            _localTime = _localTime.Date;

            return _localTime;
        }
        
        // GET: JobMains/Details/5
        public ActionResult Details(int? id, int? iType)
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

            ViewBag.Services = db.JobServices.Include(j => j.JobServicePickups).Where(j => j.JobMainId == jobMain.Id).OrderBy(s=>s.DtStart);
            ViewBag.Itinerary = db.JobItineraries.Include(j => j.Destination).Where(j => j.JobMainId == jobMain.Id);
            ViewBag.Payments = db.JobPayments.Where(j => j.JobMainId == jobMain.Id);
            ViewBag.jNotes = db.JobNotes.Where(d => d.JobMainId == jobMain.Id).OrderBy(s=>s.Sort);

            //Default form
            string sCompany = "AJ88 Car Rental Services";
            string sLine1 = "2nd Floor J. Sulit Bldg. Mac Arthur Highway, Matina Davao City ";
            string sLine2 = "Tel# (+63)822971831; (+63)9167558473; (+63)9330895358 ";
            string sLine3 = "Email: ajdavao88@gmail.com; Website: http://www.AJDavaoCarRental.com/";
            string sLine4 = "TIN: 414-880-772-001 (non-Vat)";
            string sLogo = "LOGO_AJRENTACAR.jpg";

            if (jobMain.Branch.Name == "RealBreeze")
            {
                sCompany = "Real Breeze Travel & Tours - Davao City";
                sLine1 = "2nd Floor J. Sulit Bldg. Mac Arthur Highway, Matina Davao City ";
                sLine2 = "Tel# (+63)822971831; (+63)916-755-8473; (+63)933-089-5358 ";
                sLine3 = "Email: RealBreezeDavao@gmail.com; Website: http://www.realbreezedavaotours.com//";
                sLine4 = "TIN: 414-880-772-000 (non-Vat)";
                sLogo = "RealBreezeLogo01.png";
            }
            if (jobMain.Branch.Name == "AJ88")
            {
                sCompany = "AJ88 Car Rental Services";
                sLine1 = "2nd Floor J. Sulit Bldg. Mac Arthur Highway, Matina Davao City ";
                sLine2 = "Tel# (+63)822971831; (+63)9167558473; (+63)9330895358 ";
                sLine3 = "Email: ajdavao88@gmail.com; Website: http://www.AJDavaoCarRental.com/";
                sLine4 = "TIN: 414-880-772-001 (non-Vat)";
                sLogo = "LOGO_AJRENTACAR.jpg";
            }

            ViewBag.sCompany = sCompany;
            ViewBag.sLine1 = sLine1;
            ViewBag.sLine2 = sLine2;
            ViewBag.sLine3 = sLine3;
            ViewBag.sLine4 = sLine4;
            ViewBag.sLogo = sLogo;


            if (jobMain.JobStatusId==1) //quotation
                return View("Details_Quote", jobMain);

            if( iType!= null && (int)iType == 1) //Invoice
                return View("Details_Invoice", jobMain);

            return View(jobMain);
        }

        /*
        public ActionResult SubContractors(int? id)
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

            var services = db.JobServices.Include(j => j.JobServicePickups).Where(j => j.JobMainId == jobMain.Id);

            System.Collections.ArrayList providers = new System.Collections.ArrayList();
            foreach(var item in services)
            {
                if(item.JobServicePickups != null)
                {
                    string sTmp = item.JobServicePickups.FirstOrDefault().ProviderName;
                    if( !providers.Contains(sTmp) )
                    {
                        providers.Add(sTmp);
                    }
                }
            }

            ViewBag.Providers = providers;
            return View();
        } */

        public ActionResult SubDetails(int? id, string sProvider)
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

            ViewBag.Services = db.JobServices.Include(j => j.JobServicePickups).Where(j => j.JobMainId == jobMain.Id).Where( j => j.JobServicePickups.FirstOrDefault().ProviderName == sProvider).OrderBy(s=>s.DtStart);
            ViewBag.Itinerary = db.JobItineraries.Include(j => j.Destination).Where(j => j.JobMainId == jobMain.Id);
            ViewBag.Payments = db.JobPayments.Where(j => j.JobMainId == jobMain.Id);
            ViewBag.Notes = db.JobNotes.Where(j => j.JobMainId == jobMain.Id);

            //Default form
            string sCompany = "AJ88 Car Rental Services";
            string sLine1 = "Door 1, RedDoorz Travelers Inn Bldg., Matina Pangi Rd., Matina Crossing, Davao City, 8000";
            string sLine2 = "Tel# (082) 333 5157; (+63)916-755-8473; (+63)933-089-5358";
            string sLine3 = "Email: ajdavao88@gmail.com; Website: http://www.AJDavaoCarRental.com/";
            string sLogo = "LOGO_AJRENTACAR.jpg";

            if (jobMain.Branch.Name == "RealBreeze")
            {
                sCompany = "Real Breeze Travel & Tours - Davao City";
                sLine1 = "Door 1, RedDoorz Travelers Inn Bldg., Matina Pangi Rd., Matina Crossing, Davao City, 8000";
                sLine2 = "Tel# (082) 333 5157; (+63)916-755-8473; (+63)933-089-5358";
                sLine3 = "Email: RealBreezeDavao@gmail.com; Website: http://www.realbreezedavaotours.com//";
                sLogo = "RealBreezeLogo01.png";
            }
            if (jobMain.Branch.Name == "AJ88")
            {
                sCompany = "AJ88 Car Rental Services";
                sLine1 = "Door 1, RedDoorz Travelers Inn Bldg., Matina Pangi Rd., Matina Crossing, Davao City, 8000";
                sLine2 = "Tel# (082) 333 5157; (+63)916-755-8473; (+63)933-089-5358";
                sLine3 = "Email: ajdavao88@gmail.com; Website: http://www.AJDavaoCarRental.com/";
                sLogo = "LOGO_AJRENTACAR.jpg";
            }

            ViewBag.sCompany = sCompany;
            ViewBag.sLine1 = sLine1;
            ViewBag.sLine2 = sLine2;
            ViewBag.sLine3 = sLine3;
            ViewBag.sLogo  = sLogo;

            //handle prepared by
            var encoder = db.JobTrails.Where(s => s.RefTable == "joborder" && s.RefId == jobMain.Id.ToString()).FirstOrDefault();
            ViewBag.StaffName = getStaffName(encoder.user);
            ViewBag.Sign = getStaffSign(encoder.user);
            
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
                case "jecca.realbreeze@gmail.com":
                    return "Jecca Bilason";
                case "assalvatierra@gmail.com":
                    return "Elvie S. Salvatierra";
                default:
                    return "Elvie S. Salvatierra";
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
                case "jecca.realbreeze@gmail.com":
                    return "/Images/Signature/Jecca.Sign.jpg";
                case "assalvatierra@gmail.com":
                    return "/Images/Signature-1.png";
                default:
                    return "/Images/Signature-1.png";
            }
        }

        // GET: JobMains/Create
        public ActionResult Create(int? id)
        {

            DateTime today = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Singapore Standard Time"));

            JobMain job = new JobMain();
            job.JobDate = today;
            job.NoOfDays = 1;
            job.NoOfPax = 1;
            job.JobRemarks = "CASH BASIS";
            var customerlist = new SelectList(db.Customers.Where(d => d.Status == "ACT"), "Id", "Name", id != null? id : NewCustSysId);
          
            ViewBag.CustomerId = customerlist;
            ViewBag.BranchId = new SelectList(db.Branches, "Id", "Name");
            ViewBag.JobStatusId = new SelectList(db.JobStatus, "Id", "Status");
            ViewBag.JobThruId = new SelectList(db.JobThrus, "Id", "Desc");

            return View(job);
        }
        
        // GET: JobMains/Create
        public ActionResult Create2(int? custid)
        {
            JobMain job = new JobMain();

            DateTime today = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Singapore Standard Time"));
            today = today.Date;

            job.JobDate = today;
            job.NoOfDays = 1;
            job.NoOfPax = 1;

            ViewBag.CustomerId = new SelectList(db.Customers.Where(d => d.Status == "ACT"), "Id", "Name", custid);
            ViewBag.BranchId = new SelectList(db.Branches, "Id", "Name");
            ViewBag.JobStatusId = new SelectList(db.JobStatus, "Id", "Status");
            ViewBag.JobThruId = new SelectList(db.JobThrus, "Id", "Desc");

            return View(job);
        }

        // POST: JobMains/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,JobDate,CustomerId,Description,NoOfPax,NoOfDays,AgreedAmt,JobRemarks,JobStatusId,StatusRemarks,BranchId,JobThruId,CustContactEmail,CustContactNumber")] JobMain jobMain)
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

                dbc.addEncoderRecord("joborder", jobMain.Id.ToString(), HttpContext.User.Identity.Name, "Create New Job");


                if (jobMain.CustomerId == NewCustSysId)
                    return RedirectToAction("CreateCustomer", new { jobid = jobMain.Id });
                else
                    return RedirectToAction("Services", "JobServices", new { id = jobMain.Id });
                //return RedirectToAction("JobTable", new { span = 30 });
                //return RedirectToAction("Index");
            }

            ViewBag.CustomerId = new SelectList(db.Customers.Where(d => d.Status == "ACT"), "Id", "Name", jobMain.CustomerId);
            ViewBag.BranchId = new SelectList(db.Branches, "Id", "Name", jobMain.BranchId);
            ViewBag.JobStatusId = new SelectList(db.JobStatus, "Id", "Status", jobMain.JobStatusId);
            ViewBag.JobThruId = new SelectList(db.JobThrus, "Id", "Desc", jobMain.JobThruId);
            return View(jobMain);
        }
        
        // GET: JobMains/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobMain jobMain = db.JobMains.Find(id);
            if (jobMain == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustomerId = new SelectList(db.Customers.Where(d=>d.Status == "ACT") , "Id", "Name", jobMain.CustomerId);
            ViewBag.BranchId = new SelectList(db.Branches, "Id", "Name", jobMain.BranchId);
            ViewBag.JobStatusId = new SelectList(db.JobStatus, "Id", "Status", jobMain.JobStatusId);
            ViewBag.JobThruId = new SelectList(db.JobThrus, "Id", "Desc", jobMain.JobThruId);
            
            TempData["UrlSource"] = Request.UrlReferrer.ToString();
            return View(jobMain);
        }

        // POST: JobMains/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,JobDate,CustomerId,Description,NoOfPax,NoOfDays,AgreedAmt,JobRemarks,JobStatusId,StatusRemarks,BranchId,JobThruId,CustContactEmail,CustContactNumber")] JobMain jobMain)
        {
            if (ModelState.IsValid)
            {
                if (jobMain.CustContactEmail == null && jobMain.CustContactNumber == null)
                {
                    var cust = db.Customers.Find(jobMain.CustomerId);
                    jobMain.CustContactEmail = cust.Email;
                    jobMain.CustContactNumber = cust.Contact1;
                }

                db.Entry(jobMain).State = EntityState.Modified;
                db.SaveChanges();

                //if (jobMain.Customer.Name == "<< New Customer >>")
                if (jobMain.CustomerId == NewCustSysId)
                     return RedirectToAction("CreateCustomer", new { jobid = jobMain.Id });
                else
                return Redirect((string)TempData["UrlSource"]);
                //return RedirectToAction("Services", "JobServices", new { id = jobMain.Id });
                //return RedirectToAction("Index");

            }
            ViewBag.CustomerId = new SelectList(db.Customers.Where(d => d.Status == "ACT"), "Id", "Name", jobMain.CustomerId);
            ViewBag.BranchId = new SelectList(db.Branches, "Id", "Name", jobMain.BranchId);
            ViewBag.JobStatusId = new SelectList(db.JobStatus, "Id", "Status", jobMain.JobStatusId);
            ViewBag.JobThruId = new SelectList(db.JobThrus, "Id", "Desc", jobMain.JobThruId);
            return View(jobMain);
        }

        // GET: JobMains/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobMain jobMain = db.JobMains.Find(id);
            if (jobMain == null)
            {
                return HttpNotFound();
            }
            return View(jobMain);
        }

        // POST: JobMains/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            JobMain jobMain = db.JobMains.Find(id);
            db.JobMains.Remove(jobMain);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult CreateCustomer(int? jobid)
        {
            JobMain job = db.JobMains.Find(jobid);
            ViewBag.JobOrder = job;
            Session["CurrentJobId"] = job.Id;
            return View();
        }

        [HttpPost]
        public ActionResult CreateCustomer([Bind(Include = "Id,Name,Email,Contact1,Contact2,Remarks")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                customer.Status = "ACT";
                db.Customers.Add(customer);
                db.SaveChanges();

                int currentjobid = (int)Session["CurrentJobId"];
                JobMain job = db.JobMains.Find(currentjobid);
                job.CustomerId = customer.Id;

                if (job.CustContactEmail == null && job.CustContactNumber == null)
                {
                    job.CustContactEmail = job.Customer.Email;
                    job.CustContactNumber = job.Customer.Contact1;
                }

                db.Entry(job).State = EntityState.Modified;
                db.SaveChanges();

                //return RedirectToAction("Index");
                return RedirectToAction("JobServices", "JobOrder", new { JobMainId = currentjobid });
            }

            return View(customer);
        }

        public ActionResult ConfirmJob(int? id)
        {
            JobMain job = db.JobMains.Find(id);
            job.JobStatusId = JOBCONFIRMED;
            db.Entry(job).State = EntityState.Modified;
            db.SaveChanges();

            return Redirect( Request.UrlReferrer.ToString() );

            //return RedirectToAction("JobTable", new { span = 30 });

        }

        public ActionResult CloseJobActive(int? id)
        {
            JobMain job = db.JobMains.Find(id);
            job.JobStatusId = JOBCLOSED;
            db.Entry(job).State = EntityState.Modified;
            db.SaveChanges();

            return Redirect(Request.UrlReferrer.ToString());
        }


        public string CloseOldJob()
        {
            
            //var job = db.JobMains.Where(s=> DateTime.Compare(s.JobDate, DateTime.Now ) < 60).ToList();

            //foreach (var item in job)
            //{
            //    item.JobStatusId = JOBCLOSED;
            //    db.Entry(item).State = EntityState.Modified;
            //    db.SaveChanges();
            //}

            return "300";
        }

        public ActionResult CloseJob(int? id)
        {
            JobMain job = db.JobMains.Find(id);
            job.JobStatusId = JOBCLOSED;

            TempData["UrlSource"] = Request.UrlReferrer.ToString();

            return View(job);
        }

        [HttpPost, ActionName("CloseJob")]
        public ActionResult ConfirmCloseJob([Bind(Include = "Id,JobDate,CustomerId,Description,NoOfPax,NoOfDays,AgreedAmt,JobRemarks,JobStatusId,StatusRemarks,BranchId,JobThruId")] JobMain jobMain)
        {
            //JobMain job = db.JobMains.Find(id);
            jobMain.JobStatusId = JOBCLOSED;
            db.Entry(jobMain).State = EntityState.Modified;
            db.SaveChanges();

            return Redirect((string)TempData["UrlSource"]);
            //return RedirectToAction("JobTable", new { span = 30 });
        }

        public ActionResult JobTable(int? span = 30) //version: 2018
        {
            // System.DateTime dtNow = this.GetCurrentTime();
            DateTime today = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Singapore Standard Time"));
            today = today.Date;

            System.DateTime dtNow = today;
            System.DateTime dtStart = new DateTime(dtNow.Year, dtNow.Month, dtNow.Day, 0, 0, 0);
            System.DateTime dtUntil = System.DateTime.Now.AddDays((double)span);
            

            dtUntil = new DateTime(dtUntil.Year, dtUntil.Month, dtUntil.Day, 23, 59, 59);
            //Column Date Labels
            System.Collections.ArrayList ColLabels = new System.Collections.ArrayList();
            for (DateTime dtItem = dtStart; dtItem.CompareTo(dtUntil) < 0; dtItem = dtItem.AddDays(1))
            {
                ColLabels.Add(dtItem.ToString("dd") + "-" + dtItem.DayOfWeek.ToString());
            }

            System.Collections.ArrayList CustData = new System.Collections.ArrayList();

            var jobMains2 = db.JobMains.Include(j => j.Customer).Include(j => j.Branch).Include(j => j.JobStatus).Include(j => j.JobThru).OrderBy(d => d.JobDate);

            var jobMains = jobMains2.ToList().Where(
                d => (d.JobStatusId == JOBRESERVATION || d.JobStatusId == JOBCONFIRMED)
              //                || (d.JobStatusId == JOBCLOSED && d.JobDate.CompareTo(System.DateTime.Now.AddDays(1)) >= 0)
              );

            System.Collections.ArrayList data = new System.Collections.ArrayList();
            foreach (var item in jobMains)
            {
                JobTableData cust = new JobTableData();
                cust.tblValue = new List<JobTableValue>();

                cust.iCustId = item.CustomerId;
                cust.iJobId = item.Id;

                List<Models.JobServices> svc = db.JobServices.Include(j => j.JobServicePickups).Where(d => d.JobMainId == item.Id).ToList();
               
                for (DateTime dtItem = dtStart; dtItem.CompareTo(dtUntil) < 0; dtItem = dtItem.AddDays(1))
                {
                    foreach (var svcitem in svc)
                    {
                        int istart = dtItem.CompareTo((DateTime)svcitem.DtStart);
                        int iend = dtItem.CompareTo((DateTime)svcitem.DtEnd);

                        //update jobdate from service dates

                        item.JobDate = (DateTime)svcitem.DtEnd;

                        //

                        string sLabel = dtItem.ToString("dd") + "-" + dtItem.DayOfWeek.ToString();
                        if (dtItem.CompareTo((DateTime)svcitem.DtStart) >= 0 && dtItem.CompareTo((DateTime)svcitem.DtEnd) <= 0)
                        {
                            // get Inventory items - Internal
                            var invItems = db.JobServiceItems.Where(d => d.JobServicesId == svcitem.Id);
                            foreach( var invitemtmp in invItems)
                            {
                                string itemiconpath = "*";
                                var itemcat = invitemtmp.InvItem.InvItemCategories.FirstOrDefault();
                                if (itemcat != null)
                                    itemiconpath = itemcat.InvItemCat.ImgPath;


                                JobTableValue jtvTmp = new JobTableValue
                                {
                                    DtDate = dtItem,
                                    Book = 1,
                                    supplier = "Internal",
                                    item = invitemtmp.InvItem.ItemCode + " " + invitemtmp.InvItem.Description,
                                    Incharge = "",
                                    label = sLabel,
                                    itemicon = itemiconpath
                                };

                                cust.tblValue.Add(jtvTmp);
                            }

                            // get Supplier Items - Po
                            var suppItems = db.SupplierPoDtls.Where(d=>d.JobServicesId==svcitem.Id);
                            foreach ( var supPoDtl in suppItems )
                            {
                                foreach ( var supItem in supPoDtl.SupplierPoItems)
                                {
                                    string itemiconpath = "*";
                                    var itemcat = supItem.InvItem.InvItemCategories.FirstOrDefault();
                                    if (itemcat != null)
                                        itemiconpath = itemcat.InvItemCat.ImgPath;


                                    JobTableValue jtvTmp = new JobTableValue
                                    {
                                        DtDate = dtItem,
                                        Book = 1,
                                        supplier = supPoDtl.SupplierPoHdr.Supplier.Name,
                                        item = supItem.InvItem.ItemCode + " " + supItem.InvItem.Description,
                                        Incharge = "",
                                        label = sLabel,
                                        itemicon = itemiconpath
                                    };

                                    cust.tblValue.Add(jtvTmp);
                                }
                            }

                        }
                        else
                        {
                            cust.tblValue.Add(new JobTableValue { DtDate = dtItem, Book = 0, supplier = "", item = "", Incharge = "", label = sLabel });

                        }//end of if dtItem.Compare

                    } //end of foreach ( var svcitem...

                }// end of for(DateTime

                CustData.Add(cust);
            }//end of foreach

            ViewBag.ColLabels = ColLabels;
            ViewBag.ColValues = CustData;

            return View(jobMains.ToList().Where(j=>j.JobDate.CompareTo(DateTime.Today) >= 0));

        }


        public void updateJobDate(int mainId)
        {


            DateTime today = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Singapore Standard Time"));
            today = today.Date;
            //update jobdate
            var main = db.JobMains.Where(j => mainId == j.Id).FirstOrDefault();

            //loop though all jobservices in the jobmain
            //to get the latest date
            foreach (var svc in db.JobServices.Where(s => s.JobMainId == main.Id))
            {
                //assign latest basin on today

                //get latest date
                if (DateTime.Compare(today, (DateTime)svc.DtStart) > 0)   //if today is later than datestart, assign datestart to jobdate, 
                {
                    main.JobDate = (DateTime)svc.DtStart;
                    db.Entry(main).State = EntityState.Modified;
                }
                else  //if today is later than datestart, assign datestart to jobdate, 
                {
                    main.JobDate = (DateTime)svc.DtStart;
                    db.Entry(main).State = EntityState.Modified;
                }
                db.SaveChanges();
            }
        }


        public ActionResult JobTable2(int? span=30) //2017 version
        {
            System.DateTime dtNow = this.GetCurrentTime();
            System.DateTime dtStart = new DateTime(dtNow.Year, dtNow.Month, dtNow.Day, 12,0,0);
            System.DateTime dtUntil = System.DateTime.Now.AddDays((double)span);
            dtUntil = new DateTime(dtUntil.Year, dtUntil.Month, dtUntil.Day, 23, 59, 59);
            //Column Date Labels
            System.Collections.ArrayList ColLabels = new System.Collections.ArrayList();
            for (DateTime dtItem = dtStart; dtItem.CompareTo(dtUntil) < 0; dtItem = dtItem.AddDays(1))
            {
                ColLabels.Add(dtItem.ToString("dd") + "-" + dtItem.DayOfWeek.ToString());
            }

            System.Collections.ArrayList CustData = new System.Collections.ArrayList();
            
            var jobMains2 = db.JobMains.Include(j => j.Customer).Include(j => j.Branch).Include(j => j.JobStatus).Include(j => j.JobThru).OrderBy(d => d.JobDate);

            var jobMains = jobMains2.ToList().Where(
                d => ( d.JobStatusId == JOBRESERVATION || d.JobStatusId == JOBCONFIRMED )
//                || (d.JobStatusId == JOBCLOSED && d.JobDate.CompareTo(System.DateTime.Now.AddDays(1)) >= 0)
              );

            System.Collections.ArrayList data = new System.Collections.ArrayList();
            foreach (var item in jobMains)
            {
                JobTableData cust = new JobTableData();
                cust.tblValue = new List<JobTableValue>();

                cust.iCustId = item.CustomerId;
                cust.iJobId = item.Id;

                List<Models.JobServices> svc = db.JobServices.Include(j => j.JobServicePickups).Where(d => d.JobMainId == item.Id).ToList();

                for (DateTime dtItem = dtStart; dtItem.CompareTo(dtUntil) < 0; dtItem = dtItem.AddDays(1))
                {
                    foreach (var svcitem in svc)
                    {
                        int istart = dtItem.CompareTo((DateTime)svcitem.DtStart);
                        int iend = dtItem.CompareTo((DateTime)svcitem.DtEnd);

                        string sLabel = dtItem.ToString("dd") + "-" + dtItem.DayOfWeek.ToString();
                        if (dtItem.CompareTo( (DateTime)svcitem.DtStart) >= 0 && dtItem.CompareTo( (DateTime)svcitem.DtEnd ) <= 0)
                        {
                            string sDriver = "";
                            try
                            {
                                sDriver = svcitem.JobServicePickups.FirstOrDefault().ProviderName.Trim();
                            }
                            catch(Exception e)
                            {
                                sDriver = "";
                            }
                                
                            cust.tblValue.Add(new JobTableValue { DtDate = dtItem, Book = 1, supplier = svcitem.Supplier.Name, item = svcitem.SupplierItem.Description, Incharge = sDriver, label = sLabel } );
                        }
                        else
                        {
                            cust.tblValue.Add(new JobTableValue { DtDate = dtItem, Book = 0, supplier = "", item = "", Incharge ="", label = sLabel });

                        }//end of if dtItem.Compare
                        
                    } //end of foreach ( var svcitem...

                }// end of for(DateTime

                CustData.Add(cust);
            }//end of foreach

            ViewBag.ColLabels = ColLabels;
            ViewBag.ColValues = CustData;

            return View(jobMains.ToList());
        }

        public ActionResult JobLeads()
        {
            var jobMains2 = db.JobMains.Include(j => j.JobSuppliers).Include(j => j.Customer).Include(j => j.Branch).Include(j => j.JobStatus).Include(j => j.JobThru).OrderBy(d => d.JobDate);
            var leads = jobMains2.ToList().Where(d => d.JobStatusId == JOBINQUIRY || d.JobStatusId == JOBRESERVATION);

            //var leads = jobMains2.ToList();
            //var t = leads.FirstOrDefault().JobSuppliers;
            //var data = t.FirstOrDefault().Service.Description;

            return View(leads);
        }

        public ActionResult ProductTemplates()
        {
            var jobMains2 = db.JobMains.Include(j => j.JobSuppliers).Include(j => j.Customer).Include(j => j.Branch).Include(j => j.JobStatus).Include(j => j.JobThru).OrderBy(d => d.JobDate);
            var templates = jobMains2.ToList().Where(d => d.JobStatusId == JOBTEMPLATE);

            return View(templates);
        }

        public ActionResult ConfirmJobStatus(int? id)
        {
            var Job = db.JobMains.Find(id);
            Job.JobStatusId = 3;
            db.Entry(Job).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("JobServices", "JobOrder", new { JobMainId = id });
        }

        #region Job Notes
        public ActionResult JobNotes(int? id)
        {
            var Job = db.JobMains.Where(d => d.Id == id).FirstOrDefault();
            ViewBag.JobOrder = Job;
            ViewBag.JobId = id;

            var jobnotes = db.JobNotes.Where(d => d.JobMainId == id).OrderBy(s => s.Sort);
            return View(jobnotes);
        }

        public ActionResult DeleteNote(int? id, int jobId)
        {
            JobNote notes = db.JobNotes.Find(id);
            db.JobNotes.Remove(notes);
            db.SaveChanges();

            return RedirectToAction("JobNotes", "JobMains", new { id = jobId });
        }

        public ActionResult CreateJobNote(int? id)
        {
            ViewBag.JobMainId = new SelectList(db.JobMains, "Id", "Description", id);
            JobNote jn = new JobNote();
            jn.Sort = 10 * ( 1 + db.JobNotes.Where(d => d.JobMainId == id).ToList().Count() );

            ViewBag.templateNotes = db.PreDefinedNotes.ToList();
            ViewBag.JobId = id;
            return View(jn);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateJobNote([Bind(Include = "Id,JobMainId,Sort,Note")] JobNote jobnote)
        {
            if (ModelState.IsValid)
            {
                db.JobNotes.Add(jobnote);
                db.SaveChanges();
                return RedirectToAction("JobNotes", new { id = jobnote.JobMainId });
            }

            ViewBag.JobMainId = new SelectList(db.JobMains, "Id", "Description", jobnote.JobMainId);
            return View(jobnote);
        }

        public ActionResult EditJobNote(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobNote jobNote = db.JobNotes.Find(id);
            if (jobNote == null)
            {
                return HttpNotFound();
            }
            ViewBag.JobMainId = new SelectList(db.JobMains, "Id", "Description", jobNote.JobMainId);

            return View(jobNote);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditJobNote([Bind(Include = "Id,JobMainId,Sort,Note")] JobNote jobNote)
        {
            if (ModelState.IsValid)
            {
                db.Entry(jobNote).State = EntityState.Modified;
                if ((int)jobNote.Sort % 10 != 0) jobNote.Sort = (int)jobNote.Sort * 10;

                db.SaveChanges();
                return RedirectToAction("JobNotes", new { id = jobNote.JobMainId });
            }
            ViewBag.JobMainId = new SelectList(db.JobMains, "Id", "Description", jobNote.JobMainId);

            return View(jobNote);
        }

        #endregion


        #region  JobQuickList
        public ActionResult JobQuickList()
        {

            DateTime today = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Singapore Standard Time"));
            today = today.Date;

            IQueryable<Models.JobMain> jobMains = db.JobMains.Include(j => j.Customer).Include(j => j.Branch).Include(j => j.JobStatus).Include(j => j.JobThru).OrderBy(d => d.JobDate);
            jobMains = (IQueryable<Models.JobMain>)jobMains.Where(d => (d.JobStatusId == JOBRESERVATION || d.JobStatusId == JOBCONFIRMED) && d.JobDate.CompareTo(today) < 0  );

            var p = jobMains.Select(s => s.Id);

            var data = db.JobServices.Where(w => p.Contains(w.JobMainId)).ToList().OrderBy(s => s.DtStart);


            ViewBag.Current = this.GetCurrentTime().ToString("MMM-dd-yyyy (ddd)");

            return View(data);
        }
        #endregion
    }


    public class JobTableData
    {
        public int iCustId { get; set; }
        public int iJobId { get; set; }
        public List<JobTableValue> tblValue { get; set; }
    }

    public class JobTableValue
    {
        public System.DateTime DtDate { get; set; }
        public int Book { get; set; }
        public string supplier { get; set; }
        public string item { get; set; }
        public string Incharge { get; set; }
        public string label { get; set; }
        public string itemicon { get; set; }
    }

    public class svcId
    { public int jId { get; set; } }

}
