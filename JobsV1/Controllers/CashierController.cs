using JobsV1.Models;
using JobsV1.Models.Class;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace JobsV1.Controllers
{
    public class CashierController : Controller
    {
        private JobDBContainer db = new JobDBContainer();
        private DateClass dt = new DateClass();
        private ActionTrailClass trail = new ActionTrailClass();
        private JobOrderClass jo = new JobOrderClass();

        // GET: Cashier
        public ActionResult Index(string srch, int? paymentStatus, int? jobStatus)
        {
            CheckAdminPermission("na");

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

            var tempPaymentStatus = paymentStatus ?? 2;

            //job payment status filter
            jobs = GetFilteredJobPayment(jobs, tempPaymentStatus);

            //remove whitespace
           

            //Search
            if (!srch.IsNullOrWhiteSpace())
            {
                srch = srch.Trim();

                jobs = jobs.Where(j => j.Id.ToString() == srch ||
                        j.Description.ToLower().Contains(srch.ToLower()) ||
                        j.Customer.Name.ToLower().Contains(srch.ToLower()));
            }

            //order
            jobs = jobs.OrderBy(j => j.JobDate);

            ViewBag.JobStatus = jobStatus;
            ViewBag.PaymentStatus = paymentStatus;
            ViewBag.SrchString = srch;
            ViewBag.IsAdmin = User.IsInRole("Admin");

            return View(jobs.OrderByDescending(j=>j.JobDate).ToList());
        }

        public string GetJobTotalAmount(int? id)
        {
            try
            {

                if (id == null)
                {
                    return "0.00";
                }
                var jobservices = db.JobServices.Where(j => j.JobMainId == id).ToList();

                decimal total = 0;

                foreach (var svc in jobservices)
                {
                    total += (decimal)svc.ActualAmt;
                }

                //subtract discounted amount
                //note: discount amount is negative number
                total += jo.GetJobDiscountAmount((int)id);

                return total.ToString("#,##0.00");
            }
            catch
            {
                return "0.00";
            }
        }

        public string GetJobTotalPaidAmount(int id)
        {
            decimal totalPaidAmount = 0;
            var jobpayments = db.JobPayments.Where(p => p.JobMainId == id).ToList();
            if (jobpayments != null)
            {
                foreach (var payment in jobpayments)
                {
                    if(payment.JobPaymentTypeId != 4)
                    {
                        totalPaidAmount += payment.PaymentAmt;
                    }
                }
            }

            return totalPaidAmount.ToString("#,##0.00");
        }

        public string GetCompanyAccountType(int id)
        {
            try
            {
                var company = db.JobEntMains.Where(c => c.JobMainId == id).OrderByDescending(c => c.Id).FirstOrDefault().CustEntMain;

                return company.CustEntAccountType.Name;
            }
            catch {
                return "";
            }
        }

        private IQueryable<JobMain> GetFilteredJobPayment(IQueryable<JobMain> jobs, int? paymentStatus)
        {

            var jobsForPayment = jobs.ToList();

            //payment status filters
            //job status filters
            var filteredJobIds = new List<int>();
            var tempPaymentStatus = paymentStatus ?? 2;

            foreach (var tempjob in jobsForPayment)
            {
                //check if job has payment status record
                var lastPaymentStatus = tempjob.JobMainPaymentStatus.OrderByDescending(s => s.Id).FirstOrDefault();

                if (lastPaymentStatus != null)
                {
                    switch (tempPaymentStatus)
                    {
                        case 2:
                        default:
                            //unpaid
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
            var services = db.JobServices.Where(j => j.JobMainId == id).ToList();

            ViewBag.JobServices= services;
            ViewBag.JobOrder = Job;
            ViewBag.Company = jo.GetJobCompany(Job.Id);
            ViewBag.JobStatus = Job.JobStatus;
            ViewBag.JobPaymentStatus = Job.JobMainPaymentStatus.OrderByDescending(p=>p.Id).FirstOrDefault().JobPaymentStatu
                ?? new JobsV1.Models.JobPaymentStatus() { Status = "NA" };
            ViewBag.JobDiscount = jo.GetJobDiscountAmount((int)id);
            ViewBag.IsAdmin = User.IsInRole("Admin");
            ViewBag.statusList = db.JobPaymentStatus.ToList();

            var jobPayments = db.JobPayments.Where(d => d.JobMainId == id );
            return View(jobPayments.ToList());
        }


        // GET: JobPayments/Create , remarks = "Partial Payment" 
        [HttpPost]
        public bool Ajax_AddPayment(int? id, decimal amount, int bankId, int typeId, string remarks)
        {
            try
            {

                if (id == null)
                {
                    return false;
                }

                JobPayment jp = new JobPayment();
                jp.JobMainId = (int)id;
                jp.BankId = bankId;
                jp.JobPaymentTypeId = typeId;
                jp.DtPayment = dt.GetCurrentDateTime();
                jp.Remarks = remarks;
                jp.PaymentAmt = amount;

                //check if payment type is (PaymentTypeID = 4) Discount
                if (jp.JobPaymentTypeId == 4)
                {
                    //multiply by -1 to subtract from total amount
                    jp.PaymentAmt = (-1) * (jp.PaymentAmt);
                }

                db.JobPayments.Add(jp);
                db.SaveChanges();

                //add payment status to job
                AddJobPaymentStatus(jp.JobMainId, 1); //paid

                //job trail
                trail.recordTrail("Cashier/AddPayment", HttpContext.User.Identity.Name, "Job Payment Added", jp.JobMainId.ToString());

                return true;
            }
            catch
            {
                return false;
            }

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

                //check if payment type is (PaymentTypeID = 4) Discount
                if (jobPayment.JobPaymentTypeId == 4)
                {
                    //multiply by -1 to subtract from total amount
                    jobPayment.PaymentAmt = (-1) * (jobPayment.PaymentAmt);
                }

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
                decimal totalAmount = 0;
                decimal totalPaymentAmount = 0;

                //check total amount
                var jobservices = db.JobServices.Where(j => j.JobMainId == id).ToList();

                if (jobservices != null)
                {
                    foreach(var svc in jobservices)
                    {
                        totalAmount += (decimal)svc.ActualAmt;
                    }

                    //subtract discounted amount
                    totalAmount += jo.GetJobDiscountAmount(id);

                    //check payment amount if greater than total
                    var payments = db.JobPayments.Where(p => p.JobMainId == id).ToList();
                   
                    if (payments != null)
                    {

                        foreach (var paid in payments)
                        {
                            //add payment amount except 4 = Discount
                            if(paid.JobPaymentTypeId != 4)
                                totalPaymentAmount += (decimal)paid.PaymentAmt;
                        }

                    }
                }

                //check if total payment is equal or greate than the total amount og job
                if (totalAmount != 0 && (totalPaymentAmount >= totalAmount))
                {
                    JobMainPaymentStatus paymentStatus = new JobMainPaymentStatus();
                    paymentStatus.JobMainId = id;
                    paymentStatus.JobPaymentStatusId = statusId;

                    db.JobMainPaymentStatus.Add(paymentStatus);
                    db.SaveChanges();
                    return true;
                }

                return false;
            }
            catch
            {
                return false;
            }
        }


        // GET: JobPayments/Edit/5
        public ActionResult EditPayment(int? id)
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
        public ActionResult EditPayment([Bind(Include = "Id,JobMainId,DtPayment,PaymentAmt,Remarks,BankId,JobPaymentTypeId")] JobPayment jobPayment)
        {
            if (ModelState.IsValid)
            {
                //check if payment type is (PaymentTypeID = 4) Discount
                if (jobPayment.JobPaymentTypeId == 4)
                {
                    if (jobPayment.PaymentAmt > 0)
                    {
                        //multiply by -1 to subtract from total amount
                        jobPayment.PaymentAmt = (-1) * (jobPayment.PaymentAmt);
                    }
                }

                db.Entry(jobPayment).State = EntityState.Modified;
                db.SaveChanges();

                //job trail
                trail.recordTrail("JobPayments/Edit", HttpContext.User.Identity.Name, "Job Payment Edited", jobPayment.JobMainId.ToString());

                return RedirectToAction("JobPaymentDetails", new { id = jobPayment.JobMainId });
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
            trail.recordTrail("JobPayments/Delete", HttpContext.User.Identity.Name, "Job Payment Removed", jobPayment.JobMainId.ToString());

            return RedirectToAction("JobPaymentDetails", new { id = jobPayment.JobMainId });
        }

        [HttpGet]
        public bool CheckAdminPermission(string pass)
        {
            var adminPass = "Admin123!";

            if(adminPass == pass)
            {
                return true;
            }
            return false;
        }

        [HttpPost]
        public bool UpdatePaymentStatus(int? id, int? statusId)
        {
            try
            {
                if (id == null || statusId == null)
                {
                    return false;

                }

                var job = db.JobMains.Find(id);
                if (job == null)
                {
                    return false;
                }

                //add payment status to job
                JobMainPaymentStatus paymentStatus = new JobMainPaymentStatus();
                paymentStatus.JobMainId = (int)id;
                paymentStatus.JobPaymentStatusId = (int)statusId;

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