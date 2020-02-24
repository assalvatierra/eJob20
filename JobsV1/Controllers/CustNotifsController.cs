using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using JobsV1.Models;
using Newtonsoft.Json;

namespace JobsV1.Controllers
{
    public class CustNotifsController : Controller
    {
        private JobDBContainer db = new JobDBContainer();
        private EmailBlaster emailSender = new EmailBlaster();

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
        // id = notification id
        // 
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
        #endregion

        #region EmailHandler
        //POST: Send Email Notification Test
        public string SendEmail(int? id)
        {
            if (id != null)
            {
                var email = "jahdielsvillosa@gmail.com";
                var subject = "email subject Test";
                var content = "email content test";
                emailSender.SendMail(email, subject, content);
                return "200";
            }
            return "500";
        }

        #endregion
    }
}
