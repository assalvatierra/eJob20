﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using JobsV1.Models;
using JobsV1.Models.Class;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;

namespace JobsV1.Controllers
{
    public class CustEntMainsController : Controller
    {
        private JobDBContainer db = new JobDBContainer();
        private SalesLeadClass slc = new SalesLeadClass();
        private CompanyClass comdb = new CompanyClass();
        private DBClasses dbclasses = new DBClasses();
        private DateClass dt = new DateClass();
        private JobVehicleClass jvc = new JobVehicleClass();

        private List<SelectListItem> StatusList = new List<SelectListItem> {
                new SelectListItem { Value = "PRI", Text = "Priority" },
                new SelectListItem { Value = "ACC", Text = "Accredited" },
                new SelectListItem { Value = "ACP", Text = "Accreditation on Process" },
                new SelectListItem { Value = "BIL", Text = "Billing/Terms" },
                new SelectListItem { Value = "ACT", Text = "Active" },
                new SelectListItem { Value = "INC", Text = "Inactive" },
                new SelectListItem { Value = "BAD", Text = "Bad Account" },
                new SelectListItem { Value = "SUS", Text = "Suspended" }
                };

        private List<SelectListItem> ActivityStatus = new List<SelectListItem> {
                new SelectListItem { Value = "Open", Text = "Open" },
                new SelectListItem { Value = "For Client Comment", Text = "For Client Comment" },
                new SelectListItem { Value = "Awarded", Text = "Awarded" },
                new SelectListItem { Value = "Close", Text = "Close" }
                };
        
        private List<SelectListItem> Exclusive = new List<SelectListItem> {
                new SelectListItem { Value = "PUBLIC", Text = "Public" },
                new SelectListItem { Value = "EXCLUSIVE", Text = "Exclusive" }
                };

        public ActionResult Index()
        {
            ViewBag.IsAdmin = User.IsInRole("Admin");

            var companies = db.CustEntMains.ToList();
            return View(companies);
        }

        //Ajax - Table Result 
        //Get the list of company containing the search string,
        //if search is empty, return all active items
        //Param : search = search string
        //        status = company list string
        public string TableResult(string search, string searchCat,string status, string sort)
        {
            //get lit of customers
            List<cCompanyList> custList = new List<cCompanyList>();
            var user = HttpContext.User.Identity.Name;

            //handle user roles
            if (User.IsInRole("Admin"))
            {
                custList = comdb.generateCompanyList(search, searchCat, status, sort, "admin");
            }
            else
            {
                custList = comdb.generateCompanyList(search, searchCat, status, sort, user);
            }

            //custList = comdb.generateCompanyList(search, searchCat, status, sort, user);

            //convert list to json object
            return JsonConvert.SerializeObject(custList, Formatting.Indented);
        }

        // GET: CustEntMains/Details/5
        public ActionResult Details(int? id, int? top, string sdate, string edate, string status)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            CustEntMain custEntMain = db.CustEntMains.Find(id);
            if (custEntMain == null)
            {
                return HttpNotFound();
            }

            if (top == null)
            {
                top = 30;
            }

            //get logged user account
            var user = HttpContext.User.Identity.Name;

            //check previlages
            var isAdmin = User.IsInRole("Admin");
            var isAssigned = custEntMain.AssignedTo == user ? true : false;

            //check jobcount
            var jobcount = db.JobEntMains.Where(s => s.CustEntMainId == id).Count();

            //contacts 
            var companyContactEntity = db.CustEntities.Where(c => c.CustEntMainId == id).Select(c => c.CustomerId).ToList();

