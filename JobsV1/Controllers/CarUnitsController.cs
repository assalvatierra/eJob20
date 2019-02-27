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
    public class CarUnitsController : Controller
    {
        private JobDBContainer db = new JobDBContainer();

        // GET: CarUnits
        public ActionResult Index()
        {

            var carUnits = db.CarUnits.Include(c => c.CarCategory).Include(r=>r.CarRates);
            return View(carUnits.ToList());
        }

        // GET: CarUnits/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CarUnit carUnit = db.CarUnits.Find(id);
            if (carUnit == null)
            {
                return HttpNotFound();
            }
            return View(carUnit);
        }

        // Rate
        public ActionResult CreateRate(int? unitid)
        {
            Models.CarRate rate = new CarRate();
            rate.CarUnitId = (int)unitid;
            
            return View(rate);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateRate([Bind(Include = "Id,Daily,Weekly,Monthly,KmFree,KmRate,CarUnitId,OtRate")] CarRate carRate)
        {
            if (ModelState.IsValid)
            {
                db.CarRates.Add(carRate);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(carRate);
        }

        public ActionResult Rates(int? id)
        {
            //var rate = db.CarRates.Where(d => d.CarUnitId == (int)id).FirstOrDefault();
            //if (rate == null)
            //{
            //    RedirectToAction("CreateRate", new { unitid = (int)id });
            //}

            ////rate = db.CarRates.Where(d => d.CarUnitId == (int)id).FirstOrDefault();
            //CarRate carRate = db.CarRates.Find(rate.Id);
            //return View(carRate);

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CarRate carRate = db.CarRates.Find(id);
            if (carRate == null)
            {
                return HttpNotFound();
            }
            ViewBag.CarUnitId = new SelectList(db.CarUnits, "Id", "Description", carRate.CarUnitId);
            return View(carRate);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public ActionResult Rates([Bind(Include = "Id, CarUnitId, Daily, Weekly, Monthly, KmFree, OtRate")] CarRate rate)
        //public ActionResult Rates([Bind(Include = "Id,Daily,Weekly,Monthly,KmFree,KmRate,CarUnitId,OtRate")] CarRate rate)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(rate).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
            
        //    return View(rate);
        //}
        public ActionResult Rates([Bind(Include = "Id,Daily,Weekly,Monthly,KmFree,KmRate,CarUnitId,OtRate")] CarRate carRate)
        {
            if (ModelState.IsValid)
            {
                db.Entry(carRate).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CarUnitId = new SelectList(db.CarUnits, "Id", "Description", carRate.CarUnitId);
            return View(carRate);
        }




        public ActionResult RateDetails(int? unitid)
        {
            return View(db.CarRates.Where(d => d.CarUnitId == unitid));
        }

        // GET: CarUnits/Create
        public ActionResult Create()
        {
            ViewBag.CarCategoryId = new SelectList(db.CarCategories, "Id", "Description");
            return View();
        }

        // POST: CarUnits/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Description,Remarks,CarCategoryId")] CarUnit carUnit)
        {
            if (ModelState.IsValid)
            {
                db.CarUnits.Add(carUnit);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CarCategoryId = new SelectList(db.CarCategories, "Id", "Description", carUnit.CarCategoryId);
            return View(carUnit);
        }

        // GET: CarUnits/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CarUnit carUnit = db.CarUnits.Find(id);
            if (carUnit == null)
            {
                return HttpNotFound();
            }
            ViewBag.CarCategoryId = new SelectList(db.CarCategories, "Id", "Description", carUnit.CarCategoryId);
            return View(carUnit);
        }

        // POST: CarUnits/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Description,Remarks,CarCategoryId")] CarUnit carUnit)
        {
            if (ModelState.IsValid)
            {
                db.Entry(carUnit).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CarCategoryId = new SelectList(db.CarCategories, "Id", "Description", carUnit.CarCategoryId);
            return View(carUnit);
        }

        // GET: CarUnits/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CarUnit carUnit = db.CarUnits.Find(id);
            if (carUnit == null)
            {
                return HttpNotFound();
            }
            return View(carUnit);
        }

        // POST: CarUnits/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CarUnit carUnit = db.CarUnits.Find(id);
            db.CarUnits.Remove(carUnit);
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
