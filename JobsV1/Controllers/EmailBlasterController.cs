using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using JobsV1.Models;

namespace JobsV1.Controllers
{

    public class BlasterRecipients
    {
        public int id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public List<string> Category { get; set; }
        public int reportId { get; set; }
    }

    public class EmailBlasterController : Controller
    {
        private JobDBContainer db = new JobDBContainer();
        private EmailBlaster eb = new EmailBlaster();

        private List<SelectListItem> EmailCat = new List<SelectListItem> {
                new SelectListItem { Value = "CAR-RENTAL", Text = "Car Rental" },
                new SelectListItem { Value = "TOUR", Text = "Tour" }
                };

        private List<SelectListItem> RecipientsCat = new List<SelectListItem> {
                new SelectListItem { Value = "CLIENT", Text = "Client" },
                new SelectListItem { Value = "COMPANY", Text = "Company" }
                };

        private List<SelectListItem> CompanyList = new List<SelectListItem> {
                new SelectListItem { Value = "REALBREEZE", Text = "Realbreeze Travel and Tours" },
                new SelectListItem { Value = "REALWHEELS", Text = "RealWheels Car Rental" }
                };

        // GET: CustEntMains

        // GET: EmailBlasterTemplates
        public ActionResult Index()
        {
            return View(db.EmailBlasterTemplates.ToList());
        }

        // GET: EmailBlasterTemplates/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmailBlasterTemplate emailBlasterTemplate = db.EmailBlasterTemplates.Find(id);
            if (emailBlasterTemplate == null)
            {
                return HttpNotFound();
            }
            return View(emailBlasterTemplate);
        }

        // GET: EmailBlasterTemplates/Create
        public ActionResult Create()
        {
            ViewBag.EmailCategory = new SelectList(EmailCat, "value", "text");
            ViewBag.RecipientsCategory = new SelectList(RecipientsCat, "value", "text");
            ViewBag.Company = new SelectList(CompanyList, "value", "text");
            return View();
        }

        // POST: EmailBlasterTemplates/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,EmailCategory,RecipientsCategory,EmailTitle,EmailBody,ContentPicture,AttachmentLink,Company")] EmailBlasterTemplate emailBlasterTemplate)
        {
            if (ModelState.IsValid)
            {
                //emailBlasterTemplate.EmailBody = Server.HtmlEncode(emailBlasterTemplate.EmailBody); 
                db.EmailBlasterTemplates.Add(emailBlasterTemplate);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.EmailCategory = new SelectList(EmailCat, "Value", "text");
            ViewBag.RecipientsCategory = new SelectList(RecipientsCat, "value", "text");
            ViewBag.Company = new SelectList(CompanyList, "value", "text");
            return View(emailBlasterTemplate);
        }

        // GET: EmailBlasterTemplates/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmailBlasterTemplate emailBlasterTemplate = db.EmailBlasterTemplates.Find(id);

            //emailBlasterTemplate.EmailBody = Uri(emailBlasterTemplate.EmailBody);

            if (emailBlasterTemplate == null)
            {
                return HttpNotFound();
            }

            ViewBag.EmailCategory = new SelectList(EmailCat, "value", "text", emailBlasterTemplate.EmailCategory);
            ViewBag.RecipientsCategory = new SelectList(RecipientsCat, "value", "text", emailBlasterTemplate.RecipientsCategory);
            ViewBag.Company = new SelectList(CompanyList, "value", "text",emailBlasterTemplate.Company);
            return View(emailBlasterTemplate);
        }

        // POST: EmailBlasterTemplates/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,EmailCategory,RecipientsCategory,EmailTitle,EmailBody,ContentPicture,AttachmentLink,Company")] EmailBlasterTemplate emailBlasterTemplate)
        {
            if (ModelState.IsValid)
            {
                //emailBlasterTemplate.EmailBody = Uri.EscapeDataString(emailBlasterTemplate.EmailBody);
                db.Entry(emailBlasterTemplate).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.EmailCategory = new SelectList(EmailCat, "value", "text", emailBlasterTemplate.EmailCategory);
            ViewBag.RecipientsCategory = new SelectList(RecipientsCat, "value", "text", emailBlasterTemplate.RecipientsCategory);
            ViewBag.Company = new SelectList(CompanyList, "value", "text", emailBlasterTemplate.Company);
            return View(emailBlasterTemplate);
        }

        // GET: EmailBlasterTemplates/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmailBlasterTemplate emailBlasterTemplate = db.EmailBlasterTemplates.Find(id);
            if (emailBlasterTemplate == null)
            {
                return HttpNotFound();
            }
            return View(emailBlasterTemplate);
        }

