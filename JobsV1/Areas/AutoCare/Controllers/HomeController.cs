using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JobsV1.Areas.AutoCare.Controllers
{
    public class HomeController : Controller
    {
        // GET: AutoCare/Home
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Home", new { area = "" });
        }

        public ActionResult Error()
        {
            return View();
        }
    }
}