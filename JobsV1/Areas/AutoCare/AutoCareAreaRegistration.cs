using System.Web.Mvc;

namespace JobsV1.Areas.AutoCare
{
    public class AutoCareAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "AutoCare";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "AutoCare_default",
                "AutoCare/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}