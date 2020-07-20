using JobsV1.Controllers;
using JobsV1.Models.Class;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace JobsV1.Models
{
    public class cJobOrderListing
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Customer { get; set; }
        public string Company { get; set; }
        public decimal Amount { get; set; }
        public decimal Payment { get; set; }
        public decimal PaymentFromId { get; set; }
        public DateTime JobDate { get; set; }
        public int Status { get; set; }
        public DateTime DtStart { get; set; }
        public DateTime DtEnd { get; set; }
    }

    public class cJobOrderServices
    {
        public int Id { get; set; }
        public string Particulars { get; set; }
        public string Supplier { get; set; }
        public string SupplierItem { get; set; }
        public decimal ActualAmt { get; set; }
        public DateTime DtStart { get; set; }
        public DateTime DtEnd { get; set; }
        public List<cUnitList> cUnits { get; set; }
        public string JobPickup { get; set; }
        public string ServiceType { get; set; }
        public string Remarks { get; set; }
    }

    public class cJobOnProgress
    {
        public int Id { get; set; }
        public DateTime DtStart { get; set; }
        public DateTime DtEnd { get; set; }
    }

    public class JobOrderClass
    {
        private JobDBContainer db = new JobDBContainer();
        private DBClasses dbc = new DBClasses();
        private ActionTrailClass trail = new ActionTrailClass();
        private DateClass dt = new DateClass();

        //GET : return list of ONGOING jobs
        public List<cJobOrderListing> getJobData(int sortid)
        {
            var data = new List<cJobOrderListing>();
            DateTime today = dt.GetCurrentDate();
            var confirmed = GetJobOrderListing(sortid);

            foreach (var main in confirmed)
            {
                cJobOrderListing joTmp = main;
                joTmp.Amount = 0;
                joTmp.Payment = 0;

                //get total amount
                var totalAmountList = db.JobServices.Where(d => d.JobMainId == main.Id).ToList().Select(s=>s.ActualAmt);
                foreach (var amount in totalAmountList)
                {
                    joTmp.Amount += amount != null ? (decimal)amount : decimal.Zero;
                }

                //get total payment done
                List<Models.JobPayment> jobPayment = db.JobPayments.Where(d => d.JobMainId == main.Id).ToList();
                foreach (var payment in jobPayment)
                {
                    joTmp.Payment += payment.PaymentAmt;
                }

                //get jobdate
                // joTmp.JobDate = TempJobDate(main.Id);
                joTmp.JobDate = MinJobDate(main.DtStart, main.DtEnd);

                data.Add(joTmp);
            }
            data = data.Where(s => DateTime.Compare(s.JobDate, today) >= 0 && s.Status < 4).ToList();
            return data;
        }

        //GET : get the date of the job based on the date today
        public DateTime TempJobDate(int mainId)
        {
            //update jobdate
            var main = db.JobMains.Where(j => mainId == j.Id).FirstOrDefault();
            DateTime minDate = db.JobMains.Where(j => mainId == j.Id).FirstOrDefault().JobDate.Date;
            DateTime maxDate = new DateTime(1, 1, 1);
            DateTime today = new DateTime();

            today = dt.GetCurrentDate();

            //loop though all jobservices in the jobmain
            //to get the latest date
            var counter = 1;
            foreach (var svc in db.JobServices.Where(s => s.JobMainId == mainId).OrderBy(s => s.DtStart))
            {
                var firstService = (DateTime)db.JobServices.Where(s => s.JobMainId == mainId).OrderBy(s => s.DtStart).FirstOrDefault().DtStart;
                var svcDtStart = (DateTime)svc.DtStart;
                var svcDtEnd = (DateTime)svc.DtEnd;
                //get min date
                if (counter == 1)
                {
                    minDate = firstService;
                }

                // minDate >= (DateTime)svc.DtStart;
                if (DateTime.Compare(minDate, svcDtStart.Date) >= 0)
                {
                    minDate = svcDtStart.Date; //if minDate > Dtstart
                }

                if (DateTime.Compare(today, svcDtStart.Date) >= 0 && DateTime.Compare(today, svcDtEnd.Date) <= 0)
                {
                    minDate = today; //latest date is today or within the range of start date and end date
                    //skip
                }
                else
                {
                    if (DateTime.Compare(today, svcDtStart.Date) < 0 && DateTime.Compare(today, minDate) > 0)
                    {
                        minDate = svcDtStart.Date; //if Today < Dtstart but today is greater than smallest date
                    }
                }

                //get max date
                if (DateTime.Compare(maxDate, svcDtEnd.Date) <= 0)
                {
                    maxDate = svcDtEnd.Date;
                }
            }

            //today is equal to smallest start date
            if (DateTime.Compare(today, minDate) == 0)
            {
                main.JobDate = minDate;
            }

            //today is equal to highest end date
            if (DateTime.Compare(today, maxDate) == 0)
            {
                main.JobDate = maxDate;
            }

            //today is < smallest date
            if (DateTime.Compare(today, minDate) < 0)
            {
                main.JobDate = minDate;
            }

            //today is greater than the smallest date
            //job is currently on going, adjust date
            if (DateTime.Compare(today, minDate) > 0)
            {
                if (DateTime.Compare(today, maxDate) < 0)
                {
                    main.JobDate = today;
                }

                if (DateTime.Compare(today, maxDate) > 0)
                {
                    main.JobDate = minDate;
                }
            }

            if (minDate == new DateTime(9999, 12, 30))
            {
                main.JobDate = minDate;
            }

            return main.JobDate;
            //return minDate;
        }

        //GET  : lastest date of the job based on the date today from two dates
        //P    : starDate , endDate 
        public DateTime MinJobDate(DateTime startDate, DateTime endDate)
        {
            //update jobdate

            DateTime finalDate = startDate;
            DateTime minDate = startDate;
            DateTime maxDate = endDate;

            DateTime today = new DateTime();
            today = dt.GetCurrentDate();

            //get min date

            //today is equal to smallest start date
            if (DateTime.Compare(today, minDate) == 0)
            {
                finalDate = minDate;
            }

            //today is equal to highest end date
            if (DateTime.Compare(today, maxDate) == 0)
            {
                finalDate = maxDate;
            }

            //today is < smallest date
            if (DateTime.Compare(today, minDate) < 0)
            {
                finalDate = minDate;
            }

            //today is greater than the smallest date
            //job is currently on going, adjust date
            if (DateTime.Compare(today, minDate) > 0)
            {
                if (DateTime.Compare(today, maxDate) < 0)
                {
                    finalDate = today;
                }

                if (DateTime.Compare(today, maxDate) > 0)
                {
                    finalDate = minDate;
                }
            }

            if (minDate == new DateTime(9999, 12, 30))
            {
                finalDate = minDate;
            }

            //return main.JobDate;
            return finalDate;
        }

        //GET jobcount 
        public List<cjobCounter> getJobActionCount(List<Int32> jobidlist)
        {
            #region sqlstr
            string sqlstr = @"
select max(x.jobid) JobId, x.Id CodeId, max(x.code) CodeDesc, sum(x.ActionCount) CntItem, sum(x.DoneCount) CntDone
from 
(

select max(a.JobMainId) jobid , d.Id, max(d.CatCode) code, '0' as ActionCount, count(b.Id) DoneCount
from JobServices a
left outer join JobActions b on a.Id = b.JobServicesId
left outer join SrvActionitems c on b.SrvActionItemId = c.Id
left outer join SrvActionCodes d on c.SrvActionCodeId = d.Id
Group by a.JobMainId,d.Id

union

select max(a.JobMainId) jobid , d.Id, max(d.CatCode) code, count(c.Id) as ActionCount, '0' as DoneCount
from JobServices a
left outer join [Services] b on a.ServicesId = b.Id
left outer join SrvActionitems c on b.Id = c.ServicesId
left outer join SrvActionCodes d on c.SrvActionCodeId = d.Id
Group by a.JobMainId,d.Id
)x Group by x.jobid, x.Id
order by x.jobid
;

";
            #endregion
            List<cjobCounter> jobcntr = db.Database.SqlQuery<cjobCounter>(sqlstr).Where(d => jobidlist.Contains(d.JobId)).ToList();
            return jobcntr;
        }

        //GET JobMainId
        //Filter nullable serviceId and mainId
        public int GetJobMainId(int? serviceId, int? maindId)
        {
            var jobMainId = serviceId != null ? db.JobServices.Find(serviceId).JobMainId : 0;
            jobMainId = maindId != null ? (int)maindId : jobMainId;
            return jobMainId;
        }

        //Get Job Order Listing based on the sort
        public List<cJobOrderListing> GetJobOrderListing(int sortid)
        {
            List<cJobOrderListing> joblist = new List<cJobOrderListing>();

            string sql = "";

            //filter jobs based on statusId and date
            switch (sortid)
            {
                case 1: //OnGoing
                    sql = @"SELECT Id = MIN(job.Id), DtStart = MIN(job.DtStart), DtEnd = MIN(job.DtEnd), 
	                                Description = MIN(job.Description), Customer = MIN(job.Customer), Status = MIN(job.JobStatusId)  
	                                FROM ( SELECT jm.Id,  jm.Description, jm.JobStatusId, js.DtStart, js.DtEnd,
		                            Customer = (SELECT c.Name FROM Customers c WHERE c.Id = jm.CustomerId)
		                            FROM JobMains jm LEFT JOIN JobServices js ON jm.Id = js.JobMainId ) job
		                            WHERE job.DtStart >= convert(datetime, GETDATE()) 
                                    OR (job.DtStart <= convert(datetime, GETDATE()) AND job.DtEnd >= convert(datetime, GETDATE()))
		                            AND job.JobStatusId < 4 GROUP BY job.Id ORDER BY DtStart";
                    break;
                case 2: //prev
                    sql = "select j.Id from JobMains j where j.JobStatusId < 4 AND MONTH(j.JobDate) = MONTH(GETDATE()) AND YEAR(j.JobDate) = YEAR(GETDATE()) ;";
                    break;
                case 3: //close
                    sql = "select j.Id from JobMains j where j.JobStatusId > 3 AND j.JobDate >= DATEADD(DAY, -120, GETDATE());";
                    break;
                default:
                    sql = "select * from JobMains j where j.JobStatusId < 4 AND j.JobDate >= DATEADD(DAY, -30, GETDATE());";
                    break;
            }

            //terminator
            sql += ";";

            joblist = db.Database.SqlQuery<cJobOrderListing>(sql).ToList();

            return joblist;

        }


        //Get Job Order Listing based on the sort
        public List<cJobOrderListing> GetJobOrderOnProgress(int sortid)
        {
            List<cJobOrderListing> joblist = new List<cJobOrderListing>();

            string sql = "";

            //filter jobs based on statusId and date
            switch (sortid)
            {
                case 1: //OnGoing
                    sql = @"SELECT Id = MIN(job.Id), DtStart = MIN(job.DtStart), DtEnd = MIN(job.DtEnd)
		                            FROM JobMains jm LEFT JOIN JobServices js ON jm.Id = js.JobMainId ) job
		                            WHERE job.DtStart >= convert(datetime, GETDATE()) 
                                    OR (job.DtStart <= convert(datetime, GETDATE()) AND job.DtEnd >= convert(datetime, GETDATE()))
		                            AND job.JobStatusId < 4 GROUP BY job.Id ORDER BY DtStart";
                    break;
                case 2: //prev
                    sql = "select j.Id from JobMains j where j.JobStatusId < 4 AND MONTH(j.JobDate) = MONTH(GETDATE()) AND YEAR(j.JobDate) = YEAR(GETDATE()) ;";
                    break;
                case 3: //close
                    sql = "select j.Id from JobMains j where j.JobStatusId > 3 AND j.JobDate >= DATEADD(DAY, -120, GETDATE());";
                    break;
                default:
                    sql = "select * from JobMains j where j.JobStatusId < 4 AND j.JobDate >= DATEADD(DAY, -30, GETDATE());";
                    break;
            }

            //terminator
            sql += ";";

            joblist = db.Database.SqlQuery<cJobOrderListing>(sql).ToList();

            return joblist;

        }

        //GET : the lastest date of the job based on the date today
        public DateTime GetMinMaxJobDate(int mainId, string getType)
        {
            //update jobdate
            var main = db.JobMains.Where(j => mainId == j.Id).FirstOrDefault();

            DateTime minDate = main.JobDate;
            DateTime maxDate = main.JobDate;

            //loop though all jobservices in the jobmain
            //to get the latest date
            foreach (var svc in db.JobServices.Where(s => s.JobMainId == mainId).OrderBy(s => s.DtStart))
            {

                var svcDtStart = (DateTime)svc.DtStart;
                var svcDtEnd = (DateTime)svc.DtEnd;
                //get min date

                // minDate >= (DateTime)svc.DtStart;
                if (DateTime.Compare(minDate, svcDtStart.Date) >= 0)
                {
                    minDate = svcDtStart; //if minDate > Dtstart
                }

                //get max date
                if (DateTime.Compare(maxDate, svcDtEnd.Date) <= 0)
                {
                    maxDate = svcDtEnd;
                }
            }

            //return main.JobDate;
            if (getType.ToLower() == "min")
                return minDate;
            else if (getType.ToLower() == "max")
                return maxDate;
            else
                return minDate;

        }

        public decimal GetJobDiscountAmount(int jobId)
        {
            //get job discount payments of job
            var jobpayments = db.JobPayments.Where(p => p.JobMainId == jobId && p.JobPaymentTypeId == 4).ToList();
            if (jobpayments.Count != 0)
            {
                decimal totalDiscount = 0;

                //get total discount
                jobpayments.ForEach(p =>
                    totalDiscount += p.PaymentAmt
                    );

                return totalDiscount;
            }
            return 0;
        }


    }
}