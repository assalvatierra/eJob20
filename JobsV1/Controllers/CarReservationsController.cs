﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using JobsV1.Models;
using Microsoft.Ajax.Utilities;

namespace JobsV1.Controllers
{
    public class CarReservationsController : Controller
    {
        // NEW CUSTOMER Reference ID
        private int NewCustSysId = 1;

        // Job Status
        //private int JOBINQUIRY = 1;
        //private int JOBRESERVATION = 2;
        private int JOBCONFIRMED = 3;
        //private int JOBCLOSED = 4;
        //private int JOBCANCELLED = 5;
        //private int JOBTEMPLATE = 6;

        private JobDBContainer db = new JobDBContainer();
        private DBClasses dbc = new DBClasses();

        private string SITECONFIG = ConfigurationManager.AppSettings["SiteConfig"].ToString();

        // GET: CarReservations
        public ActionResult Index(int? filter)
        {

            ViewBag.PackageList = db.CarRateUnitPackages.ToList(); 
            var carReservations = db.CarReservations.Where(c=>c.JobRefNo == 0).Include(c => c.CarUnit).Include(c=>c.CarResPackages).ToList();
            
            //add 7 days allowance 
            DateTime today = DateTime.Today.AddDays(-7);
            today = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(today, TimeZoneInfo.Local.Id, "Singapore Standard Time");
            
            switch (filter)
            {
                case 1: //OnGoing
                    carReservations = carReservations.Where(c => DateTime.Compare(DateTime.Parse(c.DtStart), today) >= 0).ToList();   //get 1 month before all entries

                    break;
                case 2: //prev
                    today = DateTime.Today;
                    carReservations = carReservations.Where(c => DateTime.Compare(DateTime.Parse(c.DtStart), today) < 0).ToList();

                    break;
                default:
                    carReservations = carReservations.Where(c => DateTime.Compare(DateTime.Parse(c.DtStart), today) >= 0).ToList();   //get 1 month before all entries
                    break;
            }
            
            return View(carReservations.ToList());
        }

        // GET: CarReservations/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CarReservation cr = db.CarReservations.Find(id);
            if (cr == null)
            {
                return HttpNotFound();
            }


            //fillout empty
            cr.LocStart = cr.LocStart == null ? "N/A" : cr.LocStart;
            cr.LocEnd = cr.LocEnd == null ? "N/A" : cr.LocEnd;
            cr.RenterCompany = cr.RenterCompany == null ? "N/A" : cr.RenterCompany;
            cr.RenterAddress = cr.RenterAddress == null ? "N/A" : cr.RenterAddress;
            cr.RenterFbAccnt = cr.RenterFbAccnt == null ? "N/A" : cr.RenterFbAccnt;
            cr.RenterLinkedInAccnt = cr.RenterLinkedInAccnt == null ? "N/A" : cr.RenterLinkedInAccnt;
            ViewBag.rentalType = cr.SelfDrive == 1 ? "With Driver" : "Self Drive";

            return View(cr);
        }

        // GET: CarReservations/Create
        public ActionResult Create(int? id)
        {
            
            ViewBag.CarUnitId = new SelectList(db.CarUnits, "Id", "Description", id);
            return View();
        }

        // POST: CarReservations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,DtTrx,CarUnitId,DtStart,LocStart,DtEnd,LocEnd,BaseRate,Destinations,UseFor,RenterName,RenterCompany,RenterEmail,RenterMobile,RenterAddress,RenterFbAccnt,RenterLinkedInAccnt,EstHrPerDay,EstKmTravel")] CarReservation carReservation)
        {
            if (ModelState.IsValid)
            {
                db.CarReservations.Add(carReservation);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CarUnitId = new SelectList(db.CarUnits, "Id", "Description", carReservation.CarUnitId);
            return View(carReservation);
        }

        // GET: CarReservations/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CarReservation carReservation = db.CarReservations.Find(id);
            if (carReservation == null)
            {
                return HttpNotFound();
            }
            ViewBag.CarUnitId = new SelectList(db.CarUnits, "Id", "Description", carReservation.CarUnitId);
            return View(carReservation);
        }

