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
            if (String.IsNullOrEmpty(orderBy))
            {
                orderBy = "DESC";
            }

            var arTransactions = ar.TransactionMgr.GetTransactions(status, sortBy, orderBy);

            ViewBag.Status = status;
            ViewBag.SortBy = sortBy;
            ViewBag.OrderBy = orderBy;
            ViewBag.Today = ar.DateClassMgr.GetCurrentDate();
            ViewBag.IsAdmin = true;

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
            //transaction.Interval = 0;
            //transaction.PrevRef = 0;
            //transaction.NextRef = 0;
            transaction.InvoiceId = 0;

            ViewBag.ArTransStatusId = new SelectList(ar.TransactionMgr.GetTransactionStatus(), "Id", "Status");
            ViewBag.ArAccountId = new SelectList(ar.AccountMgr.GetArAccounts(), "Id", "Company");
            ViewBag.ArCategoryId = new SelectList(ar.CategoryMgr.GetCategories(), "Id", "Name");
            ViewBag.ArAccContactId = new SelectList(ar.AccountMgr.GetAccContacts(), "Id", "Name");
            return View(transaction);
        }

        // POST: ArTransactions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,InvoiceId,DtInvoice,Description,DtEncoded,DtDue,Amount,IsRepeating,Remarks,ArTransStatusId,ArAccountId,ArCategoryId,DtService,DtServiceTo,InvoiceRef,ArAccContactId")] ArTransaction arTransaction)
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

                ViewBag.ArTransStatusId = new SelectList(ar.TransactionMgr.GetTransactionStatus(), "Id", "Status");
                ViewBag.ArAccountId = new SelectList(ar.AccountMgr.GetArAccounts(), "Id", "Company");
                ViewBag.ArCategoryId = new SelectList(ar.CategoryMgr.GetCategories(), "Id", "Name");
                ViewBag.ArAccContactId = new SelectList(ar.AccountMgr.GetAccContacts(), "Id", "Name");

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
           
            ViewBag.ArTransStatusId = new SelectList(ar.TransactionMgr.GetTransactionStatus(), "Id", "Status", arTransaction.ArTransStatusId);
            ViewBag.ArAccountId = new SelectList(ar.AccountMgr.GetArAccounts(), "Id", "Company", arTransaction.ArAccountId);
            ViewBag.ArCategoryId = new SelectList(ar.CategoryMgr.GetCategories(), "Id", "Name", arTransaction.ArCategoryId);
            ViewBag.ArAccContactId = new SelectList(ar.AccountMgr.GetAccContacts(), "Id", "Name", arTransaction.ArAccContactId);

            return View(arTransaction);
        }

        // POST: ArTransactions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,InvoiceId,DtInvoice,Description,DtEncoded,DtDue,Amount,IsRepeating,Remarks,ArTransStatusId,ArAccountId,ArCategoryId,DtService,DtServiceTo,InvoiceRef,ArAccContactId")] ArTransaction arTransaction)
        {
            if (ModelState.IsValid && InputValidation(arTransaction))
            {
                ar.TransactionMgr.EditTransaction(arTransaction);
                return RedirectToAction("Details", new { id = arTransaction.Id });
            }

           
            ViewBag.ArTransStatusId = new SelectList(ar.TransactionMgr.GetTransactionStatus(), "Id", "Status", arTransaction.ArTransStatusId);
            ViewBag.ArAccountId = new SelectList(ar.AccountMgr.GetArAccounts(), "Id", "Company", arTransaction.ArAccountId);
            ViewBag.ArCategoryId = new SelectList(ar.CategoryMgr.GetCategories(), "Id", "Name", arTransaction.ArCategoryId);
            ViewBag.ArAccContactId = new SelectList(ar.AccountMgr.GetAccContacts(), "Id", "Name", arTransaction.ArAccContactId);
            return View(arTransaction);
        }


        // GET: ArTransactions/Edit/5
        public ActionResult EditRemarks(int? id)
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

            ViewBag.ArTransStatusId = new SelectList(ar.TransactionMgr.GetTransactionStatus(), "Id", "Status", arTransaction.ArTransStatusId);
            ViewBag.ArAccountId = new SelectList(ar.AccountMgr.GetArAccounts(), "Id", "Company", arTransaction.ArAccountId);
            ViewBag.ArCategoryId = new SelectList(ar.CategoryMgr.GetCategories(), "Id", "Name", arTransaction.ArCategoryId);
            ViewBag.ArAccContactId = new SelectList(ar.AccountMgr.GetAccContacts(), "Id", "Name", arTransaction.ArAccContactId);

            return View(arTransaction);
        }

        // POST: ArTransactions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditRemarks([Bind(Include = "Id,InvoiceId,DtInvoice,Description,DtEncoded,DtDue,Amount,IsRepeating,Remarks,ArTransStatusId,ArAccountId,ArCategoryId,DtService,DtServiceTo,InvoiceRef,ArAccContactId")] ArTransaction arTransaction)
        {
            if (ModelState.IsValid && InputValidation(arTransaction))
            {
                ar.TransactionMgr.EditTransaction(arTransaction);
                return RedirectToAction("Details", new { id = arTransaction.Id });
            }


            ViewBag.ArTransStatusId = new SelectList(ar.TransactionMgr.GetTransactionStatus(), "Id", "Status", arTransaction.ArTransStatusId);
            ViewBag.ArAccountId = new SelectList(ar.AccountMgr.GetArAccounts(), "Id", "Company", arTransaction.ArAccountId);
            ViewBag.ArCategoryId = new SelectList(ar.CategoryMgr.GetCategories(), "Id", "Name", arTransaction.ArCategoryId);
            ViewBag.ArAccContactId = new SelectList(ar.AccountMgr.GetAccContacts(), "Id", "Name", arTransaction.ArAccContactId);
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
            //if (transaction.Interval < 0)
            //{
            //    ModelState.AddModelError("Interval", "Invalid Interval");
            //    isValid = false;
            //}


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
        public bool PostJobReceivables([Bind(Include = "Id,InvoiceId,DtInvoice,Description,DtEncoded,DtDue,Amount,IsRepeating,Remarks,ArTransStatusId,ArAccountId,ArCategoryId,DtService,DtServiceTo")] ArTransaction arTransaction,
            string Name, string Company, string Email, string Mobile)
        {
            try
            {
                arTransaction.ArAccountId = 1;
                arTransaction.ArCategoryId = 1;
                arTransaction.ArTransStatusId = 2;
                arTransaction.DtEncoded = ar.DateClassMgr.GetCurrentDateTime();
                arTransaction.IsRepeating = false;
                arTransaction.InvoiceRef = arTransaction.InvoiceId.ToString();
                //arTransaction.Interval = 0;
                //arTransaction.NextRef = 0;
                //arTransaction.PrevRef = 0;
                //arTransaction.RepeatCount = 0;
                arTransaction.Remarks = "";

                //validate
                if (ModelState.IsValid && InputValidation(arTransaction))
                {
                    var today = ar.DateClassMgr.GetCurrentDateTime();
                    var currentUser = HttpContext.User.Identity.Name;

                    if (!IsUserExist(Name) && !IsCompanyExist(Company))
                    {
                        //new account, new user
                        var acctId = CreateUser(Company, Name, Email, Mobile);
                        arTransaction.ArAccountId = acctId;

                        //existing company new user
                        acctId = CreateAccountContact(Company, Name, Email, Mobile);
                        arTransaction.ArAccContactId = acctId; 
                    }
                    else if (!IsUserExist(Name) && IsCompanyExist(Company))
                    {

                        //existing account
                        arTransaction.ArAccountId = GetUserAccountId(Company);

                        //existing company new user
                        var acctId = CreateAccountContact(Company, Name, Email, Mobile);
                        arTransaction.ArAccContactId = acctId;
                    }
                    else
                    {
                        //existing account
                        arTransaction.ArAccountId = GetUserAccountId(Company);
                        arTransaction.ArAccContactId = GetUserAccountContactId(Name);
                    }

                    ar.TransactionMgr.AddTransaction(arTransaction);

                    //new transaction action history (new bill)
                    ar.ActionMgr.AddAction(1, currentUser, arTransaction.Id);
                }

                return true;
            }
            catch
            {
                // throw ex;
                return false;
            }

        }

        public ActionResult Settlement()
        {
            //get ongoing transactions
            var transactions = ar.TransactionMgr.GetForSettlementTrans();

            ViewBag.DateToday = ar.DateClassMgr.GetCurrentDateTime();
            return View(transactions);
        }

        #region Accounts
        //Check if the user exist on the current list
        public bool IsUserExist(string name)
        {
            var userExists = ardb.ArAccContacts.Where(a => a.Name == name).ToList();

            if (userExists.Count() > 0)
            {
                return true;
            }

            return false;
        }

        //Check if the user exist on the current list
        public bool IsCompanyExist(string company)
        {
            var userExists = ardb.ArAccounts.Where(a => a.Company == company).ToList();

            if (userExists.Count() > 0)
            {
                return true;
            }

            return false;
        }



        public int GetUserAccountId(string company)
        {
            var userExists = ardb.ArAccounts.Where(a => a.Company == company).ToList();

            if (userExists.Count() > 0)
            {
                return userExists.FirstOrDefault().Id;
            }

            return 0;
        }


        public int GetUserAccountContactId(string name)
        {
            var userExists = ardb.ArAccContacts.Where(a => a.Name==name).ToList();

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


        // GET: ArTransactions/CreateUser
        // Create New User Account
        public int CreateAccountContact(string company, string name, string email, string mobile)
        {
            try
            {

                //find company by name
                if (IsCompanyExist(company))
                {

                    var accountId = ardb.ArAccounts.Where(a => a.Company == company)
                                        .FirstOrDefault().Id;


                    ArAccContact contact = new ArAccContact();
                    contact.Name = name;
                    contact.Email = email;
                    contact.Mobile = mobile;
                    contact.ArAccountId = accountId;

                    ar.AccountMgr.AddAccContact(contact);

                    return contact.Id;
                }

                return 0;
            }
            catch
            {
                return 0;
            }
        }

        [HttpPost]
        public bool CloseTransctions(int[] TransIds)
        {
            try
            {

                var arTransIds = new List<int>(TransIds);

                foreach (var artransId in arTransIds)
                {
                    UpdateTransStatus(artransId, 6); //close transaction
                }

                return true;
            }
            catch
            {

                return false;
            }
        }

        public bool UpdateInvoiceId(int id, int invoiceId)
        {
            try
            {
                if (id == 0 || invoiceId == 0)
                {
                    return false;
                }

                var arTrans = ar.TransactionMgr.GetTransactionById(id);
                arTrans.InvoiceId = invoiceId;
                arTrans.InvoiceRef = invoiceId.ToString();
                ar.TransactionMgr.EditTransaction(arTrans);

                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion

        public ActionResult StatementPrint(int accountId)
        {
            decimal accBalance = 0;
            var account = ar.AccountMgr.GetAccountById(accountId);

            //get ongoing transactions
            var transactions = ar.TransactionMgr.GetApprovedTransactions()
                .Where(a => a.ArAccountId == accountId).ToList();

            List<ArRptModel.ArAccountStatement> accStatements = new List<ArRptModel.ArAccountStatement>();

            foreach (var statement in transactions)
            {

                accStatements.Add(new ArRptModel.ArAccountStatement
                {
                    ArTransId = statement.Id,
                    InvoiceId = statement.InvoiceId,
                    InvoiceRef = statement.InvoiceRef,
                    InvoiceDate = statement.DtInvoice,
                    Description = statement.Description,
                    StartDate = statement.DtService, 
                    EndDate   = statement.DtServiceTo ,
                    Amount    = statement.Amount,
                    Payment = 0

                });

                //Statement Amount 
                accBalance += statement.Amount;

                statement.ArTransPayments.ToList().ForEach(payment => {
                    //Statement Payments 
                    accBalance -= payment.ArPayment.Amount;

                    accStatements.Add(new ArRptModel.ArAccountStatement
                    {
                        ArTransId = payment.Id,
                        InvoiceId = payment.ArTransaction.InvoiceId,
                        InvoiceRef = payment.ArTransaction.InvoiceRef,
                        InvoiceDate = payment.ArPayment.DtPayment,
                        Description = payment.ArPayment.ArPaymentType.Type + " Payment for " + payment.ArTransaction.InvoiceRef,
                        Amount = 0,
                        Payment = payment.ArPayment.Amount
                    });
                });
            }

            var user = HttpContext.User.Identity.Name;

            ViewBag.Company = account.Company;
            ViewBag.CompanyAddress = account.Address;
            ViewBag.DateToday = ar.DateClassMgr.GetCurrentDateTime();
            ViewBag.AccBalance = accBalance;
            ViewBag.PreparedBy = GetStaffName(user);
            ViewBag.PreparedSign = GetStaffSign(user);

            return View(accStatements.OrderBy(c => c.InvoiceDate));
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


        public string GetStaffName(string staffLogin)
        {
            switch (staffLogin)
            {
                case "josette.realbreeze@gmail.com":
                    return "Josette Valleser";
                case "mae.realbreeze@gmail.com":
                    return "Cristel Mae Verano";
                case "ramil.realbreeze@gmail.com":
                    return "Ramil Villahermosa";
                case "grace.realbreeze@gmail.com":
                    return "Grace-chell V. Capandac";
                case "assalvatierra@gmail.com":
                    return "Elvie S. Salvatierra ";
                case "jecca.realbreeze@gmail.com":
                    return "Jecca Bilason";
                case "kimberly.realbreeze@gmail.com":
                    return "Kimberly Pangubatan";
                default:
                    return "Elvie S. Salvatierra ";
            }
        }

        public string GetStaffSign(string staffLogin)
        {
            switch (staffLogin)
            {
                case "josette.realbreeze@gmail.com":
                    return "/Images/Signature/JoSign.jpg";
                case "mae.realbreeze@gmail.com":
                    return "/Images/Signature/MaeSign.jpg";
                case "ramil.realbreeze@gmail.com":
                    return "/Images/Signature/RamSign.jpg";
                case "grace.realbreeze@gmail.com":
                    return "/Images/Signature/GraceSign.jpg";
                case "assalvatierra@gmail.com":
                    return "/Images/Signature-1.png";
                case "jecca.realbreeze@gmail.com":
                    return "/Images/Signature/JeccaSign.jpg";
                case "kimberly.realbreeze@gmail.com":
                    return "/Images/Signature/KimSign.jpg";
                default:
                    return "/Images/Signature-1.png";
            }
        }

        public class ArStatement
        {
            public DateTime InvoiceDate { get; set; }
            public String Description { get; set; }
            public Decimal Amount { get; set; }
            public Decimal Payment { get; set; }
        }
    }
}
