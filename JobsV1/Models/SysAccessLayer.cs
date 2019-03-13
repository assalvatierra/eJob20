using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JobsV1
{
    public class SysAccessLayer
    {
        Models.SysDBContainer db = new Models.SysDBContainer();

        public string getSysSetting(string sKey)
        {
            string sValue = "";
            try
            {
                Models.SysSetting tmp = db.SysSettings.Where(d => d.SysKey == sKey).FirstOrDefault();
                if (tmp != null)
                    sValue = tmp.SysValue.ToString().Trim();
            }
            catch { }

            return sValue;
        }

        public List<Models.SysService> getServiceModules(string user)
        {
            var userAccess = db.SysAccessUsers.Where(d => d.UserId == user).OrderBy(o => o.Seqno).Select(s => s.SysMenuId);
            var userMenu = db.SysServiceMenus.Where(d => userAccess.Contains(d.SysMenuId)).OrderBy(o => o.SysMenu.Seqno).Select(s => s.SysServiceId);
            var userServices = db.SysServices.Where(d => userMenu.Contains(d.Id));

            List<Models.SysService> Services = new List<Models.SysService>();
            foreach (int iSau in userAccess)
            {
                foreach (Models.SysService ss in userServices)
                {
                    foreach (Models.SysServiceMenu ssm in ss.SysServiceMenus)
                    {
                        if (ssm.SysMenuId == iSau)
                            Services.Add(ss);
                    }
                }
            }

            return Services;
        }

        public ModRoute getMenuRoute(int id)
        {
            string sArea = "";
            Models.SysMenu menu = db.SysMenus.Find(id);
            string sParamStr = menu.Params;
            if (sParamStr != null && sParamStr.Trim() != "")
            {
                string[] sParams = sParamStr.Split(',');
                foreach (string s in sParams)
                {
                    if (s.Split(':').Count() > 1)
                    {
                        string sParamName = s.Split(':')[0];
                        string sParamvalue = s.Split(':')[1];
                        if (sParamName.ToUpper() == "AREA")
                            sArea = sParamvalue;
                    }
                }
            }

            if (menu == null)
                return new ModRoute { Controller = "Module", Action = "notFound", RootMenuId = 0 };
            else

                return new ModRoute
                {
                    Controller = menu.Controller,
                    Action = menu.Action,
                    RootMenuId = menu.Id,
                    Area = sArea
                };

            //       return new ModRoute { Controller = "Module", Action = "notProcessed" };
        }

        public ModRoute getModRoute(int id)
        {
            Models.SysServiceMenu ssm = db.SysServiceMenus.Where(d => d.SysServiceId == id).FirstOrDefault();
            string sArea = "";

            string sParamStr = ssm.SysMenu.Params;
            if (sParamStr != null && sParamStr.Trim() != "")
            {
                string[] sParams = sParamStr.Split(',');
                foreach (string s in sParams)
                {
                    if (s.Split(':').Count() > 1)
                    {
                        string sParamName = s.Split(':')[0];
                        string sParamvalue = s.Split(':')[1];
                        if (sParamName.ToUpper() == "AREA")
                            sArea = sParamvalue;
                    }
                }
            }

            if (ssm == null)
                return new ModRoute { Controller = "Module", Action = "notFound", RootMenuId = 0 };
            else

                return new ModRoute
                {
                    Controller = ssm.SysMenu.Controller,
                    Action = ssm.SysMenu.Action,
                    RootMenuId = ssm.SysMenuId,
                    Area = sArea
                };

            //       return new ModRoute { Controller = "Module", Action = "notProcessed" };
        }



        #region getMenu
        public IList<Models.SysMenu> getModMenu(int id)
        {
            var sMenu = this.getSubMenu(id).OrderBy(s => s.Seqno).ToList();
            return sMenu;
        }

        public IList<Models.SysMenu> getSubMenu(int id)
        {
            List<Models.SysMenu> sMenu = new List<Models.SysMenu>();
            sMenu.Add(db.SysMenus.Find(id));
            
            var subMenus = db.SysMenus.Where(d => d.ParentId == id);
            foreach (Models.SysMenu m in subMenus)
            {
                var sm = this.getSubMenu(m.Id);
                sMenu.AddRange(sm);
            }
            return sMenu;
        }
        #endregion
        public Models.SysService getModule(int MenuId)
        {
            int ServiceMenuId = -1;
            int parentId = 99;
            int iteration = 10;

            while (parentId != 0 && iteration > 0)
            {
                Models.SysMenu menu = db.SysMenus.Find(MenuId);
                if (menu.SysServiceMenus.Count() > 0)
                {
                    ServiceMenuId = menu.SysServiceMenus.First().SysServiceId;
                    break;
                }
                else //try upper level
                {
                    MenuId = (int)menu.ParentId;
                }

                if (MenuId == 0) //root menu reached
                    break;

                iteration--;
            }

            return db.SysServices.Find(ServiceMenuId);
        }

    }

    public class ModRoute
    {
        public string Controller { get; set; }
        public string Action { get; set; }
        public string Area { get; set; }
        public int RootMenuId { get; set; }
    }

}