using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ApModels.Models;
using ApModels.Models.Custom;
using ApServices;

namespace JobsV1.Areas.Payables.Controllers
{
    public class ApTransactionsController : Controller
    {
        private PayablesFactory ap = new PayablesFactory();
        private DateClassMgr dt = new DateClassMgr();
        private ApDBContainer db = new ApDBContainer();

        private enum STATUS : int {
            NEW = 6,
            REQUEST = 1,
            APPROVED = 2,
            RELEASED = 3,
            RETURNED = 5,
            CLOSED = 4
        };

        private enum PAYMENTSTATUS : int
        {
            REQUEST = 1,
            APPROVED = 2,
            CANCELLED = 3,
        };

        private enum CASHFLOWTYPE : int
        {
            DEBIT = 1,
            CREDIT = 2,
        };

        // GET: Payables/ApTransactions
        public ActionResult Index(int? status, string sort)
        {
            status = status == null ? 0 : status;

            var apTransactions = ap.TransactionMgr.GetTransactions((int)status, sort);

            ViewBag.IsAdmin = User.IsInRole("Admin");
            ViewBag.Today = ap.DateClassMgr.GetCurrentDate();
            ViewBag.Status= status;
            ViewBag.Sort  = sort;
            ViewBag.ApTansToday = ap.TransactionMgr.GetDailyTransactions(ap.DateClassMgr.GetCurrentDate());
            return View(apTransactions);
        }

        // GET: Payables/ApTransactions/ReleasedDaily
        public ActionResult ReleasedDaily(int? status, string sort, DateTime? dateSrch)
        {
            status = status == null ? 0 : status;

            if (dateSrch == null)
            {
                dateSrch = dt.GetCurrentDate();
            }

            var apTransactions = ap.TransactionMgr.GetDailyReleasedTransactions(sort, (DateTime)dateSrch);

            ViewBag.IsAdmin = User.IsInRole("Admin");
            ViewBag.Today = ap.DateClassMgr.GetCurrentDate();
            ViewBag.Status = status;
            ViewBag.Sort = sort;
            ViewBag.DateSrch = dateSrch;

            return View(apTransactions);
        }

        // GET: Payables/ApTransactions/ReleasedWeekly
        public ActionResult ReleasedWeekly(DateTime? dateStart, DateTime? dateEnd, int? transType, int? category, string search)
        {
            List<ApTransaction> apTransactions;
            DateTime thisMonth = dt.GetCurrentDate();

            var startMonthDate = new DateTime(thisMonth.Year, thisMonth.Month, 1);
            var endMonthDate = startMonthDate.AddMonths(1).AddDays(-1);

            dateStart = dateStart == null ? startMonthDate : dateStart;
            dateEnd = dateEnd == null ? endMonthDate : dateEnd;
            transType = transType ?? 1;
            category = category ?? 0; 

            if (transType == 2)
            {
                apTransactions = ap.TransactionMgr
                    .GetDailyReleasedByDateReturned((DateTime)dateStart, (DateTime)dateEnd, (int)category, search);
            }
            else
            {
                apTransactions = ap.TransactionMgr
                    .GetDailyReleasedByDateRange((DateTime)dateStart, (DateTime)dateEnd, (int)category, search);
            }

            ViewBag.IsAdmin = User.IsInRole("Admin");
            ViewBag.Today = ap.DateClassMgr.GetCurrentDate();
            ViewBag.DateStart = dateStart;
            ViewBag.DateEnd = dateEnd;
            ViewBag.TransType = transType ?? 0;
            ViewBag.Category = category ?? 0;

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

            ViewBag.Repeating = apTransaction.ApTransRepeat;
            return View(apTransaction);
        }

