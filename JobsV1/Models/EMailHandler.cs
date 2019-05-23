using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Web.UI.WebControls;
using System.Collections.Specialized;

namespace JobsV1.Models
{
    public class EMailHandler
    {
        private JobDBContainer db = new JobDBContainer();

        public string SendMail(int jobId, string renterMail, string mailType, string renterName, string site)
        {
            try
            {
                // configure mail server
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("mail.realwheelsdavao.com"); //smtp server

                //create email
                MailDefinition md = new MailDefinition();
                // md.From = "admin@realwheelsdavao.com";      //sender mail
                md.From = "Realwheels.Reservation@RealWheelsDavao.com";      //sender mail
                md.IsBodyHtml = true;                       //set true to enable use of html tags 
                md.Subject = "RealWheels Reservation";      //mail title

                ListDictionary replacements = new ListDictionary();
                replacements.Add("{name}", "Reservation");

                string body, message;
                string siteName = site;

                //get job details
                JobMain job = db.JobMains.Find(jobId);
                //encode white space
                string jobDesc = System.Web.HttpUtility.UrlPathEncode(job.Description);

                md.Subject = renterName + ": NEW RealWheels Reservation";   //mail title

                switch (mailType)
                {
                    case "ADMIN":
                        //mail title
                        md.Subject = renterName + ": Reservation ";

                        //find reservation
                        CarReservation reserve = db.CarReservations.Find(jobId);

                        //email content
                        message = "A NEW Reservation Inquiry has been made. Please follow the link for the reservation details. <a href='" + siteName + "" + jobId + "/" + reserve.DtTrx.Month + "/" + reserve.DtTrx.Day + "/" + reserve.DtTrx.Year + "/" + reserve.RenterName + "/' " +
                        "> View Reservation Details  </a> ";
                        break;
                    case "PAYMENT-SUCCESS":
                        //mail content for successful payment
                        //mail title
                        md.Subject = renterName + ": Reservation ";

                        //find reservation
                         reserve = db.CarReservations.Find(jobId);

                        //email content
                        message = "Paypal Payment is successful. Please follow the link for the invoice details. <a href='" + siteName + "" + jobId + "/" + reserve.DtTrx.Month + "/" + reserve.DtTrx.Day + "/" + reserve.DtTrx.Year + "/" + reserve.RenterName + "/' " +
                        "> View Invoice Details  </a> ";
                        break;
                    case "PAYMENT-DENIED":
                        //mail content for denied payment
                        //mail title
                        md.Subject = renterName + ": Reservation ";

                        //find reservation 
                        reserve = db.CarReservations.Find(jobId);

                        //email content
                        message = "Paypal Payment have been DENIED. Please follow the link for the invoice details. <a href='" + siteName + "" + jobId + "/" + reserve.DtTrx.Month + "/" + reserve.DtTrx.Day + "/" + reserve.DtTrx.Year + "/" + reserve.RenterName + "/' " +
                        "> View Invoice Details  </a> ";
                        break;
                    case "PAYMENT-PENDING":
                        //mail content for pending payment
                        //mail title
                        md.Subject = renterName + ": Reservation ";

                        //find reservation
                        reserve = db.CarReservations.Find(jobId);

                        //email content
                        message = "Paypal Payment has been sent. Please follow the link for the invoice details. <a href='" + siteName + "" + jobId + "/" + reserve.DtTrx.Month + "/" + reserve.DtTrx.Day + "/" + reserve.DtTrx.Year + "/" + reserve.RenterName + "/' " +
                        "> View Invoice  </a> ";
                        break;
                    case "CLIENT-PENDING":
                        //mail title
                        md.Subject = "Realwheels Reservation";

                        reserve = db.CarReservations.Find(jobId);

                        //mail content for pending payment
                        message = "We are happy to recieved your inquiry. We will contact you after we have processed your reservation. Please click the link below for your reservation details, Thank you. <a href='" + siteName + "" + jobId + "/" + reserve.DtTrx.Month + "/" + reserve.DtTrx.Day + "/" + reserve.DtTrx.Year + "/" + reserve.RenterName + "/' " +
                        "> View Booking Details  </a> ";
                        break;
                    case "CLIENT-INQUIRY":
                        //Client Inquiry
                        //mail title
                        md.Subject = "Realwheels Reservation";

                        //find reservation
                        reserve = db.CarReservations.Find(jobId);

                        //mail content for pending payment
                        message = "We are happy to recieved your inquiry. We will contact you after we have"+
                            " processed your reservation. Please click the link below for your reservation details,"+
                            " Thank you.<br> <a href='" + siteName + "" + jobId + "/" + reserve.DtTrx.Month + "/" + 
                            reserve.DtTrx.Day + "/" + reserve.DtTrx.Year + "/" + reserve.RenterName + "/' " +
                            "> View Booking Details  </a> ";
                        break;
                    case "CLIENT-INVOICE-SEND":
                        //mail title
                        md.Subject = "Realwheels Invoice";

                        //find reservation
                        reserve = db.CarReservations.Find(jobId);

                        //mail content for pending payment
                        message = "Good day, please follow the link for the invoice and payment of your reservation."+
                            "<br> <a href='" + siteName + "" + jobId + "/" + job.JobDate.Month + "/" + job.JobDate.Day + 
                            "/" + job.JobDate.Year + "/" + jobDesc + "/' " +
                            "> View Invoice </a> ";
                        break;
                    case "CLIENT-PAYMENT-SUCCESS":
                        //mail title
                        md.Subject = "Realwheels Payment";

                        //find reservation
                        reserve = db.CarReservations.Find(jobId);

                        //mail content for pending payment
                        message = "Thank you for your payment. Please follow the link for the invoice and payment."+
                            "<br> <a href='" + siteName + "" + jobId + "/" + job.JobDate.Month + "/" + job.JobDate.Day + 
                            "/" + job.JobDate.Year + "/" + jobDesc + "/' " +
                            "> View Invoice </a> ";
                        break;
                    case "ADMIN-INVOICE-SENT":
                        
                        //mail title
                        md.Subject = job.Description + " Invoice Sent";
                        
                        //mail content for client inquiries
                        message = " An invoice link has been sent to " + job.Description + ". Please follow the link"+
                            " for the invoice and payment.<br> <a href='" + siteName + "" + jobId + "/" + 
                            job.JobDate.Month + "/" + job.JobDate.Day + "/" + job.JobDate.Year + "/" + jobDesc + "/' " +
                            "> View Invoice </a> ";
                        break;
                    case "ADMIN-PAYMENT-SUCCESS":

                        //mail title
                        md.Subject = job.Description + " Payment Success";

                        //mail content for client inquiries
                        message = "A New Payment has been made. Please follow the link for the invoice and payment.<br>"+
                            " <a href='" + siteName + "" + jobId + "/" + job.JobDate.Month + "/" + job.JobDate.Day + "/" +
                            job.JobDate.Year + "/" + jobDesc + "/' " +
                            "> View Invoice </a> ";
                        break;
                    default:
                        //new reservation

                        //send email in /joborder 
                        md.Subject = "Realwheels Reservation";

                        //mail content for client inquiries
                        message = " Your inquiry have been processed to confirm your reservation, please follow the link"+
                            "for the invoice and payment. <a href='" + siteName + "" + jobId + "/" + job.JobDate.Month + 
                            "/" + job.JobDate.Day + "/" + job.JobDate.Year + "/" + jobDesc + "/' " +
                            "> View Booking Details  </a> ";
                        //" style='display:block;background-color:dodgerblue;margin:20px;padding:20px;text-decoration:none;font-weight:bolder;font-size:300;color:white;border-radius:3px;min-width:100px;'> View Booking Details </a> ";

                        break;
                }
                
                body =
                    "" +
                    " <div style='background-color:#f4f4f4;padding:20px' align='center'>" +
                    " <div style='background-color:white;min-width:200px;margin:30px;padding:30px;text-align:center;color:#555555;font:normal 300 16px/21px 'Helvetica Neue',Arial'>  <h1> RealWheels Car Reservation </h1>" +
                    message +
                    " <p> This is an auto-generated email. DO NOT REPLY TO THIS MESSAGE </p> " +
                    " <p> For further inquiries kindly email us through inquiries.realwheels@gmail.com or dial(+63) 82 297 1831. </p> " +
                    " </div></div>" +
                    "";

                MailMessage msg = md.CreateMailMessage(renterMail, replacements, body, new System.Web.UI.Control());

                SmtpServer.Port = 587;          //default smtp port
                SmtpServer.Credentials = new System.Net.NetworkCredential(
                    System.Web.Configuration.WebConfigurationManager.AppSettings["SmtpEmail"],
                    System.Web.Configuration.WebConfigurationManager.AppSettings["SmtpPass"]);
                SmtpServer.EnableSsl = false;   //enable for gmail smtp server
                System.Net.ServicePointManager.Expect100Continue = false;
                SmtpServer.Send(msg);           //send message
                return "success";
            }
            catch (Exception ex)
            {
                return "error: " + ex;
            }

        }
        
