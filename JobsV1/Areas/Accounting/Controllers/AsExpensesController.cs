using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using JobsV1.Areas.Accounting.Models;

namespace JobsV1.Areas.Accounting.Controllers
{
    public class AsExpensesController : Controller
    {
        private AccountingDBContainer db = new AccountingDBContainer();

        // GET: Accounting/AsExpenses
        public ActionResult Index(int? iyr = 0, int? imon = 0, int? icc = 0)
        {
            if (iyr == 0 || imon == 0 || icc == 0)
                return RedirectToAction("ParamForm");

            var asExpenses = db.AsExpenses.Include(a => a.AsCostCenter).Include(a => a.AsExpCategory).Include(a => a.AsExpBiller)
                                .Where(d => d.TrxDate.Year == iyr && d.TrxDate.Month == imon && d.AsCostCenterId == icc);
            
            return View(asExpenses.ToList());
        }

        // GET: Accounting/AsExpenses/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AsExpense asExpense = db.AsExpenses.Find(id);
            if (asExpense == null)
            {
                return HttpNotFound();
            }
            return View(asExpense);
        }

        // GET: Accounting/AsExpenses/Create
        public ActionResult Create()
        {
            ViewBag.AsCostCenterId = new SelectList(db.AsCostCenters, "Id", "ccName");
            ViewBag.AsExpCategoryId = new SelectList(db.AsExpCategories, "Id", "Desc");
            ViewBag.AsExpBillerId = new SelectList(db.AsExpBillers, "Id", "ShortName");
            return View();
        }

        // POST: Accounting/AsExpenses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,TrxDate,TrxDesc,Amount,TrxRemarks,DateEntered,AsCostCenterId,AsExpCategoryId,AsExpBillerId")] AsExpense asExpense)
        {
            if (ModelState.IsValid)
            {
                asExpense.DateEntered = System.DateTime.Now;
                db.AsExpenses.Add(asExpense);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AsCostCenterId = new SelectList(db.AsCostCenters, "Id", "ccName", asExpense.AsCostCenterId);
            ViewBag.AsExpCategoryId = new SelectList(db.AsExpCategories, "Id", "Desc", asExpense.AsExpCategoryId);
            ViewBag.AsExpBillerId = new SelectList(db.AsExpBillers, "Id", "ShortName", asExpense.AsExpBillerId);
            return View(asExpense);
        }

        // GET: Accounting/AsExpenses/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AsExpense asExpense = db.AsExpenses.Find(id);
            if (asExpense == null)
            {
                return HttpNotFound();
            }
            ViewBag.AsCostCenterId = new SelectList(db.AsCostCenters, "Id", "ccName", asExpense.AsCostCenterId);
            ViewBag.AsExpCategoryId = new SelectList(db.AsExpCategories, "Id", "Desc", asExpense.AsExpCategoryId);
            ViewBag.AsExpBillerId = new SelectList(db.AsExpBillers, "Id", "ShortName", asExpense.AsExpBillerId);
            return View(asExpense);
        }

        // POST: Accounting/AsExpenses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,TrxDate,TrxDesc,Amount,TrxRemarks,DateEntered,AsCostCenterId,AsExpCategoryId,AsExpBillerId")] AsExpense asExpense)
        {
            if (ModelState.IsValid)
            {
                db.Entry(asExpense).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AsCostCenterId = new SelectList(db.AsCostCenters, "Id", "ccName", asExpense.AsCostCenterId);
            ViewBag.AsExpCategoryId = new SelectList(db.AsExpCategories, "Id", "Desc", asExpense.AsExpCategoryId);
            ViewBag.AsExpBillerId = new SelectList(db.AsExpBillers, "Id", "ShortName", asExpense.AsExpBillerId);
            return View(asExpense);
        }

        // GET: Accounting/AsExpenses/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AsExpense asExpense = db.AsExpenses.Find(id);
            if (asExpense == null)
            {
                return HttpNotFound();
            }
            return View(asExpense);
        }

        // POST: Accounting/AsExpenses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AsExpense asExpense = db.AsExpenses.Find(id);
            db.AsExpenses.Remove(asExpense);
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


        public ActionResult ParamForm()
        {
            List<ParamObject> param = new List<ParamObject>();

            DateTime dtNow = DateTime.Now;

            for (int i = 0; i < 12; i++)
            {
                param.Add(new ParamObject
                {
                    month = dtNow.AddMonths((-1) * i).Month,
                    Year = dtNow.AddMonths((-1) * i).Year,
                    MonthName = dtNow.AddMonths((-1) * i).ToString("MMM")
                });
            }


            return View(param);
        }



    }
}
