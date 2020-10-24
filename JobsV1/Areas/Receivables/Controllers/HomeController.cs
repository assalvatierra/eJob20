using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JobsV1.Areas.Receivables.Controllers
{
    public class HomeController : Controller
    {
        // GET: Receivables/Home
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Home", new { area = "" });
        }
    }
}