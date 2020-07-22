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

        // GET: Personel/crLogFuels
        public ActionResult Index(int? statusId)
        {
            var today = dt.GetCurrentDate();
            var crLogFuels = db.crLogFuels.Include(c => c.crLogUnit).Include(c => c.crLogDriver)
                .Where(c=> DbFunctions.TruncateTime(c.dtFillup) >= today).OrderBy(c => c.dtRequest);


            if (statusId == null)
            {
                statusId = 1;
            }


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

                if(templog.LatestStatusId == statusId)
                    cCrLogFuel.Add(templog);
            }


            //check user permission
            var isAdmin = false;
            if (User.IsInRole("Admin"))
            {
                isAdmin = true;
            }

            ViewBag.IsAdmin = true;
            ViewBag.StatusId = statusId;

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

                //add status logs, REQUEST
                AddLogStatus(crLogFuel.Id, 3);

                return RedirectToAction("Index");
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
            crLogFuel crLogFuel = db.crLogFuels.Find(id);
            db.crLogFuels.Remove(crLogFuel);
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
            catch (Exception ex)
            {
                throw ex;
                //return false;
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
            catch (Exception ex) 
            { 
                throw ex; 
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
    }
}
