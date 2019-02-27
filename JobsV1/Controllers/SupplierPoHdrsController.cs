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
    public class SupplierPoHdrsController : Controller
    {
        private JobDBContainer db = new JobDBContainer();
        private DBClasses dbclasses = new DBClasses();

        // GET: SupplierPoHdrs
        public ActionResult Index()
        {
            var supplierPoHdrs = db.SupplierPoHdrs.Include(s => s.Supplier).Include(s => s.SupplierPoStatu).OrderByDescending(s=>s.PoDate);
            return View(supplierPoHdrs.ToList());
        }

        // GET: SupplierPoHdrs/Details/5
        public ActionResult Details(int? id)
        {
            return RedirectToAction("index", "SupplierPoDtls", new { hdrId = (int)id });
        }

        // GET: SupplierPoHdrs/Create
        public ActionResult Create()
        {
            SupplierPoHdr hdr = new SupplierPoHdr();
            hdr.PoDate = DateTime.Now;
            hdr.DtRequest = DateTime.Now;
            hdr.RequestBy = HttpContext.User.Identity.Name;

            ViewBag.SupplierId = new SelectList(db.Suppliers, "Id", "Name");
            ViewBag.SupplierPoStatusId = new SelectList(db.SupplierPoStatus, "Id", "Status");
            return View(hdr);
        }

        // POST: SupplierPoHdrs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,PoDate,Remarks,SupplierId,SupplierPoStatusId,RequestBy,DtRequest,ApprovedBy,DtApproved")] SupplierPoHdr supplierPoHdr)
        {
            if (ModelState.IsValid)
            {
                db.SupplierPoHdrs.Add(supplierPoHdr);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.SupplierId = new SelectList(db.Suppliers, "Id", "Name", supplierPoHdr.SupplierId);
            ViewBag.SupplierPoStatusId = new SelectList(db.SupplierPoStatus, "Id", "Status", supplierPoHdr.SupplierPoStatusId);
            return View(supplierPoHdr);
        }

        // GET: SupplierPoHdrs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SupplierPoHdr supplierPoHdr = db.SupplierPoHdrs.Find(id);
            if (supplierPoHdr == null)
            {
                return HttpNotFound();
            }
            ViewBag.SupplierId = new SelectList(db.Suppliers, "Id", "Name", supplierPoHdr.SupplierId);
            ViewBag.SupplierPoStatusId = new SelectList(db.SupplierPoStatus, "Id", "Status", supplierPoHdr.SupplierPoStatusId);
            return View(supplierPoHdr);
        }

        // POST: SupplierPoHdrs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,PoDate,Remarks,SupplierId,SupplierPoStatusId,RequestBy,DtRequest,ApprovedBy,DtApproved")] SupplierPoHdr supplierPoHdr)
        {
            if (ModelState.IsValid)
            {
                db.Entry(supplierPoHdr).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.SupplierId = new SelectList(db.Suppliers, "Id", "Name", supplierPoHdr.SupplierId);
            ViewBag.SupplierPoStatusId = new SelectList(db.SupplierPoStatus, "Id", "Status", supplierPoHdr.SupplierPoStatusId);
            return View(supplierPoHdr);
        }

        // GET: SupplierPoHdrs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SupplierPoHdr supplierPoHdr = db.SupplierPoHdrs.Find(id);
            if (supplierPoHdr == null)
            {
                return HttpNotFound();
            }
            return View(supplierPoHdr);
        }

        // POST: SupplierPoHdrs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SupplierPoHdr supplierPoHdr = db.SupplierPoHdrs.Find(id);
            db.SupplierPoHdrs.Remove(supplierPoHdr);
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

        public ActionResult updateStatus(int statusid , int hdrid ) {

            SupplierPoHdr supHdr = db.SupplierPoHdrs.Find(hdrid);
            supHdr.SupplierPoStatusId = statusid;
            db.Entry(supHdr).State = EntityState.Modified;
            db.SaveChanges();
            ViewBag.Status = "hello";
            return RedirectToAction("Index");
        }
    }
}
