using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JobsV1.Models.Class
{
    public class ActionTrailClass
    {

        private JobDBContainer db = new JobDBContainer();
        DateClass dt = new DateClass();

        // action trails
        // 
        //
        public void recordTrail(string jobTable, string user, string action)
        {
            JobTrail trail = new JobTrail();
            trail.user = user;
            trail.Action = action;
            trail.RefTable = jobTable;
            trail.dtTrail = dt.GetCurrentDateTime();
            trail.RefId = "0";
            trail.IPAddress = GetIPAddress();

            db.JobTrails.Add(trail);
            db.SaveChanges();

        }

        //override
        public void recordTrail(string jobTable, string user, string action, string refId)
        {
            JobTrail trail = new JobTrail();
            trail.user = user;
            trail.Action = action;
            trail.RefTable = jobTable;
            trail.dtTrail = dt.GetCurrentDateTime();
            trail.RefId = refId;
            trail.IPAddress = GetIPAddress();

            db.JobTrails.Add(trail);
            db.SaveChanges();

        }

        protected string GetIPAddress()
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            string ipAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (!string.IsNullOrEmpty(ipAddress))
            {
                string[] addresses = ipAddress.Split(',');
                if (addresses.Length != 0)
                {
                    return addresses[0];
                }
            }

            return context.Request.ServerVariables["REMOTE_ADDR"];
        }
    }
}