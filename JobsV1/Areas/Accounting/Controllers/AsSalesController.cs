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
    public class AsSalesController : Controller
    {
        private AccountingDBContainer db = new AccountingDBContainer();

        // GET: Accounting/AsSales
        public ActionResult Index(int? iyr = 0, int? imon = 0, int? icc = 0)
        {
            if (iyr == 0 || imon == 0 || icc == 0)
                return RedirectToAction("ParamForm");

            //int iCost = 0;
            //if (icc == 1) iCost = 1;
            //if (icc == 1) iCost = 1;

            var asSales = db.AsSales.Include(a => a.AsCostCenter).Include(a => a.AsIncCategory).Include(a => a.AsIncClient)
                .Where(d=>d.TrxDate.Year==iyr && d.TrxDate.Month==imon && d.AsCostCenterId==icc).OrderBy(d=>d.OrRef);
            return View(asSales.ToList());
        }

        // GET: Accounting/AsSales/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AsSales asSales = db.AsSales.Find(id);
            if (asSales == null)
            {
                return HttpNotFound();
            }
            return View(asSales);
        }

        // GET: Accounting/AsSales/Create
        public ActionResult Create()
        {
            ViewBag.AsCostCenterId = new SelectList(db.AsCostCenters, "Id", "ccName");
            ViewBag.AsIncCategoryId = new SelectList(db.AsIncCategories, "Id", "Desc");
            ViewBag.AsIncClientId = new SelectList(db.AsIncClients, "Id", "ShortName");
            return View();
        }

        // POST: Accounting/AsSales/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,TrxDate,TrxDesc,Amount,DateEntered,AsCostCenterId,AsIncCategoryId,AsIncClientId, OrRef")] AsSales asSales)
        {
            if (ModelState.IsValid)
            {
                asSales.DateEntered = System.DateTime.Now;
                db.AsSales.Add(asSales);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AsCostCenterId = new SelectList(db.AsCostCenters, "Id", "ccName", asSales.AsCostCenterId);
            ViewBag.AsIncCategoryId = new SelectList(db.AsIncCategories, "Id", "Desc", asSales.AsIncCategoryId);
            ViewBag.AsIncClientId = new SelectList(db.AsIncClients, "Id", "ShortName", asSales.AsIncClientId);
            return View(asSales);
        }

        // GET: Accounting/AsSales/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AsSales asSales = db.AsSales.Find(id);
            if (asSales == null)
            {
                return HttpNotFound();
            }
            ViewBag.AsCostCenterId = new SelectList(db.AsCostCenters, "Id", "ccName", asSales.AsCostCenterId);
            ViewBag.AsIncCategoryId = new SelectList(db.AsIncCategories, "Id", "Desc", asSales.AsIncCategoryId);
            ViewBag.AsIncClientId = new SelectList(db.AsIncClients, "Id", "ShortName", asSales.AsIncClientId);
            return View(asSales);
        }

        // POST: Accounting/AsSales/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,TrxDate,TrxDesc,Amount,DateEntered,AsCostCenterId,AsIncCategoryId,AsIncClientId,OrRef")] AsSales asSales)
        {
            if (ModelState.IsValid)
            {
                //asSales.DateEntered = DateTime.Now;
                //asSales.DateEntered = (System.DateTime)asSales.DateEntered.ToString("MM/dd/yyyy");
                db.Entry(asSales).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AsCostCenterId = new SelectList(db.AsCostCenters, "Id", "ccName", asSales.AsCostCenterId);
            ViewBag.AsIncCategoryId = new SelectList(db.AsIncCategories, "Id", "Desc", asSales.AsIncCategoryId);
            ViewBag.AsIncClientId = new SelectList(db.AsIncClients, "Id", "ShortName", asSales.AsIncClientId);
            return View(asSales);
        }

        // GET: Accounting/AsSales/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AsSales asSales = db.AsSales.Find(id);
            if (asSales == null)
            {
                return HttpNotFound();
            }
            return View(asSales);
        }

        // POST: Accounting/AsSales/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AsSales asSales = db.AsSales.Find(id);
            db.AsSales.Remove(asSales);
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
            DateTime dtTrxFrom = DateTime.Now.AddMonths(-12);
            var data = db.AsSales.Where(d=> DateTime.Compare(dtTrxFrom, d.TrxDate) <=0 ).GroupBy(d => new { d.TrxDate.Year, d.TrxDate.Month, d.AsCostCenterId } ).Select(s =>
             new TrxSummary
             {
                 iCenter = s.FirstOrDefault().AsCostCenterId,
                 sCenter = s.FirstOrDefault().AsCostCenter.ccName,
                 year = s.FirstOrDefault().TrxDate.Year,
                 month = s.FirstOrDefault().TrxDate.Month,
                 total = s.Sum(s1 => s1.Amount),
                 trx = s.Count()
             }).ToList();


            List<ParamObject> param = new List<ParamObject>();

            DateTime dtNow = DateTime.Now;

            for(int i=0; i<12; i++)
            {
                int imon = dtNow.AddMonths((-1) * i).Month;
                int iyr = dtNow.AddMonths((-1) * i).Year;
                List<TrxSummary> cCenter = new List<TrxSummary>();

                foreach( TrxSummary c in data)
                {
                    if( imon == c.month && iyr == c.year)
                    {
                        cCenter.Add(c);
                    }
                }

                param.Add(new ParamObject {
                    month = dtNow.AddMonths((-1) * i).Month,
                    Year = dtNow.AddMonths((-1) * i).Year,
                    MonthName = dtNow.AddMonths((-1) * i).ToString("MMM"),
                    CostCenter = cCenter
                });
            }


            return View(param);
        }

    }

    public class ParamObject
    {
        public int month { get; set; }
        public int Year { get; set; }
        public string MonthName { get; set; }
        public List<TrxSummary> CostCenter { get; set; }
    }

    public class TrxSummary
    {
        public int iCenter { get; set; }
        public string sCenter { get; set; }
        public int year { get; set; }
        public int month { get; set; }
        public decimal total { get; set; }
        public int trx { get; set; } 
    }
}
