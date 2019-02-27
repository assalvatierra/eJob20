using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using JobsV1.Models;
using System.IO;

namespace JobsV1.Controllers
{
    public class SalesLeadsController : Controller
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
        private DBClasses dbclasses = new DBClasses();
        // GET: SalesLeads
        public ActionResult Index(int? sortid, int? leadId)
        {

            if (sortid != null)
                Session["SLFilterID"] = (int)sortid;
            else
            {
                if (Session["SLFilterID"] != null)
                    sortid = (int)Session["SLFilterID"];
                else
                {
                    Session["SLFilterID"] = 3;

                }
            }

            var salesLeads = db.SalesLeads.Include(s => s.Customer)
                        .Include(s => s.SalesLeadCategories)
                        .Include(s => s.SalesStatus).OrderBy(s => s.Date)
                        .ToList();

            switch (sortid) {
                case 1://approved
                    salesLeads = db.SalesLeads.Include(s => s.Customer)
                                .Include(s => s.SalesLeadCategories)
                                .Include(s => s.SalesStatus).OrderBy(s => s.Date).Include(s => s.Customer.JobMains)
                                .Where(s => s.SalesStatus.Where(ss => ss.SalesStatusCodeId > 4)
                                .OrderByDescending(ss => ss.SalesStatusCodeId).FirstOrDefault().SalesStatusCodeId < 6) // Current
                                .ToList();
                    break;
                case 2:// closedb
                    salesLeads = db.SalesLeads.Include(s => s.Customer)
                                .Include(s => s.SalesLeadCategories)
                                .Include(s => s.SalesStatus).OrderBy(s => s.Date).Include(s => s.Customer.JobMains)
                                .Where(s => s.SalesStatus.Where(ss => ss.SalesStatusCodeId == 5)
                                .OrderByDescending(ss => ss.SalesStatusCodeId).FirstOrDefault().SalesStatusCodeId == 5) // Current
                                .ToList();
                    break;

                case 3:
                    // all
                    salesLeads = db.SalesLeads.Include(s => s.Customer)
                         .Include(s => s.SalesLeadCategories).Include(s => s.Customer.JobMains)
                         .Include(s => s.SalesStatus).OrderBy(s => s.Date)
                         .ToList();
                    break;

                case 4:
                    // OnGiong
                    salesLeads = db.SalesLeads.Include(s => s.Customer)
                                .Include(s => s.SalesLeadCategories)
                                .Include(s => s.SalesStatus).OrderBy(s => s.Date).Include(s => s.Customer.JobMains)
                                .Where(s => s.SalesStatus.Where(ss => ss.SalesStatusCodeId > 2)
                                .OrderByDescending(ss => ss.SalesStatusCodeId).FirstOrDefault().SalesStatusCodeId < 5) // Current
                                .ToList();
                    break;
                case 5:
                    // new Leads
                    salesLeads = db.SalesLeads.Include(s => s.Customer)
                                .Include(s => s.SalesLeadCategories)
                                .Include(s => s.SalesStatus).OrderBy(s => s.Date).Include(s => s.Customer.JobMains)
                                .Where(s => s.SalesStatus.Where(ss => ss.SalesStatusCodeId > 0)
                                .OrderByDescending(ss => ss.SalesStatusCodeId).FirstOrDefault().SalesStatusCodeId < 3) // Current
                                .ToList();
                    break;
                default:
                    // new Leads
                    salesLeads = db.SalesLeads.Include(s => s.Customer)
                                .Include(s => s.SalesLeadCategories)
                                .Include(s => s.SalesStatus).OrderBy(s => s.Date).Include(s => s.Customer.JobMains)
                                .Where(s => s.SalesStatus.Where(ss => ss.SalesStatusCodeId > 0)
                                .OrderByDescending(ss => ss.SalesStatusCodeId).FirstOrDefault().SalesStatusCodeId < 3) // Current
                                .ToList();
                    /*
                    salesLeads = db.SalesLeads.Include(s => s.Customer)
                         .Include(s => s.SalesLeadCategories).Include(s => s.Customer.JobMains)
                         .Include(s => s.SalesStatus).OrderByDescending(s => s.Date)
                         .ToList();
                         */
                    break;
            }

            ViewBag.LeadId = leadId;
            ViewBag.CurrentFilter = sortid;
            ViewBag.StatusCodes = db.SalesStatusCodes.ToList();

            return View(salesLeads);
        }

