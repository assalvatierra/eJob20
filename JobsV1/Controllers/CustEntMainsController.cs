using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using JobsV1.Models;
using JobsV1.Models.Class;

namespace JobsV1.Controllers
{
    public class CustEntMainsController : Controller
    {
        private JobDBContainer db = new JobDBContainer();
        private SalesLeadClass slc = new SalesLeadClass(); 
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

            ViewBag.SalesLeads = slc.getCompanyLeads((int)id);

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

        #region Delete 

        public string EditAddress( int id, string line1, string line2, string line3, string line4, string line5,  bool isPrimary , bool isBilling)
        {
            CustEntAddress editAddress = db.CustEntAddresses.Find(id);
            editAddress.Line1 = line1;
            editAddress.Line2 = line2;
            editAddress.Line3 = line3;
            editAddress.Line4 = line4;
            editAddress.Line5 = line5; 

            db.Entry(editAddress).State = EntityState.Modified;
            db.SaveChanges();
            
            return "200";
        }


        // GET: CustEntAddresses/Delete/5
        public string DeleteAddress(int? id)
        {
            if (id == null)
            {
                return "500";
            }

            CustEntAddress custEntAddress = db.CustEntAddresses.Find(id);
            if (custEntAddress == null)
            {
                return "500";
            }

            db.CustEntAddresses.Remove(custEntAddress);
            db.SaveChanges();

            return "200";
        }

        #endregion

        #region Clauses 

        // GET: CustEntMains/Create
        public ActionResult CreateClause(int companyId)
        {
            DateClass today = new DateClass();

            CustEntClauses clause = new CustEntClauses();
            ViewBag.CustEntMainId = new SelectList(db.CustEntMains, "Id", "Name", companyId);
            clause.CustEntMainId = companyId;
            clause.DtEncoded = today.GetCurrentDateTime();
            clause.ValidStart = today.GetCurrentDateTime();
            clause.ValidEnd =  today.GetCurrentDateTime().AddDays(1);
            clause.Desc1 = "";
            clause.Desc2 = "";
            clause.Desc3 = "";
            clause.EncodedBy = HttpContext.User.Identity.Name;
            return View(clause);
        }

        // POST: CustEntMains/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateClause([Bind(Include = "Id,CustEntMainId,Title,ValidStart,ValidEnd,Desc1,Desc2,Desc3,DtEncoded,EncodedBy")] CustEntClauses custEntClauses)
        {
            if (ModelState.IsValid)
            {
                db.CustEntClauses.Add(custEntClauses);
                db.SaveChanges();

                return RedirectToAction("Details", "CustEntMains", new { id = custEntClauses.CustEntMainId });
            }
            return View(custEntClauses);
        }

        public string EditClause(int id, string title, string startdate, string enddate, string desc1, string desc2, string desc3)
        {
            try
            {

                CustEntClauses editClause = db.CustEntClauses.Find(id);
                editClause.Title = title;
                editClause.ValidStart = DateTime.Parse( startdate);
                editClause.ValidEnd = DateTime.Parse( enddate);
                editClause.Desc1 = desc1;
                editClause.Desc2 = desc2;
                editClause.Desc3 = desc3;

                db.Entry(editClause).State = EntityState.Modified;
                db.SaveChanges();

                return "200";
            }
            catch (Exception ex)
            {
                return "500";
            }
        }

        // GET: CustEntAddresses/Delete/5
        public string DeleteClause(int? id)
        {
            if (id == null)
            {
                return "500"; //error
            }

            CustEntClauses custEntClause = db.CustEntClauses.Find(id);
            if (custEntClause == null)
            {
                return "500"; //error
            }

            db.CustEntClauses.Remove(custEntClause);
            db.SaveChanges();

            return "200"; // ok
        }

        #endregion
    }
}
