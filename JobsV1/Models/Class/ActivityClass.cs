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
        public int ProcMeeting { get; set; }
        public int Procurement { get; set; }
        public int JobOrder { get; set; }
        public decimal Amount { get; set; }
        public decimal ProcAmount { get; set; }
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
        public int Close { get; set; }
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
            catch
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
           var tempDate = edate;
           edate = tempDate.AddDays(1);

            List<cUserPerformance> userReport = new List<cUserPerformance>();

            string sql =
               " SELECT	UserName,"+
		       "         Quotation = (SELECT COUNT(*) FROM CustEntActivities ca WHERE ca.ActivityType = 'Quotation' AND au.UserName = ca.Assigned AND convert(datetime, ca.Date) > convert(datetime,'"+ sdate + "') AND convert(datetime, ca.Date) < convert(datetime,'"+ edate + "') ),"+
               "         Meeting = (SELECT COUNT(*) FROM CustEntActivities ca WHERE ca.ActivityType = 'Meeting' AND au.UserName = ca.Assigned AND convert(datetime, ca.Date) > convert(datetime,'" + sdate + "') AND convert(datetime, ca.Date) < convert(datetime,'" + edate + "') )," +
               "         Sales = (SELECT COUNT(*) FROM CustEntActivities ca WHERE ca.ActivityType = 'Sales' AND au.UserName = ca.Assigned AND convert(datetime, ca.Date) > convert(datetime,'" + sdate + "') AND convert(datetime, ca.Date) < convert(datetime,'" + edate + "') )," +
               "         ProcMeeting = (SELECT COUNT(*) FROM SupplierActivities sa WHERE sa.ActivityType = 'Meeting' AND au.UserName = sa.Assigned AND convert(datetime, sa.DtActivity) > convert(datetime,'" + sdate + "') AND convert(datetime, sa.DtActivity) < convert(datetime,'" + edate + "') )," +
               "         Procurement = (SELECT COUNT(*) FROM SupplierActivities sa WHERE sa.ActivityType = 'Procurement' AND au.UserName = sa.Assigned AND convert(datetime, sa.DtActivity) > convert(datetime,'" + sdate + "') AND convert(datetime, sa.DtActivity) < convert(datetime,'" + edate + "') )," +
               "         JobOrder = (SELECT COUNT(*) FROM SupplierActivities ca WHERE ca.ActivityType = 'Job Order' AND au.UserName = ca.Assigned AND convert(datetime, ca.DtActivity) > convert(datetime,'" + sdate + "') AND convert(datetime, ca.DtActivity) < convert(datetime,'" + edate + "') )," +
               "         Amount = (SELECT ISNULL(SUM(Amount),0) FROM CustEntActivities ca WHERE ca.ActivityType = 'Sales' AND au.UserName = ca.Assigned AND convert(datetime, ca.Date) > convert(datetime,'" + sdate + "') AND convert(datetime, ca.Date) < convert(datetime,'" + edate + "') )," +
               "         ProcAmount = (SELECT ISNULL(SUM(Amount),0) FROM SupplierActivities ca WHERE ca.ActivityType = 'Job Order' AND au.UserName = ca.Assigned AND convert(datetime, ca.DtActivity) > convert(datetime,'" + sdate + "') AND convert(datetime, ca.DtActivity) < convert(datetime,'" + edate + "') )" +
               "  FROM AspNetUsers au " +

               "  Where UserName NOT IN " +
               " ('jahdielvillosa@gmail.com' ,'jahdielsvillosa@gmail.com', 'assalvatierra@gmail.com', " +
               " 'admin@gmail.com' ,'demo@gmail.com', 'assalvatierra@yahoo.com' )" +

               "  ORDER BY Sales DESC, Meeting DESC, Quotation Desc ;";

            userReport = db.Database.SqlQuery<cUserPerformance>(sql).ToList();

            //Update total meeting Count
            userReport = UpdateTotalMeeting(userReport);

            return userReport;
        }

        //Override
        public List<cUserPerformance> GetUserPerformanceReport(string user,DateTime sdate, DateTime edate)
        {
            var tempDate = edate;
            edate = tempDate.AddDays(1);

            List<cUserPerformance> userReport = new List<cUserPerformance>();

            string sql =
               " SELECT	UserName," +
               "         Quotation = (SELECT COUNT(*) FROM CustEntActivities ca WHERE ca.ActivityType = 'Quotation' AND au.UserName = ca.Assigned AND convert(datetime, ca.Date) > convert(datetime,'" + sdate + "') AND convert(datetime, ca.Date) < convert(datetime,'" + edate + "') )," +
               "         Meeting = (SELECT COUNT(*) FROM CustEntActivities ca WHERE ca.ActivityType = 'Meeting' AND au.UserName = ca.Assigned AND convert(datetime, ca.Date) > convert(datetime,'" + sdate + "') AND convert(datetime, ca.Date) < convert(datetime,'" + edate + "') )," +
               "         Sales = (SELECT COUNT(*) FROM CustEntActivities ca WHERE ca.ActivityType = 'Sales' AND au.UserName = ca.Assigned AND convert(datetime, ca.Date) > convert(datetime,'" + sdate + "') AND convert(datetime, ca.Date) < convert(datetime,'" + edate + "') )," +
               "         ProcMeeting = (SELECT COUNT(*) FROM SupplierActivities sa WHERE sa.ActivityType = 'Meeting' AND au.UserName = sa.Assigned AND convert(datetime, sa.DtActivity) > convert(datetime,'" + sdate + "') AND convert(datetime, sa.DtActivity) < convert(datetime,'" + edate + "') )," +
               "         Procurement = (SELECT COUNT(*) FROM SupplierActivities ca WHERE ca.ActivityType = 'Procurement' AND au.UserName = ca.Assigned AND convert(datetime, ca.DtActivity) > convert(datetime,'" + sdate + "') AND convert(datetime, ca.DtActivity) < convert(datetime,'" + edate + "') )," +
               "         JobOrder = (SELECT COUNT(*) FROM SupplierActivities ca WHERE ca.ActivityType = 'Job Order' AND au.UserName = ca.Assigned AND convert(datetime, ca.DtActivity) > convert(datetime,'" + sdate + "') AND convert(datetime, ca.DtActivity) < convert(datetime,'" + edate + "') )," +
               "         Amount = (SELECT ISNULL(SUM(Amount),0) FROM CustEntActivities ca WHERE ca.ActivityType = 'Sales' AND au.UserName = ca.Assigned AND convert(datetime, ca.Date) > convert(datetime,'" + sdate + "') AND convert(datetime, ca.Date) < convert(datetime,'" + edate + "') )" +
               "  FROM AspNetUsers au " +

               "  Where UserName = '"+ user + "' " +

               "  ORDER BY Sales DESC, Meeting DESC, Quotation Desc ;";

            userReport = db.Database.SqlQuery<cUserPerformance>(sql).ToList();

            //Update total meeting Count
            userReport = UpdateTotalMeeting(userReport);

            return userReport;
        }


        private List<cUserPerformance> UpdateTotalMeeting(List<cUserPerformance> userPerf)
        {
            foreach (var perf in userPerf)
            {
                    perf.Meeting += perf.ProcMeeting;
            }

            return userPerf;
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
            if (!String.IsNullOrEmpty(eDate))
            {
                var tempDate = DateTime.Parse(eDate);
                eDate = tempDate.AddDays(1).ToShortDateString();
            }

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

            //Filter and Remove points on Duplicate Activity with the same code
            activity = FilterDuplicateActivity(activity);

            return activity;
        }

        private List<cUserActivity> FilterDuplicateActivity( List<cUserActivity> activityList)
        {
            //holds the Ids of unique activity
            List<string> tempCodes = new List<string>();

            foreach (var act in activityList)
            {
                if (!tempCodes.Contains(act.SalesCode))
                {
                   
                        //if Id is not in the list, add id to the list
                        //and retain the point
                        tempCodes.Add(act.SalesCode);
                    


                }
                else
                {
                    //If Id is in the list, remove the point
                    act.Points = 0;
                }
            }

            return activityList;
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

        #region Supplier Activities
        //GET : get user activities by the user 
        public List<cUserActivity> GetSupActivities(string user, string sDate, string eDate)
        {
            //add 1 day
            if (!String.IsNullOrEmpty(eDate))
            {
                var tempDate = DateTime.Parse(eDate);
                eDate = tempDate.AddDays(1).ToShortDateString();
            }

            //eDate = DateTime.Parse(eDate).AddDays(1).ToShortDateString();
            List<cUserActivity> activity = new List<cUserActivity>();
            string dateQuery = "";
            if (sDate != "" && eDate != "")
            {
                dateQuery = " AND (DtActivity >= convert(datetime, '" + sDate + "') AND DtActivity <= convert(datetime, '" + eDate + "'))  ";
            }

            //sql query with comma separated item list
            string sql =
               @" SELECT *, Company = (SELECT Name FROM Suppliers sup WHERE sup.Id = act.SupplierId ), SupplierId as CustEntMainId,  
                  Points = (SELECT Points FROM SupplierActivityTypes type WHERE type.Type = act.ActivityType), 
                  Code as SalesCode ,DtActivity as Date
                  FROM SupplierActivities act WHERE " +
                  "Assigned = '" + user + "' " + dateQuery + " ORDER BY DtActivity DESC ;";

            activity = db.Database.SqlQuery<cUserActivity>(sql).ToList();

            //Filter and Remove points on Duplicate Activity with the same code
            activity = FilterDuplicateActivity(activity);

            return activity;
        }


        //GET : return user performance report based on the count of each Activity Type
        public cUserPerformanceReport GetSupPerformance(List<cUserActivity> activities, string user)
        {
            cUserPerformanceReport performance = new cUserPerformanceReport();

            //get counts of each activity
            performance.User = dbc.UserRemoveEmail(user);
            performance.Close = activities.Where(a => a.ActivityType == "Close").Count();
            performance.Sales = activities.Where(a => a.ActivityType == "Job Order").Count();
            performance.Meeting = activities.Where(a => a.ActivityType == "Meeting").Count();
            performance.Procurement = activities.Where(a => a.ActivityType == "Procurement").Count();


            return performance;
        }
        #endregion
    }
}