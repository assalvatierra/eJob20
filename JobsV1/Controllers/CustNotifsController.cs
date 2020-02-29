using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using JobsV1.Models;
using JobsV1.Models.Class;
using Newtonsoft.Json;

namespace JobsV1.Controllers
{
    public class CustNotifsController : Controller
    {
        private JobDBContainer db = new JobDBContainer();
        private EmailBlaster emailSender = new EmailBlaster();
        private CustNotifClass notify = new CustNotifClass();
        private DateClass dt = new DateClass();

        private List<SelectListItem> OccurenceList = new List<SelectListItem> {
                new SelectListItem { Value = "ONE-TIME", Text = "One Time" },
                new SelectListItem { Value = "DAILY", Text =  "Daily" },
                new SelectListItem { Value = "WEEKLY", Text =  "Weekly" },
                new SelectListItem { Value = "MONTHLY", Text =  "Monthly" }
                };

        private List<SelectListItem> StatusList = new List<SelectListItem> {
                new SelectListItem { Value = "ACTIVE", Text = "Active" },
                new SelectListItem { Value = "INACTIVE", Text =  "Inactive" },
                new SelectListItem { Value = "DONE", Text =  "Done" },
                new SelectListItem { Value = "CANCELLED", Text =  "Cancelled" }
                };

        // GET: CustNotifs
        public ActionResult Index()
        {
            ViewBag.Customers = db.Customers.OrderBy(s=>s.Name).ToList();
            return View(db.CustNotifs.ToList());
        }

        // GET: CustNotifs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustNotif custNotif = db.CustNotifs.Find(id);
            if (custNotif == null)
            {
                return HttpNotFound();
            }
            return View(custNotif);
        }

        // GET: CustNotifs/Create
        public ActionResult Create()
        {
            ViewBag.Occurence = new SelectList(OccurenceList, "Value", "Text");
            ViewBag.Status = new SelectList(StatusList, "Value", "Text");
            return View();
        }

        // POST: CustNotifs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,MsgTitle,MsgBody,DtEncoded,DtScheduled,Occurence,IsEmail,IsSms,Status")] CustNotif custNotif)
        {
            if (ModelState.IsValid)
            {
                db.CustNotifs.Add(custNotif);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Occurence = new SelectList(OccurenceList, "Value", "Text");
            ViewBag.Status = new SelectList(StatusList, "Value", "Text");

            return View(custNotif);
        }

        // GET: CustNotifs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustNotif custNotif = db.CustNotifs.Find(id);
            if (custNotif == null)
            {
                return HttpNotFound();
            }

            ViewBag.Occurence = new SelectList(OccurenceList, "Value", "Text");
            ViewBag.Status = new SelectList(StatusList, "Value", "Text");

            return View(custNotif);
        }

        // POST: CustNotifs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,MsgTitle,MsgBody,DtEncoded,DtScheduled,Occurence,IsEmail,IsSms,Status")] CustNotif custNotif)
        {
            if (ModelState.IsValid)
            {
                db.Entry(custNotif).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Occurence = new SelectList(OccurenceList, "Value", "Text");
            ViewBag.Status = new SelectList(StatusList, "Value", "Text");

            return View(custNotif);
        }

        // GET: CustNotifs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustNotif custNotif = db.CustNotifs.Find(id);
            if (custNotif == null)
            {
                return HttpNotFound();
            }
            return View(custNotif);
        }

        // POST: CustNotifs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CustNotif custNotif = db.CustNotifs.Find(id);
            db.CustNotifs.Remove(custNotif);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        #region Recipients 

        // GET: CustNotifs/RecipientList
        public string RecipientList(int? id)
        {
           var list = db.CustNotifRecipients.ToList();

            //convert list to json object
            return JsonConvert.SerializeObject(list, Formatting.Indented);
        }

        // POST: CustNotifs/AddRecipient
        // id = CustNotif id
        public string AddRecipient(int? id, int customerId, string email, string mobile)
        {
            if (id != null)
            {
                try
                {

                    CustNotifRecipient custRecipient = new CustNotifRecipient();
                    custRecipient.CustNotifId = (int)id;
                    custRecipient.CustomerId = customerId;
                    custRecipient.NotifRecipientId = CreateRecipient(email, mobile);

                    db.CustNotifRecipients.Add(custRecipient);
                    db.SaveChanges();

                    var notif = db.CustNotifs.Find(id);
                    CreateActivity(custRecipient.Id, notif.DtScheduled, "PENDING");
                }
                catch (Exception err)
                {
                    return JsonConvert.SerializeObject(err, Formatting.Indented);
                }
                //return View(db.CustNotifRecipients.ToList());
            }
            return JsonConvert.SerializeObject("500", Formatting.Indented);
        }

