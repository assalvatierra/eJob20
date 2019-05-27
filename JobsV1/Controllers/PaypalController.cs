using Newtonsoft.Json.Linq;
using PayPal.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using JobsV1.Models;
using System.Web.Http.Cors;

namespace JobsV1.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class PaypalController : Controller
    {
        DBClasses DB = new DBClasses();
        PayPalHandler PPtrans = new PayPalHandler();

        JobDBContainer db = new JobDBContainer();

        // GET: Paypal
        public ActionResult Index()
        {
            return View();
        }


        /* Recieve Webhook events from paypal when a transaction is made.
         * Transactions events trigger on payments. 
         * Transaction status can be PENDING, COMPLETED or DENIED.
         * This will recieve the event and filter the event based on the
         * status. On payment complete, payment of the job (acquired through jobid from 'custom' field)
         * is updated. 
         * Events will be recorded in the PaypalTransactions table.
         */ 

        [System.Web.Http.HttpPost]
        public ActionResult Webhook()
        {
            // The APIContext object can contain an optional override for the trusted certificate.
            var apiContext = PayPalConfiguration.GetAPIContext();

            // Get the received request's headers
            var requestheaders = HttpContext.Request.Headers;

            // Get the received request's body
            var requestBody = string.Empty;
            using (var reader = new System.IO.StreamReader(HttpContext.Request.InputStream))
            {
                requestBody = reader.ReadToEnd();
            }

            //get event data from the request body
            dynamic jsonBody = JObject.Parse(requestBody);
            string webhookId = jsonBody.id;
            string paypalID = jsonBody.resource.id;
            decimal Totalamount = (decimal)jsonBody.resource.amount.total;
            DateTime paypalEventDate = (DateTime)jsonBody.create_time;
            DateTime paypalTransDate = (DateTime)jsonBody.resource.create_time;

            var ev = WebhookEvent.Get(apiContext, webhookId);

            // We have all the information the SDK needs, so perform the validation.
            // Note: at least on Sandbox environment this returns false.
            // var isValid = WebhookEvent.ValidateReceivedEvent(apiContext, ToNameValueCollection(requestheaders), requestBody, webhookId);

            //add record on the Notification table
            //Note: remove if events are successful. Notification table is used for sms notifications.
            DB.addTestNotification(1, "1");
           
            //get jobid from CUSTOM object field
            //int jobId =  (int)jsonBody.resource.custom ; // bookingid , 
            //PPtrans.AddPaypalNotif("1", 1, paypalEventDate, paypalTransDate, ev.event_type, Totalamount);

            //get job id from invoice number
            //PPtrans.AddPaypalNotif(paypalID, jobId, paypalEventDate, paypalTransDate, ev.event_type, Totalamount);
            //jobId = (int)jsonBody.resource.invoice_number;
            // DB.addTestNotification(jobId, paypalID);

            //get job description
            JobMain jobOrder = db.JobMains.Find(1);
            string clientName = jobOrder.Description;
            EMailHandler mail = new EMailHandler();
            string siteRedirect = "https://realwheelsdavao.com/reservation/";
            
            /* Handle transaction request from paypal webhook events 
            switch (ev.event_type)
            {
                case "PAYMENT.CAPTURE.COMPLETED":
                case "PAYMENT.SALE.COMPLETED": // Handle payment completed
                    //record payment
                    AddPaymentRecord(jobId, Totalamount);

                    //send mail
                    mail.SendMail(jobId, "reservation.realwheels@gmail.com", "PAYMENT-SUCCESS", clientName, siteRedirect);
                    //mail.SendMail(jobId, "ajdavao88@gmail.com", "PAYMENT-SUCCESS", clientName, siteRedirect);
                    //mail.SendMail(jobId, "travel.realbreeze@gmail.com", "PAYMENT-SUCCESS", clientName, siteRedirect);

                    //add to log
                    PPtrans.AddPaypalNotif(paypalID, jobId, paypalEventDate, paypalTransDate, ev.event_type, Totalamount);
                    break;
                case "PAYMENT.SALE.DENIED":
                case "PAYMENT.CAPTURE.DENIED": // Handle payment denied

                    //send mail
                    mail.SendMail(jobId, "reservation.realwheels@gmail.com", "PAYMENT-DENIED", clientName, siteRedirect);
                    //mail.SendMail(jobId, "ajdavao88@gmail.com", "PAYMENT-DENIED", clientName, siteRedirect);
                    //mail.SendMail(jobId, "travel.realbreeze@gmail.com", "PAYMENT-DENIED", clientName, siteRedirect);

                    //add to log
                    PPtrans.AddPaypalNotif(paypalID, jobId, paypalEventDate, paypalTransDate, ev.event_type, Totalamount);

                    break;
                // Handle other webhooks
                default: // Handle payment denied
                    //send mail
                    mail.SendMail(jobId, "reservation.realwheels@gmail.com", "PAYMENT-PENDING", clientName, siteRedirect);
                    //mail.SendMail2(jobId, "reservation.realwheels@gmail.com", "PAYMENT-PENDING", clientName, siteRedirect, ev.event_type);
                    //mail.SendMail(jobId, "ajdavao88@gmail.com", "PAYMENT-PENDING", clientName, siteRedirect);
                    //mail.SendMail(jobId, "travel.realbreeze@gmail.com", "PAYMENT-PENDING", clientName, siteRedirect);

                    //add to log
                    PPtrans.AddPaypalNotif(paypalID, jobId, paypalEventDate, paypalTransDate, ev.event_type, Totalamount);

                    break;
            }
            */

            //AddPaymentRecord(jobId, Totalamount);
            //add to log
            //PPtrans.AddPaypalNotif(paypalID, jobId, paypalEventDate, paypalTransDate, ev.event_type, Totalamount);

            //send mail
            //mail.SendMail(1, "reservation.realwheels@gmail.com", "PAYMENT-PENDING", clientName, siteRedirect);
            //mail.SendMail2(jobId, "reservation.realwheels@gmail.com", "PAYMENT-PENDING", clientName, siteRedirect, ev.event_type);
            //mail.SendMail(jobId, "ajdavao88@gmail.com", "PAYMENT-PENDING", clientName, siteRedirect);
            //mail.SendMail(jobId, "travel.realbreeze@gmail.com", "PAYMENT-PENDING", clientName, siteRedirect);

            return new HttpStatusCodeResult(200);
        }

        public static class PayPalConfiguration
        {
            public readonly static string ClientId;
            public readonly static string ClientSecret;

            // Static constructor for setting the readonly static members.
            static PayPalConfiguration()
            {
                var config = GetConfig();
                ClientId = config["clientId"];
                ClientSecret = config["clientSecret"];
            }

            // Create the configuration map that contains mode and other optional configuration details.
            public static Dictionary<string, string> GetConfig()
            {
                // ConfigManager.Instance.GetProperties(); // it doesn't work on ASPNET 5 
                // live
                /**/
                return new Dictionary<string, string>() {
                    { "clientId", "AVoJltP7TidcyKYb7qNwmX2S1xyd-3eFClTQOcA77WGTBky_IJtgccVAA-6C8D8EGGM3-DzjUye1jKzQ" },
                    { "clientSecret", "EI_D_W8aJRWqEg5jC6bzygsi8X44lxvfd1pwGazPp62UhbGRO65OudfsIGhXxzzcsyllIjt69gXuNi7f" }
                };
                
                /* sandbox
                return new Dictionary<string, string>() {
                    { "clientId", "AUvsEZNhW0bZQSYuzVDgNxePk5lrSEAoF4rQQHXLIByzeBd6N4-vjtLWGviKaFeVMu9U-GD_99nwCz29" },
                    { "clientSecret", "EO7kEQ47mhxybEJkYr9H4tShohBvpw-Xf1PIEOOmeiz10wfomjX4udWw6j7IPSDtH-6ec28ok0cNGrG6" }
                };
                
              
                // jahdiel test paypal
                // sandbox
                return new Dictionary<string, string>() {
                    { "clientId",     "AeKvfmAZjDaTJ4bH4PFGurLMvFZOl9OeHaK6xUlSCB0Ny8RU2WEeijZLTeRGvz0GjQXrX1SuaYvf53-H" },
                    { "clientSecret", "EASK4ghccZuqU3VDsEwA9WzEbNWqqtWPJQWXkd1UAcKflTQ1CX1dAvj2ZyKcE_nILs2ewK0rQkJ85hAX" }
                };
                  */
            }

            // Create accessToken
            private static string GetAccessToken()
            {
                // ###AccessToken
                // Retrieve the access token from OAuthTokenCredential by passing in
                // ClientID and ClientSecret
                // It is not mandatory to generate Access Token on a per call basis.
                // Typically the access token can be generated once and reused within the expiry window                
                string accessToken = new OAuthTokenCredential
                    (ClientId, ClientSecret, GetConfig()).GetAccessToken();
                return accessToken;
            }

            // Returns APIContext object
            public static APIContext GetAPIContext(string accessToken = "")
            {
                // Pass in a `APIContext` object to authenticate 
                // the call and to send a unique request id 
                // (that ensures idempotency). The SDK generates
                // a request id if you do not pass one explicitly. 
                var apiContext = new APIContext(string.IsNullOrEmpty(accessToken) ?
                    GetAccessToken() : accessToken);
                apiContext.Config = GetConfig();

                return apiContext;
            }
        }

        //add paypal payment to job payment (full/partial) 
        private void AddPaymentRecord(int JobMainId, decimal amount ) {

            DateTime today = DateTime.Now;
            today = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(today, TimeZoneInfo.Local.Id, "Singapore Standard Time");

            string remarks = "PayPal Payment";
            JobPayment jobPayment = new JobPayment();
            jobPayment.BankId = 5;                      //personal guarantee, need to add (5) paypal
            jobPayment.DtPayment = today;
            jobPayment.JobMainId = (int)JobMainId;
            jobPayment.PaymentAmt = amount;
            jobPayment.Remarks = remarks;

            db.JobPayments.Add(jobPayment);
            db.SaveChanges();
        }
    }
}