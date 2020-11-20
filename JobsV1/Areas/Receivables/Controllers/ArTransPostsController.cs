using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ArModels.Models;

namespace JobsV1.Areas.Receivables.Controllers
{
    public class ArTransPostsController : Controller
    {
        private ArDBContainer db = new ArDBContainer();

        // GET: Receivables/ArTransPosts
        public ActionResult Index()
        {
            var arTransPosts = db.ArTransPosts.Include(a => a.ArTransaction);
            return View(arTransPosts.ToList());
        }

        // GET: Receivables/ArTransPosts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ArTransPost arTransPost = db.ArTransPosts.Find(id);
            if (arTransPost == null)
            {
                return HttpNotFound();
            }
            return View(arTransPost);
        }

        // GET: Receivables/ArTransPosts/Create
        public ActionResult Create()
        {
            ViewBag.ArTransactionId = new SelectList(db.ArTransactions, "Id", "Description");
            return View();
        }

        // POST: Receivables/ArTransPosts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,DtPost,Amount,Balance,ArTransactionId")] ArTransPost arTransPost)
        {
            if (ModelState.IsValid)
            {
                db.ArTransPosts.Add(arTransPost);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ArTransactionId = new SelectList(db.ArTransactions, "Id", "Description", arTransPost.ArTransactionId);
            return View(arTransPost);
        }

        // GET: Receivables/ArTransPosts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ArTransPost arTransPost = db.ArTransPosts.Find(id);
            if (arTransPost == null)
            {
                return HttpNotFound();
            }
            ViewBag.ArTransactionId = new SelectList(db.ArTransactions, "Id", "Description", arTransPost.ArTransactionId);
            return View(arTransPost);
        }

        // POST: Receivables/ArTransPosts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,DtPost,Amount,Balance,ArTransactionId")] ArTransPost arTransPost)
        {
            if (ModelState.IsValid)
            {
                db.Entry(arTransPost).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ArTransactionId = new SelectList(db.ArTransactions, "Id", "Description", arTransPost.ArTransactionId);
            return View(arTransPost);
        }

        // GET: Receivables/ArTransPosts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ArTransPost arTransPost = db.ArTransPosts.Find(id);
            if (arTransPost == null)
            {
                return HttpNotFound();
            }
            return View(arTransPost);
        }

        // POST: Receivables/ArTransPosts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ArTransPost arTransPost = db.ArTransPosts.Find(id);
            db.ArTransPosts.Remove(arTransPost);
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
