using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace JobsV1.Models.Class
{
    public class cNotifRecipient
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public int CustomerId { get; set; }
    }

    public class cNotifActivity
    {
        public int Id { get; set; }
        public DateTime DtActivity { get; set; }
        public string Title { get; set; }
        public string Recipient { get; set; }
        public string Status { get; set; }
    }

    public class cPendingNotif
    {
        public int Id { get; set; }
        public DateTime DtActivity { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public bool IsEmail { get; set; }
        public bool IsSMS { get; set; }
        public string Recipient { get; set; }
        public string Status { get; set; }
    }


    public class CustNotifClass
    {
        private JobDBContainer db = new JobDBContainer();
        private DateClass dt = new DateClass();
        private EmailBlaster emailSender = new EmailBlaster();

        public List<cNotifRecipient> GetRecipientList()
        {
            List<cNotifRecipient> recipients = new List<cNotifRecipient>();

            //sql query with comma separated item list
            string sql =
               @"                                  
                SELECT *,
	                Name = (SELECT Name FROM Customers cu WHERE cu.Id = cr.CustomerId),
	                Email = (SELECT Email FROM NotifRecipients nr WHERE nr.Id = cr.NotifRecipientId),
	                Mobile = (SELECT Mobile FROM NotifRecipients nr WHERE nr.Id = cr.NotifRecipientId)
                FROM CustNotifRecipients cr
                WHERE CustNotifId = 1		;";

            recipients = db.Database.SqlQuery<cNotifRecipient>(sql).ToList();

            return recipients;
        }

        public List<cNotifActivity> GetActivityList()
        {
            List<cNotifActivity> recipients = new List<cNotifActivity>();

            //sql query with comma separated item list
            string sql =
               @"                                  
                SELECT cna.Id,
	            DtActivity = cna.DtActivity,
	            Title = cn.MsgTitle,
	            Status = cna.Status,
	            Recipient = (SELECT Email FROM NotifRecipients nr WHERE nr.Id = cnr.NotifRecipientId)
                FROM CustNotifActivities cna 	
                LEFT JOIN CustNotifRecipients cnr ON cnr.Id = cna.CustNotifRecipientId
                LEFT JOIN CustNotifs cn ON cn.Id = cnr.CustNotifId;";

            recipients = db.Database.SqlQuery<cNotifActivity>(sql).ToList();

            return recipients;
        }


        public List<cPendingNotif> GetPendingNotif()
        {
            var now = dt.GetCurrentDateTime();
            List<cPendingNotif> pending = new List<cPendingNotif>();

            //sql query with comma separated item list
            string sql =
               @"                                  
               SELECT cna.Id,
	                   DtActivity = cna.DtActivity,
	                   Title = cn.MsgTitle,
	                   Content = cn.MsgBody,
	                   IsEmail = cn.IsEmail,
	                   IsSMS = cn.IsSMS,
	                   Status = cna.Status,
	                   Recipient = (SELECT Email FROM NotifRecipients nr WHERE nr.Id = cnr.NotifRecipientId)
                FROM CustNotifActivities cna 	
                LEFT JOIN CustNotifRecipients cnr ON cnr.Id = cna.CustNotifRecipientId
                LEFT JOIN CustNotifs cn ON cn.Id = cnr.CustNotifId	
                WHERE cna.DtActivity <= convert(datetime, GETDATE()) AND cna.Status = 'PENDING';";

            pending = db.Database.SqlQuery<cPendingNotif>(sql).ToList();

            return pending;
        }

        //id = notification Activity Id
        public bool SendEmailNotif(int? id)
        {
            if (id != null)
            {
                //get notification
                var notifAct = db.CustNotifActivities.Find(id);

                var notification = notifAct.CustNotifRecipient.CustNotif;
                var recipient = notifAct.CustNotifRecipient.NotifRecipient;

                //get recipient list
                var email = recipient.Email;
                var subject = notification.MsgTitle;
                var content = notification.MsgBody;

                var emailResult = emailSender.SendMail(email, subject, content);

                if (emailResult == "success")
                {
                    UpdateActivity((int)id);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }

        public void UpdateActivity(int id)
        {
            var custNotif = db.CustNotifActivities.Find(id);
            custNotif.Status = "DONE";
            db.Entry(custNotif).State = EntityState.Modified;
            db.SaveChanges();
        }

    }
}