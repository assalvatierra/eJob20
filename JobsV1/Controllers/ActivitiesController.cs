using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JobsV1.Models.Class;
using JobsV1.Models;
using System.Data.Entity;
using System.Net;

namespace JobsV1.Controllers
{
    public class ActivitiesController : Controller
    {
        private ActivityClass ac = new ActivityClass();
        private JobDBContainer db = new JobDBContainer();
        private DateClass dt = new DateClass();
        private DBClasses dbc= new DBClasses();

        private List<SelectListItem> ActivityStatus = new List<SelectListItem> {
                new SelectListItem { Value = "Others", Text = "Others" },
                new SelectListItem { Value = "Indicated Price", Text = "Indicated Price" },
                new SelectListItem { Value = "Bidding Only", Text = "Bidding Only" },
                new SelectListItem { Value = "Firm Inquiry", Text = "Firm Inquiry" },
                new SelectListItem { Value = "Buying Inquiry", Text = "Buying Inquiry" },
                new SelectListItem { Value = "Job Order", Text = "Job Order" }
                };

        private List<SelectListItem> ActivityType = new List<SelectListItem> {
                new SelectListItem { Value = "Meeting", Text = "Meeting" },
                new SelectListItem { Value = "Quotation", Text = "Quotation" },
                new SelectListItem { Value = "Sales", Text = "Sales" }
                };

        // GET: Activities
        public ActionResult Index(string sdate, string edate)
        {
            var user = HttpContext.User.Identity.Name;
            var isAdmin = false;
            var date1 = new DateTime();
            var date2 = new DateTime();

            //handle date
            if (sdate != null || edate != null)
            {
                date1 = DateTime.Parse(sdate);
                date2 = DateTime.Parse(edate);
            }
            else
            {
                date1 = dt.GetCurrentDate().AddMonths(-1);
                date2 = dt.GetCurrentDate();
            }
            
            //handle user roles
            if (User.IsInRole("Admin"))
            {
                isAdmin = true;
                ViewBag.IsAdmin = isAdmin;

                //get activities of all users on Supplier
                ViewBag.SupplierActivities = ac.GetSupplierActivitiesAdmin(date1, date2);

                //get activities of all users on Companies
                var companyActivity = ac.GetCompanyActivitiesAdmin(date1,date2);
                return View(companyActivity);
            }
            else
            {
                ViewBag.IsAdmin = isAdmin;

                //get activities of the user on Supplier
                ViewBag.SupplierActivities = ac.GetSupplierActivitiesUser(user, date1, date2);

                //get activities of the user on Companies
                var companyActivity = ac.GetCompanyActivitiesUser(user, date1, date2);
                return View(companyActivity);
            }
        }

        public ViewResult Performance(string sdate, string edate)
        {
            DateTime startDate = new DateTime();
            DateTime endDate = new DateTime();
            //handle date when null
            if (sdate != null && edate != null)
            {
                startDate = DateTime.Parse(sdate);
                endDate = DateTime.Parse(edate);
            }
            else
            {
                startDate = dt.GetCurrentDate().AddMonths(-1);
                endDate = dt.GetCurrentDate();
            }

            var userReport = ac.GetUserPerformance(startDate, endDate);
            return View(userReport);
        }

        #region Company Edit
        // GET: CustEntActivities/Edit/5
        public ActionResult CompanyActsEdit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustEntActivity custEntActivity = db.CustEntActivities.Find(id);
            if (custEntActivity == null)
            {
                return HttpNotFound();
            }
            ViewBag.Assigned = new SelectList(dbc.getUsers_wdException(), "UserName", "UserName", custEntActivity.Assigned);
            ViewBag.CustEntMainId = new SelectList(db.CustEntMains, "Id", "Name", custEntActivity.CustEntMainId);
            ViewBag.Status = new SelectList(ActivityStatus, "value", "text", custEntActivity.Status);
            ViewBag.Type = new SelectList(ActivityType, "value", "text", custEntActivity.Type);
            ViewBag.Id = custEntActivity.CustEntMainId;
            return View(custEntActivity);
        }

