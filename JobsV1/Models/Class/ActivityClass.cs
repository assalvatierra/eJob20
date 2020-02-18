using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JobsV1.Models.Class
{
    public class ActivityClass
    {

        private JobDBContainer db = new JobDBContainer();

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
    }
}