using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JobsV1.Models
{
    public class PayPalHandler
    {

        JobDBContainer db = new JobDBContainer();

        public void AddPaypalNotif(string transId, int jobid, DateTime dateposted, DateTime dateTrx, string eventStatus,decimal amount)
        {

            db.PaypalTransactions.Add(new PaypalTransaction
            {
                TrxId = transId,
                DatePosted = dateposted,
                TrxDate = dateTrx,
                JobId = jobid,
                Remarks = "n/a",
                Status = eventStatus,
                Amount = amount
            });
            db.SaveChanges();

        }
        
    }
}