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
    public class ApCashFlowsController : Controller
    {
        private PayablesFactory ap = new PayablesFactory();
        private DateClassMgr dt = new DateClassMgr();
        private ApDBContainer db = new ApDBContainer();

        private enum STATUS : int
        {
            NEW = 6,
            REQUEST = 1,
            APPROVED = 2,
            RELEASED = 3,
            RETURNED = 5,
            CLOSED = 4
        };


        private enum CASHFLOWTYPE : int
        {
            DEBIT = 1,
            CREDIT = 2,
        };


        // GET: Payables/ApCashFlows
        public ActionResult Index()
        {
            try
            {
                var cashflow = ap.CashFlowMgr.GetCashFlowModelActive();

                var today = ap.DateClassMgr.GetCurrentDate();
                ViewBag.Today = today.ToString("MMM dd yyyy");
                ViewBag.IsAdmin = User.IsInRole("Admin");

                return View(cashflow.OrderBy(c => c.Date));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // GET: Payables/ApCashFlows/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApCashFlow apCashFlow = db.ApCashFlows.Find(id);
            if (apCashFlow == null)
            {
                return HttpNotFound();
            }
            return View(apCashFlow);
        }

        // GET: Payables/ApCashFlows/Create
        public ActionResult Create()
        {
            ApCashFlow apCashFlow = new ApCashFlow();

            apCashFlow.PerformedBy = GetUser();

            int pettyCashDefault = db.ApAccounts.Where(a => a.Name == "Petty Cash").FirstOrDefault().Id;
            ViewBag.ApCashFlowTypeId = new SelectList(db.ApCashFlowTypes, "Id", "Type", 2);
            ViewBag.ApAccountId = new SelectList(db.ApAccounts, "Id", "Name", pettyCashDefault);
            return View(apCashFlow);
        }

        // POST: Payables/ApCashFlows/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Description,Amount,Date,Remarks,ApCashFlowTypeId,ApAccountId,PerformedBy")] ApCashFlow apCashFlow)
        {
            if (ModelState.IsValid)
            {
                db.ApCashFlows.Add(apCashFlow);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.User = GetUser();
            ViewBag.ApCashFlowTypeId = new SelectList(db.ApCashFlowTypes, "Id", "Type", apCashFlow.ApCashFlowTypeId);
            ViewBag.ApAccountId = new SelectList(db.ApAccounts, "Id", "Name", apCashFlow.ApAccountId);
            return View(apCashFlow);
        }

        // GET: Payables/ApCashFlows/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApCashFlow apCashFlow = db.ApCashFlows.Find(id);
            if (apCashFlow == null)
            {
                return HttpNotFound();
            }
            //ViewBag.User = GetUser();
            ViewBag.ApCashFlowTypeId = new SelectList(db.ApCashFlowTypes, "Id", "Type", apCashFlow.ApCashFlowTypeId);
            ViewBag.ApAccountId = new SelectList(db.ApAccounts, "Id", "Name", apCashFlow.ApAccountId);
            return View(apCashFlow);
        }

        // POST: Payables/ApCashFlows/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Description,Amount,Date,Remarks,ApCashFlowTypeId,ApAccountId,PerformedBy")] ApCashFlow apCashFlow)
        {
            if (ModelState.IsValid)
            {
                db.Entry(apCashFlow).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.User = GetUser();
            ViewBag.ApCashFlowTypeId = new SelectList(db.ApCashFlowTypes, "Id", "Type", apCashFlow.ApCashFlowTypeId);
            ViewBag.ApAccountId = new SelectList(db.ApAccounts, "Id", "Name", apCashFlow.ApAccountId);
            return View(apCashFlow);
        }

        // GET: Payables/ApCashFlows/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApCashFlow apCashFlow = db.ApCashFlows.Find(id);
            if (apCashFlow == null)
            {
                return HttpNotFound();
            }
            return View(apCashFlow);
        }

        // POST: Payables/ApCashFlows/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ApCashFlow apCashFlow = db.ApCashFlows.Find(id);
            db.ApCashFlows.Remove(apCashFlow);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        //POST: Payables/ApCashFlows/CashFlowPost
        [HttpPost]
        public bool CashFlowPost([Bind(Include = "Id,DtPost,Amount,Balance,Cash")] ApTransPost apTransPost)
        {
            try
            {
                apTransPost.DtPost = dt.GetCurrentDate();
                if (ModelState.IsValid)
                {

                    //get cashflows for today
                    var datetoday = ap.DateClassMgr.GetCurrentDate();

                    db.ApTransPosts.Add(apTransPost);

                    //get unposted cashflows
                    var cashflow = ap.CashFlowMgr.GetCashFlowActive();

                    //add opening balance cashflow
                    if (cashflow != null)
                    {
                        var cashflowStartDate = cashflow.First().Date.Date;
                        cashflow.Add(ap.CashFlowMgr.AddOpeningBalance(cashflowStartDate));
                    }

                    //create cash flow posted group collection
                    foreach (var cash in cashflow)
                    {
                        db.ApCashFlowPostGroups.Add(new ApCashFlowPostGroup
                        {
                            ApTransPostId = apTransPost.Id,
                            ApCashFlowId = cash.Id
                        });
                    }

                    //save
                    db.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
                //return false;
            }
        }


        #region Posted Cashflow

        public ActionResult Posted()
        {
            try
            {
                var cashFlows = db.ApTransPosts.ToList();
                return View(cashFlows);
            }
            catch
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }


        public ActionResult PostedDetails(int? id)
        {
            try
            {

                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
                }

                var postedDetails = db.ApTransPosts.Find(id);

                if (postedDetails == null)
                {
                    return HttpNotFound();
                }

                List<CashFlowModel> cashFlowModel = ap.CashFlowMgr.GetCashFlowListByPostedId((int)id);

                ViewBag.Today = postedDetails.DtPost.ToString("MMM dd yyyy");

                return View(cashFlowModel.OrderBy(c => c.Date));
            }
            catch
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }


        public ActionResult PostedDelete(int? id)
        {
            try
            {

                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
                }

                var postedDetails = db.ApTransPosts.Find(id);

                if (postedDetails == null)
                {
                    return HttpNotFound();
                }

                db.ApCashFlowPostGroups.RemoveRange(postedDetails.ApCashFlowPostGroups);
                db.ApTransPosts.Remove(postedDetails);

                db.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }
        #endregion

        public string GetUser()
        {
            return HttpContext.User.Identity.Name ?? "Unknown";
        }
    }
}
