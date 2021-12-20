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
using Microsoft.Ajax.Utilities;

namespace JobsV1.Areas.Payables.Controllers
{
    public class ApAccountsController : Controller
    {
        private PayablesFactory ap = new PayablesFactory();

        // GET: Payables/ApAccounts
        public ActionResult Index()
        {
            var apAccounts = ap.AccountMgr.GetAccounts();
            return View(apAccounts);
        }

        // GET: Payables/ApAccounts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApAccount apAccount = ap.AccountMgr.GetAccountById((int)id);
            if (apAccount == null)
            {
                return HttpNotFound();
            }
            return View(apAccount);
        }

        // GET: Payables/ApAccounts/Create
        public ActionResult Create()
        {
            ViewBag.ApAccStatusId = new SelectList(ap.AccountMgr.GetAccStatus(), "Id", "Status");
            return View();
        }

        // POST: Payables/ApAccounts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Landline,Mobile,Email,ContactPerson,Address,Remarks,ApAccStatusId")] ApAccount apAccount)
        {
            if (ModelState.IsValid && InputAccValidation(apAccount))
            {
                ap.AccountMgr.AddAccount(apAccount);
                return RedirectToAction("Index");
            }

            ViewBag.ApAccStatusId = new SelectList(ap.AccountMgr.GetAccStatus(), "Id", "Status", apAccount.ApAccStatusId);
            return View(apAccount);
        }

        public ActionResult CreateTransAcc(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ViewBag.ApAccStatusId = new SelectList(ap.AccountMgr.GetAccStatus(), "Id", "Status");
            ViewBag.TransId = id;
            return View();
        }

        // POST: Payables/ApAccounts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateTransAcc([Bind(Include = "Id,Name,Landline,Mobile,Email,ContactPerson,Address,Remarks,ApAccStatusId")] ApAccount apAccount, int TransId)
        {
            if (TransId == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (ModelState.IsValid && InputAccValidation(apAccount))
            {
                ap.AccountMgr.AddAccount(apAccount);

                //update transaction account reference
                ap.TransactionMgr.UpdateTransactionAccount(TransId, apAccount.Id);


                return RedirectToAction("Details", "ApTransactions",new { id = TransId });
            }

            ViewBag.ApAccStatusId = new SelectList(ap.AccountMgr.GetAccStatus(), "Id", "Status", apAccount.ApAccStatusId);
            ViewBag.TransId = TransId;
            return View(apAccount);
        }


        // GET: Payables/ApAccounts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApAccount apAccount = ap.AccountMgr.GetAccountById((int)id);
            if (apAccount == null)
            {
                return HttpNotFound();
            }
            ViewBag.ApAccStatusId = new SelectList(ap.AccountMgr.GetAccStatus(), "Id", "Status", apAccount.ApAccStatusId);
            return View(apAccount);
        }

        // POST: Payables/ApAccounts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Landline,Mobile,Email,ContactPerson,Address,Remarks,ApAccStatusId")] ApAccount apAccount)
        {
            if (ModelState.IsValid)
            {
                ap.AccountMgr.EditAccount(apAccount);
                return RedirectToAction("Index");
            }
            ViewBag.ApAccStatusId = new SelectList(ap.AccountMgr.GetAccStatus(), "Id", "Status", apAccount.ApAccStatusId);
            return View(apAccount);
        }

        // GET: Payables/ApAccounts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApAccount apAccount = ap.AccountMgr.GetAccountById((int)id);
            if (apAccount == null)
            {
                return HttpNotFound();
            }
            return View(apAccount);
        }

        // POST: Payables/ApAccounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ApAccount apAccount = ap.AccountMgr.GetAccountById((int)id);
            ap.AccountMgr.DeleteAccount(apAccount);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                ap.AccountMgr.DisposeDB();
            }
            base.Dispose(disposing);
        }


        public bool InputAccValidation(ApAccount account)
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

            if (account.Mobile.IsNullOrWhiteSpace() )
            {
                ModelState.AddModelError("Mobile", "Invalid Mobile");
                isValid = false;
            }


            return isValid;
        }
    }
}
