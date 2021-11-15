using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using JobsV1.Areas.Products.Models;

namespace JobsV1.Models
{
    public class EMailHandler
    {
        private JobDBContainer db = new JobDBContainer();
        private ProdDBContainer pdb = new ProdDBContainer();

        /**
         *  EMAIL SENDER
         *  Send email to client and admin emails based on the mail type.
         **/
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
                md.IsBodyHtml = true;             //set true to enable use of html tags 
                md.Subject = " Reservation";      //mail title

                ListDictionary replacements = new ListDictionary();
                replacements.Add("{name}", "Reservation");

                string body, message;
                string siteName = site;

                //get job details
                JobMain job = db.JobMains.Find(jobId);

                string company = getCompany(jobId);

                //encode white space
                string jobDesc = System.Web.HttpUtility.UrlPathEncode(job.Description);

                md.Subject = renterName + ": NEW "+ job.Branch.Name  + " Reservation";   //mail title
                                                                                        
                CarReservation reserve = db.CarReservations.Find(jobId); //find reservation

                switch (mailType)
                {
                    case "ADMIN":
                        //mail title
                        md.Subject = job.Branch.Name + ": Reservation ";

                        //email content
                        message = " <h1> " + job.Branch.Name + " Inquiry </h1>";
                        message += "A NEW Reservation Inquiry has been made. Please follow the link for the reservation details. <a href='" + siteName + "" + jobId + "/" + reserve.DtTrx.Month + "/" + reserve.DtTrx.Day + "/" + reserve.DtTrx.Year + "/" + reserve.RenterName + "/' " +
                        "> View Reservation Details  </a> ";
                        break;
                    case "PAYMENT-SUCCESS":
                        //mail content for successful payment
                        //mail title
                        md.Subject = job.Branch.Name + ": Reservation ";
                        
                        //email content
                        message = "Paypal Payment is successful. Please follow the link for the invoice details. <a href='" + siteName + "" + jobId + "/" + reserve.DtTrx.Month + "/" + reserve.DtTrx.Day + "/" + reserve.DtTrx.Year + "/" + reserve.RenterName + "/' " +
                        "> View Invoice Details  </a> ";
                        break;
                    case "PAYMENT-DENIED":
                        //mail content for denied payment
                        //mail title
                        md.Subject = job.Branch.Name + ": Reservation ";
                        
                        //email content
                        message = "Paypal Payment have been DENIED. Please follow the link for the invoice details. <a href='" + siteName + "" + jobId + "/" + reserve.DtTrx.Month + "/" + reserve.DtTrx.Day + "/" + reserve.DtTrx.Year + "/" + reserve.RenterName + "/' " +
                        "> View Invoice Details  </a> ";
                        break;
                    case "PAYMENT-PENDING":
                        //mail content for pending payment
                        //mail title
                        md.Subject = job.Branch.Name + ": Reservation ";
                        
                        //email content
                        message = "Paypal Payment has been sent. Please follow the link for the invoice details. <a href='" + siteName + "" + jobId + "/" + reserve.DtTrx.Month + "/" + reserve.DtTrx.Day + "/" + reserve.DtTrx.Year + "/" + reserve.RenterName + "/' " +
                        "> View Invoice  </a> ";
                        break;
                    case "CLIENT-PENDING":
                        //mail title
                        md.Subject = "Realwheels Reservation";
                        
                        //mail content for new inquiry recieved
                        message = "We are happy to recieved your inquiry. We will contact you after we have processed your reservation. Please click the link below for your reservation details, Thank you. <a href='" + siteName + "" + jobId + "/" + reserve.DtTrx.Month + "/" + reserve.DtTrx.Day + "/" + reserve.DtTrx.Year + "/" + reserve.RenterName + "/' " +
                        "> View Booking Details  </a> ";
                        break;
                    case "CLIENT-INQUIRY":
                        //Client Inquiry
                        //mail title
                        md.Subject = "Realwheels Reservation";
                        
                        //mail content for Client Inquiry
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

                        //mail content for Invoice link
                        message = " <h1> " + job.Branch.Name + " Invoice </h1>";
                        message += "Good day, please follow the link for the invoice and payment of your reservation."+
                            "<br> <a href='" + siteName + "" + jobId + "/" + job.JobDate.Month + "/" + job.JobDate.Day + 
                            "/" + job.JobDate.Year + "/" + jobDesc + "/' " +
                            "> View Invoice </a> " +
                            " <p>For PAYPAL Payment, please click the blue paypal button at the lower right of the invoice. </p><br />"
                            ;
                        break;
                    case "CLIENT-PAYMENT-SUCCESS":
                        //mail title
                        md.Subject = job.Branch.Name + " Payment";

                        //mail content for payment successful
                        message = " <h1> " + job.Branch.Name + " Payment Success </h1>";
                        message += "Thank you for your payment. <br> Please follow the link for the invoice and payment."+
                            "<br> <a href='" + siteName + "" + jobId + "/" + job.JobDate.Month + "/" + job.JobDate.Day + 
                            "/" + job.JobDate.Year + "/" + jobDesc + "/' " +
                            "> View Invoice </a> ";
                        break;
                    case "ADMIN-INVOICE-SENT":
                        
                        //mail title
                        md.Subject = job.Description + " Invoice Sent";

                        //mail content for admin when a new invoice is sent 
                        message = " <h1> " + job.Branch.Name + " Invoice Sent </h1>";
                        message += " An invoice link has been sent to " + job.Description + ". Please follow the link"+
                            " for the invoice and payment.<br> <a href='" + siteName + "" + jobId + "/" + 
                            job.JobDate.Month + "/" + job.JobDate.Day + "/" + job.JobDate.Year + "/" + jobDesc + "/' " +
                            "> View Invoice </a> ";
                        break;
                    case "ADMIN-PAYMENT-SUCCESS":

                        //mail title
                        md.Subject = job.Description + " Payment Success";

                        //mail content for admin
                        message = " <h1> " + job.Branch.Name + " Payment </h1>";
                        message += "A New Payment has been made. Please follow the link for the invoice and payment.<br>"+
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
                        
                        break;
                }
                
