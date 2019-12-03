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
    public class CustEntAssignsController : Controller
    {
        private JobDBContainer db = new JobDBContainer();

        // GET: CustEntAssigns
        public ActionResult Index()
        {
            var custEntAssigns = db.CustEntAssigns.Include(c => c.CustEntMain);
            return View(custEntAssigns.ToList());
        }

        // GET: CustEntAssigns/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustEntAssign custEntAssign = db.CustEntAssigns.Find(id);
            if (custEntAssign == null)
            {
                return HttpNotFound();
            }
            return View(custEntAssign);
        }

        // GET: CustEntAssigns/Create
        public ActionResult Create()
        {
            ViewBag.CustEntMainId = new SelectList(db.CustEntMains, "Id", "Name");
            return View();
        }

        // POST: CustEntAssigns/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Assigned,Remarks,CustEntMainId,Date")] CustEntAssign custEntAssign)
        {
            if (ModelState.IsValid)
            {
                db.CustEntAssigns.Add(custEntAssign);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CustEntMainId = new SelectList(db.CustEntMains, "Id", "Name", custEntAssign.CustEntMainId);
            return View(custEntAssign);
        }

        // GET: CustEntAssigns/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustEntAssign custEntAssign = db.CustEntAssigns.Find(id);
            if (custEntAssign == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustEntMainId = new SelectList(db.CustEntMains, "Id", "Name", custEntAssign.CustEntMainId);
            return View(custEntAssign);
        }

        // POST: CustEntAssigns/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Assigned,Remarks,CustEntMainId,Date")] CustEntAssign custEntAssign)
        {
            if (ModelState.IsValid)
            {
                db.Entry(custEntAssign).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CustEntMainId = new SelectList(db.CustEntMains, "Id", "Name", custEntAssign.CustEntMainId);
            return View(custEntAssign);
        }

        // GET: CustEntAssigns/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustEntAssign custEntAssign = db.CustEntAssigns.Find(id);
            if (custEntAssign == null)
            {
                return HttpNotFound();
            }
            return View(custEntAssign);
        }

        // POST: CustEntAssigns/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CustEntAssign custEntAssign = db.CustEntAssigns.Find(id);
            db.CustEntAssigns.Remove(custEntAssign);
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
