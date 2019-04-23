using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using JobsV1.Models;

namespace JobsV1.Controllers
{

    public class InvItemsController : Controller
    {
        private JobDBContainer db = new JobDBContainer();

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
            return View();
        }

        // POST: InvItems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ItemCode,Description,Remarks,ContactInfo,ImgPath,ViewLabel,OrderNo")] InvItem invItem)
        {
            if (ModelState.IsValid)
            {
                db.InvItems.Add(invItem);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(invItem);
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
            if (ModelState.IsValid)
            {
                db.Entry(invItem).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(invItem);
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

        #region Availability
        public ActionResult Availability() {
            DBClasses dbclass = new DBClasses();
            Models.getItemSchedReturn gret = dbclass.ItemSchedules();
            ViewBag.dtLabel = gret.dLabel;
            
            return View(gret.ItemSched);
        }
        #endregion
    }
}
