using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Newtonsoft.Json;
using JobsV1.Models;
using System.Data;

namespace JobsV1
{
    /// <summary>
    /// Summary description for SMSWebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class SMSWebService : System.Web.Services.WebService
    {
        //database
        private JobDBContainer db = new JobDBContainer();

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }

        [WebMethod]
        public void GetNotification(int jobId)
        {

            string sData = "Pickup Details";

            Models.JobServicePickup svcpu;
            Models.JobServices svc = db.JobServices.Find(jobId);

            if (svc.JobServicePickups.FirstOrDefault() == null)
            {
                sData += "\nPickup: undefined ";
            }
            else
            {
                Decimal quote = (svc.QuotedAmt == null ? 0 : (decimal)svc.QuotedAmt);

                svcpu = svc.JobServicePickups.FirstOrDefault();
                sData += "\nDate:" + ((DateTime)svc.DtStart).ToString("dd MMM yyyy (ddd)");
                sData += "\nTime&Location:" + svcpu.JsTime + " " + svcpu.JsLocation;
                sData += "\nGuest:" + svcpu.ClientName + " #" + svcpu.ClientContact;
                sData += "\nDriver:" + svcpu.ProviderName + " #" + svcpu.ProviderContact;
                sData += "\nUnit:" + svc.SupplierItem.Description + " " + svc.SupplierItem.Remarks;
                sData += "\nRate:P" + quote.ToString("##,###.00");
                sData += "\nParticulars:" + svc.Particulars;
                sData += "\n  " + svc.Remarks;
                sData += "\n\nHave a safe trip,\nAJ88 Car Rental";

                //create table for the message
                DataTable Dt = new DataTable("Table");
                //columns for the table
                Dt.Columns.Add("Id", typeof(int));
                Dt.Columns.Add("DtSchedule", typeof(string));
                Dt.Columns.Add("Message", typeof(string));
                Dt.Columns.Add("Recipient", typeof(string));
                Dt.Columns.Add("RecType", typeof(string));

                //add for the client, driver, admin
                int Id = 0;
                string dtSchedule = ((DateTime)svc.DtStart).ToString("dd MMM yyyy");
                string msg = sData;
                string recipients = svcpu.ProviderContact;
                Dt.Rows.Add(Id, dtSchedule, msg, recipients);

                //driver
                Id = 1;
                //the same schedule
                //the same msg
                recipients = svcpu.ClientContact;
                Dt.Rows.Add(Id, dtSchedule, msg, recipients);

                //admin
                Id = 2;
                //the same schedule
                //the same msg
                recipients = "09279016517"; //admin contact
                Dt.Rows.Add(Id, dtSchedule, msg, recipients);
                
                DataSet ds = new DataSet();
                ds.Tables.Add(Dt);
                ds.DataSetName = "Table";

                Context.Response.Clear();
                Context.Response.ContentType = "application/json";
                Context.Response.Write(JsonConvert.SerializeObject(ds, Newtonsoft.Json.Formatting.Indented));

            }

        }

        [WebMethod]
        public string AddNotification(int jobServiceId)
        {
            Models.JobServicePickup svcpu;
            Models.JobServices svc = db.JobServices.Find(jobServiceId);

            svcpu = svc.JobServicePickups.FirstOrDefault();

            //add notification
            JobNotificationRequest jnr = new JobNotificationRequest();
            db.JobNotificationRequests.Add(new JobNotificationRequest
            {
                RefId = "0",    //job id
                ReqDt = DateTime.Parse(DateTime.Now.ToString("MMM dd yyyy HH:mm:ss")),
                ServiceId = jobServiceId
            });

            db.SaveChanges();
            return "Notification added to the list";

        }

        [WebMethod]
        public void CatRemove2(int Id)
        {
            InvItemCategory cat = db.InvItemCategories.Find(Id);
            db.InvItemCategories.Remove(cat);
            db.SaveChanges();

        }

        
        [WebMethod]
        public void reservationPkg(int reservationid, int pacakgeId, int mealAcc, int fuel)
        {
            CarResPackage packages = new CarResPackage();
            packages.CarRateUnitPackageId = pacakgeId;
            packages.CarReservationId = reservationid;
            packages.DrvMealByClient = mealAcc;
            packages.DrvRoomByClient = mealAcc;
            packages.FuelByClient = fuel;

            db.CarResPackages.Add(packages);
            db.SaveChanges();
        }

        [WebMethod]
        public void FormRenter( string DtTrx, string CarUnitId, string DtStart, string LocStart,
            string DtEnd, string LocEnd, string BaseRate, string Destinations, string UseFor,
            string RenterName, string RenterCompany, string RenterEmail, string RenterMobile,
            string RenterAddress, string RenterFbAccnt, string RenterLinkedInAccnt, string EstHrPerDay,
            string EstKmTravel)
        {
            CarReservation carReservation = new CarReservation();
            carReservation.DtTrx = DateTime.Parse( DtTrx);
            carReservation.CarUnitId = int.Parse( CarUnitId);
            carReservation.DtStart = DtStart;
            carReservation.LocStart = LocStart;
            carReservation.DtEnd = DtEnd;
            carReservation.LocEnd = LocEnd;
            carReservation.BaseRate = BaseRate;
            carReservation.Destinations = Destinations;
            carReservation.UseFor = UseFor;
            carReservation.RenterName = RenterName;
            carReservation.RenterEmail = RenterEmail;
            carReservation.RenterMobile = RenterMobile;
            carReservation.RenterAddress = RenterAddress;
            carReservation.RenterFbAccnt = RenterFbAccnt;
            carReservation.RenterLinkedInAccnt = RenterLinkedInAccnt;
            carReservation.EstHrPerDay = int.Parse( EstHrPerDay);
            carReservation.EstKmTravel = int.Parse( EstKmTravel);

            db.CarReservations.Add(carReservation);
            db.SaveChanges();
         
        }

    }

}
