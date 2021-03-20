using JobsV1.Models.Class;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JobsV1.Controllers
{
    public class HomeController : Controller
    {
        JobsV1.SysAccessLayer sysdb = new SysAccessLayer();
        AppointmentClass apt = new AppointmentClass();

        [Authorize]
        [HandleError(View = "Error")]
        public ActionResult Index()
        {

            ViewBag.Message = "Main page";
            ViewBag.EntHomePageLogo = "Images/EntityFiles/RBC/RealBreezeLogo01.png"; //to update
            ViewBag.EntHomeBackgroundImage = "Images/CarRental/Banner/RealwheelsDavaoBanner2.png"; //to update
            
                           
            ViewBag.TextClass = "textClass_dark"; // { textClass_dark | textClass_light }
            ViewBag.EntName = "Real Breeze Travel and Tours";
            ViewBag.EntAddress1 = "Door 1, Travellers Inn, Matina Pangi Rd., Matina Crossing, Davao city, Philipines ";
            ViewBag.EntAddress2 = "M: 0916-755-8473, 0926-733-5449, 0909-076-1575";
            ViewBag.EntAddress3 = "E: RealBreezeDavao@gmail.com, Travel.RealBreeze@gmail.com";
            ViewBag.EntAddress4 = "";
            ViewBag.EntAddress5 = "";

            ViewBag.EntServices = sysdb.getServiceModules(User.Identity.Name).OrderBy(d=>d.SeqNo).ToList();
            ViewBag.UserName = User.Identity.Name;
            //get current application version
            string sConfigVersion = "";
            string sDBActive = "+++";
            try
            {
                sConfigVersion = System.Configuration.ConfigurationManager.AppSettings["ConfigVersion"].ToString();
            }
            catch { }
            ViewBag.ConfigVersion = sConfigVersion;

            try
            {
                sDBActive = sysdb.getSysSetting("DatabaseState");
            }
            catch { }
            ViewBag.DBActive = sDBActive;
            
            return View();
        }

        public ActionResult AutoCare()
        {
            ViewBag.Message = "Welcome";
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return RedirectToAction("About","CarRental",null);
            //return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return RedirectToAction("Contact", "CarRental", null);
            //return View();
        }

        public PartialViewResult MobileModalView()
        {
            return PartialView("MobileModalView");
        }

        public ActionResult PageNotFoud()
        {
            ViewBag.Message = "Page Not Found";
            return View();
        }


        [HttpGet]
        public JsonResult GetActiveAppt()
        {
            try
            {
                return Json(apt.GetActiveAppt().ToString(), JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json("0", JsonRequestBehavior.AllowGet);
            }
        }

        #region Dynamic SiteMap 
        // [Route("sitemap.xml")]
        public ActionResult SitemapXml()
        {
            string currentUrl = Request.Url.AbsoluteUri;
            int iTmp = currentUrl.IndexOf('/', 7);
            string newurl = currentUrl.Substring(0, iTmp + 1);

            Models.SiteMap sm = new Models.SiteMap();
            var sitemapNodes = sm.GetSitemapNodes(newurl);
            string xml = sm.GetSitemapDocument(sitemapNodes);
            return this.Content(xml, "text/xml", System.Text.Encoding.UTF8);

        }

        public ActionResult SitemapXml_CarHub()
        {
            string currentUrl = Request.Url.AbsoluteUri;
            int iTmp = currentUrl.IndexOf('/', 7);
            string newurl = currentUrl.Substring(0, iTmp + 1);

            Models.SiteMap_carhub sm = new Models.SiteMap_carhub();
            var sitemapNodes = sm.GetSitemapNodes(newurl);
            string xml = sm.GetSitemapDocument(sitemapNodes);
            return this.Content(xml, "text/xml", System.Text.Encoding.UTF8);

        }

        #endregion
    }
}