using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using ArModels.Models;
using ArServices;
using Microsoft.Ajax.Utilities;

namespace JobsV1.Areas.Receivables.Controllers
{
    public class ArAccountsController : Controller
    {
        private ArDBContainer db = new ArDBContainer();
        private ReceivableFactory ar = new ReceivableFactory();

        // GET: ArAccounts
        public ActionResult Index()
        {
            var accountList = ar.AccountMgr.GetArAccounts();

            return View(accountList);
        }

        // GET: ArAccounts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var account = ar.AccountMgr.GetAccountById((int)id);

            if (account == null)
            {
                return HttpNotFound();
            }

            ViewBag.LastestCredit = ar.AccountMgr.GetLatestAccntCreditLimit((int)id);
            ViewBag.LastestTerms = ar.AccountMgr.GetLatestAccntPaymentTerm((int)id);
            return View(account);
        }

        // GET: ArAccounts/Create
        public ActionResult Create()
        {

            ViewBag.ArAccStatusId = new SelectList(ar.AccountMgr.GetArAccStatus(), "Id", "Status");
            return View();
        }

        // POST: ArAccounts/Create
        [HttpPost]
        public ActionResult Create([Bind(Include = "Id,Name,Landline,Email,Mobile,Company,Address,Remarks,ArAccStatusId,Landline2,Mobile2")] ArAccount account)
        {
            if (ModelState.IsValid && InputValidation(account))
            {
                ar.AccountMgr.AddAccount(account);

                return RedirectToAction("Index");
            }

            ViewBag.ArAccStatusId = new SelectList(ar.AccountMgr.GetArAccStatus(), "Id", "Status", account.ArAccStatusId);
            return View(account);
        }



        // GET: ArAccounts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var account = ar.AccountMgr.GetAccountById((int)id);
            if (account == null)
            {
                return HttpNotFound();
            }

            ViewBag.ArAccStatusId = new SelectList(ar.AccountMgr.GetArAccStatus(), "Id", "Status");
            return View(account);
        }

        // POST: ArAccounts/Edit/5
        [HttpPost]
        public ActionResult Edit([Bind(Include = "Id,Name,Landline,Email,Mobile,Company,Address,Remarks,ArAccStatusId,Landline2,Mobile2")] ArAccount account)
        {
            if (ModelState.IsValid && InputValidation(account))
            {
                ar.AccountMgr.EditAccount(account);
                return RedirectToAction("Index");
            }
            ViewBag.ArAccStatusId = new SelectList(ar.AccountMgr.GetArAccStatus(), "Id", "Status");
            return View(account);
        }

        // GET: ArAccounts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var account = ar.AccountMgr.GetAccountById((int)id);
            if (account == null)
            {
                return HttpNotFound();
            }
            return View(account);
        }

        // POST: ArAccounts/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                var account = ar.AccountMgr.GetAccountById((int)id);
                ar.AccountMgr.RemoveAccount(account);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public bool InputValidation(ArAccount account)
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
            else
            {
                //if(!Regex.IsMatch(account.Email, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"))
                //{
                //    isValid = false;
                //}
            }


            if (account.Company.IsNullOrWhiteSpace())
            {
                ModelState.AddModelError("Company", "Invalid Company Name");
                isValid = false;
            }

            if (account.Mobile.IsNullOrWhiteSpace())
            {
                ModelState.AddModelError("Mobile", "Invalid Mobile");
                isValid = false;
            }


            return isValid;
        }

    }
}
