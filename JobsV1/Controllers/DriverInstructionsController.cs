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
    public class DriverInstructionsController : Controller
    {
        private JobDBContainer db = new JobDBContainer();

        // GET: DriverInstructions
        public ActionResult Index()
        {
            return View(db.DriverInstructions.ToList());
        }

        // GET: DriverInstructions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DriverInstructions driverInstructions = db.DriverInstructions.Find(id);
            if (driverInstructions == null)
            {
                return HttpNotFound();
            }
            return View(driverInstructions);
        }

        // GET: DriverInstructions/Create
        public ActionResult Create()
        {
            DriverInstructions instructions = new DriverInstructions();
            instructions.OrderNo = 10;
            return View(instructions);
        }

        // POST: DriverInstructions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Description,OrderNo")] DriverInstructions driverInstructions)
        {
            if (ModelState.IsValid)
            {
                db.DriverInstructions.Add(driverInstructions);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(driverInstructions);
        }

        // GET: DriverInstructions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DriverInstructions driverInstructions = db.DriverInstructions.Find(id);
            if (driverInstructions == null)
            {
                return HttpNotFound();
            }
            return View(driverInstructions);
        }

        // POST: DriverInstructions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Description,OrderNo")] DriverInstructions driverInstructions)
        {
            if (ModelState.IsValid)
            {
                db.Entry(driverInstructions).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(driverInstructions);
        }

        // GET: DriverInstructions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DriverInstructions driverInstructions = db.DriverInstructions.Find(id);
            if (driverInstructions == null)
            {
                return HttpNotFound();
            }
            return View(driverInstructions);
        }

        // POST: DriverInstructions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DriverInstructions driverInstructions = db.DriverInstructions.Find(id);
            db.DriverInstructions.Remove(driverInstructions);
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
