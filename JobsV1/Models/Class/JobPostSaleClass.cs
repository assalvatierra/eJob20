using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JobsV1.Models.Class
{
    public class cJobsPostSale
    {
        public int Id { get; set; }
        public int? PostSaleId { get; set; }
        public int JobServiceId { get; set; }
        public int JobMainId { get; set; }
        public int ServiceId { get; set; }

        public DateTime JobDate { get; set; }
        public DateTime DtStart { get; set; }
        public DateTime DtEnd { get; set; }

        public string Customer { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }

        public string Vehicle { get; set; }
        public string Service { get; set; }
        public string Particulars { get; set; }
        public string Remarks { get; set; }
        public decimal ActualAmt { get; set; }
        public int SupplierId { get; set; }
        public int SupplierItemId { get; set; }
        public string SupplierItem { get; set; }

        public DateTime FollowUpDate { get; set; }
        public int? JobPostSalesStatusId { get; set; }
        public string PostSalesStatus { get; set; }
        public string DoneBy { get; set; }
        public string PostSaleRemarks { get; set; }
    }

    public class JobPostSaleClass
    {

        private JobDBContainer db = new JobDBContainer();

        public List<cJobsPostSale> GetJobsPostSalesPending(int? serviceId, int? statusId)

        {
            string sqlstr = @"
                            SELECT DISTINCT JobServiceId = js.Id, Customer = cu.Name, Mobile = jm.CustContactNumber, Email = jm.CustContactEmail, Service = s.Name,
                                jm.JobDate, Vehicle = jm.Description, postSaleId = jps.Id, js.*, SupplierItem = si.Description, si.Interval, 
                                FollowUpDate = DATEADD(DAY, ISNULL(si.Interval, 0), js.DtStart), PostSalesStatus = ps.Status, jps.JobPostSalesStatusId,
                                jps.DoneBy , PostSaleRemarks = jps.Remarks, PostSaleRemarks = jps.Remarks 
                                FROM JobMains jm 
                                LEFT JOIN JobServices js ON js.JobMainId = jm.Id  
                                LEFT JOIN SupplierItems si ON js.SupplierItemId = si.Id 
                                LEFT JOIN Services s ON js.ServicesId = s.Id 
                                LEFT JOIN JobPostSales jps ON js.Id = jps.JobServicesId 
                                LEFT JOIN JobPostSalesStatus ps ON jps.JobPostSalesStatusId = ps.Id 
                                LEFT JOIN Customers cu ON jm.CustomerId = cu.Id 
                                WHERE JobStatusId = 4 AND ISNULL(si.Interval, 0) > 0 
                                AND convert(datetime, GETDATE()) >= DATEADD(DAY, ISNULL(si.Interval, 0), js.DtStart) 
                            ";

            if (statusId != null)
            {
                sqlstr += " AND jps.JobPostSalesStatusId = "+ statusId +" ";
            }
            else
            {
                sqlstr += " AND ISNULL(jps.JobPostSalesStatusId, 0) < 3 ";
            }

            if ( serviceId != null && serviceId != 0 )
            {
                sqlstr += " AND js.ServicesId = " + serviceId + " ";
            }

            List<cJobsPostSale> postSales = db.Database.SqlQuery<cJobsPostSale>(sqlstr).ToList();
            return postSales;
        }
    }
}