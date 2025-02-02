﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using JobsV1.Models;
using Newtonsoft.Json;
using System.Globalization;

namespace JobsV1.Controllers
{
    public class JobExpensesController : Controller
    {
        private JobDBContainer db = new JobDBContainer();

        // GET: JobExpenses0
        public ActionResult Index()
        {
            var jobExpenses = db.JobExpenses.Include(j => j.Expens).Include(j => j.JobService);
            return View(jobExpenses.ToList());
        }

        // GET: JobExpenses/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobExpenses jobExpenses = db.JobExpenses.Find(id);
            if (jobExpenses == null)
            {
                return HttpNotFound();
            }
            return View(jobExpenses);
        }

        // GET: JobExpenses/Create
        public ActionResult Create()
        {
            ViewBag.ExpensesId = new SelectList(db.Expenses.OrderBy(s=>s.SeqNo) , "Id", "Name");
            ViewBag.JobServicesId = new SelectList(db.JobServices, "Id", "Particulars");
            return View();
        }

        // POST: JobExpenses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Amount,Remarks,JobMainId,ExpensesId,JobServicesId")] JobExpenses jobExpenses)
        {
            if (ModelState.IsValid)
            {
                db.JobExpenses.Add(jobExpenses);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ExpensesId = new SelectList(db.Expenses, "Id", "Name", jobExpenses.ExpensesId);
            ViewBag.JobServicesId = new SelectList(db.JobServices, "Id", "Particulars", jobExpenses.JobServicesId);
            return View(jobExpenses);
        }

        // GET: JobExpenses/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobExpenses jobExpenses = db.JobExpenses.Find(id);
            if (jobExpenses == null)
            {
                return HttpNotFound();
            }
            ViewBag.ExpensesId = new SelectList(db.Expenses, "Id", "Name", jobExpenses.ExpensesId);
            ViewBag.JobServicesId = new SelectList(db.JobServices, "Id", "Particulars", jobExpenses.JobServicesId);
            return View(jobExpenses);
        }

        // POST: JobExpenses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Amount,Remarks,JobMainId,ExpensesId,JobServicesId")] JobExpenses jobExpenses)
        {
            if (ModelState.IsValid)
            {
                db.Entry(jobExpenses).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ExpensesId = new SelectList(db.Expenses, "Id", "Name", jobExpenses.ExpensesId);
            ViewBag.JobServicesId = new SelectList(db.JobServices, "Id", "Particulars", jobExpenses.JobServicesId);
            return View(jobExpenses);
        }

        // GET: JobExpenses/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobExpenses jobExpenses = db.JobExpenses.Find(id);
            if (jobExpenses == null)
            {
                return HttpNotFound();
            }
            return View(jobExpenses);
        }

        // POST: JobExpenses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            JobExpenses jobExpenses = db.JobExpenses.Find(id);
            db.JobExpenses.Remove(jobExpenses);
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


        #region JobExpenses

        public ActionResult JobExpenses(int id)
        {
            DateClass today = new DateClass();

            var expenses = db.JobExpenses.Where(e => e.JobServicesId == id).ToList();
            var svcDate = db.JobServices.Find(id).DtEnd;

            ViewBag.JobServiceName = db.JobServices.Find(id).Particulars;
            ViewBag.expenseList = db.Expenses.Include(s => s.ExpensesCategory).ToList();
            ViewBag.JobMainId = db.JobServices.Find(id).JobMainId;
            ViewBag.JobServiceId = id;
            ViewBag.DateToday = svcDate != null ? svcDate : today.GetCurrentDateTime();

            return View(expenses);
        }

        public string JobExpensesAdd(int? id, int expenseId, decimal amount, string remarks, string date)
        {
            if (id != null)
            {
                string stringDate = date;
                //date = "05/07/2017 09:10 AM";
                JobExpenses jobExpense = new JobExpenses();
                jobExpense.Amount = amount;
                jobExpense.ExpensesId = expenseId;
                jobExpense.JobMainId  = db.JobServices.Find(id).JobMainId;
                jobExpense.JobServicesId = (int)id;
                jobExpense.Remarks = remarks;
                //jobExpense.DtExpense = DateTime.ParseExact(date, "MM/DD/YYYY  hh:mm A", CultureInfo.InvariantCulture);
                jobExpense.DtExpense = DateTime.Parse(stringDate);

                db.JobExpenses.Add(jobExpense);
                db.SaveChanges();

                //adding new job expense successful
                return JsonConvert.SerializeObject("200", Formatting.Indented);

            }

            //adding new job expense error
            return JsonConvert.SerializeObject("500", Formatting.Indented);
        }