            //ViewBags
            ViewBag.CustomerList = db.Customers.Where(s => s.Status == "ACT").ToList() ?? new List<Customer>();
            ViewBag.CompanyJobs = getJobList(id,top,sdate,edate,status);
            ViewBag.SalesLeads = slc.getCompanyLeads((int)id);
            ViewBag.categories = db.CustCategories.ToList();
            ViewBag.CityId = new SelectList(db.Cities.OrderBy(c => c.Name).OrderByDescending(s => s.Name).ToList(), "Id", "Name", custEntMain.CityId);
            ViewBag.City = db.Cities.Find(custEntMain.CityId) != null ? db.Cities.Find(custEntMain.CityId).Name : "NA";
            ViewBag.ContactList = new SelectList(db.Customers.Where(c=>c.Status != "INC").OrderBy(s=>s.Name).ToList(), "Id", "Name",1);
            ViewBag.Documents = GetDocumentList((int)id);
            ViewBag.CustDocuments = db.CustEntDocuments.Where(c=>c.CustEntMainId == id).ToList();
            ViewBag.CompanyId = id;
            ViewBag.isAllowedHistory = isAdmin || isAssigned ? true : false ;
            ViewBag.IsAdmin = isAdmin;
            ViewBag.HaveJob = jobcount != 0 ? true : false;
            ViewBag.CustomerVehicles = db.Vehicles.Where(v => v.CustEntMainId == id).ToList();
            ViewBag.VehicleModelList = db.VehicleModels.ToList();
            ViewBag.CompanyContacts = db.Customers.Where(c => companyContactEntity.Contains(c.Id)).ToList();
            ViewBag.SiteConfig = ConfigurationManager.AppSettings["SiteConfig"].ToString();

            custEntMain.AssignedTo = comdb.removeSpecialChar(custEntMain.AssignedTo);

            return View(custEntMain);
        }

        private IEnumerable<SupDocument> GetDocumentList(int id)
        {
            //get id lis of current documents of the company
            var custEntDocs = db.CustEntDocuments.Where(c => c.CustEntMainId == id).Select(c => c.SupDocumentId).ToList();

            //current documents
            var custEntDocList = db.SupDocuments.Where(c => custEntDocs.Contains(c.Id)).ToList();

            //complete list of documents
            var completeDocs = db.SupDocuments.ToList();

            //get documents not added
            var doclist = completeDocs.Except(custEntDocList);

            return doclist;

        }

        public Boolean checkifAdmin()
        {
            switch (HttpContext.User.Identity.Name)
            {
                case "aldrin@solid-steel-supply.com":
                case "mario@solid-steel-supply.com":
                case "assalvatierra@gmail.com":
                case "jahdielvillosa@gmail.com":
                    return true;
                default:
                    return false;

            }
        }
        
        public List<CompanyJobsList> getJobList(int? id, int? top, string sdate, string edate, string status) {

            int topFilter = (int)top;
            //PartialView for Details of the Customer
            List<CustomerJobDetails> jobList = new List<CustomerJobDetails>();

            DateTime StartDate = DateTime.Today;
            DateTime EndDate = DateTime.Today;

            //handle empty dates
            if (sdate != null && edate != null)
            {
                StartDate = DateTime.Parse(sdate).Date;
                EndDate = DateTime.Parse(edate).Date;
            }

            //get company
            var company = db.CustEntities.Where(c => c.CustEntMainId == id).Select(c => c.CustomerId);

            //get customers with companies like this
            var customers = db.Customers.Where(c => company.Contains(c.Id)).Select(c => c.Id);

            //get company 
            var jobRecord = db.JobMains.Where(j => customers.Contains(j.CustomerId)).OrderByDescending(j => j.JobDate).ToList();
            
            //handle empty status
            if (status == null || status == "" || status == "ALL")
            {
                jobRecord = jobRecord.Where(j => j.JobDate.Date.CompareTo(StartDate) >= 0 && j.JobDate.Date.CompareTo(EndDate) <= 0).ToList();
            }
            else
            {
                jobRecord = jobRecord.Where(j => j.JobDate.Date.CompareTo(StartDate) >= 0 && j.JobDate.Date.CompareTo(EndDate) <= 0 && j.JobStatus.Status == status).ToList();
            }
            
            var filteredRecords =  jobRecord.Take(topFilter).ToList();
            List<CompanyJobsList> companyJobs = new List<CompanyJobsList>();

            foreach (var items in filteredRecords)
            {
                companyJobs.Add(new CompanyJobsList {
                    Date = items.JobDate,
                    Description = items.Description,
                    Customer = items.Customer.Name,
                    Status = items.JobStatus.Status,
                    Id = items.Id,
                    Amount = getJobTotal(items.Id),
                    AssignedTo = items.AssignedTo
                });
            }

            return companyJobs;
        }

