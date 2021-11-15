using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using JobsV1.Areas.Personel.Models;
using JobsV1.Models;

namespace JobsV1.Areas.Personel.Controllers
{

    public class CarRentalCashReleaseController : Controller
    {
        private CarRentalLogDBContainer db = new CarRentalLogDBContainer();
        private DateClass dt = new DateClass();
        private CrDataLayer dl = new CrDataLayer();


        private enum CASHTYPE
        {
            SALARY          = 1,
            CA              = 2,
            PAYMENTS        = 3, 
            CONTRIBUTIONS   = 4,
            OTHERS          = 5
        }

        private enum CASHREQ_STATUS
        {
           REQUEST = 1,
           APPROVED = 2,
           RELEASED = 3,
           RETURNED = 4
        }

        // GET: Personel/CarRentalCashRelease
        public ActionResult Index(int? statusId)
        {
            #region Session
            if (statusId != null)
                Session["CRCashRelease_StatusId"] = (int)statusId;
            else
            {
                if (Session["CrCashRelease_StatusId"] != null)
                    statusId = (int)Session["CRCashRelease_StatusId"];
                else
                {
                    statusId = 1;
                    Session["CRCashRelease_StatusId"] = (int)CASHREQ_STATUS.REQUEST;
                }
            }
            #endregion

            var today = dt.GetCurrentDate();
            var DateFilter = today.AddDays(-15);
            
            if (statusId == null || statusId == (int)CASHREQ_STATUS.REQUEST)
            {
                DateFilter = today.AddDays(-10);
            }
            if (statusId == null)
                statusId = (int)CASHREQ_STATUS.REQUEST;

            //get cash releases up to -7 days from today
            var crLogCashReleases = db.crLogCashReleases.Include(c => c.crLogDriver).Include(c => c.crLogClosing)
                .Where(c => DbFunctions.TruncateTime(c.DtRelease) > DateFilter);

            List<crLogCashRelease> cashReleases = new List<crLogCashRelease>();

            foreach (var log in crLogCashReleases.ToList())
            {
                var lateststatusId = getLatestStatusId(log.Id);

                if (lateststatusId == statusId)
                {
                    //add request and accecpted logs
                    if (log.DtRelease.Date <= today.Date && lateststatusId < (int)CASHREQ_STATUS.RELEASED)
                        cashReleases.Add(log);
                    //add returned logs with date today
                    if (log.DtRelease.Date == today.Date && lateststatusId == (int)CASHREQ_STATUS.RELEASED)
                        cashReleases.Add(log);
                }
            }

            ViewBag.IsAdmin = User.IsInRole("Admin");
            ViewBag.StatusId = statusId;
            ViewBag.RequestCount = GetStatusCount(1, crLogCashReleases);
            ViewBag.ApprovedCount = GetStatusCount(2, crLogCashReleases);
            ViewBag.ReleasedCount = GetStatusCount(3, crLogCashReleases);

            return View(cashReleases.OrderBy(c=>c.DtRelease).ThenBy(c=>c.crLogDriver.OrderNo).ToList());
        }


        public ActionResult SalarySummary()
        {
        
            var today = dt.GetCurrentDate();
            var DateFilter = today.AddDays(-1);

            //get cash releases up to -2 days from today
            var crLogCashReleases = db.crLogCashReleases.Include(c => c.crLogDriver)
                .Where(c => DbFunctions.TruncateTime(c.DtRelease) >= DateFilter && c.crLogCashTypeId == (int)CASHTYPE.SALARY &&
                c.crLogCashStatus.OrderByDescending(d => d.Id).FirstOrDefault().crCashReqStatusId < 3);

            List<crLogCashRelease> cashReleases = new List<crLogCashRelease>();

            foreach (var driver in crLogCashReleases.ToList().GroupBy(c=>c.crLogDriverId))
            {
                foreach (var cashtrxReq in driver.Take(1))
                {
                    cashtrxReq.Amount = DriverSalaryAmount(cashtrxReq.Id);
                    cashReleases.Add(cashtrxReq);
                }
            }

            cashReleases = cashReleases.OrderBy(c => c.DtRelease).ThenBy(c => c.crLogDriver.OrderNo).ToList();

            ViewBag.IsAdmin = User.IsInRole("Admin");
            return View(cashReleases);
        }