        public string SendMail2(int jobId, string renterMail, string mailType, string renterName, string site, string errorText)
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("mail.realwheelsdavao.com"); //smtp server

                MailDefinition md = new MailDefinition();
                md.From = "Realwheels.Reservation@RealWheelsDavao.com";      //sender mail
                md.IsBodyHtml = true;                       //set true to enable use of html tags 
                md.Subject = "RealWheels Reservation";      //mail title

                ListDictionary replacements = new ListDictionary();
                replacements.Add("{name}", "Martin");
                replacements.Add("{unit}", "Honda City");
                replacements.Add("{tour}", "City Tour");
                replacements.Add("{type}", "w/ Driver");
                replacements.Add("{days}", "2");
                replacements.Add("{total}", "5500");

                string body, message;
                string siteName = site;
                //get job details

                md.Subject = renterName + ": NEW RealWheels Reservation";   //mail title
                
                    //send email in /joborder
                    JobMain job = db.JobMains.Find(jobId);
                    //mail title
                    md.Subject = "Realwheels Reservation : " + errorText;

                    //encode white space
                    string jobDesc = System.Web.HttpUtility.UrlPathEncode(job.Description);

                    //mail content for client inquiries

                    message = errorText + " Your inquiry have been processed to confirm your reservation, please follow the link for the invoice and payment. <a href='" + siteName + "" + jobId + "/" + job.JobDate.Month + "/" + job.JobDate.Day + "/" + job.JobDate.Year + "/" + jobDesc + "/' " +
                        "> View Booking Details  </a> ";
                        //" style='display:block;background-color:dodgerblue;margin:20px;padding:20px;text-decoration:none;font-weight:bolder;font-size:300;color:white;border-radius:3px;min-width:100px;'> View Booking Details </a> ";

