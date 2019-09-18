using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using JobsV1.Models;
using JobsV1.Areas.Products.Models;

namespace JobsV1.Controllers
{
    public class OnlineReservationsController : Controller
    {
        private JobDBContainer db = new JobDBContainer();
        private ProdDBContainer pdb = new ProdDBContainer();

        // GET: OnlineReservations
        public ActionResult Index()
        {
            return View(db.OnlineReservations.ToList());
        }

        // GET: OnlineReservations/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OnlineReservation onlineReservation = db.OnlineReservations.Find(id);
            if (onlineReservation == null)
            {
                return HttpNotFound();
            }
            var product = pdb.SmProducts.Where(p => p.Code.Equals(onlineReservation.ProductCode)).FirstOrDefault();
            ViewBag.ProductName = product.Name;
            ViewBag.ProductPrice = product.Price;
            ViewBag.paymentRecord = onlineReservation.RsvPayments.ToList();
            return View(onlineReservation);
        }

        // GET: OnlineReservations/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: OnlineReservations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,DtPosted,ProductCode,DtStart,DtEnd,Name,ContactNum,Email,PaymentAmt,PaymentStatus,DtPayment")] OnlineReservation onlineReservation)
        {
            if (ModelState.IsValid)
            {
                db.OnlineReservations.Add(onlineReservation);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(onlineReservation);
        }

        // GET: OnlineReservations/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OnlineReservation onlineReservation = db.OnlineReservations.Find(id);
            if (onlineReservation == null)
            {
                return HttpNotFound();
            }
            return View(onlineReservation);
        }

        // POST: OnlineReservations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,DtPosted,ProductCode,DtStart,DtEnd,Name,ContactNum,Email,PaymentAmt,PaymentStatus,DtPayment")] OnlineReservation onlineReservation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(onlineReservation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(onlineReservation);
        }

        // GET: OnlineReservations/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OnlineReservation onlineReservation = db.OnlineReservations.Find(id);
            if (onlineReservation == null)
            {
                return HttpNotFound();
            }
            return View(onlineReservation);
        }

        // POST: OnlineReservations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            OnlineReservation onlineReservation = db.OnlineReservations.Find(id);
            db.OnlineReservations.Remove(onlineReservation);
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

        public ActionResult Form(string tourCode, string svcType)
        {
            var product = pdb.SmProducts.Where(s=>s.Code == tourCode).FirstOrDefault();

            ViewBag.Amount = product.Price;
            ViewBag.ProductName = product.Name;
            ViewBag.tourCode = tourCode;
            ViewBag.svcType = svcType;
            ViewBag.ProdDesc = product.SmProdDescs.ToList();
            ViewBag.ProdInfo = product.SmProdInfoes.ToList();

            var adsCat = pdb.SmProdCats.Where(s => s.SmProductId == product.Id).ToList();
            var adsList = adsCat.Select(s => s.SmCategoryId).ToList();
            ViewBag.adsList = adsCat;

            //Ads
            ViewBag.ProdImg = getProdImage(product);
            ViewBag.ProdAdslist = pdb.SmProdAds.Where(s => adsList.Contains(s.SmCategoryId)).ToList().Take(6);
            ViewBag.Unit = getUoM(product);

            //get paypal keys at db
            PaypalAccount paypal = db.PaypalAccounts.Where(p => p.SysCode.Equals("RealWheels")).FirstOrDefault();
            ViewBag.key = paypal.Key;

            return View();
        }

        public string getProdImage(SmProduct product)
        {
            var tempProd = product.SmProdAds.Where(s => s.SmProductId == product.Id).FirstOrDefault();
            if (tempProd != null)
                return tempProd.Image;
            else
                return "placeholder.png";
        }

        public string getUoM(SmProduct product)
        {
            var tempOum = product.SmRates.FirstOrDefault();
            if (tempOum != null)
                return tempOum.SmRateUoM.Name;
            else
                return "Pax";
        }

        public String AddRecord(string tourCode, string name, string number, string email, string dtstart, int qty, string pickup, int amount)
        {
            DateClass today = new DateClass();
            
            OnlineReservation reserve = new OnlineReservation();
            reserve.DtPosted = today.GetCurrentDateTime();
            reserve.DtStart = DateTime.Parse(dtstart);
            reserve.Name = name;
            reserve.Email = email;
            reserve.ContactNum = number;
            reserve.ProductCode = tourCode;
            reserve.Qty = qty;
            reserve.PickupDtls = pickup;
            reserve.PaymentAmt = amount;

            db.OnlineReservations.Add(reserve);
            db.SaveChanges();

            var lastId = db.OnlineReservations.OrderByDescending(s => s.Id).FirstOrDefault().Id;

            return lastId.ToString();
        }
        
        public String UpdateRecord(int TransId, decimal PaymentAmt, string PaymentStatus, string DtPayment, string PaymentId)
        {
            DateClass today = new DateClass();

            try
            {
                OnlineReservation reserve = db.OnlineReservations.Find(TransId);
                RsvPayment payment = new RsvPayment();
                payment.OnlineReservationId = reserve.Id;
                payment.Amount = PaymentAmt;
                payment.Status = PaymentStatus;
                payment.DtPayment = today.GetCurrentDateTime();
                payment.PaypaPaymentId = PaymentId;
                
                db.RsvPayments.Add(payment);
                db.SaveChanges();

                return "300";
            }
            catch (Exception ex)
            {
                return "error";
            }

        }

        public string SendEmailPayment(int id, string svcType)
        {
            string mailResult = "error";
            EMailHandler email = new EMailHandler();
            OnlineReservation reservation = db.OnlineReservations.Find(id);
            if (reservation != null)
            {
                string companyEmail = "reservation.realwheels@gmail.com"; //realwheelsemail
                string ajdavaoEmail = "ajdavao88@gmail.com";
                string adminEmail   = "travel.realbreeze@gmail.com";
                string testadminEmail = "realbreezemark@gmail.com";

                mailResult = email.SendMailOnlineReserve(id, reservation.Email, "CLIENT", svcType); //send email to client first
                mailResult = email.SendMailOnlineReserve(id, testadminEmail, "ADMIN", svcType);     //send email to client first

                mailResult = email.SendMailOnlineReserve(id, companyEmail, "ADMIN", svcType);
                mailResult = email.SendMailOnlineReserve(id, ajdavaoEmail, "ADMIN", svcType);
                mailResult = email.SendMailOnlineReserve(id, adminEmail, "ADMIN", svcType);

            }
            return mailResult;
        }


        public string SendEmailNotification(int id, string svcType)
        {
            string mailResult = "error";
            EMailHandler email = new EMailHandler();
            OnlineReservation reservation = db.OnlineReservations.Find(id);
            if (reservation != null)
            {
                string companyEmail = "reservation.realwheels@gmail.com"; //realwheelsemail
                string ajdavaoEmail = "ajdavao88@gmail.com";
                string adminEmail = "travel.realbreeze@gmail.com";
                string testadminEmail = "realbreezemark@gmail.com";

                mailResult = email.SendMailOnlineInquire(id, reservation.Email, "CLIENT", svcType); //send email to client first
                mailResult = email.SendMailOnlineInquire(id, testadminEmail, "ADMIN", svcType);     //send email to client first

                mailResult = email.SendMailOnlineInquire(id, companyEmail, "ADMIN", svcType);
                mailResult = email.SendMailOnlineInquire(id, ajdavaoEmail, "ADMIN", svcType);
                mailResult = email.SendMailOnlineInquire(id, adminEmail, "ADMIN", svcType);

            }
            return mailResult;
        }
    }
}
