using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using JobsV1.Models;


namespace JobsV1.Controllers
{
    public class InvCarRecordsController : Controller
    {
        private JobDBContainer db = new JobDBContainer();

        // GET: InvCarRecords
        public ActionResult Index(int? unitId)
        {
            #region Session
            if (unitId != null)
            {
                Session["InvCarRecords-unitId"] = unitId;
            }
            else
            {
                if (Session["InvCarRecords-unitId"] != null)
                {
                    unitId = (int)Session["InvCarRecords-unitId"];
                }
            }
            #endregion

            //get records past their next odometer & schedule change
            //odometer
            List<InvCarRecord> priority = new List<InvCarRecord>();
           
           foreach (var carList in db.InvItems.Where(s => s.ViewLabel == "UNIT").ToList()) {

                var priorityRecords = db.InvCarRecords.Include(i => i.InvCarRecordType).Include(i => i.InvItem)
                    .Where(c=>c.InvItemId == carList.Id).OrderByDescending(s=>s.NextOdometer).FirstOrDefault();

                    priority.Add(priorityRecords);
            }

            ViewBag.InvItemsList = db.InvItems.Where(s => s.ViewLabel == "UNIT" && s.OrderNo == 100).ToList();
            ViewBag.recordTypeList = db.InvCarRecordTypes.ToList();
            ViewBag.priority = priority;
            ViewBag.UnitId = unitId;

            var invCarRecords = db.InvCarRecords.Include(i => i.InvCarRecordType).Include(i => i.InvItem).OrderByDescending(s=>s.dtDone);

            if (unitId != null && unitId != 0) {

                return View(invCarRecords.Where(s=>s.InvItemId == unitId).ToList());
            }
            else
            {
                return View(invCarRecords.ToList());

            }
        }

        // GET: InvCarRecords/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InvCarRecord invCarRecord = db.InvCarRecords.Find(id);
            if (invCarRecord == null)
            {
                return HttpNotFound();
            }
            return View(invCarRecord);
        }

        // GET: InvCarRecords/Create
        public ActionResult Create(int? carId)
        {
            InvCarRecord record = new InvCarRecord();
            if (carId != null)
            {
                record.InvCarRecordTypeId = 1;
                record.InvItemId = 1;
                record.dtDone = DateTime.Now;
                if(db.InvCarGateControls.Where(s => s.InvItemId == (int)carId).OrderByDescending(s => s.dtControl).FirstOrDefault() != null)
                    record.Odometer = int.Parse(db.InvCarGateControls.Where(s => s.InvItemId == (int)carId).OrderByDescending(s => s.dtControl).FirstOrDefault().Odometer);
            }

            var invItems = db.InvItems.Where(s => s.ViewLabel == "UNIT").Select(
                        s => new SelectListItem
                        {
                            Value = s.Id.ToString(),
                            Text = s.ItemCode.ToString() + " - " + s.Description
                        }
                 );

            ViewBag.InvCarRecordTypeId = new SelectList(db.InvCarRecordTypes, "Id", "Description");
            ViewBag.InvItemId = new SelectList(invItems, "Value", "Text", carId);
            return View(record);
        }
        

        // POST: InvCarRecords/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,InvItemId,InvCarRecordTypeId,Odometer,dtDone,NextOdometer,NextSched,Remarks")] InvCarRecord invCarRecord)
        {
            if (ModelState.IsValid)
            {
                //get record type
                InvCarRecordType recordType = db.InvCarRecordTypes.Where(r => r.Id == invCarRecord.InvCarRecordTypeId).FirstOrDefault();
                int estOdo = recordType.OdoInterval;
                int estDays = recordType.DaysInterval;

                //add estimated odometer and next schedule
                invCarRecord.NextOdometer = invCarRecord.Odometer +  estOdo;
                invCarRecord.NextSched = (DateTime)invCarRecord.dtDone.AddDays(estDays);

                db.InvCarRecords.Add(invCarRecord);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.InvCarRecordTypeId = new SelectList(db.InvCarRecordTypes, "Id", "Description", invCarRecord.InvCarRecordTypeId);
            ViewBag.InvItemId = new SelectList(db.InvItems, "Id", "ItemCode", invCarRecord.InvItemId);
            return View(invCarRecord);
        }

        // GET: InvCarRecords/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InvCarRecord invCarRecord = db.InvCarRecords.Find(id);
            if (invCarRecord == null)
            {
                return HttpNotFound();
            }
            ViewBag.InvCarRecordTypeId = new SelectList(db.InvCarRecordTypes, "Id", "Description", invCarRecord.InvCarRecordTypeId);
            ViewBag.InvItemId = new SelectList(db.InvItems, "Id", "ItemCode", invCarRecord.InvItemId);
            return View(invCarRecord);
        }

        // POST: InvCarRecords/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,InvItemId,InvCarRecordTypeId,Odometer,dtDone,NextOdometer,NextSched,Remarks")] InvCarRecord invCarRecord)
        {
            if (ModelState.IsValid)
            {
                
                db.Entry(invCarRecord).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.InvCarRecordTypeId = new SelectList(db.InvCarRecordTypes, "Id", "Description", invCarRecord.InvCarRecordTypeId);
            ViewBag.InvItemId = new SelectList(db.InvItems, "Id", "ItemCode", invCarRecord.InvItemId);
            return View(invCarRecord);
        }

        // GET: InvCarRecords/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InvCarRecord invCarRecord = db.InvCarRecords.Find(id);
            if (invCarRecord == null)
            {
                return HttpNotFound();
            }
            return View(invCarRecord);
        }

        // POST: InvCarRecords/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            InvCarRecord invCarRecord = db.InvCarRecords.Find(id);
            db.InvCarRecords.Remove(invCarRecord);
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

        [HttpGet]
        public JsonResult GetRecordNextSchedule(int? id)
        {
            var recordType = db.InvCarRecordTypes.Find(id);

            return Json(new { recordType.OdoInterval, recordType.DaysInterval },
                JsonRequestBehavior.AllowGet);
        }

    }
}
