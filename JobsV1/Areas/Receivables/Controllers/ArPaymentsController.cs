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
using Microsoft.Ajax.Utilities;

namespace JobsV1.Areas.Receivables.Controllers
{
    public class ArPaymentsController : Controller
    {
        private ReceivableFactory ar = new ReceivableFactory();

        // GET: ArPayments
        public ActionResult Index()
        {
            var arPayments = ar.PaymentMgr.GetPayments();
            return View(arPayments.ToList());
        }

        // GET: ArPayments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ArPayment arPayment = ar.PaymentMgr.GetPaymentById(id);
            if (arPayment == null)
            {
                return HttpNotFound();
            }
            return View(arPayment);
        }

        // GET: ArPayments/Create
        public ActionResult Create()
        {
            ArPayment payment = new ArPayment();
            payment.Amount = 0;

            ViewBag.ArAccountId = new SelectList(ar.AccountMgr.GetArAccounts(), "Id", "Name");
            ViewBag.ArPaymentTypeId = new SelectList(ar.PaymentMgr.GetPaymentTypes(), "Id", "Type");
            return View(payment);
        }

        // POST: ArPayments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,DtPayment,Amount,Remarks,Reference,ArAccountId,ArPaymentTypeId")] ArPayment arPayment)
        {
            if (ModelState.IsValid && InputValidation(arPayment))
            {
                ar.PaymentMgr.AddPayment(arPayment);
                return RedirectToAction("Index");
            }

            ViewBag.ArAccountId = new SelectList(ar.AccountMgr.GetArAccounts(), "Id", "Name", arPayment.ArAccountId);
            ViewBag.ArPaymentTypeId = new SelectList(ar.PaymentMgr.GetPaymentTypes(), "Id", "Type", arPayment.ArPaymentTypeId);
            return View(arPayment);
        }

        // GET: ArPayments/Create
        public ActionResult CreateTransPayment(int transId)
        {
            ArPayment payment = new ArPayment();
            payment.Amount = 0;
            payment.ArAccountId = ar.TransactionMgr.GetTransAccountId(transId);

            ViewBag.ArAccountId = new SelectList(ar.AccountMgr.GetArAccounts(), "Id", "Name", payment.ArAccountId);
            ViewBag.ArPaymentTypeId = new SelectList(ar.PaymentMgr.GetPaymentTypes(), "Id", "Type");
            ViewBag.TransId = transId;
            return View(payment);
        }

        // POST: ArPayments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateTransPayment([Bind(Include = "Id,DtPayment,Amount,Remarks,Reference,ArAccountId,ArPaymentTypeId")] ArPayment arPayment, int transId)
        {
            if (ModelState.IsValid && InputValidation(arPayment))
            {
                ar.PaymentMgr.AddPayment(arPayment);
                var createResponse = ar.TransPaymentMgr.AddTransPayment(transId, arPayment.Id);
 

                if (createResponse)
                {
                    //add activity based on statusId

                    ar.ActionMgr.AddAction(7, GetUser(), (int)transId);
                }

                return RedirectToAction("Details", "ArTransactions",new { id = transId });
            }

            ViewBag.ArAccountId = new SelectList(ar.AccountMgr.GetArAccounts(), "Id", "Name", arPayment.ArAccountId);
            ViewBag.ArPaymentTypeId = new SelectList(ar.PaymentMgr.GetPaymentTypes(), "Id", "Type", arPayment.ArPaymentTypeId);
            ViewBag.TransId = transId;
            return View(arPayment);
        }

        // GET: ArPayments/Create
        public ActionResult CreateSettlePayment(int? transId)
        {
            if (transId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ArPayment payment = new ArPayment();
            payment.Amount = 0;
            payment.ArAccountId = ar.TransactionMgr.GetTransAccountId((int)transId);

            ViewBag.ArAccountId = new SelectList(ar.AccountMgr.GetArAccounts(), "Id", "Name", payment.ArAccountId);
            ViewBag.ArPaymentTypeId = new SelectList(ar.PaymentMgr.GetPaymentTypes(), "Id", "Type");
            ViewBag.TransId = transId;
            ViewBag.AccountName = ar.TransactionMgr.GetTransactionById((int)transId).ArAccount.Name;
            return View(payment);
        }

        // POST: ArPayments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateSettlePayment([Bind(Include = "Id,DtPayment,Amount,Remarks,Reference,ArAccountId,ArPaymentTypeId")] ArPayment arPayment, int transId)
        {
            if (ModelState.IsValid && InputValidation(arPayment))
            {
                ar.PaymentMgr.AddPayment(arPayment);
                var createResponse = ar.TransPaymentMgr.AddTransPayment(transId, arPayment.Id);


                if (createResponse)
                {
                    //add activity based on statusId

                    ar.ActionMgr.AddAction(7, GetUser(), (int)transId);
                }

                return RedirectToAction("Settlement", "ArMgt");
            }

            ViewBag.ArAccountId = new SelectList(ar.AccountMgr.GetArAccounts(), "Id", "Name", arPayment.ArAccountId);
            ViewBag.ArPaymentTypeId = new SelectList(ar.PaymentMgr.GetPaymentTypes(), "Id", "Type", arPayment.ArPaymentTypeId);
            ViewBag.TransId = transId;
            ViewBag.AccountName = ar.TransactionMgr.GetTransactionById((int)transId).ArAccount.Name;
            return View(arPayment);
        }

        // GET: ArPayments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ArPayment arPayment = ar.PaymentMgr.GetPaymentById(id);
            if (arPayment == null)
            {
                return HttpNotFound();
            }
            ViewBag.ArAccountId = new SelectList(ar.AccountMgr.GetArAccounts(), "Id", "Name", arPayment.ArAccountId);
            ViewBag.ArPaymentTypeId = new SelectList(ar.PaymentMgr.GetPaymentTypes(), "Id", "Type", arPayment.ArPaymentTypeId);
            return View(arPayment);
        }

