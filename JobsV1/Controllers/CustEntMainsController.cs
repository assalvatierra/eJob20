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
    public class CustEntMainsController : Controller
    {
        private JobDBContainer db = new JobDBContainer();
        private SalesLeadClass slc = new SalesLeadClass();
        private CompanyClass comdb = new CompanyClass();
        private DBClasses dbclasses = new DBClasses();
        private DateClass dt = new DateClass();

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
                new SelectListItem { Value = "Others", Text = "Others" },
                new SelectListItem { Value = "Indicated Price", Text = "Indicated Price" },
                new SelectListItem { Value = "Bidding Only", Text = "Bidding Only" },
                new SelectListItem { Value = "Firm Inquiry", Text = "Firm Inquiry" },
                new SelectListItem { Value = "Buying Inquiry", Text = "Buying Inquiry" },
                new SelectListItem { Value = "Job Order", Text = "Job Order" }
                };
        public ActionResult Index()
        {
            var companies = db.CustEntMains.ToList();
            return View(companies);
        }

        //Ajax - Table Result 
        //Get the list of company containing the search string,
        //if search is empty, return all actve items
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
                custList = comdb.generateCompanyAdminList(search, searchCat, status, sort);
            }
            else
            {
                custList = comdb.generateCompanyList(search, searchCat, status, sort, user);
            }

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
            
            ViewBag.CompanyJobs = getJobList(id,top,sdate,edate,status);
            ViewBag.SalesLeads = slc.getCompanyLeads((int)id);
            ViewBag.categories = db.CustCategories.ToList();
            ViewBag.CityId = new SelectList(db.Cities.OrderBy(c => c.Name).ToList(), "Id", "Name", custEntMain.CityId);
            ViewBag.City = db.Cities.Find(custEntMain.CityId) != null ? db.Cities.Find(custEntMain.CityId).Name : "NA";
            ViewBag.ContactList = new SelectList(db.Customers.Where(c=>c.Status != "INC").ToList(), "Id", "Name");
            ViewBag.Documents = GetDocumentList((int)id);
            ViewBag.CustDocuments = db.CustEntDocuments.Where(c=>c.CustEntMainId == id).ToList();
            ViewBag.CompanyId = id;
            ViewBag.isAllowedHistory = checkifAdmin();

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

            return View(main);
        }

        // POST: CustEntMains/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Address,Contact1,Contact2,Mobile,iconPath,CityId,Website,Status,AssignedTo,Code")] CustEntMain custEntMain, int? id)
        {
            if (ModelState.IsValid)
            {
                
                    db.CustEntMains.Add(custEntMain);
                    db.SaveChanges();
                

                if (id != null)
                {
                    //save new company to customer
                    CustEntity company = new CustEntity();
                    company.CustEntMainId = custEntMain.Id;
                    company.CustomerId = (int)id;
                    db.CustEntities.Add(company); 
                    db.SaveChanges();

                    return RedirectToAction("Index", "CustEntMains", null);
                }

                AddAssignedRecords(custEntMain.Id, custEntMain.AssignedTo);

                return RedirectToAction("Index", "CustEntMains", null);
            }
            ViewBag.CityId = new SelectList(db.Cities.OrderBy(c => c.Name).ToList(), "Id", "Name");
            ViewBag.Status = new SelectList(StatusList, "value", "text");
            ViewBag.AssignedTo = new SelectList(dbclasses.getUsers_wdException(), "UserName", "UserName");

            return View(custEntMain);
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
            return View(custEntMain);
        }

        // POST: CustEntMains/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Address,Contact1,Contact2,Mobile,iconPath,CityId,Website,Status,AssignedTo,Code")] CustEntMain custEntMain)
        {
            if (ModelState.IsValid)
            {
                db.Entry(custEntMain).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", new { id = custEntMain.Id });
            }
            ViewBag.CityId = new SelectList(db.Cities.OrderBy(c => c.Name).ToList(), "Id", "Name", custEntMain.CityId);
            ViewBag.Status = new SelectList(StatusList, "value", "text",custEntMain.Status);
            ViewBag.AssignedTo = new SelectList(dbclasses.getUsers_wdException(), "UserName", "UserName", custEntMain.AssignedTo);
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

        #region Delete 

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
        
        public string AddContact(int companyId, int customerId, string name, string position, string email, string tel, string mobile, string social, string status)
        {
            try
            {

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
                custEnt.CustEntMainId = companyId;
                custEnt.CustomerId = customerId;
                custEnt.Position = position;

                db.CustEntities.Add(custEnt);
                db.SaveChanges();

                //create customer social account
                createSocialAccount(customerId, social);
                
                return "200";
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }


        //check if Customer Name have duplicate
        public bool HaveNameDuplicate(string custName)
        {
            var custDuplicate = db.Customers.Where(s => custName.Contains(s.Name)).ToList().Count();

            if (custDuplicate != 0)
            {
                //has duplicate
                return true;
            }
            else
            {
                //count = 0
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

    }
}
