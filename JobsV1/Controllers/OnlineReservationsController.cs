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

        public ActionResult Form(string tourCode)
        {
            var product = pdb.SmProducts.Where(s=>s.Code == tourCode).FirstOrDefault();

            ViewBag.Amount = product.Price;
            ViewBag.ProductName = product.Name;
            ViewBag.tourCode = tourCode;

            //get paypal keys at db
            PaypalAccount paypal = db.PaypalAccounts.Where(p => p.SysCode.Equals("RealWheels")).FirstOrDefault();
            ViewBag.key = paypal.Key;

            return View();
        }

        public String AddRecord(string tourCode, string name, string number, string email, string dtstart, string dtend)
        {
            DateClass today = new DateClass();
            
            OnlineReservation reserve = new OnlineReservation();
            reserve.DtPosted = today.GetCurrentDateTime();
            reserve.DtStart = DateTime.Parse(dtstart);
            reserve.DtEnd = DateTime.Parse(dtend);
            reserve.Name = name;
            reserve.Email = email;
            reserve.ContactNum = number;
            reserve.ProductCode = tourCode;

            db.OnlineReservations.Add(reserve);
            db.SaveChanges();

            var lastId = db.OnlineReservations.OrderByDescending(s => s.Id).FirstOrDefault().Id;

            return lastId.ToString();
        }


        public String UpdateRecord(int TransId, decimal PaymentAmt, string PaymentStatus, string DtPayment)
        {
            DateClass today = new DateClass();

            OnlineReservation reserve = db.OnlineReservations.Find(TransId);
            reserve.PaymentAmt = PaymentAmt;
            reserve.PaymentStatus = PaymentStatus;
            reserve.DtPayment = DateTime.Parse(DtPayment);

            db.Entry(reserve).State = EntityState.Modified;
            db.SaveChanges();
            

            return "300";
        }
    }
}
