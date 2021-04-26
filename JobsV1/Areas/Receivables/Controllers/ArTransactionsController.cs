using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using ArModels.Models;
using ArServices;
using Microsoft.Ajax.Utilities;

namespace JobsV1.Areas.Receivables.Controllers
{
    public class ArTransactionsController : Controller
    {
        private ReceivableFactory ar = new ReceivableFactory();
        private ArDBContainer ardb = new ArDBContainer();

        // GET: ArTransactions
        public ActionResult Index(string status, string sortBy, string orderBy)
        {
            var arTransactions = ar.TransactionMgr.GetTransactions(status, sortBy, orderBy);

            //TODO: check repeating receivables 
            //not priority - 1/12/2021
            //ar.TransactionMgr.CheckRepeatingTrans();             

            ViewBag.Status = status;
            ViewBag.SortBy = sortBy;
            ViewBag.OrderBy = orderBy;
            ViewBag.Today = ar.DateClassMgr.GetCurrentDate();
            return View(arTransactions.ToList());
        }

        // GET: ArTransactions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ArTransaction arTransaction = ar.TransactionMgr.GetTransactionById((int)id);
            if (arTransaction == null)
            {
                return HttpNotFound();
            }

            ViewBag.Payments = ar.TransPaymentMgr.GetTransPaymentsByTransId(arTransaction.Id);
            ViewBag.IsClosed = ar.TransactionMgr.IsClosed((int)id);
            return View(arTransaction);
        }

        // GET: ArTransactions/Create
        public ActionResult Create()
        {
            ArTransaction transaction = new ArTransaction();
            transaction.Amount = 0;
            transaction.Interval = 0;
            transaction.PrevRef = 0;
            transaction.NextRef = 0;
            transaction.InvoiceId = 0;

            var accounts = ar.AccountMgr.GetArAccounts()
                  .Select(s => new SelectListItem
                  {
                      Value = s.Id.ToString(),
                      Text = s.Company + " - " + s.Name.ToString()
                  });

            ViewBag.ArAccountId = new SelectList(accounts, "Value", "Text");
            ViewBag.ArTransStatusId = new SelectList(ar.TransactionMgr.GetTransactionStatus(), "Id", "Status");
            //ViewBag.ArAccountId = new SelectList(ar.AccountMgr.GetArAccounts(), "Id", "Company");
            ViewBag.ArCategoryId = new SelectList(ar.CategoryMgr.GetCategories(), "Id", "Name");
            return View(transaction);
        }

        // POST: ArTransactions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,InvoiceId,DtInvoice,Description,DtEncoded,DtDue,Amount,Interval,IsRepeating,Remarks,ArTransStatusId,ArAccountId,ArCategoryId,DtService,DtServiceTo,InvoiceRef,PrevRef,NextRef,RepeatCount")] ArTransaction arTransaction)
        {
            try
            {

                if (ModelState.IsValid && InputValidation(arTransaction))
                {
                    var today = ar.DateClassMgr.GetCurrentDateTime();
                    var currentUser = HttpContext.User.Identity.Name;

                    ar.TransactionMgr.AddTransaction(arTransaction);

                    //new transaction action history (new bill)
                    ar.ActionMgr.AddAction(1, currentUser, arTransaction.Id);

                    //new account
                    if (arTransaction.ArAccountId == 1)
                    {
                        return RedirectToAction("CreateAccTrans", new { transId = arTransaction.Id });
                    }

                    return RedirectToAction("Index");
                }

                var accounts = ar.AccountMgr.GetArAccounts()
                      .Select(s => new SelectListItem
                      {
                          Value = s.Id.ToString(),
                          Text = s.Company + " - " + s.Name.ToString()
                      });

                ViewBag.ArAccountId = new SelectList(accounts, "Value", "Text", arTransaction.ArAccountId);

                ViewBag.ArTransStatusId = new SelectList(ar.TransactionMgr.GetTransactionStatus(), "Id", "Status");
                //ViewBag.ArAccountId = new SelectList(ar.AccountMgr.GetArAccounts(), "Id", "Company");
                ViewBag.ArCategoryId = new SelectList(ar.CategoryMgr.GetCategories(), "Id", "Name");

                return View(arTransaction);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // GET: ArTransactions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ArTransaction arTransaction = ar.TransactionMgr.GetTransactionById((int)id);
            if (arTransaction == null)
            {
                return HttpNotFound();
            }

            var accounts = ar.AccountMgr.GetArAccounts()
                  .Select(s => new SelectListItem
                  {
                      Value = s.Id.ToString(),
                      Text = s.Company + " - " + s.Name.ToString()
                  });

            ViewBag.ArAccountId = new SelectList(accounts, "Value", "Text", arTransaction.ArAccountId);
            ViewBag.ArTransStatusId = new SelectList(ar.TransactionMgr.GetTransactionStatus(), "Id", "Status", arTransaction.ArTransStatusId);
            //ViewBag.ArAccountId = new SelectList(ar.AccountMgr.GetArAccounts(), "Id", "Company", arTransaction.ArAccountId);
            ViewBag.ArCategoryId = new SelectList(ar.CategoryMgr.GetCategories(), "Id", "Name", arTransaction.ArCategoryId);

            return View(arTransaction);
        }

        // POST: ArTransactions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,InvoiceId,DtInvoice,Description,DtEncoded,DtDue,Amount,Interval,IsRepeating,Remarks,ArTransStatusId,ArAccountId,ArCategoryId,DtService,DtServiceTo,InvoiceRef,PrevRef,NextRef,RepeatCount")] ArTransaction arTransaction)
        {
            if (ModelState.IsValid && InputValidation(arTransaction))
            {
                ar.TransactionMgr.EditTransaction(arTransaction);
                return RedirectToAction("Details", new { id = arTransaction.Id });
            }

            var accounts = ar.AccountMgr.GetArAccounts()
                  .Select(s => new SelectListItem
                  {
                      Value = s.Id.ToString(),
                      Text = s.Company + " - " + s.Name.ToString()
                  });

            ViewBag.ArAccountId = new SelectList(accounts, "Value", "Text", arTransaction.ArAccountId);

            ViewBag.ArTransStatusId = new SelectList(ar.TransactionMgr.GetTransactionStatus(), "Id", "Status", arTransaction.ArTransStatusId);
            //ViewBag.ArAccountId = new SelectList(ar.AccountMgr.GetArAccounts(), "Id", "Company", arTransaction.ArAccountId);
            ViewBag.ArCategoryId = new SelectList(ar.CategoryMgr.GetCategories(), "Id", "Name", arTransaction.ArCategoryId);
            return View(arTransaction);
        }

        // GET: ArTransactions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ArTransaction arTransaction = ar.TransactionMgr.GetTransactionById((int)id);
            if (arTransaction == null)
            {
                return HttpNotFound();
            }
            return View(arTransaction);
        }

