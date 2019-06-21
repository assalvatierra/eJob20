using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using JobsV1.Models;

namespace JobsV1.Models
{
    public class CustomerList
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Contact1  { get; set; }
        public string Contact2  { get; set; }
        public string Company   { get; set; }
        public int    JobCount  { get; set; }
        public string Status    { get; set; }
    } 

    public class CustomerClass
    {

        private JobDBContainer db = new JobDBContainer();
        private DateClass dt = new DateClass();

        //get categories of the customers by Id on Partial View
        public List<CustCategory> getCategoriesList(int id)
        {

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
                    switch (cat.CustCategory.Name)
                    {
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
        
        //get companies associated to the customer using 
        //the customer id, then return the list of companies to the controller
        public  List<CustEntMain> getCustCompanies(int id)
        {
            List<CustEntMain> CompanyList = new List<CustEntMain>();
            //error
            var CompanyRecord = db.CustEntities.Where(c => c.CustomerId == id).OrderByDescending(s => s.Id).ToList();

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
            return CompanyList;
        }

        //get files of the customers by Id  on Partial View
        public List<CustFiles> getCustFiles(int custId)
        {
            List<CustFiles> FilesList = new List<CustFiles>();
            List<CustFiles> customerFiles;
            //error
            try
            {
                customerFiles = db.CustFiles.Where(c => c.CustomerId == custId).ToList();
           
                //handle empty customer files
                if (customerFiles == null)
                {
                    FilesList.Add(new CustFiles
                    {
                        Id = 0,
                        CustomerId = 0,
                        Path = "none",
                        Desc = "none",
                        Folder = "none",
                        Remarks = ""
                    });
                }
                else
                {
                    //add customer files to the list 
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
            } catch (Exception ex) { }

            return FilesList;
        }

        //get customer jobs on Partial View
        #region Customer jobs
        //get customer jobs and details
        public List<CustomerJobDetails> getCustomerJobList(int id, int top, string sdate, string edate, string status, string sortdate)
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
            var jobRecord = db.JobMains.Where(j => j.CustomerId == id).ToList();

            if (jobRecord != null)
            {
                if (!(String.IsNullOrEmpty(sortdate)))
                {
                    if (sortdate == "1")
                    {
                        jobRecord = jobRecord.OrderByDescending(j => j.JobDate).ToList();
                    }
                    else
                    {
                        jobRecord = jobRecord.OrderBy(j => j.JobDate).ToList();
                    }
                }
                //handle empty status
                if (status == null || status == "" || status == "ALL")
                {
                    jobRecord = jobRecord.Where(j => j.JobDate.Date.CompareTo(StartDate) >= 0 && j.JobDate.Date.CompareTo(EndDate) <= 0).ToList();
                }
                else
                {
                    jobRecord = jobRecord.Where(j => j.JobDate.Date.CompareTo(StartDate) >= 0 && j.JobDate.Date.CompareTo(EndDate) <= 0 && j.JobStatus.Status == status).ToList();
                }
            }


            //get customised jobList
            jobList = getJobList(jobRecord);

            return jobList.Take(topFilter).ToList();
        }
        
        //get list of Customer Job details and return it
        private List<CustomerJobDetails> getJobList( List<JobMain> jobRecord)
        {
            List<CustomerJobDetails> jobList = new List<CustomerJobDetails>();

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
                    var svcs = db.JobServices.Where(s => s.JobMainId == record.Id).ToList();
                    decimal totalAmount = 0;

                    foreach (var services in svcs)
                    {
                        totalAmount += services.ActualAmt != null ? (decimal)services.ActualAmt : 0;
                    }

                    jobList.Add(new CustomerJobDetails
                    {
                        Id = record.Id,
                        JobDate = record.JobDate.ToString(),
                        Description = record.Description,
                        AgreedAmt = record.AgreedAmt.ToString(),
                        NoOfDays = record.NoOfDays.ToString(),
                        NoOfPax = record.NoOfPax.ToString(),
                        StatusRemarks = record.JobStatus.Status,
                        Amount = totalAmount
                    });
                }
            }
            return jobList;
        }

        #endregion

        //old table generated by through mvc
        #region old_customer_Table

        private List<SelectListItem> StatusList = new List<SelectListItem> {
                new SelectListItem { Value = "ACT", Text = "Active" },
                new SelectListItem { Value = "INC", Text = "Inactive" },
                new SelectListItem { Value = "BAD", Text = "Bad Account" }
                };

        public List<CustomerDetails> getCustomerList(string status,string search)
        {

            var customerList = new List<Customer>();

            //filter customer list with status
            customerList = filterCustomerStatus(status);

            if(!String.IsNullOrEmpty(search) && !String.IsNullOrWhiteSpace(search)) { 
                //filter customer list with search string
                customerList = searchCustomer(customerList,search);
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
                    custcat =  db.CustCats.Where(c => c.CustomerId == customer.Id).FirstOrDefault();
                    custcategory =  db.CustCategories.Where(cat => cat.Id == custcat.CustCategoryId).FirstOrDefault();

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
                    companyEntity =  db.CustEntities.Where(ce => ce.CustomerId == customer.Id).FirstOrDefault();
                    company =  db.CustEntMains.Where(co => co.Id == companyEntity.CustEntMainId).FirstOrDefault();

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
                    categories =  getCategoriesList(customer.Id),
                    companies = getCustCompanies(customer.Id)

                    //end
                });

            }

            return customerDetailList;
        }

        private  List<Customer> filterCustomerStatus(string status)
        {

            var customerList = new List<Customer>();

            switch (status)
            {
                case "ACTIVE":
                    customerList =  db.Customers.Where(s => s.Status == "ACT").ToList();
                    break;
                case "INACTIVE":
                    customerList =  db.Customers.Where(s => s.Status == "INC").ToList();
                    break;
                case "BAD":
                    customerList =  db.Customers.Where(s => s.Status == "BAD").ToList();
                    break;
                case "ALL":
                    customerList =  db.Customers.ToList();
                    break;
                default:
                    customerList =  db.Customers.Where(s => s.Status == "ACT").ToList();
                    break;
            }


            return customerList;
        }

        private List<Customer> searchCustomer(List<Customer> customerList,string search)
        {
            customerList =  customerList.Where(c => c.Name.ToLower().Contains(search.ToLower())).ToList();

            return customerList;
        }
        
        #endregion

        //new table through ajax call
        #region AJAX_Customer_Table
        //-----AJAX Functions for generating table list---------//


        public List<CustomerList> generateCustomerList(string search, string status)
        {
            List<Customer> customers = new List<Customer>();
            List<CustomerList> custList = new List<CustomerList>();
            string sql = "select Id,Name, Contact1, Contact2 , Status,"
                        + " JobCount = (Select Count(x.Id) from[JobMains] x where x.CustomerId = c.Id ) ,"
                        + " Company = (Select Top(1)  CompanyName = (Select Top(1) cem.Name from[CustEntMains] cem where ce.CustEntMainId = cem.Id)"
                         + " from[CustEntities] ce where ce.CustomerId = c.Id) from Customers c";

            //handle status filter
            if (status != "ALL")
            {
                sql += " where c.Status = '" + status + "' ";
            }

            //handle status filter
            if (status == "ALL")
            {
                sql += " ";
            }

            //handle search by name filter
            if (search != null || search != "")
            {
                //handle status filter
                if (status != "ALL")
                {
                    sql += " and  c.Name Like '%" + search + "%' ";
                }
                else
                {
                    sql += "where  c.Name Like '%" + search + "%' ";
                }
            }

            //terminator
            sql += ";";

            custList = db.Database.SqlQuery<CustomerList>(sql).ToList();


            //customers = db.Customers.Include(c => c.CustEntities).ToList();

            ////Search string filter
            //if (status == "ALL")
            //{
            //    customers = customers.ToList();
            //}
            //else
            //{
            //    customers = customers.Where(s => s.Status == status).ToList();
            //}

            ////Search string filter
            //if (!string.IsNullOrWhiteSpace(search) || !string.IsNullOrEmpty(search))
            //{
            //    customers = customers.Where(s => s.Name.ToLower().Contains(search.ToLower())).ToList();
            //}

            ////build temp supplier list
            //foreach (var item in customers)
            //{
            //    //get latest company
            //    custList.Add(new CustomerList
            //    {
            //        Id = item.Id,
            //        Name = item.Name,
            //        Contact1 = String.IsNullOrEmpty(item.Contact1) ? "--" : item.Contact1,
            //        Contact2 = String.IsNullOrEmpty(item.Contact2) ? "--" : item.Contact2,
            //        Company = getCustCompanyName(item.Id),
            //        JobsCount = getjobCount(item.Id),
            //        Status = item.Status
            //    });
            //}
            return custList;
        }

        //Get the latest company name of the customer
        private string getCustCompanyName(int custID)
        {
            string companyName = "";
            try
            {
                var custEnt = db.CustEntities.Where(s => s.CustomerId == custID).OrderByDescending(s => s.Id).FirstOrDefault() != null ?
                   db.CustEntities.Where(s => s.CustomerId == custID).OrderByDescending(s => s.Id).FirstOrDefault() : null;

                companyName = custEnt.CustEntMain != null ? custEnt.CustEntMain.Name : "NA";
            }
            catch (Exception ex)
            { }

            return companyName;

        }


        //Get the number of jobs of the customer
        private string getjobCount(int custID)
        {
            return db.Customers.Find(custID).JobMains.Count().ToString();

        }
        #endregion
        
        //decatvate customers
        #region Deactivate Operations

        //Customers on the list are customers with
        //a year past jobs and no recent jobs 
        public List<Customer> getDeactivateCustomers()
        {
            List<int> latestJobs = new List<int>();

            //adjust date by subtracting 360 days (a year)
            var datetoday = dt.GetCurrentTime().Date.AddDays(-360);

            //get customers with jobs 
            var customersWithJobs = db.JobMains.Where(j => j.CustomerId > 0).Select(s => s.CustomerId);

            //get customers with null or ACT status and have jobs 
            var customers = db.Customers.Where(s => (string.IsNullOrEmpty(s.Status) || s.Status == "ACT") && customersWithJobs.Contains(s.Id)).ToList().Select(s => s.Id);

            //check group of customers and its recent job 
            //if the job is more than a year old
            foreach (var cust in customers)
            {
                JobMain tempjob = db.JobMains.Where(j => j.Customer.Id == cust).OrderByDescending(j => j.JobDate).FirstOrDefault();
                if (tempjob.JobDate.CompareTo(datetoday) < 0)
                {
                    latestJobs.Add(cust);
                }
            }

            //get customers with status NO STATUS or ACTIVE from prev list
            var CustomersList = db.Customers.Where(s => (string.IsNullOrEmpty(s.Status) || s.Status == "ACT") && latestJobs.Contains(s.Id)).ToList();

            //get customers  not in the list of customers with jobs
            List<Customer> noJobCustomer = db.Customers.Where(s => (string.IsNullOrEmpty(s.Status) || s.Status == "ACT") && s.Id != 1 && !customersWithJobs.Contains(s.Id)).ToList();

            //merge two list
            CustomersList.AddRange(noJobCustomer);

            return CustomersList;
        }

        #endregion
        
    }
}