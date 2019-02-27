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
    public class CarRatePackagesController : Controller
    {
        private List<SelectListItem> StatusList = new List<SelectListItem> {
                new SelectListItem { Value = "ACT", Text = "Active" },
                new SelectListItem { Value = "INC", Text = "Inactive" },
                new SelectListItem { Value = "SYS", Text = "Self drive" }
                };

        private JobDBContainer db = new JobDBContainer();

        // GET: CarRatePackages
        public ActionResult Index(int? showActive)
        {
            List<CarRatePackage> packageList = new List<CarRatePackage>();

            if(showActive != null)
            {
                if(showActive == 1)
                {
                    //show active packages
                    packageList = db.CarRatePackages.Where(p => p.Status == "ACT").ToList();
                }
                else if(showActive == 2)
                {
                    //show active packages
                    packageList = db.CarRatePackages.Where(p => p.Status == "INC").ToList();
                }
                else
                {
                    //show all
                    packageList = db.CarRatePackages.ToList();
                }
            }
            else
            {
                //show all
                packageList = db.CarRatePackages.ToList();
            }

            return View(packageList);
        }

        // GET: CarRatePackages/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CarRatePackage carRatePackage = db.CarRatePackages.Find(id);
            if (carRatePackage == null)
            {
                return HttpNotFound();
            }
            return View(carRatePackage);
        }

        // GET: CarRatePackages/Create
        public ActionResult Create()
        {
            ViewBag.Status = new SelectList(StatusList, "value", "text", "ACT");
            return View();
        }

        // POST: CarRatePackages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Description,Remarks,DailyMeals,DailyRoom,DaysMin,Status")] CarRatePackage carRatePackage)
        {
            if (ModelState.IsValid)
            {
                db.CarRatePackages.Add(carRatePackage);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Status = new SelectList(StatusList, "value", "text", carRatePackage.Status);
            return View(carRatePackage);
        }

        // GET: CarRatePackages/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CarRatePackage carRatePackage = db.CarRatePackages.Find(id);
            if (carRatePackage == null)
            {
                return HttpNotFound();
            }

            ViewBag.Status = new SelectList(StatusList, "value", "text", carRatePackage.Status);
            return View(carRatePackage);
        }

        // POST: CarRatePackages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Description,Remarks,DailyMeals,DailyRoom,DaysMin,Status")] CarRatePackage carRatePackage)
        {
            if (ModelState.IsValid)
            {
                db.Entry(carRatePackage).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Status = new SelectList(StatusList, "value", "text", carRatePackage.Status);
            return View(carRatePackage);
        }

        // GET: CarRatePackages/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CarRatePackage carRatePackage = db.CarRatePackages.Find(id);
            if (carRatePackage == null)
            {
                return HttpNotFound();
            }
            return View(carRatePackage);
        }

        // POST: CarRatePackages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CarRatePackage carRatePackage = db.CarRatePackages.Find(id);
            db.CarRatePackages.Remove(carRatePackage);
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
