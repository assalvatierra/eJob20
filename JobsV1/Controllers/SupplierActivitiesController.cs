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
        

        // GET: SupplierActivities/{index}
        public ActionResult Index()
        {
            var supplierActivities = db.SupplierActivities.Include(s => s.Supplier);
            return View(supplierActivities.ToList());
        }

        // GET: SupplierActivities/{index}
        public ActionResult Records(int id)
        {
            var supplierActivities = db.SupplierActivities.Include(s => s.Supplier);

            ViewBag.SupplierName = db.Suppliers.Find(id).Name;
            ViewBag.Id = id;

            return View(supplierActivities.ToList());
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
            ViewBag.SupplierId = new SelectList(db.Suppliers, "Id", "Name");
            return View();
        }

        // POST: SupplierActivities/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Activity,Code,DtActivity,Assigned,Remarks,SupplierId")] SupplierActivity supplierActivity)
        {
            if (ModelState.IsValid)
            {
                db.SupplierActivities.Add(supplierActivity);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

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
            ViewBag.SupplierId = new SelectList(db.Suppliers, "Id", "Name", supplierActivity.SupplierId);
            return View(supplierActivity);
        }

        // POST: SupplierActivities/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Activity,Code,DtActivity,Assigned,Remarks,SupplierId")] SupplierActivity supplierActivity)
        {
            if (ModelState.IsValid)
            {
                db.Entry(supplierActivity).State = EntityState.Modified;
                db.SaveChanges();
                //return RedirectToAction("Index");
                return RedirectToAction("Records", new { id = supplierActivity.SupplierId });
            }
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
            ViewBag.SupplierId = new SelectList(db.Suppliers, "Id", "Name", id);
            return View();
        }

        // POST: SupplierActivities/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RecordsCreate([Bind(Include = "Id,Activity,Code,DtActivity,Assigned,Remarks,SupplierId")] SupplierActivity supplierActivity)
        {
            if (ModelState.IsValid)
            {
                db.SupplierActivities.Add(supplierActivity);
                db.SaveChanges();
                return RedirectToAction("Records",new { id = supplierActivity.SupplierId });
            }

            ViewBag.SupplierId = new SelectList(db.Suppliers, "Id", "Name", supplierActivity.SupplierId);
            return View(supplierActivity);
        }
    }
}
