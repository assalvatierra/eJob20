﻿using System;
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
    public class JobPostsController : Controller
    {
        private JobDBContainer db = new JobDBContainer();

        // GET: JobPosts
        public ActionResult Index()
        {
            var jobPosts = db.JobPosts.Include(j => j.JobMain);
            return View(jobPosts.ToList());
        }

        // GET: JobPosts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobPost jobPost = db.JobPosts.Find(id);
            if (jobPost == null)
            {
                return HttpNotFound();
            }
            return View(jobPost);
        }

        // GET: JobPosts/Create
        public ActionResult Create()
        {
            ViewBag.JobMainId = new SelectList(db.JobMains, "Id", "Description");
            return View();
        }

        // POST: JobPosts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,DtPost,PaymentAmt,ExpensesAmt,CarRentalInc,TourInc,OthersInc,Remarks,JobMainId")] JobPost jobPost)
        {
            if (ModelState.IsValid)
            {
                db.JobPosts.Add(jobPost);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.JobMainId = new SelectList(db.JobMains, "Id", "Description", jobPost.JobMainId);
            return View(jobPost);
        }

        // GET: JobPosts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobPost jobPost = db.JobPosts.Find(id);
            if (jobPost == null)
            {
                return HttpNotFound();
            }
            ViewBag.JobMainId = new SelectList(db.JobMains, "Id", "Description", jobPost.JobMainId);
            return View(jobPost);
        }

        // POST: JobPosts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,DtPost,PaymentAmt,ExpensesAmt,CarRentalInc,TourInc,OthersInc,Remarks,JobMainId")] JobPost jobPost)
        {
            if (ModelState.IsValid)
            {
                db.Entry(jobPost).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.JobMainId = new SelectList(db.JobMains, "Id", "Description", jobPost.JobMainId);
            return View(jobPost);
        }

        // GET: JobPosts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobPost jobPost = db.JobPosts.Find(id);
            if (jobPost == null)
            {
                return HttpNotFound();
            }
            return View(jobPost);
        }

        // POST: JobPosts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            JobPost jobPost = db.JobPosts.Find(id);
            db.JobPosts.Remove(jobPost);
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
