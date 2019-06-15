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

namespace JobsV1.Controllers
{
    public class JobExpensesController : Controller
    {
        private JobDBContainer db = new JobDBContainer();

        // GET: JobExpenses
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
            ViewBag.ExpensesId = new SelectList(db.Expenses, "Id", "Name");
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
            var expenses = db.JobExpenses.Where(e => e.JobServicesId == id).ToList();

            ViewBag.JobServiceName = db.JobServices.Find(id).Particulars;
            ViewBag.expenseList = db.Expenses.Include(s => s.ExpensesCategory).ToList();
            ViewBag.JobMainId = db.JobServices.Find(id).JobMainId;
            ViewBag.JobServiceId = id;
            return View(expenses);
        }

        public string JobExpensesAdd(int? id, int expenseId, decimal amount, string remarks)
        {
            if (id != null)
            {

                JobExpenses jobExpense = new JobExpenses();
                jobExpense.Amount = amount;
                jobExpense.ExpensesId = expenseId;
                jobExpense.JobServicesId = (int)id;
                jobExpense.Remarks = remarks;

                db.JobExpenses.Add(jobExpense);
                db.SaveChanges();

                //adding new job expense successful
                return JsonConvert.SerializeObject("200", Formatting.Indented);

            }

            //adding new job expense error
            return JsonConvert.SerializeObject("500", Formatting.Indented);
        }

        public string JobExpensesEdit(int? id, int expenseId, decimal amount, string remarks)
        {
            if (id != null)
            {

                JobExpenses jobExpense = db.JobExpenses.Find(id);
                jobExpense.Amount = amount;
                jobExpense.ExpensesId = expenseId;
                jobExpense.Remarks = remarks;

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

            ViewBag.JobMainId = jobId;
            ViewBag.jobservices = db.JobServices.Where(s => s.JobMainId == jobId).ToList();
            ViewBag.totalExpenses = getTotalExpenses(jobExps);
            ViewBag.jobDesc = db.JobMains.Find(jobId).Description;
            ViewBag.jobDate = db.JobMains.Find(jobId).JobDate;
            ViewBag.JobOrderName = db.JobMains.Find(jobId).Customer.Name;
            return View(jobExps);
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
        #endregion

    }
}
