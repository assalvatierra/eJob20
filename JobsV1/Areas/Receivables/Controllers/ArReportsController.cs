using ArServices;
using ArModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static ArModels.Models.ArRptModel;

namespace JobsV1.Areas.Receivables.Controllers
{
    public class ArReportsController : Controller
    {
        private ReceivableFactory ar = new ReceivableFactory();

        // GET: Receivables/ArReports
        public ActionResult Index(string sortBy, string orderBy)
        {
            //get list of active transactions
            var activeTrans = ar.TransactionMgr.GetActiveTransactions();

            List<ArRptTransPending> arRptTrans = new List<ArRptTransPending>();
            foreach (var trans in activeTrans)
            {
                decimal totalAmount = trans.Amount;
                decimal totalPayment = trans.ArTransPayments.Sum(c => c.ArPayment.Amount);
                decimal totalBalance = totalAmount - totalPayment;

                string accountName = "";
                if (!String.IsNullOrWhiteSpace(trans.ArAccount.Company)) {
                    accountName = trans.ArAccount.Company + " - " + trans.ArAccount.Name;
                }
                else
                {
                    accountName = trans.ArAccount.Name;
                }

                ArRptTransPending transPending;

                if (arRptTrans.Where(c=>c.AccountId == trans.ArAccountId).Count() > 0)
                {

                    var existingTransPending = arRptTrans.Where(c => c.AccountId == trans.ArAccountId).FirstOrDefault();

                    //if account exist, add to total Amount
                    existingTransPending.Amount += totalAmount;
                    existingTransPending.Payment += totalPayment;
                    existingTransPending.Balance += totalBalance;
                }
                else
                {
                    transPending = new ArRptTransPending()
                    {
                        Id = trans.Id,
                        AccountId = trans.ArAccountId,
                        Account = accountName,
                        Amount = totalAmount,
                        Payment = totalPayment,
                        Balance = totalBalance
                    };

                    arRptTrans.Add(transPending);
                }
            }

            //filter and order
            if (!String.IsNullOrWhiteSpace(sortBy))
            {

                if (!String.IsNullOrWhiteSpace(orderBy) && orderBy == "DESC")
                {
                    switch (sortBy)
                    {
                        case "Account":
                            arRptTrans = arRptTrans.OrderByDescending(t => t.Account).ToList();
                            break;
                        case "Amount":
                            arRptTrans = arRptTrans.OrderByDescending(t => t.Amount).ToList();
                            break;
                        case "Payment":
                            arRptTrans = arRptTrans.OrderByDescending(t => t.Payment).ToList();
                            break;
                        case "Balance":
                            arRptTrans = arRptTrans.OrderByDescending(t => t.Balance).ToList();
                            break;
                        default:
                            arRptTrans = arRptTrans.OrderByDescending(t => t.Account).ToList();
                            break;
                    }
                }
                else
                {
                    switch (sortBy)
                    {
                        case "Account":
                            arRptTrans = arRptTrans.OrderBy(t => t.Account).ToList();
                            break;
                        case "Amount":
                            arRptTrans = arRptTrans.OrderBy(t => t.Amount).ToList();
                            break;
                        case "Payment":
                            arRptTrans = arRptTrans.OrderBy(t => t.Payment).ToList();
                            break;
                        case "Balance":
                            arRptTrans = arRptTrans.OrderBy(t => t.Balance).ToList();
                            break;
                        default:
                            arRptTrans = arRptTrans.OrderBy(t => t.Account).ToList();
                            break;
                    }
                }
            }

            ViewBag.OrderBy = orderBy != null ? orderBy : "ASC" ;
            ViewBag.SortBy = sortBy != null ? sortBy : "Account";

            return View(arRptTrans);
        }


        // GET: ArReports/Daily
        public ActionResult Daily(DateTime? dateSrch)
        {

            if (dateSrch == null)
            {
                dateSrch = ar.DateClassMgr.GetCurrentDate();
            }

            var arTransactions = ar.TransactionMgr.GetTransactionsByDate((DateTime)dateSrch);

            ViewBag.Today = ar.DateClassMgr.GetCurrentDate();
            ViewBag.DateSrch = dateSrch;
            ViewBag.IsAdmin = true;

            return View(arTransactions.ToList());
        }


        // GET: ArReports/Monthly
        public ActionResult Monthly(DateTime? dateStart, DateTime? dateEnd)
        {

            if (dateStart == null)
            {
                dateStart = ar.DateClassMgr.GetCurrentDate();
            }


            if (dateEnd == null)
            {
                dateEnd = ar.DateClassMgr.GetCurrentDate();
            }

            var arTransactions = ar.TransactionMgr.GetTransactionsByDateRange((DateTime)dateStart, (DateTime)dateEnd);

            ViewBag.Today = ar.DateClassMgr.GetCurrentDate();
            ViewBag.DateStart = dateStart;
            ViewBag.DateEnd = dateEnd;
            ViewBag.IsAdmin = true;

            return View(arTransactions.ToList());
        }

    }
}