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
    public class ExpensesController : Controller
    {
        private JobDBContainer db = new JobDBContainer();

        // GET: Expenses
        public ActionResult Index()
        {
            return View(db.Expenses.ToList());
        }

        // GET: Expenses/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Expenses expenses = db.Expenses.Find(id);
            if (expenses == null)
            {
                return HttpNotFound();
            }
            return View(expenses);
        }

        // GET: Expenses/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Expenses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Remarks,SeqNo")] Expenses expenses)
        {
            if (ModelState.IsValid)
            {
                db.Expenses.Add(expenses);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(expenses);
        }

        // GET: Expenses/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Expenses expenses = db.Expenses.Find(id);
            if (expenses == null)
            {
                return HttpNotFound();
            }
            return View(expenses);
        }

        // POST: Expenses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Remarks,SeqNo")] Expenses expenses)
        {
            if (ModelState.IsValid)
            {
                db.Entry(expenses).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(expenses);
        }

        // GET: Expenses/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Expenses expenses = db.Expenses.Find(id);
            if (expenses == null)
            {
                return HttpNotFound();
            }
            return View(expenses);
        }

        // POST: Expenses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Expenses expenses = db.Expenses.Find(id);
            db.Expenses.Remove(expenses);
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
