using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JobsV1.Areas.Products.Controllers
{
    public class HomeController : Controller
    {
        // GET: Products/Home
        public ActionResult Index()
        {
            return View();
        }
    }
}