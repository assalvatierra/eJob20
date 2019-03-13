using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;

namespace JobsV1.CustomHelper
{
    public static class ErpHelper
    {
        #region HTML custom helpers
        public static IHtmlString Menu(this HtmlHelper helper, string rootUrl, string target, string text, int MenuId)
        {

            SysAccessLayer dal = new SysAccessLayer();
            var menu = dal.getModMenu(MenuId);

            string htmlString = "<table>";

            int parentId = -1;
            int preId = -1;

            htmlString += "<tr>";
            foreach (var item in menu)
            {
                htmlString += "<td>";
                if (item.ParentId == parentId || item.ParentId == preId)
                {
                    htmlString += "<td></td>";
                    htmlString += "<td>";
                    //        htmlString += "<a href='" + rootUrl + item.Controller + "/" + item.Action + "/'>" + item.Menu + "</a>";
                    htmlString += "<a href='" + rootUrl + "Module/Menu/" + item.Id + "/' >" + item.Menu + "</a>  | ";
                    htmlString += "</td>";
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
            htmlString += "<img src='" + rootUrl + service.IconPath + "' class='ModulePageIcon' alt='' />";
            htmlString += "</span>";
            htmlString += "<span>";
            htmlString += "<h2>" + service.Description + "</h2>";
            htmlString += "<p>" + service.Remarks + "</p>";
            htmlString += "</span>";
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


        #endregion
    }
}