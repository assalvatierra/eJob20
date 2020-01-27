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
    public class CompanyList
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Address { get; set; }
        public string Mobile    { get; set; }
        public string Website   { get; set; }
        public string Remarks   { get; set; }
        public string City      { get; set; }
        public string Category  { get; set; }

        //public string ContactName { get; set; }
        //public string ContactNumber { get; set; }
        //public string ContactEmail { get; set; }
        //public string ContactPosition { get; set; }

        public string AssignedTo { get; set; }
        public string Status { get; set; }
    }

    public class cCompanyList
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Address { get; set; }
        public string Mobile { get; set; }
        public string Website { get; set; }
        public string Remarks { get; set; }
        public string City { get; set; }
        public string Category { get; set; }
        public List<string> ContactName { get; set; }
        public List<string> ContactPosition { get; set; }
        public List<string> ContactMobileEmail { get; set; }
        public string AssignedTo { get; set; }
        public string Status { get; set; }
    }

    public class cCompanyContact
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string SocialMedia { get; set; }
        public string Telephone { get; set; }

    }
    public class CompanyClass
    {

        private JobDBContainer db = new JobDBContainer();
        private DateClass dt = new DateClass();
        
        //new table through ajax call
        #region AJAX_Customer_Table

        //-----AJAX Functions for generating table list---------//
        public List<cCompanyList> generateCompanyList(string search, string searchCat, string status, string sort)
        {
            List<CompanyList> custList = new List<CompanyList>();
             
            string sql = "SELECT * FROM (SELECT cem.*, Category = (SELECT TOP 1 Name = (SELECT Name FROM CustCategories c WHERE c.Id = b.CustCategoryId ) FROM CustEntCats b WHERE cem.Id = b.CustEntMainId ), "+
                 "City = (SELECT TOP 1  Name FROM Cities city WHERE city.Id = CityId), "+
                 
				 "cust.Name as ContactName, cust.Email as ContactEmail, cust.Contact1 as ContactNumber, "+
                 "cet.Position as ContactPosition " +

                 "FROM CustEntMains cem "+

                 "LEFT JOIN CustEntities cet ON cet.CustEntMainId = cem.Id "+

                 "LEFT JOIN Customers cust ON cust.Id = cet.CustomerId "+

                 ") as com";


            if (status != null)
            {

                if (status != "ALL")
                {
                    sql += " WHERE com.Status = '" + status + "' ";
                }
                else
                {
                    sql += " WHERE com.Name != ''  ";
                }

            }
            else
            {
                sql += " WHERE com.Status != 'INC' OR com.Status != 'BAD' ";
            }


            //handle search by name filter
            if (search != null || search != "")
            {
               

                //search using the search by category
                switch (searchCat)
                    {
                        case "Company":
                            sql += " AND com.Name Like '%" + search + "%' ";
                            break;
                        case "City":
                            sql += " AND com.City Like '%" + search + "%' ";
                            break;
                        case "ContactName":
                            sql += " AND com.ContactName Like '%" + search + "%' ";
                            break;
                        case "Category":
                            sql += " AND com.Category Like '%" + search + "%' ";
                            break;
                        case "AssignedTo":
                            sql += " AND com.AssignedTo Like '%" + search + "%' ";
                            break;
                        default:
                            sql += " ";
                            break;
                    }
            }


            if (sort != null)
            {
                switch (sort)
                {
                    //add more options for sorting
                    default:
                        sql += " ORDER BY com.Name ASC;";
                        break;
                } 
            }
            else
            {
                //terminator
                sql += " ORDER BY com.Name ASC;";

            }

            custList = db.Database.SqlQuery<CompanyList>(sql).ToList();
            

            return getCompanyList(custList);
        }
        

        private List<cCompanyList> getCompanyList(List<CompanyList> list)
        {
            List<cCompanyList> comlist = new List<cCompanyList>();
           
            foreach (var com in list)
            {
               
                //build contact list
                var contacts = db.CustEntities.Where(s => s.CustEntMainId == com.Id).ToList();
                var custEnts = db.CustEntities.Where(s => s.CustEntMainId == com.Id).ToList();
                List<string> contactNames = custEnts.Select(s => s.Customer.Name).ToList();
                List<string> contactPositions = custEnts.Select(s => s.Position).ToList();
                List<string> contactNumberEmail = new List<string>();
                foreach (var items in custEnts )
                {
                    var temp = "";
                    if (items.Customer.Contact2 != null)
                    {
                        temp = items.Customer.Contact1 + " | " + items.Customer.Contact2;
                    }
                    else
                    {
                        temp = items.Customer.Contact1;
                    }
                    contactNumberEmail.Add( temp + " <br> " + items.Customer.Email);
                }

               


                comlist.Add(new cCompanyList {
                    Id = com.Id,
                    Address = com.Address,
                    AssignedTo = removeSpecialChar(com.AssignedTo) ,
                    Category = com.Category,
                    City = com.City,
                    Code = com.Code,
                    Mobile = com.Mobile,
                    Name = com.Name,
                    Remarks = com.Remarks,
                    Status = com.Status,
                    Website = com.Website,
                    ContactName = contactNames,
                    ContactPosition = contactPositions,
                    ContactMobileEmail = contactNumberEmail

                });
            }

            return comlist;
        }

        public string removeSpecialChar(string input)
        {
            try
            {

            char ch = '@';
            int idx = input.IndexOf(ch);
            return input.Substring(0,idx);

            }
            catch (Exception ex)
            {
                return input;
            }

        }
        
        #endregion
        
        
    }
}