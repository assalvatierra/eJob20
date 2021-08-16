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
    public class crLogFuelsController : Controller
    {
        private CarRentalLogDBContainer db = new CarRentalLogDBContainer();
        private DateClass dt = new DateClass();
        private CrDataLayer dl = new CrDataLayer();

        // GET: Personel/crLogFuels
        public ActionResult Index(int? statusId)
        {
            #region Session
            if (statusId != null)
                Session["CrLogFuel_StatusId"] = (int)statusId;
            else
            {
                if (Session["CrLogFuel_StatusId"] != null)
                    statusId = (int)Session["CrLogFuel_StatusId"];
                else
                {
                    statusId = 1;
                    Session["CrLogFuel_StatusId"] = 1;
                }
            }
            #endregion

            var today = dt.GetCurrentDate();
            var DateFilter = today.AddDays(-90);

            if (statusId == null || statusId == 1 || statusId == 2)
            {
                DateFilter = today.AddDays(-10);
            }

            //get fuel request up to -7 days from today
            var crLogFuels = db.crLogFuels
                .Where(c => DbFunctions.TruncateTime(c.dtRequest) >= DateFilter);

            if (statusId == null)
                statusId = 1;


            List<cCrLogFuel> cCrLogFuel = new List<cCrLogFuel>();

            foreach(var log in crLogFuels.ToList())
            {
                var status = db.crCashReqStatus.Find(getLatestStatusId(log.Id)).Status;

                var templog = new Models.cCrLogFuel()
                {
                    crLogFuel = log,
                    LatestStatusId = getLatestStatusId(log.Id),
                    LatestStatus = status
                };

                if (templog.LatestStatusId == statusId)
                {
                    //add request and accecpted logs
                    if (log.dtRequest.Date <= today.Date && templog.LatestStatusId < 4)
                        cCrLogFuel.Add(templog);
                    //add returned logs with date today
                    if (log.dtRequest.Date == today.Date && templog.LatestStatusId == 4)
                        cCrLogFuel.Add(templog);
                }

            }


            ViewBag.IsAdmin = User.IsInRole("Admin");
            ViewBag.StatusId = statusId;
            ViewBag.crLogPaymentType = db.crLogPaymentTypes.ToList();
            ViewBag.RequestCount = GetStatusCount(1, crLogFuels);
            ViewBag.ApprovedCount = GetStatusCount(2, crLogFuels);
            ViewBag.ReleasedCount = GetStatusCount(3, crLogFuels);

            return View(cCrLogFuel.ToList());
        }


        // GET: Personel/crLogFuels
        public ActionResult Unreturned()
        {
            var today = dt.GetCurrentDate();
            var DateFilter = today.AddDays(-360);

            //get fuel request up to -7 days from today
            var crLogFuels = db.crLogFuels.Include(c => c.crLogUnit).Include(c => c.crLogDriver)
                .Where(c => DbFunctions.TruncateTime(c.dtRequest) >= DateFilter && 
                    c.crLogFuelStatus.OrderByDescending(f=>f.Id).FirstOrDefault().crCashReqStatusId < 4);

          
             var statusId = 3;

            List<cCrLogFuel> cCrLogFuel = new List<cCrLogFuel>();

            foreach (var log in crLogFuels.ToList())
            {
                var status = db.crCashReqStatus.Find(getLatestStatusId(log.Id)).Status;

                var templog = new Models.cCrLogFuel()
                {
                    crLogFuel = log,
                    LatestStatusId = getLatestStatusId(log.Id),
                    LatestStatus = status
                };

                if (templog.LatestStatusId == statusId)
                {
                    //add request and accecpted logs
                    if (log.dtRequest.Date <= today.Date && templog.LatestStatusId < 4)
                        cCrLogFuel.Add(templog);
                    //add returned logs with date today
                    if (log.dtRequest.Date == today.Date && templog.LatestStatusId == 4)
                        cCrLogFuel.Add(templog);
                }

            }


            ViewBag.IsAdmin = User.IsInRole("Admin");
            ViewBag.StatusId = statusId;
            ViewBag.crLogPaymentType = db.crLogPaymentTypes.ToList();

            return View(cCrLogFuel.ToList());
        }

        // GET: Personel/crLogFuels
        public ActionResult PrevRecords(int? unitId, DateTime? startDate, DateTime? endDate)
        {
            if (unitId == null)
            {
                unitId = 0;
            }

            if (startDate == null)
            {
                startDate = dt.GetCurrentDate().AddDays(-30);
            }


            if (endDate == null)
            {
                endDate = dt.GetCurrentDate();
            }


            var crLogFuels = db.crLogFuels.Include(c => c.crLogUnit).Include(c => c.crLogDriver)
                .Where(c => DbFunctions.TruncateTime(c.dtRequest) >= startDate &&
                DbFunctions.TruncateTime(c.dtRequest) <= endDate
                ).OrderBy(c => c.dtRequest);

            if (unitId != 0)
            {
                crLogFuels = crLogFuels.Where(c => c.crLogUnitId == unitId).OrderBy(c => c.dtRequest);
            }

            List<cCrLogFuel> cCrLogFuel = new List<cCrLogFuel>();

            foreach (var log in crLogFuels.ToList())
            {
                var status = db.crCashReqStatus.Find(getLatestStatusId(log.Id)).Status;

                var templog = new Models.cCrLogFuel()
                {
                    crLogFuel = log,
                    LatestStatusId = getLatestStatusId(log.Id),
                    LatestStatus = status
                };

                if(templog.LatestStatusId == 4)
                    cCrLogFuel.Add(templog);
            }

            DateTime _tempDtStart = new DateTime();
            DateTime.TryParse(startDate.ToString(), out _tempDtStart);

            //ViewBag.IsAdmin = User.IsInRole("Admin");
            ViewBag.IsAdmin = true;
            ViewBag.UnitList = dl.GetUnits().ToList();
            ViewBag.StartDate = startDate.ToString();
            ViewBag.EndDate = endDate.ToString();

            return View(cCrLogFuel.OrderByDescending(c=>c.crLogFuel.dtRequest).ToList());
        }

        // GET: Personel/crLogFuels/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            crLogFuel crLogFuel = db.crLogFuels.Find(id);
            if (crLogFuel == null)
            {
                return HttpNotFound();
            }
            return View(crLogFuel);
        }

        // GET: Personel/crLogFuels/Create
        public ActionResult Create()
        {
            crLogFuel logFuel = new crLogFuel();
            logFuel.Amount = 0;
            logFuel.odoFillup = 0;
            logFuel.orAmount = 0;
            logFuel.dtRequest = dt.GetCurrentDateTime();
            logFuel.dtFillup = dt.GetCurrentDateTime();

            ViewBag.crLogUnitId = new SelectList(dl.GetUnits(), "Id", "Description");
            ViewBag.crLogDriverId = new SelectList(dl.GetDrivers(), "Id", "Name");
            ViewBag.crLogTypeId = new SelectList(db.crLogTypes, "Id", "Type");
            ViewBag.crLogPaymentTypeId = new SelectList(db.crLogPaymentTypes, "Id", "Type", 5);
            return View(logFuel);
        }

        // POST: Personel/crLogFuels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,dtRequest,Amount,crLogUnitId,crLogDriverId,dtFillup,odoFillup,orAmount,crLogTypeId,isFullTank,crLogPaymentTypeId,Remarks")] crLogFuel crLogFuel)
        {
            if (ModelState.IsValid)
            {
                crLogFuel.isFullTank = false;
                db.crLogFuels.Add(crLogFuel);
                db.SaveChanges();

                //add status logs, REQUEST
                AddLogStatus(crLogFuel.Id, 1);

                return RedirectToAction("Index");
            }

            ViewBag.crLogUnitId = new SelectList(dl.GetUnits(), "Id", "Description", crLogFuel.crLogUnitId);
            ViewBag.crLogDriverId = new SelectList(dl.GetDrivers(), "Id", "Name", crLogFuel.crLogDriverId);
            ViewBag.crLogTypeId = new SelectList(db.crLogTypes, "Id", "Type", crLogFuel.crLogTypeId);
            ViewBag.crLogPaymentTypeId = new SelectList(db.crLogPaymentTypes, "Id", "Type", crLogFuel.crLogPaymentTypeId);
            return View(crLogFuel);
        }

        // GET: Personel/crLogFuels/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            crLogFuel crLogFuel = db.crLogFuels.Find(id);
            if (crLogFuel == null)
            {
                return HttpNotFound();
            }
            ViewBag.LatestStatusId = getLatestStatusId((int)id);
            ViewBag.crLogUnitId = new SelectList(dl.GetUnits(), "Id", "Description", crLogFuel.crLogUnitId);
            ViewBag.crLogDriverId = new SelectList(dl.GetDrivers(), "Id", "Name", crLogFuel.crLogDriverId);
            ViewBag.crLogTypeId = new SelectList(db.crLogTypes, "Id", "Type", crLogFuel.crLogTypeId);
            ViewBag.crLogPaymentTypeId = new SelectList(db.crLogPaymentTypes, "Id", "Type", crLogFuel.crLogPaymentTypeId);
            return View(crLogFuel);
        }

        // POST: Personel/crLogFuels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,dtRequest,Amount,crLogUnitId,crLogDriverId,dtFillup,odoFillup,orAmount,crLogTypeId,odoStart,odoEnd, isFullTank, crLogPaymentTypeId, Remarks")] crLogFuel crLogFuel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(crLogFuel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.crLogUnitId = new SelectList(dl.GetUnits(), "Id", "Description", crLogFuel.crLogUnitId);
            ViewBag.crLogDriverId = new SelectList(dl.GetDrivers(), "Id", "Name", crLogFuel.crLogDriverId);
            ViewBag.crLogTypeId = new SelectList(db.crLogTypes, "Id", "Type", crLogFuel.crLogTypeId);
            ViewBag.crLogPaymentTypeId = new SelectList(db.crLogPaymentTypes, "Id", "Type", crLogFuel.crLogPaymentTypeId);
            return View(crLogFuel);
        }


        // GET: Personel/crLogFuels/Edit/5
        public ActionResult Return(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            crLogFuel crLogFuel = db.crLogFuels.Find(id);
            if (crLogFuel == null)
            {
                return HttpNotFound();
            }
            ViewBag.crLogUnitId = new SelectList(dl.GetUnits(), "Id", "Description", crLogFuel.crLogUnitId);
            ViewBag.crLogDriverId = new SelectList(dl.GetDrivers(), "Id", "Name", crLogFuel.crLogDriverId);
            ViewBag.crLogTypeId = new SelectList(db.crLogTypes, "Id", "Type", crLogFuel.crLogTypeId);
            ViewBag.crLogPaymentTypeId = new SelectList(db.crLogPaymentTypes, "Id", "Type", crLogFuel.crLogPaymentTypeId);
            return View(crLogFuel);
        }

        // POST: Personel/crLogFuels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Return([Bind(Include = "Id,dtRequest,Amount,crLogUnitId,crLogDriverId,dtFillup,odoFillup,orAmount,crLogTypeId,odoStart,odoEnd,isFullTank,crLogPaymentTypeId,Remarks")] crLogFuel crLogFuel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(crLogFuel).State = EntityState.Modified;
                db.SaveChanges();

                //add status logs, RETURNED
                AddLogStatus(crLogFuel.Id, 4);

                return RedirectToAction("Index", new { statusId = 3 });
            }
            ViewBag.crLogUnitId = new SelectList(dl.GetUnits(), "Id", "Description", crLogFuel.crLogUnitId);
            ViewBag.crLogDriverId = new SelectList(dl.GetDrivers(), "Id", "Name", crLogFuel.crLogDriverId);
            ViewBag.crLogTypeId = new SelectList(db.crLogTypes, "Id", "Type", crLogFuel.crLogTypeId);
            ViewBag.crLogPaymentTypeId = new SelectList(db.crLogPaymentTypes, "Id", "Type", crLogFuel.crLogPaymentTypeId);
            return View(crLogFuel);
        }

        // GET: Personel/crLogFuels/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            crLogFuel crLogFuel = db.crLogFuels.Find(id);
            if (crLogFuel == null)
            {
                return HttpNotFound();
            }
            return View(crLogFuel);
        }

        // POST: Personel/crLogFuels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                if (DeleteStatusRecords(id))
                {
                    crLogFuel crLogFuel = db.crLogFuels.Find(id);
                    db.crLogFuels.Remove(crLogFuel);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

            }
            catch
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
        }

        public bool DeleteStatusRecords(int crFuelLogId)
        {
            try
            {
                var logStatusList = db.crLogFuelStatus.Where(c => c.crLogFuelId == crFuelLogId).ToList();

                if(logStatusList != null)
                {
                    db.crLogFuelStatus.RemoveRange(logStatusList);
                    db.SaveChanges();
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public int getLatestStatusId(int id)
        {
            var logStatusQuery = db.crLogFuelStatus.Where(c=>c.crLogFuelId == id).OrderByDescending(c => c.Id).FirstOrDefault();

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

        [HttpPost]
        public bool SubmitReturnLog(int? id, string date, int odo, decimal amount, bool isFullTank, int paymentTypeId, string remarks)
        {
            try
            {
                if (id == null)
                    return false;

                //find logFuel by Id
                var crLogFuel = db.crLogFuels.Find(id);

                if(crLogFuel == null)
                    return false;

                //apply changes
                crLogFuel.dtFillup = DateTime.Parse(date);
                crLogFuel.odoFillup = odo;
                crLogFuel.orAmount = amount;
                crLogFuel.isFullTank = isFullTank;
                crLogFuel.crLogPaymentTypeId = paymentTypeId;
                crLogFuel.Remarks = remarks;

                //save changes
                db.Entry(crLogFuel).State = EntityState.Modified;
                db.SaveChanges();

                //add odo log
                //AddOdoLog(crLogFuel);

                return AddLogStatus(id, 4);
            }
            catch 
            {
                return false;
            }
        }

        public bool AddLogStatus(int? id, int statusId)
        {
            try
            {
                if(id == null)
                {
                    return false;
                }

                //create new status log
                crLogFuelStatus logFuelStatus = new crLogFuelStatus();
                logFuelStatus.crCashReqStatusId = statusId;
                logFuelStatus.crLogFuelId = (int)id;
                logFuelStatus.dtStatus = dt.GetCurrentDateTime();

                db.crLogFuelStatus.Add(logFuelStatus);
                db.SaveChanges();

                return true;
            }
            catch 
            {
                return false;
            }
        }

        public ActionResult PrintApproveForm(int? id)
        {

            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var crlogFuel = db.crLogFuels.Find(id);

            if (crlogFuel == null)
            {
                return HttpNotFound();
            }

            var user = HttpContext.User.Identity.Name;
            var sepIndex = user.LastIndexOf('@');
            user = user.Substring(0, sepIndex);
            ViewBag.ReleaseUser = user;

            return View(crlogFuel);
        }


        public ActionResult PrintFuelRequestForm(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var crlogFuel = db.crLogFuels.Find(id);

            if (crlogFuel == null)
            {
                return HttpNotFound();
            }

            return View(crlogFuel);
        }


        public ActionResult PrintPOForm(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var crlogFuel = db.crLogFuels.Find(id);

            if (crlogFuel == null)
            {
                return HttpNotFound();
            }

            var crlogFuels = db.crLogFuels.Where(
                                c => c.crLogDriverId == crlogFuel.crLogDriverId
                                && c.crLogTypeId == 2 && c.crLogPaymentTypeId == 3).ToList();
            crlogFuels = crlogFuels.Where(c => c.dtRequest.Date.CompareTo(crlogFuel.dtRequest.Date) == 0).ToList();
            ViewBag.DtRequest = crlogFuel.dtRequest;
            ViewBag.PONo = crlogFuel.Id;
            ViewBag.DriverName = crlogFuel.crLogDriver.Name;
            ViewBag.Unit = crlogFuel.crLogUnit.Description;
            return View(crlogFuels);
        }


        private int GetStatusCount(int statusId, IQueryable<crLogFuel> crLogFuels)
        {
            var statusCount = 0;
            var today = dt.GetCurrentDate();

            foreach (var log in crLogFuels.ToList())
            {
                var status = db.crCashReqStatus.Find(getLatestStatusId(log.Id)).Status;

                var templog = new Models.cCrLogFuel()
                {
                    crLogFuel = log,
                    LatestStatusId = getLatestStatusId(log.Id),
                    LatestStatus = status
                };

                if (templog.LatestStatusId == statusId)
                {
                    //add request and accecpted logs
                    if (log.dtRequest.Date <= today.Date && templog.LatestStatusId < 4)
                        statusCount++;
                    //add returned logs with date today
                    if (log.dtRequest.Date == today.Date && templog.LatestStatusId == 4)
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


        #region Odo Logs
        public bool AddOdoLog(crLogFuel logFuel)
        {
            try
            {
                if (logFuel == null)
                    return false;

                crLogOdo logOdo = new crLogOdo();
                logOdo.crLogUnitId = logFuel.crLogUnitId;
                logOdo.crLogDriverId = logFuel.crLogDriverId;
                logOdo.OdoCurrent = logFuel.odoFillup;
                logOdo.dtReading = logFuel.dtFillup;
                logOdo.Remarks = " ";

                db.crLogOdoes.Add(logOdo);
                db.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
           
        }
        #endregion
    }
}
