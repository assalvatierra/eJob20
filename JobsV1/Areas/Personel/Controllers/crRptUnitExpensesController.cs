using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using JobsV1.Areas.Personel.Models;
using JobsV1.Models;

namespace JobsV1.Areas.Personel.Controllers
{
    public class crRptUnitExpensesController : Controller
    {
        private CarRentalLogDBContainer db = new CarRentalLogDBContainer();
        private CrDataLayer dl = new CrDataLayer();
        private DateClass dt = new DateClass();
        private List<SelectListItem> StatusList = new List<SelectListItem> {
                new SelectListItem { Value = "ACT", Text = "Active" },
                new SelectListItem { Value = "INC", Text = "Inactive" }
                };


        // GET: Personel/crRptUnitExpenses
        public ActionResult Index()
        {
            ViewBag.crLogUnitList = dl.GetUnits().ToList();
            ViewBag.crLogDriverList = dl.GetDrivers().ToList();
            return View(db.crRptUnitExpenses.ToList());
        }

        // GET: Personel/crRptUnitExpenses/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            crRptUnitExpense crRptUnitExpense = db.crRptUnitExpenses.Find(id);
            if (crRptUnitExpense == null)
            {
                return HttpNotFound();
            }
            return View(crRptUnitExpense);
        }

        // GET: Personel/crRptUnitExpenses/Create
        public ActionResult Create()
        {
            crRptUnitExpense unitExpense = new crRptUnitExpense();

            if (db.crRptUnitExpenses.ToList().Count() > 0)
            {
                unitExpense.RptNo = db.crRptUnitExpenses.OrderByDescending(c => c.Id).FirstOrDefault().RptNo + 1;
            }
            else
            {
                unitExpense.RptNo = 1;
            }

            ViewBag.Status = new SelectList(StatusList, "value", "text");
            return View(unitExpense);
        }

        // POST: Personel/crRptUnitExpenses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,RptName,RptNo,Status")] crRptUnitExpense crRptUnitExpense)
        {
            if (ModelState.IsValid)
            {
                db.crRptUnitExpenses.Add(crRptUnitExpense);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Status = new SelectList(StatusList, "value", "text", crRptUnitExpense.Status);
            return View(crRptUnitExpense);
        }

        // GET: Personel/crRptUnitExpenses/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            crRptUnitExpense crRptUnitExpense = db.crRptUnitExpenses.Find(id);
            if (crRptUnitExpense == null)
            {
                return HttpNotFound();
            }
            ViewBag.Status = new SelectList(StatusList, "value", "text", crRptUnitExpense.Status);
            return View(crRptUnitExpense);
        }

        // POST: Personel/crRptUnitExpenses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,RptName,RptNo,Status")] crRptUnitExpense crRptUnitExpense)
        {
            if (ModelState.IsValid)
            {
                db.Entry(crRptUnitExpense).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Status = new SelectList(StatusList, "value", "text", crRptUnitExpense.Status);
            return View(crRptUnitExpense);
        }

        // GET: Personel/crRptUnitExpenses/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            crRptUnitExpense crRptUnitExpense = db.crRptUnitExpenses.Find(id);
            if (crRptUnitExpense == null)
            {
                return HttpNotFound();
            }
            return View(crRptUnitExpense);
        }

        // POST: Personel/crRptUnitExpenses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            crRptUnitExpense crRptUnitExpense = db.crRptUnitExpenses.Find(id);
            db.crRptUnitExpenses.Remove(crRptUnitExpense);
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
