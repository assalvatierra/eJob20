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
    public class JobItinerariesController : Controller
    {
        private JobDBContainer db = new JobDBContainer();

        // GET: JobItineraries
        public ActionResult Index(int? SvcId)
        {
            IList<Models.JobItinerary> jobItineraries = db.JobItineraries.Include(j => j.JobMain).Include(j => j.Destination).OrderBy( d=>d.ItiDate).ToList();
            return View(jobItineraries.ToList());
            
        }

        public ActionResult JobItinerary(int? id, int? SvcId)
        {
            var Job = db.JobMains.Where(d => d.Id == id).FirstOrDefault();
            ViewBag.JobOrder = Job;
            ViewBag.SvcParticular = "";

            IList<Models.JobItinerary> jobItineraries = db.JobItineraries.Include(j => j.JobMain).Include(j => j.Destination).Where(d=>d.JobMainId==id).OrderBy( d=> new { d.SvcId, d.ItiDate }).ToList();
            if (SvcId != null)
            {
                TempData["SERVICEID"] = (int)SvcId;

                jobItineraries = jobItineraries.Where(d => d.SvcId == (int)SvcId).ToList();
                var svc = db.JobServices.Find(SvcId);
                ViewBag.SvcParticular = svc.Id + " - " + svc.Particulars;
            }


            return View("Index", jobItineraries);
        }

        // GET: JobItineraries/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobItinerary jobItinerary = db.JobItineraries.Find(id);
            if (jobItinerary == null)
            {
                return HttpNotFound();
            }
            return View(jobItinerary);
        }

        // GET: JobItineraries/Create
        public ActionResult Create(int? JobMainId)
        {
            int SvcId = 0;
            if ( TempData.ContainsKey("SERVICEID") )
                SvcId = (int)TempData.Peek("SERVICEID");

            Models.JobItinerary ji = new Models.JobItinerary();
            ji.JobMainId = (int) JobMainId;
            if(SvcId > 0) ji.ItiDate = (DateTime)db.JobServices.Find(SvcId).DtStart;

            ViewBag.JobMainId = new SelectList(db.JobMains, "Id", "Description");
            ViewBag.DestinationId = new SelectList(db.Destinations, "Id", "Description");
            ViewBag.SvcId = new SelectList(db.JobServices.Where(d => d.JobMainId == JobMainId), "Id", "Particulars",SvcId);
            return View(ji);
        }

        // POST: JobItineraries/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,JobMainId,DestinationId,ActualRate,Remarks,ItiDate,SvcId")] JobItinerary jobItinerary)
        {
            if (ModelState.IsValid)
            {
                db.JobItineraries.Add(jobItinerary);
                db.SaveChanges();
                return RedirectToAction("JobItinerary", new { id = jobItinerary.JobMainId });
            }

            ViewBag.JobMainId = new SelectList(db.JobMains, "Id", "Description", jobItinerary.JobMainId);
            ViewBag.DestinationId = new SelectList(db.Destinations, "Id", "Description", jobItinerary.DestinationId);
            ViewBag.SvcId = new SelectList(db.JobServices.Where(d => d.JobMainId == jobItinerary.JobMainId), "Id", "Particulars", jobItinerary.SvcId);
            return View(jobItinerary);
        }

        // GET: JobItineraries/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobItinerary jobItinerary = db.JobItineraries.Find(id);
            if (jobItinerary == null)
            {
                return HttpNotFound();
            }
            ViewBag.JobMainId = new SelectList(db.JobMains, "Id", "Description", jobItinerary.JobMainId);
            ViewBag.DestinationId = new SelectList(db.Destinations, "Id", "Description", jobItinerary.DestinationId);
            ViewBag.SvcId = new SelectList(db.JobServices.Where(d => d.JobMainId == jobItinerary.JobMainId), "Id", "Particulars",jobItinerary.SvcId);

            return View(jobItinerary);
        }

        // POST: JobItineraries/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,JobMainId,DestinationId,ActualRate,Remarks,ItiDate,SvcId")] JobItinerary jobItinerary)
        {
            if (ModelState.IsValid)
            {
                db.Entry(jobItinerary).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("JobItinerary", new { id = jobItinerary.JobMainId });
            }
            ViewBag.JobMainId = new SelectList(db.JobMains, "Id", "Description", jobItinerary.JobMainId);
            ViewBag.DestinationId = new SelectList(db.Destinations, "Id", "Description", jobItinerary.DestinationId);
            ViewBag.SvcId = new SelectList(db.JobServices.Where(d => d.JobMainId == jobItinerary.JobMainId), "Id", "Particulars", jobItinerary.SvcId);

            return View(jobItinerary);
        }

        // GET: JobItineraries/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobItinerary jobItinerary = db.JobItineraries.Find(id);
            if (jobItinerary == null)
            {
                return HttpNotFound();
            }
            return View(jobItinerary);
        }

        // POST: JobItineraries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            JobItinerary jobItinerary = db.JobItineraries.Find(id);
            db.JobItineraries.Remove(jobItinerary);
            db.SaveChanges();
            return RedirectToAction("JobItinerary", new { id = jobItinerary.JobMainId });
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
