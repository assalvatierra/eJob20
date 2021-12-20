using System.Web.Mvc;

namespace Payable.Areas.Payables
{
    public class PayablesAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Payables";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {

            //Account Login
            context.MapRoute(
              "Payables Login Redirect",
              "Payables/Account/Login",
              new { controller = "Account", action = "Login" },
              new string[] { "JobsV1.Controllers" }
            );


            context.MapRoute(
                "Payables_default",
                "Payables/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
              new string[] { "JobsV1.Areas.Payables.Controllers" }
            );
        }
    }
}