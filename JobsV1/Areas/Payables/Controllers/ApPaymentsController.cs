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
    public class ApPaymentsController : Controller
    {
        private PayablesFactory ap = new PayablesFactory();

        // GET: Payables/ApPayments
        public ActionResult Index()
        {
            var apPayments = ap.PaymentMgr.GetPayments();
            return View(apPayments.ToList());
        }

        // GET: Payables/ApPayments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApPayments apPayments = ap.PaymentMgr.GetPaymentById((int)id);
            if (apPayments == null)
            {
                return HttpNotFound();
            }
            return View(apPayments);
        }

        // GET: Payables/ApPayments/Create
        public ActionResult Create()
        {
            ViewBag.ApAccountId = new SelectList(ap.AccountMgr.GetAccounts(), "Id", "Name");
            ViewBag.ApPaymentTypeId = new SelectList(ap.PaymentMgr.GetPaymentTypes(), "Id", "Type");
            ViewBag.ApPaymentStatusId = new SelectList(ap.PaymentMgr.GetPaymentStatus(), "Id", "Status");
            return View();
        }

        // POST: Payables/ApPayments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,DtPayment,Amount,Remarks,ApAccountId,ApPaymentTypeId,ApPaymentStatusId")] ApPayments apPayments)
        {
            if (ModelState.IsValid)
            {
                ap.PaymentMgr.AddPayment(apPayments);
                return RedirectToAction("Index");
            }

            ViewBag.ApAccountId = new SelectList(ap.AccountMgr.GetAccounts(), "Id", "Name", apPayments.ApAccountId);
            ViewBag.ApPaymentTypeId = new SelectList(ap.PaymentMgr.GetPaymentTypes(), "Id", "Type", apPayments.ApPaymentTypeId);
            ViewBag.ApPaymentStatusId = new SelectList(ap.PaymentMgr.GetPaymentStatus(), "Id", "Status", apPayments.ApPaymentStatusId);
            return View(apPayments);
        }

        // GET: Payables/ApPayments/Create
        public ActionResult CreateTransPayment(int? transId)
        {
            if (transId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var transaction = ap.TransactionMgr.GetTransactionById((int)transId);

            if (transaction == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ViewBag.ApAccountId = new SelectList(ap.AccountMgr.GetAccounts(), "Id", "Name", transaction.ApAccountId);
            ViewBag.ApPaymentTypeId = new SelectList(ap.PaymentMgr.GetPaymentTypes(), "Id", "Type");
            ViewBag.ApPaymentStatusId = new SelectList(ap.PaymentMgr.GetPaymentStatus(), "Id", "Status");
            ViewBag.TransId = transId;
            return View();
        }

        // POST: Payables/ApPayments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateTransPayment([Bind(Include = "Id,DtPayment,Amount,Remarks,ApAccountId,ApPaymentTypeId,ApPaymentStatusId")] ApPayments apPayments, int transId)
        {
            if (transId == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (ModelState.IsValid)
            {
                ap.PaymentMgr.AddPayment(apPayments);

                ap.TransactionMgr.AddTransPayment(transId, apPayments.Id);

                AddPaymentAction(apPayments, transId);

                return RedirectToAction("Details", "ApTransactions", new { id = transId });
            }

            ViewBag.ApAccountId = new SelectList(ap.AccountMgr.GetAccounts(), "Id", "Name", apPayments.ApAccountId);
            ViewBag.ApPaymentTypeId = new SelectList(ap.PaymentMgr.GetPaymentTypes(), "Id", "Type", apPayments.ApPaymentTypeId);
            ViewBag.ApPaymentStatusId = new SelectList(ap.PaymentMgr.GetPaymentStatus(), "Id", "Status", apPayments.ApPaymentStatusId);
            ViewBag.TransId = transId;
            return View(apPayments);
        }

        private void AddPaymentAction(ApPayments apPayments, int transId)
        {
            if (apPayments.ApPaymentStatusId == 1)
            {
                //add action log for transaction create 
                ap.ActionMgr.AddAction(GetUser(), transId, 6);
            }
            if (apPayments.ApPaymentStatusId == 2)
            {
                //add action log for transaction create 
                ap.ActionMgr.AddAction(GetUser(), transId, 7);
            }
            if (apPayments.ApPaymentStatusId == 3)
            {
                //add action log for transaction create 
                ap.ActionMgr.AddAction(GetUser(), transId, 8);
            }

        }

        // GET: Payables/ApPayments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApPayments apPayments = ap.PaymentMgr.GetPaymentById((int)id);
            if (apPayments == null)
            {
                return HttpNotFound();
            }
            ViewBag.ApAccountId = new SelectList(ap.AccountMgr.GetAccounts(), "Id", "Name", apPayments.ApAccountId);
            ViewBag.ApPaymentTypeId = new SelectList(ap.PaymentMgr.GetPaymentTypes(), "Id", "Type", apPayments.ApPaymentTypeId);
            ViewBag.ApPaymentStatusId = new SelectList(ap.PaymentMgr.GetPaymentStatus(), "Id", "Status", apPayments.ApPaymentStatusId);
            return View(apPayments);
        }

        // POST: Payables/ApPayments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,DtPayment,Amount,Remarks,ApAccountId,ApPaymentTypeId,ApPaymentStatusId")] ApPayments apPayments)
        {
            if (ModelState.IsValid)
            {
                ap.PaymentMgr.EditPayment(apPayments);

                return RedirectToAction("Index");
            }
            ViewBag.ApAccountId = new SelectList(ap.AccountMgr.GetAccounts(), "Id", "Name", apPayments.ApAccountId);
            ViewBag.ApPaymentTypeId = new SelectList(ap.PaymentMgr.GetPaymentTypes(), "Id", "Type", apPayments.ApPaymentTypeId);
            ViewBag.ApPaymentStatusId = new SelectList(ap.PaymentMgr.GetPaymentStatus(), "Id", "Status", apPayments.ApPaymentStatusId);
            return View(apPayments);
        }


        // GET: Payables/ApPayments/EditTransPayment/5
        public ActionResult EditTransPayment(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApPayments apPayments = ap.PaymentMgr.GetPaymentById((int)id);
            if (apPayments == null)
            {
                return HttpNotFound();
            }
            ViewBag.ApAccountId = new SelectList(ap.AccountMgr.GetAccounts(), "Id", "Name", apPayments.ApAccountId);
            ViewBag.ApPaymentTypeId = new SelectList(ap.PaymentMgr.GetPaymentTypes(), "Id", "Type", apPayments.ApPaymentTypeId);
            ViewBag.ApPaymentStatusId = new SelectList(ap.PaymentMgr.GetPaymentStatus(), "Id", "Status", apPayments.ApPaymentStatusId);
            ViewBag.TransId = ap.PaymentMgr.GetFirstorDefault_TransId(apPayments.Id);
            return View(apPayments);
        }

        // POST: Payables/ApPayments/EditTransPayment/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditTransPayment([Bind(Include = "Id,DtPayment,Amount,Remarks,ApAccountId,ApPaymentTypeId,ApPaymentStatusId")] ApPayments apPayments)
        {
            var transId = ap.PaymentMgr.GetFirstorDefault_TransId(apPayments.Id);

            if (ModelState.IsValid)
            {
                ap.PaymentMgr.EditPayment(apPayments);

                //add edit payment action
                AddPaymentAction(apPayments, transId);

                return RedirectToAction("Details", "ApTransactions", new { id = transId });
            }
            ViewBag.ApAccountId = new SelectList(ap.AccountMgr.GetAccounts(), "Id", "Name", apPayments.ApAccountId);
            ViewBag.ApPaymentTypeId = new SelectList(ap.PaymentMgr.GetPaymentTypes(), "Id", "Type", apPayments.ApPaymentTypeId);
            ViewBag.ApPaymentStatusId = new SelectList(ap.PaymentMgr.GetPaymentStatus(), "Id", "Status", apPayments.ApPaymentStatusId);
            ViewBag.TransId = transId;
            return View(apPayments);
        }

        // GET: Payables/ApPayments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApPayments apPayments = ap.PaymentMgr.GetPaymentById((int)id);
            if (apPayments == null)
            {
                return HttpNotFound();
            }
            return View(apPayments);
        }

        // POST: Payables/ApPayments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ApPayments apPayments = ap.PaymentMgr.GetPaymentById((int)id);
            ap.PaymentMgr.DeletePayment(apPayments);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                ap.DbDispose();
            }
            base.Dispose(disposing);
        }


        public string GetUser()
        {
            return HttpContext.User.Identity.Name ?? "Unknown";
        }
    }
}
