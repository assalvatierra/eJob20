using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using JobsV1.Models;

namespace JobsV1.Areas.Masterlists.Controllers
{
    public class DataGroupAssignsController : Controller
    {
        private JobDBContainer db = new JobDBContainer();

        // GET: Masterlists/DataGroupAssigns
        public ActionResult Index()
        {
            var dataGroupAssigns = db.DataGroupAssigns.Include(d => d.DataGroup);
            return View(dataGroupAssigns.ToList());
        }

        // GET: Masterlists/DataGroupAssigns/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DataGroupAssign dataGroupAssign = db.DataGroupAssigns.Find(id);
            if (dataGroupAssign == null)
            {
                return HttpNotFound();
            }
            return View(dataGroupAssign);
        }

        // GET: Masterlists/DataGroupAssigns/Create
        public ActionResult Create()
        {
            ViewBag.DataGroupId = new SelectList(db.DataGroups, "Id", "Name");
            return View();
        }

        // POST: Masterlists/DataGroupAssigns/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,User,DataGroupId")] DataGroupAssign dataGroupAssign)
        {
            if (ModelState.IsValid)
            {
                db.DataGroupAssigns.Add(dataGroupAssign);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.DataGroupId = new SelectList(db.DataGroups, "Id", "Name", dataGroupAssign.DataGroupId);
            return View(dataGroupAssign);
        }

        // GET: Masterlists/DataGroupAssigns/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DataGroupAssign dataGroupAssign = db.DataGroupAssigns.Find(id);
            if (dataGroupAssign == null)
            {
                return HttpNotFound();
            }
            ViewBag.DataGroupId = new SelectList(db.DataGroups, "Id", "Name", dataGroupAssign.DataGroupId);
            return View(dataGroupAssign);
        }

        // POST: Masterlists/DataGroupAssigns/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,User,DataGroupId")] DataGroupAssign dataGroupAssign)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dataGroupAssign).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DataGroupId = new SelectList(db.DataGroups, "Id", "Name", dataGroupAssign.DataGroupId);
            return View(dataGroupAssign);
        }

        // GET: Masterlists/DataGroupAssigns/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DataGroupAssign dataGroupAssign = db.DataGroupAssigns.Find(id);
            if (dataGroupAssign == null)
            {
                return HttpNotFound();
            }
            return View(dataGroupAssign);
        }

        // POST: Masterlists/DataGroupAssigns/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DataGroupAssign dataGroupAssign = db.DataGroupAssigns.Find(id);
            db.DataGroupAssigns.Remove(dataGroupAssign);
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
