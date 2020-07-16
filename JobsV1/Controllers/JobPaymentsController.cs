using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using JobsV1.Models;
using JobsV1.Models.Class;
using Microsoft.Ajax.Utilities;

namespace JobsV1.Controllers
{
    public class JobPaymentsController : Controller
    {
        private JobDBContainer db = new JobDBContainer();
        private DateClass dt = new DateClass();
        private ActionTrailClass trail = new ActionTrailClass();
        private string SITECONFIG = ConfigurationManager.AppSettings["SiteConfig"].ToString();
        

        // GET: JobPayments
        public ActionResult Index()
        {
            var jobPayments = db.JobPayments.Include(j => j.JobMain).Include(j => j.Bank);
            return View(jobPayments.ToList());
        }

        public ActionResult AdvanceList()
        {
            var payments = db.JobPayments.Include(j => j.JobMain).Include(j => j.Bank)
                .Where(d => d.JobMain.JobStatusId != 4 && d.JobMain.JobStatusId != 5)
                .OrderBy( d=>d.DtPayment );
            return View(payments);
        }

        public ActionResult Payments(int? id)
        {
            ViewBag.JobMainId = id;


            if (db.JobTrails.Where(s => s.RefTable == "joborder" && s.RefId == id.ToString()).FirstOrDefault() != null)
            {
                ViewBag.JobEncoder = db.JobTrails.Where(s => s.RefTable == "joborder" && s.RefId == id.ToString()).FirstOrDefault();
            }
            else
            {
                ViewBag.JobEncoder = new JobTrail { Id = 0, Action = "Create", user = "none", dtTrail = dt.GetCurrentDateTime(), RefId = "0", RefTable = "none" };
            }

            var Job = db.JobMains.Where(d => d.Id == id).FirstOrDefault();
            ViewBag.JobOrder = Job;
            ViewBag.SiteConfig = SITECONFIG;
            ViewBag.JobStatus = Job.JobStatus.Status;

            var jobPayments = db.JobPayments.Include(j => j.JobMain).Include(j => j.Bank).Where(d=>d.JobMainId==id);
            return View("index",jobPayments.ToList());
        }

        // GET: JobPayments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobPayment jobPayment = db.JobPayments.Find(id);
            if (jobPayment == null)
            {
                return HttpNotFound();
            }
            return View(jobPayment);
        }

        // GET: JobPayments/Create , remarks = "Partial Payment" 
        public ActionResult Create(int? JobMainId, string remarks)
        {
            Models.JobPayment jp = new JobPayment();
            jp.JobMainId = (int)JobMainId;
            jp.DtPayment = dt.GetCurrentDateTime();
            jp.Remarks = remarks;

            ViewBag.JobMainId = new SelectList(db.JobMains, "Id", "Description", JobMainId);
            ViewBag.BankId = new SelectList(db.Banks, "Id", "BankName");
            ViewBag.JobPaymentTypeId = new SelectList(db.JobPaymentTypes, "Id", "Type");

            return View(jp);
        }


        // GET: JobPayments/Create , remarks = "Partial Payment" 
        public ActionResult CreatePG(int? JobMainId, string remarks)
        {

            JobPayment jobPayment = new JobPayment();
            jobPayment.BankId = 4;
            jobPayment.DtPayment = dt.GetCurrentDateTime();
            jobPayment.JobMainId = (int)JobMainId;
            jobPayment.PaymentAmt = 0;
            jobPayment.Remarks = remarks;

            db.JobPayments.Add(jobPayment);
            db.SaveChanges();

            ViewBag.JobMainId = JobMainId;

            var Job = db.JobMains.Where(d => d.Id == JobMainId).FirstOrDefault();
            ViewBag.JobOrder = Job;

            return RedirectToAction("Payments", new { id = JobMainId });
        }


        // POST: JobPayments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,JobMainId,DtPayment,PaymentAmt,Remarks,BankId,JobPaymentTypeId")] JobPayment jobPayment)
        {
            if (ModelState.IsValid)
            {
                db.JobPayments.Add(jobPayment);
                db.SaveChanges();

                //add payment status to job
                AddJobPaymentStatus(jobPayment.JobMainId, 1); //paid

                //job trail
                trail.recordTrail("JobPayments/Create", HttpContext.User.Identity.Name, "Job Payment Added", jobPayment.JobMainId.ToString());

                return RedirectToAction("Payments", new { id = jobPayment.JobMainId });
            }

            ViewBag.JobMainId = new SelectList(db.JobMains, "Id", "Description", jobPayment.JobMainId);
            ViewBag.BankId = new SelectList(db.Banks, "Id", "BankName", jobPayment.BankId);
            ViewBag.JobPaymentTypeId = new SelectList(db.JobPaymentTypes, "Id", "Type", jobPayment.JobPaymentTypeId);
            return View(jobPayment);
        }

        // GET: JobPayments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobPayment jobPayment = db.JobPayments.Find(id);
            if (jobPayment == null)
            {
                return HttpNotFound();
            }
            ViewBag.JobMainId = new SelectList(db.JobMains, "Id", "Description", jobPayment.JobMainId);
            ViewBag.BankId = new SelectList(db.Banks, "Id", "BankName", jobPayment.BankId);
            ViewBag.JobPaymentTypeId = new SelectList(db.JobPaymentTypes, "Id", "Type", jobPayment.JobPaymentTypeId);
            return View(jobPayment);
        }

        // POST: JobPayments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,JobMainId,DtPayment,PaymentAmt,Remarks,BankId,JobPaymentTypeId")] JobPayment jobPayment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(jobPayment).State = EntityState.Modified;
                db.SaveChanges();

                //job trail
                trail.recordTrail("JobPayments/Edit", HttpContext.User.Identity.Name, "Job Payment Edited", jobPayment.JobMainId.ToString());

                return RedirectToAction("Payments", new { id = jobPayment.JobMainId });
            }
            ViewBag.JobMainId = new SelectList(db.JobMains, "Id", "Description", jobPayment.JobMainId);
            ViewBag.BankId = new SelectList(db.Banks, "Id", "BankName", jobPayment.BankId);
            ViewBag.JobPaymentTypeId = new SelectList(db.JobPaymentTypes, "Id", "Type", jobPayment.JobPaymentTypeId);
            return View(jobPayment);
        }

        // GET: JobPayments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobPayment jobPayment = db.JobPayments.Find(id);
            if (jobPayment == null)
            {
                return HttpNotFound();
            }
            return View(jobPayment);
        }

        // POST: JobPayments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            JobPayment jobPayment = db.JobPayments.Find(id);
            int tmpId = jobPayment.JobMainId;
            db.JobPayments.Remove(jobPayment);
            db.SaveChanges();

            //job trail
            trail.recordTrail("JobPayments/Edit", HttpContext.User.Identity.Name, "Job Payment Removed", jobPayment.JobMainId.ToString());

            return RedirectToAction("Payments", new { id = tmpId });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        public bool PaymentModelsValidation(JobPayment jobPayment)
        {
            bool isValid = true;

            if (jobPayment.Remarks.IsNullOrWhiteSpace())
            {
                ModelState.AddModelError("Remarks", "Invalid Remarks");
                isValid = false;
            }

            return isValid;
        }


        private bool AddJobPaymentStatus(int id, int statusId)
        {
            try
            {
                JobMainPaymentStatus paymentStatus = new JobMainPaymentStatus();
                paymentStatus.JobMainId = id;
                paymentStatus.JobPaymentStatusId = statusId;

                db.JobMainPaymentStatus.Add(paymentStatus);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
