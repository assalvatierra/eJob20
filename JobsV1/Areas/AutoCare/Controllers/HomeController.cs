using JobsV1.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JobsV1.Areas.AutoCare.Controllers
{
    public class HomeController : Controller
    {
        private JobDBContainer db = new JobDBContainer();

        // GET: AutoCare/Home
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Home", new { area = "" });
        }

        // GET: AutoCare/Dashboard
        public ActionResult Dashboard()
        {
            //get jobs per day for the last 7 days

            return View();
        }

        public ActionResult Error()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetJobsCount()
        {
            DateTime start = new DateTime(2020, 7, 20);
            DateTime end = new DateTime(2020, 8, 20);
            int daySpan = 30;

            //var jobList = db.JobMains.GroupBy(c => DbFunctions.TruncateTime(c.JobDate))
            //    .Select(d => new dash_JobsCount()
            //    {
            //        Date = ((DateTime)d.Key).ToString(),
            //        Total =  d.Count()
            //    });

            List<dash_JobsCount> jobsPerDay = new List<dash_JobsCount>();

            for(var day= 0 ; day < daySpan; day++)
            {
                var current_Date = start.AddDays(day);

                dash_JobsCount job = new dash_JobsCount();
                job.Date = current_Date.ToString();
                job.Total = db.JobMains.Where(j => ((DateTime)(DbFunctions.TruncateTime(j.JobDate))).CompareTo(current_Date) == 0).Count();

                jobsPerDay.Add(job);
            }

            return Json(jobsPerDay, JsonRequestBehavior.AllowGet);
        }

        private class dash_JobsCount
        {
            public string Date { get; set; }
            public int Total { get; set; }
        }

    }

}