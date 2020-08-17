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
    public class SvcGroupsController : Controller
    {
        private JobDBContainer db = new JobDBContainer();

        // GET: SvcGroups
        public ActionResult Index()
        {
            var svcGroups = db.SvcGroups.Include(s => s.Service).Include(s => s.SvcDetail);
            return View(svcGroups.ToList());
        }

        // GET: SvcGroups/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SvcGroup svcGroup = db.SvcGroups.Find(id);
            if (svcGroup == null)
            {
                return HttpNotFound();
            }
            return View(svcGroup);
        }

        // GET: SvcGroups/Create
        public ActionResult Create()
        {
            ViewBag.ServicesId = new SelectList(db.Services, "Id", "Name");
            ViewBag.SvcDetailId = new SelectList(db.SvcDetails, "Id", "Name");
            return View();
        }

        // POST: SvcGroups/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ServicesId,SvcDetailId")] SvcGroup svcGroup)
        {
            if (ModelState.IsValid)
            {
                db.SvcGroups.Add(svcGroup);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ServicesId = new SelectList(db.Services, "Id", "Name", svcGroup.ServicesId);
            ViewBag.SvcDetailId = new SelectList(db.SvcDetails, "Id", "Name", svcGroup.SvcDetailId);
            return View(svcGroup);
        }


        // GET: SvcGroups/Create
        public ActionResult AddService(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            SvcGroup svcGroup = new SvcGroup();
            svcGroup.SvcDetail = db.SvcDetails.Find(id);

            var GroupSvcIds = db.SvcGroups.Where(c => c.SvcDetailId == id).ToList().Select(c => c.ServicesId);

            ViewBag.ServicesId = new SelectList(db.Services.Where(s=>!GroupSvcIds.Contains(s.Id)), "Id", "Name");
            ViewBag.SvcDetailId = new SelectList(db.SvcDetails, "Id", "Name", id);
            return View(svcGroup);
        }

        // POST: SvcGroups/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddService([Bind(Include = "Id,ServicesId,SvcDetailId")] SvcGroup svcGroup)
        {
            if (ModelState.IsValid)
            {
                db.SvcGroups.Add(svcGroup);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ServicesId = new SelectList(db.Services, "Id", "Name", svcGroup.ServicesId);
            ViewBag.SvcDetailId = new SelectList(db.SvcDetails, "Id", "Name", svcGroup.SvcDetailId);
            return View(svcGroup);
        }


        // GET: SvcGroups/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SvcGroup svcGroup = db.SvcGroups.Find(id);
            if (svcGroup == null)
            {
                return HttpNotFound();
            }
            ViewBag.ServicesId = new SelectList(db.Services, "Id", "Name", svcGroup.ServicesId);
            ViewBag.SvcDetailId = new SelectList(db.SvcDetails, "Id", "Name", svcGroup.SvcDetailId);
            return View(svcGroup);
        }

        // POST: SvcGroups/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ServicesId,SvcDetailId")] SvcGroup svcGroup)
        {
            if (ModelState.IsValid)
            {
                db.Entry(svcGroup).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ServicesId = new SelectList(db.Services, "Id", "Name", svcGroup.ServicesId);
            ViewBag.SvcDetailId = new SelectList(db.SvcDetails, "Id", "Name", svcGroup.SvcDetailId);
            return View(svcGroup);
        }

        // GET: SvcGroups/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SvcGroup svcGroup = db.SvcGroups.Find(id);
            if (svcGroup == null)
            {
                return HttpNotFound();
            }
            return View(svcGroup);
        }

        // POST: SvcGroups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SvcGroup svcGroup = db.SvcGroups.Find(id);
            db.SvcGroups.Remove(svcGroup);
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
