using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using JobsV1.Models;
using Microsoft.Ajax.Utilities;

namespace JobsV1.Controllers
{
    public class JobPostSalesController : Controller
    {
        private JobDBContainer db = new JobDBContainer();
        private DateClass dt = new DateClass();

        // GET: JobPostSales
        public ActionResult Index(int? serviceId, int? statusId)
        {
            DateTime today = dt.GetCurrentDate().Date;
            var jobPostSales = db.JobPostSales.Where(j => today > j.DtPost);

            if (statusId != null)
            {
                jobPostSales = jobPostSales.Where(j => j.JobPostSalesStatusId == statusId);
            }
            else
            {
                jobPostSales = jobPostSales.Where(j => j.JobPostSalesStatusId < 3);
            }

            if (serviceId == null)
            {
                serviceId = 0;
            }
            if (serviceId != 0)
            {
                jobPostSales = jobPostSales.Where(j => j.JobService.ServicesId == serviceId);
            }

            ViewBag.StatusId = statusId;
            ViewBag.Services = db.Services.ToList().OrderBy(s => s.Description);
            return View(jobPostSales.OrderByDescending(s=>s.DtPost).ToList());
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
        public ActionResult Create(int? id)
        {
            int jobserviceId = 1;
            if(id != null)
            {
                jobserviceId =(int)id;
            }

            JobPostSale jobPostSale = new JobPostSale();
            jobPostSale.DoneBy = HttpContext.User.Identity.Name;
            ViewBag.JobServicesId = new SelectList(db.JobServices, "Id", "Particulars", jobserviceId);
            ViewBag.JobPostSalesStatusId = new SelectList(db.JobPostSalesStatus, "Id", "Status");
            return View(jobPostSale);
        }

        // POST: JobPostSales/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,DtPost,DoneBy,Remarks,JobServicesId, DtDone, JobPostSalesStatusId")] JobPostSale jobPostSale)
        {
            if (ModelState.IsValid && InputValidation(jobPostSale))
            {
                db.JobPostSales.Add(jobPostSale);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.JobServicesId = new SelectList(db.JobServices, "Id", "Particulars", jobPostSale.JobServicesId);
            ViewBag.JobPostSalesStatusId = new SelectList(db.JobPostSalesStatus, "Id", "Status", jobPostSale.JobPostSalesStatusId);
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
            ViewBag.JobPostSalesStatusId = new SelectList(db.JobPostSalesStatus, "Id", "Status", jobPostSale.JobPostSalesStatusId);
            return View(jobPostSale);
        }

        // POST: JobPostSales/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,DtPost,DoneBy,Remarks,JobServicesId, DtDone, JobPostSalesStatusId")] JobPostSale jobPostSale)
        {
            if (ModelState.IsValid && InputValidation(jobPostSale))
            {
                if (jobPostSale.JobPostSalesStatusId > 2)
                {
                    jobPostSale.DtDone = dt.GetCurrentDateTime();
                    jobPostSale.DoneBy = HttpContext.User.Identity.Name;
                }

                db.Entry(jobPostSale).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.JobServicesId = new SelectList(db.JobServices, "Id", "Particulars", jobPostSale.JobServicesId);
            ViewBag.JobPostSalesStatusId = new SelectList(db.JobPostSalesStatus, "Id", "Status", jobPostSale.JobPostSalesStatusId);
            return View(jobPostSale);
        }


        // GET: JobPostSales/Edit/5
        public ActionResult Update(int? id)
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
            ViewBag.JobPostSalesStatusId = new SelectList(db.JobPostSalesStatus, "Id", "Status", jobPostSale.JobPostSalesStatusId);
            return View(jobPostSale);
        }

        // POST: JobPostSales/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(int Id,string Remarks, int JobPostSalesStatusId)
        {
            var jobPostSale = db.JobPostSales.Find(Id);
            if (jobPostSale != null)
            {
                if (JobPostSalesStatusId > 2)
                {
                    jobPostSale.DtDone = dt.GetCurrentDateTime();
                    jobPostSale.DoneBy = HttpContext.User.Identity.Name;
                }

                jobPostSale.JobPostSalesStatusId = JobPostSalesStatusId;
                jobPostSale.Remarks = Remarks;
                db.Entry(jobPostSale).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.JobServicesId = new SelectList(db.JobServices, "Id", "Particulars", jobPostSale.JobServicesId);
            ViewBag.JobPostSalesStatusId = new SelectList(db.JobPostSalesStatus, "Id", "Status", jobPostSale.JobPostSalesStatusId);
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


        public bool InputValidation(JobPostSale jobPostSale)
        {
            bool isValid = true;

            if (jobPostSale.DoneBy.IsNullOrWhiteSpace())
            {
                ModelState.AddModelError("DoneBy", "Invalid DoneBy");
                isValid = false;
            }

            if (jobPostSale.DtPost == null)
            {
                ModelState.AddModelError("DtPost", "Invalid DtPost");
                isValid = false;
            }

            if (jobPostSale.JobServicesId == 0)
            {
                ModelState.AddModelError("JobServicesId", "Invalid JobServicesId");
                isValid = false;
            }


            return isValid;
        }

        public ActionResult JobsForServicing()
        {

            DateTime today = dt.GetCurrentDate().Date;
            var jobserviceList = db.JobServices.Where(j => today >= (DateTime)DbFunctions.AddDays(DbFunctions.TruncateTime(j.DtStart), j.SupplierItem.Interval));

            return View(jobserviceList);
        }
    }
}
