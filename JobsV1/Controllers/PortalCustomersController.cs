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
    public class PortalCustomersController : Controller
    {
        private JobDBContainer db = new JobDBContainer();

        // GET: PortalCustomers
        public ActionResult Index()
        {
            return View(db.PortalCustomers.ToList());
        }

        // GET: PortalCustomers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PortalCustomer portalCustomer = db.PortalCustomers.Find(id);
            if (portalCustomer == null)
            {
                return HttpNotFound();
            }
            return View(portalCustomer);
        }

        // GET: PortalCustomers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PortalCustomers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ContactNum,Password,ExpiryDt,CustomerID")] PortalCustomer portalCustomer)
        {
            if (ModelState.IsValid)
            {
                db.PortalCustomers.Add(portalCustomer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(portalCustomer);
        }

        // GET: PortalCustomers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PortalCustomer portalCustomer = db.PortalCustomers.Find(id);
            if (portalCustomer == null)
            {
                return HttpNotFound();
            }
            return View(portalCustomer);
        }

        // POST: PortalCustomers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ContactNum,Password,ExpiryDt,CustomerID")] PortalCustomer portalCustomer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(portalCustomer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(portalCustomer);
        }

        // GET: PortalCustomers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PortalCustomer portalCustomer = db.PortalCustomers.Find(id);
            if (portalCustomer == null)
            {
                return HttpNotFound();
            }
            return View(portalCustomer);
        }

        // POST: PortalCustomers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PortalCustomer portalCustomer = db.PortalCustomers.Find(id);
            db.PortalCustomers.Remove(portalCustomer);
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
    }
}
