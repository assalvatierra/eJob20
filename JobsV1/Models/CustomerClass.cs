using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace JobsV1.Models
{
    public class CustomerClass
    {

        private JobDBContainer db = new JobDBContainer();
        private List<SelectListItem> StatusList = new List<SelectListItem> {
                new SelectListItem { Value = "ACT", Text = "Active" },
                new SelectListItem { Value = "INC", Text = "Inactive" },
                new SelectListItem { Value = "BAD", Text = "Bad Account" }
                };

        public async Task<List<CustomerDetails>> getCustomerList(string status,string search)
        {

            var customerList = new List<Customer>();

            //filter customer list with status
            customerList = await filterCustomerStatus(status);

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
                    custcat = await db.CustCats.Where(c => c.CustomerId == customer.Id).FirstOrDefaultAsync();
                    custcategory = await db.CustCategories.Where(cat => cat.Id == custcat.CustCategoryId).FirstOrDefaultAsync();

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
                    companyEntity = await db.CustEntities.Where(ce => ce.CustomerId == customer.Id).FirstOrDefaultAsync();
                    company = await db.CustEntMains.Where(co => co.Id == companyEntity.CustEntMainId).FirstOrDefaultAsync();

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
                    categories = await getCategoriesList(customer.Id),
                    companies = await getCompanyList(customer.Id)

                    //end
                });

            }

            return customerDetailList;
        }

        private async Task<List<Customer>> filterCustomerStatus(string status)
        {

            var customerList = new List<Customer>();

            switch (status)
            {
                case "ACTIVE":
                    customerList = await db.Customers.Where(s => s.Status == "ACT").ToListAsync();
                    break;
                case "INACTIVE":
                    customerList = await db.Customers.Where(s => s.Status == "INC").ToListAsync();
                    break;
                case "BAD":
                    customerList = await db.Customers.Where(s => s.Status == "BAD").ToListAsync();
                    break;
                case "ALL":
                    customerList = await db.Customers.ToListAsync();
                    break;
                default:
                    customerList = await db.Customers.Where(s => s.Status == "ACT").ToListAsync();
                    break;
            }


            return customerList;
        }

        private List<Customer> searchCustomer(List<Customer> customerList,string search)
        {
            customerList =  customerList.Where(c => c.Name.ToLower().Contains(search.ToLower())).ToList();

            return customerList;
        }


        private async Task<List<CustCategory>> getCategoriesList(int id)
        {

            //PartialView for Details of the Customer
            List<CustCategory> categoryDetails = new List<CustCategory>();

            //error
            var categoryList = await db.CustCats.Where(c => c.CustomerId == id).ToListAsync();

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


        private async Task<List<CustEntMain>> getCompanyList(int id)
        {

            //PartialView for Details of the Customer
            List<CustEntMain> CompanyList = new List<CustEntMain>();
            //error
            var CompanyRecord = await db.CustEntities.Where(c => c.CustomerId == id).ToListAsync();

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

            return CompanyList;
        }

    }
}