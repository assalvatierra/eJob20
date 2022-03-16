using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ArModels.Models;
using ArServices;

namespace JobsV1.Areas.Receivables.Controllers
{
    public class ArDepositsController : Controller
    {
        private ArDBContainer db = new ArDBContainer();
        private ReceivableFactory ar = new ReceivableFactory();

        // GET: Receivables/ArDeposits
        public ActionResult Index()
        {
            return View(db.ArDeposits.ToList());
        }

        // GET: Receivables/ArDeposits/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ArDeposit arDeposit = db.ArDeposits.Find(id);
            if (arDeposit == null)
            {
                return HttpNotFound();
            }
            return View(arDeposit);
        }

        // GET: Receivables/ArDeposits/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Receivables/ArDeposits/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,DtDeposit,Amount,Remarks,Referance,ArDepositBankId")] ArDeposit arDeposit)
        {
            if (ModelState.IsValid)
            {
                db.ArDeposits.Add(arDeposit);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ArDepositBankId = new SelectList(db.ArDepositBanks.ToList(), "Id", "AccountName");
            return View(arDeposit);
        }


        // GET: Receivables/ArDeposits/CreateTransDeposit
        public ActionResult CreateTransDeposit(int transId)
        {
            ArDeposit arDeposit = new ArDeposit();
            arDeposit.DtDeposit = ar.DateClassMgr.GetCurrentDate();
            ViewBag.TransId = transId;
            ViewBag.ArDepositBankId = new SelectList(db.ArDepositBanks.ToList(), "Id", "AccountName");
            return View(arDeposit);
        }

        // POST: Receivables/ArDeposits/CreateTransDeposit
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateTransDeposit([Bind(Include = "Id,DtDeposit,Amount,Remarks,Reference,ArDepositBankId")] ArDeposit arDeposit, int transId)
        {
            if (ModelState.IsValid)
            {
                db.ArDeposits.Add(arDeposit);
                db.SaveChanges();

                CreateTransDepositLink(transId, arDeposit.Id);

                return RedirectToAction("Index", "ArTransactions",null);
            }

            ViewBag.ArDepositBankId = new SelectList(db.ArDepositBanks.ToList(), "Id", "AccountName", arDeposit.ArDepositBankId);
            return View(arDeposit);
        }


        // GET: Receivables/ArDeposits/Edit/5
        public ActionResult Edit(int? id, int transId)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ArDeposit arDeposit = db.ArDeposits.Find(id);
            if (arDeposit == null)
            {
                return HttpNotFound();
            }
            ViewBag.TransId = transId;
            ViewBag.ArDepositBankId = new SelectList(db.ArDepositBanks.ToList(), "Id", "AccountName", arDeposit.ArDepositBankId);
            return View(arDeposit);
        }

        // POST: Receivables/ArDeposits/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,DtDeposit,Amount,Remarks,Reference,ArDepositBankId")] ArDeposit arDeposit, int? transId)
        {
            if (ModelState.IsValid)
            {
                db.Entry(arDeposit).State = EntityState.Modified;
                db.SaveChanges();

                if (transId == null)
                {
                    transId = arDeposit.ArTransDeposits.FirstOrDefault().ArTransactionId;
                    return RedirectToAction("Details", "ArTransactions", new { id = transId });
                }

                return RedirectToAction("Details","ArTransactions", new { id = transId });
            }
            ViewBag.ArDepositBankId = new SelectList(db.ArDepositBanks.ToList(), "Id", "AccountName", arDeposit.ArDepositBankId);
            return View(arDeposit);
        }

        // GET: Receivables/ArDeposits/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ArDeposit arDeposit = db.ArDeposits.Find(id);
            if (arDeposit == null)
            {
                return HttpNotFound();
            }
            return View(arDeposit);
        }

        // POST: Receivables/ArDeposits/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ArDeposit arDeposit = db.ArDeposits.Find(id);
            db.ArDeposits.Remove(arDeposit);
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



        public void CreateTransDepositLink(int transId, int depositId)
        {
            try
            {
                ArTransDeposit arTransDeposit = new ArTransDeposit();
                arTransDeposit.ArTransactionId = transId;
                arTransDeposit.ArDepositId = depositId;

                db.ArTransDeposits.Add(arTransDeposit);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
