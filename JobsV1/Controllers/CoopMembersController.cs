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
    public class CoopMembersController : Controller
    {

        private List<SelectListItem> StatusList = new List<SelectListItem> {
                new SelectListItem { Value = "ACT", Text = "Active" },
                new SelectListItem { Value = "INC", Text = "Inactive" },
                new SelectListItem { Value = "BAD", Text = "Bad Account" }
                };

        private JobDBContainer db = new JobDBContainer();

        // GET: CoopMembers
        public ActionResult Index()
        {
            return View(db.CoopMembers.ToList());
        }

        // GET: CoopMembers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CoopMember coopMember = db.CoopMembers.Find(id);
            if (coopMember == null)
            {
                return HttpNotFound();
            }
            return View(coopMember);
        }

        // GET: CoopMembers/Create
        public ActionResult Create()
        {
            ViewBag.Status = new SelectList(StatusList, "value", "text");
            return View();
        }

        // POST: CoopMembers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Contact1,Contact2,Contact3,Email,Details,Status")] CoopMember coopMember)
        {
            if (ModelState.IsValid)
            {
                db.CoopMembers.Add(coopMember);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(coopMember);
        }

        // GET: CoopMembers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CoopMember coopMember = db.CoopMembers.Find(id);
            if (coopMember == null)
            {
                return HttpNotFound();
            }
            return View(coopMember);
        }

        // POST: CoopMembers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Contact1,Contact2,Contact3,Email,Details,Status")] CoopMember coopMember)
        {
            if (ModelState.IsValid)
            {
                db.Entry(coopMember).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(coopMember);
        }

        // GET: CoopMembers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CoopMember coopMember = db.CoopMembers.Find(id);
            if (coopMember == null)
            {
                return HttpNotFound();
            }
            return View(coopMember);
        }

        // POST: CoopMembers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CoopMember coopMember = db.CoopMembers.Find(id);
            db.CoopMembers.Remove(coopMember);
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
