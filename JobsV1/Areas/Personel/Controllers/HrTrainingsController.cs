using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using JobsV1.Areas.Personel.Models;

namespace JobsV1.Areas.Personel.Controllers
{
    public class HrTrainingsController : Controller
    {
        private HrisDBContainer db = new HrisDBContainer();

        // GET: Personel/HrTrainings
        public ActionResult Index()
        {
            return View(db.HrTrainings.ToList());
        }

        // GET: Personel/HrTrainings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HrTraining hrTraining = db.HrTrainings.Find(id);
            if (hrTraining == null)
            {
                return HttpNotFound();
            }
            return View(hrTraining);
        }

        // GET: Personel/HrTrainings/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Personel/HrTrainings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Desc,Remarks")] HrTraining hrTraining)
        {
            if (ModelState.IsValid)
            {
                db.HrTrainings.Add(hrTraining);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(hrTraining);
        }

        // GET: Personel/HrTrainings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HrTraining hrTraining = db.HrTrainings.Find(id);
            if (hrTraining == null)
            {
                return HttpNotFound();
            }
            return View(hrTraining);
        }

        // POST: Personel/HrTrainings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Desc,Remarks")] HrTraining hrTraining)
        {
            if (ModelState.IsValid)
            {
                db.Entry(hrTraining).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(hrTraining);
        }

        // GET: Personel/HrTrainings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HrTraining hrTraining = db.HrTrainings.Find(id);
            if (hrTraining == null)
            {
                return HttpNotFound();
            }
            return View(hrTraining);
        }

        // POST: Personel/HrTrainings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            HrTraining hrTraining = db.HrTrainings.Find(id);
            db.HrTrainings.Remove(hrTraining);
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