        private decimal getJobTotal(int jobId)
        {
            decimal totalAmt = 0;
            var jobsvc = db.JobServices.Where(s => s.JobMainId == jobId).ToList();

            foreach (var item in jobsvc)
            {
                totalAmt += item.ActualAmt != null ? (decimal)item.ActualAmt : 0;
            }

            return totalAmt;
        }

        // GET: CustEntMains/Create
        public ActionResult Create()
        {
           
            CustEntMain main = new CustEntMain();

            main.iconPath = "Images/Customers/Company/organization-40.png"; //default logo 
            ViewBag.CityId = new SelectList(db.Cities.OrderBy(c=>c.Name).ToList(), "Id", "Name");
            ViewBag.Status = new SelectList(StatusList, "value", "text");
            ViewBag.AssignedTo = new SelectList(dbclasses.getUsers_wdException(), "UserName", "UserName");
            ViewBag.Exclusive = new SelectList(Exclusive, "value", "text");
            ViewBag.CustEntAccountTypeId = new SelectList(db.CustEntAccountTypes, "Id", "Name");

            return View(main);
        }

        // POST: CustEntMains/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Address,Contact1,Contact2,Mobile,iconPath,CityId,Website,Status,AssignedTo,Code,Exclusive,CustEntAccountTypeId")] CustEntMain custEntMain, int? id)
        {
            if (ModelState.IsValid)
            {
                if (CompanyCreateValidation(custEntMain))
                {

                    var DuplicateCount = db.CustEntMains.Where(s => custEntMain.Name.Contains(s.Name)).ToList().Count();

                    if (DuplicateCount == 0)
                    {
                        //create company
                        db.CustEntMains.Add(custEntMain);
                        db.SaveChanges();

                        //update company code
                        UpdateCompanyCode(custEntMain.Id);
                    }
                    else
                    {
                        ViewBag.Msg = "Customer Name already exist.";

                        ViewBag.CityId = new SelectList(db.Cities.OrderBy(c => c.Name).ToList(), "Id", "Name");
                        ViewBag.Status = new SelectList(StatusList, "value", "text");
                        ViewBag.AssignedTo = new SelectList(dbclasses.getUsers_wdException(), "UserName", "UserName");
                        ViewBag.Exclusive = new SelectList(Exclusive, "value", "text");
                        ViewBag.CustEntAccountTypeId = new SelectList(db.CustEntAccountTypes, "Id", "Name");

                        return View(custEntMain);
                    }

                    if (id != null)
                    {
                        //save new company to customer
                        CustEntity company = new CustEntity();
                        company.CustEntMainId = custEntMain.Id;
                        company.CustomerId = (int)id;
                        db.CustEntities.Add(company); 
                        db.SaveChanges();

                        return RedirectToAction("Details", "CustEntMains", new { id = custEntMain.Id });
                    }

                    AddAssignedRecords(custEntMain.Id, custEntMain.AssignedTo);

                    return RedirectToAction("Details", "CustEntMains", new { id = custEntMain.Id });
                }
            }
            ViewBag.CityId = new SelectList(db.Cities.OrderBy(c => c.Name).ToList(), "Id", "Name");
            ViewBag.Status = new SelectList(StatusList, "value", "text");
            ViewBag.AssignedTo = new SelectList(dbclasses.getUsers_wdException(), "UserName", "UserName");
            ViewBag.Exclusive = new SelectList(Exclusive, "value", "text");
            ViewBag.CustEntAccountTypeId = new SelectList(db.CustEntAccountTypes, "Id", "Name", custEntMain.CustEntAccountTypeId);

            return View(custEntMain);
        }

