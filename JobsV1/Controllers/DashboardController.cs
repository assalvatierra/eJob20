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
        public ActionResult Index()
        {
            return View();
        }

        #region API
        //GET: Dashboard/GetSalesThisMonth
        [HttpGet]
        public JsonResult GetSalesThisMonth()
        {
            var monthToday = dt.GetCurrentDate().Month;

            decimal sales = services.GetSalesByMonthNo(monthToday);

            return Json(sales, JsonRequestBehavior.AllowGet);
        }

        //GET: Dashboard/GetExpensesThisMonth
        [HttpGet]
        public JsonResult GetExpensesThisMonth()
        {
            var monthToday = dt.GetCurrentDate().Month;

            decimal expenses = services.GetExpensesByMonthNo(monthToday);

            return Json(expenses.ToString("#,##0"), JsonRequestBehavior.AllowGet);
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

        #endregion
    }
}