                body =
                    "" +
                    // " <p>Hello {name}, You have booked a {tour} and  {unit} {type} for {days} day(s). The total cost of the package is {total}. </p>" +
                    " <div style='background-color:#f4f4f4;padding:20px' align='center'>" +
                    " <div style='background-color:white;min-width:250px;margin:30px;padding:30px;text-align:center;color:#555555;font:normal 300 16px/21px 'Helvetica Neue',Arial'>  <h1> RealWheels Car Reservation </h1>" +
                    message +
                    " <p> This is an auto-generated email. DO NOT REPLY TO THIS MESSAGE </p> " +
                    " <p> For further inquiries kindly email us through inquiries.realwheels@gmail.com or dial(+63) 82 297 1831. </p> " +
                    " </div></div>" +
                    "";

                MailMessage msg = md.CreateMailMessage(renterMail, replacements, body, new System.Web.UI.Control());

                SmtpServer.Port = 587;          //default smtp port
                SmtpServer.Credentials = new System.Net.NetworkCredential(
                    System.Web.Configuration.WebConfigurationManager.AppSettings["SmtpEmail"],
                    System.Web.Configuration.WebConfigurationManager.AppSettings["SmtpPass"]);
                SmtpServer.EnableSsl = false;   //enable for gmail smtp server
                System.Net.ServicePointManager.Expect100Continue = false;
                SmtpServer.Send(msg);           //send message
                return "success";
            }
            catch (Exception ex)
            {
                return "error: " + ex;
            }
        }
        
        /**
         * ADMIN -  SEND INVOICE SUCCESSFUL
         * Send email to client on payment success
         */
        public string SendMailInvoiceAdvice(int jobId, string renterMail, string mailType, string renterName, string site)
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("mail.realwheelsdavao.com"); //smtp server

                MailDefinition md = new MailDefinition();
                md.From = "Realwheels.Reservation@RealWheelsDavao.com";      //sender mail
                md.IsBodyHtml = true;                       //set true to enable use of html tags 
                md.Subject = "RealWheels Reservation";      //mail title

                ListDictionary replacements = new ListDictionary();
                replacements.Add("<%To%>", renterMail);
                replacements.Add("<%From%>", md.From);

                string body, message;
                string siteName = site;
                //get job details
                
                //send email in /joborder
                JobMain job = db.JobMains.Find(jobId);
                //mail title
                md.Subject = job.Description + " Invoice Sent";

                //encode white space
                string jobDesc = System.Web.HttpUtility.UrlPathEncode(job.Description);

                //mail content for client inquiries

                message = " An invoice link has been sent to "+ job.Description +". Please follow the link for the invoice and payment. <a href='" + siteName + "" + jobId + "/" + job.JobDate.Month + "/" + job.JobDate.Day + "/" + job.JobDate.Year + "/" + jobDesc + "/' " +
                "> View Invoice </a> ";
                //" style='display:block;background-color:dodgerblue;margin:20px;padding:20px;text-decoration:none;font-weight:bolder;font-size:300;color:white;border-radius:3px;min-width:100px;'> View Invoice </a> ";

                body =
                    "" +
                    // " <p>Hello {name}, You have booked a {tour} and  {unit} {type} for {days} day(s). The total cost of the package is {total}. </p>" +
                    " <div style='background-color:#f4f4f4;padding:20px' align='center'>" +
                    " <div style='background-color:white;min-width:250px;margin:30px;padding:30px;text-align:center;color:#555555;font:normal 300 16px/21px 'Helvetica Neue',Arial'>  <h1> Invoice Link Sent </h1>" +
                    message +
                    " <p> This is an auto-generated email. DO NOT REPLY TO THIS MESSAGE </p> " +
                    " <p> For further inquiries kindly email us through inquiries.realwheels@gmail.com or dial(+63) 82 297 1831. </p> " +
                    " </div></div>" +
                    "";

                MailMessage msg = md.CreateMailMessage(renterMail, replacements, body, new System.Web.UI.Control());

                SmtpServer.Port = 587;          //default smtp port
                SmtpServer.Credentials = new System.Net.NetworkCredential(
                    System.Web.Configuration.WebConfigurationManager.AppSettings["SmtpEmail"],
                    System.Web.Configuration.WebConfigurationManager.AppSettings["SmtpPass"]);
                SmtpServer.EnableSsl = false;   //enable for gmail smtp server
                System.Net.ServicePointManager.Expect100Continue = false;
                SmtpServer.Send(msg);           //send message
                return "success";
            }
            catch (Exception ex)
            {
                return "error: " + ex;
            }
        }
        
        /**
         * ADMIN -  PAYMENT SUCCESSFUL
         * Send email to client on payment success
         */
        public string SendMailPaymentAdvice(int jobId, string renterMail, string mailType, string renterName, string site)
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("mail.realwheelsdavao.com"); //smtp server

                MailDefinition md = new MailDefinition();
                md.From = "Realwheels.Reservation@RealWheelsDavao.com";      //sender mail
                md.IsBodyHtml = true;                       //set true to enable use of html tags 
                md.Subject = "RealWheels Reservation";      //mail title

                ListDictionary replacements = new ListDictionary();
                replacements.Add("<%To%>", renterMail);
                replacements.Add("<%From%>", md.From);

                string body, message;
                string siteName = site;
                //get job details

                //send email in /joborder
                JobMain job = db.JobMains.Find(jobId);
                //mail title
                md.Subject = "Payment Success";

                //encode white space
                string jobDesc = System.Web.HttpUtility.UrlPathEncode(job.Description);

                //mail content for client inquiries

                message = "A New Payment has been made. Please follow the link for the invoice and payment. <a href='" + siteName + "" + jobId + "/" + job.JobDate.Month + "/" + job.JobDate.Day + "/" + job.JobDate.Year + "/" + jobDesc + "/' " +
                "> View Invoice </a> ";
                //" style='display:block;background-color:dodgerblue;margin:20px;padding:20px;text-decoration:none;font-weight:bolder;font-size:300;color:white;border-radius:3px;min-width:100px;'> View Invoice </a> ";

                body =
                    "" +
                    // " <p>Hello {name}, You have booked a {tour} and  {unit} {type} for {days} day(s). The total cost of the package is {total}. </p>" +
                    " <div style='background-color:#f4f4f4;padding:20px' align='center'>" +
                    " <div style='background-color:white;min-width:200px;margin:30px;padding:30px;text-align:center;color:#555555;font:normal 300 16px/21px 'Helvetica Neue',Arial'>  <h1> Payment Successful </h1>" +
                    message +
                    " <p> This is an auto-generated email. DO NOT REPLY TO THIS MESSAGE. </p> " +
                    " <p> For further inquiries kindly email us through inquiries.realwheels@gmail.com or dial(+63) 82 297 1831. </p> " +
                    " </div></div>" +
                    "";

                MailMessage msg = md.CreateMailMessage(renterMail, replacements, body, new System.Web.UI.Control());

                SmtpServer.Port = 587;          //default smtp port
                SmtpServer.Credentials = new System.Net.NetworkCredential(
                    System.Web.Configuration.WebConfigurationManager.AppSettings["SmtpEmail"],
                    System.Web.Configuration.WebConfigurationManager.AppSettings["SmtpPass"]);
                SmtpServer.EnableSsl = false;   //enable for gmail smtp server
                System.Net.ServicePointManager.Expect100Continue = false;
                SmtpServer.Send(msg);           //send message
                return "success";
            }
            catch (Exception ex)
            {
                return "error: " + ex;
            }
        }

        /**
         * CLIENT -  SEND INVOICE SUCCESSFUL
         * Send email to client on payment success
         */
        public string SendMailClientInvoice(int jobId, string renterMail, string mailType, string renterName, string site)
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("mail.realwheelsdavao.com"); //smtp server

                MailDefinition md = new MailDefinition();
                md.From = "Realwheels.Reservation@RealWheelsDavao.com";      //sender mail
                md.IsBodyHtml = true;                       //set true to enable use of html tags 
                md.Subject = "RealWheels Reservation";      //mail title

                ListDictionary replacements = new ListDictionary();
                replacements.Add("{name}", "Martin");
                replacements.Add("{unit}", "Honda City");
                replacements.Add("{tour}", "City Tour");
                replacements.Add("{type}", "w/ Driver");
                replacements.Add("{days}", "2");
                replacements.Add("{total}", "5500");

                string body, message;
                string siteName = site;
                //get job details

                //send email in /joborder
                JobMain job = db.JobMains.Find(jobId);
                //mail title
                md.Subject = "Realwheels Payment";

                //mail content for client inquiries
                string jobDesc = System.Web.HttpUtility.UrlPathEncode(job.Description);

                message = "Good day, please follow the link for the invoice and payment of your reservation. <a href='" + siteName + "" + jobId + "/" + job.JobDate.Month + "/" + job.JobDate.Day + "/" + job.JobDate.Year + "/" + jobDesc + "/' " +
                "> View Invoice </a> ";
                //" style='display:block;background-color:dodgerblue;margin:20px;padding:20px;text-decoration:none;font-weight:bolder;font-size:300;color:white;border-radius:3px;min-width:100px;'> View Invoice </a> ";

                body =
                    "" +
                    // " <p>Hello {name}, You have booked a {tour} and  {unit} {type} for {days} day(s). The total cost of the package is {total}. </p>" +
                    " <div style='background-color:#f4f4f4;padding:20px' align='center'>" +
                    " <div style='background-color:white;min-width:200px;margin:20px;padding:30px;text-align:center;color:#555555;font:normal 300 16px/21px 'Helvetica Neue',Arial'>  <h1> Reservation Invoice </h1>" +
                    message +
                    " <p> This is an auto-generated email. DO NOT REPLY TO THIS MESSAGE </p> " +
                    " <p> For further inquiries kindly email us through inquiries.realwheels@gmail.com or dial(+63) 82 297 1831. </p> " +
                    " </div></div>" +
                    "";

                MailMessage msg = md.CreateMailMessage(renterMail, replacements, body, new System.Web.UI.Control());

                SmtpServer.Port = 587;          //default smtp port
                SmtpServer.Credentials = new System.Net.NetworkCredential(
                    System.Web.Configuration.WebConfigurationManager.AppSettings["SmtpEmail"],
                    System.Web.Configuration.WebConfigurationManager.AppSettings["SmtpPass"]);
                SmtpServer.EnableSsl = false;   //enable for gmail smtp server
                System.Net.ServicePointManager.Expect100Continue = false;
                SmtpServer.Send(msg);           //send message
                return "success";
            }
            catch (Exception ex)
            {
                return "error: " + ex;
            }
        }
        
        /**
         * CLIENT -  PAYMENT SUCCESSFUL
         * Send email to client on payment success
         */
        public string SendMailClientPayment(int jobId, string renterMail, string mailType, string renterName, string site)
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("mail.realwheelsdavao.com"); //smtp server

                MailDefinition md = new MailDefinition();
                md.From = "Realwheels.Reservation@RealWheelsDavao.com";      //sender mail
                md.IsBodyHtml = true;                       //set true to enable use of html tags 
                md.Subject = "RealWheels Reservation";      //mail title

                ListDictionary replacements = new ListDictionary();
                replacements.Add("<%To%>", renterMail);
                replacements.Add("<%From%>", md.From);

                string body, message;
                string siteName = site;
                //get job details

                //send email in /joborder
                JobMain job = db.JobMains.Find(jobId);
                //mail title
                md.Subject = "Payment Success";

                //encode white space
                string jobDesc = System.Web.HttpUtility.UrlPathEncode(job.Description);

                //mail content for client inquiries

                message = "Thank you for your payment. Please follow the link for the invoice and payment. <a href='" + siteName + "" + jobId + "/" + job.JobDate.Month + "/" + job.JobDate.Day + "/" + job.JobDate.Year + "/" + jobDesc + "/' " +
                "> View Invoice </a> ";
                //" style='display:block;background-color:dodgerblue;margin:20px;padding:20px;text-decoration:none;font-weight:bolder;font-size:300;color:white;border-radius:3px;min-width:100px;'> View Invoice </a> ";

                body =
                    "" +
                    // " <p>Hello {name}, You have booked a {tour} and  {unit} {type} for {days} day(s). The total cost of the package is {total}. </p>" +
                    " <div style='background-color:#f4f4f4;padding:20px' align='center'>" +
                    " <div style='background-color:white;min-width:200px;margin:30px;padding:30px;text-align:center;color:#555555;font:normal 300 16px/21px 'Helvetica Neue',Arial'>  <h1> Payment Successful </h1>" +
                    message +
                    " <p> This is an auto-generated email. DO NOT REPLY TO THIS MESSAGE. </p> " +
                    " <p> For further inquiries kindly email us through inquiries.realwheels@gmail.com or dial(+63) 82 297 1831. </p> " +
                    " </div></div>" +
                    "";

                MailMessage msg = md.CreateMailMessage(renterMail, replacements, body, new System.Web.UI.Control());

                SmtpServer.Port = 587;          //default smtp port
                SmtpServer.Credentials = new System.Net.NetworkCredential(
                    System.Web.Configuration.WebConfigurationManager.AppSettings["SmtpEmail"],
                    System.Web.Configuration.WebConfigurationManager.AppSettings["SmtpPass"]);
                SmtpServer.EnableSsl = false;   //enable for gmail smtp server
                System.Net.ServicePointManager.Expect100Continue = false;
                SmtpServer.Send(msg);           //send message
                return "success";
            }
            catch (Exception ex)
            {
                return "error: " + ex;
            }
        }

        /**
         * CLIENT -  SEND EMAIL QUOTATION
         * Send email to client on payment success
         */
        public string SendMailQuotation(int jobId, string clientMail, List<JobServices> js)
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("mail.realwheelsdavao.com"); //smtp server

                MailDefinition md = new MailDefinition();
                md.From = "Realwheels.Reservation@RealWheelsDavao.com";      //sender mail
                md.IsBodyHtml = true;                       //set true to enable use of html tags 
                md.Subject = "Quotation";      //mail title

                ListDictionary replacements = new ListDictionary();
                replacements.Add("<%To%>", clientMail);
                replacements.Add("<%From%>", md.From);

                string body, message;
                //get job details

                //send email in /joborder
                JobMain job = db.JobMains.Find(jobId);

                //mail title
                md.Subject = "Quotation";

                //encode white space
                string jobDesc = System.Web.HttpUtility.UrlPathEncode(job.Description);

                //mail content for client inquiries
                var services = "";
                var destinations = "";

                var Itineraries = db.JobItineraries.Where(d => d.JobMainId == jobId).ToList();

                var JobEncoder = db.JobTrails.Where(s => s.RefTable == "joborder" && s.RefId == jobId.ToString()).FirstOrDefault();

                //get date today
                DateClass dt = new DateClass();
                DateTime today =  dt.GetCurrentTime();
                string dateValid = today.AddMonths(1).ToString("MMM dd yyyy");
                decimal totalAmount = 0;

                foreach (var service in js)
                {
                    services +=
                     "<tr><td> <b>" + service.Service.Name + "</b> <br/>"
                    + "Start: "+ service.DtStart.Value.ToString("MMM dd yyyy (ddd)") + " <br/>"
                    + "End: " + service.DtEnd.Value.ToString("MMM dd yyyy (ddd)") + " </td>"
                    + "<td> "+ service.Particulars +" <br/>"
                    + "Item: "+ service.SupplierItem.Description +" <br/>"
                    + "Remarks: "+ service.Remarks +" </td>"
                    + "<td> " + Convert.ToDecimal(service.ActualAmt).ToString("#,##0.00")  +" </td></tr>";

                    totalAmount += (decimal)service.ActualAmt;
                }

                if(Itineraries != null)
                {
                    destinations = "Destinations: ";
                }

                foreach (var dest in Itineraries)
                {
                    destinations += dest.Destination.Description +", ";
                }

                
                
                body =
                    "" +
                    // " <p>Hello {name}, You have booked a {tour} and  {unit} {type} for {days} day(s). The total cost of the package is {total}. </p>" +
                    " <div style='background-color:#f4f4f4;padding:30px' >" +

                     "<img src='"+getHeader(job.Branch.Name) +"' width='100%' />"
                    + "<div style='background-color:white;padding:30px'>"
                    + "<h1> Quotation </h1><div>"

                    //details
                    + " Job Ref # : " + job.Id + " <br/>"
                    + " Date : "+ job.JobDate.ToString("MMM dd yyyy (ddd)") + " <br/>"
                    + " Details : "+ job.Description + "<br/>"
                    + " Pax : "+ job.NoOfPax + " <br/>"
                    + " Days : "+ job.NoOfDays + " <br/>"
                    + " Remarks : "+ job.JobRemarks + " <br/>"
                    + "</div>"

                    //services
                    + "<div><h2> Services </h2>"
                    + "<table width='100%'  style='text-alight:left;'><tbody><tr><th>Type</th>"
                    + "<th> Pacticulars </th><th> Amount </th></tr>"
                    + services
                    + "</tbody></table>"

                    //destination
                    + "<br/><p> "+ destinations + " </p></div>"

                    //Summary
                    + "<div width='100%' style='background-color:#f1f1f1;padding:30px;margin-top:30px;margin-botom:20px;'>"
                    + "<h2> Summary </h2><h2> Total Due: "+ Convert.ToDecimal(totalAmount).ToString("#,##0.00")  + " </h2></div><br/><br/>"

                    + "<p></p><div><p> Prepared by: <b>"+ getStaffName(JobEncoder.user) +"</b> </p>"
                    + "<p> Validity: <b>"+ dateValid + "</b></p></div><hr>" +
                    //footer
                    " <p style='text-align:center;'> This is an auto-generated email. DO NOT REPLY TO THIS MESSAGE. </p> " +
                    " <p style='text-align:center;'> For further inquiries kindly email us through " + getcallBackEmail(job.Branch.Name) + " or dial(+63) 82 297 1831. </p> " +
                    "</div></div>" +
                    "";

                MailMessage msg = md.CreateMailMessage(clientMail, replacements, body, new System.Web.UI.Control());

                SmtpServer.Port = 587;          //default smtp port
                SmtpServer.Credentials = new System.Net.NetworkCredential(
                    System.Web.Configuration.WebConfigurationManager.AppSettings["SmtpEmail"],
                    System.Web.Configuration.WebConfigurationManager.AppSettings["SmtpPass"]);
                SmtpServer.EnableSsl = false;   //enable for gmail smtp server
                System.Net.ServicePointManager.Expect100Continue = false;
                SmtpServer.Send(msg);           //send message
                return "success";
            }
            catch (Exception ex)
            {
                return "error: " + ex;
            }
        }

        /**
         * CLIENT -  SEND EMAIL RESERVATION DETAILS
         * Send email to client on payment success
         */
        public string SendMailReservation(int jobId, string clientMail, List<JobServices> js)
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("mail.realwheelsdavao.com"); //smtp server

                MailDefinition md = new MailDefinition();
                md.From = "Realwheels.Reservation@RealWheelsDavao.com";      //sender mail
                md.IsBodyHtml = true;                       //set true to enable use of html tags 
                md.Subject = "Reservation";      //mail title

                ListDictionary replacements = new ListDictionary();
                replacements.Add("<%To%>", clientMail);
                replacements.Add("<%From%>", md.From);

                string body, message;
                //get job details

                //send email in /joborder
                JobMain job = db.JobMains.Find(jobId);

                //mail title
                md.Subject = "Reservation Details";

                //encode white space
                string jobDesc = System.Web.HttpUtility.UrlPathEncode(job.Description);

                //mail content for client inquiries
                var services = "";
                var destinations = "";
                
                //get date today
                DateClass dt = new DateClass();
                DateTime today = dt.GetCurrentTime();
                string dateValid = today.AddMonths(1).ToString("MMM dd yyyy");
                decimal totalAmount = 0;

                //get job services
                var JobEncoder = db.JobTrails.Where(s => s.RefTable == "joborder" && s.RefId == jobId.ToString()).FirstOrDefault();

                foreach (var service in js)
                {
                    string jobitems = "";
                    foreach (var item in service.JobServiceItems)
                    {
                        jobitems += " ("+ item.InvItem.ItemCode +")" 
                            + item.InvItem.Description + "<br/>";
                    }

                    services +=
                    //date time
                     "<tr style='border:1px solid black;'><td><b>" + service.Service.Name + "</b><br/>"
                    + "Start: " + service.DtStart.Value.ToString("MMM dd yyyy (ddd)") + " <br/>"
                    + "End: " + service.DtEnd.Value.ToString("MMM dd yyyy (ddd)") + " </td>"

                    //particulars
                    + "<td><b> P " + Convert.ToDecimal(service.ActualAmt).ToString("#,##0.00") + " - " + service.Particulars + "</b><br/>"
                    + "Remarks: " + service.Remarks + " <br/> "
                    + "Item: " + service.SupplierItem.Description + " </td>"

                    // Assigned items
                    + "<td> " + jobitems + " </td></tr>";

                    totalAmount += (decimal)service.ActualAmt;
                }

                //get job destinations
                var Itineraries = db.JobItineraries.Where(d => d.JobMainId == jobId).ToList();
                if (Itineraries != null)
                {
                    destinations = "Destinations: ";
                }

                foreach (var dest in Itineraries)
                {
                    destinations += dest.Destination.Description + ", ";
                }

                //get job payments
                var payments = db.JobPayments.Where(p => p.JobMainId == jobId).ToList();
                string paymentTxt = "";
                decimal partial = 0;
                if (payments != null)
                {
                    foreach (var trans in payments)
                    {
                        paymentTxt = "<tr style='border: 1px solid #dddddd;'><td> " + Convert.ToDecimal(trans.PaymentAmt).ToString("#,##0.00")  + " </td>"
                                    +"<td> " + trans.DtPayment.ToShortDateString() + " ["+ trans.Bank.BankName +"] </td></tr>";

                        partial += trans.PaymentAmt;
                    }
                }


                //get balance
                decimal balance =  totalAmount - partial ;

                //start build email body
                body =
                    "" +
                    " <div style='background-color:#f4f4f4;padding:30px' >" +
                     "<img src='" + getHeader(job.Branch.Name) + "' width='100%' />"
                    + "<div style='background-color:white;padding:30px'>"

                    //header
                    + "<h1> Reservation Details </h1><div>"
                    + " Job Ref # : " + job.Id + " <br/>"
                    + " Date : " + job.JobDate.ToString("MMM dd yyyy (ddd)") + " <br/>"
                    + " Account : " + job.Description + "<br/>"
                    + " Pax : " + job.NoOfPax + " | "
                    + " Days : " + job.NoOfDays + " <br/>"
                    + " Remarks : " + job.JobRemarks + " <br/>"
                    + " Status : " + job.JobStatus.Status + " <br/>"
                    + "</div>"

                    //Services
                    + "<div><h2> Services </h2>"
                    + "<table width='100%' style='text-alight:left;'><tbody><tr><th>Type</th>"
                    + "<th> Pacticulars </th><th> Assigned </th></tr>"
                    + services
                    + "</tbody></table>"

                    //destinations
                    + "<br/><p><b>" + destinations + "</b></p></div>"

                    //Payments
                    + "<h2>Payments</h2>"
                    + "<table width='50%'>"
                    + "<tr><td><b>Amount</b></td><td><b>Particulars</b></td></tr>"
                    + paymentTxt
                    + "</table>"

                    //Summary
                    + "<div width='100%' style='background-color:#f1f1f1;padding:30px;margin-top:30px;margin-botom:20px;'>"
                    + "<h2> Summary </h2><h3> Total Due: " + Convert.ToDecimal(totalAmount).ToString("#,##0.00")  + " </h3>"
                    + "<h3> Partial: "+ Convert.ToDecimal(partial).ToString("#,##0.00")  + "</h3>"
                    + "<h3> Balance: " + Convert.ToDecimal(balance).ToString("#,##0.00")  + "</h3></div>"
                    + "<br><br><br>"
                    + "<p></p><div><p> Prepared by: <b>" + getStaffName(JobEncoder.user) + "</b> </p>"
                    + "<p> Validity: <b>" + dateValid + "</b></p></div><br><br><br><hr>" +

                    //Footer
                    " <p style='text-align:center;'> This is an auto-generated email. DO NOT REPLY TO THIS MESSAGE. </p> " +
                    " <p style='text-align:center;'> For further inquiries kindly email us through " + getcallBackEmail(job.Branch.Name) + " or dial(+63) 82 297 1831. </p> " +
                    "</div></div>" +
                    "";

                MailMessage msg = md.CreateMailMessage(clientMail, replacements, body, new System.Web.UI.Control());

                SmtpServer.Port = 587;          //default smtp port
                SmtpServer.Credentials = new System.Net.NetworkCredential(
                    System.Web.Configuration.WebConfigurationManager.AppSettings["SmtpEmail"],
                    System.Web.Configuration.WebConfigurationManager.AppSettings["SmtpPass"]);
                SmtpServer.EnableSsl = false;   //enable for gmail smtp server
                System.Net.ServicePointManager.Expect100Continue = false;
                SmtpServer.Send(msg);           //send message
                return "success";
            }
            catch (Exception ex)
            {
                return "error: " + ex;
            }
        }

        public string getStaffName(string staffLogin)
        {
            switch (staffLogin)
            {
                case "josette.realbreeze@gmail.com ":
                    return "Josette Villaser";
                case "mae.realbreeze@gmail.com":
                    return "Mae Verano";
                case "ramil.realbreeze@gmail.com":
                    return "Ramil Salvatierra";
                case "assalvatierra@gmail.com":
                    return "Elvie S. Salvatierra ";
                default:
                    return "Elvie S. Salvatierra ";
            }
        }

        public string getcallBackEmail(string branch)
        {
            switch (branch)
            {
                case "AJ88":
                    return "inquiries.realwheels@gmail.com";
                case "Realbreeze":
                default:
                    return "travel.realbreeze@gmail.com ";
            }
        }

        public string getHeader(string branch)
        {
            switch (branch)
            {
                case "AJ88":
                    return "http://realbreezedavaotours.com/wp-content/uploads/2019/05/AJDavao.jpg";
                case "Realbreeze":
                    return "http://realbreezedavaotours.com/wp-content/uploads/2019/05/Header.jpg";
                default:
                    return "http://realbreezedavaotours.com/wp-content/uploads/2019/05/AJDavao.jpg";
            }
        }
    }
}