        // GET: SalesLeads/Details/5
        public ActionResult Details(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            SalesLead salesLead = db.SalesLeads.Find(id);
            if (salesLead == null)
            {
                return HttpNotFound();
            }

            var salesLeads = db.SalesLeads.Include(s => s.Customer)
                        .Include(s => s.SalesLeadCategories)
                        .Include(s => s.SalesStatus).OrderByDescending(s => s.Date)
                        .ToList();
            
            ViewBag.StatusCodes = db.SalesStatusCodes.ToList();


            return View(salesLead);
        }

        // GET: SalesLeads/Create
        public ActionResult Create()
        {
            var tmp = new Models.SalesLead();
            tmp.Date = DateTime.Now;
            tmp.DtEntered = DateTime.Now;
            tmp.EnteredBy = HttpContext.User.Identity.Name;
            

            ViewBag.CustomerId = new SelectList(db.Customers, "Id", "Name");
            ViewBag.AssignedTo = new SelectList(dbclasses.getUsers(), "UserName", "UserName");
            ViewBag.CustomerList = db.Customers.ToList();
            return View(tmp);
        }

        // POST: SalesLeads/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Date,Details,Remarks,Price,CustomerId,CustName,DtEntered,EnteredBy,AssignedTo,CustPhone,CustEmail")] SalesLead salesLead)
        {
            if (ModelState.IsValid && salesLead.EnteredBy != null)
            {
                db.SalesLeads.Add(salesLead);
                db.SaveChanges();

                AddSalesStatus(salesLead.Id, 1);    //NEW
                
                return RedirectToAction("Index", new { sortid = 5 , leadid = salesLead.Id});
            }

            ViewBag.CustomerId = new SelectList(db.Customers, "Id", "Name", salesLead.CustomerId);
            ViewBag.AssignedTo = new SelectList(dbclasses.getUsers(), "UserName", "UserName", salesLead.AssignedTo);

            return View(salesLead);
        }

        // GET: SalesLeads/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SalesLead salesLead = db.SalesLeads.Find(id);
            if (salesLead == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustomerId = new SelectList(db.Customers, "Id", "Name", salesLead.CustomerId);
            ViewBag.AssignedTo = new SelectList(dbclasses.getUsers(), "UserName", "UserName", salesLead.AssignedTo);
            ViewBag.CustomerList = db.Customers.ToList();
            ViewBag.leadId = id;

            return View(salesLead);
        }

