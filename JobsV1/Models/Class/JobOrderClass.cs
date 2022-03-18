using JobsV1.Controllers;
using JobsV1.Models.Class;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using JobsV1.Areas.Personel.Models;
using ArModels.Models;
using ApModels.Models;

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
        public decimal PaymentFromAR { get; set; }
        public decimal ExpenseFromAP { get; set; }
        public decimal DriversRate { get; set; }
        public DateTime JobEncodeDate { get; set; }
        public DateTime JobDate { get; set; }
        public DateTime DtStart { get; set; }
        public DateTime DtEnd { get; set; }
        public int Status { get; set; }
        public string StatusString { get; set; }
        public bool IsPosted { get; set; }
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
        private DateClass dt = new DateClass();
        private DBClasses dbc = new DBClasses();
        private JobDBContainer db = new JobDBContainer();
        private ActionTrailClass trail = new ActionTrailClass();
        private CarRentalLogDBContainer crdb = new CarRentalLogDBContainer();
        private ApDBContainer apdb = new ApDBContainer();
        private ArDBContainer ardb = new ArDBContainer();
        

        //GET : return list of jobs
        public List<cJobOrder> GetJobData(int sortid)
        {

            var confirmed = dbc.getJobConfirmedList(sortid).Select(s => s.Id);

            IEnumerable<Models.JobMain> jobMains = db.JobMains.Where(j => confirmed.Contains(j.Id))
                .Include(j => j.Customer)
                .Include(j => j.Branch)
                .Include(j => j.JobStatus)
                .Include(j => j.JobThru)
                ;

            var data = new List<cJobOrder>();

            var today = dt.GetCurrentDate();

            foreach (var main in jobMains)
            {
                cJobOrder joTmp = new cJobOrder();
                joTmp.Main = main;
                joTmp.Services = new List<cJobService>();
                joTmp.Main.AgreedAmt = 0;
                joTmp.Payment = 0;
                joTmp.Company = GetJobCompanyName(main.Id);


                List<Models.JobServices> joSvc = db.JobServices.Where(d => d.JobMainId == main.Id).OrderBy(s => s.DtStart).ToList();
                foreach (var svc in joSvc)
                {
                    cJobService cjoTmp = new cJobService();
                    cjoTmp.Service = svc;
                    var ActionDone = db.JobActions.Where(d => d.JobServicesId == svc.Id).Select(s => s.SrvActionItemId);
                    cjoTmp.SvcActionsDone = db.SrvActionItems.Where(d => d.ServicesId == svc.ServicesId && ActionDone.Contains(d.Id)).Include(d => d.SrvActionCode);
                    cjoTmp.SvcActions = db.SrvActionItems.Where(d => d.ServicesId == svc.ServicesId && !ActionDone.Contains(d.Id)).Include(d => d.SrvActionCode);
                    cjoTmp.Actions = db.JobActions.Where(d => d.JobServicesId == svc.Id).Include(d => d.SrvActionItem);
                    cjoTmp.SvcItems = db.JobServiceItems.Where(d => d.JobServicesId == svc.Id).Include(d => d.InvItem);
                    cjoTmp.SupplierPos = db.SupplierPoDtls.Where(d => d.JobServicesId == svc.Id).Include(i => i.SupplierPoHdr);
                    joTmp.Main.AgreedAmt += svc.ActualAmt;

                    joTmp.Services.Add(cjoTmp);
                }


                //get min job date
                if (sortid == 1)
                {
                    joTmp.Main.JobDate = TempJobDate(joTmp.Main.Id);
                }
                else
                {
                    joTmp.Main.JobDate = MinJobDate(joTmp.Main.Id);
                }

                joTmp.Payment += GetJobPaymentAmount(main.Id);

                data.Add(joTmp);
            }

            switch (sortid)
            {
                case 1: //OnGoing
                    data = (List<cJobOrder>)data
                       .Where(d => d.Main.JobDate.CompareTo(today.Date) >= 0).ToList();

                    break;
                case 2: //prev
                    data = (List<cJobOrder>)data
                        .Where(p => DateTime.Compare(p.Main.JobDate.Date, today.Date) < 0)
                        .ToList();
                    break;
                case 3: //close
                    data = (List<cJobOrder>)data
                        .Where(p => p.Main.JobDate.Date > today.Date.AddDays(-150)).ToList();
                    break;
                case 4: //cancelled
                    data = (List<cJobOrder>)data
                        .Where(p => p.Main.JobDate.Date > today.Date.AddDays(-150)).ToList();
                    break;

                default:
                    data = (List<cJobOrder>)data.ToList();
                    break;
            }

            return data;
        }

        //GET : return list of ONGOING jobs
        public List<cJobOrder> GetSearchJobData(string srch)
        {
            var searched = GetJobSearchQuery(srch).Select(s => s.Id);

            IEnumerable<Models.JobMain> jobMains = db.JobMains.Where(j => searched.Contains(j.Id))
                .Include(j => j.Customer)
                .Include(j => j.Branch)
                .Include(j => j.JobStatus)
                .Include(j => j.JobThru)
                ;

            var data = new List<cJobOrder>();

            DateTime today = new DateTime();
            today = dt.GetCurrentDate();

            foreach (var main in jobMains)
            {
                cJobOrder joTmp = new cJobOrder();
                joTmp.Main = main;
                joTmp.Services = new List<cJobService>();
                joTmp.Main.AgreedAmt = 0;
                joTmp.Payment = 0;
                joTmp.Company = GetJobCompany(main.Id);


                List<Models.JobServices> joSvc = db.JobServices.Where(d => d.JobMainId == main.Id).OrderBy(s => s.DtStart).ToList();
                foreach (var svc in joSvc)
                {
                    cJobService cjoTmp = new cJobService();
                    cjoTmp.Service = svc;
                    var ActionDone = db.JobActions.Where(d => d.JobServicesId == svc.Id).Select(s => s.SrvActionItemId);
                    cjoTmp.SvcActionsDone = db.SrvActionItems.Where(d => d.ServicesId == svc.ServicesId && ActionDone.Contains(d.Id)).Include(d => d.SrvActionCode);
                    cjoTmp.SvcActions = db.SrvActionItems.Where(d => d.ServicesId == svc.ServicesId && !ActionDone.Contains(d.Id)).Include(d => d.SrvActionCode);
                    cjoTmp.Actions = db.JobActions.Where(d => d.JobServicesId == svc.Id).Include(d => d.SrvActionItem);
                    cjoTmp.SvcItems = db.JobServiceItems.Where(d => d.JobServicesId == svc.Id).Include(d => d.InvItem);
                    cjoTmp.SupplierPos = db.SupplierPoDtls.Where(d => d.JobServicesId == svc.Id).Include(i => i.SupplierPoHdr);
                    joTmp.Main.AgreedAmt += svc.ActualAmt;

                    joTmp.Services.Add(cjoTmp);
                }

                //get min job date
                joTmp.Main.JobDate = TempJobDate(joTmp.Main.Id);

                joTmp.Payment += GetJobPaymentAmount(main.Id);

                data.Add(joTmp);
            }

            if (srch != null)
            {
                var srchData = data.Where(d => d.Main.Id.ToString() == srch ||
                                 d.Main.Description.ToLower().ToString().Contains(srch.ToLower()) ||
                                 d.Main.Customer.Name.ToLower().ToString().Contains(srch.ToLower()) ||
                                 d.Company.ToLower().ToString().Contains(srch.ToLower()));

                if (srchData != null)
                    data = srchData.ToList();
            }

            return data;
        }

        //GET : return list of ONGOING jobs listing
        public IEnumerable<cJobOrder> GetJobListing(IEnumerable<int> joblist)
        {
            IEnumerable<Models.JobMain> jobMains = db.JobMains.Where(j => joblist.Contains(j.Id))
                .Include(j => j.Customer)
                .Include(j => j.Branch)
                .Include(j => j.JobStatus)
                .Include(j => j.JobThru)
                .Include(j => j.JobEntMains)
                ;

            List<cjobCounter> jobActionCntr = GetJobActionCount(jobMains.Select(d => d.Id).ToList());
            var data = new List<cJobOrder>();

            DateTime today = dt.GetCurrentDate();

            foreach (var main in jobMains)
            {
                cJobOrder joTmp = new cJobOrder();
                joTmp.Main = main;
                joTmp.Services = new List<cJobService>();
                joTmp.Main.AgreedAmt = 0;
                joTmp.Payment = 0;

                List<Models.JobServices> joSvc = db.JobServices.Where(d => d.JobMainId == main.Id).OrderBy(s => s.DtStart).ToList();
                foreach (var svc in joSvc)
                {
                    cJobService cjoTmp = new cJobService();
                    cjoTmp.Service = svc;

                    joTmp.Main.AgreedAmt += svc.ActualAmt != null ? svc.ActualAmt : 0;
                    joTmp.Company = db.JobEntMains.Where(j => j.JobMainId == svc.JobMainId).FirstOrDefault() != null ? db.JobEntMains.Where(j => j.JobMainId == svc.JobMainId).FirstOrDefault().CustEntMain.Name : "";

                    joTmp.Expenses += GetJobExpensesBySVC(svc.Id);

                    joTmp.Services.Add(cjoTmp);
                }

                cjobIncome cIncome = new cjobIncome();
                cIncome.Car = 0;
                cIncome.Tour = 0;
                cIncome.Others = 0;

                //var latestPosted = db.JobPosts.Where(j => j.JobMainId == main.Id).OrderByDescending(s => s.Id).FirstOrDefault();

                //if (latestPosted == null)
                //{
                //    joTmp.isPosted = false;
                //}
                //else
                //{
                //    cIncome.Car = latestPosted.CarRentalInc;
                //    cIncome.Tour = latestPosted.TourInc;
                //    cIncome.Others = latestPosted.OthersInc;
                //    joTmp.isPosted = true;
                //}

                joTmp.isPosted = GetJobPostedInReceivables(joTmp.Main.Id);

                joTmp.PostedIncome = cIncome;

                joTmp.ActionCounter = jobActionCntr.Where(d => d.JobId == joTmp.Main.Id).ToList();

                joTmp.Main.JobDate = TempJobDate(joTmp.Main.Id);

                List<Models.JobPayment> jobPayment = db.JobPayments.Where(d => d.JobMainId == main.Id).ToList();
                foreach (var payment in jobPayment)
                {
                    joTmp.Payment += payment.PaymentAmt;
                }

                data.Add(joTmp);

            }
            return data.OrderBy(d => d.Main.JobDate).OrderByDescending(d => d.Main.JobDate);
        }

        //GET : return list of ONGOING job listing
        public List<cJobOrderListing> GetJobOrderListing(int sortid)
        {
            var data = new List<cJobOrderListing>();
            DateTime today = dt.GetCurrentDate();
            var confirmed = GetJobOrderListingQuery(sortid);

            foreach (var main in confirmed)
            {
                cJobOrderListing joTmp = main;
                joTmp.Amount = GetJobSvcAmount(main.Id);
                joTmp.Payment = GetJobSvcPayments(main.Id);
                joTmp.Company = GetJobCompanyName(main.Id);
                joTmp.JobDate = MinJobDate(main.DtStart, main.DtEnd);
                joTmp.JobEncodeDate = main.JobEncodeDate;

                data.Add(joTmp);
            }

            data = data.Where(s => DateTime.Compare(s.JobDate, today) >= 0 && s.Status < 4).ToList();
            return data;
        }

        //GET : return list of ONGOING job listing
        public List<cJobOrderListing> GetJobOrderListingMonthly(int month, int year)
        {
            var data = new List<cJobOrderListing>();

            var jobs = GetJobOrderMonthlyQuery(month, year);
            
            foreach (var main in jobs)
            {
                cJobOrderListing joTmp = main;
                joTmp.Id = main.Id;
                joTmp.Amount = GetJobSvcAmount(main.Id);
                joTmp.Payment = GetJobSvcPayments(main.Id);
                joTmp.ExpenseFromAP = GetAPExpensesByJobId(main.Id);
                joTmp.PaymentFromAR = GetARPaymentsByJobId(main.Id);
                joTmp.Company = GetJobCompanyName(main.Id);
                joTmp.JobDate = MinJobDate(main.DtStart, main.DtEnd);
                joTmp.StatusString = GetJobStatusById(main.Status);
                joTmp.DriversRate = GetTriplogRateByJobId(main.Id);
                joTmp.IsPosted = GetJobPostedInReceivables(main.Id);

                data.Add(joTmp);
            }

            return data;
        }

        private decimal GetJobSvcAmount(int jobId)
        {
            decimal totalAmount = 0;
            //get total amount
            var totalAmountList = db.JobServices.Where(d => d.JobMainId == jobId).ToList().Select(s => s.ActualAmt);
            foreach (var amount in totalAmountList)
            {
                totalAmount += amount != null ? (decimal)amount : decimal.Zero;
            }

            return totalAmount;
        }

        private decimal GetJobSvcPayments(int jobId)
        {
            decimal totalPayment = 0;

            //get total payment done
            List<Models.JobPayment> jobPayment = db.JobPayments.Where(d => d.JobMainId == jobId).ToList();
            foreach (var payment in jobPayment)
            {
                totalPayment += payment.PaymentAmt;
            }

            return totalPayment;
        }

        private decimal GetAPExpensesByJobId(int jobId)
        {
            try
            {
                decimal totalPayment = 0;

                string sql = "";

                sql = @"SELECT SUM(ap.Amount) FROM ApTransactions ap WHERE ap.JobRef = "+ jobId + " AND ( ap.ApTransStatusId = 4 OR ap.ApTransStatusId = 5 )";

                //terminator
                sql += ";";

                totalPayment = db.Database.SqlQuery<decimal>(sql).FirstOrDefault();

                return totalPayment;
            }
            catch
            {
                return 0;
            }
        }

        private decimal GetARPaymentsByJobId(int jobId)
        {
            try
            {
                decimal totalPayment = 0;

                string sql = "";

                sql = @"SELECT (SELECT SUM(p.Amount) FROM ArPayments p LEFT JOIN ArTransPayments atp ON atp.ArPaymentId =p.Id WHERE atp.ArTransactionId = ar.Id ) 
                        FROM ArTransactions ar WHERE  ([InvoiceRef] LIKE '" + jobId + @"') ";

                //terminator
                sql += ";";

                totalPayment = db.Database.SqlQuery<decimal>(sql).FirstOrDefault();

                return totalPayment;
            }
            catch
            {
                return 0;
            }
        }


        private decimal GetTriplogRateByJobId(int jobId)
        {
            try
            {
                decimal totalRate = 0;

                string sql = "";

                sql = @"SELECT  SUM(cr.DriverFee + cr.DriverOT) FROM crLogTrips cr LEFT JOIN crLogTripJobMains jm ON jm.crLogTripId = cr.Id WHERE jm.JobMainId LIKE " + jobId + " ";

                //terminator
                sql += ";";

                totalRate = db.Database.SqlQuery<decimal>(sql).FirstOrDefault();

                return totalRate;
            }
            catch
            {
                return 0;
            }
        }


        public string GetJobStatusById(int id)
        {
            try
            {
                return db.JobStatus.Find(id).Status;
            }
            catch
            {
                return "";
            }
        }


        public string GetJobStatusByJobId(int id)
        {
            try
            {
                return db.JobMains.Find(id).JobStatus.Status;
            }
            catch
            {
                return "";
            }
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

        //GET : the lastest date of the job based on the date today
        public DateTime MinJobDate(int mainId)
        {
            //update jobdate
            var main = db.JobMains.Where(j => mainId == j.Id).FirstOrDefault();

            DateTime minDate = main.JobDate;
            DateTime maxDate = new DateTime(1, 1, 1);

            DateTime today = new DateTime();
            today = dt.GetCurrentDate();

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

            //return main.JobDate;
            return minDate;
        }

        //GET jobcount 
        public List<cjobCounter> GetJobActionCount(List<Int32> jobidlist)
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


        //GET jobcount 
        public bool GetJobPostedInReceivables(int jobId)
        {
            #region sqlstr
            string sqlstr = "SELECT Id FROM ArTransactions ar WHERE ar.InvoiceId = "+ jobId +";";
            #endregion
            int count = db.Database.SqlQuery<int>(sqlstr).Count();

            if (count > 0)
            {
                return true;
            }
            return false;
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
        public List<cJobOrderListing> GetJobOrderListingQuery(int sortid)
        {
            List<cJobOrderListing> joblist = new List<cJobOrderListing>();

            string sql = "";

            //filter jobs based on statusId and date
            switch (sortid)
            {
                case 1: //OnGoing
                    sql = @"SELECT Id = MIN(job.Id), DtStart = MIN(job.DtStart), DtEnd = MIN(job.DtEnd), JobEncodeDate = MIN(job.jobDate),
	                                Description = MIN(job.Description), Customer = MIN(job.Customer), Status = MIN(job.JobStatusId)  
	                                FROM ( SELECT jm.Id,  jm.Description, jm.JobStatusId, js.DtStart, js.DtEnd,  jm.jobDate,
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
        public List<cJobOrderListing> GetJobOrderMonthlyQuery(int month, int year)
        {
            List<cJobOrderListing> joblist = new List<cJobOrderListing>();

            string sql = "";

            sql = @"SELECT Id = MIN(job.Id), DtStart = MIN(job.DtStart), DtEnd = MAX(job.DtEnd), 
	                    Description = MIN(job.Description), Customer = MIN(job.Customer), Status = MIN(job.JobStatusId)  
	                    FROM ( SELECT jm.Id,  jm.Description, jm.JobStatusId, js.DtStart, js.DtEnd,
		                Customer = (SELECT c.Name FROM Customers c WHERE c.Id = jm.CustomerId)
		                FROM JobMains jm LEFT JOIN JobServices js ON jm.Id = js.JobMainId ) job

		                WHERE month(job.DtStart) = " + month + @" AND year(job.DtStart) = "+ year +@"
		                GROUP BY job.Id ORDER BY DtStart";

            //terminator
            sql += ";";

            joblist = db.Database.SqlQuery<cJobOrderListing>(sql).ToList();

            return joblist.Where(c => c.Status > 1 && c.Status < 5).ToList();

        }

        public List<JobsV1.Models.ReportingViewModels.JobTripLogDetails> GetJobTripLogDetails()
        {
            List<ReportingViewModels.JobTripLogDetails> triplogs = new List<ReportingViewModels.JobTripLogDetails>();

            string sql = "";

            sql = @"";

            //terminator
            sql += ";";

            triplogs = db.Database.SqlQuery<ReportingViewModels.JobTripLogDetails>(sql).ToList();

            return triplogs;
        }

        //Get Job Order Listing based on the sort
        public List<cJobConfirmed> GetJobSearchQuery(string srch)
        {
            List<cJobConfirmed> joblist = new List<cJobConfirmed>();
            string sql = "";
            int srchId = 0;

            if (Int32.TryParse(srch.TrimStart('0'), out srchId))
            {
                sql = @"select j.Id from JobMains j WHERE j.id = " + srchId + " ";
            }
            else
            {
                sql =  " select j.Id from JobMains j "+
                       " LEFT JOIN Customers c ON j.CustomerId = c.Id "+
                       " LEFT JOIN JobEntMains jem ON j.Id = jem.JobMainId "+
                       " LEFT JOIN CustEntMains cem ON jem.CustEntMainId = cem.Id "+
                       " WHERE cem.Name LIKE '%"+ srch + "%' OR c.Name LIKE '%"+ srch + "%' OR j.Description LIKE '%"+ srch + "%' ";
            }

            //terminator
            sql += ";";

            joblist = db.Database.SqlQuery<cJobConfirmed>(sql).ToList();

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

        //GET : the lastest date of the job based on the date today
        public DateTime GetMinMaxServiceDate(int mainId, string getType)
        {
            var count = 1;

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

                if (count == 1)
                {
                    //set 1st service as start date
                    minDate = svcDtStart;
                    maxDate = svcDtEnd;
                }

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

                count++;
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
            try
            {
                //get job discount payments of job
                var jobPaymentDiscounts = db.JobPayments.Where(p => p.JobMainId == jobId && p.JobPaymentTypeId == 4).ToList();
                if (jobPaymentDiscounts.Count != 0)
                {
                    decimal totalDiscount = 0;

                    //get total discount
                    foreach (var payment in jobPaymentDiscounts)
                    {
                        totalDiscount += payment.PaymentAmt;
                    }

                    return totalDiscount;
                }
                return 0;
            }
            catch { 
                return 0; 
            }
        }

        public decimal GetJobPaymentAmount(int id)
        {
            try
            {
                var paymentList = db.JobPayments.Where(j => j.JobMainId == id && j.JobPaymentTypeId < 4).ToList();
                decimal totalPayment = 0;

                foreach (var payment in paymentList)
                {
                    totalPayment += payment.PaymentAmt;
                }


                //subtract discounted amount
                //note: discount amount is negative number
                totalPayment = totalPayment + GetJobDiscountAmount(id);

                return totalPayment;
            }
            catch
            {
                return 0;
            }
           
        }

        public decimal GetTotalJobAmount(int id)
        {
            try
            {
                var services = db.JobServices.Where(j => j.JobMainId == id).ToList();
                decimal totalAmount = 0;

                foreach (var svc in services)
                {
                    totalAmount += svc.ActualAmt ?? 0;
                }

                //subtract discounted amount
                //note: discount amount is negative number
                //totalAmount = totalAmount + GetJobDiscountAmount(id);

                return totalAmount;
            }
            catch { return 0; }
        }

        public JobPaymentStatus GetJobPaymentStatus(int id)
        {
            return db.JobPaymentStatus.Find(GetLastJobPaymentStatusId(id));

        }


        public int GetLastJobPaymentStatusId(int jobId)
        {
            var tempStatus = db.JobMainPaymentStatus.Where(j => j.JobMainId == jobId);

            if (tempStatus.FirstOrDefault() != null)
            {
                return tempStatus.OrderByDescending(j => j.Id).FirstOrDefault().JobPaymentStatusId;
            }

            //unpaid if no records
            return 2;
        }

        public string GetJobCompany(int jobId)
        {
            var company = db.JobEntMains.Where(j => j.JobMainId == jobId);

            if (company.FirstOrDefault() != null)
            {
                return company.OrderByDescending(j => j.Id).FirstOrDefault().CustEntMain.Name;
            }

            //if no records
            return "Personal Account";
        }

        public string GetJobVehicle(int jobId)
        {
            var jobvehicleQuery = db.JobVehicles.Where(j => j.JobMainId == jobId);
            if (jobvehicleQuery.FirstOrDefault() != null) {
                var vehicleString = "";
                var vehicle = jobvehicleQuery.OrderByDescending(c => c.Id).FirstOrDefault().Vehicle;

                vehicleString += vehicle.VehicleModel.VehicleBrand.Brand + " ";
                vehicleString += vehicle.VehicleModel.Make + " ";
                vehicleString += vehicle.VehicleModel.Variant;
                vehicleString += " "+ vehicle.YearModel;
                vehicleString += " ( " + vehicle.PlateNo + " ) ";

                return vehicleString;
            }

            //if no records
            return "NA";
        }

        public string GetCompanyAccountType(int id)
        {
            try
            {
                var company = db.JobEntMains.Where(c => c.JobMainId == id).OrderByDescending(c => c.Id).FirstOrDefault().CustEntMain;

                return company.CustEntAccountType.Name;
            }
            catch
            {
                return " ";
            }
        }

        public string GetJobReferralAgent(int id)
        {
            try
            {
                var  services = db.JobServices.Where(s => s.JobMainId == id).ToList();
                var referralAgent = "";

                foreach (var svc in services)
                {
                    var svcItems = svc.JobServiceItems.ToList();
                    foreach (var item in svcItems)
                    {
                        var itemCats = item.InvItem.InvItemCategories.ToList();

                        foreach(var category in itemCats)
                        {
                            //Service Advisor = 4
                            if (category.InvItemCatId == 4)
                            {
                                referralAgent += item.InvItem.Description + " ";
                            }
                        }
                    }
                }
                return referralAgent;
            }
            catch
            {
                return " ";
            }
        }

        public decimal GetJobExpensesBySVC(int svcId)
        {
            decimal total = 0;
            var expense = db.JobExpenses.Where(s => s.JobServicesId == svcId).ToList();
            foreach (var items in expense as IEnumerable<JobExpenses>)
            {
                total += items.Amount;
            }
            return total;
        }

        //GET : return job company name
        private string GetJobCompanyName(int jobId)
        {
            var jobmain = db.JobMains.Find(jobId);

            var jobEntMainQuery = jobmain.JobEntMains.OrderByDescending(j => j.Id).FirstOrDefault();
            if (jobEntMainQuery != null)
            {
                return jobEntMainQuery.CustEntMain.Name;
            }

            return "N/A";
        }

        public string GetJobStatus(int id)
        {
            return db.JobStatus.Find(id).Status;
        }

        private decimal GetTotalRate(IEnumerable<cJobOrder> filteredJob)
        {
            decimal totalRate = 0;
            foreach (var job in filteredJob as IEnumerable<cJobOrder>)
            {
                totalRate += job.Main.AgreedAmt != null ? (decimal)job.Main.AgreedAmt : 0;
            }

            return totalRate;
        }

        private decimal GetTotalPayment(IEnumerable<cJobOrder> filteredJob)
        {
            decimal totalPayment = 0;
            foreach (var job in filteredJob as IEnumerable<cJobOrder>)
            {
                totalPayment += job.Payment;
            }

            return totalPayment;
        }

        private decimal GetTotalExpenses(IEnumerable<cJobOrder> filteredJob)
        {
            decimal totalExpense = 0;
            List<int> SvcID = new List<int>();

            foreach (var job in filteredJob as IEnumerable<cJobOrder>)
            {
                if (job.Services.Contains(null))
                {
                    foreach (var svc in job as IEnumerable<cJobService>)
                    {
                        SvcID.Add(svc.Id);
                    }
                }
            }

            var expensesList = db.JobExpenses.Where(s => SvcID.Contains(s.JobServicesId)).ToList();

            foreach (var expense in expensesList as IEnumerable<JobExpenses>)
            {
                totalExpense += expense.Amount;
            }

            return totalExpense;
        }

        private decimal GetJobExpense(cJobOrder cjob)
        {
            List<int> SvcID = new List<int>();
            var svc = db.JobServices.Where(s => s.JobMainId == cjob.Main.Id).ToList();
            decimal totalExpenses = 0;

            //get service id
            foreach (var svcItems in svc as IEnumerable<JobServices>)
            {
                SvcID.Add(svcItems.Id);
            }

            var expensesList = db.JobExpenses.Where(s => SvcID.Contains(s.JobServicesId)).ToList();

            foreach (var exp in expensesList as IEnumerable<JobExpenses>)
            {
                totalExpenses += exp.Amount;
            }

            return totalExpenses;
        }

        public List<crLogTrip> GetTriplogsByJobId(int id)
        {
            var triplogs = crdb.crLogTrips.Where(c => c.crLogTripJobMains.Select(t => t.JobMainId).Contains(id)).ToList();

            if (triplogs == null)
            {
                return new List<crLogTrip>();
            }

            return triplogs;
        }

        public List<ApTransaction> GetExpensesByJobId(int id)
        {
            var expenses = apdb.ApTransactions.Where(c => c.JobRef == id).ToList();

            if (expenses == null)
            {
                return new List<ApTransaction>();
            }

            return expenses;

        }

        public List<ArTransaction> GetReceivablesByJobId(int id)
        {
            var receivables = ardb.ArTransactions
                .Where(t => t.InvoiceId == id && t.ArTransStatusId >= 3 && t.ArTransStatusId <= 6)
                .ToList();

            if (receivables == null)
            {
                return new List<ArTransaction>();
            }

            return receivables;
        }
    }
}