        // POST: ArTransactions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ArTransaction arTransaction = ar.TransactionMgr.GetTransactionById((int)id);
            ar.TransactionMgr.RemoveTransaction(arTransaction);
            return RedirectToAction("Index");
        }

        public bool InputValidation(ArTransaction transaction)
        {
            bool isValid = true;

            if (transaction.InvoiceId < 0)
            {
                ModelState.AddModelError("InvoiceId", "Invalid InvoiceId");
                isValid = false;
            }

            if (transaction.Description.IsNullOrWhiteSpace())
            {
                ModelState.AddModelError("Description", "Invalid Description");
                isValid = false;
            }

            if (transaction.Amount < -1)
            {
                ModelState.AddModelError("Amount", "Invalid Amount");
                isValid = false;
            }
            if (transaction.Interval < 0)
            {
                ModelState.AddModelError("Interval", "Invalid Interval");
                isValid = false;
            }


            return isValid;
        }

        public bool InputAccValidation(ArAccount account)
        {
            bool isValid = true;

            if (account.Name.IsNullOrWhiteSpace())
            {
                ModelState.AddModelError("Name", "Invalid Name");
                isValid = false;
            }

            if (account.Email.IsNullOrWhiteSpace())
            {
                ModelState.AddModelError("Email", "Invalid Email");
                isValid = false;
            }

            if (account.Company.IsNullOrWhiteSpace())
            {
                ModelState.AddModelError("Company", "Invalid Company Name");
                isValid = false;
            }

            if (account.Mobile.IsNullOrWhiteSpace() || account.Mobile.Length < 11)
            {
                ModelState.AddModelError("Mobile", "Invalid Mobile");
                isValid = false;
            }

            return isValid;
        }

        // GET: ArAccounts/Create
        public ActionResult CreateAccTrans(int transId)
        {
            ArAccount account = new ArAccount();
            account.ArAccStatusId = 1;

            ViewBag.TransId = transId;
            ViewBag.ArAccStatusId = new SelectList(ar.AccountMgr.GetArAccStatus(), "Id", "Status");
            return View(account);
        }

        // POST: ArAccounts/Create
        [HttpPost]
        public ActionResult CreateAccTrans([Bind(Include = "Id,Name,Landline,Email,Mobile,Company,Address,Remarks,ArAccStatusId,Landline2,Mobile2")] ArAccount account, int transId)
        {
            if (ModelState.IsValid && InputAccValidation(account))
            {
                ar.AccountMgr.AddAccount(account);
                //update transaction account
                ar.TransactionMgr.UpdateTransAcc(transId, account.Id);

                //ar.AccountMgr.AddAccntCreditDefault(account.Id);

                return RedirectToAction("Index");
            }

            ViewBag.TransId = transId;
            ViewBag.ArAccStatusId = new SelectList(ar.AccountMgr.GetArAccStatus(), "Id", "Status", account.ArAccStatusId);
            return View(account);
        }




        public ActionResult TransactionHistory(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var actionHistory = ar.TransactionMgr.GetTransactionById((int)id).ArActions.ToList();

            ViewBag.TransId = id;
            return View(actionHistory);
        }

        public ActionResult PostAndCloseTransaction(int id)
        {
            DateTime today = ar.DateClassMgr.GetCurrentDateTime();

            //get transaction
            var transaction = ar.TransactionMgr.GetTransactionById(id);

            decimal TotalAmount = transaction.Amount;
            decimal TotalPayment = ar.TransPaymentMgr.GetTotalTransPayment(id);
            decimal TotalBalance = TotalAmount - TotalPayment;

            if (TotalPayment >= TotalAmount)
            {
                //close
                ar.TransactionMgr.CloseTransactionStatus(id);

                //post
               var postResponse = ar.TransPostMgr.CreateTransPost(transaction, today, TotalBalance);

                if (postResponse)
                {
                    //add activity transaction closed 
                    ar.ActionMgr.AddAction(6, GetUser(), id);
                }
            }

            return RedirectToAction("Details", new { id = id });
        }

        [HttpGet]
        public JsonResult CheckAccountCredit(int id)
        {
            var account = ar.AccountMgr.GetLatestAccntCreditLimit(id);

            var creditLimit = new ArAccntCredit()
            {
                DtCredit = account.DtCredit,
                ArAccountId = account.ArAccountId,
                CreditLimit = account.CreditLimit,
                CreditWarning = account.CreditWarning,
                OverLimitAllowed = account.OverLimitAllowed
            };

            return Json(creditLimit, JsonRequestBehavior.AllowGet);
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

        //Post: \Receivables\ArTransactions\PostJobReceivables
        [HttpPost]
        public bool PostJobReceivables([Bind(Include = "Id,InvoiceId,DtInvoice,Description,DtEncoded,DtDue,Amount,Interval,IsRepeating,Remarks,ArTransStatusId,ArAccountId,ArCategoryId,DtService,DtServiceTo,InvoiceRef,PrevRef,NextRef,RepeatCount")] ArTransaction arTransaction,
            string Name, string Company, string Email, string Mobile)
        {
            try
            {
                arTransaction.ArAccountId = 1;
                arTransaction.ArCategoryId = 1;
                arTransaction.ArTransStatusId = 1;
                arTransaction.DtEncoded = ar.DateClassMgr.GetCurrentDateTime();
                arTransaction.IsRepeating = false;
                arTransaction.Interval = 0;
                arTransaction.InvoiceRef = arTransaction.InvoiceId.ToString();
                arTransaction.NextRef = 0;
                arTransaction.PrevRef = 0;
                arTransaction.RepeatCount = 0;
                arTransaction.Remarks = "";

                //validate
                if (ModelState.IsValid && InputValidation(arTransaction))
                {
                    var today = ar.DateClassMgr.GetCurrentDateTime();
                    var currentUser = HttpContext.User.Identity.Name;

                    //new account
                    if (!IsUserExist(Name))
                    {
                        var acctId = CreateUser(Company, Name, Email, Mobile);
                        arTransaction.ArAccountId = acctId;
                    }
                    else
                    {
                        //existing account
                        arTransaction.ArAccountId = GetUserAccountId(Name);
                    }

                    ardb.ArTransactions.Add(arTransaction);
                    ardb.SaveChanges();

                    //ar.TransactionMgr.AddTransaction(arTransaction);

                    //new transaction action history (new bill)
                    ar.ActionMgr.AddAction(1, currentUser, arTransaction.Id);
                }

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
                return false;
            }

        }

        #region Accounts
        //Check if the user exist on the current list
        public bool IsUserExist(string name)
        {
            var userExists = ardb.ArAccounts.Where(a => a.Name.Contains(name)).ToList();

            if (userExists.Count() > 0)
            {
                return true;
            }

            return false;
        }

        public int GetUserAccountId(string name)
        {
            var userExists = ardb.ArAccounts.Where(a => a.Name.Contains(name)).ToList();

            if (userExists.Count() > 0)
            {
                return userExists.FirstOrDefault().Id;
            }

            return 0;
        }

        // GET: ArTransactions/CreateUser
        // Create New User Account
        public int CreateUser(string company, string name, string email, string mobile)
        {
            try
            {

                ArAccount account = new ArAccount();
                account.Company = company;
                account.Name = name;
                account.Email = email;
                account.Mobile = mobile;
                account.ArAccStatusId = 1;

                ar.AccountMgr.AddAccount(account);

                return account.Id;
            }
            catch
            {
                return 0;
            }
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