        // POST: ArPayments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,DtPayment,Amount,Remarks,Reference,ArAccountId,ArPaymentTypeId")] ArPayment arPayment)
        {
            if (ModelState.IsValid && InputValidation(arPayment))
            {
                ar.PaymentMgr.EditPayment(arPayment);
                var transId = ar.TransPaymentMgr.GetTransPaymentsByPaymentId(arPayment.Id);
                return RedirectToAction("Details", "ArTransactions", new { id = transId });
            }
            ViewBag.ArAccountId = new SelectList(ar.AccountMgr.GetArAccounts(), "Id", "Name", arPayment.ArAccountId);
            ViewBag.ArPaymentTypeId = new SelectList(ar.PaymentMgr.GetPaymentTypes(), "Id", "Type", arPayment.ArPaymentTypeId);
            return View(arPayment);
        }


        // GET: ArPayments/Edit/5
        public ActionResult EditTransPayment(int? id, int transId)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ArPayment arPayment = ar.PaymentMgr.GetPaymentById(id);
            if (arPayment == null)
            {
                return HttpNotFound();
            }
            ViewBag.TransId = transId;
            ViewBag.ArAccountId = new SelectList(ar.AccountMgr.GetArAccounts(), "Id", "Name", arPayment.ArAccountId);
            ViewBag.ArPaymentTypeId = new SelectList(ar.PaymentMgr.GetPaymentTypes(), "Id", "Type", arPayment.ArPaymentTypeId);
            return View(arPayment);
        }

        // POST: ArPayments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditTransPayment([Bind(Include = "Id,DtPayment,Amount,Remarks,Reference,ArAccountId,ArPaymentTypeId")] ArPayment arPayment, int transId)
        {
            if (ModelState.IsValid && InputValidation(arPayment))
            {
                ar.PaymentMgr.EditPayment(arPayment);
                return RedirectToAction("Details", "ArTransactions", new { id = transId });
            }
            ViewBag.TransId = transId;
            ViewBag.ArAccountId = new SelectList(ar.AccountMgr.GetArAccounts(), "Id", "Name", arPayment.ArAccountId);
            ViewBag.ArPaymentTypeId = new SelectList(ar.PaymentMgr.GetPaymentTypes(), "Id", "Type", arPayment.ArPaymentTypeId);
            return View(arPayment);
        }

        // GET: ArPayments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ArPayment arPayment = ar.PaymentMgr.GetPaymentById(id);
            if (arPayment == null)
            {
                return HttpNotFound();
            }
            return View(arPayment);
        }

        // POST: ArPayments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            //remove payment transaction
            var transPayment = ar.TransPaymentMgr.GetTransPaymentsByPaymentId(id);
            ar.TransPaymentMgr.RemoveTransPayment(transPayment);
            var transId = transPayment.ArTransactionId;

            //remove payment
            ar.PaymentMgr.RemovePayment(id);
            return RedirectToAction("Details", "ArTransactions", new { id = transId });
        }


        // GET: ArPayments/Delete/5
        public ActionResult DeleteTransPayment(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ArPayment arPayment = ar.PaymentMgr.GetPaymentById(id);
            if (arPayment == null)
            {
                return HttpNotFound();
            }
            return View(arPayment);
        }

        // POST: ArPayments/Delete/5
        [HttpPost, ActionName("DeleteTransPayment")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteTransPaymentConfirmed(int id)
        {
            //remove payment transaction
            var transPayment = ar.TransPaymentMgr.GetTransPaymentsByPaymentId(id);
            ar.TransPaymentMgr.RemoveTransPayment(transPayment);

            var transId = transPayment.ArTransactionId;

            //remove payment
            ar.PaymentMgr.RemovePayment(id);
            return RedirectToAction("Details", "ArTransactions", new { id = transId });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                ar.PaymentMgr.DbDispose();
            }
            base.Dispose(disposing);
        }


        public bool InputValidation(ArPayment payment)
        {
            bool isValid = true;

            if (payment.Amount < 0)
            {
                ModelState.AddModelError("Amount", "Invalid Amount");
                isValid = false;
            }


            return isValid;
        }


        private string GetUser()
        {
            if (HttpContext.User.Identity.Name != "")
            {
                return HttpContext.User.Identity.Name;
            }
            else
            {
                return "Not Log In";
            }
        }

    }
}
