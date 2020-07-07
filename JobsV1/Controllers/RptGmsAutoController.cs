using JobsV1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JobsV1.Controllers
{
    public class RptGmsAutoController : Controller
    {
        private JobDBContainer db = new JobDBContainer();

        // GET: RptGmsAuto
        public ActionResult Index()
        {

            return View();
        }

        // GET: RptGmsAuto
        public ActionResult PageNotFound()
        {

            return View();
        }

        // Repair Order
        // id : jobMainId
        public ActionResult RepairOrder(int? id)
        {
            if (id == null)
                return RedirectToAction("PageNotFound");

            var jobmain = db.JobMains.Find(id);
            if (jobmain == null)
                return RedirectToAction("PageNotFound");

            var jobVehicle = db.JobVehicles.Where(j => j.JobMainId == id).OrderByDescending(j => j.Id).FirstOrDefault();
            if (jobVehicle == null)
                jobVehicle = new JobVehicle();

            var vehicleServiceHistory = db.JobVehicles.Where(j => j.VehicleId == jobVehicle.VehicleId && j.JobMainId != id).ToList();

            var jobServices = db.JobServices.Where(j => j.JobMainId == id).OrderBy(j=>j.DtStart).ToList();
          
            var company = jobmain.JobEntMains.Where(s => s.JobMainId == id).FirstOrDefault() ?? null;
            ViewBag.Company = company != null ? company.CustEntMain.Name : "Personal Account";
            ViewBag.CompanyAddress = company != null ? company.CustEntMain.Address : "-";
            ViewBag.JobVehicle = jobVehicle;
            ViewBag.VehicleServiceHistory = vehicleServiceHistory;
            ViewBag.JobServices = jobServices;

            return View(jobmain);
        }
    }
}