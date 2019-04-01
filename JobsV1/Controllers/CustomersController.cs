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
    public class CustomersController : Controller
    {
        private JobDBContainer db = new JobDBContainer();
        private List<SelectListItem> StatusList = new List<SelectListItem> {
                new SelectListItem { Value = "ACT", Text = "Active" },
                new SelectListItem { Value = "INC", Text = "Inactive" },
                new SelectListItem { Value = "BAD", Text = "Bad Account" }
                };

        // GET: Customers
        public ActionResult Index(string status)
        {

            var customerList = new List<Customer>();

            switch (status)
            {
                case "ACTIVE":
                    customerList = db.Customers.Where(s => s.Status == "ACT").ToList();
                     break;
                case "INACTIVE":
                    customerList = db.Customers.Where(s => s.Status == "INC").ToList();
                    break;
                case "BAD":
                    customerList = db.Customers.Where(s => s.Status == "BAD").ToList();
                    break;
                case "ALL":
                    customerList = db.Customers.ToList();
                    break;
                default:
                    customerList = db.Customers.Where(s => s.Status == "ACT").ToList();
                    break;
            }


            List<CustomerDetails> customerDetailList = new List<CustomerDetails>();
            foreach (var customer in customerList)
            {
                CustCategory custcategory = new CustCategory();
                CustCat custcat = new CustCat();
                CustEntity companyEntity = new CustEntity();
                CustEntMain company = new CustEntMain();

                try
                {
                    custcat = db.CustCats.Where(c => c.CustomerId == customer.Id).FirstOrDefault();
                    custcategory = db.CustCategories.Where(cat => cat.Id == custcat.CustCategoryId).FirstOrDefault();

                }
                catch (Exception ex)
                {
                    custcategory = new CustCategory
                    {
                        Id = 0,
                        Name = "Not Assigned",
                        iconPath = "Images/Customers/Category/unavailable-40.png"
                    };
                }

                try
                {
                    companyEntity = db.CustEntities.Where(ce => ce.CustomerId == customer.Id).FirstOrDefault();
                    company = db.CustEntMains.Where(co => co.Id == companyEntity.CustEntMainId).FirstOrDefault();

                }
                catch (Exception ex)
                {
                    company = new CustEntMain
                    {
                        Id = 0,
                        Name = "Not Assigned",
                        Address = "none",
                        Contact1 = "0",
                        Contact2 = "0",
                        iconPath = "Images/Customers/Category/unavailable-40.png"
                    };
                }


                customerDetailList.Add(new CustomerDetails
                {
                    Id = customer.Id,
                    Name = customer.Name,
                    Email = customer.Email,
                    Contact1 = customer.Contact1,
                    Contact2 = customer.Contact2,
                    Remarks = customer.Remarks,
                    Status = customer.Status,
                    JobID = customer.JobMains.Count(),
                    CustCategoryID = custcategory.Id,
                    CustCategoryIcon = custcategory.iconPath,
                    CustEntID = company.Id,
                    CustEntName = company.Name,
                    CustEntIconPath = "~/Images/Customers/Company/organization-40.png",
                    categories = getCategoriesList(customer.Id),
                    companies = getCompanyList(customer.Id)
                    
                    //end
                });

            }


            //return View(db.Customers.ToList());
            ViewBag.status = status;

            return View(customerDetailList);


        }

        private List<CustCategory> getCategoriesList(int id) {

            //PartialView for Details of the Customer
            List<CustCategory> categoryDetails = new List<CustCategory>();

            //error
            var categoryList = db.CustCats.Where(c => c.CustomerId == id).ToList();

            if (categoryList == null)
            {

                categoryDetails.Add(new CustCategory
                {
                    Id = 0,
                    iconPath = "Images/Customers/Category/unavailable-40.png",
                    Name = "not assigned"
                });

            }
            else
            {

                foreach (var cat in categoryList)
                {
                    switch (cat.CustCategory.Name) {
                        case "PRIORITY":
                            cat.CustCategory.iconPath = "Images/Customers/Category/star-filled-40.png";
                            break;
                        case "ACTIVE":
                            cat.CustCategory.iconPath = "Images/Customers/Category/Active-30.png";
                            break;
                        case "SUSPENDED":
                            cat.CustCategory.iconPath = "Images/Customers/Category/suspended-64.png";
                            break;
                        case "BAD ACCOUNT":
                            cat.CustCategory.iconPath = "Images/Customers/Category/cancel-40.png";
                            break;
                        case "CAR-RENTAL":
                            cat.CustCategory.iconPath = "Images/Customers/Category/car-40.png";
                            break;
                        case "TOUR":
                            cat.CustCategory.iconPath = "Images/Customers/Category/tour-40.png";
                            break;
                        case "CLIENT":
                            cat.CustCategory.iconPath = "Images/Customers/Category/client-40.png";
                            break;
                        case "COMPANY":
                            cat.CustCategory.iconPath = "Images/Customers/Company/organization-40.png";
                            break;
                    }



                    categoryDetails.Add(new CustCategory
                    {
                        Id = cat.CustCategory.Id,
                        iconPath = cat.CustCategory.iconPath,
                        Name = cat.CustCategory.Name

                    });
                }
            }

            return categoryDetails;
        }


        private List<CustEntMain> getCompanyList(int id)
        {

            //PartialView for Details of the Customer
            List<CustEntMain> CompanyList = new List<CustEntMain>();
            //error
            var CompanyRecord = db.CustEntities.Where(c => c.CustomerId == id).ToList();

            if (CompanyRecord == null)
            {

                CompanyList.Add(new CustEntMain
                {
                    Id = 0,
                    Name = "None",
                    Address = "None",
                    Contact1 = "None",
                    Contact2 = "None",
                    iconPath = "None"
                });

            }
            else
            {

                foreach (var record in CompanyRecord)
                {
                    CompanyList.Add(new CustEntMain
                    {
                        Id = record.CustEntMain.Id,
                        Name = record.CustEntMain.Name,
                        Address = record.CustEntMain.Address,
                        Contact1 = record.CustEntMain.Contact1,
                        Contact2 = record.CustEntMain.Contact2,
                        iconPath = "Images/Customers/Company/organization-40.png"
                    });

                }

            }

            ViewBag.companyList = CompanyList;
            ViewBag.CustomerID = id;


            return CompanyList;
        }

        // GET: Customers/Details/5
        public ActionResult Details(int? id, int? top, string sdate, string edate, string status)
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
            PartialView_Jobs(id,(int)top,sdate,edate,status);
            PartialView_Categories(id);
            PartialView_CustomerFiles(id);
            ViewBag.categoryList = db.CustCategories.ToList();
            ViewBag.custId = (int)id;

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

        private void PartialView_Companies(int? id)
        {

            //PartialView for Details of the Customer
            List<CustEntMain> CompanyList = new List<CustEntMain>();
            //error
            var CompanyRecord = db.CustEntities.Where(c => c.CustomerId == id).OrderByDescending(s=>s.Id).ToList();

            if (CompanyRecord == null)
            {
                CompanyList.Add(new CustEntMain
                {
                    Id = 0,
                    Name = "None",
                    Address = "None",
                    Contact1 = "None",
                    Contact2 = "None",
                    iconPath = "None"
                });
            }
            else
            {
                foreach (var record in CompanyRecord)
                {
                    CompanyList.Add(new CustEntMain
                    {
                        Id = record.CustEntMain.Id,
                        Name = record.CustEntMain.Name,
                        Address = record.CustEntMain.Address,
                        Contact1 = record.CustEntMain.Contact1,
                        Contact2 = record.CustEntMain.Contact2,
                        iconPath = record.CustEntMain.iconPath
                    });

                }

            }
            List<CustEntMain> List = new List<CustEntMain>();
            List = db.CustEntMains.ToList();
            ViewBag.companies = List;

            var Companies = db.CustEntities.Where(s => s.CustomerId == id).ToList();
           // var topLatestCompany = CompanyList.Where(s=> Companies.Contains(s.Id) || s.Id == 1).Select(s => s.Id).ToList();
          
            ViewBag.companyList = CompanyList.ToList();
            ViewBag.companiesPrev = Companies;
            ViewBag.CustomerID = id;

        }


        private void PartialView_Jobs(int? id, int? top, string sdate, string edate, string status)
        {

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

            //error
            var jobRecord = db.JobMains.Where(j => j.CustomerId == id).OrderByDescending(j => j.JobDate).ToList();
            
            //handle empty status
            if (status == null || status == "" || status == "ALL")
            {
                jobRecord = jobRecord.Where(j => j.JobDate.Date.CompareTo(StartDate) >= 0 && j.JobDate.Date.CompareTo(EndDate) <= 0).ToList();
            } else {

                jobRecord = jobRecord.Where(j => j.JobDate.Date.CompareTo(StartDate) >= 0 && j.JobDate.Date.CompareTo(EndDate) <= 0 && j.JobStatus.Status == status).ToList();
            }


            if (jobRecord == null)
            {

                jobList.Add(new CustomerJobDetails
                {
                    Id = 0,
                    JobDate = "7/24/2018",
                    Description = "none",
                    AgreedAmt = "0",
                    NoOfDays = "0",
                    NoOfPax = "0",
                    StatusRemarks = "none"
                });

            }
            else
            {

                foreach (var record in jobRecord)
                {
                    jobList.Add(new CustomerJobDetails
                    {

                        Id = record.Id,
                        JobDate = record.JobDate.ToString(),
                        Description = record.Description,
                        AgreedAmt = record.AgreedAmt.ToString(),
                        NoOfDays = record.NoOfDays.ToString(),
                        NoOfPax = record.NoOfPax.ToString(),
                        StatusRemarks = record.JobStatus.Status

                    });

                }

            }

            ViewBag.jobList = jobList.Take(topFilter).ToList();

        }


        private void PartialView_Categories(int? id)
        {

            ViewBag.categoryDetails = getCategoriesList((int)id);

            ViewBag.categoryList = db.CustCategories.ToList();
        }

        private void PartialView_CustomerFiles(int? id)
        {
            
            //PartialView for Details of the Customer
           List<CustFiles> FilesList = new List<CustFiles>();

            //error
            var customerFiles = db.CustFiles.Where(c => c.CustomerId == id).ToList();

            if (customerFiles == null)
            {
                FilesList.Add(new CustFiles
                {
                    Id = 0,
                    CustomerId = 0,
                    Path = "none",
                    Desc = "none",
                    Folder = "none",
                    Remarks = "",

                });

            }
            else
            {
                foreach (var file in customerFiles)
                {
                    FilesList.Add(new CustFiles
                    {
                        Id = file.Id,
                        CustomerId = file.CustomerId,
                        Path = file.Path,
                        Desc = file.Desc,
                        Folder = file.Folder,
                        Remarks = file.Remarks,

                    });
                }
            }
            ViewBag.fileList = FilesList;
        }

        public ActionResult addCompanyCat(int companyId, int userid)
        {
            if (companyId > 1)
            {
                db.CustEntities.Add(new CustEntity
                {
                    CustEntMainId = companyId,
                    CustomerId = userid
                });

                db.SaveChanges();
                return RedirectToAction("Details", "Customers", new { id = userid });
            }
            else
            {   //create new company
                return RedirectToAction("CompanyCreate", "Customers", new { id = userid });
            }
        }


        public ActionResult DiActivateOldCustomer()
        {
            var datetoday = GetCurrentTime().Date.AddDays(-360);
            //get customer id with jobs before 360 days from today
            var latestJobs = db.JobMains.Where(j => j.JobDate.CompareTo(datetoday) < 0).Select(s=>s.CustomerId);
            //get customers with status NO STATUS or ACTIVE from prev list
            var allCustomers = db.Customers.Where(s=> (s.Status == "" || s.Status == "ACT" ) && latestJobs.Contains(s.Id)).ToList();

            //get customers with jobs 
            var customersWithJobs = db.JobMains.Where(j => j.CustomerId > 0).Select(s => s.CustomerId);
            //get customers  not in the list of customers with jobs
            List<Customer> noJobCustomer = db.Customers.Where(s => (s.Status == "" || s.Status == "ACT") && s.Id != 1 && !customersWithJobs.Contains(s.Id)).ToList();

            //merge two list
            allCustomers.AddRange(noJobCustomer);

            return View(allCustomers);
        }


        public ActionResult DiActivateAll()
        {
            var datetoday = GetCurrentTime().Date.AddDays(-360);
            //get customers with jobs before 360 days from today
            var latestJobs = db.JobMains.Where(j => j.JobDate.CompareTo(datetoday) < 0 ).Select(s => s.CustomerId);

            var allCustomers = db.Customers.Where(s => (s.Status == "" || s.Status == "ACT") && latestJobs.Contains(s.Id)).ToList();

            //get customers with no jobs
            List<Customer> noJobCustomer = db.Customers.Where(s => s.JobMains == null).ToList();

            //merge two list
            allCustomers.AddRange(noJobCustomer);

            foreach (var customer in allCustomers)
            {
                customer.Status = "INC";    //diactivate customer
                db.Entry(customer).State = EntityState.Modified;
                db.SaveChanges();
            }

            return RedirectToAction("DiActivateOldCustomer", "Customers");
        }

        protected DateTime GetCurrentTime()
        {
            DateTime _localTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Singapore Standard Time"));
            _localTime = _localTime.Date;

            return _localTime;
        }

    }
}
