using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace JobsV1
{
    public class RouteConfig
    {
        //Realwheels 
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
                defaults: new { controller = "Home", action = "SitemapXml", id = UrlParameter.Optional },
                namespaces: new[] { "JobsV1.Controllers" }
                );

            routes.MapRoute(
               name: "sitemapXML",
               url: "sitemap.xml",
               defaults: new { controller = "Home", action = "SitemapXml", id = UrlParameter.Optional },
               namespaces: new[] { "JobsV1.Controllers" }
               );

            routes.MapRoute(
                name: "Home",
                url: "Home/",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "JobsV1.Controllers" }
            );


            routes.MapRoute(
                name: "page/2",
                url: "page/2",
                defaults: new { controller = "CarRental", action = "PriceList", id = UrlParameter.Optional },
                namespaces: new[] { "JobsV1.Controllers" }
            );


            #region CarRental links
            /********************************
            * Car Rental w/ Articles
            ********************************/
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
                url: "CarRental/sedan-rental",
                defaults: new { controller = "CarRental", action = "CarDetail", unitid = 5 }
            );
            routes.MapRoute(
                name: "Pickup-rental",
                url: "CarRental/pickup-rental",
                defaults: new { controller = "CarRental", action = "CarDetail", unitid = 6 }
            );
            routes.MapRoute(
                name: "ToyotaTourer-for-rent",
                url: "CarRental/ToyotaTourer-for-rent",
                defaults: new { controller = "CarRental", action = "CarDetail", unitid = 7 }
            );
            routes.MapRoute(
                name: "rush-rental",
                url: "CarRental/suvpickup4x4-rental-rates",
                defaults: new { controller = "CarRental", action = "CarDetail", unitid = 8 }
            );

            ////duplicate redirect
            //routes.MapRoute(
            //    name: "Sedan-carrental",
            //    url: "carrental/sedan-rental",
            //    defaults: new { controller = "CarRental", action = "CarDetail", unitid = 5 }
            //);

            //error on search
            routes.MapRoute(
                name: "ad-tag/car-rental-davao/Fix",
                url: "ad-tag/car-rental-davao/",
                defaults: new { controller = "CarRental", action = "CarDetail", unitid = 1 }
            );
            routes.MapRoute(
                name: "ads/honda-city-automatic/Fix",
                url: "ads/honda-city-automatic/",
                defaults: new { controller = "CarRental", action = "CarDetail", unitid = 5 }
            );
            routes.MapRoute(
                name: "ad-tag/davao-rent-a-car/Fix",
                url: "ad-tag/davao-rent-a-car/",
                defaults: new { controller = "CarRental", action = "CarDetail", unitid = 1 }
            );
            routes.MapRoute(
                name: "ad-category/vans/Fix",
                url: "ad-category/vans/",
                defaults: new { controller = "CarRental", action = "CarDetail", unitid = 1 }
            );

            routes.MapRoute(
                name: "ad-category/sedans/Fix",
                url: "ad-category/sedans/",
                defaults: new { controller = "CarRental", action = "CarDetail", unitid = 5 }
            );

            routes.MapRoute(
                name: "ads/innovacar-for-rent-davao-city/Fix",
                url: "ads/innovacar-for-rent-davao-city/",
                defaults: new { controller = "CarRental", action = "CarDetail", unitid = 1 }
            );

            routes.MapRoute(
                name: "ads/page/2/Fix",
                url: "ads/page/2/",
                defaults: new { controller = "CarRental", action = "CarDetail", unitid = 1 }
            );



            #endregion

            #region Car Rental Articles

            /********************************
            * Articles
            ********************************/

            routes.MapRoute(
                name: "Article/Article1",
                url: "Articles/Article1",
                defaults: new { controller = "CarRental", action = "ArticleView", article = "Article1" }
            );

            routes.MapRoute(
                name: "Article/NewSeatCapacity",
                url: "Articles/NewSeatCapacity",
                defaults: new { controller = "CarRental", action = "ArticleView", article = "NewSeatCapacity" }
            );

            routes.MapRoute(
                name: "Article/SUVRental",
                url: "Articles/SUVRental",
                defaults: new { controller = "CarRental", action = "ArticleView", article = "SUVRental" }
            );

            routes.MapRoute(
                name: "Article/VisitDavao",
                url: "Articles/VisitDavao",
                defaults: new { controller = "CarRental", action = "ArticleView", article = "VisitDavao" }
            );

            routes.MapRoute(
                name: "Article/WhyBook",
                url: "Articles/WhyBook",
                defaults: new { controller = "CarRental", action = "ArticleView", article = "WhyBook" }
            );

            routes.MapRoute(
                name: "Article/WhyRentACar",
                url: "Articles/WhyRentACar",
                defaults: new { controller = "CarRental", action = "ArticleView", article = "WhyRentACar" }
            );

            routes.MapRoute(
                name: "Article/QRCodeList",
                url: "Articles/mindanao-qr-code-list-2021",
                defaults: new { controller = "CarRental", action = "ArticleView", article = "QRCodeList" }
            );

            routes.MapRoute(
                name: "Article/SafeCarRental",
                url: "Articles/Safe-Car-Rental-2021",
                defaults: new { controller = "CarRental", action = "ArticleView", article = "SafeCarRental" }
            );

            routes.MapRoute(
                name: "Articles List Page 1",
                url: "Articles/1",
                defaults: new { controller = "CarRental", action = "ArticleView", article = "Page-1" }
            );

            #endregion

            #region Services

            /********************************
            * Services
            ********************************/

            routes.MapRoute(
                name: "Services/CarServices",
                url: "Car-Rental-Services",
                defaults: new { controller = "CarRental", action = "CarServices" }
            );

            routes.MapRoute(
                name: "Services/Airport-Hotel-Transfer",
                url: "Services/Airport-Hotel-Transfer",
                defaults: new { controller = "CarRental", action = "Services", service = "Airport-Hotel-Transfer" }
            );

            routes.MapRoute(
                name: "Services/Business-Trip",
                url: "Services/Business-Trip",
                defaults: new { controller = "CarRental", action = "Services", service = "Business-Trip" }
            );

            routes.MapRoute(
                name: "Services/City-Tour-Car-Rental",
                url: "Services/City-Tour-Car-Rental",
                defaults: new { controller = "CarRental", action = "Services", service = "City-Tour-Car-Rental" }
            );

            routes.MapRoute(
                name: "Services/Corporate-Travel",
                url: "Services/Corporate-Travel",
                defaults: new { controller = "CarRental", action = "Services", service = "Corporate-Travel" }
            );

            routes.MapRoute(
                name: "Services/Out-Of-Town-Trip",
                url: "Services/Out-Of-Town-Trip",
                defaults: new { controller = "CarRental", action = "Services", service = "Out-Of-Town-Trip" }
            );

            routes.MapRoute(
                name: "Services/Self-Drive",
                url: "Services/Self-Drive",
                defaults: new { controller = "CarRental", action = "Services", service = "Self-Drive" }
            );

            routes.MapRoute(
                name: "Services/Special-Occasion",
                url: "Services/Special-Occasion",
                defaults: new { controller = "CarRental", action = "Services", service = "Special-Occasion" }
            );

            routes.MapRoute(
                name: "cargo-delivery-services",
                url: "cargo-delivery-services",
                defaults: new { controller = "CarRental", action = "Services", service = "cargo-delivery-services" }
            );


            routes.MapRoute(
                name: "shuttle-service",
                url: "shuttle-service",
                defaults: new { controller = "CarRental", action = "Services", service = "shuttle-service" }
            );


            #endregion

            /********************************
            * invoice
            ********************************/
            routes.MapRoute(
                name: "JobOrderInvoice",
                url: "invoice/{id}/{month}/{day}/{year}/{rName}",
                defaults: new { controller = "JobOrder", action = "BookingRedirect", id = "id", month = "month", day = "day", year = "year", rName = "rName" }
            );  

            /********************************
            * Car Rental Reservation
            ********************************/
            routes.MapRoute(
                name: "Reservation",
                url: "reservation/{id}/{month}/{day}/{year}/{rName}",
                defaults: new { controller = "CarReservations", action = "ReservationRedirect", id = "id", month = "month", day = "day", year = "year", rName = "rName" }
            );

            routes.MapRoute(
               name: "Reservation-Reserve",
               url: "CarRental/Reservation",
               defaults: new { controller = "CarRental", action = "CarReservation", id = "id", rsvType = 1 }
           );


            routes.MapRoute(
               name: "Reservation-Reserve-dup",
               url: "CarRental/CarRental/Reservation",
               defaults: new { controller = "CarRental", action = "CarReservation", id = "id", rsvType = 1 }
           );


            routes.MapRoute(
               name: "Reservation-Reserve-withID",
               url: "CarRental/Reservation/{id}",
               defaults: new { controller = "CarRental", action = "CarReservation", id = "id", rsvType = 1 }
           );


            routes.MapRoute(
               name: "Reservation-PriceQuote",
               url: "CarRental/PriceQuote",
               defaults: new { controller = "CarRental", action = "PriceQuote", id = "id", rsvType = 2 }
           );

            routes.MapRoute(
               name: "Reservation-FormRenter",
               url: "CarRental/FormRenter/{id}",
               defaults: new { controller = "CarRental", action = "ReservationRequest", id = "id", rsvType = 1 }
           );


            routes.MapRoute(
               name: "Reservation-Client-Details",
               url: "OnlineReservations/Details/{id}",
               defaults: new { controller = "CarReservations", action = "Details", id = "id" }
           );

            /********************************
            * landing/home page
            ********************************/
            routes.MapRoute(
                name: "myHome",
                url: "CarRental/Index/{id}",
                defaults: new { controller = "CarRental", action = "Index", id = UrlParameter.Optional }
            );

            /********************************
             * missing links found by Google Search Console
             * *****************************/
            Register_CarRentalAds(routes);

            /********************************
            * AJ88 car rental default
            ********************************/
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "CarRental", action = "Index", id = UrlParameter.Optional }
            );

        }

        //Solid Steel  
        public static void RegisterRoutes_SolidSteel(RouteCollection routes)
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

            routes.MapRoute(
                name: "myHome",
                url: "Home/",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "JobsV1.Controllers" }
            );

            routes.MapRoute(
                name: "Home",
                url: "Home/",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "JobsV1.Controllers" }
            );
            
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "JobsV1.Controllers" }
            );
        }


        // Car Rental Ads links
        // Not Found Error in Google Search Console
        public static void Register_CarRentalAds(RouteCollection routes)
        {

            #region Ajcarrental links
            /*******************************
             * Custom from ajdavaocarrental
             ********************************/
            routes.MapRoute(
              name: "ads/toyota-Rush/",
              url: "ads/toyota-Rush/",
              defaults: new { controller = "CarRental", action = "CarView", carDesc = "toyota-rush" }
            );


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
             * Custom from ajdavaocarrental / Page 2/ad-tag/rent-a-car-davao-city/
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
            #endregion


        }


        // AJcarrental Davao / Davao Carrental hub 
        public static void RegisterRoutes_Carrental(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.IgnoreRoute("{*x}", new { x = @".*\.asmx(/.*)?" });
            
            /********************************
            * sitemap
            ********************************/
            routes.MapRoute(
                name: "sitemap",
                url: "sitemap",
                defaults: new { controller = "Home", action = "SitemapXml_CarHub", id = UrlParameter.Optional },
                namespaces: new[] { "JobsV1.Controllers" }
                );

            routes.MapRoute(
                name: "sitemap.xml",
                url: "sitemap.xml",
                defaults: new { controller = "Home", action = "SitemapXml_CarHub", id = UrlParameter.Optional },
                namespaces: new[] { "JobsV1.Controllers" }
                );

            routes.MapRoute(
                name: "Home",
                url: "Home/",
                defaults: new { controller = "CarRental", action = "CarView", carDesc = "ads-listing", id = UrlParameter.Optional },
                namespaces: new[] { "JobsV1.Controllers" }
            );

            routes.MapRoute(
                name: "About",
                url: "CarRental/About",
                defaults: new { controller = "CarRental", action = "About_Rental", id = UrlParameter.Optional },
                namespaces: new[] { "JobsV1.Controllers" }
            );

            #region CarRental links

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
            
            #region From Error List
            //From Error List
            //carrental/toyota-hiace-van-for-rent/
            routes.MapRoute(
                name: "toyota-hiace-van-for-rent",
                url: "carrental/toyota-hiace-van-for-rent/",
                defaults: new { controller = "CarRental", action = "CarDetail", unitid = 1 }
            );

            //carrental/tag/davao-city-car-rentals/
            routes.MapRoute(
                name: "davao-city-car-rentals",
                url: "carrental/tag/davao-city-car-rentals/",
                defaults: new { controller = "CarRental", action = "CarView", carDesc = "ads-listing" }
            );

            //carrental/honda-city/hondacitybr1/
            routes.MapRoute(
                name: "honda-city/hondacitybr1/",
                url: "carrental/honda-city/hondacitybr1/",
                defaults: new { controller = "CarRental", action = "CarDetail", unitid = 5 }
            );

            //carrental/honda-city/
            routes.MapRoute(
                name: "honda-city",
                url: "carrental/honda-city/",
                defaults: new { controller = "CarRental", action = "CarDetail", unitid = 5 }
            );

            //carrental/toyota-vios-for-rent/toyota-vios-2012/
            routes.MapRoute(
                name: "toyota-vios-for-rent/toyota-vios-2012/",
                url: "carrental/toyota-vios-for-rent/toyota-vios-2012/",
                defaults: new { controller = "CarRental", action = "CarDetail", unitid = 5 }
            );

            //carrental/ford-everest-suv-car-for-rent-davao-car-rental/ford-everest-rear01/
            routes.MapRoute(
                name: "ford-everest-suv-car-for-rent-davao-car-rental/ford-everest-rear01/",
                url: "carrental/ford-everest-suv-car-for-rent-davao-car-rental/ford-everest-rear01/",
                defaults: new { controller = "CarRental", action = "CarDetail", unitid = 3 }
            );

            //carrental/category/suvauvmpv/
            routes.MapRoute(
                name: "suvauvmpv",
                url: "carrental/category/suvauvmpv/",
                defaults: new { controller = "CarRental", action = "CarView", carDesc = "ads-listing-mpv" }
            );
            
            //carrental/toyota-avanza-for-rent/
            routes.MapRoute(
                name: "toyota-avanza-for-rent",
                url: "carrental/toyota-avanza-for-rent/",
                defaults: new { controller = "CarRental", action = "CarDetail", unitid = 4 }
            );

            //carrental/category/rates/
            routes.MapRoute(
                name: "category/rates",
                url: "category/rates/",
                defaults: new { controller = "CarRental", action = "CarDetail", unitid = 1 }
            );

            //carrental/category/realwheels/
            routes.MapRoute(
                name: "category/realwheels",
                url: "category/realwheels/",
                defaults: new { controller = "CarRental", action = "CarView", carDesc = "ads-listing" }
            );

            //carrental/van-rental-rates/
            routes.MapRoute(
                name: "van-rental-rates/",
                url: "carrental/van-rental-rates/",
              defaults: new { controller = "CarRental", action = "CarView", carDesc = "ads-listing-vans" }
            );

            //carrental/ford-everest-suv-car-for-rent-davao-car-rental/
            routes.MapRoute(
                name: "ford-everest-suv-car-for-rent-davao-car-rental/",
                url: "carrental/ford-everest-suv-car-for-rent-davao-car-rental/",
                defaults: new { controller = "CarRental", action = "CarDetail", unitid = 3 }
            );

            //carrental/author/abel/
            routes.MapRoute(
                name: "author/abel/",
                url: "carrental/author/abel/",
                defaults: new { controller = "CarRental", action = "CarView", carDesc = "ads-listing" }
            );

            //carrental/carrental/category/4x4/
            routes.MapRoute(
                name: "category/4x4/",
                url: "carrental/category/4x4/",
                defaults: new { controller = "CarRental", action = "CarDetail", unitid = 6 }
            );

            //carrental/tag/rent-a-car/
            routes.MapRoute(
                name: "tag/rent-a-car/",
                url: "carrental/tag/rent-a-car/",
              defaults: new { controller = "CarRental", action = "CarView", carDesc = "tag-davao-rent-a-car" }
            );

            //carrental/category/sedan/
            routes.MapRoute(
                name: "category/sedan/",
                url: "carrental/category/sedan/",
              defaults: new { controller = "CarRental", action = "CarView", carDesc = "sedan-listing" }
            );

            //davao-rent-a-car/
            routes.MapRoute(
                name: "davao-rent-a-car/",
                url: "carrental/davao-rent-a-car/",
              defaults: new { controller = "CarRental", action = "CarView", carDesc = "tag-davao-rent-a-car" }
            );

            //carrental/toyota-vios-for-rent/
            routes.MapRoute(
                name: "carrental/toyota-vios-for-rent/",
                url: "carrental/toyota-vios-for-rent/",
                defaults: new { controller = "CarRental", action = "CarDetail", unitid = 4 }
            );

            //page/2/
            routes.MapRoute(
                name: "page/2/",
                url: "page/2/",
              defaults: new { controller = "CarRental", action = "CarView", carDesc = "ads-listing-page-2" }
            );

            //carrental/toyota-fortuner-4x4/
            routes.MapRoute(
                name: "toyota-fortuner-4x4/",
                url: "carrental/toyota-fortuner-4x4/",
                defaults: new { controller = "CarRental", action = "CarDetail", unitid = 3 }
            );

            //carrental/mpvauvminivan-rental-rates/",
            routes.MapRoute(
                name: "mpvauvminivan-rental-rates/",
                url: "carrental/mpvauvminivan-rental-rates/",
                defaults: new { controller = "CarRental", action = "CarDetail", unitid = 4 }
            );

            //carrental/toyota-fortuner-4x4/
            routes.MapRoute(
                name: "tag/auv-rentals/",
                url: "carrental/tag/auv-rentals/",
              defaults: new { controller = "CarRental", action = "CarView", carDesc = "ads-listing-mpv" }
            );

            //carrental/chevrolet-trailblazer/
            routes.MapRoute(
                name: "chevrolet-trailblazer/",
                url: "carrental/chevrolet-trailblazer/",
                defaults: new { controller = "CarRental", action = "CarDetail", unitid = 3 }
            );

            //category/van/
            routes.MapRoute(
                name: "category/van/",
                url: "carrental/category/van/",
              defaults: new { controller = "CarRental", action = "CarView", carDesc = "ads-listing-vans" }
            );

            //toyota-fortuner-for-rent/
            routes.MapRoute(
                name: "toyota-fortuner-for-rent/",
                url: "carrental/toyota-fortuner-for-rent/",
                defaults: new { controller = "CarRental", action = "CarDetail", unitid = 3 }
            );

            //toyota-fortuner-for-rent/
            routes.MapRoute(
                name: "category/featured/",
                url: "carrental/category/featured/",
              defaults: new { controller = "CarRental", action = "CarView", carDesc = "ads-listing" }
            );

            //van-for-rent
            routes.MapRoute(
                name: "carrental/van-for-rent",
                url: "carrental/van-for-rent",
                defaults: new { controller = "CarRental", action = "CarDetail", unitid = 1 }
            );
            #endregion

            #region Ajcarrental links

            /********************************
            * Links From AJ
            ********************************/
            Register_CarRentalAds(routes);

            #endregion

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
            * Davaocarrentalhub.com landing/home page
            ********************************/
            routes.MapRoute(
                name: "CarRentalHub/Home",
                url: "{controller}/{action}/{id}",
              defaults: new { controller = "CarRental", action = "CarView", carDesc = "ads-listing", id = UrlParameter.Optional }
            );

        }


        //AutoCare 
        public static void RegisterRoutes_AutoCare(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.IgnoreRoute("{*x}", new { x = @".*\.asmx(/.*)?" });

            /********************************
            * sitemap
            ********************************/
            routes.MapRoute(
                name: "sitemap",
                url: "sitemap",
                defaults: new { controller = "Home", action = "SitemapXml", id = UrlParameter.Optional },
                namespaces: new[] { "JobsV1.Controllers" }
                );

            routes.MapRoute(
                name: "Home",
                url: "Home/",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "JobsV1.Controllers" }
            );


            /********************************
            * invoice
            ********************************/
            routes.MapRoute(
                name: "JobOrderInvoice",
                url: "invoice/{id}/{month}/{day}/{year}/{rName}",
                defaults: new { controller = "JobOrder", action = "BookingRedirect", id = "id", month = "month", day = "day", year = "year", rName = "rName" }
            );

            /********************************
            * AutoCare Appointment
            ********************************/
            routes.MapRoute(
                name: "Appointment",
                url: "Appointment/",
                defaults: new { controller = "Public", action = "Appointment" },
                namespaces: new[] { "JobsV1.Controllers" }
            );


            /********************************
            * landing/home page
            ********************************/
            routes.MapRoute(
                name: "myHome",
                url: "Home/Index/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "JobsV1.Controllers" }
            );

            /********************************
            * AJ88 car rental default
            ********************************/
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "JobsV1.Controllers" }
            );

        }

    }
}
