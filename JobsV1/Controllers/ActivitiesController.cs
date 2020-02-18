using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JobsV1.Models.Class;
using JobsV1.Models;

namespace JobsV1.Controllers
{
    public class ActivitiesController : Controller
    {
        private ActivityClass ac = new ActivityClass();
        private JobDBContainer db = new JobDBContainer();
        private DateClass dt = new DateClass();

        // GET: Activities
        public ActionResult Index(string sdate, string edate)
        {
            var user = HttpContext.User.Identity.Name;
            var date1 = DateTime.Parse(sdate);
            var date2 = DateTime.Parse(edate);


            if (User.IsInRole("Admin"))
            {
                //get activities of all users on Supplier
                ViewBag.SupplierActivities = ac.GetSupplierActivitiesAdmin(date1, date2);

                //get activities of all users on Companies
                var companyActivity = ac.GetCompanyActivitiesAdmin(date1,date2);
                return View(companyActivity);
            }
            else
            {
                //get activities of the user on Supplier
                ViewBag.SupplierActivities = ac.GetSupplierActivitiesUser(user, date1, date2);

                //get activities of the user on Companies
                var companyActivity = ac.GetCompanyActivitiesUser(user, date1, date2);
                return View(companyActivity);
            }
        }
        

    }
}