        // GET: Payables/ApTransactions/Create
        public ActionResult Create()
        {
            try
            {
                var today = dt.GetCurrentDateTime();

                ApTransaction transaction = new ApTransaction();
                transaction.Amount = 0;
                transaction.DtEncoded = today;
                transaction.DtInvoice = today;
                transaction.DtDue = today;
                transaction.DtService = today;
                transaction.DtServiceTo = today;

                ViewBag.ApAccountId = new SelectList(ap.AccountMgr.GetAccounts(), "Id", "Name");
                ViewBag.ApTransCategoryId = new SelectList(ap.TransactionMgr.GetTransCategories(), "Id", "Name");
                //ViewBag.ApTransStatusId = new SelectList(ap.TransactionMgr.GetTransStatus().OrderBy(c => c.Code), "Id", "Status");

                ViewBag.ApTransStatusId = new SelectList(db.ApTransStatus.Where(c=>c.Id == 1).ToList(), "Id", "Status");
                ViewBag.ApTransTypeId = new SelectList(ap.TransactionMgr.GetTransTypes(), "Id", "Type");
                return View(transaction);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // POST: Payables/ApTransactions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,InvoiceNo,DtInvoice,DtEncoded,Description,Amount,BudgetAmt,DtDue,DtService,DtServiceTo," +
            "JobRef,Remarks,IsRepeating,ApAccountId,ApTransStatusId,ApTransCategoryId,ApTransTypeId,IsFunded")] ApTransaction apTransaction)
        {
            if (ModelState.IsValid)
            {
                apTransaction.IsPrinted = false;
                ap.TransactionMgr.AddTransaction(apTransaction);

                //add action log for transaction create 
                ap.ActionMgr.AddAction(GetUser(), apTransaction.Id, 1);

                if (apTransaction.ApAccountId == 1)
                {
                    return RedirectToAction("CreateTransAcc", "ApAccounts", new { id = apTransaction.Id });
                }

                return RedirectToAction("Details", new { id = apTransaction.Id });
            }

            ViewBag.ApAccountId = new SelectList(ap.AccountMgr.GetAccounts(), "Id", "Name", apTransaction.ApAccountId);
            ViewBag.ApTransCategoryId = new SelectList(ap.TransactionMgr.GetTransCategories(), "Id", "Name", apTransaction.ApTransCategoryId);
            ViewBag.ApTransStatusId = new SelectList(ap.TransactionMgr.GetTransStatus(), "Id", "Status", apTransaction.ApTransStatusId);
            ViewBag.ApTransTypeId = new SelectList(ap.TransactionMgr.GetTransTypes(), "Id", "Type", apTransaction.ApTransTypeId);
            return View(apTransaction);
        }

