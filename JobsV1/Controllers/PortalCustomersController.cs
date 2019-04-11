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

namespace JobsV1.Controllers
{
    public class PortalCustomersController : Controller
    {
        private JobDBContainer db = new JobDBContainer();
        private PortalCustomersClass Pcust = new PortalCustomersClass();

        // GET: PortalCustomers
        public ActionResult Index()
        {
            return View(db.PortalCustomers.ToList());
        }

        // GET: PortalCustomers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PortalCustomer portalCustomer = db.PortalCustomers.Find(id);
            if (portalCustomer == null)
            {
                return HttpNotFound();
            }
            return View(portalCustomer);
        }

        // GET: PortalCustomers/Create
        public ActionResult Create()
        {
            PortalCustomer custTemp = new PortalCustomer();
            custTemp.ExpiryDt = GetCurrentTime().AddDays(2);

            ViewBag.CustomerId = new SelectList(db.Customers, "Id", "Name");
            return View(custTemp);
        }

        // POST: PortalCustomers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ContactNum,Password,ExpiryDt,CustomerID")] PortalCustomer portalCustomer)
        {
            if (ModelState.IsValid)
            {
                db.PortalCustomers.Add(portalCustomer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CustomerId = new SelectList(db.Customers, "Id", "Name", portalCustomer.CustomerId);

            return View(portalCustomer);
        }

        // GET: PortalCustomers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PortalCustomer portalCustomer = db.PortalCustomers.Find(id);
            if (portalCustomer == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustomerId = new SelectList(db.Customers, "Id", "Name", portalCustomer.CustomerId);
            return View(portalCustomer);
        }

        // POST: PortalCustomers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ContactNum,Password,ExpiryDt,CustomerID")] PortalCustomer portalCustomer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(portalCustomer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CustomerId = new SelectList(db.Customers, "Id", "Name",portalCustomer.CustomerId);
            return View(portalCustomer);
        }

        // GET: PortalCustomers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PortalCustomer portalCustomer = db.PortalCustomers.Find(id);
            if (portalCustomer == null)
            {
                return HttpNotFound();
            }
            return View(portalCustomer);
        }

        // POST: PortalCustomers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PortalCustomer portalCustomer = db.PortalCustomers.Find(id);
            db.PortalCustomers.Remove(portalCustomer);
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

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            ViewBag.errorMsg = "";
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(PortalCustomer model, string returnUrl)
        {
            PortalCustomersClass pCustomer = new PortalCustomersClass();
            string msg = "";
            var custId = 0;
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            //check customer login information
            custId = pCustomer.checklogin(model.ContactNum, model.Password);
            if ( custId == 0 )
            {
                //add to customer to session

                PortalCustomer cust = pCustomer.getCustomer(model.ContactNum, model.Password);
                return RedirectToAction("JobList", "PortalCustomers", new { custId = cust.CustomerId });
            }

            //login expire
            if ( custId == 1) 
            {
                msg = "Login information is expired";
            }


            //login invalid
            if (custId == 2)
            {
                msg = "Invalid number/password";
            }

            ViewBag.errorMsg = msg;
            return View(model);
        }

        private void addSession(string custNum , string pass)
        {
            Session["PortalCust_Num"]  = custNum;
            Session["PortalCust_Pass"] = pass;
        }
        
        //get current time based on Singapore Standard Time 
        //SGT - UTC +8
        protected DateTime GetCurrentTime()
        {
            DateTime _localTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Singapore Standard Time"));
            _localTime = _localTime.Date;

            return _localTime;
        }

        //get generate password using the customer ID and date
        public string getPassword(int? custId)
        {
            return custId != null ? Pcust.generatePass((int)custId) : "";
        }

        public string getCustomerNum(int? custId)
        {
            //"your string".Any(char.IsDigit)
            if (custId != null)
            {
                var custContact = db.Customers.Find(custId);
                //check if contact number 1 is a number / contains a number
                if (custContact.Contact1.Any(Char.IsDigit))
                {
                    return custContact.Contact1;
                }
                else
                {
                    return custContact.Contact2;
                }

            }
            return ""; // return null if customer Id is null    
        }

        public ActionResult JobList(int? custId)
        {
            PortalCustomersClass pCustomer = new PortalCustomersClass();

            if (custId != null)
            {
                 var joblist = pCustomer.getJobList(5, (int)custId);
                ViewBag.CustomerName = db.Customers.Find((int)custId).Name;

                return View(joblist);
            }

            return RedirectToAction("Login","PortalCustomers",null);
        }

        public bool validateCustomer()
        {
            var returnFlag = true;
            //check sessions if empty
            //if not empty, validate login
            //PortalCust_Num, PortalCust_Pass
            if (Session["PortalCust_Num"] != null)
            {
                var sessionNum = Session["PortalCust_Num"].ToString();
                var sessionPass = Session["PortalCust_Pass"].ToString();

                //verify
                var verify = Pcust.checklogin(sessionNum, sessionPass);

                switch (verify)
                {
                    case 0: //login ok
                        returnFlag = false;
                        break;
                    case 1: //login expired
                    case 2: //invalid login
                        returnFlag = true;
                        break;
                    default:
                        returnFlag = true;
                        break;
                }
            }
           

            return returnFlag;
        }

        public string getCustomerName(int custId)
        {
            return db.Customers.Find(custId) != null ? db.Customers.Find(custId).Name : "";
        }
    }
}