        // GET: Personel/crLogFuels
        public ActionResult PrevRecords()
        {
            var today = dt.GetCurrentDate();
            var crLogCashReleases = db.crLogCashReleases.Include(c => c.crLogDriver).Include(c => c.crLogClosing)
              .Where(c => DbFunctions.TruncateTime(c.DtRelease) < today).OrderByDescending(c => c.DtRelease);

            List<crLogCashRelease> cashReleases = new List<crLogCashRelease>();

            foreach (var log in crLogCashReleases.ToList())
            {
                var lateststatusId = getLatestStatusId(log.Id);

                //add returned logs with date today
                if (log.DtRelease.Date < today.Date && lateststatusId == (int)CASHREQ_STATUS.RELEASED)
                    cashReleases.Add(log);
                
            }

            ViewBag.IsAdmin = User.IsInRole("Admin");

            return View(cashReleases.ToList());
        }

        // GET: Personel/CarRentalCashRelease/Details/5
        public ActionResult Details(int? id)
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
            return View(crLogCashRelease);
        }

        // GET: Personel/CarRentalCashRelease/Create
        public ActionResult Create()
        {
            crLogCashRelease crtrx = new crLogCashRelease();
            crtrx.DtRelease = System.DateTime.Now;
            
            ViewBag.crLogDriverId = new SelectList(dl.GetDrivers(), "Id", "Name");
            ViewBag.crLogClosingId = new SelectList(db.crLogClosings, "Id", "Id");
            ViewBag.crLogCashTypeId = new SelectList(db.crLogCashTypes, "Id", "Description");
            return View(crtrx);
        }

        // POST: Personel/CarRentalCashRelease/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,DtRelease,Amount,Remarks,crLogDriverId,crLogClosingId,crLogClosingId,crLogCashTypeId")] crLogCashRelease crLogCashRelease)
        {
            if (ModelState.IsValid)
            {
                db.crLogCashReleases.Add(crLogCashRelease);
                db.SaveChanges();

                //add status
                AddLogStatus(crLogCashRelease.Id, 1);

                return RedirectToAction("Index");
            }

            ViewBag.crLogDriverId = new SelectList(dl.GetDrivers(), "Id", "Name", crLogCashRelease.crLogDriverId);
            ViewBag.crLogClosingId = new SelectList(db.crLogClosings, "Id", "Id", crLogCashRelease.crLogClosingId);
            ViewBag.crLogCashTypeId = new SelectList(db.crLogCashTypes, "Id", "Description", crLogCashRelease.crLogCashTypeId);
            return View(crLogCashRelease);
        }

        // GET: Personel/CarRentalCashRelease/Edit/5
        public ActionResult Edit(int? id)
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
        public ActionResult Edit([Bind(Include = "Id,DtRelease,Amount,Remarks,crLogDriverId,crLogClosingId,crLogCashTypeId")] crLogCashRelease crLogCashRelease)
        {
            if (ModelState.IsValid)
            {
                db.Entry(crLogCashRelease).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.crLogDriverId = new SelectList(dl.GetDrivers(), "Id", "Name", crLogCashRelease.crLogDriverId);
            ViewBag.crLogClosingId = new SelectList(db.crLogClosings, "Id", "Id", crLogCashRelease.crLogClosingId);
            ViewBag.crLogCashTypeId = new SelectList(db.crLogCashTypes, "Id", "Description", crLogCashRelease.crLogCashTypeId);
            return View(crLogCashRelease);
        }

        // GET: Personel/CarRentalCashRelease/Delete/5
        public ActionResult Delete(int? id)
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
            return View(crLogCashRelease);
        }

        // POST: Personel/CarRentalCashRelease/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (DeleteStatusRecords(id))
            {
                crLogCashRelease crLogCashRelease = db.crLogCashReleases.Find(id);

                //remove closing id on trip logs
                RemoveTripClosingId(crLogCashRelease);

                db.crLogCashReleases.Remove(crLogCashRelease);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

        }

