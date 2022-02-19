using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using JobsV1.Areas.Personel.Models;
using JobsV1.Areas.Personel.Services;

namespace JobsV1.Areas.Personel.Controllers
{
    public class crLogCashSalariesController : Controller
    {
        private CarRentalLogDBContainer db = new CarRentalLogDBContainer();
        private DateClass dt = new DateClass();
        private CrDataLayer dl = new CrDataLayer();

        // GET: Personel/crLogCashSalaries
        public ActionResult Index(int? filter)
        {
            var crLogCashSalaries = new List<crLogCashSalary>();

            var today = dt.GetCurrentDate().ToString("M/dd/yyyy");
            if (filter == 1)
            {
                crLogCashSalaries = db.crLogCashSalaries.Include(c => c.crLogDriver)
                    .Where(c => c.Date != today)
                    .ToList();
            }
            else
            {
                crLogCashSalaries = db.crLogCashSalaries.Include(c => c.crLogDriver)
                    .Where(c => c.Date == today)
                    .ToList();
            }

            return View(crLogCashSalaries);
        }

        // GET: Personel/crLogCashSalaries/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            crLogCashSalary crLogCashSalary = db.crLogCashSalaries.Find(id);
            if (crLogCashSalary == null)
            {
                return HttpNotFound();
            }

            crLogCashSalaryViewModel salary = GetCashSalaryRequests(crLogCashSalary);

            ViewBag.CABalance = GetCABalance(crLogCashSalary.crLogDriverId);

            return View(salary);
        }

        // GET: Personel/crLogCashSalaries/Details/5
        public ActionResult PrintSalary(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            crLogCashSalary crLogCashSalary = db.crLogCashSalaries.Find(id);
            if (crLogCashSalary == null)
            {
                return HttpNotFound();
            }

            crLogCashSalaryViewModel salary = GetCashSalaryRequests(crLogCashSalary);

            ViewBag.CABalance = GetCABalance(crLogCashSalary.crLogDriverId);

            return View(salary);
        }

        private crLogCashSalaryViewModel GetCashSalaryRequests(crLogCashSalary crLogCashSalary)
        {
            crLogCashSalaryViewModel salary = new crLogCashSalaryViewModel();
            salary.Id = crLogCashSalary.Id;
            salary.Driver = crLogCashSalary.crLogDriver.Name;
            salary.Date = dt.GetCurrentDate().ToString("MMM dd yyyy");
            salary.ExcludeOT = crLogCashSalary.ExcludeOT;
            salary.Amount = 0;

            salary.SalaryTrips = crLogCashSalary.crLogCashGroups
                .Where(c => c.crLogCashRelease.crLogCashType.Description == "Salary" 
                && c.crLogCashRelease.crLogClosingId != null)
                .Select(c => c.crLogCashRelease).ToList();

            salary.CA = crLogCashSalary.crLogCashGroups
                .Where(c => c.crLogCashRelease.crLogCashType.Description == "CA")
                .Select(c => c.crLogCashRelease).ToList();


            salary.CAPayment = crLogCashSalary.crLogCashGroups
                .Where(c => c.crLogCashRelease.crLogCashType.Description == "Payment")
                .Select(c => c.crLogCashRelease).ToList();


            salary.Contributions = crLogCashSalary.crLogCashGroups
                .Where(c => c.crLogCashRelease.crLogCashType.Description == "Contributions")
                .Select(c => c.crLogCashRelease).ToList();

            salary.Others = crLogCashSalary.crLogCashGroups
                .Where(c => c.crLogCashRelease.crLogCashType.Description == "Others")
                .Select(c => c.crLogCashRelease).ToList();

            salary.OtherSalary = crLogCashSalary.crLogCashGroups
                .Where(c => c.crLogCashRelease.crLogCashType.Description == "Salary" 
                && c.crLogCashRelease.crLogClosingId == null)
                .Select(c => c.crLogCashRelease).ToList();

            return salary;

        }

        // GET: Personel/crLogCashSalaries/Create
        public ActionResult Create(int id)
        {
            ViewBag.Date = dt.GetCurrentDate().ToString("M/dd/yyyy");
            ViewBag.crLogDriverId = new SelectList(db.crLogDrivers, "Id", "Name", id);
            return View();
        }

