using JobsV1.Models;
using JobsV1.Models.Class;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JobsV1.Controllers
{
    public class ProcurementController : Controller
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

        // GET: Procurement
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
                    sortid = 5;
                    Session["SLFilterID"] = 5;
                }
            }

            //get salesl eads leads
            var salesLeads = sldb.GetSalesLeads((int)sortid);

            ViewBag.LeadId = leadId;
            ViewBag.CurrentFilter = sortid;
            ViewBag.StatusCodes = db.SalesStatusCodes.ToList();

            //for adding new item 
            AddSupItemPartial();

            return View(salesLeads.OrderByDescending(s => s.Date));
        }

        public void AddSupItemPartial()
        {
            var items = db.InvItems.ToList();
            ViewBag.InvItems = items;
        }


        public ActionResult ProcActivitiesPartial(string salesCode)
        {
            var activities = db.CustEntActivities.Where(c => c.SalesCode == salesCode).ToList();

            return View(activities);
        }

        //GET: /Procurement/ListActivityCodes
        public ActionResult ListActivityCodes(int id)
        {
            var data = db.SupplierActActionCodes.ToList();
            ViewBag.SalesLeadId = id;

            return View(data);
        }
        public ActionResult SalesActivityDone(int id)
        {
            db.Database.ExecuteSqlCommand("update SalesActivities set SalesActStatusId=2 where Id=" + id);
            var slid = db.SalesActivities.Where(s => s.Id == id).FirstOrDefault().SalesLeadId;
            return RedirectToAction("Index", new { leadId = slid });
        }

        public ActionResult SalesActivityRemove(int id)
        {
            var slid = db.SalesActivities.Where(s => s.Id == id).FirstOrDefault().SalesLeadId;
            db.Database.ExecuteSqlCommand("DELETE FROM SalesActivities where Id=" + id);
            return RedirectToAction("Index", new { leadId = slid });
        }


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

        public ActionResult ListProcActivityCodes(int? id)
        {

            var salesLead = db.SalesLeads.Find(id);

            if (id != null  || !salesLead.SalesCode.IsNullOrWhiteSpace())
            {
                var actActionCodes = db.SupplierActActionCodes.ToList();
                ViewBag.SalesLeadId = id;
                ViewBag.SalesCode = salesLead.SalesCode;
                ViewBag.ProjectName = salesLead.Details;
                ViewBag.Amount = salesLead.Price;

                return View(actActionCodes);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        public ActionResult AddProcActivityCode(int? slId, int? ActCodeId)
        {
            try
            {
                var salesLead = db.SalesLeads.Find(slId);

                if (slId != null || !salesLead.SalesCode.IsNullOrWhiteSpace())
                {

                    SupplierActivity activity = new SupplierActivity();
                    activity.DtActivity = date.GetCurrentDateTime();
                    activity.Assigned = User.Identity.Name;
                    activity.Code = salesLead.SalesCode;
                    activity.ProjName = salesLead.Details;
                    activity.Amount = salesLead.Price;

                    var actCodeDefault = db.SupplierActActionCodes.Find(ActCodeId);

                    ViewBag.Assigned = new SelectList(dbclasses.getUsers_wdException(), "UserName", "UserName");
                    ViewBag.SupplierId = new SelectList(db.Suppliers, "Id", "Name");
                    ViewBag.SupplierType = new SelectList(db.SupplierTypes, "Id", "Description");

                    ViewBag.SupplierActStatusId = new SelectList(db.CustEntActStatus, "Id", "Status");
                    ViewBag.SupplierActActionStatusId = new SelectList(db.CustEntActActionStatus, "Id", "ActionStatus", actCodeDefault.DefaultActStatus);
                    ViewBag.SupplierActActionCodeId = new SelectList(db.CustEntActActionCodes, "Id", "Name", ActCodeId);

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
        public ActionResult AddProcActivityCode([Bind(Include = "Id,Date,Assigned,ProjectName,SalesCode,Amount,Status,Remarks,CustEntMainId,Type,ActivityType,CustEntActStatusId,CustEntActActionStatusId,CustEntActActionCodesId")] SupplierActivity supplierActivity, int? slId, int? ActCodeId)
        {
            if (ModelState.IsValid)
            {
                supplierActivity.Amount = Decimal.Parse(supplierActivity.Amount.ToString());
                db.SupplierActivities.Add(supplierActivity);
                db.SaveChanges();

                //add link to sales lead ang suppliers
                if (slId != null)
                {
                    SalesLeadSupplierActivity leadSupActivity = new SalesLeadSupplierActivity();

                    leadSupActivity.SupplierActivityId = supplierActivity.Id;
                    leadSupActivity.SalesLeadId = (int)slId;

                    db.SaveChanges();
                }

                return RedirectToAction("Index", new { id = slId });
            }

            ViewBag.Assigned = new SelectList(dbclasses.getUsers_wdException(), "UserName", "UserName");
            ViewBag.SupplierId = new SelectList(db.Suppliers, "Type", "Name");
            ViewBag.SupplierType = new SelectList(db.SupplierTypes, "Type", "Description");

            ViewBag.SupplierActStatusId = new SelectList(db.CustEntActStatus, "Id", "Status");
            ViewBag.SupplierActActionStatusId = new SelectList(db.CustEntActActionStatus, "Id", "ActionStatus", 1);
            ViewBag.SupplierActActionCodeId = new SelectList(db.CustEntActActionCodes, "Id", "Name", ActCodeId);

            ViewBag.Id = slId;

            return View(supplierActivity);
        }

        public ActionResult ProcActivityDone(int id)
        {
  
            var custAct = db.CustEntActivities.Find(id);
            custAct.CustEntActActionStatusId = 2;

            db.Entry(custAct).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Index", new { leadId = custAct.SalesLeadId });
        }

        public ActionResult ProcActivityRemove(int id)
        {
 
            var custAct = db.CustEntActivities.Find(id);

            custAct.CustEntActActionStatusId = 3;

            db.Entry(custAct).State = EntityState.Modified;
            db.SaveChanges();

            //db.CustEntActivities.Remove(custAct);
            //db.SaveChanges();

            return RedirectToAction("Index", new { leadId = custAct.SalesLeadId });
        }

        #endregion

    }
}