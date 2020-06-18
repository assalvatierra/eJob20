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
    public class JobPostSalesController : Controller
    {
        private JobDBContainer db = new JobDBContainer();

        // GET: JobPostSales
        public ActionResult Index()
        {
            var jobPostSales = db.JobPostSales.Include(j => j.JobService);
            return View(jobPostSales.ToList());
        }

        // GET: JobPostSales/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobPostSale jobPostSale = db.JobPostSales.Find(id);
            if (jobPostSale == null)
            {
                return HttpNotFound();
            }
            return View(jobPostSale);
        }

        // GET: JobPostSales/Create
        public ActionResult Create()
        {
            ViewBag.JobServicesId = new SelectList(db.JobServices, "Id", "Particulars");
            return View();
        }

        // POST: JobPostSales/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,DtPost,DoneBy,Remarks,JobServicesId")] JobPostSale jobPostSale)
        {
            if (ModelState.IsValid)
            {
                db.JobPostSales.Add(jobPostSale);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.JobServicesId = new SelectList(db.JobServices, "Id", "Particulars", jobPostSale.JobServicesId);
            return View(jobPostSale);
        }

        // GET: JobPostSales/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobPostSale jobPostSale = db.JobPostSales.Find(id);
            if (jobPostSale == null)
            {
                return HttpNotFound();
            }
            ViewBag.JobServicesId = new SelectList(db.JobServices, "Id", "Particulars", jobPostSale.JobServicesId);
            return View(jobPostSale);
        }

        // POST: JobPostSales/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,DtPost,DoneBy,Remarks,JobServicesId")] JobPostSale jobPostSale)
        {
            if (ModelState.IsValid)
            {
                db.Entry(jobPostSale).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.JobServicesId = new SelectList(db.JobServices, "Id", "Particulars", jobPostSale.JobServicesId);
            return View(jobPostSale);
        }

        // GET: JobPostSales/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobPostSale jobPostSale = db.JobPostSales.Find(id);
            if (jobPostSale == null)
            {
                return HttpNotFound();
            }
            return View(jobPostSale);
        }

        // POST: JobPostSales/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            JobPostSale jobPostSale = db.JobPostSales.Find(id);
            db.JobPostSales.Remove(jobPostSale);
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
