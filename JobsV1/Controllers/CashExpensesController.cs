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
    public class CashExpensesController : Controller
    {
        private JobDBContainer db = new JobDBContainer();

        // GET: CashExpenses
        public ActionResult Index()
        {
            var cashExpenses = db.CashExpenses.Include(c => c.JobMain);
            return View(cashExpenses.ToList());
        }

        public ActionResult JobExpenses(int? id)
        {
            if (id != null)
            {
                var cashExpenses = db.CashExpenses.Where(s=>s.JobMainId == id).Include(c => c.JobMain);
                ViewBag.JobName = db.JobMains.Find(id).Description;
                ViewBag.JobId = id;
                return View(cashExpenses.OrderByDescending(c=>c.DtExpense).ToList());
            }

            ViewBag.JobName = "";
            return Redirect("Shared/Error.cshtml");
        }

        // GET: CashExpenses/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CashExpense cashExpense = db.CashExpenses.Find(id);
            if (cashExpense == null)
            {
                return HttpNotFound();
            }
            return View(cashExpense);
        }

        // GET: CashExpenses/Create
        public ActionResult Create(int? jobid)
        {

            CashExpense expense = new CashExpense();
            expense.DtExpense = GetCurrentTime();
            expense.RecievedBy = HttpContext.User.Identity.Name;
            if (jobid == null)
            {
                jobid = 1;
            }
            ViewBag.JobMainId = new SelectList(db.JobMains, "Id", "Description", jobid);

            return View(expense);
        }

        // POST: CashExpenses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,JobMainId,DtExpense,Amount,Remarks,RecievedBy,ReleasedBy")] CashExpense cashExpense)
        {
            if (ModelState.IsValid)
            {
                db.CashExpenses.Add(cashExpense);
                db.SaveChanges();
                //return RedirectToAction("Index");
                return RedirectToAction("JobExpenses" , new { id = cashExpense.JobMainId });
            }

            ViewBag.JobMainId = new SelectList(db.JobMains, "Id", "Description", cashExpense.JobMainId);
            return View(cashExpense);
        }

        // GET: CashExpenses/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CashExpense cashExpense = db.CashExpenses.Find(id);

            //check if the release fields is empty, 
            //if not, assign to current user
            cashExpense.ReleasedBy = String.IsNullOrEmpty(cashExpense.ReleasedBy) ? HttpContext.User.Identity.Name : cashExpense.ReleasedBy;
            
            if (cashExpense == null)
            {
                return HttpNotFound();
            }
            ViewBag.JobMainId = new SelectList(db.JobMains, "Id", "Description", cashExpense.JobMainId);
            return View(cashExpense);
        }

        // POST: CashExpenses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,JobMainId,DtExpense,Amount,Remarks,RecievedBy,ReleasedBy")] CashExpense cashExpense)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cashExpense).State = EntityState.Modified;
                db.SaveChanges();
               // return RedirectToAction("Index");
                return RedirectToAction("JobExpenses", new { id = cashExpense.JobMainId });
            }
            ViewBag.JobMainId = new SelectList(db.JobMains, "Id", "Description", cashExpense.JobMainId);
            return View(cashExpense);
        }

        // GET: CashExpenses/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CashExpense cashExpense = db.CashExpenses.Find(id);
            if (cashExpense == null)
            {
                return HttpNotFound();
            }
            return View(cashExpense);
        }

        // POST: CashExpenses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CashExpense cashExpense = db.CashExpenses.Find(id);
            db.CashExpenses.Remove(cashExpense);
            db.SaveChanges();
            return RedirectToAction("JobExpenses", new { id = cashExpense.JobMainId });
            //return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        //get current time based on Singapore Standard Time 
        //SGT - UTC +8
        protected DateTime GetCurrentTime()
        {
            DateTime _localTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Singapore Standard Time"));

            return _localTime;
        }
    }
}
