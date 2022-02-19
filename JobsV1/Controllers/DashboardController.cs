using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JobsV1.Models;
using JobsV1.Models.Class;

namespace JobsV1.Controllers
{
    public class DashboardController : Controller
    {
        DateClass dt = new DateClass();
        DashboardServices services = new DashboardServices();

        // GET: Dashboard
        [Authorize]
        public ActionResult Index()
        {
            var today = dt.GetCurrentDate();

            ViewBag.Today = today.ToString("MMMM dd yyyy");
            ViewBag.Month = today.ToString("MMMM");
            ViewBag.Years = dt.GetYearsList();
            ViewBag.Menu = services.GetUserMenu(GetUser());
            return View();
        }

        private string GetUser()
        {
            return HttpContext.User.Identity.Name;
        }

        #region API
        //GET: Dashboard/GetSalesThisMonth
        [HttpGet]
        public JsonResult GetSalesThisMonth()
        {
            var today = dt.GetCurrentDate();

            decimal sales = services.GetSalesByMonthNo(today.Month, today.Year);

            return Json(sales.ToString("#,##0"), JsonRequestBehavior.AllowGet);
        }

        //GET: Dashboard/GetExpensesThisMonth
        [HttpGet]
        public JsonResult GetExpensesThisMonth()
        {
            var today = dt.GetCurrentDate();

            decimal expenses = services.GetExpensesByMonthNo(today.Month, today.Year);

            return Json(expenses.ToString("#,##0"), JsonRequestBehavior.AllowGet);
        }

        //GET: Dashboard/GetActiveJobs
        [HttpGet]
        public JsonResult GetActiveJobsCount()
        {
            int activejobsCount = services.GetActiveJobsToday();

            return Json(activejobsCount, JsonRequestBehavior.AllowGet);
        }

        //GET: Dashboard/GetActiveJobs
        [HttpGet]
        public JsonResult GetActiveJobs()
        {
            var activejobs = services.GetJobOrders();

            return Json(activejobs, JsonRequestBehavior.AllowGet);
        }

        //GET: Dashboard/GetActiveTriplogs
        [HttpGet]
        public JsonResult GetActiveTriplogs()
        {
            var tripLogs = services.GetTripLogsToday();

            return Json(tripLogs, JsonRequestBehavior.AllowGet);
        }

        //GET: Dashboard/GetActiveExpenses
        [HttpGet]
        public JsonResult GetActiveExpenses()
        {
            var expenses = services.GetPayablesToday();

            return Json(expenses, JsonRequestBehavior.AllowGet);
        }

        //GET: Dashboard/GetActiveReceivables
        [HttpGet]
        public JsonResult GetActiveReceivables()
        {
            var receivables = services.GetReceivablesToday();

            return Json(receivables, JsonRequestBehavior.AllowGet);
        }

        //GET: Dashboard/GetNotifications
        [HttpGet]
        public JsonResult GetNotifications()
        {
            var notifications = services.GetNotifications();

            return Json(notifications, JsonRequestBehavior.AllowGet);
        }

        //GET: Dashboard/GetNotifications
        [HttpGet]
        public JsonResult GetChartData()
        {
            var monthlyJobs = services.GetMonthlyJobsCount();

            return Json(monthlyJobs, JsonRequestBehavior.AllowGet);
        }

        //GET: Dashboard/GetNotifications
        [HttpGet]
        public JsonResult GetChartData_TripLogs(int? month, int? year)
        {
            if(month == null)
            {
                month = dt.GetCurrentDate().Month;
            }

            if (year == null)
            {
                year = dt.GetCurrentDate().Year;
            }
            var monthlyJobs = services.GetMonthlyTripLogs((int)month,(int)year);

            return Json(monthlyJobs, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}