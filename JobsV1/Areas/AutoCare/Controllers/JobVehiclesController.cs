using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using JobsV1.Models;

namespace JobsV1.Areas.AutoCare.Controllers
{
    public class JobVehiclesController : Controller
    {
        private JobDBContainer db = new JobDBContainer();

        // GET: AutoCare/JobVehicles
        public ActionResult Index()
        {
            var jobVehicles = db.JobVehicles.Include(j => j.Vehicle).Include(j => j.JobMain);
            return View(jobVehicles.ToList());
        }

        // GET: AutoCare/JobVehicles/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobVehicle jobVehicle = db.JobVehicles.Find(id);
            if (jobVehicle == null)
            {
                return HttpNotFound();
            }
            return View(jobVehicle);
        }

        // GET: AutoCare/JobVehicles/Create
        public ActionResult Create()
        {
            ViewBag.VehicleId = new SelectList(db.Vehicles, "Id", "YearModel");
            ViewBag.JobMainId = new SelectList(db.JobMains, "Id", "Description");
            return View();
        }

        // POST: AutoCare/JobVehicles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,VehicleId,JobMainId,Mileage")] JobVehicle jobVehicle)
        {
            if (ModelState.IsValid)
            {
                db.JobVehicles.Add(jobVehicle);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.VehicleId = new SelectList(db.Vehicles, "Id", "YearModel", jobVehicle.VehicleId);
            ViewBag.JobMainId = new SelectList(db.JobMains, "Id", "Description", jobVehicle.JobMainId);
            return View(jobVehicle);
        }

        // GET: AutoCare/JobVehicles/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobVehicle jobVehicle = db.JobVehicles.Find(id);
            if (jobVehicle == null)
            {
                return HttpNotFound();
            }
            ViewBag.VehicleId = new SelectList(db.Vehicles, "Id", "YearModel", jobVehicle.VehicleId);
            ViewBag.JobMainId = new SelectList(db.JobMains, "Id", "Description", jobVehicle.JobMainId);
            return View(jobVehicle);
        }

        // POST: AutoCare/JobVehicles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,VehicleId,JobMainId,Mileage")] JobVehicle jobVehicle)
        {
            if (ModelState.IsValid)
            {
                db.Entry(jobVehicle).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.VehicleId = new SelectList(db.Vehicles, "Id", "YearModel", jobVehicle.VehicleId);
            ViewBag.JobMainId = new SelectList(db.JobMains, "Id", "Description", jobVehicle.JobMainId);
            return View(jobVehicle);
        }

        // GET: AutoCare/JobVehicles/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobVehicle jobVehicle = db.JobVehicles.Find(id);
            if (jobVehicle == null)
            {
                return HttpNotFound();
            }
            return View(jobVehicle);
        }

        // POST: AutoCare/JobVehicles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                JobVehicle jobVehicle = db.JobVehicles.Find(id);
                var vehicleId = jobVehicle.VehicleId;
                var jobId = jobVehicle.JobMainId;
                db.JobVehicles.Remove(jobVehicle);
                db.SaveChanges();

                //updat job desc 
                UpdateJobDesc(jobId, "No Vehicle Assigned");

                return RedirectToAction("VehicleServices", "Vehicles",new { id = vehicleId });
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        //update job description
        public bool UpdateJobDesc(int jobId, string Desc)
        {
            try
            {
                //get job details
                var jobmain = db.JobMains.Find(jobId);

                if (jobmain == null)
                    return false;

                //change desc
                jobmain.Description = Desc;

                db.Entry(jobmain).State = EntityState.Modified;
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
