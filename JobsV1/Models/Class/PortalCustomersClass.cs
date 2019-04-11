using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JobsV1.Models;
using JobsV1.Controllers;
using System.Data.Entity;

namespace JobsV1.Models.Class
{
    public class PortalCustomersClass
    {

        // Job Status
        private int JOBINQUIRY = 1;
        private int JOBRESERVATION = 2;
        private int JOBCONFIRMED = 3;
        private int JOBCLOSED = 4;
        private int JOBCANCELLED = 5;
        private int JOBTEMPLATE = 6;

        private JobDBContainer db = new JobDBContainer();
        public int checklogin(string contactNumber, string password )
        {
            if(!(string.IsNullOrEmpty(contactNumber) && string.IsNullOrEmpty(password)))
            {
                var custTemp = db.PortalCustomers.Where(c=>c.ContactNum == contactNumber && c.Password == password) != null ?
                    db.PortalCustomers.Where(c => c.ContactNum == contactNumber && c.Password == password).FirstOrDefault() : null;
                if (custTemp != null)
                {
                    //check if login expiry date
                    if (GetCurrentTime().CompareTo(custTemp.ExpiryDt) < 0)
                    {
                        return 0; // login ok
                    }
                    else
                    {
                        return 1; // login expired
                    }
                }
            }
            return 2; //login invalid
        }

        public PortalCustomer getCustomer(string contactNumber, string password)
        {
            if (!(string.IsNullOrEmpty(contactNumber) && string.IsNullOrEmpty(password)))
            {
                var custTemp = db.PortalCustomers.Where(c => c.ContactNum == contactNumber && c.Password == password).FirstOrDefault();
                if (custTemp != null)
                {
                    //check sessions if empty
                    //if true assign
                    return custTemp;
                }

            }

            return null;
        }

        public List<cJobOrder> getJobList(int sortid, int custId)
        {

            //if (sortid != null)
            //    Session["FilterID"] = (int)sortid;
            //else
            //{
            //    if (Session["FilterID"] != null)
            //        sortid = (int)Session["FilterID"];
            //    else
            //        sortid = 1;
            //}

            //if (Session["FilterID"] == null)
            //{
            //    Session["FilterID"] = 1;
            //}


            IEnumerable<Models.JobMain> jobMains = db.JobMains.Where(j=>j.CustomerId == custId)
            .Include(j => j.Customer)
            .Include(j => j.Branch)
            .Include(j => j.JobStatus)
            .Include(j => j.JobThru)
            ;

            List<cjobCounter> jobActionCntr = getJobActionCount(jobMains.Select(d => d.Id).ToList());
            var data = new List<cJobOrder>();

            DateTime today = new DateTime();
            today = GetCurrentTime().Date;

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
                    cjoTmp.Actions = db.JobActions.Where(d => d.JobServicesId == svc.Id).Include(d => d.SrvActionItem);
                    cjoTmp.SvcItems = db.JobServiceItems.Where(d => d.JobServicesId == svc.Id).Include(d => d.InvItem);
                    cjoTmp.SupplierPos = db.SupplierPoDtls.Where(d => d.JobServicesId == svc.Id).Include(i => i.SupplierPoHdr);
                    joTmp.Main.AgreedAmt += svc.ActualAmt;

                    joTmp.Services.Add(cjoTmp);
                }

                joTmp.ActionCounter = jobActionCntr.Where(d => d.JobId == joTmp.Main.Id).ToList();
                
                data.Add(joTmp);

                List<Models.JobPayment> jobPayment = db.JobPayments.Where(d => d.JobMainId == main.Id).ToList();
                foreach (var payment in jobPayment)
                {
                    joTmp.Payment += payment.PaymentAmt;
                }
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
                        .Where(p => p.Main.JobDate.Date > today.Date.AddDays(-60)).ToList();
                    break;

                default:
                    data = (List<cJobOrder>)data.ToList();
                    break;
            }
            
            return data.OrderByDescending(d => d.Main.JobDate).ToList();
            
        }

        
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
        
        //generate password 
        //CustomerID + Datetoday
        public string generatePass(int custId)
        {
            //get day and month in integer
            int dayTemp   = GetCurrentTime().Day;
            int monthTemp = GetCurrentTime().Month;

            //build pass
            string passTemp = custId.ToString() + dayTemp.ToString() + monthTemp.ToString();
            return passTemp;
        }

        //get current time based on Singapore Standard Time 
        //SGT - UTC +8
        public DateTime GetCurrentTime()
        {
            DateTime _localTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Singapore Standard Time"));
            _localTime = _localTime.Date;

            return _localTime;
        }
    }
}