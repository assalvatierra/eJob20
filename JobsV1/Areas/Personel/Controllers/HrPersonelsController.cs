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
        private PersonnelClass pc = new PersonnelClass();
        private List<SelectListItem> StatusList = new List<SelectListItem> {
                new SelectListItem { Value = "ACT", Text = "Active" },
                new SelectListItem { Value = "INC", Text = "Inactive" },
                new SelectListItem { Value = "BAD", Text = "Bad Account" }
                };

        // GET: Personel/HrPersonels
        public ActionResult Index()
        {
            return View(pc.getPersonnelList().ToList());
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
            PartialView_Trainings((int)id);

            return View(hrPersonel);
        }

        //display list of categories assigned 
        //to the customer 
        private void PartialView_Profile(int id)
        {
            HrProfile profile = new HrProfile();
            
            //check if profile record exist
            if (db.HrProfiles.Where(h => h.HrPersonelId == id).FirstOrDefault() != null)
            {
                //if exists
                profile =  db.HrProfiles.Where(h => h.HrPersonelId == id).FirstOrDefault();
            }
            else
            {
                //if exists, add profile

                profile.LastName = "NA";
                profile.MiddleName = "NA";
                profile.FirstName = "NA";
                profile.Email = "NA";
                profile.HrPersonelId = id;
                //profile.PresentAddress = "";
                //profile.ProvincialAddress = "";
                //profile.Spouse = "";
                //profile.Mobile1 = "";
                //profile.Mobile2 = "";
                //profile.fbAccount = "";

                db.HrProfiles.Add(profile);
                db.SaveChanges();

                //if personnel have NO existing record proceed to CREATE
                //RedirectToAction("Create", new { pId = id });
            }
            //get list of categories
            ViewBag.profile = profile;
        }

        //display list of categories assigned 
        //to the customer 
        private void PartialView_Position(int id)
        {
            //get list of position
            ViewBag.perPosition = db.HrPerPositions.Where(h => h.HrPersonelId == id).ToList().OrderByDescending(h => h.DtStart);
            ViewBag.PositionList = db.HrPositions.ToList();

            //get latest position (string)
            ViewBag.latestPosition = pc.getPositionbyId(id);
        }

        //display list of Skills and Proficiency assigned 
        //to the customer 
        private void PartialView_Skills(int id)
        {
            //get list of acquired skills linked to the personnel by its Id
            List<HrPerSkill> perSkillsList = db.HrPerSkills.Where(h => h.HrPersonelId == id).ToList();

            //get list of IDs of acquired skill 
            var acquiredskillsID = perSkillsList.Select(s => s.HrSkillId).ToList();

            //get list of available skills not acquired by the personnel
            List<HrSkill> availableSkills = db.HrSkills.Where(h => !acquiredskillsID.Contains(h.Id)).ToList();

            //get list of categories
            ViewBag.perSkills = perSkillsList.OrderByDescending(h => h.DtAcquired);
            ViewBag.SkillsList = availableSkills;
            ViewBag.Proficiency = db.HrProficiencies.ToList();
        }

        //display list of trainings assigned 
        //to the customer 
        private void PartialView_Trainings(int id)
        {
            //get list of trainings linked to personnel by its Id
            List<HrPerTraining> perTrainingsList = db.HrPerTrainings.Where(h => h.HrPersonelId == id).ToList();

            //get list IDs of acquired trainings
            var acquiredTrainings = perTrainingsList.Select(s => s.HrTrainingId).ToList();

            //get list of available trainings not linked to the acquired trainings of the personnel
            List<HrTraining> availableTrainings = db.HrTrainings.Where(h => !acquiredTrainings.Contains(h.Id)).ToList();

            //get list of categories
            ViewBag.perTrainings = perTrainingsList.OrderByDescending(h => h.DtCompleted);
            ViewBag.TrainingsList = availableTrainings;

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
                return RedirectToAction("Details" , new { id = hrPersonel.Id });
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

        #region Profile
        public string EditProfile(int? id, string fname, string lname, string midname, string mobile1, string mobile2,
            string email, string fbaccount, string presAddress, string provAddress, string spouse)
        {
            if (id != null)
            {

                HrProfile profile = db.HrProfiles.Where(s => s.HrPersonelId == id).FirstOrDefault();
                profile.FirstName = fname;
                profile.LastName = lname;
                profile.MiddleName = midname;
                profile.Mobile1 = mobile1;
                profile.Mobile2 = mobile2;
                profile.Email = email;
                profile.fbAccount = fbaccount;
                profile.PresentAddress = presAddress;
                profile.ProvincialAddress = provAddress;
                profile.Spouse = spouse;

                db.Entry(profile).State = EntityState.Modified;
                db.SaveChanges();
                // return RedirectToAction("Details", new { id = profile.HrPersonelId });
                //return "ok";

                return "id :" + id;
            }

            return "error";

        }
        #endregion

        #region Position
        public string AddPosition(int? id, int pId, string date)
        {
            if (id != null)
            {
                HrPerPosition perPosition = new HrPerPosition();
                perPosition.HrPersonelId = pId;
                perPosition.HrPositionId = (int)id;
                perPosition.DtStart = DateTime.Parse(date);

                db.HrPerPositions.Add(perPosition);
                db.SaveChanges();
                return "ok";
                //return RedirectToAction("Details", new { id = pId }); //view in personnel details
            }
            //return RedirectToAction("Index");
            return "ok";
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
        [HttpPost]
        public string AddSkills(int perID, int sID, int pID, string date)
        {
            HrPerSkill perSkills = new HrPerSkill();
            perSkills.HrPersonelId = perID;
            perSkills.HrSkillId = sID;
            perSkills.HrProficiencyId = pID;
            perSkills.DtAcquired = DateTime.Parse(date);

            db.HrPerSkills.Add(perSkills);
            db.SaveChanges();

            return "OK"; //view in personnel details

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

        #region Trainings
        public ActionResult AddTraining(int id, int pId, string date)
        {
            HrPerTraining perTraining = new HrPerTraining();
            perTraining.HrPersonelId  = pId;
            perTraining.HrTrainingId  = id;
            perTraining.DtCompleted   = DateTime.Parse(date);

            db.HrPerTrainings.Add(perTraining);
            db.SaveChanges();

            return RedirectToAction("Details", new { id = pId }); //view in personnel details
        }

        public ActionResult RemoveTraining(int id, int pId)
        {
            HrPerTraining hrTraining = db.HrPerTrainings.Find(id);
            int perId = pId;
            db.HrPerTrainings.Remove(hrTraining);
            db.SaveChanges();

            return RedirectToAction("Details", new { id = perId }); //view in personnel details
        }

        #endregion

        #region Salary

        public ActionResult Salary(int? id)
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

            //get latest position (string)
            ViewBag.latestPosition = pc.getPositionbyId((int)id);

            PartialView_SSalary((int)id);
            PartialView_SPayroll((int)id);
            PartialView_SDTR((int)id);

            return View(hrPersonel);
        }

        public void PartialView_SSalary(int id)
        {
            ViewBag.salaryDetails = db.HrSalaries.Where(s => s.HrPersonelId == id).ToList().OrderByDescending(s => s.DtStart);
        }

        public void PartialView_SPayroll(int id)
        {
            ViewBag.payrollDetails = db.HrPayrolls.Where(s => s.HrPersonelId == id && s.Status == "ACT").ToList().OrderByDescending(s => s.DtStart);
        }

        public void PartialView_SDTR(int id)
        {
            ViewBag.dtrStatus  = db.HrDtrStatus.ToList();
            ViewBag.dtrList    = db.HrPayrolls.Where(s => s.HrPersonelId == id && s.Status == "ACT").OrderByDescending(s => s.DtStart).ToList();
            ViewBag.dtrDetails = db.HrDtrs.Where(s => s.HrPersonelId == id).ToList().OrderByDescending(s => s.DtrDate);
            ViewBag.rateList   = db.HrSalaries.Where(s => s.HrPersonelId == id).ToList();
        }
          
        public ActionResult AddSalary(int id, decimal Rate)
        {
            HrSalary perSalary = new HrSalary();
            perSalary.HrPersonelId = id;
            perSalary.RatePerHr = Rate;
            perSalary.DtStart = GetCurrentTime();

            db.HrSalaries.Add(perSalary);
            db.SaveChanges();

            return RedirectToAction("Salary", new { id = id }); //view in personnel details
        }

        public ActionResult RemoveSalary(int id)
        {
            HrSalary hrSalary = db.HrSalaries.Find(id);
            var perId = hrSalary.HrPersonelId;
            db.HrSalaries.Remove(hrSalary);
            db.SaveChanges();

            return RedirectToAction("Salary", new { id = perId }); //view in personnel details
        }
        #endregion

        #region Payroll
        public ActionResult AddPayroll(int perId, string DtStart, string DtEnd, decimal salary, decimal allw,
            decimal ded, int year, int mon, string status)
        {
            HrPayroll payroll = new HrPayroll();
            payroll.DtStart = DateTime.Parse(DtStart);
            payroll.DtEnd = DateTime.Parse(DtEnd);
            payroll.HrPersonelId = perId;
            payroll.Salary = salary;
            payroll.Allowance = allw;
            payroll.Deduction = ded;
            payroll.Yearno = year.ToString();
            payroll.Monthno = mon.ToString();
            payroll.Status = status;

            db.HrPayrolls.Add(payroll);
            db.SaveChanges();
            //return "500";
            return RedirectToAction("Salary", new { id = perId }); //view in personnel details
        }
        
        public ActionResult EditPayroll(int pId, int perId, string DtStart, string DtEnd, decimal salary, decimal allw,
            decimal ded, int year, int mon, string status)
        {
            HrPayroll payroll = db.HrPayrolls.Find(pId);
            payroll.DtStart = DateTime.Parse(DtStart);
            payroll.DtEnd = DateTime.Parse(DtEnd);
            payroll.HrPersonelId = perId;
            payroll.Salary = salary;
            payroll.Allowance = allw;
            payroll.Deduction = ded;
            payroll.Yearno = year.ToString();
            payroll.Monthno = mon.ToString();
            payroll.Status = status;

            db.Entry(payroll).State = EntityState.Modified;
            db.SaveChanges();
            //return "500";
            return RedirectToAction("Salary", new { id = perId }); //view in personnel details
        }

        public ActionResult RemovePayroll(int id)
        {
            HrPayroll hrPayroll = db.HrPayrolls.Find(id);
            hrPayroll.Status = "INC";

            int perId = hrPayroll.HrPersonelId;

            db.Entry(hrPayroll).State = EntityState.Modified;

            //db.HrPerTrainings.Remove(hrTraining);
            db.SaveChanges();

            return RedirectToAction("Salary", new { id = perId }); //view in personnel details
        }

        public decimal PayrollCalculate(int id)
        {
            HrPayroll payroll = db.HrPayrolls.Find(id);
            decimal totalSalary = 0;
            decimal totalAllowance = 0;
            decimal totalDeductions = 0;

            //get list of dtr with the payroll id
            var dtrList = db.HrDtrs.Where(d => d.HrPayrollId == payroll.Id).ToList();
            foreach (var dtr in dtrList)
            {
                decimal salaryToday = 0;
                salaryToday = dtr.HrSalary.RatePerHr * dtr.ActualHrs;
                salaryToday -= totalDeductions;
                //add to total salary
                totalSalary += salaryToday;
            }
            return totalSalary;
            //update payrollSalary

        }

        public ActionResult UpdatePayrollSalary(int payrollId) {
            decimal newSalary =  PayrollCalculate(payrollId);

            HrPayroll payroll = db.HrPayrolls.Find(payrollId);
            payroll.Salary = newSalary;
            db.Entry(payroll).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Salary", new { id = payroll.HrPersonelId }); //view in personnel details
        }


        #endregion

        #region DTR
        public string AddDTRecord(int pId, string date, string status,
            string timeIn, string timeOut, string actualHrs, string roundHrs, int salaryId ,string payrollID)
        {
            HrDtr dtr = new HrDtr();
            dtr.HrSalaryId = salaryId;
            dtr.DtrDate = DateTime.Parse(date);
            dtr.HrDtrStatusId = int.Parse(status);
            dtr.HrPersonelId = pId;
            dtr.TimeIn = TimeSpan.Parse(timeIn);
            dtr.TimeOut = TimeSpan.Parse(timeOut);
            dtr.ActualHrs = int.Parse(actualHrs);
            dtr.RoundHrs = int.Parse(roundHrs);
            dtr.HrPayrollId = int.Parse(payrollID);

            db.HrDtrs.Add(dtr);
            db.SaveChanges();

            //update salary
            UpdatePayrollSalary(int.Parse(payrollID));

            return "500";
            //return RedirectToAction("Details", new { id = id }); //view in personnel details
        }

        public ActionResult EditDTRecord(int Id, int PersonnelId, string date, string status,
            string timeIn, string timeOut, string actualHrs, string roundHrs , int salaryId, string payrollID)
        {
            HrDtr dtr =db.HrDtrs.Find(Id);
            dtr.HrSalaryId = salaryId;
            dtr.DtrDate = DateTime.Parse(date);
            dtr.HrDtrStatusId = int.Parse(status);
            dtr.HrPersonelId = PersonnelId;
            dtr.TimeIn = TimeSpan.Parse(timeIn);
            dtr.TimeOut = TimeSpan.Parse(timeOut);
            dtr.ActualHrs = int.Parse(actualHrs);
            dtr.RoundHrs = int.Parse(roundHrs);
            dtr.HrPayrollId = int.Parse(payrollID);

            //db.HrDtrs.Add(dtr);
            db.Entry(dtr).State = EntityState.Modified;
            db.SaveChanges();

            //update salary
            UpdatePayrollSalary(int.Parse(payrollID));

            //return "500";
            return RedirectToAction("Salary", new { id = PersonnelId }); //view in personnel details
        }

        public ActionResult RemoveDTR(int id)
        {
            HrDtr dtr = db.HrDtrs.Find(id);
            int perId = dtr.HrPersonelId;
            int payrollID = dtr.HrPayrollId;

            db.HrDtrs.Remove(dtr);
            db.SaveChanges();
            
            //update salary
            UpdatePayrollSalary(payrollID);

            return RedirectToAction("Salary", new { id = perId }); //view in personnel details
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
