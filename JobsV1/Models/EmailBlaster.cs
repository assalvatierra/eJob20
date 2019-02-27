using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Web.UI.WebControls;
using System.Collections.Specialized;

namespace JobsV1.Models
{
    public class EmailBlaster
    {

        private JobDBContainer db = new JobDBContainer();

        //Send Email to the [renterMail] one at a time,
        //[emailSubject] as email title, 
        //[emailContent] as email body,
        //[emalPicture] as Picture Link to <img>,
        //[emailAttachmentLink] as Attachment link to <a>
        public string SendMailBlaster( string renterMail, string emailSubject, string emailContent, string emailPicture,string emailAttachmentLink, string company)
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("mail.realwheelsdavao.com"); //smtp server

                MailDefinition md = new MailDefinition();
                md.From = "Realwheels.Reservation@RealWheelsDavao.com";      //sender mail
                md.IsBodyHtml = true;           //set true to enable use of html tags 
                md.Subject = emailSubject;      //mail title

                ListDictionary replacements = new ListDictionary();
                replacements.Add("{name}", "Martin");

                string body, message;

                //mail content for client inquiries
                message = emailContent;

                //build email body
                body =   
                    "" +  
                    " <div style='background-color:#f4f4f4;padding:20px' align='center'>" +
                    " <div style='background-color:white;min-width:250px;margin:20px;padding:0px;text-align:center;color:#555555;font:normal 300 16px/21px 'Helvetica Neue',Arial;'> " +
                     handleCompany(company) +
                    " <h1 style='text-align:center;padding-bottom:20px;margin-top:-60px;'> " + emailSubject + " </h1>" +
                    handlePictureLink(emailPicture) +
                    " <div style='text-align:left;padding:20px;'><h3>" +
                    message +
                    " </h3></div>" +
                    handleAttachLink(emailAttachmentLink) +
                    " <br />" +
                    " </div>" +
                    " <div style='text-align:center;color:#626262;'>" +
                    " <p> This is an auto-generated email. DO NOT REPLY TO THIS MESSAGE </p> " +
                    " <p> For inquiries, kindly email us through realbreezedavao@gmail.com or dial(+63) 82 297 1831. </p> " +
                    " </div>" +
                    " </div>" +
                    "       " ;   
               
                MailMessage msg = md.CreateMailMessage(renterMail, replacements, body, new System.Web.UI.Control());
                //msg.Attachments.Add(new Attachment(emailPicture));

                SmtpServer.Port = 587;          //default smtp port
                SmtpServer.Credentials = new System.Net.NetworkCredential("admin@realwheelsdavao.com", "Real123!");
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

        //generate and return  of the attachment link html element and string
        //return none if no link is specified
        public string handleAttachLink(string link)
        {
            string attachLink = "";

            //link is not empty
            if (link != "" || link != null)
            {
                attachLink = " <h3 ><a href='" + link + "'  target='_blank' style='padding:15px;color:white;background-color: dodgerblue;text-decoration:none;' >Open Attachments</a></h3>";
            }

            //link is empty
            if (link == "" || link == null)
            {
                attachLink = " <h3 ></h3>";
            }

            return attachLink;
        }

        //generate html element  of the picture link and return string
        //return none if no link is specified
        public string handlePictureLink(string link)
        {
            string pictureLink = "";

            //link is not empty
            if (link != "" || link != null)
            {
                pictureLink = " <img src='" + link + "' style='width:100%'> <br />";
            }

            //link is empty
            if (link == "" || link == null)
            {
                pictureLink = " ";
            }

            return pictureLink;
        }


        //generate html element  of the picture link and return string
        //return none if no link is specified
        public string handleCompany(string company)
        {
            string pictureLink = "";

            //link is not empty
            if (company != "" || company != null)
            {
                switch (company)
                {
                    case "REALBREEZE":
                        pictureLink = " <img src='http://realbreezedavaotours.com/wp-content/uploads/2019/02/Realbreeze_logo_transparent.png' width='250' style='margin-bottom:0px;'> ";
                        break;
                    case "REALWHEELS":
                        pictureLink = " <img src='http://realbreezedavaotours.com/wp-content/uploads/2019/02/RealWheels.jpg' width='250' style='margin-top:20px;'> ";
                        break;
                    default:
                        pictureLink = " <img src='http://realbreezedavaotours.com/wp-content/uploads/2019/01/realbreeze_logo_2.jpg' width='250' style='margin-bottom:0px;'> ";
                        break;
                }
            }

            //link is empty
            if (company == "" || company == null)
            {
                pictureLink = " ";
            }

            return pictureLink;
        }
    }
}