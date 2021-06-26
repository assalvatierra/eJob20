using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using JobsV1.Models;
using Microsoft.Ajax.Utilities;
using JobsV1.Models.Class;

namespace JobsV1.Controllers
{

    public class InvItemsController : Controller
    {
        private JobDBContainer db = new JobDBContainer();
        private DateClass dt = new DateClass();

        private MaintenanceServices mtServices = new MaintenanceServices();

        private string SITECONFIG = ConfigurationManager.AppSettings["SiteConfig"].ToString();

        // GET: InvItems
        public ActionResult Index()
        {
            List<InvItemCat> InvCats = db.InvItemCats.ToList();
            ViewBag.CatList = InvCats;

            List<InvItem> ItemList = db.InvItems.Where(d=>d.Remarks == "").ToList();

            List<InvItemsModified> InvListMod = new List<InvItemsModified>();

            foreach (var item in ItemList)
            {
                List<InvItemCategory> itemCats = db.InvItemCategories.Where(i => i.InvItemId == item.Id).ToList();

                InvListMod.Add(new InvItemsModified
                {
                    Id = item.Id,
                    Description = item.Description,
                    ItemCode = item.ItemCode,
                    ImgPath = item.ImgPath,
                    Remarks = item.Remarks,
                    CategoryList = itemCats
                });
            }

            List<Supplier> suppliers = new List<Supplier>();
            if (db.Suppliers.ToList() != null) {
                suppliers = db.Suppliers.ToList();
            } else {
                return RedirectToAction("Index");
            }

            ViewBag.coopList = db.CoopMembers.Where(c=>c.Status == "ACT").ToList();
            
            //include latest odo, coopMembers list
            ViewBag.SupplierList = suppliers;

            ViewBag.SiteConfig = SITECONFIG;

            //inventory items
            var itemList = db.InvItems.Include(s => s.SupplierInvItems)
                .Include(s => s.InvCarRecords)
                .Include(s => s.InvCarGateControls)
                .Include(s => s.CoopMemberItems);

            return View(itemList.OrderBy(s => s.OrderNo).ToList());
        }

        public ActionResult ItemSchedules()
        {
            DBClasses dbclass = new DBClasses();
            Models.getItemSchedReturn gret = dbclass.ItemSchedules();
            ViewBag.dtLabel = gret.dLabel;
            return View(gret.ItemSched);
        }

        public ActionResult ItemList_byServiceId(int serviceId)
        {
            var data = db.JobServiceItems.Where(d => d.JobServicesId == serviceId).Include(j=>j.InvItem).ToList();
            return View(data);
        }
        

        // GET: InvItems/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InvItem invItem = db.InvItems.Find(id);
            if (invItem == null)
            {
                return HttpNotFound();
            }
            return View(invItem);
        }

        // GET: InvItems/Create
        public ActionResult Create()
        {
            InvItem item = new InvItem();
            item.OrderNo = 999;

            return View(item);
        }

        // POST: InvItems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ItemCode,Description,Remarks,ContactInfo,ImgPath,ViewLabel,OrderNo")] InvItem invItem)
        {
            if (ModelState.IsValid && InputValidation(invItem))
            {
                db.InvItems.Add(invItem);
                db.SaveChanges();

                //if (SITECONFIG == "RealWheels")
                //{
                    addDefaultCategory(invItem.Id);
                //}

                return RedirectToAction("Index");
            }

            return View(invItem);
        }

        public void addDefaultCategory(int id)
        {
            try
            {
                db.InvItemCategories.Add(
                     new InvItemCategory
                     {
                         InvItemCatId = 1,
                         InvItemId = id
                     }
                );

                db.SaveChanges();
            }
            catch
            { }
           
        }

