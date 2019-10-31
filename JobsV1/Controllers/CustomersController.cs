using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using JobsV1.Models;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace JobsV1.Controllers
{
    public class CustomersController : Controller
    {
        private JobDBContainer db = new JobDBContainer();
        private CustomerClass custdb = new CustomerClass();
        private List<SelectListItem> StatusList = new List<SelectListItem> {
                new SelectListItem { Value = "ACT", Text = "Active" },
                new SelectListItem { Value = "INC", Text = "Inactive" },
                new SelectListItem { Value = "BAD", Text = "Bad Account" }
                };

        // GET: Customers
        public  ActionResult Index(string status, string search)
        {

            List<CustomerDetails> customerDetailList = new List<CustomerDetails>();
            //get customer list async
            customerDetailList = custdb.getCustomerList(status,search);

            ViewBag.status = status;

            return View(customerDetailList.OrderBy(s=>s.JobID));
        }
        
        // GET: Customers/Details/5
        public ActionResult Details(int? id, int? top, int? last,string sdate, string edate, string status, string sortdate)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }

            if (top == null)
            {
                top = 30;
            }

            //generate partial view list for companies
            PartialView_Companies(id);
            PartialView_Jobs((int)id,(int)top, sdate,edate,status, sortdate);
            PartialView_Categories(id);
            PartialView_CustomerFiles(id);
            ViewBag.categoryList = db.CustCategories.ToList();
            ViewBag.custId = (int)id;
            PartialView_CustSocial((int)id);
            
            return View(customer);
        }

        // GET: Customers/Create
        public ActionResult Create()
        {
            ViewBag.Status = new SelectList(StatusList, "value", "text");

            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Email,Contact1,Contact2,Remarks,Status")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                if (customer.Status == null || customer.Status.Trim() == "") customer.Status = "ACT";

                db.Customers.Add(customer);
                db.SaveChanges();
                return RedirectToAction("Details", new { id = customer .Id});
            }

            return View(customer);
        }

        // GET: Customers/CompanyCreate
        public ActionResult CompanyCreate()
        {
            return View();
        }

        // POST: Customers/CompanyCreate
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CompanyCreate([Bind(Include = "Id,Name,Address,Contact1,Contact2,iconPath")] CustEntMain custEntMain, int? id)
        {
            if (ModelState.IsValid)
            {

                db.CustEntMains.Add(custEntMain);
                db.SaveChanges();

                //save new company to customer
                CustEntity company = new CustEntity();
                company.CustEntMainId = custEntMain.Id;
                company.CustomerId = (int)id;
                db.CustEntities.Add(company);
                db.SaveChanges();

                return RedirectToAction("Details", "Customers", new { id = id });
            }

            return View(custEntMain);
        }

        // GET: Customers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }

            ViewBag.Status = new SelectList(StatusList, "value", "text", customer.Status);

            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Email,Contact1,Contact2,Remarks,Status")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", new { id = customer.Id });
            }

            ViewBag.Status = new SelectList(StatusList, "value", "text", customer.Status);

            return View(customer);
        }

        // GET: Customers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Customer customer = db.Customers.Find(id);
            db.Customers.Remove(customer);
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

        //---------------------------------------------//

        //returns the companies of the customers
        //using Viewbags
        private void PartialView_Companies(int? id)
        {
            if(id != null)
            {
                

                //PartialView for Details of the Customer
                List<CustEntMain> CompanyList = custdb.getCustCompanies((int)id);

                ViewBag.companyList = CompanyList.ToList();
         
                //get all companies
                List<CustEntMain> List = db.CustEntMains.ToList();

                ViewBag.companies = List;

                var Companies = db.CustEntities.Where(s => s.CustomerId == id).ToList();

                try
                {
                    //check if there is company linked ot customer
                    var RecentCompany = db.CustEntities.Where(s => s.CustomerId == id).OrderByDescending(s => s.Id).FirstOrDefault();
                    ViewBag.custposition = RecentCompany.Position;
                }
                catch (Exception ex)
                {
                    ViewBag.custposition = "N/A";
                }

                //var position = db.CustEntities.Where(s=>s.CustomerId == id && s.CustEntMainId == RecentCompany.CustEntMainId)


                ViewBag.companiesPrev = Companies;
                ViewBag.CustomerID = id;

            }
        }

        //customer jobs list
        private void PartialView_Jobs(int id, int top, string sdate, string edate, string status, string sortdate)
        {
            //get customer jobs and display to job list table
            ViewBag.jobList = custdb.getCustomerJobList(id,top,sdate,edate,status,sortdate);
        }

        //display list of categories assigned 
        //to the customer 
        private void PartialView_Categories(int? id)
        {
            //get list of categories
            ViewBag.categoryDetails = custdb.getCategoriesList((int)id);
            ViewBag.categoryList = db.CustCategories.ToList();
        }

        //display list of customer files 
        //uploaded on the system
        //Note: not working due to the 
        //problem adding files to online database
        private void PartialView_CustomerFiles(int? id)
        {
           //PartialView for Details of the Customer
           List<CustFiles> FilesList = custdb.getCustFiles((int)id);
            
           ViewBag.fileList = FilesList;
        }

        // /Customers/Details
        // assign company Category to the user
        public string addCompanyCat(int companyId, int userid, string position)
        {
                db.CustEntities.Add(new CustEntity
                {
                    CustEntMainId = companyId,
                    CustomerId = userid,
                    Position = position
                });
                db.SaveChanges();

                return "200";
        }

        //get list of customers with
        //a year past jobs and no recent jobs 
        public ActionResult DeactivateOldCustomer()
        {

            //get list of customers which are candidate for deactivation
            List<Customer> allCustomers =  custdb.getDeactivateCustomers();

            //Display to view customers
            return View(allCustomers);
        }


        //Deactive a multiple customer by changing its 
        //status from ACT to INC. 
        public ActionResult DeActivateAll()
        {
            //get list of customers which are candidate for deactivation
            List<Customer> dCustList = custdb.getDeactivateCustomers();

            //deactivate customers by changing its 
            //status from ACT to INC
            foreach (var customer in dCustList)
            {
                var deleteCust = db.Customers.Find(customer.Id);
                deleteCust.Status = "INC";    //deactivate customer
                db.Entry(deleteCust).State = EntityState.Modified;
                db.SaveChanges();
            }

            return RedirectToAction("DeactivateOldCustomer", "Customers");
        }

        //Deactivate a single customer by changing its 
        //status from ACT to INC
        public ActionResult DeactivateSingle(int id)
        {
            var customer = db.Customers.Find(id);
            if (customer != null)
            {
                customer.Status = "INC";    //deactivate customer
                db.Entry(customer).State = EntityState.Modified;
                db.SaveChanges();
            }

            return RedirectToAction("DeactivateOldCustomer", "Customers");
        }

        //get current time based on Singapore Standard Time 
        //SGT - UTC +8
        protected DateTime GetCurrentTime()
        {
            DateTime _localTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Singapore Standard Time"));
            _localTime = _localTime.Date;

            return _localTime;
        }
        
        //Ajax - Table Result 
        //Get the list of customers containing the search string,
        //if search is empty, return all actve items
        //Param : search = search string
        //        status = customer list string
        public string TableResult(string search, string status, string sort)
        {
            //get lit of customers
            List<CustomerList> custList = new List<CustomerList>();
            
            custList = custdb.generateCustomerList(search,status,sort);
           
            //convert list to json object
            return JsonConvert.SerializeObject(custList, Formatting.Indented);
        }


        #region Customer Social details

        //display list of categories assigned 
        //to the customer 
        private void PartialView_CustSocial(int id)
        {
            //get list of categories
            ViewBag.CustSocial = db.CustSocialAccs.Where(s=>s.CustomerId == id).ToList();
        }

        public string CreateCustSocial(int custId, string facebook, string skypeId, string viberId )
        {
            try
            {
                
                db.CustSocialAccs.Add(new CustSocialAcc
                {
                    Facebook = facebook,
                    Skype = skypeId,
                    Viber = viberId,
                    CustomerId = custId
                });
                db.SaveChanges();

                return "OK : ";
            }
            catch (Exception ex)
            {
                return "Cannot Process adding new social details.";
            }
        }
        
        // POST: Customers/Delete/5
        public ActionResult DeleteCustSocial(int id)
        {
            try
            {
                CustSocialAcc custSocial = db.CustSocialAccs.Find(id);
                var customerid = custSocial.CustomerId;
                db.CustSocialAccs.Remove(custSocial);
                db.SaveChanges();
                return RedirectToAction("Details", "Customers" , new { id = customerid });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Details", "Customers");
            }
        }


        public string EditCustSocial(int id, string facebook, string skypeId, string viberId)
        {
            try
            {
                CustSocialAcc social = db.CustSocialAccs.Find(id);
                social.Facebook = facebook;
                social.Skype = skypeId;
                social.Viber = viberId;

                db.Entry(social).State = EntityState.Modified;
                db.SaveChanges();

                return "OK : ";
            }
            catch (Exception ex)
            {
                return "Cannot Process adding new social details." + ex;
            }
        }
        #endregion
    }
}
