using System.Web.Mvc;

namespace ArWeb.Areas.Receivables
{
    public class ReceivablesAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Receivables";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {

            //Receivables Login
            context.MapRoute(
              "Receivables Login Redirect",
              "Receivables/Account/Login",
              new { controller = "Account", action = "Login" },
              new string[] { "JobsV1.Controllers" }
            );

            context.MapRoute(
                "Receivables_default",
                "Receivables/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                namespaces: new string[] { "JobsV1.Areas.Receivables.Controllers" }
            );
        }
    }
}