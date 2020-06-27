using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.UI.WebControls;

namespace JobsV1.Models.Class
{
    public class EmailBuilderClass
    {
        /**
         * SendEmail for Notifications
         * param:
         *      recipient (string)
         *      
         */ 
        public bool SendEmail_NotifTest(string recipient)
        {
            var emailContent = " <b> Email Notification Content </b>  ";

            //build email content
            var mailContent = BuildEmailContent(recipient, emailContent);
            if (mailContent != null)
                return Send(mailContent);
            else
                return false;
        }

        private MailMessage BuildEmailContent(string recipient, string htmlContent)
        {
            try
            {
                //create email
                MailDefinition md = new MailDefinition();
                md.From = "Realwheels.Reservation@RealWheelsDavao.com";      //sender mail
                md.IsBodyHtml = true;                 //set true to enable use of html tags 
                md.Subject = "Reservation Details";   //mail title

                ListDictionary replacements = new ListDictionary();
                replacements.Add("{name}", "Reservation");

                string body;
                string recipient_Email = "jahdielsvillosa@gmail.com";

                body =
                    "" +
                    " <div style='background-color:#f4f4f4;padding:20px;' align='center'>" +
                    " <div style='background-color:white;min-width:200px;width:370px;margin:30px;padding:30px;text-align:center;color:#555555;font:normal 300 16px/21px 'Helvetica Neue',Arial'> " +
                      htmlContent +
                    " <p> This is an auto-generated email.<br /> DO NOT REPLY TO THIS MESSAGE </p> " +
                    " <p> For further inquiries kindly email us through inquiries.realwheels@gmail.com or dial(+63) 082 333 5157. </p> " +
                    " </div></div>" +
                    "";

                MailMessage msg = md.CreateMailMessage(recipient_Email, replacements, body, new System.Web.UI.Control());

                return msg;
            }
            catch (Exception ex)
            {
                throw ex;
                return null;
            }
        }


        private bool Send(MailMessage mailMessage)
        {
            try
            {

                // configure mail server
                SmtpClient SmtpServer = new SmtpClient("mail.realwheelsdavao.com"); //smtp server
                SmtpServer.Port = 587;          //default smtp port
                SmtpServer.Credentials = new System.Net.NetworkCredential(
                    System.Web.Configuration.WebConfigurationManager.AppSettings["SmtpEmail"],
                    System.Web.Configuration.WebConfigurationManager.AppSettings["SmtpPass"]);
                SmtpServer.EnableSsl = false;   //enable for gmail smtp server
                System.Net.ServicePointManager.Expect100Continue = false;

                SmtpServer.Send(mailMessage);           //send message

                return true;

            }
            catch
            {
                return false;
            }
        }



    }
}