        public bool RemoveTripClosingId(crLogCashRelease crLogCashRelease)
        {
            try
            {
                var trips = db.crLogTrips.Where(c => c.crLogClosingId == crLogCashRelease.crLogClosingId).ToList();

                foreach (var item in trips)
                {
                    item.crLogClosingId = null;
                    db.Entry(item).State = EntityState.Modified;
                }

                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
           
        }

        public ActionResult DriverSummary(int id)
        {
            return RedirectToAction("CarRentalLog", "DriverSummary", new { id = id });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        public bool DeleteStatusRecords(int crLogCashReleaseId)
        {
            try
            {
                var logCashStatusList = db.crLogCashStatus.Where(c => c.crLogCashReleaseId == crLogCashReleaseId).ToList();

                if (logCashStatusList != null)
                {
                    db.crLogCashStatus.RemoveRange(logCashStatusList);
                    db.SaveChanges();
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        // POST: Personel/CarRentalCashRelease/CreateDriverRelease
        [HttpPost]
        public bool CreateDriverRelease(DriverReleaseRequest releaseRequest)
        {
            try
            {
                var statusFlag = false;

                crLogCashRelease crtrx = new crLogCashRelease();
                crtrx.DtRelease = dt.GetCurrentDateTime();
                crtrx.crLogDriverId = releaseRequest.DriverId;
                crtrx.crLogClosingId = GenerateClosingId();
                crtrx.Amount = releaseRequest.Amount;
                crtrx.Remarks = releaseRequest.Remarks;
                crtrx.crLogCashTypeId = (int)CASHTYPE.SALARY; //salary

                db.crLogCashReleases.Add(crtrx);
                db.SaveChanges();

                foreach (var tripId in releaseRequest.TripIds)
                {
                    statusFlag = UpdateTripLogClosingId(tripId, crtrx.crLogClosingId);
                    if (statusFlag == false)
                        return false;
                }

                //add status
                AddLogStatus(crtrx.Id, 1);

                return true;

            }
            catch
            {
                return false;
            }
        }



        // POST: Personel/CarRentalCashRelease/CreateDriverRelease
        [HttpPost]
        public bool CreateDriverPayment(int DriverId, decimal Amount, string Remarks)
        {
            try
            {
                crLogCashRelease crtrx = new crLogCashRelease();
                crtrx.DtRelease = dt.GetCurrentDateTime();
                crtrx.crLogDriverId = DriverId;
                crtrx.Amount = Amount;
                crtrx.Remarks = Remarks;
                crtrx.crLogCashTypeId = (int)CASHTYPE.PAYMENTS; //payment
                

                db.crLogCashReleases.Add(crtrx);
                db.SaveChanges();

                //add status
                AddLogStatus(crtrx.Id, 3);

                return true;

            }
            catch
            {
                return false;
            }
        }




        // POST: Personel/CarRentalCashRelease/CreateDriverRelease
        [HttpPost]
        public bool CreateDriverCA(int DriverId, decimal Amount, string Remarks)
        {
            try
            {

                crLogCashRelease crtrx = new crLogCashRelease();
                crtrx.DtRelease = dt.GetCurrentDateTime();
                crtrx.crLogDriverId = DriverId;
                crtrx.Amount = Amount;
                crtrx.Remarks = Remarks;
                crtrx.crLogCashTypeId = (int)CASHTYPE.CA; //CA

                db.crLogCashReleases.Add(crtrx);
                db.SaveChanges();

                //add status
                AddLogStatus(crtrx.Id, 1);

                return true;

            }
            catch
            {
                return false;
            }
        }


        //Generate Closing Id
        private int GenerateClosingId()
        {
            try
            {
                var today = dt.GetCurrentDateTime();

                crLogClosing crLog = new crLogClosing();
                crLog.dtClose = today;

                db.crLogClosings.Add(crLog);
                db.SaveChanges();

                return crLog.Id;
            }
            catch 
            {
                return 0;
            }
        }

        //Update TripLog ClosingId
        private bool UpdateTripLogClosingId(int? id, int? closingId)
        {
            try
            {
                if (id == null || closingId == null)
                    return false;

                crLogTrip logTrip = db.crLogTrips.Find(id);
                logTrip.crLogClosingId = closingId;

                db.Entry(logTrip).State = EntityState.Modified;
                db.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public int getLatestStatusId(int id)
        {
            var logStatusQuery = db.crLogCashStatus.Where(c => c.crLogCashReleaseId == id).OrderByDescending(c => c.Id).FirstOrDefault();

            if (logStatusQuery != null)
            {
                return logStatusQuery.crCashReqStatusId;
            }
            else
            {
                //default 
                return 1;
            }
        }


        public bool AddLogStatus(int? id, int statusId)
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

        [HttpPost]
        public bool ApproveRequest(int? id)
        {
            try
            {
                if (id == null)
                    return false;
                return AddLogStatus(id, 2);
            }
            catch
            {
                return false;
            }
        }

        [HttpPost]
        public bool ApproveRelease(int? id)
        {
            try
            {
                if (id == null)
                    return false;
                return AddLogStatus(id, 3);
            }
            catch
            {
                return false;
            }
        }


        public ActionResult PrintApproveForm(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var cashRelease = db.crLogCashReleases.Find(id);
            var tripLogs = new List<crLogTrip>();
            if (cashRelease.crLogClosingId != null)
            {
                tripLogs = db.crLogTrips.Where(c => c.crLogClosingId == cashRelease.crLogClosingId).ToList();
            }

            if (cashRelease == null)
            {
                return HttpNotFound();
            }


            ViewBag.crLogTrips = tripLogs ?? new List<crLogTrip>();

            return View(cashRelease);
        }



        public ActionResult PrintSalaryForm(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var cashRelease = db.crLogCashReleases.Find(id);
            var tripLogs = new List<crLogTrip>();

            if (cashRelease.crLogClosingId != null)
            {
                tripLogs = db.crLogTrips.Where(c => c.crLogClosingId == cashRelease.crLogClosingId)
                    .OrderBy(c => c.DtTrip ).ToList();
            }

            if (cashRelease == null)
            {
                return HttpNotFound();
            }

            ViewBag.CA = GetDriverCashRelease(cashRelease.crLogDriverId, 2, cashRelease.DtRelease);
            ViewBag.Payments = GetDriverCashRelease(cashRelease.crLogDriverId, 3, cashRelease.DtRelease);
            ViewBag.Contributions = GetDriverCashRelease(cashRelease.crLogDriverId, 4, cashRelease.DtRelease);
            ViewBag.Others = GetDriverCashRelease(cashRelease.crLogDriverId, 5, cashRelease.DtRelease);
            ViewBag.OtherSalary = GetDriverCashReleaseByTrxId(cashRelease.Id, 1);
            ViewBag.crLogTrips = tripLogs ?? new List<crLogTrip>();
            ViewBag.CABalance = GetCABalance(cashRelease.crLogDriverId);

            ViewBag.ActualAmount = cashRelease.Amount;
            return View(cashRelease);
        }

        public List<crLogCashRelease> GetDriverCashRelease(int id, int type, DateTime dateReq)
        {
            var today = dateReq.AddDays(-1);
            var todayAdj = dateReq.AddDays(1);

            var payments = db.crLogCashReleases.Where(c => c.crLogDriverId == id 
                              && c.crLogCashTypeId == type && todayAdj.CompareTo(c.DtRelease) >= 0 && today.CompareTo(c.DtRelease) <= 0)
                              .OrderBy(c=>c.DtRelease).ToList();
            return payments;
        }


        public List<crLogCashRelease> GetDriverCashReleaseByTrxId(int cashTrxId, int type)
        {
            var today = dt.GetCurrentDate();

            var cashRelease = db.crLogCashReleases.Find(cashTrxId);

            today = cashRelease.DtRelease.Date;
            var otherTrx = db.crLogCashReleases.Where(c => c.crLogCashTypeId == type
                                    && c.crLogDriverId == cashRelease.crLogDriverId
                                    && DbFunctions.TruncateTime(c.DtRelease) == today
                                    && c.Id != cashTrxId
                                    ).ToList();
            return otherTrx;
        }



        public ActionResult DriverTripList(int? id)
        {
            var today = dt.GetCurrentDate();

            string tripLogErr = "";
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var cashRelease = db.crLogCashReleases.Find(id);

            var tripLogs = new List<crLogTrip>();
            if (cashRelease.crLogClosingId != null)
            {
                tripLogs = db.crLogTrips.Where(c => c.crLogClosingId == cashRelease.crLogClosingId).OrderBy(c=>c.DtTrip).ToList();
            }
            else
            {
                tripLogErr = "Closing Id is null";
            }

            if (cashRelease == null)
            {
                return HttpNotFound();
            }

            today = cashRelease.DtRelease.Date;
            var otherTrx = db.crLogCashReleases.Where(c=>
                                    c.crLogDriverId == cashRelease.crLogDriverId
                                    && DbFunctions.TruncateTime(c.DtRelease) == today 
                                    && c.crLogCashStatus.FirstOrDefault().crCashReqStatusId < (int)CASHREQ_STATUS.RELEASED
                                    ).ToList();

            ViewBag.Contributions = otherTrx.Where(c => c.crLogCashTypeId == (int)CASHTYPE.CONTRIBUTIONS).ToList();
            ViewBag.OtherSalary = otherTrx.Where(c => c.crLogCashTypeId == (int)CASHTYPE.SALARY && c.Id != id).ToList();
            ViewBag.Others = otherTrx.Where(c => c.crLogCashTypeId == (int)CASHTYPE.OTHERS).ToList();
            ViewBag.CA = otherTrx.Where(c => c.crLogCashTypeId == (int)CASHTYPE.CA).ToList();
            ViewBag.Payments = otherTrx.Where(c => c.crLogCashTypeId == (int)CASHTYPE.PAYMENTS).ToList();

            ViewBag.tripLogErr = tripLogErr;
            ViewBag.DtRelease = cashRelease.DtRelease;
            ViewBag.Driver = cashRelease.crLogDriver.Name;
            ViewBag.DriverId = cashRelease.crLogDriver.Id;
            ViewBag.Amount = cashRelease.Amount;
            ViewBag.Remarks = cashRelease.Remarks;
            ViewBag.Id = id;

            return View(tripLogs);
        }


        public decimal DriverSalaryAmount(int? id)
        {
            var today = dt.GetCurrentDate();
            decimal driversFee = 0;
            decimal driversOT = 0;

            if (id == null)
            {
                return 0;
            }

            var cashRelease = db.crLogCashReleases.Find(id);

            if (cashRelease.crLogClosingId != null)
            {
               var tripLogs = db.crLogTrips.Where(c => c.crLogClosingId == cashRelease.crLogClosingId)
                    .ToList();
                driversFee = tripLogs.Sum(c => c.DriverFee);
                driversOT = tripLogs.Sum(c => c.DriverOT);
            }
            else
            {
                    driversFee = cashRelease.Amount;
                    driversOT = 0;
                
            }

            if (cashRelease == null)
            {
                return 0;
            }

            today = cashRelease.DtRelease.Date;
            var OneDayAfter = today.AddDays(1);
            var OneDayBefore = today.AddDays(-1);

            var otherTrx = db.crLogCashReleases.Where(c =>
                                    c.crLogDriverId == cashRelease.crLogDriverId
                                    && ( DbFunctions.TruncateTime(c.DtRelease) == today  ||
                                         DbFunctions.TruncateTime(c.DtRelease) == OneDayAfter)
                                         //&& cashRelease.crLogCashStatus.OrderByDescending(d=>d.dtStatus).FirstOrDefault().crCashReqStatusId < 3
                                    ).ToList();

            var otherSalary = otherTrx.Where(c => c.crLogCashTypeId == (int)CASHTYPE.SALARY && c.Id != id).ToList().Sum(c=>c.Amount);
            var driverCA = otherTrx.Where(c => c.crLogCashTypeId == (int)CASHTYPE.CA
                            ).ToList().Sum(c => c.Amount);
            var payments = otherTrx.Where(c => c.crLogCashTypeId == (int)CASHTYPE.PAYMENTS).ToList().Sum(c => c.Amount);
            var contributions = otherTrx.Where(c => c.crLogCashTypeId == (int)CASHTYPE.CONTRIBUTIONS).ToList().Sum(c => c.Amount);
            var others = otherTrx.Where(c => c.crLogCashTypeId == (int)CASHTYPE.OTHERS).ToList().Sum(c => c.Amount);


            decimal total = (driversFee + driversOT + otherSalary  + driverCA) - (payments + contributions + others);


            return total;
        }

        private decimal GetCABalance(int DriverId)
        {
            decimal CABalance = 0;

            var driverCashReleases = db.crLogCashReleases;
            var  cashAdvance = driverCashReleases.Where(d => d.crLogDriverId == DriverId
                                                    && d.crLogClosing == null && d.crLogCashTypeId == 2)
                                                    .OrderBy(s => s.DtRelease).ToList();
            var payments = driverCashReleases.Where(d => d.crLogDriverId == DriverId
                                                    && d.crLogClosing == null && d.crLogCashTypeId == 3)
                                                    .OrderBy(s => s.DtRelease).ToList();

            CABalance = cashAdvance.Sum(c => c.Amount) - payments.Sum(c => c.Amount);
            if (CABalance > 0)
            {
                return CABalance;
            }
            else
            {
                return 0;
            }
        }

        private int GetStatusCount(int statusId, IQueryable<crLogCashRelease> cashReleases)
        {
            var statusCount = 0;
            var today = dt.GetCurrentDate();

            foreach (var log in cashReleases.ToList())
            {
                var lateststatusId = getLatestStatusId(log.Id);

                if (lateststatusId == statusId)
                {
                    //add request and accecpted logs
                    if (log.DtRelease.Date <= today.Date && lateststatusId < (int)CASHREQ_STATUS.RELEASED)
                        statusCount++;
                    //add returned logs with date today
                    if (log.DtRelease.Date == today.Date && lateststatusId == (int)CASHREQ_STATUS.RELEASED)
                        statusCount++;
                }
            }

            if (statusCount > 0)
            {
                return statusCount;
            }
            else
            {
                return 0;
            }

            
        }

    }
}


public class DriverReleaseRequest
{
    public int DriverId { get; set; }
    public decimal Amount { get; set; }
    public ICollection<int> TripIds { get; set; }
    public string Remarks { get; set; }
    public int CashTypeId { get; set; }
}

