using System;
using System.Collections.Generic;
using System.Linq;

namespace JobsV1.Models.Class
{
    #region Helper Class
    public class cUserPerformance {
        public int Id { get; set; }
        public string UserName { get; set; }
        public int Quotation { get; set; }
        public int Meeting { get; set; }
        public int Sales { get; set; }
        public int Procurement { get; set; }
        public decimal Amount { get; set; }
        public string Role { get; set; }
    }

    public class cUserPerformanceReport
    {
        public string User { get; set; }
        public int Sales { get; set; }
        public int Quotation { get; set; }
        public int Meeting { get; set; }
        public int Procurement { get; set; }
        public int CallsAndEmail { get; set; }
    }

    public class cUserSalesReport
    {
        public string User { get; set; }
        public decimal TotalSales { get; set; }
        public decimal TotalQuotation { get; set; }
        public decimal TotalProcurement { get; set; }
        public decimal TotalJobOrder { get; set; }
    }
    public class cUserActivity : CustEntActivity
    {
        public string Company { get; set; }
        public int Points { get; set; }

        //remove @email from user for display
        public string UserRemoveEmail(string input)
        {
            try
            {
                char ch = '@';
                int idx = input.IndexOf(ch);
                return input.Substring(0, idx);
            }
            catch (Exception ex)
            {
                return input;
            }

        }
    }

    public class cUserRole
    {
        public string UserName { get; set; }
        public string UserRole { get; set; }
    }

    #endregion
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
        public List<cUserPerformance> GetUserPerformanceReport(DateTime sdate, DateTime edate)
        {
            List<cUserPerformance> userReport = new List<cUserPerformance>();

            string sql =
               " SELECT	UserName,"+
		       "         Quotation = (SELECT COUNT(*) FROM CustEntActivities ca WHERE ca.ActivityType = 'Quotation' AND au.UserName = ca.Assigned AND convert(datetime, ca.Date) > convert(datetime,'"+ sdate + "') AND convert(datetime, ca.Date) < convert(datetime,'"+ edate + "') ),"+
               "         Meeting = (SELECT COUNT(*) FROM CustEntActivities ca WHERE ca.ActivityType = 'Meeting' AND au.UserName = ca.Assigned AND convert(datetime, ca.Date) > convert(datetime,'" + sdate + "') AND convert(datetime, ca.Date) < convert(datetime,'" + edate + "') )," +
               "         Sales = (SELECT COUNT(*) FROM CustEntActivities ca WHERE ca.ActivityType = 'Sales' AND au.UserName = ca.Assigned AND convert(datetime, ca.Date) > convert(datetime,'" + sdate + "') AND convert(datetime, ca.Date) < convert(datetime,'" + edate + "') )," +
               "         Procurement = (SELECT COUNT(*) FROM CustEntActivities ca WHERE ca.ActivityType = 'Procurement' AND au.UserName = ca.Assigned AND convert(datetime, ca.Date) > convert(datetime,'" + sdate + "') AND convert(datetime, ca.Date) < convert(datetime,'" + edate + "') )," +
               "         Amount = (SELECT ISNULL(SUM(Amount),0) FROM CustEntActivities ca WHERE ca.ActivityType = 'Sales' AND au.UserName = ca.Assigned AND convert(datetime, ca.Date) > convert(datetime,'" + sdate + "') AND convert(datetime, ca.Date) < convert(datetime,'" + edate + "') )" +
               "  FROM AspNetUsers au "+

               //"  Where UserName NOT IN " +
               //" ('jahdielvillosa@gmail.com' ,'jahdielsvillosa@gmail.com', 'assalvatierra@gmail.com', " +
               //" 'admin@gmail.com' ,'demo@gmail.com', 'assalvatierra@yahoo.com' )" +

               "  ORDER BY Sales DESC, Meeting DESC, Quotation Desc ;";

            userReport = db.Database.SqlQuery<cUserPerformance>(sql).ToList();

            return userReport;
        }

        public string GetUserRole(string user)
        {
            if (!String.IsNullOrEmpty(user))
            {

                string sql = @"SELECT UserName, UserRole = (SELECT Name FROM AspNetRoles r WHERE r.Id = ur.RoleId) FROM AspNetUsers u
	                            LEFT JOIN AspNetUserRoles ur ON ur.UserId = u.Id
	                            WHERE UserName = '"+ user +"' ;";

                var Role = db.Database.SqlQuery<cUserRole>(sql).FirstOrDefault();
                return Role.UserRole;
            }

            return "NA";
        }

        #endregion


        #region UserActivities

        //GET : get user activities by the user 
        public List<cUserActivity> GetUserActivities(string user, string sDate, string eDate)
        {
            //eDate = DateTime.Parse(eDate).AddDays(1).ToShortDateString();
            List<cUserActivity> activity = new List<cUserActivity>();
            string dateQuery = "";
            if (sDate != "" && eDate != "")
            {
                dateQuery = " AND (Date >= convert(datetime, '" + sDate + "') AND Date <= convert(datetime, '" + eDate + "'))  ";
            }

            //sql query with comma separated item list
            string sql =
               @" SELECT *, Company = (SELECT Name FROM CustEntMains cem WHERE cem.Id = act.CustEntMainId ),
                  Points = (SELECT Points FROM CustEntActivityTypes type WHERE type.Type = act.ActivityType)
                  FROM CustEntActivities act WHERE " +
                  "Assigned = '" + user + "' "+ dateQuery + " ORDER BY Date DESC ;";

            activity = db.Database.SqlQuery<cUserActivity>(sql).ToList();

            return activity;
        }

        //GET : return user performance report based on the count of each Activity Type
        public cUserPerformanceReport GetUserPerformance(List<cUserActivity> activities, string user)
        {
            cUserPerformanceReport performance = new cUserPerformanceReport();

            //get counts of each activity
            performance.User = dbc.UserRemoveEmail(user);
            performance.Sales = activities.Where(a => a.ActivityType == "Sales").Count();
            performance.Meeting = activities.Where(a => a.ActivityType == "Meeting").Count();
            performance.Quotation = activities.Where(a => a.ActivityType == "Quotation").Count();
            performance.Procurement = activities.Where(a => a.ActivityType == "Procurement").Count();
            performance.CallsAndEmail = activities.Where(a => a.ActivityType == "Calls/Email").Count();


            return performance;
        }

        //GET : return user performance report based on the total amount of each Activity Type
        public cUserSalesReport GetUserSales(List<cUserActivity> activities, string user)
        {
            cUserSalesReport sales = new cUserSalesReport();
            sales.TotalSales = 0;
            sales.TotalQuotation = 0;
            sales.TotalProcurement = 0;
            sales.TotalJobOrder = 0;
            //get total of each Activity Type
            foreach (var act in activities)
            {
                decimal tempAmt = act.Amount != null ? (decimal)act.Amount : 0;
                switch (act.ActivityType)
                {
                    case "Sales":
                        sales.TotalSales += tempAmt;
                        break;
                    case "Quotation":
                        sales.TotalQuotation += tempAmt;
                        break;
                    case "Procurement":
                        sales.TotalProcurement += tempAmt;
                        break;
                    case "Job Order":
                        sales.TotalJobOrder += tempAmt;
                        break;
                    default:
                        break;
                }
            }

            sales.User = dbc.UserRemoveEmail(user);


            return sales;
        }
        #endregion
    }
}