        // POST: Personel/crLogCashSalaries/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Date,crLogDriverId,ExlcudeOT")] crLogCashSalary crLogCashSalary)
        {
            if (ModelState.IsValid)
            {
                db.crLogCashSalaries.Add(crLogCashSalary);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.crLogDriverId = new SelectList(db.crLogDrivers, "Id", "Name", crLogCashSalary.crLogDriverId);
            return View(crLogCashSalary);
        }

        // GET: Personel/crLogCashSalaries/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            crLogCashSalary crLogCashSalary = db.crLogCashSalaries.Find(id);
            if (crLogCashSalary == null)
            {
                return HttpNotFound();
            }
            ViewBag.crLogDriverId = new SelectList(db.crLogDrivers, "Id", "Name", crLogCashSalary.crLogDriverId);
            return View(crLogCashSalary);
        }

        // POST: Personel/crLogCashSalaries/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Date,crLogDriverId,ExlcudeOT")] crLogCashSalary crLogCashSalary)
        {
            if (ModelState.IsValid)
            {
                db.Entry(crLogCashSalary).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.crLogDriverId = new SelectList(db.crLogDrivers, "Id", "Name", crLogCashSalary.crLogDriverId);
            return View(crLogCashSalary);
        }

        // GET: Personel/crLogCashSalaries/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            crLogCashSalary crLogCashSalary = db.crLogCashSalaries.Find(id);
            if (crLogCashSalary == null)
            {
                return HttpNotFound();
            }
            return View(crLogCashSalary);
        }

        // POST: Personel/crLogCashSalaries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            crLogCashSalary crLogCashSalary = db.crLogCashSalaries.Find(id);
            db.crLogCashGroups.RemoveRange(crLogCashSalary.crLogCashGroups);
            db.crLogCashSalaries.Remove(crLogCashSalary);
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

        public ActionResult AddSalary(int id)
        {
            ViewBag.SalaryId = id;

            var cashRequests = db.crLogCashReleases
                .Where(c => c.crLogCashStatus.OrderByDescending(d=>d.crCashReqStatusId)
                .FirstOrDefault().crCashReqStatusId == 1).ToList();

            return View(cashRequests);
        }

        // GET: Personel/CarRentalCashRelease/Create
        public ActionResult AddCashTransaction(int id, int? cashtype)
        {
            try
            {

                if (cashtype == null)
                {
                    cashtype = 1;
                }

                crLogCashSalary cashSalary = db.crLogCashSalaries.Find(id);

                crLogCashRelease crtrx = new crLogCashRelease();
                crtrx.DtRelease = dt.GetCurrentDate();

                ViewBag.SalaryId = id;
                ViewBag.crLogDriverId = new SelectList(db.crLogDrivers.OrderBy(c=>c.OrderNo).ToList(), "Id", "Name", cashSalary.crLogDriverId);
                ViewBag.crLogClosingId = new SelectList(db.crLogClosings, "Id", "Id");
                ViewBag.crLogCashTypeId = new SelectList(db.crLogCashTypes, "Id", "Description", cashtype);
                return View(crtrx);
            }
            catch
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        // POST: Personel/CarRentalCashRelease/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddCashTransaction([Bind(Include = "Id,DtRelease,Amount,Remarks,crLogDriverId,crLogClosingId,crLogClosingId,crLogCashTypeId")] crLogCashRelease crLogCashRelease, int salaryId)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.crLogCashReleases.Add(crLogCashRelease);
                    db.SaveChanges();

                    //add status
                    AddLogStatus(crLogCashRelease.Id, 1);

                    //add to cash group 
                    AddCashGroup(salaryId, crLogCashRelease.Id);

                    return RedirectToAction("Details", new { id = salaryId });
                }

