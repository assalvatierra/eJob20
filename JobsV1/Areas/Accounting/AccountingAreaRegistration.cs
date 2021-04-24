using System.Web.Mvc;

namespace JobsV1.Areas.Accounting
{
    public class AccountingAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Accounting";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            //Accounting Login
            context.MapRoute(
              "Accounting Login Redirect",
              "Accounting/Account/Login",
              new { controller = "Account", action = "Login" },
              new string[] { "JobsV1.Controllers" }
            );

            context.MapRoute(
                "Accounting_default",
                "Accounting/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}