        // GET: InvItems/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InvItem invItem = db.InvItems.Find(id);
            if (invItem == null)
            {
                return HttpNotFound();
            }
            return View(invItem);
        }

        // POST: InvItems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ItemCode,Description,Remarks,ContactInfo,ImgPath,ViewLabel,OrderNo")] InvItem invItem)
        {
            if (ModelState.IsValid && InputValidation(invItem))
            {
                db.Entry(invItem).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(invItem);
        }


        public bool InputValidation(InvItem invItem)
        {
            bool isValid = true;

            if (invItem.ItemCode.IsNullOrWhiteSpace())
            {
                ModelState.AddModelError("ItemCode", "Invalid ItemCode");
                isValid = false;
            }

            if (invItem.Description.IsNullOrWhiteSpace())
            {
                ModelState.AddModelError("Description", "Invalid Description");
                isValid = false;
            }

            return isValid;
        }

        // GET: InvItems/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InvItem invItem = db.InvItems.Find(id);
            if (invItem == null)
            {
                return HttpNotFound();
            }
            return View(invItem);
        }

        // POST: InvItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            InvItem invItem = db.InvItems.Find(id);
            db.InvItems.Remove(invItem);
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


        public ActionResult addCategory(int id, int catid) {
            db.InvItemCategories.Add(
                new InvItemCategory {
                    InvItemCatId = catid,
                    InvItemId = id
                }
            );

            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: InvItems/Delete/5
        public ActionResult CatRemove(int id)
        {
            InvItemCategory cat = db.InvItemCategories.Find(id);
            db.InvItemCategories.Remove(cat);
            db.SaveChanges();

            return RedirectToAction("Index", "InvItems", null);
        }

        public ActionResult CatRemove2(int Id)
        {
            InvItemCategory cat = db.InvItemCategories.Find(Id);
            db.InvItemCategories.Remove(cat);
            db.SaveChanges();

            return Json("CatRemove:", JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddSupplier(int id, int supId) {
            SupplierInvItem newSupInv = new SupplierInvItem();
            newSupInv.InvItemId = id;
            newSupInv.SupplierId = supId;

            if (db.SupplierInvItems.Where(s=>s.InvItemId == id).FirstOrDefault() != null)
            {
                newSupInv = db.SupplierInvItems.Where(s => s.InvItemId == id).FirstOrDefault();
                newSupInv.SupplierId = supId;
                //update if record exist
                db.Entry(newSupInv).State = EntityState.Modified;
            }
            else {
                //add if record does not
                db.SupplierInvItems.Add(newSupInv);
            }

            db.SaveChanges();
            return RedirectToAction("Index", "InvItems", null);
        }

        public ActionResult removeSupplier(int id) {

            SupplierInvItem supInv = db.SupplierInvItems.Find(id);
            db.SupplierInvItems.Remove(supInv);
            db.SaveChanges();

            return RedirectToAction("Index", "InvItems", null);
        }

        //CoopMember link
        public ActionResult LinkCoopMember(int id, int memberid)
        {
            CoopMemberItem coopMemItem = new CoopMemberItem();
            coopMemItem.InvItemId = id;
            coopMemItem.CoopMemberId = memberid;
            db.CoopMemberItems.Add(coopMemItem);
            db.SaveChanges();
            return RedirectToAction("Index", "InvItems", null);
        }

        public ActionResult CoopRemove(int id)
        {
            CoopMemberItem coopMemberItem = db.CoopMemberItems.Find(id);
            db.CoopMemberItems.Remove(coopMemberItem);
            db.SaveChanges();

            return RedirectToAction("Index", "InvItems", null);
        }

        [HttpGet]
        public JsonResult GetItemDetails(int? itemid, DateTime? date)
        {
            try
            {

                if (itemid == null || date == null)
                {
                    return null;
                }
                var selectedDate = DateTime.Parse(date.ToString()).Date;

                var endDate = selectedDate.AddDays(1);
                var jsitemIds = db.JobServiceItems.Where(s => s.InvItemId == itemid).Select(s => s.JobServicesId).ToList();
                var jsitems = db.JobServices.Where(s => jsitemIds.Contains(s.Id)).ToList();

                List<ItemDetailsList> data = new List<ItemDetailsList>();

                foreach (var item in jsitems)
                {
                    var jsStartDt = DateTime.Parse(item.DtStart.ToString()).Date;
                    var jsEndDt = DateTime.Parse(item.DtEnd.ToString()).Date;

                    var sDateComp = selectedDate.CompareTo(jsStartDt);
                    var eDateComp = selectedDate.CompareTo(jsEndDt); 

                    if (sDateComp >= 0 && eDateComp <= 0 )
                    {

                        ItemDetailsList tempData = new ItemDetailsList();

                        tempData.JobDescription = item.JobMain.Description;
                        tempData.Id = item.Id;
                        tempData.JobId = item.JobMainId;
                        tempData.Service = item.Service.Name;
                        tempData.ServiceDesc = item.Particulars;
                        tempData.Customer = item.JobMain.Customer.Name;
                        tempData.Company = GetJobCustCompany(item.JobMainId);

                        data.Add(tempData);

                    }
                }

                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }

        public string GetJobCustCompany(int jobId)
        {
            try
            {
                var jobCustEntQuery = db.JobEntMains.Where(j => j.JobMainId == jobId);
                if (jobCustEntQuery.FirstOrDefault() != null)
                {
                    var latestCompany = jobCustEntQuery.OrderByDescending(s=>s.Id).FirstOrDefault();

                    var company = latestCompany.CustEntMain.Name;

                    return company;
                }

                return null;
            }
            catch
            {
                return null;
            }
        }

        #region Maintenance

        public ActionResult Maintenance()
        {
            var vehicles = mtServices.GetMaintenanceVehicles();

            ViewBag.Today = dt.GetCurrentDate();
            ViewBag.RecordTypes = db.InvCarRecordTypes.ToList();
            return View(vehicles);
        }

        public string GetImportOdo()
        {
            var vehicles = mtServices.GetMaintenanceVehicles().Select(v => v.Id).ToList();

            var importRes = mtServices.UpdateOdoFromVehicleList(vehicles);

            return "OK";
        }

        [HttpGet]
        public JsonResult GetUnitRecords(int id)
        {
            var records = mtServices.GetInvCarRecords(id)
                .Select(c => new { 
                    c.Id,
                    RecordType = c.InvCarRecordType.Description,
                    Unit = c.InvItem.Description,
                    c.InvItem.ItemCode,
                    dtDone = c.dtDone.ToString("MMM dd yyyy"),
                    c.Odometer,
                    c.Remarks
                });

            return Json(records, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Availability
        public ActionResult Availability() {
            DBClasses dbclass = new DBClasses();
            Models.getItemSchedReturn gret = dbclass.ItemSchedules();
            ViewBag.dtLabel = gret.dLabel;
            
            return View(gret.ItemSched);
        }


        public ActionResult AvailabilityTime()
        {
            DBClasses dbclass = new DBClasses();
            Models.getItemSchedReturn gret = dbclass.ItemSchedulesByHour();
            ViewBag.dtLabel = gret.dLabel;

            return View(gret.ItemSched);
        }
        #endregion

      
    }
}


public class ItemDetailsList
{
    public int Id { get; set; }
    public string JobDescription { get; set; }
    public string Customer { get; set; }
    public int JobId { get; set; }
    public string Service { get; set; }
    public string ServiceDesc { get; set; }
    public string Company { get; set; }
}
