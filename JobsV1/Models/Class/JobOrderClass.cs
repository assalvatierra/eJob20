using JobsV1.Controllers;
using JobsV1.Models.Class;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace JobsV1.Models
{


    public class JobOrderClass
    {
        // Job Status
        private int JOBINQUIRY = 1;
        private int JOBRESERVATION = 2;
        private int JOBCONFIRMED = 3;
        private int JOBCLOSED = 4;
        private int JOBCANCELLED = 5;
        private int JOBTEMPLATE = 6;

        private JobDBContainer db = new JobDBContainer();
        private DBClasses dbc = new DBClasses();
        private ActionTrailClass trail = new ActionTrailClass();
        private DateClass dt = new DateClass();

        //GET : return list of confirmed jobs
        public List<cJobOrder> getJobData(int sortid)
        {
            var confirmed = dbc.getJobConfirmedList(sortid).Select(s => s.Id);

            IEnumerable<Models.JobMain> jobMains = db.JobMains.Where(j => confirmed.Contains(j.Id))
                .Include(j => j.Customer)
                .Include(j => j.Branch)
                .Include(j => j.JobStatus)
                .Include(j => j.JobThru)
                ;

            List<cjobCounter> jobActionCntr = getJobActionCount(jobMains.Select(d => d.Id).ToList());
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

                List<Models.JobServices> joSvc = db.JobServices.Where(d => d.JobMainId == main.Id).OrderBy(s => s.DtStart).ToList();
                foreach (var svc in joSvc)
                {
                    cJobService cjoTmp = new cJobService();
                    cjoTmp.Service = svc;

                    var ActionDone = db.JobActions.Where(d => d.JobServicesId == svc.Id).Select(s => s.SrvActionItemId);

                    cjoTmp.SvcActions = db.SrvActionItems.Where(d => d.ServicesId == svc.ServicesId && !ActionDone.Contains(d.Id)).Include(d => d.SrvActionCode);
                    cjoTmp.SvcActionsDone = db.SrvActionItems.Where(d => d.ServicesId == svc.ServicesId && ActionDone.Contains(d.Id)).Include(d => d.SrvActionCode);
                    cjoTmp.Actions = db.JobActions.Where(d => d.JobServicesId == svc.Id).Include(d => d.SrvActionItem);
                    cjoTmp.SvcItems = db.JobServiceItems.Where(d => d.JobServicesId == svc.Id).Include(d => d.InvItem);
                    cjoTmp.SupplierPos = db.SupplierPoDtls.Where(d => d.JobServicesId == svc.Id).Include(i => i.SupplierPoHdr);
                    joTmp.Main.AgreedAmt += svc.ActualAmt;

                    joTmp.Services.Add(cjoTmp);
                }

                joTmp.ActionCounter = jobActionCntr.Where(d => d.JobId == joTmp.Main.Id).ToList();

                //get min job date
                if (sortid == 1)
                {
                    joTmp.Main.JobDate = TempJobDate(joTmp.Main.Id);
                    //joTmp.Main.JobDate = MinJobDate(joTmp.Main.Id);
                }
                if (sortid == 2)
                {
                    //joTmp.Main.JobDate = TempJobDate(joTmp.Main.Id);
                    //joTmp.Main.JobDate = MinJobDate(joTmp.Main.Id);
                }
                else
                {
                    joTmp.Main.JobDate = MinJobDate(joTmp.Main.Id);
                }

                List<Models.JobPayment> jobPayment = db.JobPayments.Where(d => d.JobMainId == main.Id).ToList();
                foreach (var payment in jobPayment)
                {
                    joTmp.Payment += payment.PaymentAmt;
                }

                data.Add(joTmp);
            }

            switch (sortid)
            {
                case 1: //OnGoing
                    data = (List<cJobOrder>)data
                        .Where(d => (d.Main.JobStatusId == JOBINQUIRY || d.Main.JobStatusId == JOBRESERVATION || d.Main.JobStatusId == JOBCONFIRMED)).ToList()
                       .Where(d => d.Main.JobDate.CompareTo(today.Date) >= 0).ToList();

                    break;
                case 2: //prev
                    data = (List<cJobOrder>)data
                        .Where(d => (d.Main.JobStatusId == JOBINQUIRY || d.Main.JobStatusId == JOBRESERVATION || d.Main.JobStatusId == JOBCONFIRMED)).ToList()
                        .Where(p => DateTime.Compare(p.Main.JobDate.Date, today.Date) < 0)

                        .ToList();

                    break;
                case 3: //close
                    data = (List<cJobOrder>)data
                        .Where(d => (d.Main.JobStatusId == JOBCLOSED || d.Main.JobStatusId == JOBCANCELLED)).ToList()
                        .Where(p => p.Main.JobDate.Date > today.Date.AddDays(-150)).ToList();
                    break;

                default:
                    data = (List<cJobOrder>)data.ToList();
                    break;
            }

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

        //GET the lastest date of the job based on the date today
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

    }
}