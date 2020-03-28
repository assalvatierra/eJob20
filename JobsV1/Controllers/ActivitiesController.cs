using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JobsV1.Models.Class;
using JobsV1.Models;
using System.Data.Entity;
using System.Net;
using Newtonsoft.Json;

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

        #region Performance 
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

            var userReport = ac.GetUserPerformanceReport(startDate, endDate);
            userReport = AssignUserRoles(userReport);
            return View(userReport);
        }

        public List<cUserPerformance> AssignUserRoles(List<cUserPerformance> UserList)
        {
            foreach (var user in UserList)
            {
                user.Role = ac.GetUserRole(user.UserName);
            }

            return UserList;
        }

        #endregion

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
            ViewBag.Status = new SelectList(db.CustEntActStatus, "Status", "Status", custEntActivity.Status);
            ViewBag.Type = new SelectList(db.CustEntActTypes, "Type", "Type", custEntActivity.Type);
            ViewBag.ActivityType = new SelectList(db.CustEntActivityTypes, "Type", "Type", custEntActivity.ActivityType);
            ViewBag.Id = custEntActivity.CustEntMainId;
            return View(custEntActivity);
        }

        // POST: CustEntActivities/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CompanyActsEdit([Bind(Include = "Id,Date,Assigned,ProjectName,SalesCode,Amount,Status,Remarks,CustEntMainId,Type,ActivityType")] CustEntActivity custEntActivity)
        {
            if (ModelState.IsValid)
            {
                db.Entry(custEntActivity).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Assigned = new SelectList(dbc.getUsers_wdException(), "UserName", "UserName", custEntActivity.Assigned);
            ViewBag.CustEntMainId = new SelectList(db.CustEntMains, "Id", "Name", custEntActivity.CustEntMainId);
            ViewBag.Status = new SelectList(db.CustEntActStatus, "Status", "Status", custEntActivity.Status);
            ViewBag.Type = new SelectList(db.CustEntActTypes, "Type", "Type", custEntActivity.Type);
            ViewBag.ActivityType = new SelectList(db.CustEntActivityTypes, "Type", "Type", custEntActivity.ActivityType);
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

        #region User Activitiy

        //Get User Activities within a specific date range
        public ActionResult UserActivities(string user, string sDate, string eDate)
        {
            if (!String.IsNullOrEmpty(user))
            {
                var custAct = ac.GetUserActivities(user, sDate, eDate);

                ViewBag.User = user;
                ViewBag.IsAdmin = User.IsInRole("Admin");
                ViewBag.PerfSummary = ac.GetUserPerformance(custAct, user);
                ViewBag.SalesSummary = ac.GetUserSales(custAct, user);

                //For Create Activity View for the user
                Partial_ActivityCreate(user);

                return View(custAct);
            }

            return RedirectToAction("Index");
        }

        // GET : User Activity Type report from date range
        [HttpGet]
        public string GetUserActivitySummary(string user, string sDate, string eDate)
        {
            if (!String.IsNullOrEmpty(user))
            {
                List<cUserActivity> tempAct = new List<cUserActivity>();
                var custAct = ac.GetUserActivities(user, sDate, eDate);
                foreach(var act in custAct)
                {
                    act.Assigned = act.UserRemoveEmail(act.Assigned);
                    tempAct.Add(act);
                }

                //convert list to json object
                return JsonConvert.SerializeObject(tempAct, Formatting.Indented);
            }

            return "Error";
        }

        // GET : User Activity Type report from date range
        [HttpGet]
        public string GetUserTypeSummary(string user, string sDate, string eDate)
        {
            if (!String.IsNullOrEmpty(user))
            {

                var custAct = ac.GetUserActivities(user, sDate, eDate);
                var userPerf = ac.GetUserPerformance(custAct, user);

                //convert list to json object
                return JsonConvert.SerializeObject(userPerf, Formatting.Indented);
            }

            return "Error";
        }

        // GET : User sales report from date range
        [HttpGet]
        public string GetUserSalesSummary(string user, string sDate, string eDate)
        {
            if (!String.IsNullOrEmpty(user))
            {
                var custAct = ac.GetUserActivities(user, sDate, eDate);
                var userPerf = ac.GetUserSales(custAct, user);

                //convert list to json object
                return JsonConvert.SerializeObject(userPerf, Formatting.Indented);
            }

            return "Error";
        }

        public void Partial_ActivityCreate(string user)
        {
            ViewBag.Assigned = user;
            ViewBag.CustEntMainId = new SelectList(db.CustEntMains.OrderBy(c=>c.Name), "Id", "Name");
            ViewBag.Status = new SelectList(db.CustEntActStatus, "Status", "Status");
            ViewBag.Type = new SelectList(db.CustEntActTypes, "Type", "Type");
            ViewBag.ActivityType = new SelectList(db.CustEntActivityTypes, "Type", "Type");

            CustEntActivity activity = new CustEntActivity();
            activity.Amount = 0;
            activity.Date = dt.GetCurrentDateTime();
            activity.Assigned = user;
            ViewBag.CustEntActivity = activity;
        }

        //CREATE : Activities/CreateActivity
        [HttpPost]
        public string CreateActivity( string actDate, string Assigned, string ProjectName, string SalesCode, string Amount, string Status, string Remarks, string CustEntMainId, string Type, string ActivityType )
        {
            try
            {
                //create activity
                CustEntActivity act = new CustEntActivity();
                act.Date = DateTime.Parse(actDate);
                act.Assigned = Assigned;
                act.ProjectName = ProjectName;
                act.SalesCode = SalesCode;
                act.Amount = Decimal.Parse(Amount);
                act.Status = Status;
                act.Remarks = Remarks;
                act.CustEntMainId = int.Parse(CustEntMainId);
                act.Type = Type;
                act.ActivityType = ActivityType;

                db.CustEntActivities.Add(act);
                db.SaveChanges();

                return "New Activity Added";
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        //GET : Activities/GetActivityRecord
        [HttpGet]
        public string GetActivityRecord(int id)
        {
            try
            {
                //create activity
                CustEntActivity act = db.CustEntActivities.Find(id);
                if (act != null)
                {
                    cUserActivity userAct = new cUserActivity();
                    userAct.Id = act.Id;
                    userAct.Date = act.Date;
                    userAct.ProjectName = act.ProjectName;
                    userAct.SalesCode = act.SalesCode;
                    userAct.Amount = act.Amount;
                    userAct.Status = act.Status;
                    userAct.Remarks = act.Remarks;
                    userAct.Company = act.CustEntMain.Name;
                    userAct.CustEntMainId = act.CustEntMainId;
                    userAct.Assigned = act.Assigned;
                    userAct.Type = act.Type;
                    userAct.ActivityType = act.ActivityType;

                    return JsonConvert.SerializeObject(userAct, Formatting.Indented);
                }
                return "ERROR";
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        //EDIT : Activities/EditActivity
        [HttpPost]
        public string EditActivity(int id, string actDate, string Assigned, string ProjectName, string SalesCode, string Amount, string Status, string Remarks, string CustEntMainId, string Type, string ActivityType)
        {
            try
            {
                //create activity
                CustEntActivity act = db.CustEntActivities.Find(id);
                act.Date = DateTime.Parse(actDate);
                act.Assigned = Assigned;
                act.ProjectName = ProjectName;
                act.SalesCode = SalesCode;
                act.Amount = Decimal.Parse(Amount);
                act.Status = Status;
                act.Remarks = Remarks;
                act.CustEntMainId = int.Parse(CustEntMainId);
                act.Type = Type;
                act.ActivityType = ActivityType;

                db.Entry(act).State = EntityState.Modified;
                db.SaveChanges();

                return "Edit Success";
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        //DELETE : Activities/DeleteActivityRecord
        [HttpPost]
        public string DeleteActivityRecord(int id)
        {
            try
            {
                //create activity
                CustEntActivity act = db.CustEntActivities.Find(id);
                if (act != null)
                {

                    db.CustEntActivities.Remove(act);
                    db.SaveChanges();
                    return "DELETE SUCCESS ";
                }
                return "DELETE ERROR ";
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }



        #endregion
    }

}