using System.Web;
using System.Web.Optimization;

namespace JobsV1
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-{version}.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*",
                        "~/Scripts/cleave.min.js"
                        ));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/flickity").Include(
                      "~/Scripts/Flicky.2.2.1.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/ModalUser.css",
                      "~/Content/themes/base/jquery-ui.css",
                      "~/Content/themes/base/bootstrap-directional-buttons.min.css",
                      "~/Content/login.css",
                      "~/Content/Erp.css",
                      "~/Content/UserStyles.css",
                      "~/Content/ModalSalesLead.css",
                      "~/Content/site.css",
                      "~/Content/font-awesome.min.css",
                      "~/Content/TableStyles.css",
                      "~/Content/Chart.min.css",
                      "~/Content/CarRental.css"
                      ));

            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
                        "~/Content/themes/base/all.css",
                        "~/Content/themes/base/base.css",
                        "~/Content/themes/base/selectmenu.css",
                        "~/Content/themes/base/core.css",
                        "~/Content/themes/base/jquiry-ui.css",
                        "~/Content/themes/base/resizable.css",
                        "~/Content/themes/base/selectable.css",
                        "~/Content/themes/base/accordion.css",
                        "~/Content/themes/base/autocomplete.css",
                        "~/Content/themes/base/button.css",
                        "~/Content/themes/base/dialog.css",
                        "~/Content/themes/base/slider.css",
                        "~/Content/themes/base/tabs.css",
                        "~/Content/themes/base/datepicker.css",
                        "~/Content/themes/base/progressbar.css",
                        "~/Content/themes/base/theme.css"));

            bundles.Add(new StyleBundle("~/Content/theme/darkness").Include(
                "~/Content/themes/ui-darkness/jquery-ui.ui-darkness.css"
               ));
            bundles.Add(new StyleBundle("~/Content/theme/lightness").Include(
                "~/Content/themes/ui-lightness/jquery-ui.ui-lightness.css"
               ));

            bundles.Add(new ScriptBundle("~/bundles/DateRangePicker").Include(
                    "~/Scripts/sort-table.js",
                    "~/Scripts/DateRangePicker/moment.js",
                    "~/Scripts/DateRangePicker/daterangepicker.js"
                ));

            bundles.Add(new StyleBundle("~/Content/css/DateRangePicker").Include(
                    "~/Content/daterangepicker.css"
                ));

            bundles.Add(new StyleBundle("~/Content/TableStyles").Include(
                      "~/Content/TableStyles.css"
                      ));

            bundles.Add(new StyleBundle("~/Content/ReportStyles").Include(
                      "~/Content/RptTripLog.css"
                      ));

            bundles.Add(new StyleBundle("~/Content/jobMonitor").Include(
                      "~/Content/JobMonitorStyles.css"
                      ));

            bundles.Add(new StyleBundle("~/Content/flickity").Include(
                      "~/Content/flickity.css"
                      ));

            //Bundling and Minification
            //see https://docs.microsoft.com/en-us/aspnet/mvc/overview/performance/bundling-and-minification
            BundleTable.EnableOptimizations = true;
        }
    }
}
