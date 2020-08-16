using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JobsV1.CustomHelper
{
    public static class ErpHelper
    {
        private static string SITECONFIG = ConfigurationManager.AppSettings["SiteConfig"].ToString();
        static SysAccessLayer dal = new SysAccessLayer();

        #region HTML custom helpers
        public static IHtmlString Menu(this HtmlHelper helper, string rootUrl, string target, string text, int MenuId, string username)
        {
            
            SysAccessLayer dal = new SysAccessLayer();
            var menu = dal.getModMenu2(MenuId, username);

            string htmlString = "<table>";

            int parentId = -1;
            int preId = -1;

            htmlString += "<tr >";
            foreach (var item in menu)
            {
                htmlString += "<td >";
                if (item.ParentId == parentId || item.ParentId == preId)
                {
                    htmlString += "<td></td>";
                    htmlString += "<td><p>";
                    //        htmlString += "<a href='" + rootUrl + item.Controller + "/" + item.Action + "/'>" + item.Menu + "</a>";
                    htmlString += "<a href='" + rootUrl + "Module/Menu/" + item.Id + "/' >" + item.Menu + "</a>  | ";
                    htmlString += "</p></td>";
                }
                else
                {
                    htmlString += "<td colspan='2'>";
                    // htmlString += item.Menu;
                    //htmlString += "<a href='" + rootUrl + "Module/Menu/" + item.Id + "/' >" + item.Menu + "</a>  | ";
                    htmlString += "</td>";
                }

                parentId = item.ParentId;
                preId = item.Id;

                htmlString += "</td>";
            }
            htmlString += "<a class:'float:right;' href='" + rootUrl + "Home/' style='float:right;' >Home</a>";
            
            htmlString += "</tr>";

            htmlString += "</table>";

            return new HtmlString(htmlString);
        }

        public static IHtmlString Module(this HtmlHelper helper, string rootUrl, string target, string text, int MenuId)
        {
            SysAccessLayer dal = new SysAccessLayer();
            var service = dal.getModule(MenuId);
            string htmlString = "<span>";
            htmlString += "<img src='" + rootUrl + service.IconPath.TrimStart('/') + "' class='ModulePageIcon' alt='' />";
            htmlString += "</span>";
            htmlString += "<span style='float:left;'>";
            htmlString += "<h2>" + service.Description + "</h2>";
            htmlString += "<p>" + service.Remarks + "</p>";
            htmlString += "</span>";
            if (!dal.getSysSetting("ICON").IsNullOrWhiteSpace())
            {
                htmlString += "<img src='" + dal.getSysSetting("ICON") + "' class='pull-right Company-Icon'  width='200' style='margin:10px;' />";
            }

            return new HtmlString(htmlString);
        }

        /*
         public static IHtmlString Module(this HtmlHelper helper, string rootUrl, string target, string text, int MenuId)
         {
            SysService service = new ErpDataLayer().getModule(MenuId);
            string str2 = "<span>";
            return new HtmlString(((((str2 + "<img src='" + rootUrl + service.IconPath + "' class='ModulePageIcon' alt='' />") + "</span>" + "<span>") + "<h2>" + service.Description + "</h2>") + "<p>" + service.Remarks + "</p>") + "</span>");
         }
         */

        private static int parentId = -1;
        private static int preId = -1;
        public static IHtmlString Menu2(this HtmlHelper helper, string rootUrl, string target, string text, int MenuId, string username)
        {

            SysAccessLayer dal = new SysAccessLayer();
            var menu = dal.getModMenu2(MenuId, username);

            string htmlString = " ";


            htmlString += "";
            foreach (var item in menu)
            {
                htmlString += "<div class='nav-submenu-container'>";
                if (item.ParentId == parentId || item.ParentId == preId)
                {
                    htmlString += "";
                    htmlString += "<span class='nav-submenus'>";
                    //        htmlString += "<a href='" + rootUrl + item.Controller + "/" + item.Action + "/'>" + item.Menu + "</a>";
                    htmlString += " <a href='" + rootUrl + "Module/Menu/" + item.Id + "/' >" + item.Menu  +"</a>   ";
                    htmlString += "</span>";
                }
                else
                {
                    htmlString += "";
                    // htmlString += item.Menu;
                    //htmlString += "<a href='" + rootUrl + "Module/Menu/" + item.Id + "/' >" + item.Menu + "</a>  | ";
                    htmlString += "";
                }

                parentId = item.ParentId;
                preId = item.Id;

                htmlString += "</div>";
            }
            htmlString += "<div class='nav-submenu-home'> <a href='" + rootUrl + "Home/' >Home</a></div>";

            htmlString += "";

            htmlString += "";

            return new HtmlString(htmlString);
        }

     
        #endregion
    }
}