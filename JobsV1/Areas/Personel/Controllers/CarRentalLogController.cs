using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using JobsV1.Areas.Personel.Models;

namespace JobsV1.Areas.Personel.Controllers
{
    public class CarRentalLogController : Controller
    {
        private CarRentalLogDBContainer db = new CarRentalLogDBContainer();

        // GET: Personel/CarRentalLog
        public ActionResult Index()
        {
            var crLogTrips = db.crLogTrips.Include(c => c.crLogDriver).Include(c => c.crLogUnit).Include(c => c.crLogCompany).Include(c => c.crLogClosing);
            return View(crLogTrips.ToList());
        }

        // GET: Personel/CarRentalLog/Details/5
        public ActionResult Details(int? id)
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
            return View(crLogTrip);
        }

        // GET: Personel/CarRentalLog/Create
        public ActionResult Create()
        {
            crLogTrip trip = new crLogTrip();
            trip.DtTrip = System.DateTime.Now;
            trip.Rate = 0;
            trip.Expenses = 0;
            trip.Addon = 0;
            trip.DriverFee = 0;


            ViewBag.crLogDriverId = new SelectList(db.crLogDrivers, "Id", "Name");
            ViewBag.crLogUnitId = new SelectList(db.crLogUnits, "Id", "Description");
            ViewBag.crLogCompanyId = new SelectList(db.crLogCompanies, "Id", "Name");
            //ViewBag.crLogClosingId = new SelectList(db.crLogClosings, "Id", "Id");
            return View(trip);
        }

        // POST: Personel/CarRentalLog/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,crLogDriverId,crLogUnitId,crLogCompanyId,DtTrip,Rate,Addon,Expenses,DriverFee,Remarks")] crLogTrip crLogTrip)
        {
            if (ModelState.IsValid)
            {
                db.crLogTrips.Add(crLogTrip);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.crLogDriverId = new SelectList(db.crLogDrivers, "Id", "Name", crLogTrip.crLogDriverId);
            ViewBag.crLogUnitId = new SelectList(db.crLogUnits, "Id", "Description", crLogTrip.crLogUnitId);
            ViewBag.crLogCompanyId = new SelectList(db.crLogCompanies, "Id", "Name", crLogTrip.crLogCompanyId);
            //ViewBag.crLogClosingId = new SelectList(db.crLogClosings, "Id", "Id", crLogTrip.crLogClosingId);
            return View(crLogTrip);
        }

        // GET: Personel/CarRentalLog/Edit/5
        public ActionResult Edit(int? id)
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
            ViewBag.crLogDriverId = new SelectList(db.crLogDrivers, "Id", "Name", crLogTrip.crLogDriverId);
            ViewBag.crLogUnitId = new SelectList(db.crLogUnits, "Id", "Description", crLogTrip.crLogUnitId);
            ViewBag.crLogCompanyId = new SelectList(db.crLogCompanies, "Id", "Name", crLogTrip.crLogCompanyId);
            //ViewBag.crLogClosingId = new SelectList(db.crLogClosings, "Id", "Id", crLogTrip.crLogClosingId);
            return View(crLogTrip);
        }

        // POST: Personel/CarRentalLog/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,crLogDriverId,crLogUnitId,crLogCompanyId,DtTrip,Rate,Addon,Expenses,DriverFee,Remarks")] crLogTrip crLogTrip)
        {
            if (ModelState.IsValid)
            {
                db.Entry(crLogTrip).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.crLogDriverId = new SelectList(db.crLogDrivers, "Id", "Name", crLogTrip.crLogDriverId);
            ViewBag.crLogUnitId = new SelectList(db.crLogUnits, "Id", "Description", crLogTrip.crLogUnitId);
            ViewBag.crLogCompanyId = new SelectList(db.crLogCompanies, "Id", "Name", crLogTrip.crLogCompanyId);
            //ViewBag.crLogClosingId = new SelectList(db.crLogClosings, "Id", "Id", crLogTrip.crLogClosingId);
            return View(crLogTrip);
        }

        // GET: Personel/CarRentalLog/Delete/5
        public ActionResult Delete(int? id)
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
            return View(crLogTrip);
        }

        // POST: Personel/CarRentalLog/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            crLogTrip crLogTrip = db.crLogTrips.Find(id);
            db.crLogTrips.Remove(crLogTrip);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult DriverSummary(int id)
        {
            crLogDriver driver = db.crLogDrivers.Find(id);
            List<crLogTrip> trips = db.crLogTrips.Where(d => d.crLogDriverId == id && d.crLogClosingId==null).ToList();
            List<crLogCashRelease> cashtrx = db.crLogCashReleases.Where(d => d.crLogDriverId == id && d.crLogClosingId == null).ToList();

            crDriverSummary driversummary = new crDriverSummary();
            driversummary.Driver = driver;
            driversummary.DriverTrips = trips;
            driversummary.DriverCash = cashtrx;

            return View(driversummary);
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
            foreach(var item in cashtrx)
            {
                _cashAmt += item.Amount;
            }
            _paytoCloseAmt = _tripAmt - _cashAmt;

            if (_paytoCloseAmt != amount)
                sErrorMessage = "Amount is not valid to close the transactions.";


            int closingId = this.generateClosingTrx();
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
            catch(Exception e)
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

        int generateClosingTrx()
        {
            crLogClosing ctrx = new crLogClosing()
            { dtClose = System.DateTime.Now };

            db.crLogClosings.Add(ctrx);
            db.SaveChanges();

            return ctrx.Id;
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
