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
    public class SupplierActivitiesController : Controller
    {
        private JobDBContainer db = new JobDBContainer();
        private DBClasses dbclasses = new DBClasses();
        private SupplierClass supdb = new SupplierClass();
        private DateClass date = new DateClass();

        private List<SelectListItem> ActivityType = new List<SelectListItem> {
                new SelectListItem { Value = "Meeting", Text = "Meeting" },
                new SelectListItem { Value = "Quotation", Text = "Quotation" },
                new SelectListItem { Value = "Sales", Text = "Sales" }
                };

        // GET: SupplierActivities/{index}
        public ActionResult Index()
        {
            var supplierActivities = db.SupplierActivities.Include(s => s.Supplier);
            return View(supplierActivities.OrderByDescending(s=>s.DtActivity).ToList());
        }

        // GET: SupplierActivities/{index}
        public ActionResult Records(int id)
        {
            var supplierActivities = db.SupplierActivities.Where(s=>s.SupplierId == id).Include(s => s.Supplier).ToList();

            ViewBag.SupplierName = db.Suppliers.Find(id).Name;
            ViewBag.Id = id;

            foreach (var act in supplierActivities)
            {
                act.Assigned = supdb.removeEmailString(act.Assigned);
            }

            return View(supplierActivities);
        }

        // GET: SupplierActivities/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SupplierActivity supplierActivity = db.SupplierActivities.Find(id);
            if (supplierActivity == null)
            {
                return HttpNotFound();
            }
            return View(supplierActivity);
        }

        // GET: SupplierActivities/Create
        public ActionResult Create()
        {
            ViewBag.Assigned = new SelectList(dbclasses.getUsers_wdException(), "UserName", "UserName");
            ViewBag.SupplierId = new SelectList(db.Suppliers, "Id", "Name");
            return View();
        }

        // POST: SupplierActivities/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Code,DtActivity,Assigned,Remarks,SupplierId,Amount")] SupplierActivity supplierActivity)
        {
            if (ModelState.IsValid)
            {
                supplierActivity.Type = "Meeting"; // default for all supplier activity history
                db.SupplierActivities.Add(supplierActivity);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Assigned = new SelectList(dbclasses.getUsers_wdException(), "UserName", "UserName", supplierActivity.Assigned);
            ViewBag.SupplierId = new SelectList(db.Suppliers, "Id", "Name", supplierActivity.SupplierId);
            return View(supplierActivity);
        }

        // GET: SupplierActivities/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SupplierActivity supplierActivity = db.SupplierActivities.Find(id);
            if (supplierActivity == null)
            {
                return HttpNotFound();
            }
            ViewBag.Assigned = new SelectList(dbclasses.getUsers_wdException(), "UserName", "UserName", supplierActivity.Assigned);
            ViewBag.SupplierId = new SelectList(db.Suppliers, "Id", "Name", supplierActivity.SupplierId);
            ViewBag.Id = supplierActivity.SupplierId;
            return View(supplierActivity);
        }

        // POST: SupplierActivities/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Code,DtActivity,Assigned,Remarks,SupplierId,Amount")] SupplierActivity supplierActivity)
        {
            if (ModelState.IsValid)
            {
                db.Entry(supplierActivity).State = EntityState.Modified;
                db.SaveChanges();
                //return RedirectToAction("Index");
                return RedirectToAction("Records", new { id = supplierActivity.SupplierId });
            }
            ViewBag.Assigned = new SelectList(dbclasses.getUsers_wdException(), "UserName", "UserName", supplierActivity.Assigned);
            ViewBag.SupplierId = new SelectList(db.Suppliers, "Id", "Name", supplierActivity.SupplierId);
            return View(supplierActivity);
        }

        // GET: SupplierActivities/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SupplierActivity supplierActivity = db.SupplierActivities.Find(id);
            if (supplierActivity == null)
            {
                return HttpNotFound();
            }
            ViewBag.Id = supplierActivity.SupplierId;
            return View(supplierActivity);
        }

        // POST: SupplierActivities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SupplierActivity supplierActivity = db.SupplierActivities.Find(id);
            db.SupplierActivities.Remove(supplierActivity);
            db.SaveChanges();
            //return RedirectToAction("Index");
            return RedirectToAction("Records", new { id = supplierActivity.SupplierId });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        // GET: SupplierActivities/Create
        public ActionResult RecordsCreate(int id)
        {
            ViewBag.Assigned = new SelectList(dbclasses.getUsers_wdException(), "UserName", "UserName");
            ViewBag.SupplierId = new SelectList(db.Suppliers, "Id", "Name", id);
            ViewBag.Type = new SelectList(db.CustEntActTypes, "Type", "Type");
            ViewBag.ActivityType = new SelectList(db.SupplierActivityTypes, "Type", "Type");

            SupplierActivity supAct = new SupplierActivity();
            supAct.DtActivity = date.GetCurrentDateTime();

            return View(supAct);
        }

        // POST: SupplierActivities/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RecordsCreate([Bind(Include = "Id,Code,DtActivity,Assigned,Remarks,SupplierId,Amount,Type,ActivityType")] SupplierActivity supplierActivity)
        {
            if (ModelState.IsValid)
            {
                db.SupplierActivities.Add(supplierActivity);
                db.SaveChanges();
                return RedirectToAction("Records",new { id = supplierActivity.SupplierId });
            }

            ViewBag.Assigned = new SelectList(dbclasses.getUsers_wdException(), "UserName", "UserName", supplierActivity.Assigned);
            ViewBag.SupplierId = new SelectList(db.Suppliers, "Id", "Name", supplierActivity.SupplierId);
            ViewBag.Type = new SelectList(db.CustEntActTypes, "Type", "Type", supplierActivity.Type);
            ViewBag.ActivityType = new SelectList(db.SupplierActivityTypes, "Type", "Type", supplierActivity.ActivityType);
            return View(supplierActivity);
        }


        // GET: SupplierActivities/Edit/5
        public ActionResult RecordsEdit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SupplierActivity supplierActivity = db.SupplierActivities.Find(id);
            if (supplierActivity == null)
            {
                return HttpNotFound();
            }
            ViewBag.Assigned = new SelectList(dbclasses.getUsers_wdException(), "UserName", "UserName", supplierActivity.Assigned);
            ViewBag.SupplierId = new SelectList(db.Suppliers, "Id", "Name", supplierActivity.SupplierId);
            ViewBag.Type = new SelectList(db.CustEntActTypes, "Type", "Type", supplierActivity.Type);
            ViewBag.ActivityType = new SelectList(db.SupplierActivityTypes, "Type", "Type", supplierActivity.ActivityType);
            ViewBag.Id = supplierActivity.SupplierId;
            return View(supplierActivity);
        }

        // POST: SupplierActivities/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RecordsEdit([Bind(Include = "Id,Code,DtActivity,Assigned,Remarks,SupplierId,Amount,Type,ActivityType")] SupplierActivity supplierActivity)
        {
            if (ModelState.IsValid)
            {
                db.Entry(supplierActivity).State = EntityState.Modified;
                db.SaveChanges();
                //return RedirectToAction("Index");
                return RedirectToAction("Records", new { id = supplierActivity.SupplierId });
            }
            ViewBag.Assigned = new SelectList(dbclasses.getUsers_wdException(), "UserName", "UserName", supplierActivity.Assigned);
            ViewBag.SupplierId = new SelectList(db.Suppliers, "Id", "Name", supplierActivity.SupplierId);
            ViewBag.Type = new SelectList(db.CustEntActTypes, "Type", "Type", supplierActivity.Type);
            ViewBag.ActivityType = new SelectList(db.SupplierActivityTypes, "Type", "Type", supplierActivity.ActivityType);
            return View(supplierActivity);
        }
    }
}
