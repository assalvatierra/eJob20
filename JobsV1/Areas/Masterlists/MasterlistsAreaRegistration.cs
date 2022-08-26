using System.Web.Mvc;

namespace JobsV1.Areas.Masterlists
{
    public class MasterlistsAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Masterlists";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Masterlists_default",
                "Masterlists/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}