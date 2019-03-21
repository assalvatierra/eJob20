using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace JobsV1
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.IgnoreRoute("{*x}", new { x = @".*\.asmx(/.*)?" });


            /********************************
            * sitemap
            ********************************/
            routes.MapRoute(
                name: "sitemap",
                url: "sitemap",
                defaults: new { controller = "Home", action = "SitemapXml", id = UrlParameter.Optional }
                );
            
            #region wordpress links
            routes.MapRoute(
                name: "van-for-rent",
                url: "CarRental/van-for-rent",
                defaults: new { controller = "CarRental", action = "CarDetail", unitid = 1 }
            );
            routes.MapRoute(
                name: "NissanUrvanPremium-for-rent",
                url: "CarRental/NissanUrvanPremium-for-rent",
                defaults: new { controller = "CarRental", action = "CarDetail", unitid = 2 }
            );
            routes.MapRoute(
                name: "suvpickup4x4-rental-rates",
                url: "CarRental/suvpickup4x4-rental-rates",
                defaults: new { controller = "CarRental", action = "CarDetail", unitid = 3 }
            );
            routes.MapRoute(
                name: "toyota-innova-for-rent",
                url: "{controller}/toyota-innova-for-rent",
                defaults: new { controller = "CarRental", action = "CarDetail", unitid = 4 }
            );
            routes.MapRoute(
                name: "Sedan-rental",
                url: "carrental/sedan-rental",
                defaults: new { controller = "CarRental", action = "CarDetail", unitid = 5 }
            );
            routes.MapRoute(
                name: "Pickup-rental",
                url: "carrental/pickup-rental",
                defaults: new { controller = "CarRental", action = "CarDetail", unitid = 6 }
            );
            #endregion

            #region POTTMPC
            routes.MapRoute(
                name: "Van-Starex-for-rent",
                url: "CarCoop/Van-Starex-for-rent",
                defaults: new { controller = "CarCoop", action = "CarDetail", unitid = 1 }
            );
            routes.MapRoute(
                name: "Nissan-Urvan-Premium-for-rent",
                url: "CarCoop/Nissan-Urvan-Premium-for-rent",
                defaults: new { controller = "CarCoop", action = "CarDetail", unitid = 2 }
            );
            routes.MapRoute(
                name: "Nissan-Urvan-for-rent",
                url: "CarCoop/Nissan-Urvan-for-rent",
                defaults: new { controller = "CarCoop", action = "CarDetail", unitid = 3 }
            );
            #endregion

            /********************************
            * landing/home page
            ********************************/
            //routes.MapRoute(
            //    name: "myHome",
            //    url: "MainGeneric/Index/{id}",
            //    defaults: new { controller = "MainGeneric", action = "Index", id = UrlParameter.Optional }
            //);

            /********************************
            * Generic Default page
            ********************************/
            //routes.MapRoute(
            //    name: "Default",
            //    url: "{controller}/{action}/{id}",
            //    defaults: new { controller = "MainGeneric", action = "Index", id = UrlParameter.Optional }
            //);
            
            /*******************************
             * Custom from ajdavaocarrental
             ********************************/
            routes.MapRoute(
              name: "ads-honda-city-automatic",
              url: "ads/honda-city-automatic/",
              defaults: new { controller = "CarRental", action = "CarView", carDesc = "honda-city" }
            );

            routes.MapRoute(
              name: "rent-a-car-suv-for-rent-davao-city",
              url: "ads/rent-a-car-suv-for-rent-davao-city/",
              defaults: new { controller = "CarRental", action = "CarView", carDesc = "ford-everest" }
            );

            routes.MapRoute(
              name: "ads/toyota-hiace-gl-grandia/",
              url: "ads/toyota-hiace-gl-grandia/",
              defaults: new { controller = "CarRental", action = "CarView", carDesc = "toyota-glgrandia" }
            );

            routes.MapRoute(
              name: "ads/toyota-innova-d-4d/",
              url: "ads/toyota-innova-d-4d/",
              defaults: new { controller = "CarRental", action = "CarView", carDesc = "toyota-innova" }
            );

            routes.MapRoute(
              name: "ads/car-for-rent-davao-city/",
              url: "ads/car-for-rent-davao-city/",
              defaults: new { controller = "CarRental", action = "CarView", carDesc = "honda-city-2012" }
            );

            routes.MapRoute(
              name: "ads/rent-a-car-davao-city-self-drive-2/",
              url: "ads/rent-a-car-davao-city-self-drive-2/",
              defaults: new { controller = "CarRental", action = "CarView", carDesc = "self-drive" }
            );

            routes.MapRoute(
              name: "ads/minivan-and-sedan-rentals/",
              url: "ads/minivan-and-sedan-rentals/",
              defaults: new { controller = "CarRental", action = "CarView", carDesc = "minivan-sedan" }
            );

            routes.MapRoute(
              name: "ads/sedan-davao-city-car-rental/",
              url: "ads/sedan-davao-city-car-rental/",
              defaults: new { controller = "CarRental", action = "CarView", carDesc = "rent-a-car" }
            );

            routes.MapRoute(
              name: "ads/toyota-avanza-1-3e-at/",
              url: "ads/toyota-avanza-1-3e-at/",
              defaults: new { controller = "CarRental", action = "CarView", carDesc = "toyota-avanza" }
            );
            routes.MapRoute(
              name: "ads/van-rental-davao-city/",
              url: "ads/van-rental-davao-city/",
              defaults: new { controller = "CarRental", action = "CarView", carDesc = "van-rental" }
            );


            /*******************************
             * Custom from ajdavaocarrental / Page 2
             ********************************/

            routes.MapRoute(
              name: "ads/ford-fiesta-1-6l-sedan-2012/",
              url: "ads/ford-fiesta-1-6l-sedan-2012/",
              defaults: new { controller = "CarRental", action = "CarView", carDesc = "ford-fiesta" }
            );

            routes.MapRoute(
              name: "ads/innovacar-for-rent-davao-city/",
              url: "ads/innovacar-for-rent-davao-city/",
              defaults: new { controller = "CarRental", action = "CarView", carDesc = "p2-innova-rental" }
            );


            routes.MapRoute(
              name: "ads/rent-a-car-davao-city-self-drive/",
              url: "ads/rent-a-car-davao-city-self-drive/",
              defaults: new { controller = "CarRental", action = "CarView", carDesc = "innova-self-drive" }
            );

            routes.MapRoute(
              name: "ads/car-rental-honda-city-for-rent-self-drive/",
              url: "ads/car-rental-honda-city-for-rent-self-drive/",
              defaults: new { controller = "CarRental", action = "CarView", carDesc = "honda-self-drive" }
            );

            routes.MapRoute(
              name: "ads/4x4-rental-suv-for-rent-davao/",
              url: "ads/4x4-rental-suv-for-rent-davao/",
              defaults: new { controller = "CarRental", action = "CarView", carDesc = "toyota-fortuner" }
            );

            routes.MapRoute(
              name: "ads/4x4-rental-pickup-for-rent-davao/",
              url: "ads/4x4-rental-pickup-for-rent-davao/",
              defaults: new { controller = "CarRental", action = "CarView", carDesc = "pickup" }
            );


            /*******************************
             * Custom from ajdavaocarrental / listing
             ********************************/
            routes.MapRoute(
              name: "/ad-category/sedans/",
              url: "ad-category/sedans/",
              defaults: new { controller = "CarRental", action = "CarView", carDesc = "sedan-listing" }
            );

            routes.MapRoute(
              name: "ads/",
              url: "ads/",
              defaults: new { controller = "CarRental", action = "CarView", carDesc = "ads-listing" }
            );

            routes.MapRoute(
              name: "ads/page/1/",
              url: "ads/page/1/",
              defaults: new { controller = "CarRental", action = "CarView", carDesc = "ads-listing" }
            );

            routes.MapRoute(
              name: "ads/page/2/",
              url: "ads/page/2/",
              defaults: new { controller = "CarRental", action = "CarView", carDesc = "ads-listing-page-2" }
            );

            routes.MapRoute(
              name: "ad-category/others/",
              url: "ad-category/others/",
              defaults: new { controller = "CarRental", action = "CarView", carDesc = "ads-listing-others" }
            );

            routes.MapRoute(
              name: "ad-category/vans/",
              url: "ad-category/vans/",
              defaults: new { controller = "CarRental", action = "CarView", carDesc = "ads-listing-vans" }
            );
            routes.MapRoute(
              name: "ad-category/mpv/",
              url: "ad-category/mpv/",
              defaults: new { controller = "CarRental", action = "CarView", carDesc = "ads-listing-mpv" }
            );
            routes.MapRoute(
              name: "ad-category/pickup/",
              url: "ad-category/pickup/",
              defaults: new { controller = "CarRental", action = "CarView", carDesc = "ads-listing-pickup" }
            );
            /*******************************
             * Custom from ajdavaocarrental / ad-tags
             ********************************/
            routes.MapRoute(
              name: "ad-tag/car-rental-davao/",
              url: "ad-tag/car-rental-davao/",
              defaults: new { controller = "CarRental", action = "CarView", carDesc = "tag-car-rental-davao" }
            );

            routes.MapRoute(
              name: "ad-tag/davao-rent-a-car/",
              url: "ad-tag/davao-rent-a-car/",
              defaults: new { controller = "CarRental", action = "CarView", carDesc = "tag-davao-rent-a-car" }
            );
            routes.MapRoute(
              name: "ad-tag/rent-a-car-davao-city/",
              url: "ad-tag/rent-a-car-davao-city/",
              defaults: new { controller = "CarRental", action = "CarView", carDesc = "rent-a-car-davao-city" }
            );

            routes.MapRoute(
              name: "ad-tag/van-for-rent-davao-city/",
              url: "ad-tag/van-for-rent-davao-city/",
              defaults: new { controller = "CarRental", action = "CarView", carDesc = "van-for-rent-davao-city" }
            );


            /********************************
            * invoice
            ********************************/
            routes.MapRoute(
                name: "JobOrderInvoice",
                url: "invoice/{id}/{month}/{day}/{year}/{rName}",
                defaults: new { controller = "JobOrder", action = "BookingRedirect", id="id" , month="month", day="day", year="year", rName = "rName"  }
            );

            /********************************
            * Car Rental Reservation
            ********************************/
            routes.MapRoute(
                name: "Reservation",
                url: "reservation/{id}/{month}/{day}/{year}/{rName}",
                defaults: new { controller = "CarReservations", action = "ReservationRedirect", id = "id", month = "month", day = "day", year = "year", rName = "rName" }
            );

            /********************************
            * CarCoop landing/home page
            
            routes.MapRoute(
                name: "POTTMPC/Home",
                url: "CarCoop/Index/{id}",
                defaults: new { controller = "CarCoop", action = "Index", id = UrlParameter.Optional }
            );
            
            routes.MapRoute(
                name: "POTTMPC/Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "CarCoop", action = "Index", id = UrlParameter.Optional }
            );
            
            ********************************/

            /********************************
            * landing/home page
            ********************************/
            routes.MapRoute(
                name: "myHome",
                url: "CarRental/Index/{id}",
                defaults: new { controller = "CarRental", action = "Index", id = UrlParameter.Optional }
            );
            /********************************
            * AJ88 car rental default
            ********************************/
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "CarRental", action = "Index", id = UrlParameter.Optional }
            );

        }
    }
}
