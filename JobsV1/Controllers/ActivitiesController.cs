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
        public ActionResult Index( )
        {
            if (User.IsInRole("Admin"))
            {
                //get activities of all users on Supplier
                ViewBag.SupplierActivities = GetSupplierActivitiesAdmin();

                //get activities of all users on Companies
                var companyActivity = db.CustEntActivities.ToList();
                return View(companyActivity.OrderByDescending(s=>s.Date));
            }
            else
            {
                //get activities of the user on Supplier
                ViewBag.SupplierActivities = GetSupplierActivitiesUser();

                //get activities of the user on Companies
                var companyActivity = db.CustEntActivities.Where(s=>s.Assigned == HttpContext.User.Identity.Name).ToList();
                return View(companyActivity.OrderByDescending(s => s.Date));
            }
        }
        

        // GET: Activities
        public IOrderedEnumerable<SupplierActivity> GetSupplierActivitiesUser()
        {
            //get activities of the user
            var companyActivity = db.SupplierActivities.Where(s => s.Assigned == HttpContext.User.Identity.Name).ToList();
            return companyActivity.OrderByDescending(s => s.DtActivity);
        }


        // GET: Activities
        public IOrderedEnumerable<SupplierActivity> GetSupplierActivitiesAdmin()
        {
            //get activities of all users
            var companyActivity = db.SupplierActivities.ToList();
            return companyActivity.OrderByDescending(s => s.DtActivity);
        }
    }
}