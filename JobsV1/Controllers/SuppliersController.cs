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
    public class SuppliersController : Controller
    {
        private JobDBContainer db = new JobDBContainer();

        private List<SelectListItem> StatusList = new List<SelectListItem> {
                new SelectListItem { Value = "ACT", Text = "Active" },
                new SelectListItem { Value = "INC", Text = "Inactive" },
                new SelectListItem { Value = "BAD", Text = "Bad Account" }
                };

        // GET: Suppliers
        public ActionResult Index()
        {
            var suppliers = db.Suppliers.Include(s => s.City);
            return View(suppliers.ToList());
        }

        // GET: Suppliers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Supplier supplier = db.Suppliers.Find(id);
            if (supplier == null)
            {
                return HttpNotFound();
            }
            return View(supplier);
        }

        // GET: Suppliers/Create
        public ActionResult Create()
        {
            ViewBag.CityId = new SelectList(db.Cities, "Id", "Name");
            ViewBag.SupplierTypeId = new SelectList(db.SupplierTypes, "Id", "Description");
            ViewBag.Status = new SelectList(StatusList, "value", "text");

            return View();
        }

        // POST: Suppliers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Contact1,Contact2,Contact3,Email,Details,CityId,SupplierTypeId,Status")] Supplier supplier)
        {
            if (ModelState.IsValid)
            {
                db.Suppliers.Add(supplier);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CityId = new SelectList(db.Cities, "Id", "Name", supplier.CityId);
            ViewBag.SupplierTypeId = new SelectList(db.SupplierTypes, "Id", "Description");

            return View(supplier);
        }

        // GET: Suppliers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Supplier supplier = db.Suppliers.Find(id);
            if (supplier == null)
            {
                return HttpNotFound();
            }
            ViewBag.CityId = new SelectList(db.Cities, "Id", "Name", supplier.CityId);
            ViewBag.SupplierTypeId = new SelectList(db.SupplierTypes, "Id", "Description");

            ViewBag.Status = new SelectList(StatusList, "value", "text", supplier.Status);

            return View(supplier);
        }

        // POST: Suppliers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Contact1,Contact2,Contact3,Email,Details,CityId,SupplierTypeId,Status")] Supplier supplier)
        {
            if (ModelState.IsValid)
            {
                db.Entry(supplier).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CityId = new SelectList(db.Cities, "Id", "Name", supplier.CityId);
            ViewBag.SupplierTypeId = new SelectList(db.SupplierTypes, "Id", "Description");
            ViewBag.Status = new SelectList(StatusList, "value", "text", supplier.Status);

            return View(supplier);
        }

        // GET: Suppliers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Supplier supplier = db.Suppliers.Find(id);
            if (supplier == null)
            {
                return HttpNotFound();
            }
            return View(supplier);
        }

        // POST: Suppliers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Supplier supplier = db.Suppliers.Find(id);
            db.Suppliers.Remove(supplier);
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

        public ActionResult InvItems(int id)
        {
            ViewBag.SupplierId = id;
            ViewBag.SupplierName = db.Suppliers.Find(id).Name;
            ViewBag.ItemList = db.InvItems.ToList();
            return View(db.SupplierInvItems.Where(s=>s.SupplierId == id).ToList());
        }

        public ActionResult AddInvItems(int InvID, int supID) {
            db.SupplierInvItems.Add(new SupplierInvItem {
                InvItemId = InvID,
                SupplierId = supID
            });
            db.SaveChanges();

            return RedirectToAction("InvItems", "Suppliers", new { id = supID });
        }


        public ActionResult RemoveInvItems(int id)
        {
            SupplierInvItem item = db.SupplierInvItems.Find(id);
            db.SupplierInvItems.Remove(item);

            db.SaveChanges();

            return RedirectToAction("InvItems", "Suppliers", new { id = item.SupplierId });
        }
    }
}