        public string JobExpensesEdit(int? id, int expenseId, decimal amount, string remarks, string Date)
        {
            if (id != null)
            {

                JobExpenses jobExpense = db.JobExpenses.Find(id);
                jobExpense.Amount = amount;
                jobExpense.ExpensesId = expenseId;
                jobExpense.Remarks = remarks;
                jobExpense.DtExpense = DateTime.Parse(Date); 

                db.Entry(jobExpense).State = EntityState.Modified;
                db.SaveChanges();

                //adding new job expense successful
                return JsonConvert.SerializeObject("200", Formatting.Indented);

            }

            //adding new job expense error
            return JsonConvert.SerializeObject("500", Formatting.Indented);
        }

        public string JobExpensesRemove(int? id)
        {
            if (id != null)
            {

                JobExpenses jobExpense = db.JobExpenses.Find(id);
                db.JobExpenses.Remove(jobExpense);
                db.SaveChanges();
                return JsonConvert.SerializeObject("200", Formatting.Indented);

            }
            return JsonConvert.SerializeObject("500", Formatting.Indented);
        }

        public ActionResult CashExpenses(int jobId)
        {
            IEnumerable<JobExpenses> jobExps = db.JobExpenses.Where(e => e.JobService.JobMainId == jobId).Include(e=>e.JobService).ToList();

            JobMain job = db.JobMains.Find(jobId);
            var jobCount = db.JobPosts.Where(s => s.JobMainId == jobId).Count();

            ViewBag.JobMainId = jobId;
            ViewBag.jobservices = db.JobServices.Where(s => s.JobMainId == jobId).ToList();
            ViewBag.totalExpenses = getTotalExpenses(jobExps);
            ViewBag.jobDesc = job.Description;
            ViewBag.jobDate = job.JobDate;
            ViewBag.JobOrderName = job.Customer.Name;
            ViewBag.isPosted = job.JobStatus.Status == "CLOSED" && jobCount > 0 ? true : false ;

            var jobpayment = db.JobPayments.Where(s => s.JobMainId == jobId).ToList();

            ViewBag.jobPosted = db.JobPosts.Where(s => s.JobMainId == jobId).ToList();

            //expenses
            PartialView_CashPayments(jobId);

            getTotalBalance(jobExps, jobpayment);

            //ViewBag.jobAmount = db.JobMains.Find(jobId).AgreedAmt != null ? db.JobMains.Find(jobId).AgreedAmt : 0 ;
            ViewBag.jobAmount = getTotalQuotedAmount(db.JobServices.Where(s=>s.JobMainId == jobId).ToList());
            ViewBag.totalPayment = getTotalPayment(jobpayment);
            ViewBag.totalExpenses = getTotalExpenses(jobExps);

            //income
            ViewBag.carRentalIncome = getServiceIncome(jobId,"Car Rental");
            ViewBag.tourIncome = getServiceIncome(jobId, "Tour Package");
            ViewBag.othersIncome = getServiceIncome(jobId, "Others");

            //status
            ViewBag.jobstatus = job.JobStatus.Status;
            return View(jobExps);
        }

        public void PartialView_CashPayments(int jobId)
        {
            //cJobPayment
            var payment = db.JobPayments.Where(s => s.JobMainId == jobId).ToList();

            ViewBag.PaymentRecord = payment;
            ViewBag.totalPayment = getTotalPayment(payment);
            
        } 
        
        public void getTotalBalance(IEnumerable<JobExpenses> expenses, IEnumerable<JobPayment> payments)
        {
            ViewBag.totalBalance = getTotalPayment(payments) - getTotalExpenses(expenses);
        }

        public decimal getTotalExpenses(IEnumerable<JobExpenses> expenses)
        {
            decimal total = 0;
            foreach (var exp in expenses)
            {
                total += exp.Amount;
            }
            return total;
        }
        
        public decimal getTotalPayment(IEnumerable<JobPayment> payments)
        {
            decimal total = 0;
            foreach (var exp in payments)
            {
                total += exp.PaymentAmt;
            }
            return total;
        }
        
