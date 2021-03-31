using JobsV1.Models;
using JobsV1.Models.Class;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
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
        public ActionResult Index(int? id, int? sortid, int? leadId)
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

            //get salesl eads leads
            var salesLeads = sldb.GetcProcLeads((int)sortid);

            ViewBag.LeadId = id;
            ViewBag.CurrentFilter = sortid;
            ViewBag.StatusCodes = db.SalesStatusCodes
                .Where(s => s.SalesStatusTypeId == 1 || s.SalesStatusTypeId == 3)
                .OrderBy(s => s.SeqNo).ThenBy(s=>s.Id).ToList();
            ViewBag.UnitList = db.SupplierUnits.ToList();
            ViewBag.Suppliers = db.Suppliers.Where(s => s.Status != "INC").OrderBy(s => s.Name).ToList();
            ViewBag.Items = db.InvItems.ToList();
            ViewBag.User = HttpContext.User.Identity.Name;

            //for adding new item 
            AddSupItemPartial();

            return View(salesLeads.OrderByDescending(s => s.Date));
        }

        //Partial View: /Procurement/AddSupItemPartial
        public void AddSupItemPartial()
        {
            var items = db.InvItems.ToList();
            ViewBag.InvItems = items;
        }


        //GET: /Procurement/ProcActivitiesPartial
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


        //GET: SalesLeads/GetLastestActivityType 
        public string GetLastestActivityType(int id)
        {
            var salesLead = db.SalesLeads.Find(id);

            var lastActivity = db.SupplierActivities.Where(s => s.Code == salesLead.SalesCode);

            if (lastActivity.FirstOrDefault() != null)
            {
                var activity = lastActivity.OrderByDescending(s => s.Id).FirstOrDefault();

                string activityStatus = activity.SupplierActStatu.Status;
                activityStatus = activityStatus + " - " + activity.Type;

                return activityStatus;
            }

            return "";
        }


        #region Procurement Activity 

        //POST: /Procurement/UpdateProcActivities
        public bool UpdateProcActivities(int salesLeadId, string salesCode)
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


        //GET: /Procurement/ListProcActivityCodes
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

        //GET: /Procurement/AddProcActivityCode
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
                    if (actCodeDefault != null)
                    {
                        activity.Remarks = actCodeDefault.Desc;
                    }

                    ViewBag.Assigned = new SelectList(dbclasses.getUsers_wdException(), "UserName", "UserName");
                    ViewBag.SupplierId = new SelectList(db.Suppliers.OrderBy(s=>s.Name), "Id", "Name");
                    ViewBag.SupplierType = new SelectList(db.SupplierTypes, "Id", "Description");
                    ViewBag.Type = new SelectList(db.CustEntActTypes, "Type", "Type");
                    ViewBag.ActivityType = new SelectList(db.SupplierActivityTypes, "Type", "Type");

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

        //POST: /Procurement/AddProcActivityCode
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddProcActivityCode([Bind(Include = "Id,Code,DtActivity,Assigned,Amount,Currency,Remarks,SupplierId,Amount,Type,ActivityType,SupplierActStatusId,ProjName,SupplierActActionCodeId")] SupplierActivity supplierActivity, int? slId, int? ActCodeId)
        {
            if (ModelState.IsValid)
            {
                supplierActivity.SupplierActActionStatusId = 1; // default 
                supplierActivity.Amount = Decimal.Parse(supplierActivity.Amount.ToString());

                db.SupplierActivities.Add(supplierActivity);
                db.SaveChanges();

                //add link to sales lead ang suppliers
                if (slId != null)
                {
                    SalesLeadSupActivity leadSupActivity = new SalesLeadSupActivity();

                    leadSupActivity.SupplierActivityId = supplierActivity.Id;
                    leadSupActivity.SalesLeadId = (int)slId;

                    db.SalesLeadSupActivities.Add(leadSupActivity);
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

        //POST : Procurement/ProcActivityDone
        [HttpPost]
        public bool ProcActivityDone(int id)
        {
            try
            {
                var supAct = db.SupplierActivities.Find(id);
                supAct.SupplierActActionStatusId = 2;

                db.Entry(supAct).State = EntityState.Modified;
                db.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
          
        }

        //POST : Procurement/ProcActivityRemove
        [HttpPost]
        public bool ProcActivityRemove(int id)
        {
            try
            {

                var custAct = db.SupplierActivities.Find(id);

                custAct.SupplierActActionStatusId = 3;

                db.Entry(custAct).State = EntityState.Modified;
                db.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion

        #region Suplier Items Rate
        //POST : Procurement/CreateSupplierItem
        public bool CreateSupplierItem(int salesLeadItemId, int supplierId, int itemId, string particulars, string materials, decimal rate,
            int unitTypeId, string tradeTerm, string tolerance, string remarks, DateTime validTo, DateTime validFrom,
            string procuredBy, string offeredBy )
        {
            try
            {
                //create supplierItem
                SupplierItemRate supplierItem = new SupplierItemRate {
                    ItemRate = rate.ToString(),
                    SupplierUnitId = unitTypeId,
                    Remarks = remarks ?? " ",
                    DtValidFrom = validFrom.ToShortDateString(),
                    DtValidTo = validTo.ToShortDateString(),
                    Material = materials ?? " ",
                    ProcBy = procuredBy ?? "N/A",
                    TradeTerm = tradeTerm,
                    Tolerance = tolerance,
                    DtEntered = date.GetCurrentDateTime().ToString(),
                    By = offeredBy ?? "N/A",
                    Particulars = particulars ?? "N/A"

                };

                if (!HasSupplierItem(supplierId, itemId))
                {
                    //create link to supplier and invetory item
                    var supInvItemId = CreateSupplierInvItem(supplierId, itemId);
                    supplierItem.SupplierInvItemId = supInvItemId;

                }
                else
                {
                    var supInvItemId = GetSupplierInvItemId(supplierId, itemId);
                    if (supInvItemId != 0 )
                    {
                        supplierItem.SupplierInvItemId = supInvItemId;
                    }
                    else
                    {
                        return false;
                    }
                }

                //create supplier item rate
                var itemRateId = CreateSupplierItemRate(supplierItem);
                if (itemRateId == 0)
                {
                    return false;
                }

                //link to sales lead
                CreateSalesLeadItemLink(salesLeadItemId, itemRateId);

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
                return false;   
            }
        }


        private int GetSupplierInvItemId(int supplierId, int itemId)
        {
            //check if supplier has item rate

            var supInvItem = db.SupplierInvItems.Where(s => s.SupplierId == supplierId
                && s.InvItemId == itemId);

            if (supInvItem.Count() != 0)
            {
                return supInvItem.FirstOrDefault().Id;
            }

            return 0;
        }


        private bool HasSupplierItem(int supplierId, int itemId)
        {
            //check if supplier has item rate
            var supplier = db.Suppliers.Find(supplierId);

            if (db.SupplierInvItems.Where(s => s.SupplierId == supplierId
                && s.InvItemId == itemId).Count() != 0)
            {
                return true;
            }

            return false;
        }

        private int CreateSupplierInvItem(int supplierId, int itemId)
        {
            SupplierInvItem supInvItem = new SupplierInvItem();
            supInvItem.SupplierId = supplierId;
            supInvItem.InvItemId = itemId;

            db.SupplierInvItems.Add(supInvItem);
            db.SaveChanges();

            return supInvItem.Id;
        }

        private int CreateSupplierItemRate(SupplierItemRate supItemRate)
        {
            try
            {
                db.SupplierItemRates.Add(supItemRate);
                db.SaveChanges();

                return supItemRate.Id;
            }
            catch 
            {
                return 0;
            }
              
        }


        private bool CreateSalesLeadItemLink(int salesLeadItemId, int supplierItemRateId)
        {
            try
            {
                SalesLeadQuotedItem slQuotedItem = new SalesLeadQuotedItem();
                slQuotedItem.SalesLeadItemsId    = salesLeadItemId;
                slQuotedItem.SupplierItemRateId  = supplierItemRateId;
                slQuotedItem.SalesLeadQuotedItemStatusId = 1; //PENDING DEFAULT

                db.SalesLeadQuotedItems.Add(slQuotedItem);
                db.SaveChanges();

                return true;
            }
            catch(Exception ex)
            {
                throw ex;
                return false;
            }
        }


        //GET : Procurement/GetItemSuppliers
        [HttpGet]
        public string GetItemSuppliers(int id)
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
                        SupplierName = sup.Supplier.Name,
                        Unit = rates.SupplierUnit.Unit,
                        SupRateId = sup.InvItemId.ToString(),
                        ValidStart = rates.DtValidFrom,
                        ValidEnd = rates.DtValidTo,
                        Particulars = rates.Particulars,
                        Materials = rates.Material,
                    });

                }
            }

            //convert list to json object
            return JsonConvert.SerializeObject(itemSupDetails, Formatting.Indented);

        }

        //GET: Procurement/GetSupItemDetails
        // id: ItemRateId
        [HttpGet]
        public JsonResult GetSupItemDetails(int id)
        {
            //get list of suppliers of the given item
            var itemRate = db.SupplierItemRates.Find(id);

            if(itemRate != null) {

                //convert list to json object
                return Json(
                    new {
                        itemRate.Id,
                        itemRate.SupplierUnitId,
                        itemRate.SupplierInvItem.SupplierId,
                        itemRate.Particulars,
                        itemRate.Material,
                        itemRate.ItemRate,
                        itemRate.Remarks,
                        itemRate.Tolerance,
                        itemRate.TradeTerm,
                        itemRate.DtValidFrom,
                        itemRate.DtValidTo,
                        itemRate.ProcBy,
                        itemRate.By,
                        itemRate.SupplierInvItem.InvItem.Description,
                    }, JsonRequestBehavior.AllowGet);
            }

            return null;
        }


        //POST: Procurement/EditSupplierItem
        public bool EditSupplierItem(int supItemId, int supplierId, string particulars, string materials, decimal rate,
            int unitTypeId, string tradeTerm, string tolerance, string remarks, DateTime validTo, DateTime validFrom,
            string procuredBy, string offeredBy)
        {

            //get item
            try
            {
                var supItemRate = db.SupplierItemRates.Find(supItemId);

                if (supItemRate == null)
                {
                    return false;
                }

                supItemRate.SupplierInvItem.SupplierId = supplierId;
                supItemRate.Particulars = particulars ?? "N/A";
                supItemRate.Material = materials ?? "N/A";
                supItemRate.ItemRate = rate.ToString();
                supItemRate.SupplierUnitId = unitTypeId;
                supItemRate.TradeTerm = tradeTerm;
                supItemRate.Tolerance = tolerance;
                supItemRate.Remarks = remarks;
                supItemRate.DtValidTo = validTo.ToString();
                supItemRate.DtValidFrom = validFrom.ToString();
                supItemRate.ProcBy = procuredBy ?? "N/A";
                supItemRate.By = offeredBy ?? "N/A";

                db.Entry(supItemRate).State = EntityState.Modified;
                db.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        // POST : Procurement/UpdateLeadItemStatus
        [HttpPost]
        public bool UpdateLeadItemStatus(int id, int statusId)
        {
            try
            {

                var salesLeadQuotedItem = db.SalesLeadQuotedItems.Find(id);
                salesLeadQuotedItem.SalesLeadQuotedItemStatusId = statusId;

                db.Entry(salesLeadQuotedItem).State = EntityState.Modified;
                db.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion
    }
}