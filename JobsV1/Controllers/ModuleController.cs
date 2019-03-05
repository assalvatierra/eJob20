using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JobsV1.Controllers
{
    public class ModuleController : Controller
    {
        SysAccessLayer dal = new SysAccessLayer();

        public ActionResult Index()
        {
            return View();
        }

        public int getInitModuleId(int serviceId)
        {
            return 1;
        }

        public int getServiceId(int moduleId)
        {
            return 1;
        }

        public ActionResult Menu(int id)
        {
            System.Diagnostics.Trace.WriteLine("Module (" + System.DateTime.Now.ToString() + ")" + "Menu:" + id.ToString());

            ModRoute mr = dal.getMenuRoute(id);
            Session["CURRENTMENUID"] = (int)mr.RootMenuId;

            string sAction = (mr.Action == null || mr.Action.Trim() == "" ? "Index" : mr.Action);
            string sController = (mr.Controller == null || mr.Controller.Trim() == "" ? "Home" : mr.Controller);

            Object oArea = new { area = mr.Area };
            return RedirectToAction(mr.Action, mr.Controller, oArea);
        }

        public ActionResult Service(int id)
        {
            System.Diagnostics.Trace.WriteLine("Module (" + System.DateTime.Now.ToString() + ")" + "Service:" + id.ToString());

            ModRoute mr = dal.getModRoute(id);
            Session["ROOTMENUID"] = (int)mr.RootMenuId;

            string sAction = (mr.Action == null || mr.Action.Trim() == "" ? "Index" : mr.Action);
            string sController = (mr.Controller == null || mr.Controller.Trim() == "" ? "Home" : mr.Controller); 

            Object oArea = new { area = mr.Area };

            //return RedirectToAction(mr.Action, mr.Controller, oArea);
            return RedirectToAction(sAction, sController);

            //redirect to action if correct
            //redirect to controller/action if redirect option is not found.
            
        }

        public ActionResult Error()
        {
            return View();
        }


    }
}