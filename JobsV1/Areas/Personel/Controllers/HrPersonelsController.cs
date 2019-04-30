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
    public class HrPersonelsController : Controller
    {
        private HrisDBContainer db = new HrisDBContainer();

        // GET: Personel/HrPersonels
        public ActionResult Index()
        {
            var hrPersonels = db.HrPersonels.Include(h => h.HrPersonelStatu);
            return View(hrPersonels.ToList());
        }

        // GET: Personel/HrPersonels/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HrPersonel hrPersonel = db.HrPersonels.Find(id);
            if (hrPersonel == null)
            {
                return HttpNotFound();
            }

            PartialView_Profile((int)id);
            PartialView_Position((int)id);
            PartialView_Skills((int)id);

            return View(hrPersonel);
        }


        //display list of categories assigned 
        //to the customer 
        private void PartialView_Profile(int id)
        {
            //get list of categories
            ViewBag.profile = db.HrProfiles.Where(h => h.HrPersonelId == id).FirstOrDefault();
        }

        //display list of categories assigned 
        //to the customer 
        private void PartialView_Position(int id)
        {
            //get list of categories
            ViewBag.perPosition = db.HrPerPositions.Where(h => h.HrPersonelId == id).ToList().OrderByDescending(h=>h.DtStart);
            ViewBag.PositionList = db.HrPositions.ToList();
        }

        //display list of categories assigned 
        //to the customer 
        private void PartialView_Skills(int id)
        {
            //get list of categories
            ViewBag.perSkills = db.HrPerSkills.Where(h => h.HrPersonelId == id).ToList().OrderByDescending(h => h.DtAcquired);
            ViewBag.SkillsList = db.HrSkills.ToList();
            ViewBag.Proficiency = db.HrProficiencies.ToList();
        }

        // GET: Personel/HrPersonels/Create
        public ActionResult Create()
        {
            ViewBag.HrPersonelStatusId = new SelectList(db.HrPersonelStatus, "Id", "Desc");
            return View();
        }

        // POST: Personel/HrPersonels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,PassportNo,SSSid,Tin,DriverId,Remarks,HrPersonelStatusId")] HrPersonel hrPersonel)
        {
            if (ModelState.IsValid)
            {
                db.HrPersonels.Add(hrPersonel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.HrPersonelStatusId = new SelectList(db.HrPersonelStatus, "Id", "Desc", hrPersonel.HrPersonelStatusId);
            return View(hrPersonel);
        }

        // GET: Personel/HrPersonels/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HrPersonel hrPersonel = db.HrPersonels.Find(id);
            if (hrPersonel == null)
            {
                return HttpNotFound();
            }
            ViewBag.HrPersonelStatusId = new SelectList(db.HrPersonelStatus, "Id", "Desc", hrPersonel.HrPersonelStatusId);
            return View(hrPersonel);
        }

        // POST: Personel/HrPersonels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,PassportNo,SSSid,Tin,DriverId,Remarks,HrPersonelStatusId")] HrPersonel hrPersonel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(hrPersonel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.HrPersonelStatusId = new SelectList(db.HrPersonelStatus, "Id", "Desc", hrPersonel.HrPersonelStatusId);
            return View(hrPersonel);
        }

        // GET: Personel/HrPersonels/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HrPersonel hrPersonel = db.HrPersonels.Find(id);
            if (hrPersonel == null)
            {
                return HttpNotFound();
            }
            return View(hrPersonel);
        }

        // POST: Personel/HrPersonels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            HrPersonel hrPersonel = db.HrPersonels.Find(id);
            db.HrPersonels.Remove(hrPersonel);
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

        #region Position
        public ActionResult AddPosition(int? id,int pId)
        {
            if(id != null)
            {
                HrPerPosition perPosition = new HrPerPosition();
                perPosition.HrPersonelId = pId;
                perPosition.HrPositionId = (int)id;
                perPosition.DtStart = GetCurrentTime();

                db.HrPerPositions.Add(perPosition);
                db.SaveChanges();
                return RedirectToAction("Details", new { id = pId }); //view in personnel details
            }
            return RedirectToAction("Index");
        }

        public ActionResult RemovePosition(int id)
        {
            HrPerPosition hrPosition = db.HrPerPositions.Find(id);
            int perId = hrPosition.HrPersonelId;
            db.HrPerPositions.Remove(hrPosition);
            db.SaveChanges();

            return RedirectToAction("Details", new { id = perId }); //view in personnel details
        }

        #endregion


        #region Skills
        public ActionResult AddSkills(int? id, int profId, int pId)
        {
            if (id != null)
            {
                HrPerSkill perSkills = new HrPerSkill();
                perSkills.HrPersonelId = pId;
                perSkills.HrSkillId = (int)id;
                perSkills.HrProficiencyId = profId;
                perSkills.DtAcquired = GetCurrentTime();

                db.HrPerSkills.Add(perSkills);
                db.SaveChanges();

                return RedirectToAction("Details", new { id = pId }); //view in personnel details
            }
            return RedirectToAction("Index");
        }

        public ActionResult RemoveSkill(int id)
        {
            HrPerSkill hrSkill = db.HrPerSkills.Find(id);
            int perId = hrSkill.HrPersonelId;
            db.HrPerSkills.Remove(hrSkill);
            db.SaveChanges();

            return RedirectToAction("Details", new { id = perId }); //view in personnel details
        }

        #endregion


        //get current time based on Singapore Standard Time 
        //SGT - UTC +8
        public DateTime GetCurrentTime()
        {
            DateTime _localTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Singapore Standard Time"));
            _localTime = _localTime.Date;

            return _localTime;
        }
    }
}