        // POST: CarReservations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,DtTrx,CarUnitId,DtStart,LocStart,DtEnd,LocEnd,BaseRate,Destinations,UseFor,RenterName,RenterCompany,RenterEmail,RenterMobile,RenterAddress,RenterFbAccnt,RenterLinkedInAccnt,EstHrPerDay,EstKmTravel")] CarReservation carReservation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(carReservation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CarUnitId = new SelectList(db.CarUnits, "Id", "Description", carReservation.CarUnitId);
            return View(carReservation);
        }

        // GET: CarReservations/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CarReservation carReservation = db.CarReservations.Find(id);
            if (carReservation == null)
            {
                return HttpNotFound();
            }
            return View(carReservation);
        }

        // POST: CarReservations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CarReservation carReservation = db.CarReservations.Find(id);
            db.CarReservations.Remove(carReservation);
            DeleteCarResPackages(carReservation.Id);
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

        public void DeleteCarResPackages(int ReservationId) {
            CarResPackage carResReservation = db.CarResPackages.Where(c=>c.CarReservationId == ReservationId).FirstOrDefault();
            db.CarResPackages.Remove(carResReservation);
            db.SaveChanges();
        }


        public ActionResult ReservationRedirect(int id, string month, string day, string year, string rName)
        {
            String DateBook = month + "/" + day + "/" + year;
            DateTime booking = DateTime.Parse(DateBook);
            int iMonth = int.Parse(month);
            int iday = int.Parse(day);
            int iyear = int.Parse(year);

            CarReservation job = db.CarReservations.
                Where(j => j.Id == id).Where(j => j.DtTrx.Month == iMonth && j.DtTrx.Day == iday && j.DtTrx.Year == iyear).
                Where(j=>j.RenterName == rName).
                FirstOrDefault();

            //fillout empty
            job.LocStart      = job.LocStart      == null ? "N/A" : job.LocStart;
            job.LocEnd        = job.LocEnd        == null ? "N/A" : job.LocEnd;
            job.RenterCompany = job.RenterCompany == null ? "N/A" : job.RenterCompany;
            job.RenterAddress = job.RenterAddress == null ? "N/A" : job.RenterAddress;
            job.RenterFbAccnt = job.RenterFbAccnt == null ? "N/A" : job.RenterFbAccnt;
            job.RenterLinkedInAccnt = job.RenterLinkedInAccnt == null ? "N/A" : job.RenterLinkedInAccnt;

            ViewBag.rentalType = job.SelfDrive == 1 || job.SelfDrive == null ? "Self Drive" : "With Driver";

            return View("Details", job);
        }


        // GET: CarReservations/CreateQuotation
        public ActionResult CreateQuotation(int? id, int rsvId, string dtStart,string dtEnd,string renter,string details, int rentType, int unit, decimal rate, string renterNum, string renterEmail)
        {

            DateTime today = DateTime.Today.AddDays(1);
            today = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(today, TimeZoneInfo.Local.Id, "Singapore Standard Time");
            string unitdesc = db.CarUnits.Find(unit).Description;
            DateTime DtStart = DateTime.Parse(dtStart);
            DateTime DtEnd = DateTime.Parse(dtEnd);
            int days = (int)DtEnd.Subtract(DtStart).TotalDays;
           
            JobMain job = new JobMain();
            job.JobDate = today;
            job.NoOfDays = days == 0 ? 1 : days ;
            job.NoOfPax = 1;
            job.Description = renter + " - " +  details;
            job.JobRemarks = rentType == 0 ? unitdesc + " - With Driver" : unitdesc + " - Self Drive";
            job.AgreedAmt = rate;
            job.CustContactEmail = renterEmail;
            job.CustContactNumber = renterNum;


            if (id == null)
            {
                ViewBag.CustomerId = new SelectList(db.Customers.Where(d => d.Status == "ACT"), "Id", "Name", NewCustSysId);
            }
            else
            {
                ViewBag.CustomerId = new SelectList(db.Customers.Where(d => d.Status == "ACT"), "Id", "Name", id);
            }

            ViewBag.CompanyList = db.CustEntMains.OrderBy(s => s.Name).ToList() ?? new List<CustEntMain>();
            ViewBag.CustomerList = db.Customers.Where(s => s.Status == "ACT").OrderBy(s => s.Name).ToList() ?? new List<Customer>();
            ViewBag.CompanyId = new SelectList(db.CustEntMains, "Id", "Name");
            ViewBag.BranchId = new SelectList(db.Branches, "Id", "Name", 2);
            ViewBag.JobStatusId = new SelectList(db.JobStatus, "Id", "Status", JOBCONFIRMED);
            ViewBag.JobThruId = new SelectList(db.JobThrus, "Id", "Desc");
            ViewBag.AssignedTo = new SelectList(dbc.getUsers_wdException(), "UserName", "UserName");
            ViewBag.JobPaymentStatusId = new SelectList(db.JobPaymentStatus, "Id", "Status", 2);
            ViewBag.SiteConfig = SITECONFIG;

            return View(job);
        }


        // POST: CarReservations/CreateQuotation
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult jobCreate([Bind(Include = "Id,JobDate,CustomerId,Description,NoOfPax,NoOfDays,AgreedAmt,JobRemarks,JobStatusId,StatusRemarks,BranchId,JobThruId,CustContactEmail,CustContactNumber,AssignedTo")] JobMain jobMain, int? CompanyId, int? JobPaymentStatusId, int rsvid)
        {
            //if (ModelState.IsValid)
            //{
            //    if (jobMain.CustContactEmail == null && jobMain.CustContactNumber == null)
            //    {
            //        var cust = db.Customers.Find(jobMain.CustomerId);
            //        jobMain.CustContactEmail = cust.Email;
            //        jobMain.CustContactNumber = cust.Contact1;
            //    }

            //    db.JobMains.Add(jobMain);
            //    db.SaveChanges();

            //    //link job to reservation
            //    linkQuotation(rsvid, jobMain.Id);

            //    dbc.addEncoderRecord("joborder-reservation", jobMain.Id.ToString(), HttpContext.User.Identity.Name, "Create New Job for Reservation");
                
            //    return RedirectToAction("Index", "JobOrder", new { sortid = 1 });

            //}

            if (ModelState.IsValid)
            {
                if (JobCreateValidation(jobMain))
                {

                    if (jobMain.CustContactEmail == null && jobMain.CustContactNumber == null)
                    {
                        var cust = db.Customers.Find(jobMain.CustomerId);
                        jobMain.CustContactEmail = cust.Email;
                        jobMain.CustContactNumber = cust.Contact1;
                    }

                    db.JobMains.Add(jobMain);
                    db.SaveChanges();


                    //link job to reservation
                    linkQuotation(rsvid, jobMain.Id);

                    if (CompanyId != null)
                    {
                        AddjobCompany(jobMain.Id, (int)CompanyId);
                    }

                    if (JobPaymentStatusId != null)
                    {
                        AddJobPaymentStatus((int)JobPaymentStatusId, jobMain.Id);
                    }

                    dbc.addEncoderRecord("joborder", jobMain.Id.ToString(), HttpContext.User.Identity.Name, "Create New Job for Reservation");
                    return RedirectToAction("JobServices", "JobOrder", new { JobMainId = jobMain.Id });

                }
            }

            ViewBag.CompanyList = db.CustEntMains.ToList() ?? new List<CustEntMain>();
            ViewBag.CustomerList = db.Customers.Where(s => s.Status == "ACT").ToList() ?? new List<Customer>();
            ViewBag.CompanyId = new SelectList(db.CustEntMains, "Id", "Name");
            ViewBag.CustomerId = new SelectList(db.Customers.Where(d => d.Status != "INC"), "Id", "Name", jobMain.CustomerId);
            ViewBag.BranchId = new SelectList(db.Branches, "Id", "Name", jobMain.BranchId);
            ViewBag.JobStatusId = new SelectList(db.JobStatus, "Id", "Status", jobMain.JobStatusId);
            ViewBag.JobThruId = new SelectList(db.JobThrus, "Id", "Desc", jobMain.JobThruId);
            ViewBag.AssignedTo = new SelectList(dbc.getUsers(), "UserName", "UserName", jobMain.AssignedTo);
            ViewBag.JobPaymentStatusId = new SelectList(db.JobPaymentStatus, "Id", "Status", (int)JobPaymentStatusId);
            ViewBag.SiteConfig = SITECONFIG;


            return View(jobMain);
        }

        public int linkQuotation(int rsvId, int jobId)
        {
            try{

                CarReservation rsv = db.CarReservations.Find(rsvId);
                rsv.JobRefNo = jobId;

                db.Entry(rsv).State = EntityState.Modified;
                db.SaveChanges();
            }
            catch {
                return 1;
            }
            return 0;
        }


        public bool JobCreateValidation(JobMain jobMain)
        {
            bool isValid = true;

            if (jobMain.JobDate == null)
            {
                ModelState.AddModelError("JobDate", "Invalid JobDate");
                isValid = false;
            }

            if (jobMain.Description.IsNullOrWhiteSpace())
            {
                ModelState.AddModelError("Description", "Invalid Description");
                isValid = false;
            }


            if (jobMain.CustContactNumber.IsNullOrWhiteSpace())
            {
                ModelState.AddModelError("CustContactNumber", "Invalid Contact Number");
                isValid = false;
            }
            else
            {
                if (jobMain.CustContactNumber.Length < 11)
                {
                    ModelState.AddModelError("CustContactNumber", "Invalid Contact Number");
                    isValid = false;
                }

            }
            return isValid;
        }

        public void AddjobCompany(int jobId, int companyId)
        {
            JobEntMain jobCompany = new JobEntMain();
            jobCompany.JobMainId = jobId;
            jobCompany.CustEntMainId = companyId;

            db.JobEntMains.Add(jobCompany);
            db.SaveChanges();
        }

        public bool AddJobPaymentStatus(int id, int jobId)
        {
            try
            {
                JobMainPaymentStatus paymentStatus = new JobMainPaymentStatus();
                paymentStatus.JobMainId = jobId;
                paymentStatus.JobPaymentStatusId = id;

                db.JobMainPaymentStatus.Add(paymentStatus);
                db.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        [HttpGet]
        public JsonResult GetUnitImages(int? unitId)
        {
            try
            {
                if (unitId != null)
                {
                    var carUnitImages = db.CarImages.Where(c => c.CarUnitId == unitId && c.SysCode == "DESC")
                        .ToList().Select(c => c.ImgUrl);

                    return Json(carUnitImages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(null, JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

    }
}