        // POST: EmailBlasterTemplates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            EmailBlasterTemplate emailBlasterTemplate = db.EmailBlasterTemplates.Find(id);
            db.EmailBlasterTemplates.Remove(emailBlasterTemplate);
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

        //send email to customers / clients
        public ActionResult SendEmailBlast(int id)
        {
            //get email parameters using id
            var emailTemplate = db.EmailBlasterTemplates.Find(id);
            string recipientFilter = emailTemplate.RecipientsCategory;
            string emailFilter = emailTemplate.EmailCategory;

            //create Email Content
            string emailTitle = emailTemplate.EmailTitle;
            string emailBody = emailTemplate.EmailBody;
            string emailPicture = emailTemplate.ContentPicture;
            string emailAttachmentLink = emailTemplate.AttachmentLink;
            string company = emailTemplate.Company;

            //get recipients list from customer's table then filter
            var recipients = db.Customers.Where(c=>c.Status == "ACT").ToList();

            List<BlasterRecipients> blasterRecipients = new List<BlasterRecipients>();
            int count = 0;
            foreach (var recipient in recipients)
            {
                //get categories of recipients
                if (recipient.CustCats != null)
                {

                    var recipientCategories = recipient.CustCats.Where(r => r.CustomerId == recipient.Id ).ToList();


                    //get categories
                    List<String> categoryList = new List<String>();
                    foreach (var category in recipientCategories)
                    {
                        categoryList.Add(category.CustCategory.Name);
                    }
                    
                    //build recipient list for filter
                    blasterRecipients.Add(new BlasterRecipients
                    {
                        id = recipient.Id,
                        Name = recipient.Name,
                        Email = recipient.Email,
                        Category = categoryList
                    });

                }//end if
            }

            //Filter out recipients with matching the user categories
            var FinalRecipients = blasterRecipients.Where(b => b.Category.Contains(emailFilter) || b.Category.Contains(recipientFilter)).ToList();

            //send one by one, get status of each email and log
            int logId = 1;
            int blastReportId = getPrevBlastRecord();

            foreach (var recipient in FinalRecipients)
            {
                string status = eb.SendMailBlaster(recipient.Email, emailTitle, emailBody, emailPicture, emailAttachmentLink,company);
                logId         = LogEmailBlastResult(recipient.id, recipient.Email, status);
                blastReportId = BlastRecord(logId, id, blastReportId);
            }

            //display result
            return RedirectToAction("BlastResult", new { reportId = blastReportId , sDate = "today", eDate = "today"});
            
        }

        public string EmailBlastSingle(int custId, int templateId ) {

            int blastReportId = getPrevBlastRecord();
            var customer = db.Customers.Find(custId);
            var blasterTemp = db.EmailBlasterTemplates.Find(templateId);
            
            string status = eb.SendMailBlaster(customer.Email, blasterTemp.EmailTitle,
                blasterTemp.EmailBody, blasterTemp.ContentPicture, blasterTemp.AttachmentLink, blasterTemp.Company);
            var logId = LogEmailBlastResult(customer.Id, customer.Email, status);
            blastReportId = BlastRecord(logId, templateId, blastReportId);

            return "200";
        }


