using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ArModels.Models;
using ArServices;

namespace JobsV1.Areas.Receivables.Controllers
{
    public class ArMgtController : Controller
    {
        private ReceivableFactory ar = new ReceivableFactory();

        #region Due Date Management
        // GET: Receivables/ArMgt
        public ActionResult Index()
        {
            var today = ar.DateClassMgr.GetCurrentDate();
            var tomorrow = today.AddDays(1);
            var yesterday = today.AddDays(-1);

            //get ongoing transactions
            var transactions = ar.TransactionMgr.GetApprovedTransactions();
            var overDue = transactions.Where(t => t.DtDue.AddDays(1) < today);

            transactions = transactions.Where(c => !overDue.Contains(c)).ToList();

            ViewBag.today = today;
            ViewBag.OverDueTrans = overDue.OrderBy(t => t.DtDue).ToList();

            return View(transactions.OrderBy(d => d.DtDue));
        }

        // GET: Receivables/ArMgt/UpdateDueDate
        // Param:   transId = transactionId
        //          dueDate = new dueDate
        [HttpPost]
        public bool UpdateDueDate(int? transId, string dueDate)
        {

            if (transId == null || String.IsNullOrEmpty(dueDate.Trim()))
            {
                return false;
            }

            var transaction = ar.TransactionMgr.GetTransactionById((int)transId);

            if (transaction == null)
            {
                return false;
            }

            //try parse if due date is datetime obj
            var newDueDate = new DateTime();
            if (!DateTime.TryParseExact(dueDate, "MM/dd/yyyy",
                       CultureInfo.InvariantCulture,
                       DateTimeStyles.None, out newDueDate))
            {
                return false;
            }

            //edit 
            transaction.DtDue = newDueDate;
            var editResult = ar.TransactionMgr.EditTransaction(transaction);

            if (editResult == true)
            {
                //first reminder
                ar.ActionMgr.AddAction(8, GetUser(), (int)transId, "Change Due Date");
            }

            return editResult;
        }

        // GET: Receivables/ArMgt/GetCustomerMobile
        // Param : transId = transactionId
        [HttpGet]
        public JsonResult GetTransactionDetails(int transId)
        {
            var transaction = ar.TransactionMgr.GetTransactionById(transId);

            if (transaction == null)
            {
                return Json("Error", JsonRequestBehavior.AllowGet);
            }

            return Json(new
            {
                transaction.Id,
                transaction.InvoiceId,
                transaction.DtInvoice,
                transaction.DtDue,
                transaction.Description,
                transaction.Amount,
                transaction.ArTransStatu.Status,

            }
                , JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public bool SendEmailReminder(int? transId, string recipient, string emailMessage)
        {
            try
            {
                if (!String.IsNullOrWhiteSpace(recipient.Trim()) && transId != null)
                {

                    emailMessage = emailMessage.Replace("\n", "<br>");

                    var EmailResult = ar.EmailMgr.SendEmail(recipient, emailMessage);

                    //add activity history
                    var today = ar.DateClassMgr.GetCurrentDateTime();

                    //first reminder
                    ar.ActionMgr.AddAction(9, GetUser(), (int)transId);

                    return EmailResult;
                }

                return false;
            }
            catch
            {
                return false;
            }
        }

        #endregion


        #region For Approval Module
        public ActionResult Approval()
        {
            //get ongoing transactions
            var transactions = ar.TransactionMgr.GetForApprovalTrans();

            return View(transactions);
        }


        [HttpPost]
        public bool UpdateTransStatus(int? transId, int? statusId)
        {
            if (transId == null || statusId == null)
            {
                return false;
            }

            var trans = ar.TransactionMgr.GetTransactionById((int)transId);

            if (trans == null)
            {
                return false;
            }

            trans.ArTransStatusId = (int)statusId;
            //update
            var editResponse = ar.TransactionMgr.EditTransaction(trans);

            if (editResponse)
            {
                var user = GetUser(); //edit to get user here!

                ar.ActionMgr.AddAction((int)statusId, user, (int)transId);

                return true;
            }

            return false;

        }

        #endregion


        #region Settlement Module
        public ActionResult Settlement()
        {
            //get ongoing transactions
            var transactions = ar.TransactionMgr.GetForSettlementTrans();

            ViewBag.DateToday = ar.DateClassMgr.GetCurrentDateTime();
            return View(transactions);
        }


        public string SaveSettlementPrintIds(int[] transIds)
        {
            if (transIds.Length > 0)
            {
                Session["ArSettlementPrintIds"] = transIds;
            }

            return "OK";
        }


        public ActionResult SettlementPrint()
        {

            int[] transIds = Session["ArSettlementPrintIds"] as int[];

            //convert to list
            List<int> IdList = new List<int>();
            if (transIds != null)
            {
                IdList.AddRange(transIds);
            }

            //get ongoing transactions
            var transactions = ar.TransactionMgr.GetForSettlementTrans()
                .Where(a => IdList.Contains(a.Id));

            ViewBag.DateToday = ar.DateClassMgr.GetCurrentDateTime();
            return View(transactions.ToList());
        }

        [HttpPost]
        public bool UpdatePaymentAsDeposited(int transId)
        {
            ar.PaymentMgr.UpdateTransDeposit(transId, true);
            UpdateTransStatus(transId, 6);

            return true;
        }

        [HttpPost]
        public bool UpdateTranPaymentsAsDeposited(int[] transIds)
        {
            var artransIdList = new List<int>(transIds);
            foreach (var arTransId in artransIdList)
            {
                ar.PaymentMgr.UpdateTransDeposit(arTransId, true);
                UpdateTransStatus(arTransId, 6);
            }

            return true;
        }

        #endregion

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