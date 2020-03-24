using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using JobsV1.Models;

namespace JobsV1.Controllers
{
    public class CustEntActivitiesController : Controller
    {
        private JobDBContainer db   = new JobDBContainer();
        private DBClasses dbclasses = new DBClasses();
        private DateClass date      = new DateClass();
        private CompanyClass comdb  = new CompanyClass();

        private List<SelectListItem> ActivityStatus = new List<SelectListItem> {
                new SelectListItem { Value = "Open", Text = "Open" },
                new SelectListItem { Value = "For Client Comment", Text = "For Client Comment" },
                new SelectListItem { Value = "Awarded", Text = "Awarded" },
                new SelectListItem { Value = "Close", Text = "Close" }
                };

        private List<SelectListItem> ActivityType = new List<SelectListItem> {
                new SelectListItem { Value = "Others", Text = "Others" },
                new SelectListItem { Value = "Indicated Price", Text = "Indicated Price" },
                new SelectListItem { Value = "Bidding Only", Text = "Bidding Only" },
                new SelectListItem { Value = "Firm Inquiry", Text = "Firm Inquiry" },
                new SelectListItem { Value = "Buying Inquiry", Text = "Buying Inquiry" },
                new SelectListItem { Value = "Others", Text = "Others" }
                };

        // GET: CustEntActivities
        public ActionResult Index(int? id)
        {
            if (id != null)
            {
                var custEntActivities = db.CustEntActivities.Where(c=>c.CustEntMainId == id).Include(c => c.CustEntMain);
                ViewBag.companyName = db.CustEntMains.Find(id).Name;
                ViewBag.Id = id;

                foreach (var act in custEntActivities)
                {
                    act.Assigned = comdb.removeSpecialChar(act.Assigned);
                }

                return View(custEntActivities.OrderByDescending(c => c.Date).ToList());
            }

            var custEntActivitiesList = db.CustEntActivities.Include(c => c.CustEntMain).OrderByDescending(c=>c.Date).ToList();

            return View(custEntActivitiesList);
        }

        // GET: CustEntActivities/Details/5
        public ActionResult Details(int? id)
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
            return View(custEntActivity);
        }

        // GET: CustEntActivities/Create
        public ActionResult Create(int? id)
        {
            ViewBag.Assigned = new SelectList(dbclasses.getUsers_wdException(), "UserName", "UserName");
            ViewBag.CustEntMainId = new SelectList(db.CustEntMains, "Id", "Name", id);
            ViewBag.Status = new SelectList(db.CustEntActStatus, "Status", "Status");
            ViewBag.Type = new SelectList(db.CustEntActTypes, "Type", "Type");
            ViewBag.ActivityType = new SelectList(db.CustEntActivityTypes, "Type", "Type");

            CustEntActivity activity = new CustEntActivity();
            activity.Amount = 0;
            activity.Date = date.GetCurrentDateTime();
            ViewBag.Id = id;

            return View(activity);
        }

        // POST: CustEntActivities/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Date,Assigned,ProjectName,SalesCode,Amount,Status,Remarks,CustEntMainId,Type,ActivityType")] CustEntActivity custEntActivity)
        {
            if (ModelState.IsValid)
            {
                custEntActivity.Amount = Decimal.Parse(custEntActivity.Amount.ToString());
                db.CustEntActivities.Add(custEntActivity);
                db.SaveChanges();
                return RedirectToAction("Index", new { id = custEntActivity.CustEntMainId });
            }

            ViewBag.Assigned = new SelectList(dbclasses.getUsers_wdException(), "UserName", "UserName", custEntActivity.Assigned);
            ViewBag.CustEntMainId = new SelectList(db.CustEntMains, "Id", "Name", custEntActivity.CustEntMainId);
            ViewBag.Status = new SelectList(db.CustEntActStatus, "Status", "Status", custEntActivity.Status);
            ViewBag.Type = new SelectList(db.CustEntActTypes, "Type", "Type", custEntActivity.Type);
            ViewBag.ActivityType = new SelectList(db.CustEntActivityTypes, "Type", "Type", custEntActivity.ActivityType);
            return View(custEntActivity);
        }

        // GET: CustEntActivities/Edit/5
        public ActionResult Edit(int? id)
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
            ViewBag.Assigned = new SelectList(dbclasses.getUsers_wdException(), "UserName", "UserName", custEntActivity.Assigned);
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
        public ActionResult Edit([Bind(Include = "Id,Date,Assigned,ProjectName,SalesCode,Amount,Status,Remarks,CustEntMainId,Type,ActivityType")] CustEntActivity custEntActivity)
        {
            if (ModelState.IsValid)
            {
                db.Entry(custEntActivity).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { id =custEntActivity.CustEntMainId });
            }
            ViewBag.Assigned = new SelectList(dbclasses.getUsers_wdException(), "UserName", "UserName", custEntActivity.Assigned);
            ViewBag.CustEntMainId = new SelectList(db.CustEntMains, "Id", "Name", custEntActivity.CustEntMainId);
            ViewBag.Status = new SelectList(db.CustEntActStatus, "Status", "Status", custEntActivity.Status);
            ViewBag.Type = new SelectList(db.CustEntActTypes, "Type", "Type", custEntActivity.Type);
            ViewBag.ActivityType = new SelectList(db.CustEntActivityTypes, "Type", "Type", custEntActivity.ActivityType);
            ViewBag.Id = custEntActivity.CustEntMainId;
            return View(custEntActivity);
        }

        // GET: CustEntActivities/Delete/5
        public ActionResult Delete(int? id)
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
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CustEntActivity custEntActivity = db.CustEntActivities.Find(id);
            var companyId = custEntActivity.CustEntMainId;
            db.CustEntActivities.Remove(custEntActivity);
            db.SaveChanges();

            return RedirectToAction("Index", new { id = companyId });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


    }
}
