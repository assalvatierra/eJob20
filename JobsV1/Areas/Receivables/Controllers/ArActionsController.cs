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
    public class ArActionsController : Controller
    {
        private ArDBContainer db = new ArDBContainer();

        // GET: Receivables/ArActions
        public ActionResult Index()
        {
            var arActions = db.ArActions.Include(a => a.ArTransaction).Include(a => a.ArActionItem);
            return View(arActions.ToList());
        }

        // GET: Receivables/ArActions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ArAction arAction = db.ArActions.Find(id);
            if (arAction == null)
            {
                return HttpNotFound();
            }
            return View(arAction);
        }

        // GET: Receivables/ArActions/Create
        public ActionResult Create(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ArAction action = new ArAction();
            action.PreformedBy = GetUser();

            ViewBag.TransactionId = id;
            ViewBag.ArTransactionId = new SelectList(db.ArTransactions, "Id", "Description", id);
            ViewBag.ArActionItemId = new SelectList(db.ArActionItems, "Id", "Action");
            return View(action);
        }

        // POST: Receivables/ArActions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,DtPerformed,PreformedBy,ArTransactionId,ArActionItemId,Remarks")] ArAction arAction)
        {
            if (ModelState.IsValid)
            {
                db.ArActions.Add(arAction);
                db.SaveChanges();

                //return RedirectToAction("Index");
                return RedirectToAction("TransactionHistory", "ArTransactions", new { id = arAction.ArTransactionId });
            }

            ViewBag.TransactionId = arAction.ArTransactionId;
            ViewBag.ArTransactionId = new SelectList(db.ArTransactions, "Id", "Description", arAction.ArTransactionId);
            ViewBag.ArActionItemId = new SelectList(db.ArActionItems, "Id", "Action", arAction.ArActionItemId);
            return View(arAction);
        }

        // GET: Receivables/ArActions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ArAction arAction = db.ArActions.Find(id);
            if (arAction == null)
            {
                return HttpNotFound();
            }
            ViewBag.ArTransactionId = new SelectList(db.ArTransactions, "Id", "Description", arAction.ArTransactionId);
            ViewBag.ArActionItemId = new SelectList(db.ArActionItems, "Id", "Action", arAction.ArActionItemId);
            return View(arAction);
        }

        // POST: Receivables/ArActions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,DtPerformed,PreformedBy,ArTransactionId,ArActionItemId,Remarks")] ArAction arAction)
        {
            if (ModelState.IsValid)
            {
                db.Entry(arAction).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("TransactionHistory", "ArTransactions", new { id = arAction.ArTransactionId });
                //return RedirectToAction("Index");
            }
            ViewBag.ArTransactionId = new SelectList(db.ArTransactions, "Id", "Description", arAction.ArTransactionId);
            ViewBag.ArActionItemId = new SelectList(db.ArActionItems, "Id", "Action", arAction.ArActionItemId);
            return View(arAction);
        }

        // GET: Receivables/ArActions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ArAction arAction = db.ArActions.Find(id);
            if (arAction == null)
            {
                return HttpNotFound();
            }
            return View(arAction);
        }

        // POST: Receivables/ArActions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ArAction arAction = db.ArActions.Find(id);
            db.ArActions.Remove(arAction);
            db.SaveChanges();
            //return RedirectToAction("Index");
            return RedirectToAction("TransactionHistory", "ArTransactions", new { id = arAction.ArTransactionId });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        private string GetUser()
        {
            if (HttpContext.User.Identity.Name != "")
            {
                return HttpContext.User.Identity.Name;
            }
            else
            {
                return "Unknown User";
            }
        }
    }
}
