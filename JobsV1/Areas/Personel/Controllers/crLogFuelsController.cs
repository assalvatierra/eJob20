﻿using System;
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

        // GET: Personel/crLogFuels
        public ActionResult Index(int? statusId)
        {
            var today = dt.GetCurrentDate();
            var DateFilter = today.AddDays(-7);
            var crLogFuels = db.crLogFuels.Include(c => c.crLogUnit).Include(c => c.crLogDriver).OrderBy(c => c.dtRequest)
                .Where(c => DbFunctions.TruncateTime(c.dtRequest) > DateFilter);

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

            return View(cCrLogFuel.ToList());
        }

        // GET: Personel/crLogFuels
        public ActionResult PrevRecords()
        {
            var today = dt.GetCurrentDate();
            var crLogFuels = db.crLogFuels.Include(c => c.crLogUnit).Include(c => c.crLogDriver)
                .Where(c => DbFunctions.TruncateTime(c.dtFillup) < today).OrderBy(c => c.dtRequest);

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

                cCrLogFuel.Add(templog);
            }


            ViewBag.IsAdmin = User.IsInRole("Admin");

            return View(cCrLogFuel.ToList());
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

            ViewBag.crLogUnitId = new SelectList(db.crLogUnits, "Id", "Description");
            ViewBag.crLogDriverId = new SelectList(db.crLogDrivers, "Id", "Name");
            ViewBag.crLogTypeId = new SelectList(db.crLogTypes, "Id", "Type");
            return View(logFuel);
        }

        // POST: Personel/crLogFuels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,dtRequest,Amount,crLogUnitId,crLogDriverId,dtFillup,odoFillup,orAmount,crLogTypeId")] crLogFuel crLogFuel)
        {
            if (ModelState.IsValid)
            {
                db.crLogFuels.Add(crLogFuel);
                db.SaveChanges();

                //add status logs, REQUEST
                AddLogStatus(crLogFuel.Id, 1);

                return RedirectToAction("Index");
            }

            ViewBag.crLogUnitId = new SelectList(db.crLogUnits, "Id", "Description", crLogFuel.crLogUnitId);
            ViewBag.crLogDriverId = new SelectList(db.crLogDrivers, "Id", "Name", crLogFuel.crLogDriverId);
            ViewBag.crLogTypeId = new SelectList(db.crLogTypes, "Id", "Type", crLogFuel.crLogTypeId);
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
            ViewBag.crLogUnitId = new SelectList(db.crLogUnits, "Id", "Description", crLogFuel.crLogUnitId);
            ViewBag.crLogDriverId = new SelectList(db.crLogDrivers, "Id", "Name", crLogFuel.crLogDriverId);
            ViewBag.crLogTypeId = new SelectList(db.crLogTypes, "Id", "Type", crLogFuel.crLogTypeId);
            return View(crLogFuel);
        }

        // POST: Personel/crLogFuels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,dtRequest,Amount,crLogUnitId,crLogDriverId,dtFillup,odoFillup,orAmount,crLogTypeId")] crLogFuel crLogFuel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(crLogFuel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.crLogUnitId = new SelectList(db.crLogUnits, "Id", "Description", crLogFuel.crLogUnitId);
            ViewBag.crLogDriverId = new SelectList(db.crLogDrivers, "Id", "Name", crLogFuel.crLogDriverId);
            ViewBag.crLogTypeId = new SelectList(db.crLogTypes, "Id", "Type", crLogFuel.crLogTypeId);
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
            ViewBag.crLogUnitId = new SelectList(db.crLogUnits, "Id", "Description", crLogFuel.crLogUnitId);
            ViewBag.crLogDriverId = new SelectList(db.crLogDrivers, "Id", "Name", crLogFuel.crLogDriverId);
            ViewBag.crLogTypeId = new SelectList(db.crLogTypes, "Id", "Type", crLogFuel.crLogTypeId);
            return View(crLogFuel);
        }

        // POST: Personel/crLogFuels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Return([Bind(Include = "Id,dtRequest,Amount,crLogUnitId,crLogDriverId,dtFillup,odoFillup,orAmount,crLogTypeId")] crLogFuel crLogFuel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(crLogFuel).State = EntityState.Modified;
                db.SaveChanges();

                //add status logs, RETURNED
                AddLogStatus(crLogFuel.Id, 4);

                return RedirectToAction("Index", new { statusId = 3 });
            }
            ViewBag.crLogUnitId = new SelectList(db.crLogUnits, "Id", "Description", crLogFuel.crLogUnitId);
            ViewBag.crLogDriverId = new SelectList(db.crLogDrivers, "Id", "Name", crLogFuel.crLogDriverId);
            ViewBag.crLogTypeId = new SelectList(db.crLogTypes, "Id", "Type", crLogFuel.crLogTypeId);
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
        public bool SubmitReturnLog(int? id, string date, int odo, decimal amount)
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

                //save changes
                db.Entry(crLogFuel).State = EntityState.Modified;
                db.SaveChanges();

                //add odo log
                AddOdoLog(crLogFuel);

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

            return View(crlogFuel);
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
