using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace JobsV1
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            
            var currentSite = ConfigurationManager.AppSettings["SiteConfig"].ToString();
            switch (currentSite){
                case "AutoCare":
                    RouteConfig.RegisterRoutes_AutoCare(RouteTable.Routes);
                    break;
                case "Realwheels":
                    RouteConfig.RegisterRoutes(RouteTable.Routes);
                    break;
                default:
                    RouteConfig.RegisterRoutes(RouteTable.Routes);
                    break;
            }

            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
