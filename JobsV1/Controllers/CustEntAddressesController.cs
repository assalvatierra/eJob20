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
    public class CustEntAddressesController : Controller
    {
        private JobDBContainer db = new JobDBContainer();

        // GET: CustEntAddresses
        public ActionResult Index()
        {
            var custEntAddresses = db.CustEntAddresses.Include(c => c.CustEntMain);
            return View(custEntAddresses.ToList());
        }

        // GET: CustEntAddresses/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustEntAddress custEntAddress = db.CustEntAddresses.Find(id);
            if (custEntAddress == null)
            {
                return HttpNotFound();
            }
            return View(custEntAddress);
        }

        // GET: CustEntAddresses/Create
        public ActionResult Create()
        {
            ViewBag.CustEntMainId = new SelectList(db.CustEntMains, "Id", "Name");
            return View();
        }

        // POST: CustEntAddresses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,CustEntMainId,Line1,Line2,Line3,Line4,Line5,isBilling,isPrimary")] CustEntAddress custEntAddress)
        {
            if (ModelState.IsValid)
            {
                db.CustEntAddresses.Add(custEntAddress);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CustEntMainId = new SelectList(db.CustEntMains, "Id", "Name", custEntAddress.CustEntMainId);
            return View(custEntAddress);
        }


        // GET: CustEntAddresses/Create
        public ActionResult CreateAddress(int companyId)
        {
            ViewBag.CustEntMainId = new SelectList(db.CustEntMains, "Id", "Name", companyId);
            return View();
        }

        // POST: CustEntAddresses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateAddress([Bind(Include = "Id,CustEntMainId,Line1,Line2,Line3,Line4,Line5,isBilling,isPrimary")] CustEntAddress custEntAddress)
        {
            if (ModelState.IsValid)
            {
                db.CustEntAddresses.Add(custEntAddress);
                db.SaveChanges();
                return RedirectToAction("Details", "CustEntMains", new { id = custEntAddress.CustEntMainId });
            }

            ViewBag.CustEntMainId = new SelectList(db.CustEntMains, "Id", "Name", custEntAddress.CustEntMainId);
            return View(custEntAddress);
        }

        // GET: CustEntAddresses/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustEntAddress custEntAddress = db.CustEntAddresses.Find(id);
            if (custEntAddress == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustEntMainId = new SelectList(db.CustEntMains, "Id", "Name", custEntAddress.CustEntMainId);
            return View(custEntAddress);
        }

        // POST: CustEntAddresses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CustEntMainId,Line1,Line2,Line3,Line4,Line5,isBilling,isPrimary")] CustEntAddress custEntAddress)
        {
            if (ModelState.IsValid)
            {
                db.Entry(custEntAddress).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CustEntMainId = new SelectList(db.CustEntMains, "Id", "Name", custEntAddress.CustEntMainId);
            return View(custEntAddress);
        }

        // GET: CustEntAddresses/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustEntAddress custEntAddress = db.CustEntAddresses.Find(id);
            if (custEntAddress == null)
            {
                return HttpNotFound();
            }
            return View(custEntAddress);
        }

        // POST: CustEntAddresses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CustEntAddress custEntAddress = db.CustEntAddresses.Find(id);
            db.CustEntAddresses.Remove(custEntAddress);
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