                body =
                    "" +
                    " <div style='background-color:#f4f4f4;padding:20px' align='center'>" +
                    " <div style='background-color:white;min-width:200px;margin:30px;padding:30px;text-align:center;color:#555555;font:normal 300 16px/21px 'Helvetica Neue',Arial'> " +
                    message +
                    " <p> This is an auto-generated email. DO NOT REPLY TO THIS MESSAGE </p> " +
                    " <p> For further inquiries kindly email us through inquiries.realwheels@gmail.com or dial(+63) 082 333 5157. </p> " +
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
                throw ex;
                //return "error: " + ex.Message;
            }
        }
        
        //EMAIL TEMPLATE FOR CUSTOM EMAIL BODY
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
                       
                body = "" +
                    " <div style='background-color:#f4f4f4;padding:20px' align='center'>" +
                    " <div style='background-color:white;min-width:250px;margin:30px;padding:30px;text-align:center;color:#555555;font:normal 300 16px/21px 'Helvetica Neue',Arial'>  <h1> RealWheels Car Reservation </h1>" +
                    message +
                    " <p> This is an auto-generated email. DO NOT REPLY TO THIS MESSAGE </p> " +
                    " <p> For further inquiries kindly email us through inquiries.realwheels@gmail.com or dial(+63) 082 333 5157. </p> " +
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
         * Send email to admin on payment success
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

                string company = getCompany(jobId);

                //mail title
                md.Subject = job.Description + " Invoice Sent";

                //encode white space
                string jobDesc = System.Web.HttpUtility.UrlPathEncode(job.Description);

                //mail content for client inquiries

                message = " An invoice link has been sent to "+ job.Description +". Please follow the link for the invoice and payment. <a href='" + siteName + "" + jobId + "/" + job.JobDate.Month + "/" + job.JobDate.Day + "/" + job.JobDate.Year + "/" + jobDesc + "/' " +
                "> View Invoice </a> ";
               
                body =
                    "" +
                    " <div style='background-color:#f4f4f4;padding:20px' align='center'>" +
                    " <div style='background-color:white;min-width:250px;margin:30px;padding:30px;text-align:center;color:#555555;font:normal 300 16px/21px 'Helvetica Neue',Arial'>  <h1> Invoice Link Sent </h1>" +
                    message +
                    " <p> This is an auto-generated email. DO NOT REPLY TO THIS MESSAGE </p> " +
                    " <p> For further inquiries kindly email us through inquiries.realwheels@gmail.com or dial(+63) 082 333 5157. </p> " +
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
         * ADMIN -  PAYMENT SUCCESSFUL ADVICE
         * Send email to admin on payment success
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
               
                body =
                    "" +
                    " <div style='background-color:#f4f4f4;padding:20px' align='center'>" +
                    " <div style='background-color:white;min-width:200px;margin:30px;padding:30px;text-align:center;color:#555555;font:normal 300 16px/21px 'Helvetica Neue',Arial'>  <h1> Payment Successful </h1>" +
                    message +
                    " <p> This is an auto-generated email. DO NOT REPLY TO THIS MESSAGE. </p> " +
                    " <p> For further inquiries kindly email us through inquiries.realwheels@gmail.com or dial(+63) 082 333 5157. </p> " +
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
         * CLIENT -  SEND INVOICE TO CLIENT
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
                md.Subject = "Invoice for Payment";      //mail title

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
                
                string company = getCompany(jobId);

                //mail title
                md.Subject = company +  " Payment";

                //mail content for client inquiries
                string jobDesc = System.Web.HttpUtility.UrlPathEncode(job.Description);

                message = "Good day, please follow the link for the invoice and payment of your reservation. <a href='" + siteName + "" + jobId + "/" + job.JobDate.Month + "/" + job.JobDate.Day + "/" + job.JobDate.Year + "/" + jobDesc + "/' " +
                "> View Invoice </a> ";
               
                body =
                    "" +
                    " <div style='background-color:#f4f4f4;padding:20px' align='center'>" +
                    " <div style='background-color:white;min-width:200px;margin:20px;padding:30px;text-align:center;color:#555555;font:normal 300 16px/21px 'Helvetica Neue',Arial'>  <h1> Reservation Invoice </h1>" +
                    message +
                    " <p> This is an auto-generated email. DO NOT REPLY TO THIS MESSAGE </p> " +
                    " <p> For further inquiries kindly email us through inquiries.realwheels@gmail.com or dial(+63) 082 333 5157. </p> " +
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
                message = "Thank you for your payment. <br> Follow the link for the invoice. <a href='" + siteName + "" + jobId + "/" + job.JobDate.Month + "/" + job.JobDate.Day + "/" + job.JobDate.Year + "/" + jobDesc + "/' " +
                "> View Invoice </a> ";
               
                body =
                    "" +
                    " <div style='background-color:#f4f4f4;padding:20px' align='center'>" +
                    " <div style='background-color:white;min-width:200px;margin:30px;padding:30px;text-align:center;color:#555555;font:normal 300 16px/21px 'Helvetica Neue',Arial'>  <h1> Payment Successful </h1>" +
                    message +
                    " <p> This is an auto-generated email. DO NOT REPLY TO THIS MESSAGE. </p> " +
                    " <p> For further inquiries kindly email us through inquiries.realwheels@gmail.com or dial(+63) 082 333 5157. </p> " +
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
         * Send email to client 
         */
        public string SendMailQuotation(int jobId, string clientMail, List<JobServices> js)
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("mail.realwheelsdavao.com"); //smtp server

                MailDefinition md = new MailDefinition();
                md.From = "Realwheels.Reservation@RealWheelsDavao.com";      //sender mail
                md.IsBodyHtml = true;          //set true to enable use of html tags 
                md.Subject = "Quotation";      //mail title

                ListDictionary replacements = new ListDictionary();
                replacements.Add("<%To%>", clientMail);
                replacements.Add("<%From%>", md.From);

                string body;
                //get job details

                //send email in /joborder
                JobMain job = db.JobMains.Find(jobId);

                //mail title
                md.Subject = job.Description +  " Quotation";

                //encode white space
                string jobDesc = System.Web.HttpUtility.UrlPathEncode(job.Description);

                //mail content for client inquiries
                var services = "";
                var destinations = "";

                var Itineraries = db.JobItineraries.Where(d => d.JobMainId == jobId).ToList();

                var JobEncoder = db.JobTrails.Where(s => s.RefTable == "joborder" && s.RefId == jobId.ToString()).FirstOrDefault();

                //get date today
                DateClass dt = new DateClass();
                DateTime today =  dt.GetCurrentDate();
                string dateValid = today.AddMonths(1).ToString("MMM dd yyyy");
                decimal totalAmount = 0;

                foreach (var service in js)
                {
                    var remarks = service.Remarks != null ? "Remarks: " + service.Remarks + " </td>" : "";
                    var item = service.SupplierItem.Description != "Default" ? "Item: " + service.SupplierItem.Description + " <br/>" : "";

                    services +=
                     "<tr><td> <b>" + service.Service.Name + "</b> <br/>"
                    + "Start: "+ service.DtStart.Value.ToString("MMM dd yyyy (ddd)") + " <br/>"
                    + "End: " + service.DtEnd.Value.ToString("MMM dd yyyy (ddd)") + " </td>"
                    + "<td> "+ service.Particulars +" <br/>"
                    + item
                    + remarks
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


                var jNotes = db.JobNotes.Where(s => s.JobMainId == jobId).ToList();
                //get job notes
                string sNotes = "";
                foreach (var notetmp in jNotes)
                {
                    string s = notetmp.Note;

                    if (sNotes.Trim() != "") { sNotes += "<br>"; }

                    else { sNotes += "Terms & Conditions:<br>"; }

                    sNotes += " " + notetmp.Note;
                }

                var jobname = job.Description.Trim() == job.Customer.Name.Trim() ? job.Customer.Name.Trim() : job.Description + " / " + job.Customer.Name;
                var jobpax = job.NoOfPax != 0 ? " Pax : " + job.NoOfPax.ToString() + " <br/>" : "";
                var jobdays = job.NoOfDays != 0 ? " Days : " + job.NoOfDays.ToString() + " <br/>" : "";
                var jobremarks = job.JobRemarks != null ? " Remarks : " + job.JobRemarks + " <br/>" : "";

                body =
                    "" +
                    " <div style='background-color:#f4f4f4;padding:30px' >" +

                     "<img src='" + getHeader(job.Branch.Name) + "' width='100%' />"
                    + "<div style='background-color:white;padding:30px'>"
                    + "<h1> Quotation </h1><div>"

                    //details
                    + " Job Ref # : " + job.Id + " <br/>"
                    + " Date : " + job.JobDate.ToString("MMM dd yyyy (ddd)") + " <br/>"
                    + " Details : " + getCustCompany(jobId) + "<br/>"
                    + "           " + jobname + "<br/>";

                body += jobpax;

                body += jobdays;

                body += jobremarks + "</div>";

                //services
                body += "<div><h2> Services </h2>"
                     + "<table width='100%'  style='text-alight:left;'><tbody><tr><th>Type</th>"
                     + "<th> Pacticulars </th><th> Amount </th></tr>"
                     + services
                     + "</tbody></table>";

                    //destination
                body += "<br/><p> " + destinations + " </p></div>";

                //Summary
                body += "<div width='100%' style='background-color:#f1f1f1;padding:30px;margin-top:30px;margin-botom:20px;'>"
                     + "<h2> Summary </h2><h2> Total Due: " + Convert.ToDecimal(totalAmount).ToString("#,##0.00") + " </h2></div>"
                     + "<p>" + sNotes + " <p><br>"

                     + "<p></p><div><p> Prepared by: <b>" + getStaffName(JobEncoder.user) + "</b> </p>"
                     + "<p> Validity: <b>" + dateValid + "</b></p></div><hr>";
                    //footer
               body += " <p style='text-align:center;'> This is an auto-generated email. DO NOT REPLY TO THIS MESSAGE. </p> " +
                    " <p style='text-align:center;'> For further inquiries kindly email us through " + getcallBackEmail(job.Branch.Name) + " or dial(+63) 082 333 5157. </p> " +
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
         * Send email to client of Reservation Details
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

                string body;
                //get job details

                //send email in /joborder
                JobMain job = db.JobMains.Find(jobId);

                string company = getCompany(jobId);

                //mail title
                md.Subject = job.Description + " : " + company + " Reservation Details";

                //encode white space
                string jobDesc = System.Web.HttpUtility.UrlPathEncode(job.Description);

                //mail content for client inquiries
                var services = "";
                //var destinations = "";
                
                //get date today
                DateClass dt = new DateClass();
                DateTime today = dt.GetCurrentDate();
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

                        if (item.InvItem.ContactInfo != "")
                        {
                            jobitems += item.InvItem.ContactInfo;
                        }
                    }

                    var pickup = service.JobServicePickups.Where(s => s.JobServicesId == service.Id).FirstOrDefault();
                    var remarks = service.Remarks != null ? "Remarks: " + service.Remarks + " <br/>" : "";
                    var supitem = service.SupplierItem.Description != "Default" ? "Item: " + service.SupplierItem.Description + " " : "";

                    services +=
                    //date time
                     "<tr style='border-bottom:1px solid black;margin-bottom:15px;vertical-align:text-top;'><td style='padding-left:10px;'><b>" + service.Service.Name + "</b><br/>"
                    + "Start: " + service.DtStart.Value.ToString("MMM dd yyyy (ddd)") + " <br/>"
                    + "End: " + service.DtEnd.Value.ToString("MMM dd yyyy (ddd)") + " </td>"
                               
                    //particulars
                    + "<td style='padding-left:10px;'><b> P " + Convert.ToDecimal(service.ActualAmt).ToString("#,##0.00") + " - " + service.Particulars + "</b><br/>"
                    + remarks
                    + supitem + "</td>";
                               
                    if (pickup != null) { 
                        services += "<b> Pickup " + pickup.JsTime + " " + pickup.JsLocation.ToString() + "<br/>"
                        + "Contact " + pickup.ClientName + " " + pickup.ClientContact + "</b> ";
                    }

                    services += " </td>";
                    // Assigned items
                    services += "<td style='padding-left:10px;'> " + jobitems + " </td></tr>";

                    totalAmount += (decimal)service.ActualAmt;

                    services += "<tr><td style='border-bottom:1px solid black;padding:10px;' colspan='3'>";
                    //get job destinations
                    var Itineraries = db.JobItineraries.Where(d => d.SvcId == service.Id).ToList();
                    if (Itineraries != null)
                    {
                        services += "Destinations: ";
                    }

                    foreach (var dest in Itineraries)
                    {
                        services += dest.Destination.Description + ", ";
                    }
                    services += "</td></tr>";

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

                var jNotes = db.JobNotes.Where(s => s.JobMainId == jobId).ToList();
                //get job notes
                string sNotes = "";
                foreach (var notetmp in jNotes)
                {
                    string s = notetmp.Note;

                    if (sNotes.Trim() != "") { sNotes += "<br>"; }

                    else { sNotes += "Terms & Conditions:<br>"; }

                    sNotes += " " + notetmp.Note;
                }

                //get balance
                decimal balance =  totalAmount - partial;

                var jobname = job.Description.Trim() == job.Customer.Name.Trim() ? job.Customer.Name.Trim() : job.Description + " / " + job.Customer.Name;
                var jobpax = job.NoOfPax != 0 ? " Pax : " + job.NoOfPax.ToString() + " <br/>" : "";
                var jobdays = job.NoOfDays != 0 ? " Days : " + job.NoOfDays.ToString() + " <br/>" : "";
                var jobremarks = job.JobRemarks != null ? " Remarks : " + job.JobRemarks + " <br/>" : "";

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
                    + " Account : " + getCustCompany(jobId) + "<br/>"
                    + "           " + jobname + "<br/>";

                    if (job.NoOfPax != 0) {
                       body += " Pax : " + job.NoOfPax + " | ";
                    }

                body += jobdays
                    + jobremarks
                    + " Status : " + job.JobStatus.Status + " <br/>"
                    + "</div>"

                    //Services
                    + "<div><h2> Services </h2>"
                    + "<table class='table' width='100%' style='text-alight:left;vertical-align:text-top;'><tbody><tr><th style='text-alight:left;'>Type</th>"
                    + "<th style='text-alight:left;'> Pacticulars </th><th style='text-alight:left;'> Assigned </th></tr>"
                    + services
                    + "</tbody></table>"

                    //destinations
                    //+ "<br/><p><b>" + destinations + "</b></p></div>"

                    //Payments
                    + "<h2>Payments</h2>"
                    + "<table width='50%'>"
                    + "<tr><td><b>Amount</b></td><td><b>Particulars</b></td></tr>"
                    + paymentTxt
                    + "</table>";

                    //Summary
                body += "<div width='100%' style='background-color:#f1f1f1;padding:30px;margin-top:30px;margin-botom:20px;'>"
                    + "<h2> Summary </h2><h3> Total Due: " + Convert.ToDecimal(totalAmount).ToString("#,##0.00")  + " </h3>"
                    + "<h3> Partial: "+ Convert.ToDecimal(partial).ToString("#,##0.00")  + "</h3>"
                    + "<h3> Balance: " + Convert.ToDecimal(balance).ToString("#,##0.00")  + "</h3></div>"
                    + "<br><br><br>"
                    + "<p></p><div>"
                    + "<p>" + sNotes + " <p><br>"
                    + "<p> Prepared by: <b>" + getStaffName(JobEncoder.user) + "</b> </p>"
                    + "<p> Validity: <b>" + dateValid + "</b></p></div>"

                    + "<br><br><br><hr>"
                    //Footer
                    + " <p style='text-align:center;'> This is an auto-generated email. DO NOT REPLY TO THIS MESSAGE. </p> " +
                    " <p style='text-align:center;'> For further inquiries kindly email us through " + getcallBackEmail(job.Branch.Name) + " or dial(+63) 082 333 5157. </p> " +
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
         * CLIENT - SEND EMAIL FOR ONLINE RESERVATION PAYMENT SUCCESS
         * Send email to client after payment reservation success
         **/ 
        public string SendMailOnlineReserve(int reservationId, string email , string emailType, string svcType)
        {
            var reservation = db.OnlineReservations.Find(reservationId);
            var product = pdb.SmProducts.Where(s => s.Code == reservation.ProductCode).FirstOrDefault();

            //buld email subject / title
            string subject = " Online Reservation For " + product.Name;

            if (emailType == "ADMIN")
            {
                subject = reservation.Name + " : Online Reservation Payment For " + reservation.ProductCode;
            }
            
            //build email body
            string message = "<p> Your reservation is being processed, please wait for our agents to contact you via call or email.</p>";
            string title = " <h1> Thank you for availing our " + product.Name + "</h1> ";

            if (svcType == "CAR")
            {
                message += "<div style='margin:10px auto;text-align:left;padding-left:200px;background-color:white;width:400px;min-width:160px;'> " +
                            "<h2> Reservation Details </h2><span style='font-size:15px;'>" +
                            "<b> Product : </b> " + product.Name + "<br />" +
                            "<b> Product Code: </b> " + reservation.ProductCode + "<br />" +
                            "<b> Date: </b> " + reservation.DtStart.ToString("MMM dd yyyy (ddd)") + "<br />" +
                            "<b> Name: </b> " + reservation.Name + "<br />" +
                            "<b> Contact No.: </b> " + reservation.ContactNum + "<br />" +
                            "<b> Email: </b> " + reservation.Email + "<br />" +
                            "<b> Pickup: </b>  " + reservation.PickupDtls + "<br />" +
                            "<b> No. Days: </b>  " + reservation.Qty + "<br />" +
                            "<b> Amount: </b> P " + reservation.PaymentAmt + "<br />" +
                            "<b> Payment method: </b> Paypal <br />" +
                            "</span></div>";
            }
            else
            {   //TOUR DEFAULT
                message += "<div style='margin:10px auto;text-align:left;padding-left:200px;background-color:white;width:400px;min-width:160px;'> " +
                            "<h2> Reservation Details </h2><span style='font-size:15px;'>" +
                            "<b> Product : </b> " + product.Name + "<br />" +
                            "<b> Code: </b> " + reservation.ProductCode + "<br />" +
                            "<b> Date: </b> " + reservation.DtStart.ToString("MMM dd yyyy (ddd)") + "<br />" +
                            "<b> Name: </b> " + reservation.Name + "<br />" +
                            "<b> Contact No.: </b> " + reservation.ContactNum + "<br />" +
                            "<b> Email: </b> " + reservation.Email + "<br />" +
                            "<b> Pickup: </b>  " + reservation.PickupDtls + "<br />" +
                            "<b> No. Pax: </b>  " + reservation.Qty + "<br />" +
                            "<b> Amount: </b> P " + reservation.PaymentAmt + "<br />" +
                            "<b> Payment method: </b> Paypal <br />" +
                            "</span></div>";
            }

            //FOR ADMIN , Add button to view details
            if (emailType == "ADMIN")
            {
                message += "<div style='text-align:center;padding-left:220px;'><a href='https://realwheelsdavao.com/OnlineReservations/Details/" + reservationId +"' >" +
                    "<div style='background-color: dodgerblue; width: 120px; padding: 10px; color: white;text-align:center;'> " +
                    " View Details "+
                    "</div></a></div>";
                title = " <h1> Online Reservation : Payment Success </h1> ";
            }
            
            string body = "" +
                " <div style='background-color:#f4f4f4;padding:20px' align='center'>" +
                " <div style='background-color:white;min-width:200px;width:600px;;margin:30px;padding:30px;text-align:center;color:#555555;font:normal 300 16px/21px 'Helvetica Neue',Arial'>"+
                //" <img src='http://realbreezedavaotours.com/wp-content/uploads/2019/07/Realbreeze_logo.png' width='170px' >" +
                title +
                message +
                " <p> This is an auto-generated email. DO NOT REPLY TO THIS MESSAGE </p> " +
                " <p> For further inquiries kindly email us through inquiries.realwheels@gmail.com or dial(+63) 082 333 5157. </p> " +
                " </div></div>" ;

            return Email(body, email, subject);
        }

        /**
         * CLIENT - SEND EMAIL FOR ONLINE RESERVATION PAYMENT SUCCESS
         * Send email to client after payment reservation success
         **/
        public string SendMailOnlineInquire(int reservationId, string email, string emailType, string svcType)
        {
            var reservation = db.OnlineReservations.Find(reservationId);
            var product = pdb.SmProducts.Where(s => s.Code == reservation.ProductCode).FirstOrDefault();

            //buld email subject / title
            string subject = " Online Inquiry For " + product.Name;

            if (emailType == "ADMIN")
            {
                subject = reservation.Name + " : Online Inquiry For " + reservation.ProductCode;
            }

            //build email body
            string message = "<p> Your Inquiry is being processed, please wait for our agents to contact you via call or email.</p>";
            string title = " <h1>  " + product.Name + "</h1> ";

            if (svcType == "CAR")
            {
                message += "<div style='margin:10px auto;text-align:left;padding-left:200px;background-color:white;width:400px;min-width:160px;'> " +
                            "<h2> Inquiry Details </h2><span style='font-size:15px;'>" +
                            "<b> Product : </b> " + product.Name + "<br />" +
                            "<b> Product Code: </b> " + reservation.ProductCode + "<br />" +
                            "<b> Date: </b> " + reservation.DtStart.ToString("MMM dd yyyy (ddd)") + "<br />" +
                            "<b> Name: </b> " + reservation.Name + "<br />" +
                            "<b> Contact No.: </b> " + reservation.ContactNum + "<br />" +
                            "<b> Email: </b> " + reservation.Email + "<br />" +
                            "<b> Pickup: </b>  " + reservation.PickupDtls + "<br />" +
                            "<b> No. Days: </b>  " + reservation.Qty + "<br />" +
                            "</span></div>";
            }
            else
            {   //TOUR DEFAULT
                message += "<div style='margin:10px auto;text-align:left;padding-left:200px;background-color:white;width:400px;min-width:160px;'> " +
                            "<h2> Reservation Details </h2><span style='font-size:15px;'>" +
                            "<b> Product : </b> " + product.Name + "<br />" +
                            "<b> Code: </b> " + reservation.ProductCode + "<br />" +
                            "<b> Date: </b> " + reservation.DtStart.ToString("MMM dd yyyy (ddd)") + "<br />" +
                            "<b> Name: </b> " + reservation.Name + "<br />" +
                            "<b> Contact No.: </b> " + reservation.ContactNum + "<br />" +
                            "<b> Email: </b> " + reservation.Email + "<br />" +
                            "<b> Pickup: </b>  " + reservation.PickupDtls + "<br />" +
                            "<b> No. Pax: </b>  " + reservation.Qty + "<br />" +
                            "</span></div>";
            }

            //FOR ADMIN , Add button to view details
            if (emailType == "ADMIN")
            {
                message += "<div style='text-align:center;padding-left:220px;'><a href='https://realwheelsdavao.com/OnlineReservations/Details/" + reservationId + "' >" +
                    "<div style='background-color: dodgerblue; width: 120px; padding: 10px; color: white;text-align:center;'> " +
                    " View Details " +
                    "</div></a></div>";
                title = " <h1> Online Inquiry </h1> ";
            }

            string body = "" +
                " <div style='background-color:#f4f4f4;padding:20px' align='center'>" +
                " <div style='background-color:white;min-width:200px;width:600px;;margin:30px;padding:30px;text-align:center;color:#555555;font:normal 300 16px/21px 'Helvetica Neue',Arial'>" +
                //" <img src='http://realbreezedavaotours.com/wp-content/uploads/2019/07/Realbreeze_logo.png' width='170px' >" +
                title +
                message +
                " <p> This is an auto-generated email. DO NOT REPLY TO THIS MESSAGE </p> " +
                " <p> For further inquiries kindly email us through inquiries.realwheels@gmail.com or dial(+63) 082 333 5157. </p> " +
                " </div></div>";

            return Email(body, email, subject);
        }

        /**
         * CLIENT - SEND EMAIL FOR ONLINE RESERVATION PAYMENT SUCCESS
         * Send email to client after payment reservation success
         **/
        public string SendMailRsvRequest(int reservationId, string email, string emailType, string recipientName, string svcType)
        {
            try
            {
                var reservation = db.CarReservations.Find(reservationId);
                var CarRespkg = db.CarResPackages;
                //var product = pdb.SmProducts.Where(s => s.Code == reservation.ProductCode).FirstOrDefault();

                //buld email subject / title
                string subject = reservation.CarResType.Type + " Request ";

                if (emailType == "ADMIN")
                {
                    subject = reservation.CarResType.Type + " Request by " + recipientName;
                }

                var isSelfDrive = "Self Drive";
                if (reservation.SelfDrive == 0)
                {
                    isSelfDrive = "With Driver";
                }

                //build email body
                string message = "<p> Your " + reservation.CarResType.Type + " Request is being processed, our agents to contact you via call or email.</p>";
                string title = " <h1> " + reservation.CarResType.Type + " Request </h1> ";

                if (svcType == "CAR")
                {
                    message += "<div style='margin:10px auto;text-align:left;padding-left:200px;background-color:white;width:400px;min-width:160px;'> " +
                                "<h2> Request Details </h2><span style='font-size:15px;'>" +
                                "<b> Unit Type : </b> " + reservation.CarUnit.Description + "<br />" +
                                "<b> Rate: </b> " + reservation.BaseRate + "<br />" +
                                "<b> Date Start: </b> " + reservation.DtStart + "<br />" +
                                "<b> Date End: </b> " + reservation.DtEnd + "<br />" +
                                "<b> No of Days: </b> " + reservation.NoDays + "<br />" +
                                "<b> Pickup: </b> " + reservation.LocStart + "<br />" +
                                "<b> DropOff: </b> " + reservation.LocEnd + "<br />" +
                                "<b> Destination: </b> " + reservation.Destinations + "<br />" +
                                "<b> Rental Type: </b> " + isSelfDrive + "<br /><br />" +

                                "<b> Name: </b> " + reservation.RenterName + "<br />" +
                                "<b> Company: </b>  " + reservation.RenterCompany + "<br />" +
                                "<b> Contact No.: </b> " + reservation.RenterMobile + "<br />" +
                                "<b> Email: </b> " + reservation.RenterEmail + "<br />" +
                                "<b> Address: </b>  " + reservation.RenterAddress + "<br />" +
                                "<b> FB: </b>  " + reservation.RenterFbAccnt + "<br />" +
                                "<b> Linkedln: </b>  " + reservation.RenterLinkedInAccnt + "<br />" +
                                "</span></div>";
                }
                else
                {   //TOUR DEFAULT
                    message += "<div style='margin:10px auto;text-align:left;padding-left:200px;background-color:white;width:400px;min-width:160px;'> " +
                                "<h2> Reservation Details </h2><span style='font-size:15px;'>" +
                                //"<b> Product : </b> " + product.Name + "<br />" +
                                //"<b> Code: </b> " + reservation.ProductCode + "<br />" +
                                //"<b> Date: </b> " + reservation.DtStart.ToString("MMM dd yyyy (ddd)") + "<br />" +
                                //"<b> Name: </b> " + reservation.Name + "<br />" +
                                //"<b> Contact No.: </b> " + reservation.ContactNum + "<br />" +
                                //"<b> Email: </b> " + reservation.Email + "<br />" +
                                //"<b> Pickup: </b>  " + reservation.PickupDtls + "<br />" +
                                //"<b> No. Pax: </b>  " + reservation.Qty + "<br />" +
                                "</span></div>";
                }

                //FOR ADMIN , Add button to view details
                if (emailType == "ADMIN")
                {
                    message += "<div style='text-align:center;padding-left:220px;'><a href='https://realwheelsdavao.com/OnlineReservations/Details/" + reservationId + "' >" +
                        "<div style='background-color: dodgerblue; width: 120px; padding: 10px; color: white;text-align:center;'> " +
                        " View Details " +
                        "</div></a></div>";
                    title = "<h1> " + reservation.CarResType.Type + " : " + reservation.CarUnit.Description + "</h1> <h3>  ADMIN COPY </h3>";
                }

                string body = "" +
                    " <div style='background-color:#f4f4f4;padding:20px' align='center'>" +
                    " <div style='background-color:white;min-width:200px;width:600px;;margin:30px;padding:30px;text-align:center;color:#555555;font:normal 300 16px/21px 'Helvetica Neue',Arial'>" +
                    //" <img src='http://realbreezedavaotours.com/wp-content/uploads/2019/07/Realbreeze_logo.png' width='170px' >" +
                    " <h3 align='center'> Realwheels Davao </h3>  " +
                    title +
                    message +
                    " <p> This is an auto-generated email. DO NOT REPLY TO THIS MESSAGE </p> " +
                    " <p> For further inquiries kindly email us through inquiries.realwheels@gmail.com or dial(+63) 082 333 5157. </p> " +
                    " <p style='text-align:center;'> www.realwheelsdavao.com </p>" +
                    " </div></div>";

                return Email(body, email, subject);
            }
            catch (Exception ex)
            {
                return "Error " + ex.Message;
            }
        }

        private string Email(string emailBody, string recipientEmail, string emailSubject )
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("mail.realwheelsdavao.com"); //smtp server

                MailDefinition md = new MailDefinition();
                md.From = "Realwheels.Reservation@RealWheelsDavao.com";      //sender mail
                md.IsBodyHtml = true;                       //set true to enable use of html tags 
                md.Subject = "RealWheels Reservation";      //initial mail title

                ListDictionary replacements = new ListDictionary();
                replacements.Add("{name}", "Reservation");

                //change mail title
                md.Subject = emailSubject;

                //encode white space
                //string jobDesc = System.Web.HttpUtility.UrlPathEncode(job.Description);
                
                MailMessage msg = md.CreateMailMessage(recipientEmail, replacements, emailBody, new System.Web.UI.Control());

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
                return "error: " + ex.Message;
            }
        }
        
        /**
         * FILTER AND RETURN STAFF NAME BY EMAIL
         */
        public string getStaffName(string staffLogin)
        {
            switch (staffLogin)
            {
                case "josette.realbreeze@gmail.com":
                    return "Josette Valleser";
                case "mae.realbreeze@gmail.com":
                    return "Cristel Mae Verano";
                case "ramil.realbreeze@gmail.com":
                    return "Ramil Villahermosa";
                case "grace.realbreeze@gmail.com":
                    return "Grace-chell V. Capandac";
                case "assalvatierra@gmail.com":
                    return "Elvie S. Salvatierra ";
                case "jecca.realbreeze@gmail.com":
                    return "Jecca Bilason";
                default:
                    return "Elvie S. Salvatierra ";
            }
        }

        /**
         *  RETURN EMAIL BASED ON JOB BRANCH NAME
         **/ 
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

        /**
         *  RETURN EMAIL HEADER IMAGE LINK BASED ON JOB BRANCH NAME
         **/ 
        public string getHeader(string branch)
        {
            switch (branch)
            {
                case "AJ88":
                    return "http://realbreezedavaotours.com/wp-content/uploads/2019/10/AJDavao-Header.jpg";
                case "RealBreeze":
                    return "http://realbreezedavaotours.com/wp-content/uploads/2019/10/Realbreeze-Header.jpg";
                case "RealWheels":
                    return "http://realbreezedavaotours.com/wp-content/uploads/2019/10/RealWheels-Header.jpg";
                default:
                    return "http://realbreezedavaotours.com/wp-content/uploads/2019/10/AJDavao-Header.jpg";
            }
        }
        
        /**
         *  RETURN COMPANY NAME BASED ON JOB ID
         **/
        public string getCompany(int jobId)
        {
            JobMain job = db.JobMains.Find(jobId);

            string company = "RealBreeze";

            if (job.Branch.Name == "RealBreeze")
            {
                company = "RealBreeze";
            }

            switch (job.Branch.Name)
            {
                case "Realbreeze":
                    company = "RealBreeze";
                    break;

                case "Realwheels":
                    company = "Realwheels";
                    break;
                case "AJ88":
                    company = "AJ88";
                    break;
                default:
                    company = "RealBreeze";
                    break;

            }

            return company;
        }

        /**
         *  RETURN COMPANY NAME OF THE JOB 
         **/
        public string getCustCompany(int id)
        {
            var jobMain = db.JobMains.Find(id);
            string custCompany = "";
            //check customer if assigned to a company
            if (jobMain.JobEntMains.Where(c => c.JobMainId == jobMain.Id).FirstOrDefault() != null)
            {
                var company = jobMain.JobEntMains.Where(c => c.JobMainId == jobMain.Id).FirstOrDefault().CustEntMain;

                //hide company name if company is 1 = New (not defined)
                if (company.Id == 1)
                {
                    custCompany = " ";
                }
                else
                {
                    custCompany = jobMain.JobEntMains.Where(c => c.JobMainId == jobMain.Id).FirstOrDefault().CustEntMain.Name;
                }
            }
            return custCompany;
        }
    }
}