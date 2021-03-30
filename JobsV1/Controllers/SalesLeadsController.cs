﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using JobsV1.Models;
using JobsV1.Models.Class;
using System.IO;
using Newtonsoft.Json;
using Microsoft.Ajax.Utilities;

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
        private SalesLeadClass sldb = new SalesLeadClass();
        private DateClass date = new DateClass();

        // GET: SalesLeads
        public ActionResult IndexOld(int? sortid, int? leadId)
        {

            if (sortid != null)
                Session["SLFilterID"] = (int)sortid;
            else
            {
                if (Session["SLFilterID"] != null)
                    sortid = (int)Session["SLFilterID"];
                else
                {
                    sortid = 1;
                    Session["SLFilterID"] = 1;
                }
            }

            //get salesl eads leads
            var salesLeads = sldb.GetSalesLeads((int)sortid);


            ViewBag.LeadId = leadId;
            ViewBag.CurrentFilter = sortid;
            ViewBag.StatusCodes = db.SalesStatusCodes
                .Where(s => s.SalesStatusTypeId == 1 || s.SalesStatusTypeId == 2)
                .OrderBy(s => s.SeqNo).ThenBy(s => s.Id).ToList();
            ViewBag.User = HttpContext.User.Identity.Name;

            //for adding new item 
            AddSupItemPartial();
           
            return View(salesLeads.OrderByDescending(s=>s.Date));
        }


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
                    sortid = 1;
                    Session["SLFilterID"] = 1;
                }
            }

            //get sales leads list
            var salesLeads = sldb.GetcSalesLeads((int)sortid);

            ViewBag.LeadId = leadId;
            ViewBag.CurrentFilter = sortid;
            ViewBag.StatusCodes = db.SalesStatusCodes
                .Where(s => s.SalesStatusTypeId == 1 || s.SalesStatusTypeId == 2)
                .OrderBy(s => s.SeqNo).ThenBy(s => s.Id).ToList();
            ViewBag.User = HttpContext.User.Identity.Name;
            ViewBag.ActTypes = db.CustEntActTypes.ToList();
            //for adding new item 
            AddSupItemPartial();

            return View(salesLeads.OrderByDescending(s => s.Date));
        }

        public List<SalesStatusCode> GetSalesStatuses()
        {
            var statusList = db.SalesStatusCodes
                 .Where(s => s.SalesStatusTypeId == 1 || s.SalesStatusTypeId == 2)
                 .OrderBy(s => s.SeqNo).ThenBy(s => s.Id).ToList();

            var limit1 = 500000;
            var limit2 = 3000000;

            return statusList;
        }


        // GET: SalesLeads
        public ActionResult Status(int? sortid, int? leadId)
        {

            if (sortid != null)
                Session["SLFilterID"] = (int)sortid;
            else
            {
                if (Session["SLFilterID"] != null)
                    sortid = (int)Session["SLFilterID"];
                else
                {
                    sortid = 3;
                    Session["SLFilterID"] = 3;

                }
            }

            var salesLeads = sldb.GetSalesLeads((int)sortid);

            ViewBag.LeadId = leadId;
            ViewBag.CurrentFilter = sortid;
            ViewBag.StatusCodes = db.SalesStatusCodes
                .ToList();

            //for adding new item 
            AddSupItemPartial();

            return View(salesLeads.OrderByDescending(s => s.Date));
        }


        // GET: SalesLeads
        public ActionResult IndexCompanies(int? sortid, int? leadId, int? companyId)
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

            var leadList = sldb.generateList(null, null, null, null).ToList();
            
            ViewBag.LeadId = leadId;
            ViewBag.CurrentFilter = sortid;
            ViewBag.StatusCodes = db.SalesStatusCodes.ToList();

            //for adding new item 
            AddSupItemPartial();

            return View(leadList);
        }


        // GET: SalesLeads
        public ActionResult LeadDetails2(int? sortid, int? id)
        {
            int leadId = (int)id;
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

            if (leadId != 0)
            {
                var Id = (int)leadId;
                var salesLeads = db.SalesLeads.Include(s => s.Customer)
                        .Include(s => s.SalesLeadCategories)
                        .Include(s => s.SalesStatus).OrderBy(s => s.Date)
                        .Where(s => s.Id == Id);


                ViewBag.LeadId = leadId;
                ViewBag.CurrentFilter = sortid;
                ViewBag.StatusCodes = db.SalesStatusCodes.ToList();

                //for adding new item 
                AddSupItemPartial();

                return View(salesLeads);
            }
            return RedirectToAction("Index", new { sortid = 1 });

        }

        // GET: SalesLeads
        public ActionResult LeadDetails(int? id,int? sortid)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            int leadId = (int)id;
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

            if (leadId != 0)
            {
                var Id = (int)id;
                var salesLeads = db.SalesLeads.Include(s => s.Customer)
                        .Include(s => s.SalesLeadCategories)
                        .Include(s => s.SalesStatus).OrderBy(s => s.Date)
                        .Where(s => s.Id == Id);


                ViewBag.LeadId = leadId;
                ViewBag.CurrentFilter = sortid;
                ViewBag.StatusCodes = db.SalesStatusCodes.ToList();

                //for adding new item 
                AddSupItemPartial();

                return View(salesLeads);
            }

            return RedirectToAction("Index", new { sortid = 1 });

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
            ViewBag.Company = salesLead.SalesLeadCompanies.OrderByDescending(s => s.Id).FirstOrDefault().Id;

            return View(salesLead);
        }

        // GET: SalesLeads/Create
        public ActionResult Create()
        {

            var tmp = new Models.SalesLead();
            tmp.Date = date.GetCurrentDateTime();
            tmp.DtEntered = date.GetCurrentDateTime();
            tmp.EnteredBy = HttpContext.User.Identity.Name;

            
            ViewBag.CustomerId = new SelectList(db.Customers.Where(s=>s.Status == "ACT"), "Id", "Name");
            ViewBag.AssignedTo = new SelectList(dbclasses.getUsers_wdException(), "UserName", "UserName");
            ViewBag.CompanyId = new SelectList(db.CustEntMains, "Id", "Name");

            ViewBag.CustomerList = db.Customers.Where(s=>s.Status == "ACT").ToList();
            ViewBag.CompanyList = db.CustEntMains.Where(s => s.Status != "INC").ToList();

            return View(tmp);
        }

        // POST: SalesLeads/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Date,Details,Remarks,Price,CustomerId,CustName,DtEntered,EnteredBy,AssignedTo,CustPhone,CustEmail,AssignedTo,SalesCode")] SalesLead salesLead, int? CompanyId)
        {
            if (ModelState.IsValid && SalesLeadValidation(salesLead))
            {
                if (CompanyId != null)
                {

                    if (salesLead.EnteredBy.IsNullOrWhiteSpace())
                    {
                        salesLead.EnteredBy = "Guest";
                    }
                    //int compId = salesLead.SalesLeadCompanies.OrderByDescending(s => s.Id).FirstOrDefault().Id; //get lastest company id
                    db.SalesLeads.Add(salesLead);
                    db.SaveChanges();

                    AddSalesStatus(salesLead.Id, 1);    //NEW Lead Status
                    addCompany((int)CompanyId, salesLead.Id);

                    if (!salesLead.SalesCode.IsNullOrWhiteSpace())
                    {
                        //if sales code is not null
                        //find customer activities with the same sales code
                        //and add this SalesLead ID to the CustEntActivity entries
                        UpdateCustActivities(salesLead.Id, salesLead.SalesCode);
                    }

                    return RedirectToAction("Index", new { sortid = 1 , leadid = salesLead.Id});

                }
                else
                {
                    ModelState.AddModelError("CustomerId", "Invalid Company");
                }
            }

            ViewBag.CustomerId = new SelectList(db.Customers, "Id", "Name", salesLead.CustomerId);
            ViewBag.AssignedTo = new SelectList(dbclasses.getUsers_wdException(), "UserName", "UserName", salesLead.AssignedTo);
            ViewBag.CompanyId = new SelectList(db.CustEntMains, "Id", "Name", CompanyId);

            ViewBag.CustomerList = db.Customers.Where(s => s.Status == "ACT").ToList();
            ViewBag.CompanyList = db.CustEntMains.Where(s => s.Status != "INC").ToList();

            ViewBag.CompanyName = CompanyId == null ? "" : db.CustEntMains.Find(CompanyId).Name;
            return View(salesLead);
        }

        public void addCompany(int compId, int leadId)
        {
            SalesLeadCompany slCompany = new SalesLeadCompany();
            slCompany.CustEntMainId = compId;
            slCompany.SalesLeadId = leadId;

            db.SalesLeadCompanies.Add(slCompany);
            db.SaveChanges();
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
            var company = salesLead.SalesLeadCompanies.OrderByDescending(s => s.Id).FirstOrDefault();

            ViewBag.CustomerId = new SelectList(db.Customers, "Id", "Name", salesLead.CustomerId);
            ViewBag.AssignedTo = new SelectList(dbclasses.getUsers_wdException(), "UserName", "UserName", salesLead.AssignedTo);
            ViewBag.CompanyId = new SelectList(db.CustEntMains, "Id", "Name", company.CustEntMainId);
            ViewBag.CustomerList = db.Customers.Where(s => s.Status == "ACT").ToList();
            ViewBag.CompanyList = db.CustEntMains.Where(s => s.Status != "INC").ToList();
            ViewBag.leadId = id;
            ViewBag.CompanyName = company.CustEntMain.Name;

            return View(salesLead);
        }

        // POST: SalesLeads/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Date,Details,Remarks,Price,CustomerId,CustName,DtEntered,EnteredBy,AssignedTo,CustPhone,CustEmail,AssignedTo,SalesCode")] SalesLead salesLead, int CompanyId)
        {
            if (ModelState.IsValid && SalesLeadValidation(salesLead))
            {
                db.Entry(salesLead).State = EntityState.Modified;
                db.SaveChanges();

                //update salesLead
                updateCompany(CompanyId, salesLead.Id);

                return RedirectToAction("Index", "SalesLeads", new { leadId = salesLead.Id });
            }

            var company = salesLead.SalesLeadCompanies.OrderByDescending(s => s.Id).FirstOrDefault();

            ViewBag.CustomerId = new SelectList(db.Customers, "Id", "Name", salesLead.CustomerId);
            ViewBag.AssignedTo = new SelectList(dbclasses.getUsers_wdException(), "UserName", "UserName", salesLead.AssignedTo);
            ViewBag.CompanyId = new SelectList(db.CustEntMains, "Id", "Name", CompanyId);
            ViewBag.CustomerList = db.Customers.Where(s => s.Status == "ACT").ToList();
            ViewBag.CompanyList = db.CustEntMains.Where(s => s.Status != "INC").ToList();
            ViewBag.leadId = salesLead.Id;
            ViewBag.CompanyName = company.CustEntMain.Name;

            return View(salesLead);
        }

        public void updateCompany(int compId, int leadId)
        {
            SalesLeadCompany slCompany = db.SalesLeadCompanies.Where(s=>s.SalesLeadId == leadId).FirstOrDefault();
            slCompany.CustEntMainId = compId;
            db.Entry(slCompany).State = EntityState.Modified;
            db.SaveChanges();
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


        public bool SalesLeadValidation(SalesLead salesLead)
        {
            bool isValid = true;

            if (salesLead.Date == null)
            {
                ModelState.AddModelError("Date", "Invalid Date");
                isValid = false;
            }

            if (salesLead.Details.IsNullOrWhiteSpace())
            {
                ModelState.AddModelError("Details", "Invalid Description");
                isValid = false;
            }

            if (salesLead.SalesCode.IsNullOrWhiteSpace())
            {
                ModelState.AddModelError("SalesCode", "Invalid SalesCode");
                isValid = false;
            }

            if (salesLead.CustPhone.IsNullOrWhiteSpace())
            {
                ModelState.AddModelError("CustPhone", "Invalid Contact Number");
                isValid = false;
            }

            else
            {
                if (salesLead.CustPhone.Length < 11)
                {
                    ModelState.AddModelError("CustPhone", "Invalid Contact Number");
                    isValid = false;
                }
            }

            return isValid;
        }

        [HttpPost]
        public bool UpdateSalesLeadRemarks(int Id, string Remarks)
        {
            try
            {

                var saleslead = db.SalesLeads.Find(Id);

                saleslead.Remarks = Remarks;

                db.Entry(saleslead).State = EntityState.Modified;
                db.SaveChanges();
                return true;

            }
            catch
            {
                return false;
            }
        }


        //Get Company Id 
        //Param : id - SalesLead ID
        [HttpGet]
        public string getCompanyId(int custId)
        {
            var custEnt = db.CustEntities.Where(s => s.CustomerId == custId).OrderByDescending(s => s.Id).FirstOrDefault();
            try
            {
                return custEnt.CustEntMainId.ToString();
            }
            catch 
            {
                return "1"; //default
            }
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

            if (db.SalesStatus.Where(s => s.SalesLeadId == slId && 
                s.SalesStatusCodeId == StatusId && s.SalesStatusStatusId == 1).FirstOrDefault() == null) {

                strMsg = UpdateLeadStatus(slId, StatusId);

                ViewBag.Message = strMsg;

            }

            return RedirectToAction("Index");
        }

        public ActionResult AddSalesStatus_Status(int slId, int StatusId)
        {
            string strMsg = "";

            if (db.SalesStatus.Where(s => s.SalesLeadId == slId && 
                s.SalesStatusCodeId == StatusId && s.SalesStatusStatusId == 1).FirstOrDefault() == null)
            {
                strMsg = UpdateLeadStatus(slId, StatusId);

                ViewBag.Message = strMsg;
            }

            return RedirectToAction("Status");
        }

        //POST: SalesLeads/PostLeadStatus
        [HttpPost]
        public bool PostLeadStatus(int slId, int StatusId)
        {
            string strMsg = "";

            if (db.SalesStatus.Where(s => s.SalesLeadId == slId && 
                s.SalesStatusCodeId == StatusId && s.SalesStatusStatusId == 1).FirstOrDefault() == null)
            {
                strMsg = UpdateLeadStatus(slId, StatusId);
            }

            if (strMsg == "Success")
            {
                return true;
            }

            return false;
        }

        public string UpdateLeadStatus(int slId, int StatusId)
        {
            string strMsg;
            string defaultStatusId = "1";
            try
            {
                //db.Database.ExecuteSqlCommand(@"
                //    Insert into SalesStatus([DtStatus],[SalesStatusCodeId],[SalesLeadId],[SalesStatusStatusId])
                //    Values('" + date.GetCurrentDateTime().ToString("MM/dd/yyyy HH:mm:ss") + "','" + StatusId + "','" + slId.ToString() + "','"+ defaultStatusId + @"');
                //    ");

                //db.SaveChanges();


                var salesStatus = new SalesStatus();
                salesStatus.DtStatus = date.GetCurrentDateTime();
                salesStatus.SalesLeadId = slId;
                salesStatus.SalesStatusCodeId = StatusId;
                salesStatus.SalesStatusStatusId = 1;

                db.SalesStatus.Add(salesStatus);
                db.SaveChanges();

                strMsg = "Success";

                switch (StatusId)
                {
                    case 1: //New Leads
                        Session["SLFilterID"] = 1;
                        break;
                    case 2:
                        Session["SLFilterID"] = 2;
                        break;
                    case 3:
                        Session["SLFilterID"] = 3;
                        break;
                    case 4:
                        Session["SLFilterID"] = 4;
                        break;
                    case 5:
                        Session["SLFilterID"] = 5;
                        break;
                    case 6:
                        Session["SLFilterID"] = 6;
                        break;
                    case 7:
                        Session["SLFilterID"] = 7;
                        break;
                    case 8:
                        Session["SLFilterID"] = 8;
                        break;
                    case 15:
                        //Approved by Aldrin
                        Session["SLFilterID"] = 15;
                        RevolveForApprove(slId);
                        break;
                    case 16:
                        //Approved by Mario
                        Session["SLFilterID"] = 16;
                        RevolveForApprove(slId);
                        break;
                    default:
                        break;  //SLFilterID = 3 (All)
                }
            }
            catch (Exception Ex)
            {
                strMsg = "Error:" + Ex.Message;
            }

            return strMsg;
        }

        private void RevolveForApprove(int id)
        {
            var salesLead = db.SalesLeads.Find(id);

            var price = salesLead.Price;

            var ApprovedCount = 0;

            var allowedUsers= db.SalesStatusRestrictions.ToList().Select(s=>s.SalesStatusCodeId);
            var leadStatus = salesLead.SalesStatus.Where(s => allowedUsers.Contains(s.SalesStatusCodeId)).ToList();

            var lastActivityType= sldb.GetLastActivityType(id);
            string Bidding = "Bidding Only";
            string BuyingInquiry = "Buying Inquiry";
            string FirmInquiry = "Firm Inquiry";

            if (leadStatus.Count() > 0)
            {
                foreach (var approved in leadStatus)
                {
                    ApprovedCount++;
                }
            }

            decimal ThreeMil = (decimal)3000000;
            decimal OneMil = (decimal)1000000;

            //price filter
            if ((price >= OneMil && price <= ThreeMil && lastActivityType == Bidding) ||
                (price < OneMil && lastActivityType == FirmInquiry || lastActivityType == BuyingInquiry))
            {

                if (ApprovedCount == 1)
                {
                    //mark as approved
                    UpdateLeadStatus(id, 5);
                }
            }

            if ((price >= ThreeMil) ||
                (price >= OneMil && price <= ThreeMil && (lastActivityType == FirmInquiry || lastActivityType == BuyingInquiry)))
            {
                if (ApprovedCount == 2)
                {
                    //mark as approved
                    UpdateLeadStatus(id, 5);
                }
            }

        }

        public string GetLeadStatusCount(int statusId)
        {
            try
            {

                var salesLeadCount = sldb.GetSalesLeads((int)statusId).Count();

                if (salesLeadCount > 0)
                {
                    return salesLeadCount.ToString();
                }

            return "";

            }
            catch
            {
                return "";
            }

        }


        public string GetSalesLeadCount(int statusId)
        {
            try
            {

                var salesLeadCount = sldb.GetSalesLeads((int)statusId).Count();

                if (salesLeadCount > 0)
                {
                    return salesLeadCount.ToString();
                }

                return "0";

            }
            catch
            {
                return "0";
            }

        }

        //GET: SalesLeads/ForApproval
        public ActionResult ForApproval()
        {
            return RedirectToAction("Index","SalesLeads", new { sortid = 4 });
        }

        //GET: SalesLeads/GetLastestActivityType 
        public string GetLastestActivityType(int id)
        {
            var salesLead = db.SalesLeads.Find(id);

            var lastActivity = db.CustEntActivities.Where(s => s.SalesCode == salesLead.SalesCode);

            if (lastActivity.FirstOrDefault() != null)
            {
                var activity = lastActivity.OrderByDescending(s=>s.Id).FirstOrDefault();
              
                    string activityStatus = activity.CustEntActStatu.Status;
                    activityStatus = activityStatus + " - " + activity.Type;

                    return activityStatus;
            }

            return "";
        }

        //POST : SalesLead/Revision/{id}
        //id = SalesLead ID
        public ActionResult Revision(int id)
        {

            //get salesLead
            var salesLead = db.SalesLeads.Find(id);

            //get all status 
            var leadstatus = salesLead.SalesStatus.Where(s => s.SalesStatusCodeId < 17).ToList();

            leadstatus.ForEach(s => {
                s.SalesStatusStatusId = 2; //inactive for revision
            });


            foreach (var status in leadstatus)
            {
                db.Entry(status).State = EntityState.Modified;
            }
            //db.Entry(leadstatus).State = EntityState.Modified;
            db.SaveChanges();


            AddSalesStatus(salesLead.Id, 1);    //NEW Lead Status

            return RedirectToAction("Index", new { sortid = 1, leadid = salesLead.Id });
        }


        public bool UpdateLeadActivityStatus(int id, string status)
        {
            //get sales lead
            try
            {

                var salesLead = db.SalesLeads.Find(id);

                //add sales lead activity to update status
                CustEntActivity activity = new CustEntActivity();
                activity.Date = date.GetCurrentDateTime();
                activity.Assigned = HttpContext.User.Identity.Name;
                activity.SalesCode = salesLead.SalesCode;
                activity.ProjectName = salesLead.Details;
                activity.Amount = salesLead.Price;
                activity.Status = "Open";
                activity.Remarks = status;
                activity.Type = status;
                activity.ActivityType = "Status Update";
                activity.SalesLeadId = id;
                activity.CustEntActStatusId = 1;
                activity.CustEntActActionStatusId = 1;
                activity.CustEntActActionCodesId = 1;

                if (salesLead.SalesLeadCompanies.FirstOrDefault() != null) {
                    activity.CustEntMainId = salesLead.SalesLeadCompanies.FirstOrDefault().CustEntMainId;
                }

                db.CustEntActivities.Add(activity);
                db.SaveChanges();
                return true;

            }
            catch 
            {
                return false;
            }

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
            data.DtActivity = date.GetCurrentDateTime();
            data.SalesActStatusId = (int)db.SalesActCodes.Find(ActCodeId).DefaultActStatus;
            data.DtEntered = date.GetCurrentDateTime();
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

        #region Customer Activity 
        public bool UpdateCustActivities(int salesLeadId, string salesCode)
        {
            try
            {

                //find all customer activites with the same sales code
                var custActivities = db.CustEntActivities.Where(c => c.SalesCode == salesCode).ToList();

                //update salesLeadId
                foreach (var act in custActivities)
                {
                    act.SalesLeadId = salesLeadId;

                    db.Entry(act).State = EntityState.Modified;
                    db.SaveChanges();

                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public ActionResult ListCustActivityCodes(int? id, string salesCode, string projectName, decimal amount, int? companyId)
        {
            if (id != null || companyId != null || !salesCode.IsNullOrWhiteSpace())
            {
                var actActionCodes = db.CustEntActActionCodes.ToList();
                ViewBag.SalesLeadId = id;
                ViewBag.SalesCode = salesCode;
                ViewBag.ProjectName = projectName;
                ViewBag.Amount = amount;
                ViewBag.CompanyId = companyId;

                return View(actActionCodes);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
        public ActionResult AddCustActivityCode(int? slId, int ActCodeId, string salesCode, string projectName, decimal amount, int? companyId)
        {
            try
            {
                if (slId != null || companyId != null || !salesCode.IsNullOrWhiteSpace())
                {

                    var actCodeDefault = db.CustEntActActionCodes.Find(ActCodeId);

                    CustEntActivity activity = new CustEntActivity();
                    activity.Date = date.GetCurrentDateTime();
                    activity.Assigned = User.Identity.Name;
                    activity.SalesCode = salesCode;
                    activity.ProjectName = projectName;
                    activity.Amount = amount;
                    activity.SalesLeadId = slId;

                    if (actCodeDefault != null)
                    {
                        activity.Remarks = actCodeDefault.Desc;
                    }

                    ViewBag.Assigned = new SelectList(dbclasses.getUsers_wdException(), "UserName", "UserName");
                    ViewBag.CustEntMainId = new SelectList(db.CustEntMains, "Id", "Name", companyId);
                    ViewBag.Status = new SelectList(db.CustEntActStatus, "Status", "Status");
                    ViewBag.Type = new SelectList(db.CustEntActTypes, "Type", "Type");
                    ViewBag.ActivityType = new SelectList(db.CustEntActivityTypes, "Type", "Type");

                    ViewBag.CustEntActStatusId = new SelectList(db.CustEntActStatus, "Id", "Status");
                    ViewBag.CustEntActActionStatusId = new SelectList(db.CustEntActActionStatus, "Id", "ActionStatus", actCodeDefault.DefaultActStatus);
                    ViewBag.CustEntActActionCodesId = new SelectList(db.CustEntActActionCodes, "Id", "Name", ActCodeId);

                    ViewBag.Id = slId;

                    return View(activity);
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddCustActivityCode([Bind(Include = "Id,Date,Assigned,ProjectName,SalesCode,Amount,Status,Remarks,CustEntMainId,Type,ActivityType,CustEntActStatusId,CustEntActActionStatusId,CustEntActActionCodesId")] CustEntActivity custEntActivity)
        {
            if (ModelState.IsValid)
            {
                custEntActivity.Amount = Decimal.Parse(custEntActivity.Amount.ToString());
                db.CustEntActivities.Add(custEntActivity);
                db.SaveChanges();
                return RedirectToAction("Index", new { id = custEntActivity.SalesLeadId });
            }

            ViewBag.Assigned = new SelectList(dbclasses.getUsers_wdException(), "UserName", "UserName", custEntActivity.Assigned);
            ViewBag.CustEntMainId = new SelectList(db.CustEntMains, "Id", "Name", custEntActivity.CustEntMainId);
            ViewBag.Status = new SelectList(db.CustEntActStatus, "Status", "Status", custEntActivity.Status);
            ViewBag.Type = new SelectList(db.CustEntActTypes, "Type", "Type", custEntActivity.Type);
            ViewBag.ActivityType = new SelectList(db.CustEntActivityTypes, "Type", "Type", custEntActivity.ActivityType);

            ViewBag.CustEntActStatusId = new SelectList(db.CustEntActStatus, "Id", "Status", custEntActivity.CustEntActStatusId);
            ViewBag.CustEntActActionStatusId = new SelectList(db.CustEntActActionStatus, "Id", "ActionStatus", custEntActivity.CustEntActActionStatusId);
            ViewBag.CustEntActActionCodesId = new SelectList(db.CustEntActActionCodes, "Id", "Name", custEntActivity.CustEntActActionCodesId);

            ViewBag.Id = custEntActivity.SalesLeadId;

            return View(custEntActivity);
        }

        public ActionResult CustActivityDone(int id)
        {
            //db.Database.ExecuteSqlCommand("update SalesActivities set SalesActStatusId=2 where Id=" + id);
            //var slid = db.SalesActivities.Where(s => s.Id == id).FirstOrDefault().SalesLeadId;

            var custAct = db.CustEntActivities.Find(id);
            custAct.CustEntActActionStatusId = 2;

            db.Entry(custAct).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Index", new { leadId = custAct.SalesLeadId });
        }

        [HttpPost]
        public bool PostCustActivityDone(int id)
        {
            try
            {
                var custAct = db.CustEntActivities.Find(id);
                custAct.CustEntActActionStatusId = 2;

                db.Entry(custAct).State = EntityState.Modified;
                db.SaveChanges();
                return true;
            }
            catch
            {

                return false;
            }

        }


        public ActionResult CustActivityRemove(int id)
        {
            //var slid = db.SalesActivities.Where(s => s.Id == id).FirstOrDefault().SalesLeadId;
            //db.Database.ExecuteSqlCommand("DELETE FROM SalesActivities where Id=" + id);

            var custAct = db.CustEntActivities.Find(id);

            custAct.CustEntActActionStatusId = 3;

            db.Entry(custAct).State = EntityState.Modified;
            db.SaveChanges();

            //db.CustEntActivities.Remove(custAct);
            //db.SaveChanges();

            return RedirectToAction("Index", new { leadId = custAct.SalesLeadId });
        }


        [HttpPost]
        public bool PostCustActivityRemove(int id)
        {
            try
            {
                var custAct = db.CustEntActivities.Find(id);

                custAct.CustEntActActionStatusId = 3;

                db.Entry(custAct).State = EntityState.Modified;
                db.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }

        }

        public ActionResult CustActivitiesPartial(string salesCode)
        {
            var activities = db.CustEntActivities.Where(c => c.SalesCode == salesCode).ToList();

            return View(activities);
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

        #region Items
        public void AddSupItemPartial()
        {
            var items = db.InvItems.ToList();
            ViewBag.InvItems = items;
        }

        public string addSupItem(int SalesLeadId, int ItemId, decimal price, string remarks)
        {
            SalesLeadItems item = new SalesLeadItems();
            item.InvItemId = ItemId;
            item.SalesLeadId = SalesLeadId;
            item.QuotedPrice = price;
            item.Remarks = remarks;

            db.SalesLeadItems.Add(item);
            db.SaveChanges();

            return "200";
        }

        public string getItemSuppliers(int id)
        {
            //get list of suppliers of the given item
            var supplier = db.SupplierInvItems.Where(s => s.InvItemId.Equals(id)).ToList();
            List<cItemSupplier> itemSupDetails = new List<cItemSupplier>();

            foreach (var sup in supplier)
            {
                var itemRates = sup.SupplierItemRates.ToList();

                foreach (var rates in itemRates)
                {
                    itemSupDetails.Add(new cItemSupplier
                    {
                        Id = rates.Id,
                        Rate = rates.ItemRate,
                        Particulars = rates.Particulars,
                        Materials = rates.Material,
                        SupplierName = sup.Supplier.Name,
                        Unit = rates.SupplierUnit.Unit,
                        SupRateId = sup.InvItemId.ToString(),
                        ValidStart = rates.DtValidFrom,
                        ValidEnd = rates.DtValidTo
                    });

                }
            }

            //convert list to json object
            return JsonConvert.SerializeObject(itemSupDetails, Formatting.Indented);
      
        }

        public string AddSupItemRate(int SalesLeadId, int ItemRateId)
        {
            if (SalesLeadId != 0)
            {
                SalesLeadQuotedItem leaditemRate = new SalesLeadQuotedItem();
                leaditemRate.SalesLeadItemsId = SalesLeadId;
                leaditemRate.SupplierItemRateId = ItemRateId;
                leaditemRate.SalesLeadQuotedItemStatusId = 1; //default pending

                db.SalesLeadQuotedItems.Add(leaditemRate);
                db.SaveChanges();
                return "200";
            }
            return "500";
        }


        public string RemoveSupItemRate(int id)
        {
            try
            {
                SalesLeadQuotedItem leaditemRate = db.SalesLeadQuotedItems.Find(id);
                db.SalesLeadQuotedItems.Remove(leaditemRate);
                db.SaveChanges();

                return "OK";
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }

        public string RemoveItem(int? id)
        {
            try
            {
                var result = "";
                if (id != null)
                {
                    SalesLeadItems leadItem = db.SalesLeadItems.Find(id);

                    //try to remove children items from the list
                    result = removeSubItems(leadItem);

                    //remove parent item
                    db.SalesLeadItems.Remove(leadItem);
                    db.SaveChanges();
                }
                return result;
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }

        public string removeSubItems(SalesLeadItems items)
        {
            try
            {
                //remove al items from the list
                var leaditems = items.SalesLeadQuotedItems.ToList();
                foreach (var item in leaditems)
                {
                    RemoveSupItemRate(item.Id);
                }

                return "";
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }

        public string EditItem(int id, int invItemId, decimal rate, string remarks )
        {
            try
            {
                var leaditems = db.SalesLeadItems.Find(id);
                leaditems.InvItemId = invItemId;
                leaditems.QuotedPrice = rate;
                leaditems.Remarks = remarks;

                db.Entry(leaditems).State = EntityState.Modified;
                db.SaveChanges();

                return "OK";
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }

        public string GetSalesStatusAllowedUser(int statusCodeId)
        {
            var user = HttpContext.User.Identity.Name;

            var restriction = db.SalesStatusRestrictions.Where(s => s.SalesStatusCodeId == statusCodeId).ToList();

            if (restriction.Count() > 0)
            {
                if (restriction.Where(s=> s.SalesStatusAllowedUser.User == user).Count() > 0)
                {
                    return "True";
                }
            }
            return "False";
        }

        #endregion


    }
}