        public bool UpdateCompanyCode(int id)
        {
            try
            {
                iCustEntCode custEntCode;
                var companyCode = "";

                var company = db.CustEntMains.Find(id);
                if (company == null)
                    return false;

                var accountSysCode = db.CustEntAccountTypes.Find(company.CustEntAccountTypeId).SysCode;
                switch (accountSysCode)
                {
                    case "TYPE01":
                        custEntCode = new CustEntCode_AutoCare();
                        companyCode = custEntCode.GenerateCode(id);
                        break;
                    case "NOGEN":
                        custEntCode = new CustEntCode_Default();
                        companyCode = custEntCode.GenerateCode(id);
                        break;
                    default:
                        companyCode = null;
                        break;
                }

                if (companyCode.IsNullOrWhiteSpace())
                    return true;

                company.Code = companyCode;

                //save company code changes
                db.Entry(company).State = EntityState.Modified;
                db.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }


        public bool CompanyCreateValidation(CustEntMain custEntMain)
        {
            bool isValid = true;

            if (custEntMain.Name.IsNullOrWhiteSpace())
            {
                ModelState.AddModelError("Name", "Invalid Name");
                isValid = false;
            }

            return isValid;
        }


        // GET: CustEntMains/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustEntMain custEntMain = db.CustEntMains.Find(id);
            if (custEntMain == null)
            {
                return HttpNotFound();
            }

            ViewBag.CityId = new SelectList(db.Cities.OrderBy(c => c.Name).ToList(), "Id", "Name", custEntMain.CityId);
            ViewBag.Status = new SelectList(StatusList, "value", "text", custEntMain.Status);
            ViewBag.AssignedTo = new SelectList(dbclasses.getUsers_wdException(), "UserName", "UserName", custEntMain.AssignedTo);
            ViewBag.Exclusive = new SelectList(Exclusive, "value", "text", custEntMain.Exclusive);
            ViewBag.CustEntAccountTypeId = new SelectList(db.CustEntAccountTypes, "Id", "Name", custEntMain.CustEntAccountTypeId);
            return View(custEntMain);
        }

        // POST: CustEntMains/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Address,Contact1,Contact2,Mobile,iconPath,CityId,Website,Status,AssignedTo,Code,Exclusive,CustEntAccountTypeId")] CustEntMain custEntMain)
        {
            if (ModelState.IsValid)
            {
                if (CompanyCreateValidation(custEntMain))
                {
                    db.Entry(custEntMain).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Details", new { id = custEntMain.Id });
                }
            }
            ViewBag.CityId = new SelectList(db.Cities.OrderBy(c => c.Name).ToList(), "Id", "Name", custEntMain.CityId);
            ViewBag.Status = new SelectList(StatusList, "value", "text",custEntMain.Status);
            ViewBag.AssignedTo = new SelectList(dbclasses.getUsers_wdException(), "UserName", "UserName", custEntMain.AssignedTo);
            ViewBag.Exclusive = new SelectList(Exclusive, "value", "text");
            ViewBag.CustEntAccountTypeId = new SelectList(db.CustEntAccountTypes, "Id", "Name", custEntMain.CustEntAccountTypeId);
            return View(custEntMain);
        }

