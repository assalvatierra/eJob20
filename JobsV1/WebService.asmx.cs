
using JobsV1.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace JobsV1
{
    /// <summary>
    /// Summary description for WebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class WebService : System.Web.Services.WebService
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
                string dtSchedule = ((DateTime)svc.DtStart).ToString("dd MMM yyyy (ddd)");
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


            /*
            var recentControls = rpidb.RpiControls.Where(m => m.RpiDeviceId == deviceId).OrderByDescending(m => m.DtSchedule).ToList();

            DataTable Dt = new DataTable("Table");

            Dt.Columns.Add("Id", typeof(int));
            Dt.Columns.Add("DtSchedule", typeof(DateTime));
            Dt.Columns.Add("Data", typeof(string));
            Dt.Columns.Add("RpiDeviceId", typeof(int));

            //get details of each failed items from recipientId
            foreach (var control in recentControls)
            {

                int Id = control.Id; //component id  
                DateTime dtSchedule = DateTime.Parse(control.DtSchedule);
                string Data = control.Data;
                int rpiDeviceId = control.RpiDeviceId;

                if (DateTime.Parse(dtSchedule.ToString()).CompareTo(DateTime.Now) <= 0)
                {
                    Dt.Rows.Add(Id, dtSchedule, Data, rpiDeviceId);
                    break;
                }

            }

            DataSet ds = new DataSet();
            ds.Tables.Add(Dt);
            ds.DataSetName = "Table";

            Context.Response.Clear();
            Context.Response.ContentType = "application/json";
            Context.Response.Write(JsonConvert.SerializeObject(ds, Newtonsoft.Json.Formatting.Indented));
            */
        }

    }
}
