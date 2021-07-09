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
        private DateClassMgr dt = new DateClassMgr();

        // GET: Payables/ApTransactions
        public ActionResult Index(int? status, string sort)
        {
            status = status == null ? 0 : status;

            var apTransactions = ap.TransactionMgr.GetTransactions((int)status, sort);

            ViewBag.IsAdmin = User.IsInRole("Admin");
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
            var today = dt.GetCurrentDateTime();

            ApTransaction transaction = new ApTransaction();
            transaction.Amount = 0;
            transaction.NextRef = 0;
            transaction.PrevRef = 0;
            transaction.DtEncoded = today;
            transaction.DtInvoice = today;
            transaction.DtDue = today;
            transaction.DtService = today;
            transaction.DtServiceTo = today;


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
        public ActionResult Create([Bind(Include = "Id,InvoiceNo,DtInvoice,DtEncoded,Description,Amount,IsRepeating,Interval,DtDue,DtService,DtServiceTo," +
            "Remarks,ApAccountId,ApTransStatusId,ApTransCategoryId,NextRef,PrevRef,RepeatCount,BudgetAmt")] ApTransaction apTransaction)
        {
            if (ModelState.IsValid)
            {
                apTransaction.RepeatNo = 1;
                apTransaction.IsPrinted = false;
                ap.TransactionMgr.AddTransaction(apTransaction);

                //add action log for transaction create 
                ap.ActionMgr.AddAction(GetUser(), apTransaction.Id, 1);

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
        public ActionResult Edit([Bind(Include = "Id,InvoiceNo,DtInvoice,DtEncoded,Description,Amount,IsRepeating,Interval,DtDue,DtService,DtServiceTo," +
            "Remarks,ApAccountId,ApTransStatusId,ApTransCategoryId,NextRef,PrevRef,RepeatCount,RepeatNo,IsPrinted,BudgetAmt,ReleaseAmt,DtRelease")] ApTransaction apTransaction)
        {
            if (ModelState.IsValid)
            {
                ap.TransactionMgr.EditTransaction(apTransaction);

                //add action log for transaction edit 
                ap.ActionMgr.AddAction(GetUser(), apTransaction.Id, 11);

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

            //add action log for transaction delete 
            ap.ActionMgr.AddAction(GetUser(), apTransaction.Id, 12);

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
        public bool AcceptPayment(int paymentid, int transId)
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


                //add action log for transaction update status 
                ap.ActionMgr.AddAction(GetUser(), (int)transId, 7);

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


            //add action log for transaction update status 
            ap.ActionMgr.AddAction(GetUser(), (int)transId, (int)statusId);

            if (updateResponse)
            {
                return true;
            }

            return false;
        }


        [HttpGet]
        public JsonResult GetRepeatingPayablesCount()
        {
            var repeatingPayables = ap.TransactionMgr.GetRepeatingTransactions();

            return Json(repeatingPayables.Count(), JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult GetRepeatingPayables()
        {
            var repeatingPayables = ap.TransactionMgr.GetRepeatingTransactions();

            return Json(repeatingPayables.Select(p=> 
                new {
                    p.Id,
                    p.ApAccount.Name,
                    p.ApAccount.ContactPerson,
                    p.Amount,
                    p.Description,
                    p.DtDue,
                    p.Interval
                }),
                JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public bool RepeatSelectedPayables(int[] payableIds)
        {

            var copyResult = true;

            if (payableIds == null || payableIds.Length == 0)
            {
                return false;
            }

            foreach (var payableId in payableIds)
            {
                //repeat items based on interval
                copyResult = ap.TransactionMgr.CopyRepeatingTrans(payableId);

                if (!copyResult)
                {
                    return copyResult;
                }

                //add action log for transaction create 
                ap.ActionMgr.AddAction(GetUser(), payableId, 12);
            }

            return copyResult;
        }

        [HttpPost]
        public bool CancelRepeatingTrans(int? transId)
        {
            if (transId == null || transId == 0)
            {
                return false;
            }

            ApTransaction transaction = ap.TransactionMgr.GetTransactionById((int)transId);
            transaction.IsRepeating = false;

            //add action log for transaction create 
            ap.ActionMgr.AddAction(GetUser(), transaction.Id, 13);

            //save changes
            return ap.TransactionMgr.EditTransaction(transaction);

        }

        public string GetUser()
        {
           return HttpContext.User.Identity.Name ?? "Unknown";
        }

        [HttpGet]
        public JsonResult GetDuePayables()
        {
            var duePayables = ap.TransactionMgr.GetDueTransactions()
                .Select(
                    t=> new {
                        t.Id,
                        t.ApAccount.Name,
                        t.ApTransStatu.Status,
                        t.DtDue,
                        t.DtInvoice,
                        InvoiceNo = t.InvoiceNo == null ? "" : t.InvoiceNo,
                        t.Amount,
                        t.Description,
                        totalPayment = t.ApTransPayments.Sum(p=>p.ApPayment.Amount)
                    }
                );

            return Json(duePayables, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public string ReleasePayment(int? id, decimal? amount, string date)
        {
            DateTime dtRelease = new DateTime();

            if (DateTime.TryParse(date, out dtRelease) && id != null && amount != null)
            {
                ap.TransactionMgr.UpdateReleaseAmount((int)id, (decimal)amount, dtRelease);
                return "OK";
            }

            return "Unable to Release Payment";
        }

        #region Print Request Form
        public ActionResult PrintRequestForm(int id)
        {
            var payables = ap.TransactionMgr.GetPrintGroup(id);

            ViewBag.Today = dt.GetCurrentDateTime().ToShortDateString();
            ViewBag.PrintGroupId = id;
            return View(payables);
        }

        [HttpPost]
        public int SendPrintRequest(int[] transIds)
        {
            var printGroupId = ap.TransactionMgr.AddPrintRequest(transIds, GetUser());

            //update print status of each payables transaction to true
            for (int i=0; i< transIds.Length; i++)
            {
                UpdatePrintStatus(transIds[i]);
            }

            return printGroupId;
        }


        //POST : /Payables/ApTransactions/UpdatePrintStatus
        //Param : id = payable transaction Id
        [HttpPost]
        public bool UpdatePrintStatus(int id)
        {
            //update transaction printed status to true
            return ap.TransactionMgr.UpdatePrintStatus(id, true);
        }
        #endregion

        #region History Actions 
        public ActionResult ActionHistory(int? id)
        {
            if (id == null )
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var actionList = ap.TransactionMgr.GetTransactionById((int)id);
            ViewBag.TransId = (int)id;
            return View(actionList.ApActions.ToList());
        }
    }

    #endregion
}
