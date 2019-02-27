using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JobsV1.Controllers
{
    public class MepuMainController : Controller
    {
        // GET: MepuMain
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult ProtectedAreas()
        {
            return View();
        }

        public ActionResult SearchAreas(string Options)
        {
            string opt = Options;
            if (opt == "TEST")
            {
                return Json(new ObjResult { status = "OK", message = "Test Result:" + System.DateTime.Now.ToString() });
            }
            else
            {
                return PartialView("_PartialViewSearchAreas");
                //return Json(new ObjResult { status = "OK", message = "Normal Result:" + System.DateTime.Now.ToString() });
            }
        }

        public ActionResult PartialSearchAreas(string Options)
        {
            string opt = Options;
            if (opt == "TEST")
            {
                return PartialView("_PartialViewSearchAreas");
                //return Json(new ObjResult { status = "OK", message = "Test Result:" + System.DateTime.Now.ToString() });
            }
            else
            {
                return PartialView("_PartialViewSearchAreas");
                //return Json(new ObjResult { status = "OK", message = "Normal Result:" + System.DateTime.Now.ToString() });
            }

        }


    }


    public class ObjResult
    {
        public string status { get; set; }
        public string message { get; set; }
    }



}