        //get list of cutomers
        public ActionResult EmailBlastList(int id)
        {
            //get email parameters using id
            var emailTemplate = db.EmailBlasterTemplates.Find(id);
            string recipientFilter = emailTemplate.RecipientsCategory;
            string emailFilter = emailTemplate.EmailCategory;

            //create Email Content
            string emailTitle = emailTemplate.EmailTitle;
            string emailBody = emailTemplate.EmailBody;
            string emailPicture = emailTemplate.ContentPicture;
            string emailAttachmentLink = emailTemplate.AttachmentLink;
            string company = emailTemplate.Company;

            //get recipients list from customer's table then filter
            var recipients = db.Customers.Where(c => c.Status == "ACT").ToList();

            List<BlasterRecipients> blasterRecipients = new List<BlasterRecipients>();
            int count = 0;
            foreach (var recipient in recipients)
            {
                //get categories of recipients
                if (recipient.CustCats != null)
                {

                    var recipientCategories = recipient.CustCats.Where(r => r.CustomerId == recipient.Id).ToList();


                    //get categories
                    List<String> categoryList = new List<String>();
                    foreach (var category in recipientCategories)
                    {
                        categoryList.Add(category.CustCategory.Name);
                    }

                    //build recipient list for filter
                    blasterRecipients.Add(new BlasterRecipients
                    {
                        id = recipient.Id,
                        Name = recipient.Name,
                        Email = recipient.Email,
                        Category = categoryList
                    });

                }//end if
            }

            //Filter out recipients with matching the user categories
            var FinalRecipients = blasterRecipients.Where(b => b.Category.Contains(emailFilter) || b.Category.Contains(recipientFilter)).ToList();

            ////send one by one, get status of each email and log
            //int logId = 1;
            //int blastReportId = getPrevBlastRecord();

            //foreach (var recipient in FinalRecipients)
            //{
            //    string status = eb.SendMailBlaster(recipient.Email, emailTitle, emailBody, emailPicture, emailAttachmentLink, company);
            //    logId = LogEmailBlastResult(recipient.id, recipient.Email, status);
            //    blastReportId = BlastRecord(logId, id, blastReportId);
            //}

            //display result
            //return RedirectToAction("BlastResult", new { reportId = blastReportId, sDate = "today", eDate = "today" });


            ViewBag.templateId = id;
            return View(FinalRecipients);

        }

        //Save log of blast in the server
        public int LogEmailBlastResult(int recipientId,string email,string EmailStatus)
        {
            EmailBlasterLogs logs = new EmailBlasterLogs();
            DateTime Datetoday = DateTime.Now;
            string status = EmailStatus;

            logs.DateTime = Datetoday;
            logs.Status = status == null ? "failed" : status;
            logs.Email = email;
            logs.CustId = recipientId;

            db.EmailBlasterLogs.Add(logs);
            db.SaveChanges();
            var latestblasterlog = db.EmailBlasterLogs != null ? db.EmailBlasterLogs.OrderByDescending(c=>c.Id).FirstOrDefault() : db.EmailBlasterLogs.FirstOrDefault() ;
            
            return latestblasterlog.Id;

        }

        public int BlastRecord(int logId, int templateId, int reportId)
        {
            BlasterLog record = new BlasterLog();
            record.EmailBlasterLogsId = logId;
            record.EmailBlasterTemplateId = templateId;
            record.ReportId = reportId;

            db.BlasterLogs.Add(record);
            db.SaveChanges();

            return record.ReportId;
        }

        public int getPrevBlastRecord()
        {
            int prevRecordId = 0;
                
            if (db.BlasterLogs.FirstOrDefault() != null)
            {
                prevRecordId = db.BlasterLogs.OrderByDescending(s=>s.Id).FirstOrDefault().ReportId+1;
            }else
            {
                prevRecordId = 1;
            }
               

            return prevRecordId;
        }

        //show blast result
        public ActionResult BlastResult(int reportId, string sDate, string eDate)
        {
            if (reportId != 0)
            {
                if (sDate != "today" && eDate != "today")
                {
                    DateTime Date1 = DateTime.Parse(sDate);
                    DateTime Date2 = DateTime.Parse(eDate);
                                                        
                    return View(db.BlasterLogs.Include(c => c.EmailBlasterLog)
                        .Where(c => c.EmailBlasterLog.DateTime.Date.CompareTo(Date1.Date ) >= 0 && c.EmailBlasterLog.DateTime.Date.CompareTo(Date2.Date) <= 0)
                        .ToList());
                }

                return View(db.BlasterLogs.Include(c => c.EmailBlasterLog).Where(c => c.ReportId == reportId).ToList());

            }

            var allRecord = db.BlasterLogs.ToList();
            
            return View(allRecord);
        }

        
    }
}