        // GET: CustEntMains/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustEntMain custEntMain = db.CustEntMains.Find(id);
            if (custEntMain == null)
            {
                return HttpNotFound();
            }
            return View(custEntMain);
        }

        // POST: CustEntMains/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CustEntMain custEntMain = db.CustEntMains.Find(id);
            db.CustEntMains.Remove(custEntMain);
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

        #region Address 
        [HttpPost]
        public bool CreateAddress(int custEntMainId, string line1, string line2, string line3, string line4, string isBilling, string isPrimary )
        {
            if (custEntMainId != null)
            {

                CustEntAddress address = new CustEntAddress();
                address.CustEntMainId = custEntMainId;
                address.Line1 = line1;
                address.Line2 = line2;
                address.Line3 = line3;
                address.Line4 = line4;
                address.isBilling = isBilling == "true" ? true : false;
                address.isPrimary = isPrimary == "true" ? true : false;

                db.CustEntAddresses.Add(address);
                db.SaveChanges();

                return true;
            }

            return false;

        }


        public string EditAddress( int id, string line1, string line2, string line3, string line4, string line5,  bool isPrimary , bool isBilling)
        {
            CustEntAddress editAddress = db.CustEntAddresses.Find(id);
            editAddress.Line1 = line1;
            editAddress.Line2 = line2;
            editAddress.Line3 = line3;
            editAddress.Line4 = line4;
            editAddress.Line5 = line5; 

            db.Entry(editAddress).State = EntityState.Modified;
            db.SaveChanges();
            
            return "200";
        }


        // GET: CustEntAddresses/Delete/5
        public string DeleteAddress(int? id)
        {
            if (id == null)
            {
                return "500";
            }

            CustEntAddress custEntAddress = db.CustEntAddresses.Find(id);
            if (custEntAddress == null)
            {
                return "500";
            }

            db.CustEntAddresses.Remove(custEntAddress);
            db.SaveChanges();

            return "200";
        }

        #endregion

        #region Clauses 

        // GET: CustEntMains/Create
        public ActionResult CreateClause(int companyId)
        {
            DateClass today = new DateClass();

            CustEntClauses clause = new CustEntClauses();
            ViewBag.CustEntMainId = new SelectList(db.CustEntMains, "Id", "Name", companyId);
            clause.CustEntMainId = companyId;
            clause.DtEncoded = today.GetCurrentDateTime();
            clause.ValidStart = today.GetCurrentDateTime();
            clause.ValidEnd =  today.GetCurrentDateTime().AddDays(1);
            clause.Desc1 = "";
            clause.Desc2 = "";
            clause.Desc3 = "";
            clause.EncodedBy = HttpContext.User.Identity.Name;
            return View(clause);
        }

        // POST: CustEntMains/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateClause([Bind(Include = "Id,CustEntMainId,Title,ValidStart,ValidEnd,Desc1,Desc2,Desc3,DtEncoded,EncodedBy")] CustEntClauses custEntClauses)
        {
            if (ModelState.IsValid)
            {
                db.CustEntClauses.Add(custEntClauses);
                db.SaveChanges();

                return RedirectToAction("Details", "CustEntMains", new { id = custEntClauses.CustEntMainId });
            }
            return View(custEntClauses);
        }

        public string EditClause(int id, string title, string startdate, string enddate, string desc1, string desc2, string desc3)
        {
            try
            {

                CustEntClauses editClause = db.CustEntClauses.Find(id);
                editClause.Title = title;
                editClause.ValidStart = DateTime.Parse( startdate);
                editClause.ValidEnd = DateTime.Parse( enddate);
                editClause.Desc1 = desc1;
                editClause.Desc2 = desc2;
                editClause.Desc3 = desc3;

                db.Entry(editClause).State = EntityState.Modified;
                db.SaveChanges();

                return "200";
            }
            catch (Exception ex)
            {
                return "500";
            }
        }

        // GET: CustEntAddresses/Delete/5
        public string DeleteClause(int? id)
        {
            if (id == null)
            {
                return "500"; //error
            }

            CustEntClauses custEntClause = db.CustEntClauses.Find(id);
            if (custEntClause == null)
            {
                return "500"; //error
            }

            db.CustEntClauses.Remove(custEntClause);
            db.SaveChanges();

            return "200"; // ok
        }

        #endregion

        #region Category

        // POST: CustCats/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.

        public ActionResult addCategory(int catId,int companyId )
        {
                CustEntCat companyCategory = new CustEntCat();
                companyCategory.CustCategoryId = catId;
                companyCategory.CustEntMainId = companyId;

                db.CustEntCats.Add(companyCategory);
                db.SaveChanges();
               return RedirectToAction("Details",new { id = companyId });
          
        }

        public ActionResult DeleteCategory(int id)
        {
            CustEntCat companyCat = db.CustEntCats.Find(id);
            var companyId = companyCat.CustEntMainId;
            db.CustEntCats.Remove(companyCat);

            db.SaveChanges();

            return RedirectToAction("Details", new { id = companyId });
        }
        #endregion

        #region company Contact
        public ActionResult DeleteContact(int id)
        {
            CustEntity custEnt = db.CustEntities.Find(id);
            var companyId = custEnt.CustEntMainId;
            db.CustEntities.Remove(custEnt);

            db.SaveChanges();

            return RedirectToAction("Details", new { id = companyId });
        }
        
        public bool AddContact(int? companyId, int? customerId, string name, string position, string email, string tel, string mobile, string social, string status)
        {
            try
            {
                if(companyId == null || customerId == null || name.IsNullOrWhiteSpace())
                {
                    return false;
                }

                //NEW CUSTOMER
                if (customerId == 1)
                {
                    //create new customer based on company id
                    Customer newCust = new Customer();
                    newCust.Name = name;
                    newCust.Email = email;
                    newCust.Contact1 = tel;
                    newCust.Contact2 = mobile;
                    newCust.Status = status;

                    db.Customers.Add(newCust);
                    db.SaveChanges();

                    customerId = newCust.Id;
                }
                else
                {
                    //existing customer
                    Customer cust = db.Customers.Find(customerId);

                    //update customer
                    cust.Contact1 = tel;
                    cust.Contact2 = mobile;
                    cust.Email = email;
                    

                    db.Entry(cust).State = EntityState.Modified;
                    db.SaveChanges();
                }
                
                //create new connection from customer and company
                CustEntity custEnt = new CustEntity();
                custEnt.CustEntMainId = (int)companyId;
                custEnt.CustomerId = (int)customerId;
                custEnt.Position = position;

                db.CustEntities.Add(custEnt);
                db.SaveChanges();

                //create customer social account
                createSocialAccount((int)customerId, social);
                
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
                return false;
            }
        }


        //check if Customer Name have duplicate
        public bool CheckNameDuplicate(string custName)
        {
            var custDuplicate = db.Customers.Where(s => custName.Contains(s.Name)).ToList().Count();

            if (custDuplicate != 0)
            {
                //has duplicate
                return true;
            }
            else
            {
                //no duplicate
                return false;
            }
        }


        private void createSocialAccount(int custId, string account)
        {
            CustSocialAcc social = new CustSocialAcc();
            social.CustomerId = custId;
            social.Facebook = account;
            social.Skype = "";
            social.Viber = "";
            
            db.CustSocialAccs.Add(social);
            db.SaveChanges();
        }

        private void updateStaffSocial(int customerId, int companyId, string socialAcc)
        {
            if (db.CustSocialAccs.Where(c => c.CustomerId == customerId).OrderByDescending(c => c.Id).FirstOrDefault() != null)
            {
                CustSocialAcc social = db.CustSocialAccs.Where(c => c.CustomerId == customerId).OrderByDescending(c => c.Id).FirstOrDefault();
                social.Facebook = socialAcc;

                db.Entry(social).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        public string getCustomerAccount(int? id)
        {
            if (id != 0)
            { 
                //get lit of customers
                Customer customer = db.Customers.Find(id);

                //build for json
                cCompanyContact contact = new cCompanyContact();
                contact.CustomerId = customer.Id;
                contact.Name = customer.Name;
                contact.Position = "";
                contact.Email = customer.Email;
                contact.Telephone = customer.Contact1;
                contact.Mobile = customer.Contact2;
                contact.SocialMedia = "";

                //check if customer has company position
                if (db.CustEntities.Where(c => c.CustomerId == customer.Id).OrderByDescending(s => s.Id).FirstOrDefault() != null)
                {
                    var companyContact = db.CustEntities.Where(c => c.CustomerId == customer.Id).OrderByDescending(s => s.Id).FirstOrDefault();
                    if (companyContact.Position != null)
                    {
                        contact.Position = companyContact.Position;
                    }
                }

                //check if customer has social media
                if (db.CustSocialAccs.Where(c => c.CustomerId == customer.Id).OrderByDescending(s => s.Id).FirstOrDefault() != null)
                {
                    var socialAcc = db.CustSocialAccs.Where(c => c.CustomerId == customer.Id).OrderByDescending(s => s.Id).FirstOrDefault();

                    contact.SocialMedia = socialAcc.Facebook;
                }

                
                //convert list to json object
                return JsonConvert.SerializeObject(contact, Formatting.Indented);
            }
            return "Error";
           
        }
        #endregion

        #region Assigned Records

        //List of Records from company
        public ActionResult AssignedRecords(int companyId)
        {
            var assignRecords = db.CustEntAssigns.Where(s => s.CustEntMainId == companyId).ToList();
            ViewBag.CompanyName = db.CustEntMains.Find(companyId).Name.ToString();
            ViewBag.companyId = companyId;
            ViewBag.Assigned = new SelectList(dbclasses.getUsers_wdException(), "UserName", "UserName");
            ViewBag.CustEntMainId = new SelectList(db.CustEntMains, "Id", "Name", companyId);
            ViewBag.ActivityStatus = new SelectList(ActivityStatus, "value", "text");
            ViewBag.Date = dt.GetCurrentDateTime();
            return View(assignRecords);
        }
        
        public string AssignedRecordsCreate( string Assigned, string Remarks, int CompanyId, string Date )
        {
            try
            {

                CustEntAssign assign = new CustEntAssign();
                assign.Assigned = Assigned;
                assign.Remarks = Remarks;
                assign.CustEntMainId = CompanyId;
                assign.Date = DateTime.Parse(Date);

                db.CustEntAssigns.Add(assign);
                db.SaveChanges();

                //update assigned in company
                UpdateAssigned(CompanyId, Assigned);

                return "OK";
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
            //return RedirectToAction("AssignedRecords" , new { companyId = CompanyId });
            //ViewBag.ActivityStatus = new SelectList(ActivityStatus, "value", "text", Status);


        }

        //Add Assigned on the Record History after Create Company
        // Param
        // companyId = CompanyId
        public void AddAssignedRecords(int companyId, string Assigned)
        {
            //add new assigned 
            CustEntAssign assigned = new CustEntAssign();
            assigned.CustEntMainId = companyId;
            assigned.Assigned = Assigned;
            assigned.Date = dt.GetCurrentDateTime();
            db.CustEntAssigns.Add(assigned);
            db.SaveChanges();
            
        }

        public ActionResult DeleteAssigned(int id)
        {
            CustEntAssign custComAssign = db.CustEntAssigns.Find(id);
            var companyId = custComAssign.CustEntMainId;
            var Assigned = custComAssign.Assigned;

            db.CustEntAssigns.Remove(custComAssign);
            db.SaveChanges();

            //update assigned in company
            UpdateAssigned(companyId, Assigned);

            return RedirectToAction("AssignedRecords" ,  new { companyId = companyId });
        }

        //Update Company Assigned Email after Adding new company record
        private string UpdateAssigned(int? companyId , string AssignedEmail)
        {
            try
            {
                if (companyId != null)
                {

                    if (db.CustEntAssigns.Where(c => c.CustEntMainId == companyId).FirstOrDefault() != null)
                    {
                        //get latest record 
                        CustEntAssign custComAssign = db.CustEntAssigns.Where(c=>c.CustEntMainId == companyId).OrderByDescending(s=>s.Date).FirstOrDefault();
                      
                        //assign in company 
                        CustEntMain custEntMain = db.CustEntMains.Find(companyId);
                        custEntMain.AssignedTo = custComAssign.Assigned;
                        db.Entry(custEntMain).State = EntityState.Modified;

                        db.SaveChanges();
                        return "OK";
                    }
                }
                    return "BAD";
            }
            catch(Exception ex){
                return ex.ToString();
            }
        }


        public string AssignedRecordsEdit(int? id, int companyId, string AssignedTo, string Date, string Remarks)
        {
            try
            {

                if (id != null)
                {

                    CustEntAssign custComAssign = db.CustEntAssigns.Find(id);
                    custComAssign.Assigned = AssignedTo.ToString();
                    custComAssign.Date = DateTime.Parse(Date);
                    custComAssign.Remarks = Remarks;
                    custComAssign.CustEntMainId = companyId;
                    ViewBag.Status = new SelectList(ActivityStatus, "value", "text");

                    db.Entry(custComAssign).State = EntityState.Modified;
                    db.SaveChanges();

                    return "OK";

                }
                return "ERROR";
                }
            catch (Exception ex)
            {
                return ex.ToString();
            }

        }
        #endregion
        
        #region Documents

        public ActionResult AddDocuments(int comId, int docId)
        {
            if (comId != 0 && docId != 0)
            {
                CustEntDocuments companyDocs = new CustEntDocuments();
                companyDocs.CustEntMainId = comId;
                companyDocs.SupDocumentId = docId;

                db.CustEntDocuments.Add(companyDocs);
                db.SaveChanges();
                return RedirectToAction("Details", new { id = comId });
            }
            return RedirectToAction("Details", new { id = comId });
        }

        public string DeleteDocument(int id)
        {
            try
            {
                CustEntDocuments custEntDocs = db.CustEntDocuments.Find(id);
                db.CustEntDocuments.Remove(custEntDocs);
                db.SaveChanges();
                return "OK";
            }
            catch (Exception ex)
            {
                return "Error";
            }
        }

        #endregion


        #region Vehicles 
        [HttpPost]
        public bool AddCustomerVehicle(int vehicleModelId, string yearModel, string plateNo, string conduction, string engineNo, string chassisNo, string color, int customerId, int custEntMainId, string remarks)
        {
            try
            {
                Vehicle vehicle = new Vehicle();

                vehicle.VehicleModelId = vehicleModelId;
                vehicle.YearModel = yearModel;
                vehicle.PlateNo = plateNo;
                vehicle.Conduction = conduction;
                vehicle.EngineNo = engineNo;
                vehicle.ChassisNo = chassisNo;
                vehicle.Color = color;
                vehicle.CustomerId = customerId;
                vehicle.CustEntMainId = custEntMainId;
                vehicle.Remarks = remarks;

                db.Vehicles.Add(vehicle);
                db.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        [HttpGet]
        public JsonResult GetVehicleDetails(int? id)
        {
            if (id == null)
                return null;
            var vehicle = db.Vehicles.Where(v => v.Id == id).Select(v => new {
                v.VehicleModelId,
                v.VehicleModel.Make,
                v.VehicleModel.VehicleBrand.Brand,
                v.VehicleModel.VehicleTransmission.Type,
                v.YearModel,
                v.PlateNo,
                v.Conduction,
                v.EngineNo,
                v.ChassisNo,
                v.Color,
                v.CustomerId,
                v.Customer.Name,
                v.CustEntMainId,
                v.Remarks,
                v.Id
            });

            return Json(vehicle, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public bool EditCustomerVehicle(int Id, int vehicleModelId, string yearModel, string plateNo, string conduction, string engineNo, string chassisNo, string color, int customerId, int custEntMainId, string remarks)
        {

            try
            {
                Vehicle vehicle = db.Vehicles.Find(Id);

                if (vehicle == null)
                    return false;

                vehicle.VehicleModelId = vehicleModelId;
                vehicle.YearModel = yearModel;
                vehicle.PlateNo = plateNo;
                vehicle.Conduction = conduction;
                vehicle.EngineNo = engineNo;
                vehicle.ChassisNo = chassisNo;
                vehicle.Color = color;
                vehicle.CustomerId = customerId;
                vehicle.CustEntMainId = custEntMainId;
                vehicle.Remarks = remarks;

                db.Entry(vehicle).State = EntityState.Modified;
                db.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        [HttpPost]
        public bool DeleteCustomerVehicle(int Id)
        {

            try
            {
                Vehicle vehicle = db.Vehicles.Find(Id);

                if (vehicle == null)
                    return false;

                db.Vehicles.Remove(vehicle);
                db.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public ActionResult VehicleServices(int? id, int? companyId)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }
            var vehicleServices = jvc.GetJobVehicleServices((int)id);

            //get vehicle details
            var vehicle = db.Vehicles.Find(id);
            string vehicleDetails = vehicle.VehicleModel.VehicleBrand.Brand + " " + vehicle.VehicleModel.Make + " " + vehicle.YearModel +
                " (" + vehicle.PlateNo + ")";

            ViewBag.VehicleDetails = vehicleDetails;
            ViewBag.CompanyId = companyId;
            ViewBag.Customer = vehicle.Customer.Name;
            ViewBag.Company = vehicle.CustEntMain.Name;

            return View(vehicleServices);
        }

        #endregion

    }
}
