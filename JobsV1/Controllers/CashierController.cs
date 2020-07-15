using JobsV1.Models;
using JobsV1.Models.Class;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JobsV1.Controllers
{
    public class CashierController : Controller
    {
        private JobDBContainer db = new JobDBContainer();
        private DateClass dt = new DateClass();
        private ActionTrailClass trail = new ActionTrailClass();

        // GET: Cashier
        public ActionResult Index(string srch, int? paymentStatus, int? jobStatus)
        {
            var jobs = db.JobMains.Where(j => j.JobStatusId < 5);

            //job status filters
            if (jobStatus != null)
            {
                if(jobStatus > 1)
                    jobs = jobs.Where(j => j.JobStatusId == (int)jobStatus);
            }
            else
            {
                //default
                jobs = jobs.Where(j => j.JobStatusId == 4);
            }

            //job payment status filter
            jobs = GetFilteredJobPayment(jobs, paymentStatus);

            //Search
            if (!srch.IsNullOrWhiteSpace())
            {
                jobs = jobs.Where(j => j.Id.ToString() == srch ||
                        j.Description.ToLower().Contains(srch.ToLower()) ||
                        j.Customer.Name.ToLower().Contains(srch.ToLower()));

            }

            //order
            jobs = jobs.OrderBy(j => j.JobDate);

            ViewBag.JobStatus = jobStatus;
            ViewBag.PaymentStatus = paymentStatus;
            ViewBag.SrchString = srch;

            return View(jobs.ToList());
        }

        public string GetJobTotalAmount(int id)
        {
            var jobservices = db.JobServices.Where(j => j.JobMainId == id).ToList();

            decimal total = 0;

            foreach (var svc in jobservices)
            {
                total += (decimal)svc.ActualAmt;
            }

            return total.ToString("#,##0.00");
        }

        public string GetJobTotalPaidAmount(int id)
        {
            decimal totalPaidAmount = 0;
            var jobpayments = db.JobPayments.Where(p => p.JobMainId == id).ToList();
            if (jobpayments != null)
            {
                foreach (var payment in jobpayments)
                {
                    totalPaidAmount += payment.PaymentAmt;
                }
            }

            return totalPaidAmount.ToString("#,##0.00");
        }


        private IQueryable<JobMain> GetFilteredJobPayment(IQueryable<JobMain> jobs, int? paymentStatus)
        {

            var jobsForPayment = jobs.ToList();

            //payment status filters
            //job status filters
            var filteredJobIds = new List<int>();
            var tempPaymentStatus = paymentStatus ?? 3;

            foreach (var tempjob in jobsForPayment)
            {
                //check if job has payment status record
                var lastPaymentStatus = tempjob.JobMainPaymentStatus.OrderByDescending(s => s.Id).FirstOrDefault();

                if (lastPaymentStatus != null)
                {
                    switch (tempPaymentStatus)
                    {
                        case 2:
                            //paid
                            //compare status to param payment status
                            if (lastPaymentStatus.JobPaymentStatusId == 2)
                            {
                                //get id to add job to list
                                filteredJobIds.Add(tempjob.Id);
                            }
                            break;
                        case 3:
                            //all
                            //add all jobs to the list
                            break;
                        case 1:
                        default:
                            //unpaid
                            //compare status to param payment status
                            if (lastPaymentStatus.JobPaymentStatusId == 1)
                            {
                                //get id to add job to list
                                filteredJobIds.Add(tempjob.Id);
                            }
                            break;
                    }
                }
            }
            //filter jobs except  payment filter 'all' = 3
            if (tempPaymentStatus < 3)
            {
                //filter jobs not in paymentStatus
                jobs = jobs.Where(j => filteredJobIds.Contains(j.Id));
            }


            return jobs;
        }


        public ActionResult JobPaymentDetails(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("PageNotFound", "Home", null);
            }
            ViewBag.JobMainId = id;

            var Job = db.JobMains.Where(d => d.Id == id).FirstOrDefault();
            ViewBag.JobOrder = Job;
            ViewBag.JobStatus = Job.JobStatus.Status;

            var jobPayments = db.JobPayments.Where(d => d.JobMainId == id);
            return View(jobPayments.ToList());
        }



        // GET: JobPayments/Create , remarks = "Partial Payment" 
        public ActionResult AddPayment(int? id, string remarks)
        {
            JobPayment jp = new JobPayment();
            jp.JobMainId = (int)id;
            jp.DtPayment = dt.GetCurrentDateTime();
            jp.Remarks = remarks;

            ViewBag.JobMainId = new SelectList(db.JobMains, "Id", "Description", id);
            ViewBag.BankId = new SelectList(db.Banks, "Id", "BankName");
            ViewBag.JobPaymentTypeId = new SelectList(db.JobPaymentTypes, "Id", "Type");
            ViewBag.JobId = id;
            return View(jp);
        }

        // POST: JobPayments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddPayment([Bind(Include = "Id,JobMainId,DtPayment,PaymentAmt,Remarks,BankId,JobPaymentTypeId")] JobPayment jobPayment)
        {
            if (ModelState.IsValid)
            {
                db.JobPayments.Add(jobPayment);
                db.SaveChanges();

                //add payment status to job
                AddJobPaymentStatus(jobPayment.JobMainId, 1); //paid

                //job trail
                trail.recordTrail("Cashier/AddPayment", HttpContext.User.Identity.Name, "Job Payment Added", jobPayment.JobMainId.ToString());
              
                return RedirectToAction("JobPaymentDetails", new { id = jobPayment.JobMainId });
            }

            ViewBag.JobMainId = new SelectList(db.JobMains, "Id", "Description", jobPayment.JobMainId);
            ViewBag.BankId = new SelectList(db.Banks, "Id", "BankName", jobPayment.BankId);
            ViewBag.JobPaymentTypeId = new SelectList(db.JobPaymentTypes, "Id", "Type", jobPayment.JobPaymentTypeId);

            return View(jobPayment);
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