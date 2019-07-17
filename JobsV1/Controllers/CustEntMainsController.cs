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
    public class CustEntMainsController : Controller
    {
        private JobDBContainer db = new JobDBContainer();

        public ActionResult Index()
        {
            return View(db.CustEntMains.ToList());
        }

        // GET: CustEntMains/Details/5
        public ActionResult Details(int? id, int? top, string sdate, string edate, string status)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustEntMain custEntMain = db.CustEntMains.Find(id);
            if (custEntMain == null)
            {
                return HttpNotFound();
            }

            if (top == null)
            {
                top = 30;
            } 
            
            ViewBag.CompanyJobs = getJobList(id,top,sdate,edate,status);

            return View(custEntMain);
        }

        public List<CompanyJobsList> getJobList(int? id, int? top, string sdate, string edate, string status) {

            int topFilter = (int)top;
            //PartialView for Details of the Customer
            List<CustomerJobDetails> jobList = new List<CustomerJobDetails>();

            DateTime StartDate = DateTime.Today;
            DateTime EndDate = DateTime.Today;

            //handle empty dates
            if (sdate != null && edate != null)
            {
                StartDate = DateTime.Parse(sdate).Date;
                EndDate = DateTime.Parse(edate).Date;
            }

            //get company
            var company = db.CustEntities.Where(c => c.CustEntMainId == id).Select(c => c.CustomerId);

            //get customers with companies like this
            var customers = db.Customers.Where(c => company.Contains(c.Id)).Select(c => c.Id);

            //get company 
            var jobRecord = db.JobMains.Where(j => customers.Contains(j.CustomerId)).OrderByDescending(j => j.JobDate).ToList();
            
            //handle empty status
            if (status == null || status == "" || status == "ALL")
            {
                jobRecord = jobRecord.Where(j => j.JobDate.Date.CompareTo(StartDate) >= 0 && j.JobDate.Date.CompareTo(EndDate) <= 0).ToList();
            }
            else
            {
                jobRecord = jobRecord.Where(j => j.JobDate.Date.CompareTo(StartDate) >= 0 && j.JobDate.Date.CompareTo(EndDate) <= 0 && j.JobStatus.Status == status).ToList();
            }
            
            var filteredRecords =  jobRecord.Take(topFilter).ToList();
            List<CompanyJobsList> companyJobs = new List<CompanyJobsList>();

            foreach (var items in filteredRecords)
            {
                companyJobs.Add(new CompanyJobsList {
                    Date = items.JobDate,
                    Description = items.Description,
                    Customer = items.Customer.Name,
                    Status = items.JobStatus.Status,
                    Id = items.Id,
                    Amount = getJobTotal(items.Id)
                });
            }

            return companyJobs;
        }

        private decimal getJobTotal(int jobId)
        {
            decimal totalAmt = 0;
            var jobsvc = db.JobServices.Where(s => s.JobMainId == jobId).ToList();

            foreach (var item in jobsvc)
            {
                totalAmt += item.ActualAmt != null ? (decimal)item.ActualAmt : 0;
            }

            return totalAmt;
        }

        // GET: CustEntMains/Create
        public ActionResult Create()
        {
            CustEntMain main = new CustEntMain();
            main.iconPath = "Images/Customers/Company/organization-40.png"; //default logo 
            return View(main);
        }

        // POST: CustEntMains/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Address,Contact1,Contact2,iconPath")] CustEntMain custEntMain, int? id)
        {
            if (ModelState.IsValid)
            {
                db.CustEntMains.Add(custEntMain);
                db.SaveChanges();

                if (id != null)
                {
                    //save new company to customer
                    CustEntity company = new CustEntity();
                    company.CustEntMainId = custEntMain.Id;
                    company.CustomerId = (int)id;
                    db.CustEntities.Add(company);
                    db.SaveChanges();
                    return RedirectToAction("Index", "CustEntMains", null);
                }
                 return RedirectToAction("Index", "CustEntMains", null);
            }

            return View(custEntMain);
        }

        // GET: CustEntMains/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustEntMain custEntMain = db.CustEntMains.Find(id);
            if (custEntMain == null)
            {
                return HttpNotFound();
            }
            return View(custEntMain);
        }

        // POST: CustEntMains/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Address,Contact1,Contact2,iconPath")] CustEntMain custEntMain)
        {
            if (ModelState.IsValid)
            {
                db.Entry(custEntMain).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(custEntMain);
        }

        // GET: CustEntMains/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustEntMain custEntMain = db.CustEntMains.Find(id);
            if (custEntMain == null)
            {
                return HttpNotFound();
            }
            return View(custEntMain);
        }

        // POST: CustEntMains/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CustEntMain custEntMain = db.CustEntMains.Find(id);
            db.CustEntMains.Remove(custEntMain);
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
    }
}