                ViewBag.crLogDriverId = new SelectList(db.crLogDrivers.OrderBy(c => c.OrderNo).ToList(), "Id", "Name");
                ViewBag.crLogClosingId = new SelectList(db.crLogClosings, "Id", "Id", crLogCashRelease.crLogClosingId);
                ViewBag.crLogCashTypeId = new SelectList(db.crLogCashTypes, "Id", "Description", crLogCashRelease.crLogCashTypeId);
                return View(crLogCashRelease);
            }
            catch
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }




        public ActionResult AddSalarySelect(int id, int CashReleaseId)
        {
            //create and add to group
            try
            {
                crLogCashGroup cashGroup = new crLogCashGroup();
                cashGroup.crLogCashSalaryId = id;
                cashGroup.crLogCashReleaseId = CashReleaseId;

                db.crLogCashGroups.Add(cashGroup);
                db.SaveChanges();

                return RedirectToAction("Details", new { id });
            }
            catch
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        public ActionResult ContributionsList(int id)
        {
            try
            {

                List<crLogCashRelease> contrList = new List<crLogCashRelease>();

                //get salary request
                var cashSalary = db.crLogCashSalaries.Find(id);

                //get driver contributions
                var driverContr = cashSalary.crLogDriver.crLogDriverPayments.ToList();

                //Create contribution list
                foreach (var contribution in driverContr)
                {
                    contrList.Add(new crLogCashRelease()
                    {
                        Remarks = contribution.Description,
                        Amount = contribution.Amount,
                        crLogDriverId = contribution.crLogDriverId,
                        crLogCashTypeId = 4,
                        DtRelease = dt.GetCurrentDate()
                    });
                }

                ViewBag.SalaryId = id;
                return View(contrList);
            }
            catch
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        public ActionResult AddContSelect(int id, string remarks, decimal amount)
        {
            try
            {
                //find salary request by id
                var cashSalary = db.crLogCashSalaries.Find(id);

                //create cash release
                var crLogCashRelease = new crLogCashRelease()
                {
                    Remarks = remarks,
                    Amount = amount,
                    crLogDriverId = cashSalary.crLogDriverId,
                    crLogCashTypeId = 4,
                    DtRelease = dt.GetCurrentDate(),
                };

                db.crLogCashReleases.Add(crLogCashRelease);
                db.SaveChanges();

                //add status
                AddLogStatus(crLogCashRelease.Id, 1);

                //add to cash group 
                AddCashGroup(id, crLogCashRelease.Id);
                return RedirectToAction("Details", new { id });
            }
            catch
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        // GET: Personel/CarRentalCashRelease/Edit/5
        public ActionResult EditCashTransaction(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            crLogCashRelease crLogCashRelease = db.crLogCashReleases.Find(id);
            if (crLogCashRelease == null)
            {
                return HttpNotFound();
            }
            ViewBag.crLogDriverId = new SelectList(dl.GetDrivers(), "Id", "Name", crLogCashRelease.crLogDriverId);
            ViewBag.crLogClosingId = new SelectList(db.crLogClosings, "Id", "Id", crLogCashRelease.crLogClosingId);
            ViewBag.crLogCashTypeId = new SelectList(db.crLogCashTypes, "Id", "Description", crLogCashRelease.crLogCashTypeId);
            return View(crLogCashRelease);
        }

        // POST: Personel/CarRentalCashRelease/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditCashTransaction([Bind(Include = "Id,DtRelease,Amount,Remarks,crLogDriverId,crLogClosingId,crLogCashTypeId")] crLogCashRelease crLogCashRelease)
        {
            if (ModelState.IsValid)
            {
                db.Entry(crLogCashRelease).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", new { crLogCashRelease.crLogCashGroups.FirstOrDefault().crLogCashSalaryId });
            }
            ViewBag.crLogDriverId = new SelectList(dl.GetDrivers(), "Id", "Name", crLogCashRelease.crLogDriverId);
            ViewBag.crLogClosingId = new SelectList(db.crLogClosings, "Id", "Id", crLogCashRelease.crLogClosingId);
            ViewBag.crLogCashTypeId = new SelectList(db.crLogCashTypes, "Id", "Description", crLogCashRelease.crLogCashTypeId);
            return View(crLogCashRelease);
        }

        public ActionResult RemoveSalaryGroup(int id)
        {
            try
            {
                //get salary group
                crLogCashGroup cashGroup = db.crLogCashGroups.Find(id);
                int salaryID = cashGroup.crLogCashSalaryId;

                //remove salary group
                db.crLogCashGroups.Remove(cashGroup);
                db.SaveChanges();

                return RedirectToAction("Details", new { id = salaryID });
            }
            catch
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        public ActionResult DeleteSalaryGroup(int id)
        {
            try
            {
                //cash group
                crLogCashGroup cashGroup = db.crLogCashGroups.Find(id);
                int salaryID = cashGroup.crLogCashSalaryId;

                //get cash transactions
                var cashtrx = cashGroup.crLogCashRelease;

                //remove cash group
                db.crLogCashGroups.Remove(cashGroup);
                db.SaveChanges();

                //remove cash transactions
                DeleteCashTrx(cashtrx);

                return RedirectToAction("Details", new { id = salaryID });
            }
            catch
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        #region Services

        private bool AddCashGroup(int id, int CashReleaseId)
        {
            //create and add to group
            try
            {
                crLogCashGroup cashGroup = new crLogCashGroup();
                cashGroup.crLogCashSalaryId = id;
                cashGroup.crLogCashReleaseId = CashReleaseId;

                //add salary group
                db.crLogCashGroups.Add(cashGroup);
                db.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        private bool DeleteCashTrx(crLogCashRelease crLogCashRelease)
        {
            try
            {
                db.crLogCashReleases.Remove(crLogCashRelease);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        private bool AddLogStatus(int? id, int statusId)
        {
            try
            {
                if (id == null)
                {
                    return false;
                }

                //create new status log
                crLogCashStatus logCashStatus = new crLogCashStatus();
                logCashStatus.crCashReqStatusId = statusId;
                logCashStatus.crLogCashReleaseId = (int)id;
                logCashStatus.dtStatus = dt.GetCurrentDateTime();

                db.crLogCashStatus.Add(logCashStatus);
                db.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        private decimal GetCABalance(int DriverId)
        {
            try
            {

                decimal CABalance = 0;

                var driverCashReleases = db.crLogCashReleases;
                //Get CA
                var cashAdvance = driverCashReleases.Where(d => d.crLogDriverId == DriverId
                                                       && d.crLogClosing == null && d.crLogCashTypeId == 2)
                                                        .OrderBy(s => s.DtRelease).ToList();
                //Get Payments
                var payments = driverCashReleases.Where(d => d.crLogDriverId == DriverId
                                                        && d.crLogClosing == null && d.crLogCashTypeId == 3)
                                                        .OrderBy(s => s.DtRelease).ToList();
                //Calculate Balance
                CABalance = cashAdvance.Sum(c => c.Amount) - payments.Sum(c => c.Amount);

                if (CABalance > 0)
                {
                    return CABalance;
                }

                //do not return negative Balance
                return 0;
            }
            catch
            {
                //when error, return 0
                return 0;
            }

        }
        #endregion

        #region APIs

        // POST: Personal/crLogCashSalaries/ApproveSalary
        // Param : id = crLogCashSalaries.Id
        [HttpPost]
        public HttpResponseMessage ApproveSalary(int id)
        {
            try
            {

                var salaryRequest = db.crLogCashSalaries.Find(id);

                var salaryGroup = salaryRequest.crLogCashGroups.ToList();

                salaryGroup.ForEach(s => {
                    //Approve
                    AddLogStatus(s.crLogCashReleaseId, 2);
                });

                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            catch
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
        }

        #endregion

        public class crLogCashSalaryViewModel
        {
            public int Id { get; set; }
            public string Date { get; set; }
            public bool ExcludeOT { get; set; }
            public string Driver { get; set; }
            public decimal Amount { get; set; }
            public List<crLogCashRelease> SalaryTrips { get; set; }
            public List<crLogCashRelease> CA { get; set; }
            public List<crLogCashRelease> CAPayment { get; set; }
            public List<crLogCashRelease> Contributions { get; set; }
            public List<crLogCashRelease> Others { get; set; }
            public List<crLogCashRelease> OtherSalary { get; set; }
        }
    }
}