        // POST: SalesLeads/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Date,Details,Remarks,Price,CustomerId,CustName,DtEntered,EnteredBy,AssignedTo,CustPhone,CustEmail")] SalesLead salesLead)
        {
            if (ModelState.IsValid)
            {
                db.Entry(salesLead).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "SalesLeads", new { leadId = salesLead.Id });
            }
            ViewBag.CustomerId = new SelectList(db.Customers, "Id", "Name", salesLead.CustomerId);
            ViewBag.AssignedTo = new SelectList(dbclasses.getUsers(), "UserName", "UserName", salesLead.AssignedTo);
            return View(salesLead);
        }

        // GET: SalesLeads/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SalesLead salesLead = db.SalesLeads.Find(id);
            if (salesLead == null)
            {
                return HttpNotFound();
            }
            return View(salesLead);
        }

        // POST: SalesLeads/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SalesLead salesLead = db.SalesLeads.Find(id);
            db.SalesLeads.Remove(salesLead);
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


        #region Sales Lead Category
        public ActionResult SalesLeadCat(int id)
        {
            var data = db.SalesLeadCategories.Where(d => d.SalesLeadId == id);
            ViewBag.CategoryList = db.SalesLeadCatCodes.ToList();
            ViewBag.SalesLeadId = (int)id;
            return View(data.ToList());
        }

        public ActionResult AddSalesLeadCat(int CodeId, int slId)
        {
            string Message = "";
            try
            {
                db.Database.ExecuteSqlCommand(@"
                insert into SalesLeadCategories([SalesLeadCatCodeId],[SalesLeadId])
                values (" + CodeId.ToString() + "," + slId.ToString() + "); "
                    );

                Message = "Success: " + CodeId.ToString() + "Added...";
                ViewBag.SalesLeadId = slId;
            }
            catch(Exception ex)
            {
                Message = "Error: " + ex.Message;
            }

            return RedirectToAction("SalesLeadCat", new { id = slId });
        }

        public ActionResult RemoveSalesLeadCat(int CodeId, int slId)
        {
            string Message = "";
            try
            {
                db.Database.ExecuteSqlCommand(@"
                delete from SalesLeadCategories
                where (SalesLeadCatCodeId=" + CodeId.ToString() + @"
                AND SalesLeadId=" + slId.ToString() + "); "
                    );

                Message = "Success: " + CodeId.ToString() + "Removed...";
                ViewBag.SalesLeadId = slId;
            }
            catch (Exception ex)
            {
                Message = "Error: " + ex.Message;
            }

            return RedirectToAction("SalesLeadCat", new { id = slId });
        }
        #endregion

        #region AddSalesStatus
        public ActionResult AddSalesStatus(int slId, int StatusId)
        {
            string strMsg = "";

            if (db.SalesStatus.Where(s => s.SalesLeadId == slId && s.SalesStatusCodeId == StatusId).FirstOrDefault() == null) {

            try
            {
                db.Database.ExecuteSqlCommand(@"
                Insert into SalesStatus([DtStatus],[SalesStatusCodeId],[SalesLeadId])
                Values('" + DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss") + "','" + StatusId + "','" + slId.ToString() + @"');
                ");

                db.SaveChanges();

                strMsg = "Success";

                switch (StatusId)
                {
                    case 1: //New Leads
                    case 2:
                        Session["SLFilterID"] = 5;
                        break;
                    case 3: //New Leads
                    case 4:
                        Session["SLFilterID"] = 4;
                        break;
                    case 5: //New Leads
                    case 6:
                        Session["SLFilterID"] = 1;
                        break;
                    case 7: //New Leads
                        Session["SLFilterID"] = 2;
                        break;
                    default:
                        break;  //SLFilterID = 3 (All)
                }
            }
            catch(Exception Ex)
            {
                strMsg = "Error:" + Ex.Message;
            }

            ViewBag.Message = strMsg;

            }

            return RedirectToAction("Index");
        }
        #endregion

        #region Add Request
        public ActionResult ListActivityCodes(int id)
        {
            var data = db.SalesActCodes.ToList();
            ViewBag.SalesLeadId = id;

            return View(data);
        }
        public ActionResult AddActivityCode(int slId, int ActCodeId)
        {
            var data = new Models.SalesActivity();
            data.SalesLeadId = slId;
            data.SalesActCodeId = ActCodeId;
            data.DtActivity = DateTime.Now;
            data.SalesActStatusId = (int)db.SalesActCodes.Find(ActCodeId).DefaultActStatus;
            data.DtEntered = DateTime.Now;
            data.EnteredBy = User.Identity.Name;
            data.Particulars = db.SalesActCodes.Find(ActCodeId).Desc;

            ViewBag.SalesActCodeId = new SelectList(db.SalesActCodes, "Id", "Name", ActCodeId);
            ViewBag.SalesLeadId = new SelectList(db.SalesLeads, "Id", "Details", slId);
            ViewBag.SalesActStatusId = new SelectList(db.SalesActStatus, "Id", "Name", data.SalesActStatusId);
            ViewBag.sId = slId;
            return View(data);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddActivityCode([Bind(Include = "Id,SalesLeadId,SalesActCodeId,Particulars,DtActivity,SalesActStatusId,DtEntered,EnteredBy")] SalesActivity salesActivity)
        {
            if (ModelState.IsValid)
            {
                db.SalesActivities.Add(salesActivity);
                db.SaveChanges();
                return RedirectToAction("Index", new { leadId = salesActivity.SalesLeadId});
            }

            ViewBag.SalesActCodeId = new SelectList(db.SalesActCodes, "Id", "Name", salesActivity.SalesActCodeId);
            ViewBag.SalesLeadId = new SelectList(db.SalesLeads, "Id", "Details", salesActivity.SalesLeadId);
            ViewBag.SalesActStatusId = new SelectList(db.SalesActStatus, "Id", "Name", salesActivity.SalesActStatusId);
            return View(salesActivity);
        }

        public ActionResult SalesActivityDone(int id)
        {
            db.Database.ExecuteSqlCommand("update SalesActivities set SalesActStatusId=2 where Id=" + id);
            var slid = db.SalesActivities.Where(s => s.Id == id).FirstOrDefault().SalesLeadId;
            return RedirectToAction("Index", new { leadId = slid});
        }

        public ActionResult SalesActivityRemove(int id)
        {
            var slid = db.SalesActivities.Where(s => s.Id == id).FirstOrDefault().SalesLeadId;
            db.Database.ExecuteSqlCommand("DELETE FROM SalesActivities where Id=" + id);
            return RedirectToAction("Index", new { leadId = slid});
        }
        #endregion

        #region Customer

        private List<SelectListItem> StatusList = new List<SelectListItem> {
                new SelectListItem { Value = "ACT", Text = "Active" },
                new SelectListItem { Value = "INC", Text = "Inactive" },
                new SelectListItem { Value = "BAD", Text = "Bad Account" }
                };

        public ActionResult CompanyDetail(int slid, int CustId)
        {
            var data = db.Customers.Find(CustId);

            if (data.Name == "<< New Customer >>")
            {
                return RedirectToAction("CreateCustomer", new { SlId = slid } );
            }

            ViewBag.Status = new SelectList(StatusList, "value", "text", data.Status);
            ViewBag.leadId = slid;
            return View(data);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CompanyDetail([Bind(Include = "Id,Name,Email,Contact1,Contact2,Remarks,Status")] Customer customer, int? leadId)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customer).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index", "SalesLeads",new {leadId = leadId} );
            }
            ViewBag.Status = new SelectList(StatusList, "value", "text", customer.Status);

            return View(customer);
        }



        public ActionResult CreateCustomer(int SlId)
        {
            var dslCust = db.SalesLeads.Find(SlId);
            var data = new Models.Customer();
            data.Name = dslCust.CustName;
            data.Email = dslCust.CustEmail;
            data.Contact1 = dslCust.CustPhone;

            data.Status = "ACT";

            ViewBag.Status = new SelectList(StatusList, "value", "text", data.Status);
            ViewBag.LeadId = SlId;
            return View(data);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateCustomer([Bind(Include = "Id,Name,Email,Contact1,Contact2,Remarks,Status")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                if (customer.Status == null || customer.Status.Trim() == "") customer.Status = "ACT";

                db.Customers.Add(customer);
                db.SaveChanges();

                string SlId = Request.Form["SalesLeadId"];
                db.Database.ExecuteSqlCommand(@"
                    Update SalesLeads set CustomerId="+ customer.Id +" where Id=" + SlId +";"
                    );

                return RedirectToAction("Index");
            }

            ViewBag.Status = new SelectList(StatusList, "value", "text", customer.Status);
            return View(customer);
        }

        #endregion

        #region SalesLeadFiles

        // GET: SalesLeads/Create
        public ActionResult FileCreate(int custid, int salesleadId)
        {
            ViewBag.CustomerId = new SelectList(db.Customers, "Id", "Name", custid);
            ViewBag.SalesleadId = salesleadId;
            return View();
        }

        // POST: SalesLeads/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FileCreate([Bind(Include = "Id,Date,Details,Remarks,Price,CustomerId,CustName,DtEntered,EnteredBy,AssignedTo,CustPhone,CustEmail")] SalesLead salesLead)
        {
            if (ModelState.IsValid && salesLead.EnteredBy != null)
            {
                db.SalesLeads.Add(salesLead);
                db.SaveChanges();
                return RedirectToAction("FileList",new { custid  = salesLead.CustomerId, salesleadId = salesLead.Id});
            }

            ViewBag.CustomerId = new SelectList(db.Customers, "Id", "Name", salesLead.CustomerId);
            ViewBag.AssignedTo = new SelectList(dbclasses.getUsers(), "UserName", "UserName", salesLead.AssignedTo);

            return View(salesLead);
        }


        [HttpPost]
        public ActionResult UploadFiles(HttpPostedFileBase file, [Bind(Include = "Id,Desc,Folder,Path,Remarks,CustomerId")] CustFiles custFiles, int salesleadId)
        {
            if (file != null && file.ContentLength > 0)
                try
                {

                    string extension = Path.GetExtension(file.FileName);

                    //  ~/Images/CustomerFiles/(customerid)/filename.png Path.GetFileName(file.FileName)
                    string path = Path.Combine(Server.MapPath("~/Images/CustomerFiles/" + custFiles.CustomerId),
                                               Path.GetFileName(file.FileName));
                    string directory = Request.Url.GetLeftPart(UriPartial.Authority) + "/Images/CustomerFiles/" + custFiles.CustomerId + "/";
                    if (ModelState.IsValid)
                    {
                        //add customer
                        custFiles.Folder = custFiles.CustomerId.ToString(); // ~/customerid
                        custFiles.Path = directory + Path.GetFileName(file.FileName);
                        db.CustFiles.Add(custFiles);
                        db.SaveChanges();

                        AddFileReference(salesleadId, custFiles.Id);

                        //create directory if does not exist
                        var folder = Server.MapPath("~/Images/CustomerFiles/" + custFiles.CustomerId);
                        if (!Directory.Exists(folder))
                        {
                            Directory.CreateDirectory(folder);
                        }

                        file.SaveAs(path);
                        ViewBag.Message = "File uploaded successfully";
                    }
                    else
                    {
                        ViewBag.Message = "File uploaded unsuccessfully";
                        return View("#");
                    }

                    ViewBag.CustomerId = new SelectList(db.Customers, "Id", "Name", custFiles.CustomerId);
                    
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "ERROR:" + ex.Message.ToString();
                }
            else
            {
                ViewBag.Message = "You have not specified a file.";
            }

            return RedirectToAction("FileList", new { custid = custFiles.Id, salesleadId = salesleadId });

        }

        public void AddFileReference(int RefId, int custfileId) {
            db.CustFileRefs.Add(new CustFileRef {
                RefTable = "SalesLead",
                RefId    = RefId,
                CustFilesId = custfileId
            });
            db.SaveChanges();
        }


        public ActionResult FileList(int custid, int salesleadId) {
            List<CustFileRef> Files = db.CustFileRefs.Where(f => f.CustFile.Customer.Id == custid && f.RefId == salesleadId).ToList();
            ViewBag.CustId = custid;
            ViewBag.SalesLeadId = salesleadId;

            return View(Files);
        }

        public ActionResult DeleteFile(int id, int custid, int salesleadId) {
            
            CustFiles custfiles = db.CustFiles.Find(id);
            db.CustFiles.Remove(custfiles);
            db.SaveChanges();

            return RedirectToAction("FileList", new { custid = custid, salesleadId = salesleadId });
        }
        #endregion

        #region Quotation
        public ActionResult QuotationCreate(int id, int custid,
            decimal amount, string cusmail, string contact,
            string desc, string remarks, DateTime leadDT) {

            //initial values from 
            JobMain job = new JobMain();
            job.JobDate = System.DateTime.Today;
            job.NoOfDays = 1;
            job.NoOfPax = 1;
            job.AgreedAmt = amount;
            job.CustContactEmail = cusmail;
            job.CustContactNumber = contact;
            job.Description = desc;
            job.JobRemarks = remarks;
            job.JobDate = leadDT;

            ViewBag.BranchId = new SelectList(db.Branches, "Id", "Name");
            ViewBag.JobStatusId = new SelectList(db.JobStatus, "Id", "Status",1);
            ViewBag.JobThruId = new SelectList(db.JobThrus, "Id", "Desc");
            ViewBag.CustomerId = new SelectList(db.Customers , "Id", "Name", custid);
            ViewBag.Id = id;

            return View(job);
        }


        // POST: JobMains/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult QuotationCreate([Bind(Include = "Id,JobDate,CustomerId,Description,NoOfPax,NoOfDays,AgreedAmt,JobRemarks,JobStatusId,StatusRemarks,BranchId,JobThruId,CustContactEmail,CustContactNumber")] JobMain jobMain, int leadId)
        {
            int NewCustSysId = 1;
            int jobMainId = jobMain.Id;
            if (ModelState.IsValid)
            {
                if (jobMain.CustContactEmail == null && jobMain.CustContactNumber == null)
                {
                    var cust = db.Customers.Find(jobMain.CustomerId);
                    jobMain.CustContactEmail = cust.Email;
                    jobMain.CustContactNumber = cust.Contact1;
                    
                }

                db.JobMains.Add(jobMain);
               // db.SaveChanges();

                db.SalesLeadLinks.Add(new SalesLeadLink {
                    JobMainId = jobMain.Id,
                    SalesLeadId = leadId,
                });
                db.SaveChanges();

                if (jobMain.CustomerId == NewCustSysId)
                    return RedirectToAction("CreateCustomer", "JobMains", new { jobid = jobMain.Id });
                else
                    return RedirectToAction("Services", "JobServices", new { id = jobMain.Id });
            }

            ViewBag.CustomerId = new SelectList(db.Customers.Where(d => d.Status == "ACT"), "Id", "Name", jobMain.CustomerId);
            ViewBag.BranchId = new SelectList(db.Branches, "Id", "Name", jobMain.BranchId);
            ViewBag.JobStatusId = new SelectList(db.JobStatus, "Id", "Status", jobMain.JobStatusId);
            ViewBag.JobThruId = new SelectList(db.JobThrus, "Id", "Desc", jobMain.JobThruId);
            return View(jobMain);
        }


        // GET: SalesLead/QuotationEdit/5
        public ActionResult QuotationEdit(int? id)
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
            ViewBag.CustomerId = new SelectList(db.Customers.Where(d => d.Status == "ACT"), "Id", "Name", jobMain.CustomerId);
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
        public ActionResult QuotationEdit([Bind(Include = "Id,JobDate,CustomerId,Description,NoOfPax,NoOfDays,AgreedAmt,JobRemarks,JobStatusId,StatusRemarks,BranchId,JobThruId,CustContactEmail,CustContactNumber")] JobMain jobMain)
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
                    return RedirectToAction("CreateCustomer", "JobMains",new { jobid = jobMain.Id });
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


        public ActionResult ConfirmJob(int? id)
        {
            JobMain job = db.JobMains.Find(id);
            job.JobStatusId = JOBCONFIRMED;
            db.Entry(job).State = EntityState.Modified;
            db.SaveChanges();

            return Redirect(Request.UrlReferrer.ToString());
        }


        public ActionResult QuotationLink(int id, int? linkid)
        {
            var links = db.SalesLeadLinks.ToList();
            var main = db.JobMains.Where(s=> s.SalesLeadLinks.Where(l=>l.JobMainId == s.Id).FirstOrDefault().JobMainId > 0).ToList();
            // Get all anotherHumans where the record does not exist in humans
            ViewBag.links = links;
            var jobMains2 = db.JobMains.Include(j => j.JobSuppliers).Include(j => j.Customer).Include(j => j.Branch).Include(j => j.JobStatus).Include(j => j.JobThru).Include(s=>s.SalesLeadLinks).OrderBy(d => d.JobDate);

            IEnumerable<JobMain> leads = jobMains2.ToList();
            if (linkid == 1)
            {
                //joborder
                leads = jobMains2.ToList().Where(d => d.JobStatusId > 1).Except(main);

            }
            else 
{
                //quotation
                leads = jobMains2.ToList().Where(d => d.JobStatusId == JOBINQUIRY).Except(main);

            }

            ViewBag.LeadId = id;

            return View(leads);
        }


        public ActionResult QuotationLinkSelect(int id, int leadId)
        {
            db.SalesLeadLinks.Add(new SalesLeadLink{
                JobMainId = id,
                SalesLeadId = leadId
            });
            db.SaveChanges();
            return RedirectToAction("Index", new {  leadId = leadId });
        }


        public ActionResult QuotationUnlink(int id)
        {


            SalesLeadLink salesLeadlinks = db.SalesLeadLinks.Where(s => s.SalesLeadId == id).FirstOrDefault(); ;
            db.SalesLeadLinks.Remove(salesLeadlinks);
            db.SaveChanges();
            return RedirectToAction("Index" , new { leadId = id });
        }
        #endregion
    }
}
