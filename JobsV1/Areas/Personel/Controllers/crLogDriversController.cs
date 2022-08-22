using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using JobsV1.Areas.Personel.Models;
using Microsoft.Ajax.Utilities;
using JobsV1.Areas.Personel.Services;

namespace JobsV1.Areas.Personel.Controllers
{
    public class crLogDriversController : Controller
    {
        private CarRentalLogDBContainer db = new CarRentalLogDBContainer();
        private DateClass dt = new DateClass();
        private CrDataLayer dl = new CrDataLayer();
        private CrLogServices crServices;

        private List<SelectListItem> StatusList = new List<SelectListItem> {
                new SelectListItem { Value = "ACT", Text = "Active" },
                new SelectListItem { Value = "INC", Text = "Inactive" }
                };

        private List<SelectListItem> OrderList = new List<SelectListItem> {
                new SelectListItem { Value = "100", Text = "Realbreeze" },
                new SelectListItem { Value = "200", Text = "Office" },
                new SelectListItem { Value = "300", Text = "3rd Party" },
                new SelectListItem { Value = "400", Text = "AJ3s Drivers" },
                new SelectListItem { Value = "500", Text = "Kabacan Group" },
                new SelectListItem { Value = "600", Text = "Others" },
                new SelectListItem { Value = "900", Text = "Inactive" }
                };
        // GET: Personel/crLogDrivers
        public ActionResult Index()
        {
            return View(db.crLogDrivers.OrderBy(c=> c.OrderNo ?? 999).ToList());
        }

        // GET: Personel/crLogDrivers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            crLogDriver crLogDriver = db.crLogDrivers.Find(id);
            if (crLogDriver == null)
            {
                return HttpNotFound();
            }
            return View(crLogDriver);
        }

        // GET: Personel/crLogDrivers/Create
        public ActionResult Create()
        {
            crLogDriver crLogDriver = new crLogDriver();
            crLogDriver.OrderNo = 100;

            ViewBag.OrderNo = new SelectList(OrderList, "value", "text");
            ViewBag.Status = new SelectList(StatusList, "value", "text");
            return View(crLogDriver);
        }

        // POST: Personel/crLogDrivers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name, Contact1, Contact2, OrderNo, Status")] crLogDriver crLogDriver)
        {
            if (ModelState.IsValid && InputValidation(crLogDriver))
            {
                db.crLogDrivers.Add(crLogDriver);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.OrderNo = new SelectList(OrderList, "value", "text", crLogDriver.OrderNo);
            ViewBag.Status = new SelectList(StatusList, "value", "text", crLogDriver.Status);
            return View(crLogDriver);
        }

        // GET: Personel/crLogDrivers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            crLogDriver crLogDriver = db.crLogDrivers.Find(id);
            if (crLogDriver == null)
            {
                return HttpNotFound();
            }
            ViewBag.OrderNo = new SelectList(OrderList, "value", "text", crLogDriver.OrderNo);
            ViewBag.Status = new SelectList(StatusList, "value", "text", crLogDriver.Status);
            return View(crLogDriver);
        }

        // POST: Personel/crLogDrivers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Contact1, Contact2, OrderNo, Status")] crLogDriver crLogDriver)
        {
            if (ModelState.IsValid && InputValidation(crLogDriver))
            {
                db.Entry(crLogDriver).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.OrderNo = new SelectList(OrderList, "value", "text", crLogDriver.OrderNo);
            ViewBag.Status = new SelectList(StatusList, "value", "text", crLogDriver.Status);
            return View(crLogDriver);
        }

        // GET: Personel/crLogDrivers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            crLogDriver crLogDriver = db.crLogDrivers.Find(id);
            if (crLogDriver == null)
            {
                return HttpNotFound();
            }
            return View(crLogDriver);
        }

        // POST: Personel/crLogDrivers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            crLogDriver crLogDriver = db.crLogDrivers.Find(id);
            db.crLogDrivers.Remove(crLogDriver);
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

        public bool InputValidation(crLogDriver crLogDriver)
        {
            bool isValid = true;

            if (crLogDriver.Name.IsNullOrWhiteSpace())
            {
                ModelState.AddModelError("Name", "Invalid Name");
                isValid = false;
            }


            return isValid;
        }


        // GET: Personel/CarRentalLog/EditCashTrx/5
        public ActionResult EditCashTrx(int? id)
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