        public int CreateRecipient(string email, string mobile)
        {

            NotifRecipient custRecipient = new NotifRecipient();
            custRecipient.Email = email;
            custRecipient.Mobile = mobile;

            db.NotifRecipients.Add(custRecipient);
            db.SaveChanges();
            return custRecipient.Id;

        }

        public string GetRecipientList(int id)
        {
            var recipientList = notify.GetRecipientList(id);
           return JsonConvert.SerializeObject(recipientList, Formatting.Indented);

        }

        public string GetRecipient(int id)
        {
            var customer =  db.Customers.Find(id);
            cNotifRecipient cRecipient = new cNotifRecipient();
            cRecipient.Id = customer.Id;
            cRecipient.Name = customer.Name;
            cRecipient.Email = customer.Email;
            cRecipient.Mobile = customer.Contact2;
            cRecipient.CustomerId = customer.Id;

            return JsonConvert.SerializeObject(cRecipient, Formatting.Indented);
        }

        //TODO : Cancell CustNotification Activity on DeleteRecipient
        public string DeleteRecipient(int id)
        {
            try
            {
                //check if recipient has activitity 
                //if yes, then remove activity
                 var custActs = db.CustNotifActivities.Where(s => s.CustNotifRecipientId == id).ToList();
                if (custActs.Count > 0)
                {

                    foreach (var act in custActs)
                    {
                        db.CustNotifActivities.Remove(act);
                        db.SaveChanges();
                    }

                }
                //remove recipient
                CustNotifRecipient custNotif = db.CustNotifRecipients.Find(id);
                db.CustNotifRecipients.Remove(custNotif);
                db.SaveChanges();

                return "200";
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }

        }
        #endregion

        #region EmailHandler
        //POST: Send Email Notification Test
        public string SendEmail(int? id)
        {
            if (id != null)
            {
                //get notification
                var notif = db.CustNotifs.Find(id);

                //get recipient list
                foreach (var recipients in notif.CustNotifRecipients)
                {
                    var email = recipients.NotifRecipient.Email;
                    var subject = notif.MsgTitle;
                    var content = notif.MsgBody;
                    emailSender.SendMail(email, subject, content);

                    //CreateActivity((int)recipients.Id, notif.DtScheduled, "PENDING");
                }

                return "200";
            }
            return "500";
        }
        //POST: Send Email Notification Test
        //id : Notification Activity Id
        public string SendPendingEmail()
        {
            try
            {
                //get notification
                var notifActList = notify.GetPendingNotif();

                foreach (var notification in notifActList)
                {
                    //send email
                    notify.SendEmailNotif(notification.Id);
                }

                return "200";
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
        
        //GET: get pending count
        public string CheckPendingCount()
        {
            var count = notify.GetPendingNotif().Count;
            return JsonConvert.SerializeObject(count, Formatting.Indented);
        }

        #endregion

        #region Activity
        // GET: CustNotifs
        public ActionResult Activity()
        {
            return View();
        }
        
        //GET: ajax get activity list
        public string GetActivity()
        {
            var activity = notify.GetActivityList();

            return JsonConvert.SerializeObject(activity, Formatting.Indented);
        }

        //GET: ajax get activity list
        public string CreateActivity(int id, DateTime date ,string status)
        {
            CustNotifActivity activity = new CustNotifActivity();
            activity.DtActivity = date;
            activity.Status = status;
            activity.CustNotifRecipientId = id;

            db.CustNotifActivities.Add(activity);
            db.SaveChanges();

            return "";
        }


        //UPDATE: update activity status
        public string UpdateActivityStatus(int id, string status)
        {
            try
            {

                CustNotifActivity activity = db.CustNotifActivities.Find(id);
                activity.Status = status;

                db.Entry(activity).State = EntityState.Modified;
                db.SaveChanges();

                return "200";

            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
        #endregion

    }
}
