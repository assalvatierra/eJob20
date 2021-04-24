using System.Web.Mvc;

namespace JobsV1.Areas.Products
{
    public class ProductsAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Products";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {

            //Products Login
            context.MapRoute(
              "Products Login Redirect",
              "Products/Account/Login",
              new { controller = "Account", action = "Login" },
              new string[] { "JobsV1.Controllers" }
            );

            context.MapRoute(
                "Products_default",
                "Products/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}