        // POST: Personel/CarRentalLog/EditCashTrx/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditCashTrx([Bind(Include = "Id,DtRelease,Amount,Remarks,crLogDriverId,crLogClosingId,crLogCashTypeId")] crLogCashRelease crLogCashRelease)
        {
            if (ModelState.IsValid)
            {
                db.Entry(crLogCashRelease).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("DriverSummary", "CrLogDrivers", new { id = crLogCashRelease.crLogDriverId });
            }
            ViewBag.crLogDriverId = new SelectList(dl.GetDrivers(), "Id", "Name", crLogCashRelease.crLogDriverId);
            ViewBag.crLogClosingId = new SelectList(db.crLogClosings, "Id", "Id", crLogCashRelease.crLogClosingId);
            ViewBag.crLogCashTypeId = new SelectList(db.crLogCashTypes, "Id", "Description", crLogCashRelease.crLogCashTypeId);
            return View(crLogCashRelease);
        }


        // GET: Personel/CarRentalLog/CloseCashTrx/5
        public ActionResult CloseCashTrx(int? id)
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
            try
            {
                //create closing Id
                crServices = new CrLogServices(db);

                crLogCashRelease.crLogClosingId = crServices.GenerateClosingId();

                db.Entry(crLogCashRelease).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("DriverSummary", new { id = crLogCashRelease.crLogDriverId });
            }
            catch
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }


        /*  id = DriverId
         *  reqStatus = 1 ( Salary Request added)
         *              2 ( CA Request added)
         *              3 ( Payment added)
         *              4 ( Closed All CA & Payments Success )
         *              5 ( Closed All CA & Payments Error )
         * 
         */
        public ActionResult DriverSummary(int id, int? reqStatus, string DtStart, string DtEnd, int? rptId)
        {
            DateTime sDate = new DateTime();
            DateTime eDate = new DateTime();
            var today = dt.GetCurrentDate();

            crLogDriver driver = db.crLogDrivers.Find(id);
            List<crLogTrip> trips = db.crLogTrips.Where(d => d.crLogDriverId == id && d.crLogClosingId == null).OrderBy(s => s.DtTrip).ToList();

            if (DateTime.TryParse(DtStart, out sDate) && DateTime.TryParse(DtEnd, out eDate))
            {
                TimeSpan duration = new TimeSpan(23, 59, 0); //11:59:0 PM
                eDate = eDate.Add(duration);

                trips = trips.Where(d => d.DtTrip >= sDate && d.DtTrip <= eDate).ToList();
            }


            //if report is generated from unit expenses reports
            if (rptId != null)
            {
                //get unitId List on report
                var unitReport = db.crRptUnitExpenses.Find(rptId);
                if (unitReport == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                var unitList = unitReport.CrRptUnits.Select(c => c.crLogUnitId).ToList();

                trips = trips.Where(t => unitList.Contains(t.crLogUnitId)).ToList();
            }

            var driverCashReleases = db.crLogCashReleases;
            List<crLogCashRelease> cashAdvance = driverCashReleases.Where(d => d.crLogDriverId == id && d.crLogClosing == null && d.crLogCashTypeId == 2).OrderBy(s => s.DtRelease).ToList();
            List<crLogCashRelease> payments = driverCashReleases.Where(d => d.crLogDriverId == id && d.crLogClosing == null && d.crLogCashTypeId == 3).OrderBy(s => s.DtRelease).ToList();
            List<crLogCashRelease> contributions = driverCashReleases.Where(d => d.crLogDriverId == id && d.crLogClosing == null && d.crLogCashTypeId == 4).OrderBy(s => s.DtRelease).ToList();
            List<crLogCashRelease> noStatus = driverCashReleases.Where(d => d.crLogDriverId == id && d.crLogClosing == null && d.crLogCashTypeId == 1).OrderBy(s => s.DtRelease).ToList();
            List<crLogCashRelease> cashtrx = driverCashReleases.Where(d => d.crLogDriverId == id && d.crLogClosing == null && d.crLogCashTypeId != 1).OrderBy(s => s.DtRelease).ToList();
            List<crLogDriverTerm> terms = db.crLogDriverTerms.Where(d => d.crLogDriverId == id).ToList();

            crDriverSummary driversummary = new crDriverSummary();
            driversummary.Driver = driver;
            driversummary.DriverTrips = trips;
            driversummary.DriverCash = cashAdvance;
            driversummary.DriverPayments = payments;
            driversummary.NoStatus = noStatus;
            driversummary.DriverTrx = cashtrx;
            driversummary.DriverContributions = contributions;
            driversummary.Terms = terms;

            var rptName = "";
            if (rptId != null)
            {
                rptName = db.crRptUnitExpenses.Find(rptId).RptName;
            }

            ViewBag.rptId = rptId ?? 0;
            ViewBag.rptName = rptName;
            ViewBag.DtStart = DtStart;
            ViewBag.DtEnd = DtEnd;
            ViewBag.DriverId = id;
            ViewBag.crLogDriverId = new SelectList(dl.GetDrivers().Where(d => d.OrderNo <= 200).ToList(), "Id", "Name", id);
            ViewBag.reqStatus = reqStatus ?? 0;
            ViewBag.IsAdmin = User.IsInRole("Admin");

            return View(driversummary);
        }

        // GET: Personel/CarRentalLog/Edit/5
        public ActionResult DriverSummaryTripEdit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            crLogTrip crLogTrip = db.crLogTrips.Find(id);
            if (crLogTrip == null)
            {
                return HttpNotFound();
            }

            var unitList = dl.GetUnits().Select(c => new {
                Id = c.Id,
                Description = c.Description + " (" + c.crLogOwner.Name + ")"
            }).ToList();

            ViewBag.crLogDriverId = new SelectList(dl.GetDrivers(), "Id", "Name", crLogTrip.crLogDriverId);
            ViewBag.crLogUnitId = new SelectList(unitList, "Id", "Description", crLogTrip.crLogUnitId);
            ViewBag.crLogCompanyId = new SelectList(dl.GetCompanies(), "Id", "Name", crLogTrip.crLogCompanyId);
            //ViewBag.crLogClosingId = new SelectList(db.crLogClosings, "Id", "Id", crLogTrip.crLogClosingId);
            return View(crLogTrip);
        }

        // POST: Personel/CarRentalLog/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DriverSummaryTripEdit([Bind(Include = "Id,crLogDriverId,crLogUnitId,crLogCompanyId,DtTrip,Rate,Addon,Expenses," +
            "DriverFee,Remarks, OdoStart, OdoEnd, crLogClosingId, DriverOt, TripHours, StartTime, EndTime, OTRate, DriverOTRate, " +
            "AddonOT, IsFinal, AllowEdit, TripTicket, IncludeOT")] crLogTrip crLogTrip)
        {
            if (ModelState.IsValid)
            {
                db.Entry(crLogTrip).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("DriverSummary", new { id = crLogTrip.crLogDriverId });
            }

            var unitList = dl.GetUnits().Select(c => new {
                Id = c.Id,
                Description = c.Description + " (" + c.crLogOwner.Name + ")"
            }).ToList();

            ViewBag.crLogDriverId = new SelectList(dl.GetDrivers(), "Id", "Name", crLogTrip.crLogDriverId);
            ViewBag.crLogUnitId = new SelectList(unitList, "Id", "Description", crLogTrip.crLogUnitId);
            ViewBag.crLogCompanyId = new SelectList(dl.GetCompanies(), "Id", "Name", crLogTrip.crLogCompanyId);
            //ViewBag.crLogClosingId = new SelectList(db.crLogClosings, "Id", "Id", crLogTrip.crLogClosingId);
            return View(crLogTrip);
        }

        public ActionResult CloseCashBalance(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            crLogDriver driver = db.crLogDrivers.Find(id);

            //Get CA 
            List<crLogCashRelease> cashAdvance = db.crLogCashReleases
                .Where(d => d.crLogDriverId == id && d.crLogClosing == null && d.crLogCashTypeId == 2)
                .OrderBy(s => s.DtRelease).ToList();
            //Get Payments
            List<crLogCashRelease> payments = db.crLogCashReleases
                .Where(d => d.crLogDriverId == id && d.crLogClosing == null && d.crLogCashTypeId == 3)
                .OrderBy(s => s.DtRelease).ToList();
            //Get Cash
            List<crLogCashRelease> cashtrx = db.crLogCashReleases
                .Where(d => d.crLogDriverId == id && d.crLogClosing == null && d.crLogCashTypeId != 1)
                .OrderBy(s => s.DtRelease).ToList();

            decimal totalCA = 0;
            decimal totalPayments = 0;
            //decimal Balance = 0;

            foreach (var cash in cashAdvance)
            {
                totalCA += cash.Amount;
            }

            foreach (var cash in payments)
            {
                totalPayments += cash.Amount;
            }

            //when balance is Payments >= totalCA
            if (totalPayments >= totalCA)
            {
                //close all trx
                foreach (var trx in cashtrx)
                {
                    //close
                    var result = CloselogCashRelease(trx);

                    if (!result)
                    {
                        return RedirectToAction("DriverSummary", new { id, reqStatus = 5 });
                    }
                }
            }

            return RedirectToAction("DriverSummary", new { id, reqStatus = 4 });
        }

        public ActionResult PaytoClose(int id, decimal amount)
        {
            crLogDriver driver = db.crLogDrivers.Find(id);
            List<crLogTrip> trips = db.crLogTrips.Where(d => d.crLogDriverId == id && d.crLogClosingId == null).ToList();
            List<crLogCashRelease> cashtrx = db.crLogCashReleases.Where(d => d.crLogDriverId == id && d.crLogClosingId == null).ToList();

            string sErrorMessage = "";
            decimal _tripAmt = 0;
            decimal _cashAmt = 0;
            decimal _paytoCloseAmt = 0;
            foreach (var item in trips)
            {
                _tripAmt += item.DriverFee;
            }
            foreach (var item in cashtrx)
            {
                _cashAmt += item.Amount;
            }
            _paytoCloseAmt = _tripAmt - _cashAmt;

            if (_paytoCloseAmt != amount)
                sErrorMessage = "Amount is not valid to close the transactions.";


            int closingId = GenerateClosingTrx();
            decimal _tripAmt2 = 0;
            try
            {
                foreach (var item in trips)
                {
                    _tripAmt2 += item.DriverFee;
                    item.crLogClosingId = closingId;
                }
                if (_tripAmt2 == _tripAmt)
                    db.SaveChanges();
                else
                    sErrorMessage = "Amount in Trips has changed and not ready for closing. please try again.";
            }
            catch (Exception e)
            {
                sErrorMessage = "Unable to finish closing the trips logs \n\n" + e.Message;
            }

            decimal _cashAmt2 = 0;
            try
            {
                foreach (var item in cashtrx)
                {
                    _cashAmt2 += item.Amount;
                    item.crLogClosingId = closingId;
                }
                if (_cashAmt2 == _cashAmt)
                    db.SaveChanges();
                else
                    sErrorMessage = "Amount in Cash Trx has changed and not ready for closing. please try again.";
            }
            catch (Exception e)
            {
                sErrorMessage = "Unable to finish closing cash trasactions \n\n" + e.Message;
            }


            if (sErrorMessage.Trim() == "")
            {
                ViewBag.ReturnCode = "1";
                ViewBag.ReturnMessage = "Closing successfull...";
            }
            else
            {
                ViewBag.ReturnCode = "-1";
                ViewBag.ReturnMessage = sErrorMessage;
            }

            ViewBag.DriverId = id;
            return View();
        }

        public ActionResult DriverTrxHistory(int? id)
        {
            if (id != null)
            {
                var cashLogs = db.crLogCashReleases.Where(c => c.crLogDriverId == id).ToList();

                ViewBag.DriverId = id;
                ViewBag.Driver = db.crLogDrivers.Find(id).Name;

                return View(cashLogs.OrderByDescending(c => c.DtRelease));
            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        public ActionResult DriverExpenseHistory(int? id)
        {
            if (id != null)
            {
                var expenseLogs = db.crLogFuels.Where(c => c.crLogDriverId == id).ToList();

                var crLogFuels = db.crLogFuels.Include(c => c.crLogUnit).Include(c => c.crLogDriver)
                    .Where(c => c.crLogDriverId == id).OrderBy(c => c.dtRequest);

                List<cCrLogFuel> cCrLogFuel = new List<cCrLogFuel>();

                foreach (var log in crLogFuels.ToList())
                {
                    var status = db.crCashReqStatus.Find(GetLatestStatusId(log.Id)).Status;

                    var templog = new Models.cCrLogFuel()
                    {
                        crLogFuel = log,
                        LatestStatusId = GetLatestStatusId(log.Id),
                        LatestStatus = status
                    };

                    if (templog.LatestStatusId == 4)
                        cCrLogFuel.Add(templog);
                }


                ViewBag.IsAdmin = User.IsInRole("Admin");
                ViewBag.DriverId = id;
                ViewBag.Driver = db.crLogDrivers.Find(id).Name;

                return View(cCrLogFuel.OrderByDescending(c => c.crLogFuel.dtRequest).ToList());
            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        public int GetLatestStatusId(int id)
        {
            var logStatusQuery = db.crLogFuelStatus.Where(c => c.crLogFuelId == id).OrderByDescending(c => c.Id).FirstOrDefault();

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

        private int GenerateClosingTrx()
        {
            crLogClosing ctrx = new crLogClosing()
            { dtClose = System.DateTime.Now };

            db.crLogClosings.Add(ctrx);
            db.SaveChanges();

            return ctrx.Id;
        }

        private bool CloselogCashRelease(crLogCashRelease crLogCashRelease)
        {
            if (crLogCashRelease == null)
            {
                return false;
            }
            try
            {
                //create closing Id
                crLogCashRelease.crLogClosingId = GenerateClosingTrx();
                db.Entry(crLogCashRelease).State = EntityState.Modified;
                db.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }


        private bool CloseTrxPost(int? id)
        {
            if (id == null)
            {
                return false;
            }


            try
            {
                crLogCashRelease crLogCashRelease = db.crLogCashReleases.Find(id);

                if (crLogCashRelease == null)
                {
                    return false;
                }

                //create closing Id
                crServices = new CrLogServices(db);

                crLogCashRelease.crLogClosingId = crServices.GenerateClosingId();

                db.Entry(crLogCashRelease).State = EntityState.Modified;
                db.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

    }
}
