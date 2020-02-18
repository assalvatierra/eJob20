using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JobsV1.Models.Class
{
    public class cUserPerformance {
        public int Id { get; set; }
        public string UserName { get; set; }
        public int Quotation { get; set; }
        public int Meeting { get; set; }
        public int Sales { get; set; }
        public decimal Amount { get; set; }
    }

    public class ActivityClass
    {

        private JobDBContainer db = new JobDBContainer();
        private DBClasses dbc = new DBClasses();

        public List<CustEntActivity> GetCompanyActivities(){

            List<CustEntActivity> activity = new List<CustEntActivity>();
            
            //sql query with comma separated item list
            string sql =
               @" 
                SELECT * FROM CustEntActivities ;";

            activity = db.Database.SqlQuery<CustEntActivity>(sql).ToList();
            
            return activity;
        }


        public List<SupplierActivity> GetSupplierActivities()
        {

            List<SupplierActivity> activity = new List<SupplierActivity>();

            //sql query with comma separated item list
            string sql =
               @" SELECT * CustEntActivities";

            activity = db.Database.SqlQuery<SupplierActivity>(sql).ToList();

            return activity;
        }


        // GET: Supplier Activities
        public IOrderedEnumerable<SupplierActivity> GetSupplierActivitiesUser(string user, DateTime sdate, DateTime edate)
        {
            //get activities of the user
            var companyActivity = db.SupplierActivities
                .Where(c => c.DtActivity.CompareTo(sdate) >= 0 && c.DtActivity.CompareTo(edate) <= 0 && c.Assigned == user)
                .ToList();
            return companyActivity.OrderByDescending(s => s.DtActivity);
        }


        // GET: Supplier Activities
        public IOrderedEnumerable<SupplierActivity> GetSupplierActivitiesAdmin(DateTime sdate, DateTime edate)
        {
            //get activities of all users
            var companyActivity = db.SupplierActivities
                .Where(c => c.DtActivity.CompareTo(sdate) >= 0 && c.DtActivity.CompareTo(edate) <= 0)
                .ToList();
            return companyActivity.OrderByDescending(s => s.DtActivity);
        }

        // GET: Company Activities
        public IOrderedEnumerable<CustEntActivity> GetCompanyActivitiesUser(string user, DateTime sdate, DateTime edate)
        {
            //get activities of the user
            var companyActivity = db.CustEntActivities
                .Where(c => c.Date.CompareTo(sdate) >= 0 && c.Date.CompareTo(edate) <= 0 && c.Assigned == user)
                .ToList();

            return companyActivity.OrderByDescending(s => s.Date);
        }


        // GET: Company Activities
        public IOrderedEnumerable<CustEntActivity> GetCompanyActivitiesAdmin(DateTime sdate, DateTime edate)
        {
            //get activities of all users
            var companyActivity = db.CustEntActivities
                .Where(c=>c.Date.CompareTo(sdate) >= 0 && c.Date.CompareTo(edate) <= 0)
                .ToList();
            return companyActivity.OrderByDescending(s => s.Date);
        }

        #region Performance Report
        public List<cUserPerformance> GetUserPerformance(DateTime sdate, DateTime edate)
        {
            List<cUserPerformance> userReport = new List<cUserPerformance>();

            string sql =
               " SELECT	UserName,"+
		       "         Quotation = (SELECT COUNT(*) FROM CustEntActivities ca WHERE ca.Type = 'Quotation' AND au.UserName = ca.Assigned AND convert(datetime, ca.Date) > convert(datetime,'"+ sdate + "') AND convert(datetime, ca.Date) < convert(datetime,'"+ edate + "') ),"+
               "         Meeting = (SELECT COUNT(*) FROM CustEntActivities ca WHERE ca.Type = 'Meeting' AND au.UserName = ca.Assigned AND convert(datetime, ca.Date) > convert(datetime,'" + sdate + "') AND convert(datetime, ca.Date) < convert(datetime,'" + edate + "') )," +
               "         Sales = (SELECT COUNT(*) FROM CustEntActivities ca WHERE ca.Type = 'Sales' AND au.UserName = ca.Assigned AND convert(datetime, ca.Date) > convert(datetime,'" + sdate + "') AND convert(datetime, ca.Date) < convert(datetime,'" + edate + "') )," +
               "         Amount = (SELECT ISNULL(SUM(Amount),0) FROM CustEntActivities ca WHERE ca.Type = 'Sales' AND au.UserName = ca.Assigned AND convert(datetime, ca.Date) > convert(datetime,'" + sdate + "') AND convert(datetime, ca.Date) < convert(datetime,'" + edate + "') )" +
               "  FROM AspNetUsers au "+
               "  ORDER BY Sales DESC, Meeting DESC, Quotation Desc ;";

            userReport = db.Database.SqlQuery<cUserPerformance>(sql).ToList();


            return userReport;
        }


        #endregion 
    }
}