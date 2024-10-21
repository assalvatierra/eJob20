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
    public class DataGroupsController : Controller
    {
        private JobDBContainer db = new JobDBContainer();
        private DBClasses dbclasses = new DBClasses();

        // GET: Masterlists/DataGroups
        public ActionResult Index()
        {
            return View(db.DataGroups.ToList());
        }

        // GET: Masterlists/DataGroups/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DataGroup dataGroup = db.DataGroups.Find(id);
            if (dataGroup == null)
            {
                return HttpNotFound();
            }
            return View(dataGroup);
        }

        // GET: Masterlists/DataGroups/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Masterlists/DataGroups/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Remarks")] DataGroup dataGroup)
        {
            if (ModelState.IsValid)
            {
                db.DataGroups.Add(dataGroup);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(dataGroup);
        }

        // GET: Masterlists/DataGroups/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DataGroup dataGroup = db.DataGroups.Find(id);
            if (dataGroup == null)
            {
                return HttpNotFound();
            }
            return View(dataGroup);
        }

        // POST: Masterlists/DataGroups/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Remarks")] DataGroup dataGroup)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dataGroup).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(dataGroup);
        }

        // GET: Masterlists/DataGroups/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DataGroup dataGroup = db.DataGroups.Find(id);
            if (dataGroup == null)
            {
                return HttpNotFound();
            }
            return View(dataGroup);
        }

        // POST: Masterlists/DataGroups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DataGroup dataGroup = db.DataGroups.Find(id);
            db.DataGroups.Remove(dataGroup);
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


        // GET: Masterlists/DataGroupAssigns/Create
        public ActionResult CreateAssign(int groupId)
        {
            if (groupId == null)
            {
                groupId = 1;
            }
            ViewBag.User = new SelectList(dbclasses.getUsers_wdException(), "UserName", "UserName");
            ViewBag.DataGroupId = new SelectList(db.DataGroups, "Id", "Name", groupId);
            return View();
        }

        // POST: Masterlists/DataGroupAssigns/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateAssign([Bind(Include = "Id,User,DataGroupId")] DataGroupAssign dataGroupAssign)
        {
            if (ModelState.IsValid)
            {
                db.DataGroupAssigns.Add(dataGroupAssign);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.User = new SelectList(dbclasses.getUsers_wdException(), "UserName", "UserName");
            ViewBag.DataGroupId = new SelectList(db.DataGroups, "Id", "Name", dataGroupAssign.DataGroupId);
            return View(dataGroupAssign);
        }


        // GET: Masterlists/DataGroupAssigns/Delete/5
        public ActionResult DeleteAssign(int? id)
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
        [HttpPost, ActionName("DeleteAssign")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteAssignConfirmed(int id)
        {
            DataGroupAssign dataGroupAssign = db.DataGroupAssigns.Find(id);
            db.DataGroupAssigns.Remove(dataGroupAssign);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
