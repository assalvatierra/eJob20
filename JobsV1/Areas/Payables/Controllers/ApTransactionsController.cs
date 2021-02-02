using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ApModels.Models;
using ApServices;

namespace Payable.Areas.Payables.Controllers
{
    public class ApTransactionsController : Controller
    {
        private PayablesFactory ap = new PayablesFactory();

        // GET: Payables/ApTransactions
        public ActionResult Index(int? status, string sort)
        {
            status = status == null ? 0 : status;

            var apTransactions = ap.TransactionMgr.GetTransactions((int)status, sort);

            ViewBag.Today = ap.DateClassMgr.GetCurrentDate();
            ViewBag.Status = status;
            ViewBag.Sort = sort;

            return View(apTransactions);
        }

        // GET: Payables/ApTransactions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApTransaction apTransaction = ap.TransactionMgr.GetTransactionById((int)id);
            if (apTransaction == null)
            {
                return HttpNotFound();
            }
            return View(apTransaction);
        }

        // GET: Payables/ApTransactions/Create
        public ActionResult Create()
        {
            ApTransaction transaction = new ApTransaction();
            transaction.Amount = 0;
            transaction.NextRef = 0;
            transaction.PrevRef = 0;

            ViewBag.ApAccountId = new SelectList(ap.AccountMgr.GetAccounts(), "Id", "Name");
            ViewBag.ApTransCategoryId = new SelectList(ap.TransactionMgr.GetTransCategories(), "Id", "Name");
            ViewBag.ApTransStatusId = new SelectList(ap.TransactionMgr.GetTransStatus(), "Id", "Status");
            return View(transaction);
        }

        // POST: Payables/ApTransactions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,InvoiceId,DtInvoice,DtEncoded,Description,Amount,IsRepeating,Interval,DtDue,DtService,DtServiceTo,Remarks,ApAccountId,ApTransStatusId,ApTransCategoryId,NextRef,PrevRef,RepeatCount")] ApTransaction apTransaction)
        {
            if (ModelState.IsValid)
            {
                ap.TransactionMgr.AddTransaction(apTransaction);

                if (apTransaction.ApAccountId == 1)
                {
                    return RedirectToAction("CreateTransAcc", "ApAccounts" ,new { id = apTransaction.Id });
                }

                return RedirectToAction("Details", new { id = apTransaction.Id });
            }

            ViewBag.ApAccountId = new SelectList(ap.AccountMgr.GetAccounts(), "Id", "Name", apTransaction.ApAccountId);
            ViewBag.ApTransCategoryId = new SelectList(ap.TransactionMgr.GetTransCategories(), "Id", "Name", apTransaction.ApTransCategoryId);
            ViewBag.ApTransStatusId = new SelectList(ap.TransactionMgr.GetTransStatus(), "Id", "Status", apTransaction.ApTransStatusId);
            return View(apTransaction);
        }

        // GET: Payables/ApTransactions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApTransaction apTransaction = ap.TransactionMgr.GetTransactionById((int)id);
            if (apTransaction == null)
            {
                return HttpNotFound();
            }
            ViewBag.ApAccountId = new SelectList(ap.AccountMgr.GetAccounts(), "Id", "Name", apTransaction.ApAccountId);
            ViewBag.ApTransCategoryId = new SelectList(ap.TransactionMgr.GetTransCategories(), "Id", "Name", apTransaction.ApTransCategoryId);
            ViewBag.ApTransStatusId = new SelectList(ap.TransactionMgr.GetTransStatus(), "Id", "Status", apTransaction.ApTransStatusId);
            return View(apTransaction);
        }

        // POST: Payables/ApTransactions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,InvoiceId,DtInvoice,DtEncoded,Description,Amount,IsRepeating,Interval,DtDue,DtService,DtServiceTo,Remarks,ApAccountId,ApTransStatusId,ApTransCategoryId,NextRef,PrevRef,RepeatCount")] ApTransaction apTransaction)
        {
            if (ModelState.IsValid)
            {
                ap.TransactionMgr.EditTransaction(apTransaction);
                return RedirectToAction("Details", new { id = apTransaction.Id });
            }
            ViewBag.ApAccountId = new SelectList(ap.AccountMgr.GetAccounts(), "Id", "Name", apTransaction.ApAccountId);
            ViewBag.ApTransCategoryId = new SelectList(ap.TransactionMgr.GetTransCategories(), "Id", "Name", apTransaction.ApTransCategoryId);
            ViewBag.ApTransStatusId = new SelectList(ap.TransactionMgr.GetTransStatus(), "Id", "Status", apTransaction.ApTransStatusId);
            return View(apTransaction);
        }

        // GET: Payables/ApTransactions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApTransaction apTransaction = ap.TransactionMgr.GetTransactionById((int)id);
            if (apTransaction == null)
            {
                return HttpNotFound();
            }
            return View(apTransaction);
        }

        // POST: Payables/ApTransactions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ApTransaction apTransaction = ap.TransactionMgr.GetTransactionById((int)id);
            ap.TransactionMgr.DeleteTransaction(apTransaction);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                ap.TransactionMgr.DisposeDB();
            }
            base.Dispose(disposing);
        }

        [HttpPost]
        public bool AcceptPayment(int paymentid)
        {
            try
            {
                var payment = ap.PaymentMgr.GetPaymentById(paymentid);

                if (payment == null)
                {
                    return false;
                }

                //accepted
                payment.ApPaymentStatusId = 2;

                //update
                return ap.PaymentMgr.EditPayment(payment);
            }
            catch
            {
                return false;
            }
        }

        public bool UpdateTransStatus(int? transId, int? statusId)
        {

            if (transId == null || statusId == null)
            {
                return false;
            }

            //get transaction payable
            var payable = ap.TransactionMgr.GetTransactionById((int)transId);

            payable.ApTransStatusId = (int)statusId;

            //update payable
            var updateResponse= ap.TransactionMgr.EditTransaction(payable);

            if (updateResponse)
            {
                return true;
            }

            return false;

        }
    }
}
