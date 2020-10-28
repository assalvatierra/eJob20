using System.Web.Mvc;

namespace JobsV1.Areas.Personel
{
    public class PersonelAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Personel";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
              "DriverLogin",
              "DriversLogin",
              new { controller = "crLogPassengers", action = "DriversPortal" },
              new string[] { "JobsV1.Areas.Personel.Controllers" }
            );

            context.MapRoute(
                "mod-trip-list",
                "mod/110/driver/{id}",
                new { controller = "crLogPassengers", action = "DriversTripList", id = UrlParameter.Optional },
                new string[] { "JobsV1.Areas.Personel.Controllers" }
            );

            context.MapRoute(
                "mod-driver-trip",
                "mod/110/driver/trip/{id}",
                new { controller = "crLogPassengers", action = "DriversTripPassengers", id = UrlParameter.Optional },
                new string[] { "JobsV1.Areas.Personel.Controllers" }
            );


            context.MapRoute(
                "mod-trip-admin",
                "mod/110/admin",
                new { controller = "crLogPassengerList", action = "Index" },
                new string[] { "JobsV1.Areas.Personel.Controllers" }
            );


            context.MapRoute(
                "Personel_default",
                "Personel/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }


    }
}