        // POST: CustEntActivities/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CompanyActsEdit([Bind(Include = "Id,Date,Assigned,ProjectName,SalesCode,Amount,Status,Remarks,CustEntMainId,Type")] CustEntActivity custEntActivity)
        {
            if (ModelState.IsValid)
            {
                db.Entry(custEntActivity).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Assigned = new SelectList(dbc.getUsers_wdException(), "UserName", "UserName", custEntActivity.Assigned);
            ViewBag.CustEntMainId = new SelectList(db.CustEntMains, "Id", "Name", custEntActivity.CustEntMainId);
            ViewBag.Status = new SelectList(ActivityStatus, "value", "text", custEntActivity.Status);
            ViewBag.Type = new SelectList(ActivityType, "value", "text", custEntActivity.Type);
            ViewBag.Id = custEntActivity.CustEntMainId;
            return RedirectToAction("Index", "CompanyActsEdit", new { id = custEntActivity.Id});
        }


        // GET: CustEntActivities/Delete/5
        public ActionResult CompanyActsDelete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustEntActivity custEntActivity = db.CustEntActivities.Find(id);
            if (custEntActivity == null)
            {
                return HttpNotFound();
            }

            var companyId = custEntActivity.CustEntMainId;

            ViewBag.Id = companyId;

            return View(custEntActivity);
        }

        // POST: CustEntActivities/Delete/5
        [HttpPost, ActionName("CompanyActsDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult CompanyActsDeleteConfirmed(int id)
        {
            CustEntActivity custEntActivity = db.CustEntActivities.Find(id);
            var companyId = custEntActivity.CustEntMainId;
            db.CustEntActivities.Remove(custEntActivity);
            db.SaveChanges();

            return RedirectToAction("Index");
        }
        #endregion

        #region Supplier Activities

        // GET: SupplierActivities/Edit/5
        public ActionResult SupplierActsEdit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SupplierActivity supplierActivity = db.SupplierActivities.Find(id);
            if (supplierActivity == null)
            {
                return HttpNotFound();
            }
            ViewBag.Assigned = new SelectList(dbc.getUsers_wdException(), "UserName", "UserName", supplierActivity.Assigned);
            ViewBag.SupplierId = new SelectList(db.Suppliers, "Id", "Name", supplierActivity.SupplierId);
            ViewBag.Type = new SelectList(ActivityType, "value", "text", supplierActivity.Type);
            ViewBag.Id = supplierActivity.SupplierId;
            return View(supplierActivity);
        }

        // POST: SupplierActivities/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SupplierActsEdit([Bind(Include = "Id,Code,DtActivity,Assigned,Remarks,SupplierId,Amount")] SupplierActivity supplierActivity)
        {
            if (ModelState.IsValid)
            {
                db.Entry(supplierActivity).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Assigned = new SelectList(dbc.getUsers_wdException(), "UserName", "UserName", supplierActivity.Assigned);
            ViewBag.SupplierId = new SelectList(db.Suppliers, "Id", "Name", supplierActivity.SupplierId);
            ViewBag.Type = new SelectList(ActivityType, "value", "text", supplierActivity.Type);
            return View(supplierActivity);
        }

        // GET: SupplierActivities/Delete/5
        public ActionResult SupplierActsDelete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SupplierActivity supplierActivity = db.SupplierActivities.Find(id);
            if (supplierActivity == null)
            {
                return HttpNotFound();
            }
            ViewBag.Id = supplierActivity.SupplierId;
            return View(supplierActivity);
        }

        // POST: SupplierActivities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult SupplierActsDeleteConfirmed(int id)
        {
            SupplierActivity supplierActivity = db.SupplierActivities.Find(id);
            db.SupplierActivities.Remove(supplierActivity);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        #endregion
    }
}