        public decimal getTotalQuotedAmount(IEnumerable<JobServices> services)
        {
            decimal total = 0;
            foreach (var svc in services)
            {
                total += svc.QuotedAmt != null ? (decimal)svc.QuotedAmt : 0;
            }
            return total;
        }
        
        public decimal getTotalQuotedAmount(int jobid)
        {
            decimal total = 0;
            
            return total;
        }

        public decimal getServiceIncome(int jobid, string serviceType)
        {
            decimal total = 0;
            //int count = 0;

            var carRentalServices = db.JobServices.Where(s => s.JobMainId == jobid).ToList();
            List<int> svcIds = new List<int>();

            //get total quoted amount
            foreach (var csvc in carRentalServices)
            {
                if (serviceType == "Car Rental")
                {
                    if (csvc.Service.Id == 1) { //Car Rental
                        total += getIncome(csvc);
                    }
                }else if (serviceType == "Tour Package")
                {
                    if (csvc.Service.Id == 3) { //Car Rental
                        total += getIncome(csvc);
                    }
                }else
                {
                    if (csvc.Service.Id == 2 || csvc.Service.Id == 4  
                     || csvc.Service.Id == 5 || csvc.Service.Id == 6
                     || csvc.Service.Id == 7)
                    { //Car Rental
                        total += getIncome(csvc);
                    }
                }
            }

            //get total expenses 

            return total;
        }

        private decimal getIncome(JobServices csvc)
        {
            decimal income = 0;
            income += csvc.ActualAmt != null ? (decimal)csvc.ActualAmt: 0;

            //get expenses based on service id
            var expenses = db.JobExpenses.Where(e => e.JobServicesId == csvc.Id).ToList();

            foreach (var exp in expenses)
            {
                income -= exp.Amount;
            }
            return income;
        }

        //Post and close job payments, expenses and income
        //table : jobPosting
        public string PostJobAccount(int jobId, decimal paymentAmount, decimal expensesAmount
            ,decimal incomeAmount, decimal carRentalIncome, decimal tourIncome, decimal otherIncome
            ,string remarks)
        {
            DateClass date = new DateClass();

            if (jobId != 0)
            {
                JobPost posting = new JobPost();
                posting.DtPost = date.GetCurrentDateTime();
                posting.JobMainId = jobId;
                posting.PaymentAmt = paymentAmount;
                posting.ExpensesAmt = expensesAmount;
                posting.CarRentalInc = carRentalIncome;
                posting.TourInc = tourIncome;
                posting.OthersInc = otherIncome;
                posting.Remarks = remarks;

                db.JobPosts.Add(posting);
                db.SaveChanges();

                //change job status to close
                //closeJob(jobId);

                //return RedirectToAction("JobServices","JobOrder",new { JobMainId = jobId , action  = "JOBPOST"});
                return JsonConvert.SerializeObject("200", Formatting.Indented);
            }
            //return RedirectToAction("Index","JobPosts",null);
            return JsonConvert.SerializeObject("200", Formatting.Indented);
        }
        
        public string closeJob(int jobId)
        {
            try
            {
                if (jobId != 0)
                {
                    JobMain job = db.JobMains.Find(jobId);
                    job.JobStatusId = 4;
                    
                    db.Entry(job).State = EntityState.Modified;
                    db.SaveChanges();

                    return JsonConvert.SerializeObject("200", Formatting.Indented);
                }
                return JsonConvert.SerializeObject("500", Formatting.Indented);
            }
            catch (Exception ex)
            { return JsonConvert.SerializeObject(ex, Formatting.Indented); }
        }

        #endregion

        #region Posted Account
        public ActionResult PostedDelete(int? id)
        {
            if(id != null)
            {

                JobPost posted = db.JobPosts.Find(id);
                var jobid = posted.JobMainId;
                db.JobPosts.Remove(posted);
                db.SaveChanges();
                return RedirectToAction("CashExpenses", new { jobId = jobid });
            }
            return RedirectToAction("Index");
        }

        public ActionResult EditPost(int id, decimal carRentalIncome, decimal tourIncome, decimal otherIncome, string remarks) {

            JobPost posted = db.JobPosts.Find(id);
            posted.CarRentalInc = carRentalIncome;
            posted.TourInc = tourIncome;
            posted.OthersInc = otherIncome;
            posted.Remarks = remarks;

            db.Entry(posted).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("CashExpenses", new { jobId = posted.JobMainId });
        }
        #endregion
    }
}