        // GET: Payables/ApTransactions/Edit/5
        public ActionResult Edit(int? id)
        {
            try
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
                ViewBag.ApTransStatusId = new SelectList(ap.TransactionMgr.GetTransStatus().OrderBy(c => c.Code), "Id", "Status", apTransaction.ApTransStatusId);
                ViewBag.ApTransTypeId = new SelectList(ap.TransactionMgr.GetTransTypes(), "Id", "Type", apTransaction.ApTransTypeId);
                return View(apTransaction);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // POST: Payables/ApTransactions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,InvoiceNo,DtInvoice,DtEncoded,Description,Amount,BudgetAmt,DtDue,DtService,DtServiceTo," +
            "ReleaseAmt,DtRelease,JobRef,Remarks,IsRepeating,ApAccountId,ApTransStatusId,ApTransCategoryId,ApTransTypeId,IsFunded")] ApTransaction apTransaction)
        {

            try
            {
                var isAdmin = User.IsInRole("Admin");

                if (ModelState.IsValid)
                {
                    if (apTransaction.ApTransStatusId > 2 && apTransaction.ApTransStatusId < 5)
                    {
                        if (isAdmin)
                        {
                            ap.TransactionMgr.EditTransaction(apTransaction);
                        }

                        //add action log for transaction edit 
                        ap.ActionMgr.AddAction(GetUser(), apTransaction.Id, 11);

                        return RedirectToAction("Details", new { id = apTransaction.Id });
                    }

                    ap.TransactionMgr.EditTransaction(apTransaction);

                    //add action log for transaction edit 
                    ap.ActionMgr.AddAction(GetUser(), apTransaction.Id, 11);

                    return RedirectToAction("Details", new { id = apTransaction.Id });
                }

                ViewBag.ApAccountId = new SelectList(ap.AccountMgr.GetAccounts(), "Id", "Name", apTransaction.ApAccountId);
                ViewBag.ApTransCategoryId = new SelectList(ap.TransactionMgr.GetTransCategories(), "Id", "Name", apTransaction.ApTransCategoryId);
                ViewBag.ApTransStatusId = new SelectList(ap.TransactionMgr.GetTransStatus(), "Id", "Status", apTransaction.ApTransStatusId);
                ViewBag.ApTransTypeId = new SelectList(ap.TransactionMgr.GetTransTypes(), "Id", "Type", apTransaction.ApTransTypeId);
                return View(apTransaction);
            }
            catch(Exception ex)
            {
                throw ex;
            }
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

            ap.TransactionMgr.DeleteTransActions((int)id);

            ap.TransactionMgr.DeleteTransaction(apTransaction);

            //add action log for transaction delete 
            // ap.ActionMgr.AddAction(GetUser(), apTransaction.Id, 12);

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

        #region API
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
            var updateResponse = ap.TransactionMgr.EditTransaction(payable);

            //add action log for transaction update status 
            ap.ActionMgr.AddAction(GetUser(), (int)transId, (int)statusId);

            if (updateResponse)
            {
                //Add cashflow
                UpdateCashFlowByTrans(payable);

                return true;
            }

            return false;
        }

        public void UpdateCashFlowByTrans(ApTransaction payable)
        {
            //RELEASED
            if (payable.ApTransStatusId == ((int)STATUS.RELEASED))
            {
                ReleasedCashFlow(payable);
            }

            //RETURN
            if (payable.ApTransStatusId == ((int)STATUS.RETURNED))
            {
                ReturnedCashFlow(payable);
            }
        }

        public void ReleasedCashFlow(ApTransaction payable)
        {
            var today = dt.GetCurrentDate().Date;

            //RELEASED
            if (payable.ReleaseAmt != 0)
            {
                ap.CashFlowMgr.AddCashFlow(new ApCashFlow
                {
                    Description = payable.Description,
                    Date = payable.DtRelease ?? today,
                    Amount = payable.ReleaseAmt ?? 0,
                    Remarks = payable.Remarks,
                    ApCashFlowTypeId = 1,
                    ApAccountId = payable.ApAccountId,
                    PerformedBy = GetUser()
                });
            }
        }

        public void ReturnedCashFlow(ApTransaction payable)
        {
            var today = dt.GetCurrentDate().Date;

            //RETURN
            
                var budget = payable.ReleaseAmt ?? 0;
                var payments = payable.ApTransPayments == null ? 0 : payable.ApTransPayments.ToList().Sum(c => c.ApPayment.Amount);
                var payableChange = budget - payments;
                if (payableChange > 0)
                {
                    ap.CashFlowMgr.AddCashFlow(new ApCashFlow
                    {
                        Description = "Change " + payable.Description,
                        Date = today,
                        Amount = payableChange,
                        Remarks = payable.Remarks,
                        ApCashFlowTypeId = 2,
                        ApAccountId = payable.ApAccountId,
                        PerformedBy = GetUser()
                    });
                }
            
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

            return Json(repeatingPayables.Select(p =>
                new
                {
                    p.Id,
                    p.ApAccount.Name,
                    p.ApAccount.ContactPerson,
                    p.Amount,
                    p.Description,
                    p.DtDue,
                    p.IsRepeating,
                    p.ApTransRepeat.Interval,
                   
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


                //add action log for transaction create 
                ap.ActionMgr.AddAction(GetUser(), payableId, 12);

                if (!copyResult)
                {
                    return copyResult;
                }
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
                    t => new
                    {
                        t.Id,
                        t.ApAccount.Name,
                        t.ApTransStatu.Status,
                        t.DtDue,
                        t.DtInvoice,
                        InvoiceNo = t.InvoiceNo == null ? "" : t.InvoiceNo,
                        t.Amount,
                        t.Description,
                        totalPayment = t.ApTransPayments.Sum(p => p.ApPayment.Amount)
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

        [HttpPost]
        public string ReturnAmount(int? id, decimal? amount, string remarks, DateTime invoiceDate)
        {
            if (id != null && amount != null)
            {
                ap.TransactionMgr.UpdateReturnAmount((int)id, (decimal)amount, remarks, invoiceDate);
                return "OK";
            }

            return "Unable to Release Payment";
        }

        [HttpPost]
        public string AddPayment(int? id, decimal? amount, string date, string remarks)
        {
            DateTime dtRelease = new DateTime();

            if (DateTime.TryParse(date, out dtRelease) && id != null && amount != null)
            {
                var trans = ap.TransactionMgr.GetTransactionById((int)id);

                ApPayments payment = new ApPayments
                {
                    ApAccountId = trans.ApAccountId,
                    ApPaymentStatusId = 2,
                    ApPaymentTypeId = 1,
                    DtPayment = dtRelease,
                    Remarks = remarks,
                    Amount = (decimal)amount,
                };

                ap.PaymentMgr.AddPayment(payment);

                ap.TransactionMgr.AddTransPayment((int)id, payment.Id);

                return "OK";
            }

            return "Unable to Release Payment";
        }

        public bool SetFunded(int id)
        {
            try
            {
                var payable = ap.TransactionMgr.GetTransactionById(id);

                payable.IsFunded = true;

                return ap.TransactionMgr.EditTransaction(payable);
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region Print Request Form
        public ActionResult PrintRequestForm(int id)
        {
            var payables = ap.TransactionMgr.GetPrintGroup(id);

            ViewBag.Today = dt.GetCurrentDateTime().ToShortDateString();
            ViewBag.PrintGroupId = id;

            return View(payables);
        }

        public ActionResult PrintRequestPOForm(int id)
        {
            var payables = ap.TransactionMgr.GetPrintGroup(id);
            var today = dt.GetCurrentDateTime().ToShortDateString();

            ViewBag.PONo = id;
            ViewBag.Today = today;
            ViewBag.dtRequest = dt.GetCurrentDateTime().ToString("MMM dd yyyy");
            ViewBag.PrintGroupId = payables.First().Id;
            ViewBag.Name = "ONE STOP SHELL STATION";
            ViewBag.Address = "Km 4 Quimpo Blvd., Ecoland, Davao City";
            ViewBag.Landline = "299-2010";

            return View(payables);
        }

        [HttpPost]
        public int SendPrintRequest(int[] transIds)
        {
            var printGroupId = ap.TransactionMgr.AddPrintRequest(transIds, GetUser());

            //update print status of each payables transaction to true
            for (int i = 0; i < transIds.Length; i++)
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


        public ActionResult PrintRequestList()
        {
            var requestList = ap.TransactionMgr.GetPrintRequests();

            return View(requestList);
        }


        public ActionResult PrintRequestDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var requestDetails = ap.TransactionMgr.GetPrintRequestDetails((int)id);

            return View(requestDetails);
        }


        public ActionResult PrintPOForm(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var request = db.ApTransactions.Find(id);

            if (request == null)
            {
                return HttpNotFound();
            }

            UpdatePrintStatus((int)id);
            ViewBag.Company = request.ApAccount;
            ViewBag.DtRequest = request.DtInvoice;
            ViewBag.PONo = request.Id;
            return View(request);
        }

        #endregion

        #region History Actions 
        public ActionResult ActionHistory(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var actionList = ap.TransactionMgr.GetTransactionById((int)id);
            ViewBag.TransId = (int)id;
            return View(actionList.ApActions.ToList());
        }


        // POST: Payables/ApTransactions/Delete/5
        [HttpPost]
        public string ActionHistoryDeleteConfirmed(int id)
        {
            try
            {

                var action = db.ApActions.Find(id);
                var transId = action.ApTransactionId;
                //add action log for transaction delete 
                db.ApActions.Remove(action);
                db.SaveChanges();

                return "Deleted";
            }
            catch
            {
                return "Unable to delete";
            }
        }
        #endregion

    }
}
